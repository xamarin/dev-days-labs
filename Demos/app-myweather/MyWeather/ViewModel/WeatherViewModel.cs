using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using MyWeather.Helpers;
using MyWeather.Models;
using MyWeather.Services;
using Xamarin.Essentials;

namespace MyWeather.ViewModels
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        readonly WeakEventManager onPropertyChangedEventManager = new WeakEventManager();

        bool useGPS;
        string temperature = string.Empty;
        string location = Settings.City;
        bool isImperial = Settings.IsImperial;
        string condition = string.Empty;
        bool isBusy = false;
        WeatherForecastRoot forecast;
        ICommand getWeatherCommand;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => onPropertyChangedEventManager.AddEventHandler(value);
            remove => onPropertyChangedEventManager.RemoveEventHandler(value);
        }

        public ICommand GetWeatherCommand => getWeatherCommand ??
            (getWeatherCommand = new AsyncCommand(() => ExecuteGetWeatherCommand(UseGPS), continueOnCapturedContext: false));

        WeatherService WeatherService { get; } = new WeatherService();

        public string Location
        {
            get => location;
            set => SetProperty(ref location, value, () => Settings.City = value);
        }

        public bool UseGPS
        {
            get => useGPS;
            set => SetProperty(ref useGPS, value);
        }

        public bool IsImperial
        {
            get => isImperial;
            set => SetProperty(ref isImperial, value, () => Settings.IsImperial = value);
        }

        public string Temperature
        {
            get => temperature;
            set => SetProperty(ref temperature, value);
        }

        public string Condition
        {
            get => condition;
            set => SetProperty(ref condition, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public WeatherForecastRoot Forecast
        {
            get => forecast;
            set => SetProperty(ref forecast, value);
        }

        async Task ExecuteGetWeatherCommand(bool useGps)
        {
            try
            {
                WeatherRoot weatherRoot;

                var units = IsImperial ? Units.Imperial : Units.Metric;

                if (useGps)
                {
                    var gps = await Geolocation.GetLocationAsync().ConfigureAwait(false);
                    weatherRoot = await WeatherService.GetWeather(gps.Latitude, gps.Longitude, units).ConfigureAwait(false);
                }
                else
                {
                    //Get weather by city
                    weatherRoot = await WeatherService.GetWeather(Location.Trim(), units).ConfigureAwait(false);
                }

                //Get forecast based on cityId
                Forecast = await WeatherService.GetForecast(weatherRoot, units).ConfigureAwait(false);

                var unit = IsImperial ? "F" : "C";
                Temperature = $"Temp: {weatherRoot?.MainWeather?.Temperature ?? 0}°{unit}";
                Condition = $"{weatherRoot.Name}: {weatherRoot?.Weather?[0]?.Description ?? string.Empty}";

                IsBusy = false;

                await TextToSpeech.SpeakAsync(Temperature + " " + Condition).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                DebugServices.Report(e);
                Temperature = "Unable to get Weather";
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected void SetProperty<T>(ref T backingStore, T value, Action? onChanged = null, [CallerMemberName] string propertyname = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            onPropertyChangedEventManager.HandleEvent(this, new PropertyChangedEventArgs(name), nameof(INotifyPropertyChanged.PropertyChanged));
        }

    }
}
