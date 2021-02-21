using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherServiceApp.Models;

namespace WeatherServiceApp.Services
{
    public class CurrentWeatherService
    {
        private readonly HttpClient _httpClient;

        private string ApiHost { get; init; }
        private string ApiKey { get; init; }

        public CurrentWeatherService(
            HttpClient httpClient,
            IOptions<OpenWeather> options)
        {
            _httpClient = httpClient;
            ApiHost = options.Value.Host;
            ApiKey = options.Value.ApiKey;
        }

        private record Weather(string description);

        private record Main(decimal temp);

        private record Forecast(Weather[] weather, Main main);

        public async Task<CurrentWeather> GetCurrentWeatherAsync(string cityName)
        {
            Forecast apiResult = await _httpClient
                .GetFromJsonAsync<Forecast>(
                $"https://{ApiHost}/data/2.5/weather?q={cityName}&appid={ApiKey}&units=metric");

            CurrentWeather curWeather = new CurrentWeather
            {
                Date = DateTime.Now,
                TemperatureC = apiResult.main.temp,
                Summary = apiResult.weather[0]?.description
            };

            return curWeather;
        }
    }
}
