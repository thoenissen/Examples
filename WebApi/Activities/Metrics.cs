using System.Diagnostics.Metrics;

namespace WebApi.Activities;

internal static class Metrics
{
    private static readonly Meter _meter = new("OpenTelemetryDemo.Metrics");
    public static Counter<long> WeatherForecastCalls { get; } = _meter.CreateCounter<long>(nameof(WeatherForecastCalls));
    public static Histogram<int> WeatherForecastTemperature { get; } = _meter.CreateHistogram<int>(nameof(WeatherForecastTemperature), "°C", advice: new InstrumentAdvice<int> { HistogramBucketBoundaries = [-22, 5, 25, 55] });
}