using Android.App;
using Android.Content.PM;
using Android.OS;

using Plugin.CurrentActivity;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace MyWeather.Droid
{
	[Activity (Label = "My Weather", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : FormsAppCompatActivity
    {
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

		protected override void OnCreate (Bundle savedInstanceState)
		{
		    ToolbarResource = Resource.Layout.toolbar;
		    TabLayoutResource = Resource.Layout.tabs;

		    base.OnCreate (savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);


            Forms.Init(this, savedInstanceState);
			CrossCurrentActivity.Current.Init(this, savedInstanceState);

		    LoadApplication(new App());
		}
    }
}


