using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherConsoleUI.Models
{
    public class CurrentWeatherModel
    {
        public DateTime Date { get; set; }
        public decimal TemperatureC { get; set; }
        public decimal TemperatureF { get; set; }
        public string Summary { get; set; }
    }
}
