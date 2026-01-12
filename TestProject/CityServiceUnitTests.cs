using Moq;
using CitySearchingAssignment.Services;
using CitySearchingAssignment.Repositories;
using CitySearchingAssignment.Models;

namespace CitySearchingAssignment.Tests;

public class CityServiceUnitTests
{
    private readonly Mock<ICityRepository> _mockRepo;
    private readonly CityService _service;

    public CityServiceUnitTests()
    {
        _mockRepo = new Mock<ICityRepository>();
        _service = new CityService(_mockRepo.Object);
    }

    [Fact]
    public void Search_ShouldPrioritizeExactMatch()
    {
        var data = new List<CityInternalData> {
            new() { Name = "Bangkok Extra", Country = "TH" },
            new() { Name = "Bangkok", Country = "TH" }
        };
        _mockRepo.Setup(r => r.GetCities()).Returns(data);

        var result = _service.Search("Bangkok").ToList();
        Assert.Equal("Bangkok", result[0].Name);
    }

    [Fact]
    public void Search_WithCountryFilter_ShouldWork()
    {
        var data = new List<CityInternalData> {
            new() { Name = "Paris", Country = "FR" },
            new() { Name = "Paris", Country = "US" }
        };
        _mockRepo.Setup(r => r.GetCities()).Returns(data);

        var result = _service.Search("Paris, US").ToList();
        Assert.Single(result);
        Assert.Equal("US", result[0].Country);
    }

    [Fact]
    public void Search_DirtyData_ShouldNotBeFound()
    {
        var data = new List<CityInternalData> {
            new() { Name = "HiddenCity", Country = "XX" }
        };
        _mockRepo.Setup(r => r.GetCities()).Returns(data);

        var result = _service.Search("http").ToList();
        Assert.Empty(result);
    }
}