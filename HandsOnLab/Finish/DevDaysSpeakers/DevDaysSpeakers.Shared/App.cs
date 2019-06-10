using DevDaysSpeakers.View;

using Xamarin.Forms;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
namespace DevDaysSpeakers
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            var content = new SpeakersPage();

            MainPage = new NavigationPage(content);
        }
      
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
