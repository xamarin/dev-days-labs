using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;

namespace DevDaysTasks.Droid
{
	[Activity (Label = "Tasks", 
		Icon = "@drawable/icon", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
    {
		protected override void OnCreate (Bundle bundle)
		{
            ToolbarResource = Resource.Layout.toolbar;
            TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate (bundle);

            // Initialize Azure Mobile App for the current platform
            CurrentPlatform.Init();

            // Initialize and load the Xamarin.Forms application
            Forms.Init (this, bundle);
			LoadApplication (new App ());
		}
	}
}

