# Lab: Xamarin Android Player

![Xamarin Android Player](Images/player.png)

### Challenge

Test out the brand new high performance Xamarin Android Player for Mac or PC.

### Walkthrough 

* Download Xamarin Android Player for Mac or PC from [xamarin.com/android-player](http://xamarin.com/android-player)

* Launch Xamarin Android Player

* Open AndroidPlayerMiniHack.sln in Visual Studio or Xamarin Studio

* Select Xamarin Android Player as Target

![Xamarin Android Player Select](Images/SelectXamarinAndroidPlayer.png)

* Ensure NuGet Packages are installed (Right Click on Solution and Restore Packages in Xamarin Studio or Select Manage NuGet Packages in Visual Studio)

* Debug application

* Click Xamarin Android Player Logo

* Click Battery Tab

![Change Battery](https://cloud.githubusercontent.com/assets/8173345/4462520/54e19790-48c3-11e4-86d7-5d2c98627b72.png)

* Change Battery Level

* Select Refresh Battery


# Bonus: Get GPS Location

1. Add NuGet Package:  “Geolocator Plugin for Xamarin and Windows” ![](Images/nuget.PNG)

2. Add Geolocator Permissions: You must request `ACCESS_COARSE_LOCATION` & `ACCESS_FINE_LOCATION` permission. These can be added under the project settings.

3. Add new TextView and Button in Resources/layout/main.xml

4. Find Views and add click handler to button

5. Get location and update view with Geolocator Plugin at the end of OnCreate() in MainActivity.cs
