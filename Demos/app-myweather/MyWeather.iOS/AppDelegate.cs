using Foundation;
using UIKit;

namespace MyWeather.iOS
{
	[Register (nameof(AppDelegate))]
	public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
		public override bool FinishedLaunching (UIApplication uiApplication, NSDictionary launchOptions)
		{
            Xamarin.Forms.Forms.Init();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageSourceHandler();

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
	}
}