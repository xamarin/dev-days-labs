using Xamarin.Essentials;

namespace MyWeather.Services
{
    public static class Settings
    {
        public static bool IsImperial
        {
            get => Preferences.Get(nameof(IsImperial), true);
            set => Preferences.Set(nameof(IsImperial), value);
        }

        public static bool UseCity
        {
            get => Preferences.Get(nameof(UseCity), true);
            set => Preferences.Set(nameof(UseCity), value);
        }

        public static string City
        {
            get => Preferences.Get(nameof(City), "Seattle,USA");
            set => Preferences.Set(nameof(City), value);
        }
    }
}