using System.Diagnostics;

namespace WebApi.Activities;

internal static class Tracing
{
    public static ActivitySource General { get; } = new("OpenTelemetryDemo.Tracing");
}