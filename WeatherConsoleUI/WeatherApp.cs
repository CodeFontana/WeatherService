using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherConsoleUI.Models;
using WeatherConsoleUI.Services;

namespace WeatherConsoleUI;

public class WeatherApp : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IConfiguration _config;
    private readonly ILogger<WeatherApp> _logger;
    private readonly CurrentWeatherService _currentWeatherService;

    public WeatherApp(IHostApplicationLifetime hostApplicationLifetime,
                     IConfiguration configuration,
                     ILogger<WeatherApp> logger, 
                     CurrentWeatherService currentWeatherService)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _config = configuration;
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
        Console.WriteLine(JsonSerializer.Serialize(curWeather, new JsonSerializerOptions { WriteIndented = true }));
    }
}
