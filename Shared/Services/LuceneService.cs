using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Shared.Models;

namespace Shared.Services;

public class LuceneService
{
    private static readonly LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
    private readonly string _indexPath;
    private FSDirectory _directory;
    private StandardAnalyzer _analyzer;

    public LuceneService(string indexPath)
    {
        _indexPath = indexPath;
        _directory = FSDirectory.Open(new DirectoryInfo(_indexPath));
        _analyzer = new StandardAnalyzer(AppLuceneVersion);
    }

    public void CreateIndex(IEnumerable<Product> products)
    {
        var config = new IndexWriterConfig(AppLuceneVersion, _analyzer);
        using (var writer = new IndexWriter(_directory, config))
        {
            foreach (var product in products)
            {
                var doc = new Document
            {
                new StringField("Id", product.Id.ToString(), Field.Store.YES),
                new TextField("Name", product.Name, Field.Store.YES),
                new TextField("Description", product.Description, Field.Store.YES),
                new StringField("CategoryId", product.CategoryId.ToString(), Field.Store.YES),
                new DoubleField("Price", (double)product.Price, Field.Store.YES),
                new TextField("ImageBase64", product.ImageBase64 ?? string.Empty, Field.Store.YES)
            };

                if (product.Specifications != null && product.Specifications.Any())
                {
                    foreach (var spec in product.Specifications)
                    {
                        doc.Add(new TextField($"Spec_{spec.Key}", spec.Value, Field.Store.YES));
                    }
                }

                writer.AddDocument(doc);
            }
            writer.Commit();
        }
    }

    public IEnumerable<Product> Search(Dictionary<string, string> specifications)
    {
        var results = new List<Product>();
        using (var reader = DirectoryReader.Open(_directory))
        {
            var searcher = new IndexSearcher(reader);
            var queryBuilder = new BooleanQuery();

            foreach (var spec in specifications)
            {
                var specQuery = new TermQuery(new Term($"Spec_{spec.Key}", spec.Value));
                queryBuilder.Add(specQuery, Occur.MUST);
            }

            var hits = searcher.Search(queryBuilder, 10).ScoreDocs;
            foreach (var hit in hits)
            {
                var doc = searcher.Doc(hit.Doc);
                var product = new Product
                {
                    Id = int.Parse(doc.Get("Id")),
                    Name = doc.Get("Name"),
                    Description = doc.Get("Description"),
                    CategoryId = int.Parse(doc.Get("CategoryId")),
                    Price = (decimal)double.Parse(doc.Get("Price")),
                    ImageBase64 = doc.Get("ImageBase64")
                };

                results.Add(product);
            }
        }
        return results;
    }
}
