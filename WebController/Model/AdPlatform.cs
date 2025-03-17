public class AdPlatform
{
    public string Name { get; init; }
    public List<string> Locations { get; init; }

    public AdPlatform(string name, List<string> locations)
    {
        Name = name;
        Locations = locations;
    }
}