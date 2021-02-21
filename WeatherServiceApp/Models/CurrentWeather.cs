using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherServiceApp.Models
{
    public class CurrentWeather
    {
        public DateTime Date { get; set; }
        public decimal TemperatureC { get; set; }
        public decimal TemperatureF => 32 + (TemperatureC / (decimal)0.5556);
        public string Summary { get; set; }
    }
}
