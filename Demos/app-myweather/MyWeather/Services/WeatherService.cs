using System;
using System.Net.Http;
using System.Threading.Tasks;
using MyWeather.Models;
using Polly;
using Refit;
using Xamarin.Forms;

namespace MyWeather.Services
{
    enum Units
    {
        Imperial,
        Metric
    }

    static class WeatherService
    {
        public const string AppId = "c62b6ebb8355e7c293d97020bcbb52e8";

        readonly static Lazy<IWeatherApi> weatherApiServiceHolder =
            new Lazy<IWeatherApi>(() => RestService.For<IWeatherApi>(CreateHttpClient("https://api.openweathermap.org/data/2.5")));

        static IWeatherApi WeatherApiService => weatherApiServiceHolder.Value;

        public static Task<WeatherRoot> GetWeather(double latitude, double longitude, Units units = Units.Imperial) =>
            AttemptAndRetry(() => WeatherApiService.GetWeather(latitude, longitude, units.ToLowerString()));

        public static Task<WeatherRoot> GetWeather(string city, Units units = Units.Imperial) =>
            AttemptAndRetry(() => WeatherApiService.GetWeather(city, units.ToLowerString()));

        public static Task<WeatherForecastRoot> GetForecast(WeatherRoot weather, Units units = Units.Imperial)
        {
            if (weather.CityId is 0)
                return GetForecast(weather.Coordinates.Latitude, weather.Coordinates.Longitude, units);

            return GetForecast(weather.CityId, units);
        }

        static Task<WeatherForecastRoot> GetForecast(int id, Units units = Units.Imperial) =>
            AttemptAndRetry(() => WeatherApiService.GetForecast(id, units.ToLowerString()));

        static Task<WeatherForecastRoot> GetForecast(double latitude, double longitude, Units units = Units.Imperial) =>
            AttemptAndRetry(() => WeatherApiService.GetForecast(latitude, longitude, units.ToLowerString()));

        static HttpClient CreateHttpClient(string url)
        {
            HttpClient client;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    client = new HttpClient();
                    break;
                default:
                    client = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip });
                    break;
            }
            client.BaseAddress = new Uri(url);

            return client;
        }

        static Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 3)
        {
            return Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);

            TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }

        static string ToLowerString(this Units units) => units.ToString().ToLower();
    }
}
