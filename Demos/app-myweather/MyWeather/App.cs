using MyWeather.View;
using MyWeather.ViewModels;

using Xamarin.Forms;

namespace MyWeather
{
	public class App : Application
	{
		public App()
		{
			var tabs = new TabbedPage
			{
				Title = "My Weather",
				BindingContext = new WeatherViewModel(),
				Children =
				{
					new WeatherView(),
					new ForecastView()
				}
			};

			MainPage = new NavigationPage(tabs)
			{
				BarBackgroundColor = Color.FromHex("3498db"),
				BarTextColor = Color.White
			};
		}
	}
}

