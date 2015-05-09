using MyWeather.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyWeather.Services
{
	public enum Units
	{
		Imperial,
		Metric
	}


	public class WeatherService
	{
		private const string WeatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}";
		private const string WeatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0}&units={1}";


		public async Task<WeatherRoot> GetWeather(double latitude, double longitude, Units units = Units.Imperial)
		{
			using (var client = new HttpClient())
			{
				var url = string.Format(WeatherCoordinatesUri, latitude, longitude, units.ToString().ToLower());
				var json = await client.GetStringAsync(url);

				if (string.IsNullOrWhiteSpace(json))
					return null;

				return JsonConvert.DeserializeObject<WeatherRoot>(json);
			}
			
		}

		public async Task<WeatherRoot> GetWeather(string city, Units units = Units.Imperial)
		{
			using (var client = new HttpClient())
			{
				var url = string.Format(WeatherCityUri, city, units.ToString().ToLower());
				var json = await client.GetStringAsync(url);

				if (string.IsNullOrWhiteSpace(json))
					return null;

				return JsonConvert.DeserializeObject<WeatherRoot>(json);
			}

		}
	}
}
