using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherConsoleUI.Models;

namespace WeatherConsoleUI.Services;

public class CurrentWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CurrentWeatherService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    private record Weather(string description);
    private record Main(decimal temp);
    private record Forecast(Weather[] weather, Main main);

    public async Task<CurrentWeatherModel> GetCurrentWeatherAsync(string cityName, string state)
    {
        string baseUrl = _configuration.GetValue<string>("OpenWeatherMap:Host");
        string apiKey = _configuration.GetValue<string>("OpenWeatherMap:ApiKey");

        Forecast apiResult = await _httpClient
            .GetFromJsonAsync<Forecast>(
            $"https://{baseUrl}/data/2.5/weather?q={cityName},{state}&appid={apiKey}&units=metric");

        CurrentWeatherModel curWeather = new()
        {
            Date = DateTime.Now,
            TemperatureC = Math.Round(apiResult.main.temp),
            TemperatureF = 32 + Math.Round(apiResult.main.temp / (decimal)0.5556, 0),
            Summary = apiResult.weather[0]?.description
        };

        return curWeather;
    }
}
