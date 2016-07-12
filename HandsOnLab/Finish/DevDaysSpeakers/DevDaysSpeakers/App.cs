using DevDaysSpeakers.View;
using AppServiceHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using AppServiceHelpers.Abstractions;

namespace DevDaysSpeakers
{
    public class App : Application
    {
        public static IEasyMobileServiceClient AzureClient { get; set; }
        public App()
        {
            AzureClient = EasyMobileServiceClient.Create();
            AzureClient.Initialize("https://montemagnospeakers.azurewebsites.net");
            AzureClient.RegisterTable<Model.Speaker>();
            AzureClient.FinalizeSchema();

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
