using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MyWeather.Models
{
    class Coord
    {
        public Coord(double lon, double lat) =>
            (Longitude, Latitude) = (lon, lat);

        [JsonProperty("lon")]
        public double Longitude { get; }

        [JsonProperty("lat")]
        public double Latitude { get; }
    }

    class Sys
    {
        public Sys(string country) => Country = country;

        [JsonProperty("country")]
        public string Country { get; }
    }

    class Weather
    {
        public Weather(int id, string main, string description, string icon) =>
            (Id, Main, Description, Icon) = (id, main, description, icon);

        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("main")]
        public string Main { get; }

        [JsonProperty("description")]
        public string Description { get; }

        [JsonProperty("icon")]
        public string Icon { get; }
    }

    class Main
    {
        public Main(double temp, double pressure, double humidity, double temp_min, double temp_max) =>
            (Temperature, Pressure, Humidity, MinTemperature, MaxTemperature) = (temp, pressure, humidity, temp_min, temp_max);

        [JsonProperty("temp")]
        public double Temperature { get; }
        [JsonProperty("pressure")]
        public double Pressure { get; }

        [JsonProperty("humidity")]
        public double Humidity { get; }
        [JsonProperty("temp_min")]
        public double MinTemperature { get; }

        [JsonProperty("temp_max")]
        public double MaxTemperature { get; }
    }

    class Wind
    {
        public Wind(double speed, double deg) =>
            (Speed, WindDirectionDegrees) = (speed, deg);

        [JsonProperty("speed")]
        public double Speed { get; }

        [JsonProperty("deg")]
        public double WindDirectionDegrees { get; }

    }

    class Clouds
    {
        public Clouds(int all) => CloudinessPercent = all;

        [JsonProperty("all")]
        public int CloudinessPercent { get; }
    }

    class WeatherRoot
    {
        public WeatherRoot(Coord coord, Sys sys, IEnumerable<Weather> weather, Main main, Wind wind, Clouds clouds, int id, string name, string dt_txt) =>
            (Coordinates, System, Weather, MainWeather, Wind, Clouds, CityId, Name, Date) = (coord, sys, weather.ToList(), main, wind, clouds, id, name, dt_txt);

        [JsonIgnore]
        public string DisplayDate => DateTime.Parse(Date).ToLocalTime().ToString("g");
        [JsonIgnore]
        public string DisplayTemp => $"Temp: {MainWeather.Temperature}° {Weather[0]?.Main ?? string.Empty}";
        [JsonIgnore]
        public string DisplayIcon => $"https://openweathermap.org/img/w/{Weather?[0]?.Icon}.png";

        [JsonProperty("coord")]
        public Coord Coordinates { get; }

        [JsonProperty("sys")]
        public Sys System { get; }

        [JsonProperty("weather")]
        public List<Weather> Weather { get; }

        [JsonProperty("main")]
        public Main MainWeather { get; }

        [JsonProperty("wind")]
        public Wind Wind { get; }

        [JsonProperty("clouds")]
        public Clouds Clouds { get; }

        [JsonProperty("id")]
        public int CityId { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("dt_txt")]
        public string Date { get; }
    }

    class WeatherForecastRoot
    {
        public WeatherForecastRoot(City city, string cod, double message, int cnt, IEnumerable<WeatherRoot> list) =>
            (City, Cod, Message, Cnt, Items) = (city, cod, message, cnt, list.ToList());

        [JsonProperty("city")]
        public City City { get; }

        [JsonProperty("cod")]
        public string Cod { get; }

        [JsonProperty("message")]
        public double Message { get; }

        [JsonProperty("cnt")]
        public int Cnt { get; }

        [JsonProperty("list")]
        public List<WeatherRoot> Items { get; }
    }

    class City
    {
        public City(int id, string name, Coord coord, string country, int population, Sys sys) =>
            (Id, Name, Coord, Country, Population, Sys) = (id, name, coord, country, population, sys);

        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("coord")]
        public Coord Coord { get; }

        [JsonProperty("country")]
        public string Country { get; }

        [JsonProperty("population")]
        public int Population { get; }

        [JsonProperty("sys")]
        public Sys Sys { get; }
    }
}
