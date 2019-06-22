using Xamarin.Forms;

namespace MyWeather.View
{
    public partial class WeatherView : ContentPage
    {
        public WeatherView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform.Equals(Device.iOS))
                IconImageSource = "tab1.png";
        }
    }
}
