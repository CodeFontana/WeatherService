using System;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherConsoleUI.Models;
using WeatherConsoleUI.Services;

namespace WeatherConsoleUI;

public class WeatherApp
{
    private readonly CurrentWeatherService _currentWeatherService;

    public WeatherApp(CurrentWeatherService currentWeatherService)
    {
        _currentWeatherService = currentWeatherService;
    }

    public async Task Run()
    {
        CurrentWeatherModel curWeather = await _currentWeatherService.GetCurrentWeatherAsync("Center Moriches", "New York");
        Console.WriteLine(JsonSerializer.Serialize(curWeather, new JsonSerializerOptions { WriteIndented = true }));
    }
}
