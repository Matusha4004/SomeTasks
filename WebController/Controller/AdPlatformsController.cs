using Microsoft.AspNetCore.Mvc;
using WebController.Model;
using System.Linq;

namespace WebController.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdPlatformsController : ControllerBase, IAdPlatformsController
    {
        // In-memory storage for advertising platforms
        private readonly IDictionary<string, AdPlatform> _adPlatforms = new Dictionary<string, AdPlatform>();
        
        private readonly IDictionary<string, List<string>> _cache = new Dictionary<string, List<string>>();
        
        private readonly ILogger<AdPlatformsController> _logger;
        
        public AdPlatformsController(ILogger<AdPlatformsController> logger)
        {
            _logger = logger;
        }

        // POST: api/AdPlatforms/upload
        [HttpPost("upload")]
        public IActionResult Upload([FromForm] IFormFile file)
        {
            _logger.LogInformation("Внутренние данные удалены");
            _adPlatforms.Clear();
            _cache.Clear();
            _logger.LogInformation("Запрос на загрузку файла получен.");
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Файл не был предоставлен.");
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    _adPlatforms.Clear(); // Clear existing data
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(':', 2);
                        if (parts.Length == 2)
                        {
                            var platform = new AdPlatform(parts[0].Trim(),
                                parts[1].Split(',').Select(l => l.Trim()).ToList());
                            _adPlatforms[platform.Name] = platform;
                        }
                    }
                }
                _logger.LogInformation("Файл успешно обработан.");
                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке файла.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/AdPlatforms/search?location=/ru/svrd/revda
        [HttpGet("search")]
        public IActionResult CacheSearch([FromQuery] string location)
        {
            _logger.LogInformation("Запрос на поиск по локации: {Location}", location);
            if (_cache.TryGetValue(location, out var result))
            {
                if (result == null)
                {
                    return StatusCode(404, $"Location {location} not found.");
                }
                _logger.LogInformation("Поиск завершен хэшом для локации: {Location}", location);
                return Ok(result);
            }
            var results = Search(location);
            if (results == null)
            {
                return StatusCode(404, $"Location {location} not found.");
            }
            _logger.LogInformation("Поиск завершен для локации: {Location} и добавлен в хэш", location);
            return Ok(_cache[location] = results);
        }
        
        private List<string>? Search(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return null;
            }
            var q = _adPlatforms;
            var matchingPlatforms = q.Values
                .Where(platform =>
                    platform.Locations
                        .Any(loc => location.StartsWith(loc, StringComparison.OrdinalIgnoreCase) ||
                                    loc.StartsWith(location, StringComparison.OrdinalIgnoreCase)))
                .Select(platform => platform.Name).Distinct().ToList();
            return matchingPlatforms;
        }
    }
}