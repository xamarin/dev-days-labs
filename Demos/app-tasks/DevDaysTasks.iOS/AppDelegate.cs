using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;

namespace DevDaysTasks.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
            // Initialize Azure Mobile App for the current platform
            CurrentPlatform.Init();

            // Initialize and load the Xamarin.Forms application
            global::Xamarin.Forms.Forms.Init();            
            LoadApplication (new App());

			return base.FinishedLaunching (app, options);
		}
	}
}

