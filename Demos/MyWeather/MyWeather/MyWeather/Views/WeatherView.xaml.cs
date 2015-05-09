using MyWeather.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyWeather.Views
{
	public partial class WeatherView : ContentPage
	{
		public WeatherView()
		{
			InitializeComponent();
			BindingContext = new WeatherViewModel();
		}
	}
}
