## Xamarin Dev Days Hands On Lab

Today we will build a cloud connected [Xamarin.Forms](http://xamarin.com/forms) application that will display a list of Xamarin Dev Days speaker. We will start by building the business logic backend that pulls down json-ecoded data from a RESTful endpoint. Then we will connect it to an Azure Mobile App backend in just a few lines of code.


### Get Started

Open **Start/DevDaysSpeakers.sln**

This solution contains 4 projects

* DevDaysSpeakers  - Shared Project that will have all shared code (model, views, and view models)
* DevDaysSpeakers.Droid - Xamarin.Android application
* DevDaysSpeakers.iOS - Xamarin.iOS application (requires a macOS build host)
* DevDaysSpeakers.UWP - Windows 10 UWP application (requires Visual Studio 2015/2017 on Windows 10)

![Solution](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/44f4caa9-efb9-4405-95d4-7341608e1c0a/Portable.png)

The **DevDaysSpeakers** project also has blank code files and XAML pages that we will use during the Hands on Lab.

#### NuGet Restore

All projects have the required NuGet packages already installed, so there will be no need to install additional packages during the Hands on Lab. The first thing that we must do is restore all of the NuGet packages from the internet.

This can be done by **Right-clicking** on the **Solution** and selecting **Restore NuGet packages...**

![Restore NuGets](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/a31a6bff-b45d-4c60-a602-1359f984e80b/2016-07-11_1328.png)

### Model

We will download details about the speakers. Open the **DevDaysSpeakers/Model/Speaker.cs** file and add the following properties to the **Speaker** class:

```csharp
public string Id { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public string Website { get; set; }
public string Title { get; set; }
public string Avatar { get; set; }
```

### View Model

The **SpeakersViewModel.cs** will provide all of the functionality to display data in our our main Xamarin.Forms view. It will consist of a list of speakers and a method that can be called to get the speakers from the server. It will also contain a boolean flag that indicates if we are getting data in a background task.

#### Implementing INotifyPropertyChanged

*INotifyPropertyChanged* is important for data binding in MVVM Frameworks. This is an interface that, when implemented, lets our view know about changes to the model.

Update:

```csharp
public class SpeakersViewModel
{

}
```

to

```csharp
public class SpeakersViewModel : INotifyPropertyChanged
{

}
```
Right click and tap **Implement Interface**, which will add the following line of code:

```csharp
public event PropertyChangedEventHandler PropertyChanged;
```

We will code a helper method named **OnPropertyChanged** that will raise the **PropertyChanged** event (see below). We will invoke the helper method whenever a property changes.

##### C# 6 (Visual Studio 2015/2017 or Visual Studio for Mac)
```csharp
private void OnPropertyChanged([CallerMemberName] string name = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
```


##### C# 5 (Visual Studio 2012 or 2013)
```csharp
private void OnPropertyChanged([CallerMemberName] string name = null)
{
    var changed = PropertyChanged;
    
    if (changed == null)
       return;

    changed.Invoke(this, new PropertyChangedEventArgs(name));
}
```

Now, we can call **OnPropertyChanged();** whenever a property updates. Let's create our first property now.

#### IsBusy
We will create a backing field and accessors for a boolean property. This will let our view know that our view model is busy so we don't perform duplicate operations (like allowing the user to refresh the data multiple times).

First, create the backing field:

```csharp
private bool isBusy;
```

Next, create the property:

```csharp
public bool IsBusy
{
    get { return isBusy; }
    set
    {
        isBusy = value;
        OnPropertyChanged();
    }
}
```

Notice that we call **OnPropertyChanged();** when the value changes. The Xamarin.Forms binding infrastructure will subscribe to our **PropertyChanged** event so the UI will be notified of the change.

#### ObservableCollection of Speaker

We will use an **ObservableCollection<Speaker>** that will be cleared and then loaded with **Speaker** objects. We use an **ObservableCollection** because it has built-in support to raise **CollectionChanged** events when we Add or Remove items from the collection. This means we don't call **OnPropertyChanged** when updating the collection.

Above the constructor of the **SpeakersViewModel** class definition, declare an auto-property:

```csharp
public ObservableCollection<Speaker> Speakers { get; set; }
```

Inside of the constructor, create a new instance of the `ObservableCollection`:

```csharp
public SpeakersViewModel()
{
    Speakers = new ObservableCollection<Speaker>();
}
```

#### GetSpeakers Method

We are ready to create a method named **GetSpeakers** which will retrieve the speaker data from the internet. We will first implement this with a simple HTTP request, and later update it to grab and sync the data from Azure!

Create a method named **GetSpeakers** with that returns an *async Task*:

```csharp
private async Task GetSpeakers()
{

}
```
The following code will be written INSIDE of this method.

Check the IsBusy boolean to see if we are already getting data:

```csharp
private async Task GetSpeakers()
{
    if (IsBusy)
        return;
}
```

Next, add some scaffolding for try/catch/finally blocks:

```csharp
private async Task GetSpeakers()
{
    if (IsBusy)
        return;

    Exception error = null;
    try
    {
        IsBusy = true;

    }
    catch (Exception ex)
    {
        error = ex;
    }
    finally
    {
       IsBusy = false;
    }

}
```

Notice, that we toggle *IsBusy* to true and then false when we start to call to the server and when we finish.

Now, we will use *HttpClient* to get the json-ecoded data from the server inside of the **try** block.

 ```csharp
using(var client = new HttpClient())
{
    var json = await client.GetStringAsync("http://demo4404797.mockable.io/speakers");
} 
```

Still inside of the **using**, we deserialize the json data and turn it into a list of Speakers using Json.NET:

```csharp
var items = JsonConvert.DeserializeObject<List<Speaker>>(json);
```

Still inside of the **using**, clear the Speakers ObservableCollection collection and then add the new speaker data:

```csharp
Speakers.Clear();
foreach (var item in items)
    Speakers.Add(item);
```
If anything goes wrong the **catch** block will save the exception. After the finally block we can display an alert:

```csharp
if (error != null)
    await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
```

The completed code should look like this:

```csharp
private async Task GetSpeakers()
{
    if (IsBusy)
        return;

    Exception error = null;
    try
    {
        IsBusy = true;
        
        using(var client = new HttpClient())
        {
            //grab json from server
            var json = await client.GetStringAsync("http://demo4404797.mockable.io/speakers");
            
            //Deserialize json
            var items = JsonConvert.DeserializeObject<List<Speaker>>(json);
            
            //Load speakers into list
            Speakers.Clear();
            foreach (var item in items)
                Speakers.Add(item);
        } 
    }
    catch (Exception ex)
    {
        Debug.WriteLine("Error: " + ex);
        error = ex;
    }
    finally
    {
        IsBusy = false;
    }

    if (error != null)
        await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
}
```

Our main method for getting data is now complete!

#### GetSpeakers Command

Instead of invoking this method directly, we will expose it with a **Command**. A Command has an interface that knows what method to invoke and has an optional way of describing if the Command is enabled.

Create a new Command called **GetSpeakersCommand**:

```csharp
public Command GetSpeakersCommand { get; set; }
```

Inside of the `SpeakersViewModel` constructor, create the `GetSpeakersCommand` and pass it two methods: one to invoke when the command is executed, and another that determines if the command is enabled. Both methods can be implemented as lambda expressions as shown below:

```csharp
GetSpeakersCommand = new Command(
                async () => await GetSpeakers(),
                () => !IsBusy);
```

The only modification that we will have to make is when we set the IsBusy property, as we will want to re-evaluate the enabled function that we created. In the **set** of **IsBusy** invoke the **ChangeCanExecute** method on the **GetSpeakersCommand** as shown below:

```csharp
set
{
    isBusy = value;
    OnPropertyChanged();
    //Update the can execute
    GetSpeakersCommand.ChangeCanExecute();
}
```

## The User Interface
It is now time to build the Xamarin.Forms user interface in **View/SpeakersPage.xaml**.

### SpeakersPage.xaml

In the first page we will add several vertically-stacked controls using a StackLayout. Between the `ContentPage` tags add the following:

```xml
 <StackLayout Spacing="0">

  </StackLayout>
```

This is the layout container that will host the child controls. By setting the Spacing to zero, we're requesting that no space is added between the childern.

Next, add a Button that has a binding to the **GetSpeakersCommand** that we created (see below). The command takes the place of a clicked handler and will be executed whenever the user taps the button.

```xml
<Button Text="Sync Speakers" Command="{Binding GetSpeakersCommand}"/>
```

Under the button we can display a loading bar when we are gathering data from the server. We can use an ActivityIndicator to do this and bind to the IsBusy property we created:

```xml
<ActivityIndicator IsRunning="{Binding IsBusy}" IsVisible="{Binding IsBusy}"/>
```

We will use a ListView that binds to the Speakers collection to display all of the items. We can use a special property called *x:Name=""* to name any control:

```xml
<ListView
    x:Name="ListViewSpeakers"
    ItemsSource="{Binding Speakers}">
        <!--Add ItemTemplate Here-->
</ListView>
```

We still need to describe what each item looks like, and to do so, we can use an ItemTemplate that has a DataTemplate with a specific View inside of it. Xamarin.Forms contains a few default Cells that we can use, and we will use the **ImageCell** that has an image and two rows of text.

Replace <!--Add ItemTemplate Here--> with: 

```xml
<ListView.ItemTemplate>
    <DataTemplate>
        <ImageCell
            Text="{Binding Name}"
            Detail="{Binding Title}"
            ImageSource="{Binding Avatar}"/>
    </DataTemplate>
</ListView.ItemTemplate>
```
Xamarin.Forms will automatically download, cache, and display the image from the server.

### Connect the View with the ViewModel
As we have bound some elements of the View to ViewModel properties, we have to tell the View which ViewModel to bind against. For this, we have to set the `BindingContext` to the `SpeakersViewModel`, we created. Open the `SpeakersPage.xaml.cs` file and see, that we already did this binding for you.

```csharp
SpeakersViewModel vm;

public SpeakersPage()
{
    InitializeComponent();

    // Create the view model and set as binding context
    vm = new SpeakersViewModel();
    BindingContext = vm;
}
```

### Validate App.cs

Open the App.cs file and you will see the entry point for the application, which is the constructor for `App()`. It creates the SpeakersPage, and then wraps it in a navigation page which adds title bar and enables stack navigation.

### Run the App!

Set the iOS, Android, or UWP project as the startup project and start debugging.

![Startup project](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/020972ff-2a81-48f1-bbc7-1e4b89794369/2016-07-11_1442.png)

#### iOS
If you are on a Windows PC then you will need to be connected to a macOS build host with the Xamarin tools installed to run and debug the app.

If connected, you will see a Green connection status. Select `iPhoneSimulator` as your target, and then select a Simulator to debug on.

![iOS Setup](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/a6b32d62-cd3d-41ea-bd16-1bcc1fbe1f9d/2016-07-11_1445.png)

#### Android

Set the DevDaysSpeakers.Droid as the startup project and select a simulator. The first compile may take some additional time as Support Packages may need to be downloaded.

If you run into an issue building the project with an error such as:

**aapt.exe exited with code** or **Unsupported major.minor version 52** then your Java JDK may not be setup correctly, or you have newer build tools installed then what is supported. See this technical bulletin for support: https://releases.xamarin.com/technical-bulletin-android-sdk-build-tools-24/

Additionally, see James' blog for visual reference: http://motzcod.es/post/149717060272/fix-for-unsupported-majorminor-version-520

If you are running into issues with Android support packages that can't be unzipped because of corruption please check: https://xamarinhelp.com/debugging-xamarin-android-build-and-deployment-failures/

#### Windows 10

Set the DevDaysSpeakers.UWP as the startup project and select debug to **Local Machine**.


## Details

Now, let's add navigation to a second page that displays speaker details. Open the code-behind for **SpeakersPage.xaml** called **SpeakersPage.xaml.cs**.

### ItemSelected Event

In the code-behind you will find the setup for the SpeakersViewModel. Under **BindingContext = vm;**, let's add an event to the **ListViewSpeakers** to get notified when an item is selected:

```csharp
ListViewSpeakers.ItemSelected += ListViewSpeakers_ItemSelected;
```

Implement this method so it navigates to the DetailsPage:

```csharp
private async void ListViewSpeakers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
{
    var speaker = e.SelectedItem as Speaker;
    if (speaker == null)
        return;

    await Navigation.PushAsync(new DetailsPage(speaker));

    ListViewSpeakers.SelectedItem = null;
}
```

In the above code we check to see if the selected item is non-null and then use the built in **Navigation** API to push a new page and deselect the item.

### DetailsPage.xaml

Let's add UI to the DetailsPage. Similar to the SpeakersPage, we will use a StackLayout, but we will wrap it in a ScrollView. This allows the user to scroll if the page content is longer than the avaliable screen space.

```xml
<ScrollView Padding="10">
    <StackLayout Spacing="10">
        <!-- Detail controls here -->
    </StackLayout>    
</ScrollView>
```

Now add controls and bindings for the properties in the Speaker class:

```xml
<Image Source="{Binding Avatar}" HeightRequest="200" WidthRequest="200"/>
      
<Label Text="{Binding Name}" FontSize="24"/>
<Label Text="{Binding Title}" TextColor="Purple"/>
<Label Text="{Binding Description}"/>
```

Add two buttons; give them names so we can access them in the code-behind. We'll be adding click handlers to each button.

```xml
<Button Text="Speak" x:Name="ButtonSpeak"/>
<Button Text="Go to Website" x:Name="ButtonWebsite"/>
```

### Text to Speech

If we open up **DetailsPage.xaml.cs** we can now add a few more click handlers. Let's start with ButtonSpeak, where we will use the [Text To Speech Plugin](https://github.com/jamesmontemagno/TextToSpeechPlugin) to read back the speaker's description.

In the constructor, add a clicked handler below the BindingContext:

```csharp
ButtonSpeak.Clicked += ButtonSpeak_Clicked;
```

Then we can add the clicked handler and call the cross-platform API for text to speech:

```csharp
private void ButtonSpeak_Clicked(object sender, EventArgs e)
{
    CrossTextToSpeech.Current.Speak(this.speaker.Description);
}
```

### Open Website
Xamarin.Forms includes many APIs for performing common tasks such as opening a URL in the default browser.

Let's add another clicked handler, but this time for `ButtonWebsite`:

```csharp
ButtonWebsite.Clicked += ButtonWebsite_Clicked;
```

Then, we can use the static Device class to call the OpenUri method:

```csharp
private void ButtonWebsite_Clicked(object sender, EventArgs e)
{
    if (speaker.Website.StartsWith("http"))
        Device.OpenUri(new Uri(speaker.Website));
}
```

### Compile & Run
Now, we should be all set to compile and run our application!

## Connect to Azure Mobile Apps

Being able to grab data from a RESTful end point is great, but what about creating the back-end service? This is where Azure Mobile Apps comes in. Let's update our application to use an Azure Mobile Apps back-end.

If you don't already have an Azure account, go to [http://portal.azure.com](http://portal.azure.com) and register.

Once you're registered, open the Azure portal, select the **+ New** button and search for **mobile apps**. You will see the results as shown below. Select **Mobile Apps Quickstart**

![Quickstart](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/c2894f06-c688-43ad-b812-6384b34c5cb0/2016-07-11_1546.png)

The Quickstart blade will open, select **Create**

![Create quickstart](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/344d6fc2-1771-4cb7-a49a-6bd9e9579ba6/2016-07-11_1548.png)

This will open a settings blade with 4 settings:

**App name**

This is a unique name for the app that you will need when connecting your Xamarin.Forms client app to the hosted Azure Mobile App. You will need to choose a globally-unique name; for example, you could try something like *yourlastnamespeakers*.

**Subscription**
Select a subscription or create a pay-as-you-go account (this service will not cost you anything).

**Resource Group**
Select *Create new* and call it **DevDaysSpeakers**.

A resource group is logical container the can hold multiple Azure services. Using a resource group allows you to delete a collection of related services in one step.

**App Service plan/Location**
Click this field and select **Create New**, give it a unique name, select a location (typically you would choose a location close to your customers), and then select the F1 Free tier:

![service plan](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/7559d3f1-7ee6-490f-ac5e-d1028feba88f/2016-07-11_1553.png)

Finally check **Pin to dashboard** and click create:

![](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/a844c283-550c-4647-82d3-32d8bda4282f/2016-07-11_1554.png)

This will take about 3-5 minutes to setup, so let's head back to the code!


### Update AzureService.cs
We will use the [Azure Mobile Apps SDK](https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-xamarin-forms-get-started/) to connect our mobile app to our Azure back-end with just a few lines of code.

Open the DevDaysSpeakers/Services/AzureService.cs and add our url to the Initialize method:

```csharp
var appUrl = "https://OUR-APP-NAME-HERE.azurewebsites.net";
```

Be sure to update YOUR-APP-NAME-HERE with the app name you specified when creating your Azure Mobile App.

The logic in the Initialize method will setup our database and create our `IMobileServiceSyncTable<Speaker>` table that we can use to retieve speaker data from the Azure Mobile App. There are two methods that we need to fill in to get and sync data from the server.


#### GetSpeakers
In this method, we will need to initialize, sync, and query the table for items. We can use complex LINQ queries to order the results:

```csharp
await Initialize();
await SyncSpeakers();
return await table.OrderBy(s => s.Name).ToEnumerableAsync();   
```

#### SyncSpeakers
Our Azure backend can push any local changes and then pull all of the latest data from the server using the following code that can be added to the try inside of the SyncSpeakers method:

```csharp
await Client.SyncContext.PushAsync();
await table.PullAsync("allSpeakers", table.CreateQuery());
```
That is it for our Azure code! Just a few lines, and we are ready to pull the data from Azure.

### Update SpeakersViewModel.cs

Update async Task GetSpeakers():

Now, instead of using the HttpClient to get a string, let's query the Table:

Change the *try* block of code to:

```csharp
try
{
    IsBusy = true;

    var service = DependencyService.Get<AzureService>();
    var items = await service.GetSpeakers();

    Speakers.Clear();
    foreach (var item in items)
        Speakers.Add(item);
}
```

Now, we have implemented the code we need in our app! Amazing isn't it? The AzureService object will automatically handle all communication with your Azure back-end for you, do online/offline synchronization so your app works even when it's not connected.

Let's head back to the Azure Portal and populate the database.

When the Quickstart finishes you should see the following screen, or can go to it by tapping the pin on the dashboard:

![Quickstart](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/71ad3e06-dcc5-4c8b-8ebd-93b2df9ea2b2/2016-07-11_1601.png)

Under **Features** select **Easy Tables**.

It will have created a `TodoItem`, which you should see, but we can create a new table and upload a default set of data by selecting **Add from CSV** from the menu.

Ensure that you have downloaded this repo and have the **Speaker.csv** file that is in this folder.

Select the file and it will add a new table name and find the fields that we have listed. Then hit Start Upload.

![upload data](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/eea2bca6-2dd0-45b3-99af-699d14a0113c/2016-07-11_1603.png)

> Note: If you get an error while uploading the Speaker.CSV file, it may be a bug that has been resolved. To workaround this, go to the "Application settings" under the "Settings" section and scroll to "App Settings". Change the value for MobileAppsManagement_EXTENSION_VERSION to 1.0.367 and save the changes. Now retry the "Add from CSV" process again

![application settings fix](appsettingsfix.png)

Now you can re-run your application and get data from Azure!


## Bonus Take Home Challenges

Take Dev Days further with these additional challenges that you can complete at home after Dev Days ends.

### Challenge 1: Cognitive Services
For fun, you can add the [Cognitive Service Emotion API](https://www.microsoft.com/cognitive-services/en-us/emotion-api) and add another Button to the detail page to analyze the speaker's face for happiness level. 

Go to: http://microsoft.com/cognitive and create a new account and an API key for the Emotion service.

Follow these steps:

1.) Add **Microsoft.ProjectOxford.Emotion** NuGet package to all projects

2.) Add a new class called EmotionService and add the following code (ensure you update the API key in the GetHappinessAsync call):

```csharp
public class EmotionService
{
    private static async Task<Emotion[]> GetHappinessAsync(string url)
    {        
        var emotionClient = new EmotionServiceClient("INSERT_EMOTION_SERVICE_KEY_HERE");

        var emotionResults = await emotionClient.RecognizeAsync(url);

        if (emotionResults == null || emotionResults.Count() == 0)
        {
            throw new Exception("Can't detect face");
        }

        return emotionResults;
    }

    //Average happiness calculation in case of multiple people
    public static async Task<float> GetAverageHappinessScoreAsync(string url)
    {
        Emotion[] emotionResults = await GetHappinessAsync(url);

        float score = 0;
        foreach (var emotionResult in emotionResults)
        {
            score = score + emotionResult.Scores.Happiness;
        }

        return score / emotionResults.Count();
    }

    public static string GetHappinessMessage(float score)
    {
        score = score * 100;
        double result = Math.Round(score, 2);

        if (score >= 50)
            return result + " % :-)";
        else
            return result + "% :-(";
    }
}
```

3.) Now add a new button to the Details Page and expose it with **x:Name="ButtonAnalyze**

4.) Add a new clicked handler and add the async keyword to it.

5.) Call 
```csharp
var level = await EmotionService.GetAverageHappinessScoreAsync(this.speaker.Avatar);
```

6.) Then display a pop-up alert:
```csharp
await DisplayAlert("Happiness Level", EmotionService.GetHappinessMessage(level), "OK");
```

### Challenge 2: Edit Speaker Details

In this challenge we will make the speaker's Title editable.

Open DetailsPage.xaml and change the Label that is displaying the Title from:

```xml
<Label Text="{Binding Title}" TextColor="Purple"/>
```

to an Entry with a OneWay data binding (this means when we enter text it will not change the actual data), and a Name to expose it in the code behind.

```xml
<Entry
    Text="{Binding Title, Mode=OneWay}" 
    TextColor="Purple" 
    x:Name="EntryTitle"/>
```

Let's add a save Button under the Go To Website button.

```xml
<Button Text="Save" x:Name="ButtonSave"/>
```

#### Update SpeakersViewModel

Open AzureService and add a new method called UpdateSpeaker(Speaker speaker), that will update the speaker, sync, and refresh the list:

```csharp
public async Task UpdateSpeaker(Speaker speaker)
{
    await Initialize();
    await table.UpdateAsync(speaker);
    await SyncSpeakers();
}
```

Open the SpeakersViewModel.cs and add a similar method:

```
public async Task UpdateSpeaker(Speaker speaker)
{
    var service = DependencyService.Get<AzureService>();
    service.UpdateSpeaker(speaker);
    await GetSpeakers();         
}
```

#### Update DetailsPage.xaml.cs
Let's update the constructor to pass in the SpeakersViewModel for the DetailsPage:

Before:
```csharp
Speaker speaker;
public DetailsPage(Speaker item)
{
    InitializeComponent();
    this.speaker = item;
    
    // ...
}
```
After:
```csharp
Speaker speaker;
SpeakersViewModel vm;
public DetailsPage(Speaker item, SpeakersViewModel viewModel)
{
    InitializeComponent();
    this.speaker = item;
    this.vm = viewModel;
    
    // ...
}
```

Under the other clicked handlers, we will add another clicked handler for ButtonSave.

```csharp
ButtonSave.Clicked += ButtonSave_Clicked;
```
When the button is clicked, we will update the speaker, and call save and then navigate back:

```csharp
private async void ButtonSave_Clicked(object sender, EventArgs e)
{
    speaker.Title = EntryTitle.Text;
    await vm.UpdateSpeaker(speaker);
    await Navigation.PopAsync();
}
```

Finally, we will need to pass in the ViewModel when we navigate in the SpeakersPage.xaml.cs in the ListViewSpeakers_ItemSelected method:

```csharp
//Pass in view model now.
await Navigation.PushAsync(new DetailsPage(speaker, vm));
```

There you have it!
