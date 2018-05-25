using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace MyWeather.iOS
{
	[Register (nameof(AppDelegate))]
	public class AppDelegate : FormsApplicationDelegate
    {
		
		public override bool FinishedLaunching (UIApplication uiApplication, NSDictionary launchOptions)
		{
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(43, 132, 211); //bar background
            UINavigationBar.Appearance.TintColor = UIColor.White; //Tint color of button items
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                Font = UIFont.FromName("HelveticaNeue-Light", 20f),
                TextColor = UIColor.White
            });

            Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
	}
}