using System.Globalization;
using WeatherServiceApp.Models;

namespace WeatherServiceApp.Services;

public sealed class CurrentWeatherService : ICurrentWeatherService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public CurrentWeatherService(IConfiguration configuration,
                                 IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    private record Weather(string Description);
    private record Main(decimal Temp);
    private record CurrentWeather(Weather[] Weather, Main Main);

    public async Task<CurrentWeatherResultModel> GetCurrentWeatherAsync(string city, string? state = null)
    {
        string? apiKey = _configuration.GetValue<string>("OpenWeatherMap:ApiKey")
            ?? throw new InvalidOperationException("OpenWeatherMap API key is not configured.");
        
        string requestUri = state switch {
             _ when string.IsNullOrWhiteSpace(state) => $"weather?q={city}&appid={apiKey}&units=metric",
             _ => $"weather?q={city},{state}&appid={apiKey}&units=metric"
        };

        HttpClient http = _httpClientFactory.CreateClient("currentWeather");
        CurrentWeather? result = await http.GetFromJsonAsync<CurrentWeather>(requestUri)
            ?? throw new InvalidOperationException("Failed to retrieve current weather data.");
        
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        CurrentWeatherResultModel curWeather = new()
        {
            TemperatureC = Math.Round(result.Main.Temp),
            TemperatureF = 32 + Math.Round(result.Main.Temp / (decimal)0.5556, 0),
            Summary = textInfo.ToTitleCase(result.Weather[0]?.Description ?? "No description")
        };

        return curWeather;
    }
}
