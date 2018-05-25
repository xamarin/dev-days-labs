using Android.App;
using Android.Content.PM;
using Android.OS;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace DevDaysTasks.Droid
{
	[Activity (Label = "Tasks", 
		Icon = "@drawable/icon", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
    {
		protected override void OnCreate (Bundle savedInstanceState)
		{
            ToolbarResource = Resource.Layout.toolbar;
            TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate (savedInstanceState);
            
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            
            Forms.Init (this, savedInstanceState);

			LoadApplication (new App ());
		}
	}
}

