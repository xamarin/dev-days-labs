using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

using MyWeather.Helpers;
using MyWeather.Models;
using MyWeather.Services;

using Plugin.Geolocator;
using Plugin.TextToSpeech;

using Xamarin.Forms;

namespace MyWeather.ViewModels
{
	public class WeatherViewModel : INotifyPropertyChanged
	{
		bool useGPS;
		string temperature = string.Empty;
		string location = Settings.City;
		bool isImperial = Settings.IsImperial;
		string condition = string.Empty;
		bool isBusy = false;
		WeatherForecastRoot forecast;
		ICommand getWeather;

		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand GetWeatherCommand => getWeather ??
			(getWeather = new Command(async () => await ExecuteGetWeatherCommand()));

		WeatherService WeatherService { get; } = new WeatherService();

		public string Location
		{
			get { return location; }
			set
			{
				location = value;
				OnPropertyChanged();
				Settings.City = value;
			}
		}

		public bool UseGPS
		{
			get { return useGPS; }
			set
			{
				useGPS = value;
				OnPropertyChanged();
			}
		}

		public bool IsImperial
		{
			get { return isImperial; }
			set
			{
				isImperial = value;
				OnPropertyChanged();
				Settings.IsImperial = value;
			}
		}

		public string Temperature
		{
			get { return temperature; }
			set
			{
				temperature = value;
				OnPropertyChanged();
			}
		}

		public string Condition
		{
			get { return condition; }
			set
			{
				condition = value;
				OnPropertyChanged();
			}
		}

		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				isBusy = value;
				OnPropertyChanged();
			}
		}

		public WeatherForecastRoot Forecast
		{
			get { return forecast; }
			set
			{
				forecast = value;
				OnPropertyChanged();
			}
		}

		async Task ExecuteGetWeatherCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

            try
            {
                WeatherRoot weatherRoot = null;

				var units = IsImperial ? Units.Imperial : Units.Metric;

				if (UseGPS)
				{
					var gps = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
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

				await CrossTextToSpeech.Current.Speak(Temperature + " " + Condition).ConfigureAwait(false);
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

		void OnPropertyChanged([CallerMemberName]string name = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
