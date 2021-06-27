using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherServiceApp.Models
{
    public class CurrentWeatherResultModel
    {
        public decimal TemperatureC { get; set; }
        public decimal TemperatureF { get; set; }
        public string Summary { get; set; }
    }
}
