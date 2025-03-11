using Microsoft.AspNetCore.Mvc;
using AdPlatformsService.Controllers;
using AdPlatformsService.Models;
using System.Collections.Concurrent;

namespace AdPlatformsService.Tests
{
    public class AdPlatformsControllerTests
    {
        [Fact]
        public void Search_ReturnsMatchingPlatforms()
        {
            // Arrange
            var controller = new AdPlatformsController();
            controller._adPlatforms = new ConcurrentDictionary<string, AdPlatform>();
            controller._adPlatforms["Яндекс.Директ"] = new AdPlatform
            {
                Name = "Яндекс.Директ",
                Locations = new List<string> { "/ru" }
            };
            controller._adPlatforms["Ревдинский рабочий"] = new AdPlatform
            {
                Name = "Ревдинский рабочий",
                Locations = new List<string> { "/ru/svrd/revda", "/ru/svrd/pervik" }
            };

            // Act
            var result = controller.Search("/ru/svrd/revda") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var platforms = result.Value as List<string>;
            Assert.Contains("Яндекс.Директ", platforms);
            Assert.Contains("Ревдинский рабочий", platforms);
        }
    }
}