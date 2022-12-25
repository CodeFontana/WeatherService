using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WeatherConsoleUI.Services;
using ConsoleLoggerLibrary;
using Microsoft.Extensions.Configuration;

namespace WeatherConsoleUI;

class Program
{
    static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddUserSecrets<Program>(optional: true);
            })
            .ConfigureLogging((context, builder) =>
            {
                builder.ClearProviders();
                builder.AddConsoleLogger(context.Configuration);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient<CurrentWeatherService>();
                services.AddHostedService<WeatherApp>();
            })
            .RunConsoleAsync();
    }
}
