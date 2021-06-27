using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WeatherConsoleUI.Services;

namespace WeatherConsoleUI
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static async Task Main(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            bool isDevelopment = string.IsNullOrEmpty(env) || env.ToLower() == "development";

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            Configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddHttpClient<CurrentWeatherService>();
            services.AddTransient<WeatherApp>();
            services.BuildServiceProvider();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            try
            {
                await serviceProvider.GetService<WeatherApp>().Run();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: { e.Message }");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
