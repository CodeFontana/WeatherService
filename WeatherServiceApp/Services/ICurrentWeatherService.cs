using WeatherServiceApp.Models;

namespace WeatherServiceApp.Services;

public interface ICurrentWeatherService
{
    Task<CurrentWeatherResultModel> GetCurrentWeatherAsync(string city, string? state = null);
}