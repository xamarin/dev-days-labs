using MyWeather.Views;
using MyWeather.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace MyWeather
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            FFImageLoading.ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration
            {
                HttpHeadersTimeout = 60
            });

            var tabbedPage = new Xamarin.Forms.TabbedPage
            {
                Title = "My Weather",
                BindingContext = new WeatherViewModel(),
                Children =
                {
                    new WeatherView(),
                    new ForecastView()
                }
            };
            tabbedPage.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            if (Device.RuntimePlatform is Device.Android)
                tabbedPage.BarBackgroundColor = Color.FromHex("1FAECE");

            var navigationPage = new Xamarin.Forms.NavigationPage(tabbedPage)
            {
                BarBackgroundColor = Color.FromHex("1FAECE"),
                BarTextColor = Color.White
            };
            navigationPage.On<iOS>().SetPrefersLargeTitles(true);

            MainPage = navigationPage;
        }
    }
}