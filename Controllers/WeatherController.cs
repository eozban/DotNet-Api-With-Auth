using api_with_auth.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace api_with_auth.Controllers
{
    [ApiController]
    [Route("api/")]
    public class WeatherController : ControllerBase
    {
        [HttpGet("weather")]
        //[ServiceFilter(typeof(ApiKeyAuthFilter))]
        [ApiKeyAuth]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Globals.Summaries[rng.Next(Globals.Summaries.Length)]
            }).ToArray();
        }

        [HttpGet("weather2")]
        public IEnumerable<WeatherForecast> Get2()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Globals.Summaries[rng.Next(Globals.Summaries.Length)]
            })
            .ToArray();
        }
    }
}