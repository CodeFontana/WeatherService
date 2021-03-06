using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherServiceApp.Models;

namespace WeatherServiceApp.Services
{
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

        public string FeedbackMessage { get; set; }

        public async Task<CurrentWeatherResultModel> GetCurrentWeatherAsync(string city, string state = null)
        {
            FeedbackMessage = "";

            if (string.IsNullOrWhiteSpace(city))
            {
                FeedbackMessage = "City name must be non-empty!";
                return null;
            }
            
            string baseUrl = _configuration.GetValue<string>("OpenWeatherMap:Host");
            string apiKey = _configuration.GetValue<string>("OpenWeatherMap:ApiKey");
            string requestUri;

            if (string.IsNullOrWhiteSpace(state))
            {
                requestUri = $"https://{baseUrl}/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
            }
            else
            {
                requestUri = $"https://{baseUrl}/data/2.5/weather?q={city},{state}&appid={apiKey}&units=metric";
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            Forecast apiResult;

            if (response.IsSuccessStatusCode)
            {
                apiResult = JsonSerializer.Deserialize<Forecast>(await response.Content.ReadAsStringAsync());

                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                CurrentWeatherResultModel curWeather = new()
                {
                    TemperatureC = Math.Round(apiResult.main.temp),
                    TemperatureF = 32 + Math.Round(apiResult.main.temp / (decimal)0.5556, 0),
                    Summary = textInfo.ToTitleCase(apiResult.weather[0]?.description)
                };

                return curWeather;
            }
            else
            {
                FeedbackMessage = $"Error: {response.ReasonPhrase}";
                return null;
            }
        }
    }
}
