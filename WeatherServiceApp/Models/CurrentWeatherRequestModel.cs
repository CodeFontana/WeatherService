using System.ComponentModel.DataAnnotations;

namespace WeatherServiceApp.Models;

public class CurrentWeatherRequestModel
{
    [Required]
    public string City { get; set; }
    public string State { get; set; }
}
