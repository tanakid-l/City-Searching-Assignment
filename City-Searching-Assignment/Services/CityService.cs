using CitySearchingAssignment.Models;
using CitySearchingAssignment.Repositories;

namespace CitySearchingAssignment.Services;

public interface ICityService
{
    IEnumerable<CityDto> Search(string query);
}

public class CityService : ICityService
{
    private readonly ICityRepository _repo;
    public CityService(ICityRepository repo) => _repo = repo;

    public IEnumerable<CityDto> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return Enumerable.Empty<CityDto>();

        var parts = query.Split(',').Select(p => p.Trim()).ToList();
        var term = parts[0];
        var countryFilter = parts.Count > 1 ? parts[1] : null;

        return _repo.GetCities()
            .Where(c => (c.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                         c.AlternativeNames.Any(a => a.Contains(term, StringComparison.OrdinalIgnoreCase))) &&
                        (countryFilter == null || c.Country.Equals(countryFilter, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(c => c.Name.Equals(term, StringComparison.OrdinalIgnoreCase))
            .ThenByDescending(c => c.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase))
            .Take(10)
            .Select(c => new CityDto
            {
                Name = c.Name,
                Country = c.Country,
                Population = c.Population
            });
    }
}