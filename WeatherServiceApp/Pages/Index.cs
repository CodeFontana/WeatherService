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
        private CurrentWeatherRequestModel _request = new();
        private CurrentWeatherResultModel result;

        [Inject]
        public CurrentWeatherService ForecastService { get; set; }

        private async Task HandleValidSubmit()
        {
            result = await ForecastService.GetCurrentWeatherAsync(_request.City, _request.State);
        }
    }
}
