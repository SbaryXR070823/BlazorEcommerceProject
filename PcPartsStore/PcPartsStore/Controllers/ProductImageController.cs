using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace PcPartsStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            const int maxFileSizeBytes = 16 * 1024 * 1024;
            if (file.Length > maxFileSizeBytes)
                return BadRequest("File size exceeds 5MB limit.");

            try
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var base64 = Convert.ToBase64String(ms.ToArray());
                return Ok(base64);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing image: {ex.Message}");
            }
        }
    }
}