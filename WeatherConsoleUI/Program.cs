using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherConsoleUI.Services;
using WeatherConsoleUI;
using ConsoleLoggerLibrary;

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
