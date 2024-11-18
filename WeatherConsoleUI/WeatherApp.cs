using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherConsoleUI.Models;
using WeatherConsoleUI.Services;

namespace WeatherConsoleUI;

public class WeatherApp : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<WeatherApp> _logger;
    private readonly CurrentWeatherService _currentWeatherService;

    public WeatherApp(IHostApplicationLifetime hostApplicationLifetime,
                      ILogger<WeatherApp> logger,
                      CurrentWeatherService currentWeatherService)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
        _currentWeatherService = currentWeatherService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _hostApplicationLifetime.ApplicationStarted.Register(async () =>
        {
            try
            {
                await Task.Yield(); // https://github.com/dotnet/runtime/issues/36063
                await ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception!");
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task ExecuteAsync()
    {
        CurrentWeatherModel curWeather = await _currentWeatherService.GetCurrentWeatherAsync("Center Moriches", "New York");
        JsonSerializerOptions options = new() { WriteIndented = true };
        _logger.LogInformation("{message}", JsonSerializer.Serialize(curWeather, options));
        _logger.LogInformation("Press any key to exit...");
        Console.ReadLine();
    }
}
