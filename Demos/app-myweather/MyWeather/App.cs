using MyWeather.View;
using MyWeather.ViewModels;
using Xamarin.Forms;
using static System.Diagnostics.Debug;

namespace MyWeather
{
    public class App : Application
    {
        public App()
        {
            var tabs = new TabbedPage
            {
                Title ="My Weather",
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

        protected override void OnStart()
        {
            base.OnStart();
            WriteLine("Application OnStart");
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            WriteLine("Application OnSleep");
        }

        protected override void OnResume()
        {
            base.OnResume();
            WriteLine("Application OnResume");
        }
    }
}

