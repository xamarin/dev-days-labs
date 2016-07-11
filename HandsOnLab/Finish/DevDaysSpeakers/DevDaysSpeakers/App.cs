using DevDaysSpeakers.View;
using AppServiceHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DevDaysSpeakers
{
    public class App : Application
    {
        public App()
        {
            var client = EasyMobileServiceClient.Create();
            client.Initialize("https://montemagnospeakers.azurewebsites.net");
            client.RegisterTable<Model.Speaker>();
            client.FinalizeSchema();

            // The root page of your application
            var content = new SpeakersPage(client);

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
