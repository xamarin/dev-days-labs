MyWeather - Dev Days Demo
=========
* Author: James Montemagno, Developer Evangelist, Xamarin
* Blog: http://www.motzcod.es
* Twitter: [@JamesMontemagno](http://www.twitter.com/jamesmontemagno)

What is MyWeather?
-------------------
MyWeather is a cross-platform weather application built on top of Xamarin.Forms to target iOS, Android, and Windows Phone. Demonstraights how to use Xamarin.Forms to share all of your UI code and common business logic across all platforms.


The demo app allow the user specify a location (city, state) to pull weather info at that location using [OpenWeatherMap](http://www.openweathermap.com). Additionally, the user can specify units and if they want to use their geolocation.

Features & Implementations
-----------------------------
- The UI was designed using XAML and Data Binding.
- Uses HttpClient to call OpenWeatherMap REST service.
- JSON data deserialization with Newtonsoft JSON.NET.
- Text-to-Speech integration the Text To Speech Plugin.
- Persitant Settings with the Settings Plugin.
- Geolocation across the different platforms with the Gelocator Plugin.
- Platform differentiation in UI via <OnPlatform> XAML elements in Xamarin.Forms.

Requirements
------------
This demo project requires at minimum Xamarin.iOS and Xamarin.Android Indie Edition (or trial). To open the project in Visual Studio instead of Xamarin Studio, you will need Xamarin.iOS and Xamarin.Android Business Editions (or trial).

For more info visit www.Xamarin.com.
