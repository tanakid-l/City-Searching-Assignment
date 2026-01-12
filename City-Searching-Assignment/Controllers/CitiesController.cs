using Microsoft.AspNetCore.Mvc;
using CitySearchingAssignment.Services;

namespace CitySearchingAssignment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ICityService _service;
    public CitiesController(ICityService service) => _service = service;

    [HttpGet]
    public IActionResult Get([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            return BadRequest(new { message = "Query must be at least 2 characters." });

        return Ok(_service.Search(name));
    }
}