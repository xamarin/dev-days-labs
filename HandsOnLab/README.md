# Xamarin Dev Days Hands On Lab

Today we will build a cloud connected [Xamarin.Forms](https://docs.microsoft.com/xamarin/xamarin-forms?WT.mc_id=devdayslabs-github-bramin) application that will display a list of Xamarin Dev Days speaker. We will start by building the business logic backend that pulls down json-ecoded data from a RESTful endpoint. Then we will connect it to an Azure Mobile App backend in just a few lines of code.

## Mobile App Walkthrough

### 1. Open Solution in Visual Studio

1. Open **Start/DevDaysSpeakers.sln**

This solution contains 4 projects

* DevDaysSpeakers  - Shared Project that will have all shared code (model, views, and view models)
* DevDaysSpeakers.Droid - Xamarin.Android application
* DevDaysSpeakers.iOS - Xamarin.iOS application (requires a Mac)
* DevDaysSpeakers.UWP - Windows 10 UWP application (requires Visual Studio on PC)

![Solution](https://content.screencast.com/users/JamesMontemagno/folders/Jing/media/44f4caa9-efb9-4405-95d4-7341608e1c0a/Portable.png)

The **DevDaysSpeakers** project also has blank code files and XAML pages that we will use during the Hands on Lab.

### 2. NuGet Restore

All projects have the required NuGet packages already installed, so there will be no need to install additional packages during the Hands on Lab. The first thing that we must do is restore all of the NuGet packages from the internet.

1. **Right-click** on the **Solution** and selecting **Restore NuGet packages...**

![Restore NuGets](https://content.screencast.com/users/JamesMontemagno/folders/Jing/media/a31a6bff-b45d-4c60-a602-1359f984e80b/2016-07-11_1328.png)

### 3. Model

We will download details about the speakers.

1. Open `Speaker.cs`
2. In `Speaker.cs`, copy/paste the following properties:

```csharp
public string Id { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public string Website { get; set; }
public string Title { get; set; }
public string Avatar { get; set; }
```

### 4. Implementing INotifyPropertyChanged

*INotifyPropertyChanged* is important for data binding in MVVM Frameworks. This is an interface that, when implemented, lets our view know about changes to the model.

1. In Visual Studio, open `SpeakersViewModel.cs`
2. In `SpeakersViewModel.cs`, implement INotifyPropertyChanged by changing this

```csharp
public class SpeakersViewModel
{

}
```

to this

```csharp
public class SpeakersViewModel : INotifyPropertyChanged
{

}
```

3. In `SpeakersViewModel.cs`, right click on `INotifyPropertyChanged`
4. Implement the `INotifyPropertyChanged` Interface
   - (Visual Studio Mac) In the right-click menu, select Quick Fix -> Implement Interface
   - (Visual Studio PC) In the right-click menu, select Quick Actions and Refactorings -> Implement Interface

5. In `SpeakersViewModel.cs`, ensure this line of code now appears:

```csharp
public event PropertyChangedEventHandler PropertyChanged;
```

6. In `SpeakersViewModel.cs`, create a new method called `OnPropertyChanged`
    - Note: We will call `OnPropertyChanged` whenever a property updates

```csharp
private void OnPropertyChanged([CallerMemberName] string name = null)
{

}
```

7. Add code to `OnPropertyChanged`:

```csharp
private void OnPropertyChanged([CallerMemberName] string name = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
```

### 5. Implementing IsBusy

We will create a backing field and accessors for a boolean property. This property will let our view know that our view model is busy so we don't perform duplicate operations (like allowing the user to refresh the data multiple times).

1. In `SpeakersViewModel.cs`, create the backing field:

```csharp
public class SpeakersViewModel : INotifyPropertyChanged
{
    private bool isBusy;
    //...
}
```

2. Create the property:

```csharp
public class SpeakersViewModel : INotifyPropertyChanged
{
    //...
    public bool IsBusy
    {
        get => isBusy;
        set
        {
            isBusy = value;
            OnPropertyChanged();

            GetSpeakersCommand.ChangeCanExecute();
        }
    }
    //...
}
```

Notice that we call `OnPropertyChanged` when the value changes. The Xamarin.Forms binding infrastructure will subscribe to our **PropertyChanged** event so the UI will be notified of the change.

### 6. Create ObservableCollection of Speaker

We will use an `ObservableCollection<Speaker>` that will be cleared and then loaded with **Speaker** objects. We use an `ObservableCollection` because it has built-in support to raise `CollectionChanged` events when we Add or Remove items from the collection. This means we don't call `OnPropertyChanged` when updating the collection.

1. In `SpeakersViewModel.cs` declare an auto-property which we will initialize to an empty collection

```csharp
public class SpeakersViewModel : INotifyPropertyChanged
{
    //...
    public ObservableCollection<Speaker> Speakers { get; } = new ObservableCollection<Speaker>();
    //...
}
```

### 7. Create GetSpeakers Method

We are ready to create a method named `GetSpeakers` which will retrieve the speaker data from the internet. We will first implement this with a simple HTTP request, and later update it to grab and sync the data from Azure.

1. In `SpeakersViewModel.cs`, create a method named `GetSpeakers` with that returns `async Task`:

```csharp
public class SpeakersViewModel : INotifyPropertyChanged
{
    //...
    private async Task GetSpeakers()
    {

    }
    //...
}
```

2. In `GetSpeakers`, add some scaffolding for try/catch/finally blocks
    - Notice, that we toggle *IsBusy* to true and then false when we start to call to the server and when we finish.

```csharp
private async Task GetSpeakers()
{
    try
    {
        IsBusy = true;
    }
    catch (Exception e)
    {

    }
    finally
    {
       IsBusy = false;
    }

}
```

3. In the `try` block of `GetSpeakers`, create a new instance of `HttpClient`.     - We will use `HttpClient` to get the json-ecoded data from the server

```csharp
private async Task GetSpeakers()
{
    ...
    try
    {
        IsBusy = true;

        using(var client = new HttpClient())
        {
            var json = await client.GetStringAsync("https://demo8598876.mockable.io/speakers");
        }
    }
    ... 
}
```

4. Inside of the `using` we just created, deserialize the json data and turn it into a list of Speakers using Json.NET:

```csharp
private async Task GetSpeakers()
{
    ...
    try
    {
        IsBusy = true;

        using(var client = new HttpClient())
        {
            var json = await client.GetStringAsync("https://demo8598876.mockable.io/speakers");

            var items = JsonConvert.DeserializeObject<List<Speaker>>(json);
        }
    }
    ... 
}
```

5. Inside of the `using`, clear the `Speakers` property and then add the new speaker data:

```csharp
private async Task GetSpeakers()
{
    //...
    try
    {
        IsBusy = true

        using(var client = new HttpClient())
        {
            var json = await client.GetStringAsync("https://demo8598876.mockable.io/speakers");

            var items = JsonConvert.DeserializeObject<List<Speaker>>(json);

            Speakers.Clear();

            foreach (var item in items)
                Speakers.Add(item);
        }
    }
    //...
}
```

6. In `GetSpeakers`, add this code to the `catch` block to display a popup if the data retrieval fails:

```csharp
private async Task GetSpeakers()
{
    //...
    catch(Exception e)
    {
        await Application.Current.MainPage.DisplayAlert("Error!", e.Message, "OK");
    }
    //...
}
```

7. Ensure the completed code looks like this:

```csharp
private async Task GetSpeakers()
{
    try
    {
        using(var client = new HttpClient())
        {
            //GET json from server
            var json = await client.GetStringAsync("https://demo8598876.mockable.io/speakers");

            //Deserialize json
            var items = JsonConvert.DeserializeObject<List<Speaker>>(json);

            //Load speakers into list
            Speakers.Clear();
            foreach (var item in items)
                Speakers.Add(item);
        }
    }
    catch (Exception e)
    {
        await Application.Current.MainPage.DisplayAlert("Error!", e.Message, "OK");
    }
    finally
    {
        IsBusy = false;
    }
}
```

Our method for getting data is now complete.

#### 8. Create GetSpeakers Command

Instead of invoking this method directly, we will expose it with a `Command`. A `Command` has an interface that knows what method to invoke and has an optional way of describing if the Command is enabled.

1. In `SpeakersViewModel.cs`, create a new Command called `GetSpeakersCommand`:

```csharp
public class SpeakersViewModel : INotifyPropertyChanged
{
    //...
    public Command GetSpeakersCommand { get; }
    //...
}
```

2. Inside of the `SpeakersViewModel` constructor, create the `GetSpeakersCommand` and pass it two methods
    - One to invoke when the command is executed
    - Another that determines if the command is enabled. Both methods can be implemented as lambda expressions as shown below:

```csharp
public class SpeakersViewModel : INotifyPropertyChanged
{
    //...
    public SpeakersViewModel()
    {
        GetSpeakersCommand = new Command(async () => await GetSpeakers(),() => !IsBusy);
    }
    //...
}
```

## 9. Build The SpeakersPage User Interface
It is now time to build the Xamarin.Forms user interface in `View/SpeakersPage.xaml`.

1. In `SpeakersPage.xaml`, add a `StackLayout` between the `ContentPage` tags
    - By setting `Spacing="0"`, we're requesting that no space is added between the contents of the `StackLayout`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DevDaysSpeakers.View.SpeakersPage"
             Title="Speakers">

</ContentPage>
```

2. In `SpeakersPage.xaml`, add a ListView that binds to the `Speakers` collection to display all of the items. 
    - We will use `x:Name="ListViewSpeakers"` so that we can access this XAML control from the C# code-behind

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DevDaysSpeakers.View.SpeakersPage"
             Title="Speakers">

    <ListView x:Name="ListViewSpeakers"
              ItemsSource="{Binding Speakers}"
              IsPullToRefreshEnabled="true"
              RefreshCommand="{Binding GetSpeakersCommand}"
              IsRefreshing="{Binding IsBusy}"
              CachingStrategy="RecycleElement">
    <!--Add ItemTemplate Here-->
    </ListView>

</ContentPage>
```

3. In `SpeakersPage.xaml`, add a `ItemTemplate` to describe what each item looks like
    - Xamarin.Forms contains a few default Templates that we can use, and we will use the `ImageCell` that displays an image and two rows of text

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DevDaysSpeakers.View.SpeakersPage"
             Title="Speakers">

    <ListView x:Name="ListViewSpeakers"
              ItemsSource="{Binding Speakers}"
              IsPullToRefreshEnabled="true"
              RefreshCommand="{Binding GetSpeakersCommand}"
              IsRefreshing="{Binding IsBusy}"
              CachingStrategy="RecycleElement">
      <ListView.ItemTemplate>
        <DataTemplate>
          <ImageCell Text="{Binding Name}"
                     Detail="{Binding Title}"
                     ImageSource="{Binding Avatar}"/>
        </DataTemplate>
      </ListView.ItemTemplate>
    </ListView>

</ContentPage>
```

### 10. Connect SpeakersPage with SpeakersViewModel

Because we have bound some elements of the View to ViewModel properties, we have to tell the View with which ViewModel to bind. For this, we have to set the `BindingContext` to the `SpeakersViewModel`.

1. In `SpeakersPage.xaml.cs`, create a field `SpeakersViewModel vm`, initialize `vm` and assign it to the `BindingContext`

```csharp
public partial class SpeakersPage : ContentPage
{
    readonly SpeakersViewModel vm;

    public SpeakersPage()
    {
        InitializeComponent();

        // Create the view model and set as binding context
        vm = new SpeakersViewModel();
        BindingContext = vm;
    }
}
```

2. In `SpeakersPage.xaml.xs`, override `OnAppearing()` by adding the following method which tells the ListView to automatically refresh when the page appears on the screen:

```csharp
protected override void OnAppearing()
{
    base.OnAppearing();

    ListViewSpeakers.BeginRefresh();
}
```

### 11. Run the App

1. In Visual Studio, set the iOS, Android, or UWP project as the startup project 

![Startup project](https://content.screencast.com/users/JamesMontemagno/folders/Jing/media/020972ff-2a81-48f1-bbc7-1e4b89794369/2016-07-11_1442.png)

2. In Visual Studio, click "Start Debugging"
    - If you are having any trouble, see the Setup guides below for your runtime platform

#### iOS Setup
If you are on a Windows PC then you will need to be connected to a macOS build host with the Xamarin tools installed to run and debug the app.

If connected, you will see a Green connection status. Select `iPhoneSimulator` as your target, and then select a Simulator to debug on.

![iOS Setup](https://content.screencast.com/users/JamesMontemagno/folders/Jing/media/a6b32d62-cd3d-41ea-bd16-1bcc1fbe1f9d/2016-07-11_1445.png)

#### Android Setup

Set the DevDaysSpeakers.Droid as the startup project and select a simulator. The first compile may take some additional time as Support Packages may need to be downloaded.

If you run into an issue building the project with an error such as:

**aapt.exe exited with code** or **Unsupported major.minor version 52** then your Java JDK may not be setup correctly, or you have newer build tools installed then what is supported. See this technical bulletin for support: https://releases.xamarin.com/technical-bulletin-android-sdk-build-tools-24/

Additionally, see James' blog for visual reference: https://motzcod.es/post/149717060272/fix-for-unsupported-majorminor-version-520

If you are running into issues with Android support packages that can't be unzipped because of corruption please check: https://xamarinhelp.com/debugging-xamarin-android-build-and-deployment-failures/

#### Windows 10 Setup

Set the DevDaysSpeakers.UWP as the startup project and select debug to **Local Machine**.

### 12. Add Navigation

Now, let's add navigation to a second page that displays speaker details!

1. In `SpeakersPage.xaml.cs`, under `BindingContext = vm;`, add an event to the `ListViewSpeakers` to get notified when an item is selected:

```csharp
public partial class SpeakersPage : ContentPage
{
    readonly SpeakersViewModel vm;

    public SpeakersPage()
    {
        InitializeComponent();

        // Create the view model and set as binding context
        vm = new SpeakersViewModel();
        BindingContext = vm;

        ListViewSpeakers.ItemSelected += ListViewSpeakers_ItemSelected;
    }
}
```

2. In `SpeakersPage.xaml.cs`, create a method called `ListViewSpeakers_ItemSelected`:
    - This code checks to see if the selected item is non-null and then use the built in `Navigation` API to push a new page and deselect the item.

```csharp
private async void ListViewSpeakers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
{
    if (e.SelectedItem is Speaker speaker)
    {
        await Navigation.PushAsync(new DetailsPage(speaker));
        ListViewSpeakers.SelectedItem = null;
    }
}
```

### 13. Create DetailsPage.xaml UI

Let's add UI to the DetailsPage. Similar to the SpeakersPage, we will use a StackLayout, but we will wrap it in a ScrollView. This allows the user to scroll if the page content is longer than the available screen space.

1. In `DetailsPage.xaml`, add a `ScrollView` and a `StackLayout`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DevDaysSpeakers.View.DetailsPage"
             Title="Details">
    <ScrollView Padding="10">
        <StackLayout Spacing="10">
            <!-- Detail controls here -->
        </StackLayout>
    </ScrollView>
</ContentPage>
```

2.  In `DetailsPage.xaml`, add controls and bindings for the properties in the Speaker class:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DevDaysSpeakers.View.DetailsPage"
             Title="Details">

    <ScrollView Padding="10">
        <StackLayout Spacing="10">
            <Image Source="{Binding Avatar}" HeightRequest="200" WidthRequest="200"/>

            <Label Text="{Binding Name}" FontSize="24"/>
            <Label Text="{Binding Title}" TextColor="Purple"/>
            <Label Text="{Binding Description}"/>
        </StackLayout>
    </ScrollView>
</ContentPage>
```

3. In `DetailsPage.xaml`, add two buttons and give them names so we can access them in the code-behind.
     - We'll be adding click handlers to each button.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DevDaysSpeakers.View.DetailsPage"
             Title="Details">

    <ScrollView Padding="10">
        <StackLayout Spacing="10">
            <Image Source="{Binding Avatar}" HeightRequest="200" WidthRequest="200"/>

            <Label Text="{Binding Name}" FontSize="24"/>
            <Label Text="{Binding Title}" TextColor="Purple"/>
            <Label Text="{Binding Description}"/>

            <Button Text="Speak" x:Name="ButtonSpeak"/>
            <Button Text="Go to Website" x:Name="ButtonWebsite"/>
        </StackLayout>
    </ScrollView>
</ContentPage>
```

### 14. Add Text to Speech

If we open up `DetailsPage.xaml.cs` we can now add a few more click handlers. Let's start with ButtonSpeak, where we will use the [Text To Speech Plugin](https://github.com/jamesmontemagno/TextToSpeechPlugin) to read back the speaker's description.

1. In `DetailsPage.xaml.cs`, in the constructor, add a clicked handler below the BindingContext

```csharp
public partial class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker speaker)
    {
        InitializeComponent();

        //Set local instance of speaker and set BindingContext
        speaker = speaker;
        BindingContext = speaker;

        ButtonSpeak.Clicked += ButtonSpeak_Clicked;
    }
}
```

2. In `DetailsPage.xaml.cs`, create the `ButtonSpeak_Clicked` method which will call the cross-platform API for text to speech

```csharp
public partial class DetailsPage : ContentPage
{
    //...
    private async void ButtonSpeak_Clicked(object sender, EventArgs e)
    {
        await TextToSpeech.SpeakAsync(speaker.Description);
    }
}
```

### 15. Add Open Website Functionality
Xamarin.Forms includes many APIs for performing common tasks such as opening a URL in the default browser.

1. In `DetailsPage.xaml.cs`, add a clicked handler for `ButtonWebsite.Clicked`:

```csharp
public partial class DetailsPage : ContentPage
{
    //...
    public DetailsPage(Speaker speaker)
    {
        InitializeComponent();

        //Set local instance of speaker and set BindingContext
        speaker = speaker;
        BindingContext = speaker;

        ButtonSpeak.Clicked += ButtonSpeak_Clicked;
        ButtonWebsite.Clicked += ButtonWebsite_Clicked;
    }
    //...
}
```

2. In `DetailsPage.xaml.cs`, create the `ButtonSpeak_Clicked` method which will use the static class `Device` to call the `OpenUri` method

```csharp
public partial class DetailsPage : ContentPage
{
    //...
    private async void ButtonWebsite_Clicked(object sender, EventArgs e)
    {
        if (speaker.Website.StartsWith("https"))
            await Browser.OpenAsync(speaker.Website);
    }
}
```

### 16. Compile & Run
Now, we should be all set to compile and run our application!

## Azure Backend Walkthrough

Being able to grab data from a RESTful end point is great, but what about creating the back-end service? This is where Azure Mobile Apps comes in. Let's update our application to use an Azure Mobile Apps back-end.

### 1. Create Azure Mobile App

1. (If you don't yet have an Azure account) Create a Free Azure account including a free $200 credit by navigating to [this Azure Sign Up Page](https://azure.microsoft.com/free/services/mobile-apps?WT.mc_id=devdayslab-github-bramin) and creating an account

2. In the [Azure Portal](https://portal.azure.com?WT.mc_id=devdayslabs-github-bramin), select the **Create a resource** button
3. In **New** window, tap **Mobile**
4. In **New** window, tap **Mobile App**

![Create Resource](https://user-images.githubusercontent.com/13558917/40452936-6032bcf2-5e98-11e8-991d-8bca36d61bf1.png)

5. In the **Mobile App** window, enter your **App name**
    - This is a unique name for the app that you will need when connecting your Xamarin.Forms client app to the hosted Azure Mobile App
    - You will need to choose a globally-unique name
    - I recommend using `[Your Last Name]speakers`

6. In the **Mobile App** window, select your **Subscription**
    - Select a subscription or create a pay-as-you-go account
        - We'll be using the free tier of Azure Mobile App and it will not cost anything

7. In the **Mobile App** window, create a new **Resource Group**
    - Select *Create new* and call it **DevDaysSpeakers**
    - A resource group is just a folder that holds multiple Azure services

8. In the **Mobile App** window, select **App Service plan/Location**
9. In the **New App Service Plan** window, enter a unique name
    - I recommend `[Your Last Name]speakersserver`
10. In the **New App Service Plan** window, select a location (typically you would choose a location close to your customers)
11. In the **New App Service Plan** window, select **Pricing tier**
12. In the **Pricing Tier** window, select **Dev/Test**
13. In the **Pricing Tier** window, select **Free**
14. In the **Pricing Tier** window, select **Apply**
15. In the **New App Service Plan** window, select **OK**
16. In the **Mobile App** window, click Create

![Create Mobile App](https://user-images.githubusercontent.com/13558917/40457467-6c74d398-5eab-11e8-8fe4-bf8b6669a64d.png)

After clicking **Create**, it will take Azure about 3-5 minutes to create the new service, so let's head back to the code!

### 2. Update AzureService.cs

We will use the [Azure Mobile Apps SDK](https://azure.microsoft.com/documentation/articles/app-service-mobile-xamarin-forms-get-started/?none-XamarinWorkshop-bramin) to connect our mobile app to our Azure back-end with just a few lines of code.

1. In `AzureService.cs`, add your url to the Initialize method:
    - Be sure to update YOUR-APP-NAME-HERE with the app name you specified when creating your Azure Mobile App.
    - My appUrl is "https://minnickspeakers.azurewebsites.net"

```csharp
var appUrl = "https://YOUR-APP-NAME-HERE.azurewebsites.net";
```

The logic in the `Initialize` method will setup our database and create our `IMobileServiceSyncTable<Speaker>` table that we can use to retrieve speaker data from the Azure Mobile App. There are two methods that we need to fill in to get and sync data from the server.


2. In `AzureService.cs`, update the `GetSpeakers` method to initialize, sync, and query the table for items using LINQ queries to order the results:

```csharp
public async Task<IEnumerable<Speaker>> GetSpeakers()
{
    await Initialize();
    await SyncSpeakers();
    return await table.OrderBy(s => s.Name).ToEnumerableAsync();
}
```

3. In `AzureService.cs`, update the `SyncSpeakers` method to sync the local database our in our app with our remote database in Azure:

```csharp
public async Task SyncSpeakers()
{
    try
    {
        await Client.SyncContext.PushAsync();
        await table.PullAsync("allSpeakers", table.CreateQuery());
    }
    catch (Exception ex)
    {
        Debug.WriteLine("Unable to sync speakers, that is alright as we have offline capabilities: " + ex);
    }
}
```

That is it for our Azure code! Just a few lines, and we are ready to pull the data from Azure.

### 3. Update SpeakersViewModel.cs

1. In `SpeakersViewModel.cs`, update `GetSpeakers` to use the Azure Service by amending the code in the `try` block:

```csharp
private async Task GetSpeakers()
{
    //...
    try
    {
        IsBusy = true;

        var service = DependencyService.Get<AzureService>();
        var items = await service.GetSpeakers();

        Speakers.Clear();
        foreach (var item in items)
            Speakers.Add(item);
    }
    //...
}
```

Now, we have implemented the code we need in our app. `AzureService` will automatically handle all communication with your Azure back-end for you including online/offline synchronization so your app works even when it's not connected to the internet.

### 3. Populate Azure Database

Let's head back to the Azure Portal and populate the database!

1. In the [Azure Portal Dashboard](https://portal.azure.com?WT.mc_id=devdayslabs-github-bramin), click on the notification button (bell icon)

1. In the **Notifiations** window, click **Go to resource**

![Select Azure Mobile App](https://user-images.githubusercontent.com/13558917/59301196-ec80b200-8c45-11e9-9631-83da7ee355ce.png)

3. On the left-hand menu, select **Quickstart**

4. In the new window, select **Xamarin.Forms**

![Xamarin Forms Quick Start](https://user-images.githubusercontent.com/13558917/40458465-f9bf6362-5eb0-11e8-8520-4159ee8f22b3.png)

5. In the **Quick Start** menu, select the box below **Connect a database**
6. In the **Data Connections** window, select **+ Add**
7. In the **Add data connection** window, select the **SQL Database** box
8. In the **Database** window, select **Create a new database**

![Create Database](https://user-images.githubusercontent.com/13558917/40458584-886e203a-5eb1-11e8-9185-8a0e959e20f9.png)

8. In the **SQL Database** window, enter a name
    - The name must be unique
    - I recommend using *LastnameSpeakersDatabase*
10. In the **SQL Database** window, select **Target Server** *Configure required settings*
11. In the **Server** window, select **Create a new server**
12. In the **New server** window, enter a Server name
  - The server name must be unique and all lower-case
    - I recommend using *lastnamespeakerserver*
13. In the **New server** window, create a **Server admin login**
    - This will be your username for accessing the database remotely (which we won't be doing in this lab)
14. In the **New server** window, create a **Password**
    - This will be your password for accessing the database remotely (which we won't be doing in this lab)
15. In the **New server** window, **Confirm Password**
16. In the **New server** window, select a **Location**
    - This is the physical location where your server will be located
    - I recommend selecting a location that is closest to your users
17. In the **New server** window, select **Select**

![Configure Database Server](https://user-images.githubusercontent.com/13558917/40458706-4797a8c8-5eb2-11e8-9c83-af6f4d9a5cca.png)

18. In the **SQL Database** window, select **Pricing Tier**
19. In the **Configure** window, select **Free**
20. In the **Configure** window, select **Apply**

![Database Pricing Tier](https://user-images.githubusercontent.com/13558917/40458930-84e5ecac-5eb3-11e8-82a1-75b958936bcf.png)

21. In the **SQL Database** window, select **Select**

![Select SQL Database](https://user-images.githubusercontent.com/13558917/40459019-e2aa3d3e-5eb3-11e8-9dc6-258f8871db40.png)

22. In the **Add data connection** window, select **Connection String**
23. In the **Connection string** window, select **OK**, leaving the default value
24. In the **Add data connection** window, select **OK**

![Connection String](https://user-images.githubusercontent.com/13558917/40459075-43d05a3a-5eb4-11e8-8b47-6971c22176b2.png)

25. Standby while Azure creates the Data Connection
    - This may take 3-5 minutes

![Data Connection Create](https://user-images.githubusercontent.com/13558917/40459143-a5437ffe-5eb4-11e8-8558-5acd9dc0e6a5.png)

![Data Connection Completed](https://user-images.githubusercontent.com/13558917/40459358-cdf13eb8-5eb5-11e8-9f1f-3f161d4d3eed.png)

Our database is now created! Let's populate it with some data!

### 4. Populate Database with Data

1. In the [Azure Portal Dashboard](https://portal.azure.com?WT.mc_id=devdayslabs-github-bramin), click on the  **Mobile App**

![Select Azure Mobile App](https://user-images.githubusercontent.com/13558917/40458389-9a48ad9e-5eb0-11e8-9378-4464d4381958.png)

2. In the **Mobile App** menu, enter **easy** into the search bar
3. In the **Mobile App** menu, select **Easy tables**

![Easy Tables](https://user-images.githubusercontent.com/13558917/40459471-6874bf96-5eb6-11e8-8e29-edb5ef08b9f8.png)

4. In the **Easy tables** window, select **Click here to continue**
5. In the new **Easy tables** window, check the box **I acknowledge that this will overwrite all site contents.**
6. In the new **Easy tables** window, select **Create TodoItem table**
    - Ignore that it says "TodoItem Table"; selecting **Create** will create an empty table
![Initialize Easy Tables](https://user-images.githubusercontent.com/13558917/40459585-0f9d2d8a-5eb7-11e8-8bed-6480ab69862c.png)

7. In the **Easy tables** window, select **Add from CSV**

![Add From CSV](https://user-images.githubusercontent.com/13558917/40459674-92841420-5eb7-11e8-80d3-618ccc630882.png)

8. In the **Add from CSV** window, select the **Folder Icon**
9. In the file browser, locate and select the [**Speakers.csv** file](https://github.com/brminnick/dev-days-labs/blob/master/HandsOnLab/Speaker.csv) in the HandsOnLabs folder
10. In the **Add from CSV** window, after uploading the CSV file, select the blank white button at the bottom
    - This is the save button; it's blank because of a bug
    - Note: If you get an error while uploading the Speaker.CSV file, it may be a bug that has been resolved. To workaround this, go to the "Application settings" under the "Settings" section and scroll to "App Settings". Change the value for MobileAppsManagement_EXTENSION_VERSION to 1.0.367 and save the changes. Now retry the "Add from CSV" process again

![Upload CSV](https://user-images.githubusercontent.com/13558917/40459751-fc8e29f0-5eb7-11e8-9534-4ad6f276c003.png)

![application settings fix](appsettingsfix.png)

11. Re-run your application and get data from Azure!
