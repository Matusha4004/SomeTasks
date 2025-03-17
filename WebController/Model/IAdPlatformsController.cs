using Microsoft.AspNetCore.Mvc;

namespace WebController.Model;

public interface IAdPlatformsController
{
    public IActionResult Upload([FromForm] IFormFile file);

    public IActionResult Search([FromQuery] string location);
}