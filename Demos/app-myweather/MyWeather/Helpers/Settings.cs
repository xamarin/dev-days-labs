// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MyWeather.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string IsImperialKey = "is_imperial";
        private static readonly bool IsImperialDefault = true;


        private const string UseCityKey = "use_city";
        private static readonly bool UseCityDefault = true;


        private const string CityKey = "city";
        private static readonly string CityDefault = "Seattle,WA";

        #endregion


        public static bool IsImperial
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsImperialKey, IsImperialDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsImperialKey, value);
            }
        }


        public static bool UseCity
        {
            get
            {
                return AppSettings.GetValueOrDefault(UseCityKey, UseCityDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UseCityKey, value);
            }
        }

        public static string City
        {
            get
            {
                return AppSettings.GetValueOrDefault(CityKey, CityDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CityKey, value);
            }
        }

    }
}