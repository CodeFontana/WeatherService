using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text.Json;
using WeatherServiceApp.Interfaces;
using WeatherServiceApp.Models;

namespace WeatherServiceApp.Services;

public class CurrentWeatherService : ICurrentWeatherService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public CurrentWeatherService(IConfiguration configuration, 
                                 IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    private record Weather(string description);

    private record Main(decimal temp);

    private record CurrentWeather(Weather[] weather, Main main);

    public async Task<CurrentWeatherResultModel> GetCurrentWeatherAsync(string city, string state = null)
    {
        string apiKey = _configuration.GetValue<string>("OpenWeatherMap:ApiKey");
        string requestUri;

        if (string.IsNullOrWhiteSpace(state))
        {
            requestUri = $"weather?q={city}&appid={apiKey}&units=metric";
        }
        else
        {
            requestUri = $"weather?q={city},{state}&appid={apiKey}&units=metric";
        }

        HttpClient http = _httpClientFactory.CreateClient("currentWeather");
        CurrentWeather result = await http.GetFromJsonAsync<CurrentWeather>(requestUri);
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        CurrentWeatherResultModel curWeather = new()
        {
            TemperatureC = Math.Round(result.main.temp),
            TemperatureF = 32 + Math.Round(result.main.temp / (decimal)0.5556, 0),
            Summary = textInfo.ToTitleCase(result.weather[0]?.description)
        };

        return curWeather;
    }
}
