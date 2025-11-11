using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;

namespace HappyWarehouse.Application.Extensions;

public static class AppExtension
{
    public static void SerilogConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console();
            loggerConfiguration.WriteTo.File(new JsonFormatter(), "applogs.txt", rollingInterval:RollingInterval.Day);
        });
    }
}