using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TripExpenses.Views;
using Xamarin.Forms;

namespace TripExpenses
{
	
	public class App : Application
	{
		public App()
		{
			// The root page of your application
			MainPage = new NavigationPage(new ExpenseListPage());
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
