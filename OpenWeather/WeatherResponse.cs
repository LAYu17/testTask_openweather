using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeather
{
    class WeatherResponse
    {
        public float lat { get; set; }
        public float lon { get; set; }
        public TemperatureInfo current { get; set; }
        public List<TemperatureInfo> hourly { get; set; }
    }
}
