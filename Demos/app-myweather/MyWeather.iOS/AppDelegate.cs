using Foundation;
using UIKit;

namespace MyWeather.iOS
{
	[Register (nameof(AppDelegate))]
	public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
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

            Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageSourceHandler();

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
	}
}