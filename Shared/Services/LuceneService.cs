﻿using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shared.Services
{
    public class LuceneService
    {
        private static readonly LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
        private readonly string _indexPath;
        private FSDirectory _directory;
        private StandardAnalyzer _analyzer;
        private readonly object _indexLock = new object();

        public LuceneService(string indexPath)
        {
            _indexPath = indexPath;

            if (!System.IO.Directory.Exists(_indexPath))
            {
                System.IO.Directory.CreateDirectory(_indexPath);
            }

            _directory = FSDirectory.Open(new DirectoryInfo(_indexPath));
            _analyzer = new StandardAnalyzer(AppLuceneVersion);
        }

        public void CreateIndex(IEnumerable<Product> products)
        {
            lock (_indexLock)
            {
                var config = new IndexWriterConfig(AppLuceneVersion, _analyzer)
                {
                    OpenMode = OpenMode.CREATE 
                };

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
                                string normalizedKey = spec.Key.ToLowerInvariant();
                                string normalizedValue = spec.Value.ToLowerInvariant();

                                doc.Add(new TextField($"Spec_{normalizedKey}_text", normalizedValue, Field.Store.YES));
                                doc.Add(new StringField($"Spec_{normalizedKey}_exact", normalizedValue, Field.Store.YES));

                                doc.Add(new TextField("AllSpecs", $"{normalizedKey}:{normalizedValue}", Field.Store.NO));
                            }
                        }

                        writer.AddDocument(doc);
                    }

                    writer.Commit();
                }
            }
        }

        public IEnumerable<Product> Search(Dictionary<string, string> specifications)
        {
            var results = new List<Product>();

            if (!DirectoryReader.IndexExists(_directory))
            {
                Console.WriteLine("Index does not exist. Creating empty index.");
                using (var writer = new IndexWriter(_directory, new IndexWriterConfig(AppLuceneVersion, _analyzer)))
                {
                    writer.Commit();
                }
                return results;
            }

            using (var reader = DirectoryReader.Open(_directory))
            {
                var searcher = new IndexSearcher(reader);
                var queryBuilder = new BooleanQuery();

                foreach (var spec in specifications)
                {
                    string normalizedKey = spec.Key.ToLowerInvariant();
                    string normalizedValue = spec.Value.ToLowerInvariant();

                    var specQuery = new BooleanQuery();

                    var exactQuery = new TermQuery(new Term($"Spec_{normalizedKey}_exact", normalizedValue));
                    specQuery.Add(exactQuery, Occur.SHOULD);

                    try
                    {
                        var parser = new QueryParser(AppLuceneVersion, $"Spec_{normalizedKey}_text", _analyzer);
                        var textQuery = parser.Parse(QueryParserBase.Escape(normalizedValue));
                        specQuery.Add(textQuery, Occur.SHOULD);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing query for spec {spec.Key}: {ex.Message}");
                    }

                    queryBuilder.Add(specQuery, Occur.MUST);
                }

                Console.WriteLine($"Executing Lucene query: {queryBuilder}");

                var hits = searcher.Search(queryBuilder, reader.MaxDoc).ScoreDocs;
                Console.WriteLine($"Found {hits.Length} results");

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
}