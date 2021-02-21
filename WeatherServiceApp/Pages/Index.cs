using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherServiceApp.Models;
using WeatherServiceApp.Services;

namespace WeatherServiceApp.Pages
{
    public partial class Index
    {
        private CurrentWeather _current;
        private string _city = "Center Moriches";

        [Inject]
        public CurrentWeatherService ForecastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _current = await ForecastService.GetCurrentWeatherAsync(_city);
        }
    }
}
