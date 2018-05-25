using System;
using System.Net.Http;
using System.Threading.Tasks;

using MyWeather.Models;

using static Newtonsoft.Json.JsonConvert;

namespace MyWeather.Services
{
	public enum Units
	{
		Imperial,
		Metric
	}

	public class WeatherService : BaseHttpClientService
	{
		const string openWeatherMapUrl = "https://api.openweathermap.org/data/2.5";
		const string appId = "fc9f6c524fc093759cd28d41fda89a1b";

		public Task<WeatherRoot> GetWeather(double latitude, double longitude, Units units = Units.Imperial) =>
			GetObjectFromAPI<WeatherRoot>($"{openWeatherMapUrl}/weather?lat={latitude}&lon={longitude}&units={units.ToString().ToLower()}&appid={appId}");

		public Task<WeatherRoot> GetWeather(string city, Units units = Units.Imperial) =>
			GetObjectFromAPI<WeatherRoot>($"{openWeatherMapUrl}/weather?q={city}&units={units.ToString().ToLower()}&appid={appId}");

		public Task<WeatherForecastRoot> GetForecast(WeatherRoot weather, Units units = Units.Imperial)
		{
			if (weather.CityId == 0)
				return GetForecast(weather.Coordinates.Latitude, weather.Coordinates.Longitude, units);

			return GetForecast(weather.CityId, units);
		}

		Task<WeatherForecastRoot> GetForecast(int id, Units units = Units.Imperial) =>
			GetObjectFromAPI<WeatherForecastRoot>($"{openWeatherMapUrl}/forecast?id={id}&units={units.ToString().ToLower()}&appid={appId}");

		Task<WeatherForecastRoot> GetForecast(double latitude, double longitude, Units units = Units.Imperial) =>
			GetObjectFromAPI<WeatherForecastRoot>($"{openWeatherMapUrl}/forecast?lat={latitude}&lon={longitude}&units={units.ToString().ToLower()}&appid={appId}");
	}
}
