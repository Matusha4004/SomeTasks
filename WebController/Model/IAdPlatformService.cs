namespace WebController.Model;

public interface IAdPlatformService
{
    void UploadPlatforms(List<AdPlatform> platforms);
    List<string> Search(string location);
}