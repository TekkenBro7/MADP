using Microsoft.AspNetCore.Mvc;

namespace Web_253505_Sniazhko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _imagePath;
        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }
        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_imagePath, fileName);
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            using (var fileStream = fileInfo.Create())
            {
                await file.CopyToAsync(fileStream);
            }
            // Получение URL файла
            var host = HttpContext.Request.Host;
            var fileUrl = $"https://{host}/Images/{fileName}";

            return Ok(fileUrl);
        }
        [HttpDelete("{file}")]
        public IActionResult DeleteFile(string file)
        {
            var filePath = Path.Combine(_imagePath, file);
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                return Ok();
            }
            return NotFound("File not found.");
        }
    }
}