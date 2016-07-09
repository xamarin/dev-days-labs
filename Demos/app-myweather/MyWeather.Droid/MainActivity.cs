using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Plugin.Permissions;
using Android.Content.PM;

namespace MyWeather.Droid
{
	[Activity (Label = "My Weather", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : FormsAppCompatActivity
    {

		protected override void OnCreate (Bundle bundle)
		{
			
		    ToolbarResource = Resource.Layout.toolbar;
		    TabLayoutResource = Resource.Layout.tabs;
		    base.OnCreate (bundle);

		    Forms.Init(this, bundle);
		    LoadApplication(new App());
		}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}


