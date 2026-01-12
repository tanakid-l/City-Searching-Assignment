using System.Text.Json;
using CitySearchingAssignment.Models;

namespace CitySearchingAssignment.Repositories;

public interface ICityRepository
{
    IEnumerable<CityInternalData> GetCities();
}

public class JsonCityRepository : ICityRepository
{
    private readonly List<CityInternalData> _cities = new();

    public JsonCityRepository(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", "current.city.list.json");

        if (File.Exists(path))
        {
            using var stream = File.OpenRead(path);
            var raw = JsonSerializer.Deserialize<List<CityJsonData>>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (raw != null)
            {
                _cities = raw.Select(c => new CityInternalData
                {
                    Name = c.Name,
                    Country = c.Country,
                    Population = (long)Math.Round((double)(c.Stat?.Population ?? 0)),
                    AlternativeNames = c.Langs?.SelectMany(d => d.Values).ToList() ?? new()
                }).ToList();
            }
        }
    }

    public IEnumerable<CityInternalData> GetCities() => _cities;
}