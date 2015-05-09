# Azure Mobile Services

### The Challenge

In this challenge, you will use Azure Mobile Services and Xamarin.iOS to build a scalable, native iOS app in C#.

Login to the Azure portal and create a new Mobile Service and SQL database.  Add the Azure Mobile Services NuGet package to your Xamarin Forms project.    


### Challenge Walkthrough

* Login to your Microsoft Azure account at http://manage.windowsazure.com.  (If you don't have an Azure account, you can obtain a free 30-day trial at http://account.windowsazure.com/signup?WT.mc_id=evolve2014. Once you have an Azure account, you can create up to 10 free mobile services that you can keep even after your trial period ends. If you're an MSDN subscriber, you should activate your Azure benefit and get free credits each month.)

* Click **New** -> **Compute** -> **Mobile Service** -> **Create**.  Then specify URL and database login/password order to create a new Mobile Service and the associated SQL database. Choose **JavaScript** as the backend.

* Select your new mobile service.

* Go to the **Data** tab then click the **Create** button at the bottom to add a new table to your SQL database called **TodoItem**. 

* On the dashboard tab, copy your application URL to a text file.

* Click the **Manage Keys** button at the bottom and copy your Application Key to the text file.

* Fire up Xamarin Studio and select **Blank App (Xamarin.Forms Shared)**.

* Add the NuGet package for Azure Mobile Services (**WindowsAzure.MobileServices**) to *both* the iOS and Android project. To add the package, right click on the project and select **Add** -> **Packages**.

* In the iOS project, add the following code in `AppDelegate.cs` in `FinishedLaunching`, after the call to `Forms.Init()`:

        Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init (); 

* In the shared project, add the following declaration to `App.cs`:

        using Microsoft.WindowsAzure.MobileServices;

* Replace the code in `App.cs` with the following:

        public class App
        {
            public static Page GetMainPage ()
            {             
                return new TodoPage ();
            }
                
            public static MobileServiceClient MobileService = new MobileServiceClient(
                "https://your-mobile-service.azure-mobile.net/",
                "APP KEY"
            );
        }

        public class TodoItem
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public bool Complete { get; set; } 
        }

* Replace the placeholder strings above with the application URL and application key that you copied earlier        

* Add a new file to the shared code project named `TodoPage.cs`, with the following code:

        using System;
        using Xamarin.Forms;
        using System.Threading.Tasks;
        using Microsoft.WindowsAzure.MobileServices;

        public class TodoPage : ContentPage
        {
            async Task Refresh()
            {
                itemsView.ItemsSource = await todoTable
                    .OrderBy (x => x.Text)
                    .Select (x => x.Text)
                    .ToListAsync (); 
            }

            protected async override void OnAppearing ()
            {
                await Refresh ();
                base.OnAppearing ();
            }

            private ListView itemsView;
            private IMobileServiceTable<TodoItem> todoTable;

            public TodoPage()
            {
                todoTable = App.MobileService.GetTable<TodoItem>();

                itemsView = new ListView();

                var textBox = new Entry { HorizontalOptions = LayoutOptions.FillAndExpand };

                var addButton = new Button { Text = "Add" };
                var refreshButton = new Button { Text = "Refresh" };

                this.Content = new StackLayout {
                    Padding = 20,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        new StackLayout {
                            Orientation = StackOrientation.Horizontal,
                            Children = {
                                textBox, addButton, refreshButton
                            }
                        },
                        itemsView
                    }
                };

                // event handlers
                addButton.Clicked += async (sender, e) => {
                    await todoTable.InsertAsync(new TodoItem { Text = textBox.Text, Complete = false });
                    await Refresh();
                    textBox.Text = "";
                };

                refreshButton.Clicked += async (sender, e) => {
                    refreshButton.IsEnabled = false;
                    await Refresh();
                    refreshButton.IsEnabled = true;
                };
            }
        }


* Run either the iOS or Android application and add some todo items

* If you head back to the [Windows Azure Portal](manage.windowsazure.com), you'll see that items you add to the list are now stored in your SQL database.

#####Bonus Challenge #1 Walkthrough

For the bonus challenge, change the app to one that works even when there is no network access, using the offline sync feature of Mobile Services.

* For both the Android and iOS project, add the pre-release package **WindowsAzure.MobileServices.SQLiteStore**. Make sure the checkbox **Show pre-release packages** is checked in the Add Packages dialog.

* Make sure the project builds without errors. (You may have to add the Android Support v13 library binding package for the Android app to compile. Search for **Xamarin.Android.Support.v13** in NuGet and add it to the project.)

* In the iOS project, edit `AppDelegate.cs` and add the following line to `FinishedLaunching`:

            SQLitePCL.CurrentPlatform.Init ();

* In the shared project, add the following declarations to `TodoPage.cs`:

        using Microsoft.WindowsAzure.MobileServices.Sync;
        using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

* Add the following code to the start of `OnAppearing`:

            var store = new MobileServiceSQLiteStore ("localstore.db");
            store.DefineTable<TodoItem> ();

            await App.MobileService.SyncContext.InitializeAsync (store);

* Change the declaration of `todoTable` to the following:

        private IMobileServiceSyncTable<TodoItem> syncTable;

* Change the definition of `Refresh()` to use syncTable:

        async Task Refresh()
        {
            itemsView.ItemsSource = await syncTable
                .OrderBy (x => x.Text)
                .Select (x => x.Text)
                .ToListAsync (); 
        }

* In the `TodoPage` constructor, replace the initialization of `todoTable` with the following:

            syncTable = App.MobileService.GetSyncTable<TodoItem>();

* Change the handler for `addButton` to use `syncTable` instead of `todoTable`:

            await syncTable.InsertAsync(new TodoItem { Text = textBox.Text, Complete = false });

* In the handler for `refreshButton`, add the following lines before the call to `Refresh()`:

            await App.MobileService.SyncContext.PushAsync();
            await syncTable.PullAsync("todoItems", syncTable.CreateQuery());

* Launch the app. You will notice that the list of todo items is now empty, since it is now reading from the local database instead of the remote service. 

* Click the refresh button to synchronize the local data store with the remote one. 

* Close the app. Turn off your network connection or change your mobile service initialization in `App.cs` to use an invalid mobile service name. 

* Re-launch the app. You will see that your list of todo items appears on startup. Add some items and restore your network connection. 

* Click the Refresh button to synchronize changes. Confirm that the changes appear in the Data tab of your mobile service.

Congratulations, you've completed an Azure Bonus Challenge mini-hack!

#####Bonus Challenge #2 Walkthrough

* Create a new mobile service and choose .NET as the backend.

* Go to the quickstart tab in the portal and download the Xamarin.iOS or Xamarin.Android starter project. Or, open Visual Studio 2013 Update 3, and use the Mobile Services new project template to create a mobile service project.

* Open the project in Visual Studio. Press F5 to run the service project locally.

* Click the **Try it out** link in the webpage and test your mobile service by sending HTTP requests.

* In Visual Studio, right click your project and select Deploy. Login to Azure and select your mobile service name.

* In Visual Studio or Xamarin Studio, run the Xamarin.iOS or Xamarin.Android client project and confirm that it connects to your new mobile service.

Congratulations, you've completed an Azure Bonus Challenge mini-hack!
