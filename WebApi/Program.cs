using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var resourceBuilder = ResourceBuilder.CreateDefault()
                                                 .AddService("OpenTelemetryDemo", null, "1.0.0");

            builder.Services.AddOpenTelemetry()
                            .WithTracing(tracerProviderBuilder =>
                                         {
                                             tracerProviderBuilder.SetResourceBuilder(resourceBuilder)
                                                                  .AddAspNetCoreInstrumentation()
                                                                  .AddHttpClientInstrumentation()
                                                                  .AddSource("OpenTelemetryDemo.Tracing")
                                                                  .AddConsoleExporter()
                                                                  .AddOtlpExporter(otlpOptions =>
                                                                                   {
                                                                                       otlpOptions.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_TRACES_ENDPOINT")!);
                                                                                       otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                                                                                   });
                                         })
                            .WithMetrics(meterProviderBuilder =>
                                         {
                                             meterProviderBuilder.SetResourceBuilder(resourceBuilder)
                                                                 .AddAspNetCoreInstrumentation()
                                                                 .AddHttpClientInstrumentation()
                                                                 .AddMeter("OpenTelemetryDemo.Metrics")
                                                                 .AddOtlpExporter(otlpOptions =>
                                                                                  {
                                                                                      otlpOptions.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_METRICS_ENDPOINT")!);
                                                                                      otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                                                                                  })
                                                                 .AddConsoleExporter();
                                         });

            builder.Logging.AddOpenTelemetry(options =>
                                             {
                                                 options.SetResourceBuilder(resourceBuilder)
                                                        .AddOtlpExporter(otlpOptions =>
                                                                         {
                                                                             otlpOptions.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_LOGS_ENDPOINT")!);
                                                                             otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                                                                         });
                                             })
                           .AddConsole();

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
