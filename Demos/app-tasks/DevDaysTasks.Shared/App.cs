using System;

using Xamarin.Forms;

namespace DevDaysTasks
{
	public class App : Application
	{
		public App ()
		{
            // The root page of your application
            MainPage = new NavigationPage(new TodoList())
            {
                BarBackgroundColor = Color.FromHex("#5ABAFF"),
                BarTextColor = Color.White
            };

		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

