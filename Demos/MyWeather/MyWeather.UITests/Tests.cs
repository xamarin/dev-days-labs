using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Threading;

namespace MyWeather.UITests
{
	[TestFixture (Platform.Android)]
	[TestFixture (Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		public Tests (Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest ()
		{
			app = AppInitializer.StartApp (platform);
		}

		[Test]
		public void A_AppLaunches ()
		{
			app.Screenshot ("First screen.");
			app.Repl ();
		}

		[Test]
		public void B_GetWeatherSuccess()
		{
			app.Screenshot ("First screen.");
			app.ClearText(x => x.Marked("EntryCity"));
			app.Screenshot ("Screen Cleared");
			app.EnterText(x => x.Marked("EntryCity"), "Cleveland,OH");
			app.Screenshot ("Enter Cleveland");
			app.Tap (x => x.Marked ("LabelUseCity"));//dismiss keyboard
			app.Tap(x => x.Marked("ButtonGetWeather"));
			app.Screenshot ("Click Get Weather");
			Thread.Sleep (100);
			app.Screenshot ("Is Loading");
			app.WaitForNoElement (x => x.Marked ("IsBusyIndicator"));
			app.Screenshot ("Done Loading");
			app.WaitForElement (x => x.Marked ("LabelBigTemp"));
			app.Screenshot ("Display Weather");
			var results = app.Query (x => x.Marked ("LabelBigTemp"));
			int test = 0;
			var passed = int.TryParse (results [0].Text.Replace("°", ""), out test);
			Assert.IsTrue (passed, "Didn't Display Result");
		}



		[Test]
		public void C_EntryCityDisablesWhenSwitchedOff()
		{
			app.Screenshot ("First screen.");
			app.Tap(x => x.Marked("SwitchUseCity"));
			app.Screenshot ("Switch off use city");
			var results = app.Query(x => x.Marked("EntryCity"));
			Assert.IsFalse (results [0].Enabled, "Entry was not disabled when toggled off");
		}

		[Test]
		public void D_GetWeatherButtonDisabledNoCitySpecified()
		{
			app.Screenshot ("First screen.");
			app.ClearText(x => x.Marked("EntryCity"));
			app.Screenshot ("Screen Cleared");
			var results = app.Query(x => x.Marked("ButtonGetWeather"));
			Assert.IsFalse (results [0].Enabled, "Button was not disabled when city was cleared");
		}
	}
}

