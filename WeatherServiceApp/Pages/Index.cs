using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WeatherServiceApp.Models;
using WeatherServiceApp.Services;

namespace WeatherServiceApp.Pages
{
    public partial class Index
    {
        [Inject] public CurrentWeatherService ForecastService { get; set; }

        private CurrentWeatherRequestModel _request = new();
        private CurrentWeatherResultModel result;

        private async Task HandleValidSubmit()
        {
            result = await ForecastService.GetCurrentWeatherAsync(_request.City, _request.State);
        }
    }
}
