using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using WeatherConsoleUI.Services;

namespace WeatherConsoleUI
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static async Task Main(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            using IHost host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            try
            {
                await services.GetRequiredService<WeatherApp>().Run();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: { e.Message }");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logger =>
                {
                    logger.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient<CurrentWeatherService>();
                    services.AddTransient<WeatherApp>();
                });
    }
}
