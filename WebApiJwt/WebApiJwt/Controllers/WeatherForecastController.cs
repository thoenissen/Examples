using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiJwt.DTO;

namespace WebApiJwt.Controllers;

/// <summary>
/// Weather API
/// </summary>
[ApiController]
[Authorize(Roles = "ExampleRole")]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    #region Fields

    /// <summary>
    /// Summaries
    /// </summary>
    private static readonly string[] _summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    #endregion // Fields

    #region Methods

    [HttpGet]
    public IEnumerable<WeatherForecastDTO> Get()
    {
        return Enumerable.Range(1, 5)
                         .Select(index => new WeatherForecastDTO
                                          {
                                              Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                              Temperature = Random.Shared.Next(-20, 55),
                                              Summary = _summaries[Random.Shared.Next(_summaries.Length)]
                                          })
                         .ToArray();
    }

    #endregion // Methods
}