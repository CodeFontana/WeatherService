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

    public CurrentWeatherService(HttpClient httpClient,
                                 IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    private record Weather(string Description);
    private record Main(decimal Temp);
    private record Forecast(Weather[] Weather, Main Main);

    public async Task<CurrentWeatherModel> GetCurrentWeatherAsync(string cityName, string state)
    {
        string baseUrl = _configuration.GetValue<string>("OpenWeatherMap:Host");
        string apiKey = _configuration.GetValue<string>("OpenWeatherMap:ApiKey");
        string requestUri = $"https://{baseUrl}/data/2.5/weather?q={cityName},{state}&appid={apiKey}&units=metric";

        Forecast apiResult = await _httpClient.GetFromJsonAsync<Forecast>(requestUri);

        CurrentWeatherModel curWeather = new()
        {
            Date = DateTime.Now,
            TemperatureC = Math.Round(apiResult.Main.Temp),
            TemperatureF = 32 + Math.Round(apiResult.Main.Temp / (decimal)0.5556, 0),
            Summary = apiResult.Weather[0]?.Description
        };

        return curWeather;
    }
}
