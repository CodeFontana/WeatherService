﻿using Microsoft.Extensions.Configuration;
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

        public async Task<CurrentWeatherModel> GetCurrentWeatherAsync(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
            {
                return null;
            }
            
            string baseUrl = _configuration.GetValue<string>("OpenWeatherMap:Host");
            string apiKey = _configuration.GetValue<string>("OpenWeatherMap:ApiKey");

            Forecast apiResult = await _httpClient
                .GetFromJsonAsync<Forecast>(
                $"https://{baseUrl}/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric");

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
}
