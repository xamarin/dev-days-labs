using System.Threading.Tasks;
using MyWeather.Models;
using MyWeather.Services;
using Refit;

namespace MyWeather
{
    interface IWeatherApi
    {
        [Get("/forecast")]
        Task<WeatherForecastRoot> GetForecast([AliasAs("id")] int cityId, string units, [AliasAs("appid")] string appId = WeatherService.AppId);

        [Get("/forecast")]
        Task<WeatherForecastRoot> GetForecast([AliasAs("lat")] double latitude, [AliasAs("lon")] double longitude, string units, [AliasAs("appid")] string appId = WeatherService.AppId);

        [Get("/weather")]
        Task<WeatherRoot> GetWeather([AliasAs("lat")]double latitude, [AliasAs("lon")] double longitude, string units, [AliasAs("appid")] string appId = WeatherService.AppId);

        [Get("/weather")]
        Task<WeatherRoot> GetWeather([AliasAs("q")] string city, string units, [AliasAs("appid")] string appId = WeatherService.AppId);
    }
}

