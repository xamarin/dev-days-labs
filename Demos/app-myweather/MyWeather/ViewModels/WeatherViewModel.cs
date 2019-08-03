using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using MyWeather.Models;
using MyWeather.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyWeather.ViewModels
{
    class WeatherViewModel : INotifyPropertyChanged
    {
        readonly WeakEventManager onPropertyChangedEventManager = new WeakEventManager();

        bool useGPS;
        string temperature = string.Empty;
        string location = Settings.City;
        bool isImperial = Settings.IsImperial;
        string condition = string.Empty;
        bool isBusy;
        WeatherForecastRoot forecast;
        AsyncCommand getWeatherCommand;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => onPropertyChangedEventManager.AddEventHandler(value);
            remove => onPropertyChangedEventManager.RemoveEventHandler(value);
        }

        public AsyncCommand GetWeatherCommand => getWeatherCommand ??
            (getWeatherCommand = new AsyncCommand(() => ExecuteGetWeatherCommand(UseGPS), _ => !IsBusy));

        public bool IsLocationEntryEnabled => !UseGPS;

        public List<WeatherRoot> ForecastItems => Forecast?.Items;

        public string Location
        {
            get => location;
            set => SetProperty(ref location, value, () => Settings.City = value);
        }

        public bool UseGPS
        {
            get => useGPS;
            set => SetProperty(ref useGPS, value, () => OnPropertyChanged(nameof(IsLocationEntryEnabled)));
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
            set => SetProperty(ref isBusy, value, () => Device.BeginInvokeOnMainThread(GetWeatherCommand.RaiseCanExecuteChanged));
        }

        public WeatherForecastRoot Forecast
        {
            get => forecast;
            set => SetProperty(ref forecast, value, () => OnPropertyChanged(nameof(ForecastItems)));
        }

        async Task ExecuteGetWeatherCommand(bool useGps)
        {
            IsBusy = true;
            Temperature = Condition = string.Empty;

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

                TextToSpeech.SpeakAsync(Temperature + " " + Condition).SafeFireAndForget();
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

        protected void SetProperty<T>(ref T backingStore, in T value, in Action onChanged = null, [CallerMemberName] in string propertyname = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyname);
        }

        void OnPropertyChanged([CallerMemberName] in string propertyName = "") =>
            onPropertyChangedEventManager.HandleEvent(this, new PropertyChangedEventArgs(propertyName), nameof(INotifyPropertyChanged.PropertyChanged));
    }
}
