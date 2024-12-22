using System;

namespace WebApiJwt.DTO;

/// <summary>
/// Weather forecast
/// </summary>
public class WeatherForecastDTO
{
    /// <summary>
    /// Date
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Temperature in °C
    /// </summary>
    public int Temperature { get; set; }

    /// <summary>
    /// Summary
    /// </summary>
    public string Summary { get; set; }
}