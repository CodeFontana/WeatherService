using System;

namespace WeatherConsoleUI.Models;

public sealed class CurrentWeatherModel
{
    public DateTime Date { get; set; }
    public decimal TemperatureC { get; set; }
    public decimal TemperatureF { get; set; }
    public string? Summary { get; set; }
}
