using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherServiceApp.Models
{
    public class OpenWeather
    {
        public string Host { get; set; }
        public string City { get; set; }
        public string ApiKey { get; set; }
    }
}
