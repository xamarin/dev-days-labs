﻿using Xamarin.Forms;

namespace MyWeather.View
{
    public partial class WeatherView : ContentPage
    {
        public WeatherView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                Icon = new FileImageSource { File = "tab1.png" };
        }
    }
}
