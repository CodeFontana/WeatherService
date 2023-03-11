using Microsoft.AspNetCore.Components;
using WeatherServiceApp.Interfaces;
using WeatherServiceApp.Models;

namespace WeatherServiceApp.Pages;

public partial class Index
{
    [Inject] public ICurrentWeatherService ForecastService { get; set; }

    private CurrentWeatherRequestModel _request = new();
    private CurrentWeatherResultModel result;

    private async Task HandleValidSubmit()
    {
        result = await ForecastService.GetCurrentWeatherAsync(_request.City, _request.State);
    }
}
