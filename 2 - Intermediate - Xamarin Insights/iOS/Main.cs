using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace XamarinInsights.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		//TODO: set api key

		static void Main (string[] args)
		{
			if (string.IsNullOrEmpty (App.InsightsApiKey))
				throw new Exception ("A Xamarin Insights Api key is required");
			Xamarin.Insights.Initialize (App.InsightsApiKey);
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}

