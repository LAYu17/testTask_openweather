using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeather
{
    class TemperatureInfo
    {
        public long dt { get; set; }
        public float Temp { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
        public float Feels_Like { get; set; }
    }
}
