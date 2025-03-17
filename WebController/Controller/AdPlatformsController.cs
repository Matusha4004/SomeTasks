using Microsoft.AspNetCore.Mvc;
using WebController.Model;

[ApiController]
[Route("api/[controller]")]
public class AdPlatformsController : ControllerBase, IAdPlatformsController
{
    private readonly IAdPlatformService _service;
    private readonly ILogger<AdPlatformsController> _logger;

    public AdPlatformsController(IAdPlatformService service, ILogger<AdPlatformsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("upload")]
    public IActionResult Upload([FromForm] IFormFile? file)
    {
        _logger.LogInformation("Запрос на загрузку файла.");
        if (file is null || file.Length == 0)
        {
            _logger.LogWarning("Файл не был предоставлен.");
            return BadRequest("No file uploaded.");
        }

        try
        {
            var platforms = new List<AdPlatform>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        platforms.Add(new AdPlatform(
                            parts[0].Trim(),
                            parts[1].Split(',').Select(l => l.Trim()).ToList()
                        ));
                    }
                }
            }

            _service.UploadPlatforms(platforms);
            return Ok("File uploaded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке файла.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string location)
    {
        _logger.LogInformation("Поиск по локации: {Location}", location);
        var results = _service.Search(location);
        return Ok(results);
    }
}