using Xamarin.Essentials;

namespace MyWeather.Helpers
{
	public static class Settings
	{
		const string isImperialKey = nameof(isImperialKey);
		static readonly bool isImperialDefault = true;

		const string useCityKey = nameof(useCityKey);
		static readonly bool useCityDefault = true;

		const string cityKey = nameof(cityKey);
		static readonly string cityDefault = "Seattle,USA";

		public static bool IsImperial
		{
			get { return Preferences.Get(isImperialKey, isImperialDefault); }
            set { Preferences.Set(isImperialKey, value); }
		}

		public static bool UseCity
		{
            get { return Preferences.Get(useCityKey, useCityDefault); }
            set { Preferences.Set(useCityKey, value); }
		}

		public static string City
		{
            get { return Preferences.Get(cityKey, cityDefault); }
            set { Preferences.Set(cityKey, value); }
		}
	}
}