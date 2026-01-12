using CitySearchingAssignment.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace CitySearchingAssignment.Tests;

public class CityApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CityApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Search_ValidQuery_ReturnsOkAndData()
    {
        var response = await _client.GetAsync("/api/cities?name=Bangkok");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<List<CityDto>>();
        Assert.NotNull(result);
        Assert.Contains(result, c => c.Name == "Bangkok");
    }

    [Fact]
    public async Task Search_ShortQuery_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/cities?name=A");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Search_WikipediaKey_ShouldNotReturnGarbage()
    {
        var response = await _client.GetAsync("/api/cities?name=wikipedia");
        var result = await response.Content.ReadFromJsonAsync<List<CityDto>>();

        Assert.Empty(result ?? new List<CityDto>());
    }
}