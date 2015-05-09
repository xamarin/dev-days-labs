Xamarin.Forms-TripExpenses-TechEd
==========================

Xamarin.Forms Azure Demo of a simple expense app with Azure Mobile Services online and offline sync with a backed SQLite database for iOS, Android, and Windows Phone.

##Setup

* Signup for an Azure Mobile Services account: http://azure.microsoft.com/en-us/services/mobile-services/
* Create a new Azure Mobile Services Table Called "TripExpense"
* Open "DataStore.cs" in TripExpenses shared project
* Edit: MobileService = new MobileServiceClient(
        "https://"+"PUT-SITE-HERE" +".azure-mobile.net/",
        "PUT-YOUR-API-KEY-HERE");


##Watch

* Full Session on Channel 9: http://channel9.msdn.com/Events/dotnetConf/2014/Developing-Native-iOS-Android-and-Windows-Apps-with-Xamarin
* Slides available: http://www.slideshare.net/JamesMontemagno/dotnetconf-introduction-to-xamarin-and-xamarinforms