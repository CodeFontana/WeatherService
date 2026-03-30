namespace WeatherServiceApp.Models;

public sealed class CurrentWeatherResultModel
{
    public decimal TemperatureC { get; set; }
    public decimal TemperatureF { get; set; }
    public string? Summary { get; set; }
}
