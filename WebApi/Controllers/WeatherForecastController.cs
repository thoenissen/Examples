using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using WebApi.Activities;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            Metrics.WeatherForecastCalls.Add(1);

            _logger.LogInformation("Weather data requested");

            if (TryGetCache(out var forecasts))
            {
                return forecasts;
            }

            return GetWeatherForecasts();
        }

        private bool TryGetCache(out IEnumerable<WeatherForecast> data)
        {
            data = null;

            using (var activity = Tracing.General.StartActivity())
            {
                activity?.SetTag("cache.key", "general");

                if (Random.Shared.NextDouble() > 0.75)
                {
                    if (Random.Shared.NextDouble() > 0.75)
                    {
                        throw new InvalidOperationException("Simulating a critical error.");
                    }

                    Thread.Sleep(1000);

                    activity?.SetStatus(ActivityStatusCode.Error);

                    _logger.LogWarning("Cache did not respond");
                }

                _logger.LogInformation("Missing cache entry");
            }

            return false;
        }

        private IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            using (Tracing.General.StartActivity())
            {
                return Enumerable.Range(1, 5)
                                 .Select(index =>
                                         {
                                             var temperature = Random.Shared.Next(-20, 55);
                                         
                                             Metrics.WeatherForecastTemperature.Record(temperature);
                                         
                                             return new WeatherForecast
                                                    {
                                                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                        TemperatureC = temperature,
                                                        Summary = GetSummary(DateOnly.FromDateTime(DateTime.Now.AddDays(index)))
                                                    };
                                         })
                                 .ToArray();
            }
        }

        private string? GetSummary(DateOnly date)
        {
            using (var activity = Tracing.General.StartActivity())
            {
                activity?.SetTag("date", date);

                var summary = Summaries[Random.Shared.Next(Summaries.Length)];

                _logger.LogInformation("{Summary} written for {date}", summary, date);
                
                return summary;
            }
        }
    }
}
