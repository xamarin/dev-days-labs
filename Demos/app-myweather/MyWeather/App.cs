using MyWeather.View;
using MyWeather.ViewModels;

using Xamarin.Forms;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
namespace MyWeather
{
    public class App : Application
    {
        public App()
        {
            var tabbedPage = new TabbedPage
            {
                Title = "My Weather",
                BindingContext = new WeatherViewModel(),
                Children =
                {
                    new WeatherView(),
                    new ForecastView()
                }
            };

            if (Device.RuntimePlatform is Device.Android)
                tabbedPage.BarBackgroundColor = Color.FromHex("1FAECE");

            MainPage = new NavigationPage(tabbedPage)
            {
                BarBackgroundColor = Color.FromHex("1FAECE"),
                BarTextColor = Color.White
            };
        }
    }
}

