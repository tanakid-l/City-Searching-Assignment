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
            .Where(IsMatch)
            .OrderByDescending(IsExactMatch)
            .ThenByDescending(IsPrefixMatch)
            .Take(10)
            .Select(ToDto);

        // --- Local Functions (Logic Detail) ---

        bool IsMatch(CityInternalData city)
        {
            bool nameMatches = city.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                               city.AlternativeNames.Any(a => a.Contains(term, StringComparison.OrdinalIgnoreCase));

            bool countryMatches = countryFilter == null ||
                                  city.Country.Equals(countryFilter, StringComparison.OrdinalIgnoreCase);

            return nameMatches && countryMatches;
        }

        bool IsExactMatch(CityInternalData city) =>
            city.Name.Equals(term, StringComparison.OrdinalIgnoreCase);

        bool IsPrefixMatch(CityInternalData city) =>
            city.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase);

        CityDto ToDto(CityInternalData city) => new CityDto
        {
            Name = city.Name,
            Country = city.Country,
            Population = city.Population
        };
    }
}