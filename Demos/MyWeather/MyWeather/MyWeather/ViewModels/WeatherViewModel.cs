using Geolocator.Plugin;
using MyWeather.Helpers;
using MyWeather.Models;
using MyWeather.Services;
using Refractored.Xam.TTS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyWeather.ViewModels
{
	public class WeatherViewModel : INotifyPropertyChanged
	{
		WeatherService weatherService;
		public WeatherViewModel()
		{
			weatherService = new WeatherService();
		}


		private string location = Settings.City;
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

		private bool useCity = Settings.UseCity;
		public bool UseCity
		{
			get { return useCity; }
			set 
			{
				useCity = value; 
				OnPropertyChanged();
				Settings.UseCity = value;
			}
		}


		private bool isImperial = Settings.IsImperial;
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



		private string temp = string.Empty;
		public string Temp
		{
			get { return temp; }
			set { temp = value; OnPropertyChanged(); }
		}


		private string tempHigh = string.Empty;
		public string TempHigh
		{
			get { return tempHigh; }
			set { tempHigh = value; OnPropertyChanged(); }
		}

		private string tempLow = string.Empty;
		public string TempLow
		{
			get { return tempLow; }
			set { tempLow = value; OnPropertyChanged(); }
		}

		private string tempBig = string.Empty;
		public string TempBig
		{
			get { return tempBig; }
			set { tempBig = value; OnPropertyChanged(); }
		}

		private string condition = string.Empty;
		public string Condition
		{
			get { return condition; }
			set { condition = value; OnPropertyChanged(); }
		}



		private bool isBusy = false;
		public bool IsBusy
		{
			get { return isBusy; }
			set { isBusy = value; OnPropertyChanged(); }
		}

		
		private ICommand getWeather;
		public ICommand GetWeatherCommand
		{
			get
			{
				return getWeather ?? (getWeather = new Command(
				  () => ExecuteGetWeatherCommand()));
			}
		}
		private async Task ExecuteGetWeatherCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			try
			{
				WeatherRoot weatherRoot = null;
				var units = IsImperial ? Units.Imperial : Units.Metric;
				if(UseCity)
				{
					Xamarin.Insights.Track("GetWeather", new Dictionary<string, string>
						{
							{"city", Location.Trim()},
							{"units", units.ToString()}
						});
					weatherRoot = await weatherService.GetWeather(Location.Trim(), units);

				}
				else
				{
					
					var location = await CrossGeolocator.Current.GetPositionAsync(10000);
					Xamarin.Insights.Track("GetWeather", new Dictionary<string, string>
						{
							{"city", "gps"},
							{"units", units.ToString()}
						});
					weatherRoot = await weatherService.GetWeather(location.Latitude, location.Longitude, units);
				}


				var unit = IsImperial ? "F" : "C";
				

				Temp = String.Format("Temperature: {0}°" +  unit, (int)weatherRoot.MainWeather.Temperature);
				TempHigh = String.Format("High: {0}°" +  unit, (int)weatherRoot.MainWeather.MaxTemperature);
				TempLow = String.Format("Low: {0}°" +  unit, (int)weatherRoot.MainWeather.MinTemperature);

				TempBig = String.Format("{0}°", (int)weatherRoot.MainWeather.Temperature);
				Condition = (weatherRoot.Weather == null || weatherRoot.Weather.Count == 0) ? string.Empty : weatherRoot.Weather[0].Description;


				// TEXT-TO-SPEECH INTEGRATION
				string greeter = "";
				switch(Device.OS)
				{
					case TargetPlatform.Android:
                    greeter = "This is the voice of Android";
						break;
					case TargetPlatform.iOS:
                    greeter = "My name is Siri";
						break;
					case TargetPlatform.Other:
					case TargetPlatform.WinPhone:
                    greeter = "Hi, I'm Cortana";
						break;
				}
				string weatherMessageTemplate = "{0}. The current temperature in {1} is {2}°F, with a high today of {3}° and a low of {4}°.";
				string weatherMessage = string.Format(weatherMessageTemplate, greeter, weatherRoot.Name, (int)weatherRoot.MainWeather.Temperature, (int)weatherRoot.MainWeather.MaxTemperature, (int)weatherRoot.MainWeather.MinTemperature);

				CrossTextToSpeech.Current.Speak(weatherMessage,
					speakRate: Device.OS == TargetPlatform.iOS ? .25f : 1.0f);
			}
			catch (Exception ex)
			{
				Temp = "Unable to get Weather";
				Xamarin.Insights.Report (ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName]string name = "")
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
	}
}
