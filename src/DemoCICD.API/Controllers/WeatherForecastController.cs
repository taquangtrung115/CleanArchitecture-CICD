using Microsoft.AspNetCore.Mvc;

namespace DemoCICD.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<string> Get()
    {
        return Enumerable.Range(1, 5).Select(index =>
            $"{DateTime.Now.AddDays(index).ToShortDateString()} - {Summaries[Random.Shared.Next(Summaries.Length)]}")
            .ToArray();
    }
}
