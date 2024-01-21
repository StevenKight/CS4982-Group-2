using API.Dal;
using API.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    #region Data members

    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> logger;

    private readonly DataContext context;

    #endregion

    #region Constructors

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    #endregion

    #region Methods

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();

        return Ok(this.context.WeatherForecasts);
    }

    #endregion
}