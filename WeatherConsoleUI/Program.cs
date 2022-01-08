using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WeatherConsoleUI.Services;

namespace WeatherConsoleUI;

class Program
{
    public static IConfigurationRoot Configuration { get; set; }

    static async Task Main(string[] args)
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        bool isDevelopment = string.IsNullOrEmpty(env) || env.ToLower() == "development";

        await Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile($"appsettings.json", true, true);
                config.AddJsonFile($"appsettings.{env}.json", true, true);
                config.AddEnvironmentVariables();

                if (isDevelopment)
                {
                    config.AddUserSecrets<Program>(optional: true);
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient<CurrentWeatherService>();
                services.AddHostedService<WeatherApp>();
            })
            .ConfigureLogging((hostContext, config) =>
            {
                config.ClearProviders();
                config.AddSimpleConsole(options =>
                {
                    options.SingleLine = false;
                    options.TimestampFormat = "MM/dd/yyyy HH:mm:ss ";
                });
                config.SetMinimumLevel(LogLevel.Debug);
                config.AddFilter("Microsoft", LogLevel.None);
            })
            .RunConsoleAsync();
    }
}
