using Microsoft.AspNetCore.Mvc;
using AdPlatformsService.Models;
using System.Collections.Concurrent;

namespace AdPlatformsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdPlatformsController : ControllerBase
    {
        // In-memory storage for advertising platforms
        public ConcurrentDictionary<string, AdPlatform> _adPlatforms = new();

        // POST: api/AdPlatforms/upload
        [HttpPost("upload")]
        public IActionResult Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
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
                            var platform = new AdPlatform
                            {
                                Name = parts[0].Trim(),
                                Locations = parts[1].Split(',').Select(l => l.Trim()).ToList()
                            };
                            _adPlatforms[platform.Name] = platform;
                        }
                    }
                }
                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/AdPlatforms/search?location=/ru/svrd/revda
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return BadRequest("Location parameter is required.");
            }

            var matchingPlatforms = _adPlatforms.Values
                .Where(platform => platform.Locations.Any(loc => location.StartsWith(loc) || loc.StartsWith(location)))
                .Select(platform => platform.Name)
                .Distinct()
                .ToList();

            return Ok(matchingPlatforms);
        }
    }
}