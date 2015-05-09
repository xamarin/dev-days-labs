using System;
using Xamarin.Forms;

namespace XamarinInsights
{
	public class App
	{
		public const string InsightsApiKey = "";
		public static Page GetMainPage ()
		{	
			return new UserInformationPage ();
		}
		public static Action<Action> RunOnMainThread;
	}
}

