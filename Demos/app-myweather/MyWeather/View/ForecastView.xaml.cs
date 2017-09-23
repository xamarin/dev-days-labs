﻿using Xamarin.Forms;

namespace MyWeather.View
{
    public partial class ForecastView : ContentPage
    {
        public ForecastView()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                Icon = new FileImageSource { File = "tab2.png" };
        }
    }
}
