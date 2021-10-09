using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace OpenWeather
{
    class Program
    {
        static void Main(string[] args)
        {
            var lat = 56.7320;
            var lon = 37.1669;
            var appid = "1234567890"; // https://home.openweathermap.org/api_keys - to do your appid key
            var units = "metric";

            var today = DateTime.Now;

            var listDates = new List<DateTime>();
            var listUnixDates = new List<long>();
            List<string> listResponse = new List<string>();
            var weatherResponseList = new List<WeatherResponse>();

            for (int i = 0; i < 5; i++)
            {
                listDates.Add(today.AddDays(-i));
                listUnixDates.Add(((DateTimeOffset)listDates[i]).ToUnixTimeSeconds());
            }

            var dateFrom = listDates[0].ToString().Split(' ');
            var dateTo = listDates[4].ToString().Split(' ');

            for (int i = 0; i < 5; i++)
            {
                var dt = listUnixDates[i];
                string url = $"http://api.openweathermap.org/data/2.5/onecall/timemachine?lat={lat}&lon={lon}&dt={dt}&appid={appid}&units={units}";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string response;

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                listResponse.Add(response);
                weatherResponseList.Add(JsonConvert.DeserializeObject<WeatherResponse>(response));
            }

            var listDiff = new List<float>();
            var listDiffsun = new List<TimeSpan>();
            float min;
            TimeSpan maxT;
            for (int i = 0; i < 5; i++)
            {
                listDiff.Add(weatherResponseList[i].hourly[0].Temp - weatherResponseList[i].hourly[0].Feels_Like);
                listDiffsun.Add(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(weatherResponseList[i].current.sunset).ToLocalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(weatherResponseList[i].current.sunrise).ToLocalTime());
            }
            min = listDiff[0];
            maxT = listDiffsun[0];
            int indexD = 0;
            int indexDS = 0;
            for (int i = 1; i < 5; i++)
            {
                if (min > listDiff[i])
                {
                    min = listDiff[i];
                    indexD = i;
                }
                if (maxT < listDiffsun[i])
                {
                    maxT = listDiffsun[i];
                    indexDS = i;
                }
            }
            var indexDateD = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(weatherResponseList[indexD].current.dt).ToLocalTime().ToString();
            string[] parts = indexDateD.Split(' ');
            var indexDateDS = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(weatherResponseList[indexDS].current.dt).ToLocalTime().ToString();
            string[] partsDS = indexDateDS.Split(' ');
            Console.WriteLine("1) Temperature in {0} at night (time taken 00:00) with minimum difference: {1} °С.",
                parts[0], listDiff[indexD]);
            Console.WriteLine("2)Maximum daylight hours in {0} - {1}." +
                "\nDates range: {2} - {3}.", partsDS[0], maxT.ToString(), dateTo[0], dateFrom[0]);

            Console.ReadLine();

        }

    }
}

