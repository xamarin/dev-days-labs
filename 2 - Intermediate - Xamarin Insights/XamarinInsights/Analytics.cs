using System;
using Xamarin;

namespace XamarinInsights
{
	public static class Analytics
	{
		public static void UserAuthenticated(User user)
		{
			//TODO: Xamarin.Insights.Identify()
			//The UID can be any identifier you use to identify a user. Email can be a good option
			//The Table parameter allows you to send any other traits about your users.
			//Check Insights.Traits for some reccomended/supported traits.
			//Feel free to track any additional data you need to about your user
			throw new NotImplementedException ();
		}

		public static void LogKeyPress(string key)
		{
			//TODO:Xamarin.Insights.Track();
			//Track using the identifier KeyPress
			//Pass in the actual key press by using the dictionary with a key of "Key"
			throw new NotImplementedException ();
		}

		public static void LogPageView(string pageView)
		{
			//TODO:Xamarin.Insights.Track();
			throw new NotImplementedException ();
		}
		public static void LogNewLevel(int level,string currentCombination, double timeRemaining)
		{
			//TODO:Xamarin.Insights.Track();
			//Pass in the other parameters using the dictionary
			throw new NotImplementedException ();
		}
	}
}

