namespace CitySearchingAssignment.Models;

public class CityJsonData
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public CityStat? Stat { get; set; }
    public List<Dictionary<string, string>>? Langs { get; set; }
}

public class CityStat
{
    public double? Population { get; set; }
}

public class CityDto
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public long Population { get; set; }
}

public class CityInternalData : CityDto
{
    public List<string> AlternativeNames { get; set; } = new();
}