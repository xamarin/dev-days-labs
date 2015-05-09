using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeather.Models
{
	public class Coord
	{
		[JsonProperty("lon")]
		public double Longitude { get; set; }

		[JsonProperty("lat")]
		public double Latitude { get; set; }
	}

	public class Sys
	{

		[JsonProperty("country")]
		public string Country { get; set; }
	}

	public class Weather
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("main")]
		public string Main { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }
	}

	public class Main
	{
		[JsonProperty("temp")]
		public double Temperature { get; set; }
		[JsonProperty("pressure")]
		public double Pressure { get; set; }

		[JsonProperty("humidity")]
		public double Humidity { get; set; }
		[JsonProperty("temp_min")]
		public double MinTemperature { get; set; }

		[JsonProperty("temp_max")]
		public double MaxTemperature { get; set; }
	}

	public class Wind
	{
		[JsonProperty("speed")]
		public double Speed { get; set; }

		[JsonProperty("deg")]
		public double WindDirectionDegrees { get; set; }

	}

	public class Clouds
	{

		[JsonProperty("all")]
		public int CloudinessPercent { get; set; }
	}

	public class WeatherRoot
	{
		[JsonProperty("coord")]
		public Coord Coordinates { get; set; }

		[JsonProperty("sys")]
		public Sys System { get; set; }

		[JsonProperty("weather")]
		public List<Weather> Weather { get; set; }

		[JsonProperty("main")]
		public Main MainWeather { get; set; }

		[JsonProperty("wind")]
		public Wind Wind { get; set; }

		[JsonProperty("clouds")]
		public Clouds Clouds { get; set; }

		[JsonProperty("id")]
		public int CityId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

}
