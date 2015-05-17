using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MyWeather.WinPhone
{
	public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();
			SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
			Xamarin.Insights.Initialize("dbf5b73b9b8dc2b52514eb4d62edb05b63a6c8b5");
			Xamarin.Insights.ForceDataTransmission = true;
			
			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new MyWeather.App());
		}
	}
}
