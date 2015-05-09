using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;


namespace XamarinInsights.Android
{
	[Activity (Label = "XamarinInsights.Android.Android", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : AndroidActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			if (string.IsNullOrEmpty (App.InsightsApiKey))
				throw new Exception ("A Xamarin Insights Api key is required");
			Xamarin.Insights.Initialize (App.InsightsApiKey,this);
			Xamarin.Forms.Forms.Init (this, bundle);

			App.RunOnMainThread = (a) => this.RunOnUiThread (a);
			SetPage (App.GetMainPage ());
		}
	}
}

