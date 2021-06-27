using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherServiceApp.Models
{
    public class CurrentWeatherRequestModel
    {
        public string City { get; set; }
        public string State { get; set; }
    }
}
