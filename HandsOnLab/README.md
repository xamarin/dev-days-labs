## Xamrin Dev Days Hands On Lab

Today, we will be building a cloud connected [Xamarin.Forms](http://xamarin.com/forms) application that will display a list of speakers at Xamarin Dev Days and show their details. We will start by building some business logic backend that pulls down json from a RESTful endpoint and then we will connect it to an Azure Mobile App backend in just a few lines of code.


### Get Started

Open **Start/DevDaysSpeakers.sln**

This solution contains 4 projects

* DevDaysSpeakers (Portable) - Portable Class Library that will have all shared code (model, views, and view models).
* DevDaysSpeakers.Droid - Xamarin.Android application
* DevDaysSpeakers.iOS - Xamarin.iOS application
* DevDaysSpeakers.UWP - Windows 10 UWP application (can only be run from VS 2015 on Windows 10)

![Solution](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/44f4caa9-efb9-4405-95d4-7341608e1c0a/Portable.png)

The **DevDaysSpeakers (Portable)** also has blank code files and XAML pages that we will use during the Hands on Lab.

#### NuGet Restore

All projects have the required NuGet packages already installed, so there will be no need to install additional packages during the Hands on Lab. The first thing that we must do is Restore all of the NuGet packages from the internet.

This can be done by **Right-clicking** on the **Solution** and clicking on **Restore NuGet packages...**

![Restore NuGets](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/a31a6bff-b45d-4c60-a602-1359f984e80b/2016-07-11_1328.png)

### Model

We will be pulling down information about spearks. Open the **DevDaysSpeakers/Model/Speaker.cs** file and add the following properties inside of the **Speaker** class:

```csharp
public string Id { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public string Website { get; set; }
public string Title { get; set; }
public string Avatar { get; set; }
```

### View Model

The **SpeakersViewModel.cs** will provide all of the functionality for how our main Xamarin.Forms view will display data. It will consist of a list of speakers and a method that can be called to get the speakers from the server. It will also contain a boolean flag that will indicate if the we are getting data in a background task.

#### Implementing INotifyPropertyChanged

*INotifyPropertyChanged* is the key data binding in MVVM Frameworks. It is an interface that provides a contract that the user interface can subscribe to for notifications when the code in the code behind changes.

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

Simply right click and tap **Implement Interface**, which will add the following line of code:

```csharp
public event PropertyChangedEventHandler PropertyChanged;
```

This is the method that we will invoke whenever a property changes. This means we need a helper method to implement called **OnPropertyChanged**:

##### C# 6 (Visual Studio 2015 or Xamarin Studio on Mac)
```csharp
void OnPropertyChanged([CallerMemberName] string name = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
```


##### C# 5 (Visual Studio 2012 or 2013)
```csharp
void OnPropertyChanged([CallerMemberName] string name = null)
{
    var changed = PropertyChanged;

            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(name));
}
```

Now, we can call **OnPropertyChanged();** whenever a property updates. Let's create our first bindable property now.

#### IsBusy

In the class we will want to create a backing field and auto-properties for getting and setting a boolean.

First, create the backing field:

```csharp
bool busy;
```

Next, let's create the auto-property:

```csharp
public bool IsBusy
{
    get { return busy; }
    set
    {
        busy = value;
        OnPropertyChanged();
    }
}
```

Notice that we call **OnPropertyChanged();** so Xamarin.Forms can be notified automatically.

#### ObservableCollection of Speaker

We will use an ObservableCollection<Speaker> that will be cleared and then loaded with speakers. We will use an **ObservableCollection** because it has built in support for **CollectionChanged** events when we Add or Remove from it. This is very nice so we don't have to call **OnPropertyChanged** each time.

In the class above the constructor simply create an autoproperty:

```csharp
public ObservableCollection<Speaker> Speakers { get; set; }
```

Inside of the constructor create a new instance of the ObservableCollection so the constructor looks like:

```csharp
public SpeakersViewModel()
{
    Speakers = new ObservableCollection<Speaker>();
}
```

#### GetSpeakers Method

We are now set to create our method called GetSpeakers, which will call to get all of the speaker data from the internet. We will first implement this with a simply HTTP request, but later on update it to grab and sync the data from Azure!

First, create a method called **GetSpeakers** which is of type *async Task* (it is a Task because it is using Async methods):

```csharp
async Task GetSpeakers()
{

}
```
The following code will be written INSIDE of this method:

First is to check if we are already grabbind data:

```csharp
async Task GetSpeakers()
{
    if(IsBusy)
        return;
}
```

Next we will create some scaffolding for try/catch/finally blocks:

```csharp
async Task GetSpeakers()
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

Notice, that we set IsBusy to true and then false when we start to call to the server and when we finish.

Now, we will use *HttpClient* to grab a the json from the server inside of the **try** block.

 ```csharp
using(var client = new HttpClient())
{
    //grab json from server
    var json = await client.GetStringAsync("http://demo4404797.mockable.io/speakers");
} 
```

Still inside of the **using**, we will Deserialize the json and turn it into a list of Speakers with Json.NET:

```csharp
var items = JsonConvert.DeserializeObject<List<Speaker>>(json);
```

Still inside of the **using**, we can will clear the speakers and then load them into the ObservableCollection:

```csharp
Speakers.Clear();
foreach (var item in items)
    Speakers.Add(item);
```
If anything goes wrong the **catch** will save out the exception and AFTER the finally block we can pop up an alert:

```csharp
if (error != null)
    await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
```

The completed code should look like:

```csharp
async Task GetSpeakers()
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

Our main method for gettering data is now complete!

#### GetSpeakers Command

Intead of invoking this method directly, we will expose it with a **Command**. A Command has an interface that knows what method to invoke and has an optional way of describing if the Command is enabled.

Where we created our ObservableCollection<Speaker> Speakers {get;set;} create a new Command called **GetSpeakersCommand**:

```csharp
public Command GetSpeakersCommand { get; set; }
```

Inside of the **SpeakersViewModel()** constructor we can create the GetSpeakersCommand and assign it a method to use. We can also pass in an enabled flag leveraging our IsBusy:

```csharp
GetSpeakersCommand = new Command(
                async () => await GetSpeakers(),
                () => !IsBusy);
```

The only modification that we will have to make is when we set the IsBusy property, as we will want to re-evaluate the enabled function that we created. In the **set** of **IsBusy** simply invoke the **ChangeCanExecute** method on the **GetSpeakersCommand** such as:

```csharp
set
{
    busy = value;
    OnPropertyChanged();
    //Update the can execute
    GetSpeakersCommand.ChangeCanExecute();
}
```

## The User Interface!!!
It is now finally time to build out our first Xamarin.Forms user interface in the *View/SpeakersPage.xaml**

### SpeakersPage.xaml

For the first page of the app we will add a few controls onto the page that are stacked vertically. We can use a StackLayout to do this. Inbetween the ContentPage add the following:

```xml
 <StackLayout Spacing="0">

  </StackLayout>
```

This will be the base where all of the child controls will go and will have no space inbetween them.

Next, let's add a Button that has a binding to the **GetSpeakersCommand** that we created:

```xml
<Button Text="Sync Speakers" Command="{Binding GetSpeakersCommand}"/>
```

Under the button we can display a loading bar when we are gathering data form the server. We can use an ActivityIndicator to do this and bind to the IsBusy property we created:

```xml
<ActivityIndicator IsRunning="{Binding IsBusy}" IsVisible="{Binding IsBusy}"/>
```

We will use a ListView that binds to the Speakers collection to display all of the items. We can use a special property called *x:Name=""* to name any control:

```xml
<ListView x:Name="ListViewSpeakers"
              ItemsSource="{Binding Speakers}">
        <!--Add ItemTemplate Here-->
</ListView>
```

We still need to describe what each item looks like, and to do so, we can use an ItemTemplate that has a DataTemplate with a specific View inside of it. Xamarin.Forms contains a few default Cells that we can use, and we will use the **ImageCell** that has an image and two rows of text

Replace <!--Add ItemTemplate Here--> with: 

```xml
<ListView.ItemTemplate>
    <DataTemplate>
        <ImageCell Text="{Binding Name}"
                    Detail="{Binding Title}"
                    ImageSource="{Binding Avatar}"/>
    </DataTemplate>
</ListView.ItemTemplate>
```
Xamarin.Forms will automatically download, cache, and display the image from the server.

### Validate App.cs

Open the App.cs file and you will see the entry point for the application, which is the constructor for App(). It simply creates the new SpeakersPage, and then wraps it in a navigation page to get a nice title bar.

### Run the App!

Now, you can set the iOS, Android, or UWP (Windows/VS2015 only) as the start project and start debugging.

![Startup project](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/020972ff-2a81-48f1-bbc7-1e4b89794369/2016-07-11_1442.png)

#### iOS
If you are on a PC then you will need to be connected to a macOS device with Xamarin installed to run and debug the app.

If connected, you will see a Green connection status. Select **iPhoneSimulator as your target, and then select the Simulator to debug on.

![iOS Setup](http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/a6b32d62-cd3d-41ea-bd16-1bcc1fbe1f9d/2016-07-11_1445.png)

#### Android

Simply set the DevDaysSpeakers.Droid as the startup project and select a simulator to run on.

#### Windows 10

Simply set the DevDaysSpeakers.UWP as the startup project and select debug to **Local Machine**.

## Details

Now, let's do some navigation and display some Details. Let's upen up the code behind for **SpeakersPage.xaml** called **SpeakersPage.xaml.cs**.

### ItemSelected Event

In the code behind you will find the setup for the SpeakersViewModel. Under **BindingContext = vm;**, let's add an event to the **ListViewSpeakers** to get notified when an item is selected

```csharp
ListViewSpeakers.ItemSelected += ListViewSpeakers_ItemSelected;
```

Let's create and fill in this method and navigate to the DetailsPage.

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

In the above code we check to see if the selected item is not null and then use the built in **Navigation** API to push a new page and then deselect the item.

### DetailsPage.xaml

Let's now fill in the Details page. Similar to the SpeakersPage, we will use a StackLayout, but we will wrap it in a ScrollView, incase we have long text.

```xml
  <ScrollView Padding="10">
    <StackLayout Spacing="10">
     <!-- Detail controls here -->
    </StackLayout>    
  </ScrollView>
```

Now, let's add controls and bindings for the properties in the Speaker object:

```xml
<Image Source="{Binding Avatar}" HeightRequest="200" WidthRequest="200"/>
      
<Label Text="{Binding Name}" FontSize="24"/>
<Label Text="{Binding Title}" TextColor="Purple"/>
<Label Text="{Binding Description}"/>
```

Now, for fun, let's add two buttons that we will add click events to in the code behind:

```xml
<Button Text="Speak" x:Name="ButtonSpeak"/>
<Button Text="Go to Website" x:Name="ButtonWebsite"/>
```

### Text to Speech

If we open up **DetailsPage.xaml.cs** we can now add a few more click handlers. Let's start with ButtonSpeak, where we will use the [Text To Speech Plugin](https://github.com/jamesmontemagno/TextToSpeechPlugin) to read back the speaker's description.

In the constructor, add a click handler below the BindingContext:

```csharp
ButtonSpeak.Clicked += ButtonSpeak_Clicked;
```

Then we can add the click handler and call the cross platform API for text to speech:

```csharp
private void ButtonSpeak_Clicked(object sender, EventArgs e)
{
    CrossTextToSpeech.Current.Speak(this.speaker.Description);
}
```

### Open Website
Xamarin.Forms itself has some nice APIs built write in for cross platform functionality, such as opening a URL in the default browser.

Let's add another click handler, but this time for ButtonWebsite:

```csharp
ButtonWebsite.Clicked += ButtonWebsite_Clicked;
```

Then, we can use the Device keyword to call the OpenUri method:

```csharp
private void ButtonWebsite_Clicked(object sender, EventArgs e)
{
    if (speaker.Website.StartsWith("http"))
        Device.OpenUri(new Uri(speaker.Website));
}
```

### Compile & Run
Now, we should be all set to compile, and run just like before!

## Connect to Azure Mobile Apps

//Coming soon

## Bonus

Take Dev Days father with the need challenges!

### Challenge 1: Cognitive Services
For fun, you can add the [Cognitive Serivce Emotion API](https://www.microsoft.com/cognitive-services/en-us/emotion-api) and add another Button to the detail page to analyze the speakers face for happiness level. 

Go to: http://microsoft.com/cognitive and create a new account and an API key for the Emotion service.

Follow these steps:

1.) Add **Microsoft.ProjectOxford.Emotion** nuget package to all projects

2.) Add a new class called EmotionService and add the following code:

```csharp
public class EmotionService
{
    private static async Task<Emotion[]> GetHappinessAsync(string url)
    {
        var client = new HttpClient();
        var stream = await client.GetStreamAsync(url);
        var emotionClient = new EmotionServiceClient(CognitiveServicesKeys.Emotion);

        var emotionResults = await emotionClient.RecognizeAsync(stream);

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

4.) Add a new click handler and add the async keyword to it.

5.) Call 
```csharp
var level = await EmotionService.GetAverageHappinessScoreAsync(this.speaker.Avatar);
```

6.) Then display a pop up alert:
```csharp
await DisplayAlert("Happiness Level", level, "OK");
```

### Challenge 2: Edit Speaker Details

//coming soon