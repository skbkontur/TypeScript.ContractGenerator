using System.Text.Json;

using AspNetCoreExample.Api.Models;

using Microsoft.AspNetCore.Mvc;

using SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController;

namespace AspNetCoreExample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        this.logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5)
                         .Select(index => new WeatherForecast
                             {
                                 Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                 TemperatureC = Random.Shared.Next(-20, 55),
                                 Summary = summaries[Random.Shared.Next(summaries.Length)]
                             })
                         .ToArray();
    }

    [HttpPost("Update/{city}")]
    public void Update(string city, [FromBody] WeatherForecast forecast, CancellationToken cancellationToken)
    {
    }

    [HttpPost("~/[action]")]
    public void Reset(int seed)
    {
    }

    [UrlOnly]
    [HttpGet("{city}")]
    public ActionResult Download(string city)
    {
        var forecast = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            };
        return File(JsonSerializer.SerializeToUtf8Bytes(forecast), "application/json");
    }

    private static readonly string[] summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> logger;
}