using WeatherServiceApp.Models;

namespace WeatherServiceApp.Interfaces;
public interface ICurrentWeatherService
{
    Task<CurrentWeatherResultModel> GetCurrentWeatherAsync(string city, string state = null);
}