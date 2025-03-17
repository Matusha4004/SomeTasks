using System.Collections.Concurrent;
using Gma.DataStructures.StringSearch;
using WebController.Model;

public class AdPlatformService : IAdPlatformService
{
    private ConcurrentDictionary<string, List<string>> _cashedAd = new();
    private List<AdPlatform> _activeData = new();
    private List<AdPlatform> _bufferData = new();
    private readonly ILogger<AdPlatformService> _logger;

    public AdPlatformService(ILogger<AdPlatformService> logger)
    {
        _logger = logger;
    }

    public void UploadPlatforms(List<AdPlatform> platforms)
    {
        _logger.LogInformation("Начало загрузки данных.");
        _bufferData.Clear();

        _bufferData = platforms;
        
        foreach (var platform in platforms)
        {
            
            
        }
        
        _logger.LogInformation("Данные успешно загружены.");
    }

    public List<string> Search(string location)
    {
        var result = _activeData.
    }
}