using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UploadData.API.Services;

namespace UploadData.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;
        private readonly long _maxFileSizeBytes;

        public UploadController(IUploadService uploadService, IConfiguration cfg)
        {
            _uploadService = uploadService;
            _maxFileSizeBytes = cfg.GetSection("Upload").GetValue<long>("MaxFileSizeBytes", 200_000_000);
        }

        [HttpPost("excel/stg")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadExcelToStaging([FromForm] UploadExcelDto dto, CancellationToken ct)
        {
            if (dto.File is null || dto.File.Length == 0)
                return BadRequest("Sube un archivo Excel válido.");

            if (dto.File.Length > _maxFileSizeBytes)
                return BadRequest($"El archivo excede el tamaño máximo permitido ({_maxFileSizeBytes:N0} bytes).");

            string tempPath = string.Empty;
            try
            {
                tempPath = await SaveTempAsync(dto.File, ct);
                var result = await _uploadService.ProcessExcelToStagingAsync(tempPath, dto.Sheet, dto.Semestre, ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error procesando el archivo.",
                    detail = ex.Message
                });
            }
            finally
            {
                if (!string.IsNullOrEmpty(tempPath) && System.IO.File.Exists(tempPath))
                {
                    try { System.IO.File.Delete(tempPath); } catch { }
                }
            }
        }

        [HttpPost("excel/final")]
        [Consumes("multipart/form-data")]
        public IActionResult UploadExcelToFinal([FromForm] UploadExcelDto dto)
            => StatusCode(StatusCodes.Status501NotImplemented,
                new { message = "Pendiente: flujo a tabla FINAL aún no implementado." });

        private static async Task<string> SaveTempAsync(IFormFile file, CancellationToken ct)
        {
            var uploadsDir = Path.Combine(AppContext.BaseDirectory, "uploads");
            Directory.CreateDirectory(uploadsDir);
            var tempPath = Path.Combine(uploadsDir, $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}");
            await using var fs = System.IO.File.Create(tempPath);
            await file.CopyToAsync(fs, ct);
            return tempPath;
        }
    }

    public class UploadExcelDto
    {
        [Required] public IFormFile File { get; set; } = default!;
        public string? Sheet { get; set; }
        public int? Semestre { get; set; }
    }
}
