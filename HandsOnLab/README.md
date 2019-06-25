# Xamarin Dev Days Hands On Lab

Today we will build a cloud connected [Xamarin.Forms](https://docs.microsoft.com/xamarin/xamarin-forms?WT.mc_id=devdayslabs-github-bramin) application that will display a list of Xamarin Dev Days speakers. We will start by building the business logic backend that pulls down json-ecoded data from an existing RESTful endpoint. Then we will connect it to an Azure Functions backend in just a few lines of code.

## Mobile App Walkthrough

### 0. Configure Your Machine for Xamarin

1. On you machine, follow the steps in the [Workshop Setup](./WorkshopSetup.md) to install and configure Xamarin and Visual Studio

### 1. Open Solution in Visual Studio

1. Open **HandsOnLab/Start/DevDaysSpeakers.sln**

This solution contains 3 solution folders (Mobile, Common and Backend)

Mobile contains 4 projects:

* DevDaysSpeakers  - .NET Standard Project that is used by the Droid, iOS and UWP projects and will have all shared code (model, views, and view models)
* DevDaysSpeakers.Droid - Xamarin.Android application
* DevDaysSpeakers.iOS - Xamarin.iOS application (requires a Mac)
* DevDaysSpeakers.UWP - Windows 10 UWP application (requires Visual Studio on PC)

![Solution](https://content.screencast.com/users/JamesMontemagno/folders/Jing/media/44f4caa9-efb9-4405-95d4-7341608e1c0a/Portable.png)

Common contains 1 project:
* DevDaysSpeakers.Shared - this is a Shared Project that contains code used by both the Backend and Mobile solutions

Backend contains 1 project: 
* DevDaysSpeakers.Functions - this is an Azure Functions project we will use to create an API running in the cloud

The **DevDaysSpeakers** project also has blank code files that we will use during the Hands on Lab.

### 2. NuGet Restore

All projects have the required NuGet packages already installed, so there will be no need to install additional packages during the Hands on Lab. The first thing that we must do is restore all of the NuGet packages from the internet.

1. **Right-click** on the **Solution** and selecting **Restore NuGet packages...**

![Restore NuGets](https://content.screencast.com/users/JamesMontemagno/folders/Jing/media/a31a6bff-b45d-4c60-a602-1359f984e80b/2016-07-11_1328.png)

### 3. Model

We will download details about the speakers.

1. Open `Common/DevDaysSpeakers.Shared/Models/Speaker.cs`
2. In `Speaker.cs`, copy/paste the following properties:

```csharp
namespace DevDaysSpeakers.Shared.Models
{
    public class Speaker
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
    }
}

```

### 4. Implementing INotifyPropertyChanged

*INotifyPropertyChanged* is important for data binding in MVVM Frameworks. This is an interface that, when implemented, lets our view know about changes to the model.

1. In Visual Studio, open `Mobile/DevDaysSpeakers/ViewModels/SpeakersViewModel.cs`
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

1. In `SpeakersViewModel.cs` declare a read-only property which we will initialize to an empty collection

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

1. In `SpeakersViewModel.cs`, create a new `ICommand` called `GetSpeakersCommand`:

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
It is now time to build the Xamarin.Forms UI.

1. In `Mobile/DevDaysSpeakers/Views/SpeakersPage.cs`, in the constructor, set the `BindingContext` equal to `new SpeakersViewModel()`. This connects SpeakersPage with SpeakersViewModel via MVVM.

```csharp
public SpeakersPage()
{
    BindingContext = new SpeakersViewModel();
}
```

2. In `SpeakersPage.cs`, initialize the `ListView`:

```csharp
public SpeakersPage()
{
    BindingContext = new SpeakersViewModel();

    speakersListView = new ListView(ListViewCachingStrategy.RecycleElement)
    {
        IsPullToRefreshEnabled = true,
        ItemTemplate = new DataTemplate(typeof(SpeakersCell)),
        SeparatorColor = Color.Transparent
    };
}
```

3. In `SpeakersPage.cs`, set the bindings for `speakersListView`. This follows the MVVM pattern to allow the `ListView` to display the data from `SpeakersViewModel`

```csharp
public SpeakersPage()
{
    BindingContext = new SpeakersViewModel();

    speakersListView = new ListView(ListViewCachingStrategy.RecycleElement)
    {
        IsPullToRefreshEnabled = true,
        ItemTemplate = new DataTemplate(typeof(SpeakersCell)),
        SeparatorColor = Color.Transparent
    };
    speakersListView.SetBinding(ListView.ItemsSourceProperty, nameof(SpeakersViewModel.Speakers));
    speakersListView.SetBinding(ListView.RefreshCommandProperty, nameof(SpeakersViewModel.GetSpeakersCommand));
    speakersListView.SetBinding(ListView.IsRefreshingProperty, nameof(SpeakersViewModel.IsBusy));
}
```

4. In `SpeakersPage.cs`, set the `Title` and `Content` for the page

```csharp
public SpeakersPage()
{
    BindingContext = new SpeakersViewModel();

    speakersListView = new ListView(ListViewCachingStrategy.RecycleElement)
    {
        IsPullToRefreshEnabled = true,
        ItemTemplate = new DataTemplate(typeof(SpeakersCell)),
        SeparatorColor = Color.Transparent
    };
    speakersListView.SetBinding(ListView.ItemsSourceProperty, nameof(SpeakersViewModel.Speakers));
    speakersListView.SetBinding(ListView.RefreshCommandProperty, nameof(SpeakersViewModel.GetSpeakersCommand));
    speakersListView.SetBinding(ListView.IsRefreshingProperty, nameof(SpeakersViewModel.IsBusy));

    Title = "Speakers";

    Content = speakersListView;
}
```



2. In `SpeakersPage.xaml.xs`, override `OnAppearing()` by adding the following method which tells the ListView to automatically refresh when the page appears on the screen:

```csharp
public SpeakersPage()
{
    BindingContext = new SpeakersViewModel();

    speakersListView = new ListView(ListViewCachingStrategy.RecycleElement)
    {
        IsPullToRefreshEnabled = true,
        ItemTemplate = new DataTemplate(typeof(SpeakersCell)),
        SeparatorColor = Color.Transparent
    };
    speakersListView.SetBinding(ListView.ItemsSourceProperty, nameof(SpeakersViewModel.Speakers));
    speakersListView.SetBinding(ListView.RefreshCommandProperty, nameof(SpeakersViewModel.GetSpeakersCommand));
    speakersListView.SetBinding(ListView.IsRefreshingProperty, nameof(SpeakersViewModel.IsBusy));

    Title = "Speakers";

    Content = speakersListView;
}

protected override void OnAppearing()
{
    base.OnAppearing();

    speakersListView.BeginRefresh();
}
```

### 10. Run the App

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

### 11. Add Navigation

Now, let's add navigation to a second page that displays speaker details!

1. In `SpeakersPage.cs`, create a method called `ListViewSpeakers_ItemSelected`:
    - This code checks to see if the selected item is non-null and then use the built in `Navigation` API to push a new page and deselect the item.
    - `listView.SelectedItem = null;` tells the `ListView` to unhighlight the selected row

```csharp
private async void ListViewSpeakers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
{
    if (sender is ListView listView && e.SelectedItem is Speaker speaker)
    {
        await Navigation.PushAsync(new DetailsPage(speaker));
        listView.SelectedItem = null;
    }
}
```

2. In `SpeakersPage.cs`, in the constructor, assign `ListViewSpeakers_ItemSelected` to the `speakersListView.ItemSelected`

```csharp
public SpeakersPage()
{
    BindingContext = new SpeakersViewModel();

    speakersListView = new ListView(ListViewCachingStrategy.RecycleElement)
    {
        IsPullToRefreshEnabled = true,
        ItemTemplate = new DataTemplate(typeof(SpeakersCell)),
        SeparatorColor = Color.Transparent
    };
    speakersListView.SetBinding(ListView.ItemsSourceProperty, nameof(SpeakersViewModel.Speakers));
    speakersListView.SetBinding(ListView.RefreshCommandProperty, nameof(SpeakersViewModel.GetSpeakersCommand));
    speakersListView.SetBinding(ListView.IsRefreshingProperty, nameof(SpeakersViewModel.IsBusy));

    Title = "Speakers";

    Content = speakersListView;

    speakersListView.ItemSelected += ListViewSpeakers_ItemSelected;
}
```

### 12. Create DetailsPage UI

Let's add UI to the DetailsPage. Similar to the SpeakersPage, we will use a StackLayout, but we will wrap it in a ScrollView. This allows the user to scroll if the page content is longer than the available screen space.

1. In `Mobile/DevDaysSpeakers/Views/DetailsPage.cs`, assign `item` to `speaker`;

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;
    }
```

2.  In `DetailsPage.cs`, in the constructor, initialize the `Image` and `Label` controls and bindings for the properties in the Speaker class:

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;

        var avatarImage = new Image
        {
            HeightRequest = 200,
            WidthRequest = 200,
            Source = speaker.Avatar
        };

        var nameLabel = new Label
        {
            FontSize = 24,
            Text = speaker.Name
        };

        var titleLabel = new Label
        {
            TextColor = Color.Purple,
            Text = speaker.Title
        };

        var descriptionLabel = new Label
        {
            Text = speaker.Description
        };
    }
}
```

3.  In `DetailsPage.cs`, in the constructor, add a `StackLayout` and adding each control

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;

        var avatarImage = new Image
        {
            HeightRequest = 200,
            WidthRequest = 200,
            Source = speaker.Avatar
        };

        var nameLabel = new Label
        {
            FontSize = 24,
            Text = speaker.Name
        };

        var titleLabel = new Label
        {
            TextColor = Color.Purple,
            Text = speaker.Title
        };

        var descriptionLabel = new Label
        {
            Text = speaker.Description
        };

        var stackLayout = new StackLayout
        {
            Spacing = 10
        };
        stackLayout.Children.Add(avatarImage);
        stackLayout.Children.Add(nameLabel);
        stackLayout.Children.Add(titleLabel);
        stackLayout.Children.Add(descriptionLabel);
    }
}
```

4.  In `DetailsPage.cs`, in the constructor, assign the `Padding`, `Title`, and `Content`
    - `Padding` adds spacing between the `ContentPage` and the edge of the device
    - The content will use a `ScrollView` that allows the `StackLayout` to scroll if its content is too long to fit onto the screen

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;

        var avatarImage = new Image
        {
            HeightRequest = 200,
            WidthRequest = 200,
            Source = speaker.Avatar
        };

        var nameLabel = new Label
        {
            FontSize = 24,
            Text = speaker.Name
        };

        var titleLabel = new Label
        {
            TextColor = Color.Purple,
            Text = speaker.Title
        };

        var descriptionLabel = new Label
        {
            Text = speaker.Description
        };

        var stackLayout = new StackLayout
        {
            Spacing = 10
        };
        stackLayout.Children.Add(avatarImage);
        stackLayout.Children.Add(nameLabel);
        stackLayout.Children.Add(titleLabel);
        stackLayout.Children.Add(descriptionLabel);

        Title = speaker.Name;

        Padding = new Thickness(10);

        Content = new ScrollView 
        { 
            Content = stackLayout 
        };
    }
}
```

### 13. Add Text to Speech

1. In `DetailsPage.cs`, add an event handler called `HandleSpeakButtonClicked`

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;

        var avatarImage = new Image
        {
            HeightRequest = 200,
            WidthRequest = 200,
            Source = speaker.Avatar
        };

        var nameLabel = new Label
        {
            FontSize = 24,
            Text = speaker.Name
        };

        var titleLabel = new Label
        {
            TextColor = Color.Purple,
            Text = speaker.Title
        };

        var descriptionLabel = new Label
        {
            Text = speaker.Description
        };

        var stackLayout = new StackLayout
        {
            Spacing = 10
        };
        stackLayout.Children.Add(avatarImage);
        stackLayout.Children.Add(nameLabel);
        stackLayout.Children.Add(titleLabel);
        stackLayout.Children.Add(descriptionLabel);

        Title = speaker.Name;

        Padding = new Thickness(10);

        Content = new ScrollView 
        { 
            Content = stackLayout 
        };
    }

    async void HandleSpeakButtonClicked(object sender, EventArgs e)
    {
        await TextToSpeech.SpeakAsync(speaker.Description);
    }
}
```

2. In `DetailsPage.cs`, add a `Button` called `speakButton`

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;

        var avatarImage = new Image
        {
            HeightRequest = 200,
            WidthRequest = 200,
            Source = speaker.Avatar
        };

        var nameLabel = new Label
        {
            FontSize = 24,
            Text = speaker.Name
        };

        var titleLabel = new Label
        {
            TextColor = Color.Purple,
            Text = speaker.Title
        };

        var descriptionLabel = new Label
        {
            Text = speaker.Description
        };

        var speakButton = new Button
        {
            Text = "Speak",
        };
        speakButton.Clicked += HandleSpeakButtonClicked;

        var stackLayout = new StackLayout
        {
            Spacing = 10
        };
        stackLayout.Children.Add(avatarImage);
        stackLayout.Children.Add(nameLabel);
        stackLayout.Children.Add(titleLabel);
        stackLayout.Children.Add(descriptionLabel);

        Title = speaker.Name;

        Padding = new Thickness(10);

        Content = new ScrollView
        {
            Content = stackLayout
        };
    }

    async void HandleSpeakButtonClicked(object sender, EventArgs e)
    {
        await TextToSpeech.SpeakAsync(speaker.Description);
    }
}
```

3. In `DetailsPage.cs`, add `speakButton` to as a child to the `StackLayout`

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;

        var avatarImage = new Image
        {
            HeightRequest = 200,
            WidthRequest = 200,
            Source = speaker.Avatar
        };

        var nameLabel = new Label
        {
            FontSize = 24,
            Text = speaker.Name
        };

        var titleLabel = new Label
        {
            TextColor = Color.Purple,
            Text = speaker.Title
        };

        var descriptionLabel = new Label
        {
            Text = speaker.Description
        };

        var speakButton = new Button
        {
            Text = "Speak",
        };
        speakButton.Clicked += HandleSpeakButtonClicked;

        var stackLayout = new StackLayout
        {
            Spacing = 10
        };
        stackLayout.Children.Add(avatarImage);
        stackLayout.Children.Add(nameLabel);
        stackLayout.Children.Add(titleLabel);
        stackLayout.Children.Add(descriptionLabel);
        stackLayout.Children.Add(speakButton);

        Title = speaker.Name;

        Padding = new Thickness(10);

        Content = new ScrollView
        {
            Content = stackLayout
        };
    }

    async void HandleSpeakButtonClicked(object sender, EventArgs e)
    {
        await TextToSpeech.SpeakAsync(speaker.Description);
    }
}
```

### 14. Add Open Website Functionality

1. In `DetailsPage.cs`, add an event handler method called `HandleWebsiteButtonClicked.Clicked`:

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;

        var avatarImage = new Image
        {
            HeightRequest = 200,
            WidthRequest = 200,
            Source = speaker.Avatar
        };

        var nameLabel = new Label
        {
            FontSize = 24,
            Text = speaker.Name
        };

        var titleLabel = new Label
        {
            TextColor = Color.Purple,
            Text = speaker.Title
        };

        var descriptionLabel = new Label
        {
            Text = speaker.Description
        };

        var speakButton = new Button
        {
            Text = "Speak",
        };
        speakButton.Clicked += HandleSpeakButtonClicked;

        var stackLayout = new StackLayout
        {
            Spacing = 10
        };
        stackLayout.Children.Add(avatarImage);
        stackLayout.Children.Add(nameLabel);
        stackLayout.Children.Add(titleLabel);
        stackLayout.Children.Add(descriptionLabel);
        stackLayout.Children.Add(speakButton);

        Title = speaker.Name;

        Padding = new Thickness(10);

        Content = new ScrollView
        {
            Content = stackLayout
        };
    }

    async void HandleSpeakButtonClicked(object sender, EventArgs e)
    {
        await TextToSpeech.SpeakAsync(speaker.Description);
    }

    async void HandleWebsiteButtonClicked(object sender, EventArgs e)
    {
        if (speaker.Website.StartsWith("https"))
            await Browser.OpenAsync(speaker.Website);
    }
}
```

2. In `DetailsPage.cs`, in the constructor, add a `Button` called `websiteButton`

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;

        var avatarImage = new Image
        {
            HeightRequest = 200,
            WidthRequest = 200,
            Source = speaker.Avatar
        };

        var nameLabel = new Label
        {
            FontSize = 24,
            Text = speaker.Name
        };

        var titleLabel = new Label
        {
            TextColor = Color.Purple,
            Text = speaker.Title
        };

        var descriptionLabel = new Label
        {
            Text = speaker.Description
        };

        var speakButton = new Button
        {
            Text = "Speak",
        };
        speakButton.Clicked += HandleSpeakButtonClicked;

        var websiteButton = new Button
        {
            Text = "Go to Website"
        };
        websiteButton.Clicked += HandleWebsiteButtonClicked;

        var stackLayout = new StackLayout
        {
            Spacing = 10
        };
        stackLayout.Children.Add(avatarImage);
        stackLayout.Children.Add(nameLabel);
        stackLayout.Children.Add(titleLabel);
        stackLayout.Children.Add(descriptionLabel);
        stackLayout.Children.Add(speakButton);

        Title = speaker.Name;

        Padding = new Thickness(10);

        Content = new ScrollView
        {
            Content = stackLayout
        };
    }

    async void HandleSpeakButtonClicked(object sender, EventArgs e)
    {
        await TextToSpeech.SpeakAsync(speaker.Description);
    }

    async void HandleWebsiteButtonClicked(object sender, EventArgs e)
    {
        if (speaker.Website.StartsWith("https"))
            await Browser.OpenAsync(speaker.Website);
    }
}
```

3. In `DetailsPage.cs`, add `websiteButton` as a child to the `StackLayout`

```csharp
public class DetailsPage : ContentPage
{
    readonly Speaker speaker;

    public DetailsPage(Speaker item)
    {
        speaker = item;

        var avatarImage = new Image
        {
            HeightRequest = 200,
            WidthRequest = 200,
            Source = speaker.Avatar
        };

        var nameLabel = new Label
        {
            FontSize = 24,
            Text = speaker.Name
        };

        var titleLabel = new Label
        {
            TextColor = Color.Purple,
            Text = speaker.Title
        };

        var descriptionLabel = new Label
        {
            Text = speaker.Description
        };

        var speakButton = new Button
        {
            Text = "Speak",
        };
        speakButton.Clicked += HandleSpeakButtonClicked;

        var websiteButton = new Button
        {
            Text = "Go to Website"
        };
        websiteButton.Clicked += HandleWebsiteButtonClicked;

        var stackLayout = new StackLayout
        {
            Spacing = 10
        };
        stackLayout.Children.Add(avatarImage);
        stackLayout.Children.Add(nameLabel);
        stackLayout.Children.Add(titleLabel);
        stackLayout.Children.Add(descriptionLabel);
        stackLayout.Children.Add(speakButton);
        stackLayout.Children.Add(websiteButton);

        Title = speaker.Name;

        Padding = new Thickness(10);

        Content = new ScrollView
        {
            Content = stackLayout
        };
    }

    async void HandleSpeakButtonClicked(object sender, EventArgs e)
    {
        await TextToSpeech.SpeakAsync(speaker.Description);
    }

    async void HandleWebsiteButtonClicked(object sender, EventArgs e)
    {
        if (speaker.Website.StartsWith("https"))
            await Browser.OpenAsync(speaker.Website);
    }
}
```

### 15. Compile & Run
Now, we are ready to compile and run our application

## Azure Backend Walkthrough

Being able to grab data from a RESTful end point is great, but what about creating the back-end service? Let's update our application to use an Azure Functions back-end.

### 1. Write Code for Azure Functions Backend

1. In `Backend/DevDaysSpeakers.Function/GetSpeakersFunction.cs`, populate `GenerateSpeakers()` with the following `List<Speaker>`

    - Feel free to add yourself to the List

```csharp
static List<Speaker> GenerateSpeakers()
{
    return new List<Speaker>
    {
        new Speaker
        {
            Name = "Kim Noel",
            Description = "Kim is a co-organizer for Montreal Mobile .NET Developers",
            Title = "Community Engineer",
            Website = "https://www.linkedin.com/in/kimcodes/",
            Avatar = "https://pbs.twimg.com/profile_images/1095401068442386433/83JOBFoE_400x400.jpg"
        },
        new Speaker
        {
            Name = "Martijn van Dijk",
            Description = "Martijn is a Xamarin consultant at Xablu, Xamarin MVP, contributor of MvvmCross, and creator of several Xamarin plugins.",
            Title = "Xamarin Consultant",
            Website = "https://www.xablu.com/",
            Avatar = "https://pbs.twimg.com/profile_images/696643425706340353/QGsT4xLt_400x400.png",
        },
        new Speaker
        {
            Name = "Michael Stonis",
            Description = "Michael Stonis is a partner at Eight-Bot, a software consultancy in Chicago, where he focuses on mobile and integration solutions for enterprises using .NET. He loves mobile technology and how it has opened up our world in new and interesting ways that seemed like an impossibility just a few years ago. Outside of work, you will probably find him spending time with his family, brewing beer, or playing pinball.",
            Title = "President",
            Website = "https://www.eightbot.com/",
            Avatar = "https://pbs.twimg.com/profile_images/3544049213/c90b7bfed6c5cbc1067b7d13b4f6f0e6_400x400.png",
        },
        new Speaker
        {
            Name = "Kasey Uhlenhuth",
            Description = "Kasey Uhlenhuth is a program manager on the .NET Managed Languages team at Microsoft. She is currently working on modernizing the C# developer experience, but has also worked on C# scripting and the REPL. Before joining Microsoft, Kasey studied computer science and played varsity lacrosse at Harvard University. In her free time she can be found creating art, reading, or playing volleyball and ultimate frisbee.",
            Title = "Program Manager, .NET Managed Languages",
            Website = "https://microsoft.com/",
            Avatar = "https://pbs.twimg.com/profile_images/704473408638050304/bVbzez9X_400x400.jpg",
        },
        new Speaker
        {
            Name = "Santosh Hari",
            Description = "Santosh is an Azure MVP, Azure Consultant at NewSignature, President of Orlando Dot Net User Group and organizer of Orlando Code Camp.",
            Title = "Azure Consultant",
            Website = "http://santoshhari.wordpress.com/",
            Avatar = "https://pbs.twimg.com/profile_images/1108107477017493504/rKaK9ZPO_400x400.png",
        },
        new Speaker
        {
            Name = "Ana Betts",
            Description = "Ana is a developer at Slack who works on the Windows and Linux application. Previously she was at GitHub where she built the GitHub Desktop application on Windows, as well as the popular Xamarin libraries ReactiveUI, ModernHttpClient, and Akavache.",
            Title = "Engineer",
            Website = "https://slack.com/",
            Avatar = "https://pbs.twimg.com/profile_images/1119744877825105920/Sv7VY9rm_400x400.png",
        },
    };
}
```

2. In `GetSpeakersFunction.cs`, populate `Run` with the following code:

```csharp
[FunctionName(nameof(GetSpeakersFunction))]
public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, ILogger log)
{
    log.LogInformation("Generating Speakers");

    var speakersList = GenerateSpeakers();

    log.LogInformation("Returning Speaker List");

    return new OkObjectResult(speakersList);
}
```

### 2a. Publish Code to Azure Functions, Visual Studio PC

Let's publish the Azure Functions code we wrote to the Azure cloud.

The following steps are for Visual Studio on PC. If you are using Visual Studio for Mac, skip to **Step 2b**.

1. (If you don't yet have an Azure account) Create a Free Azure account including a free $200 credit by navigating to [this Azure Sign Up Page](https://azure.microsoft.com/free/services/mobile-apps?WT.mc_id=devdayslab-github-bramin) and creating an account.

2. In Visual Studio on PC, on the top-right menu bar, select **Sign in**
    - **Note:** If you have already signed in, skip this step

![Sign In](https://user-images.githubusercontent.com/13558917/59391534-68075f80-8d29-11e9-87aa-b7a210fd53bf.png)

3. On the **Sign in to your account** popup, sign into your Microsoft account
    - **Note:** If you have already signed in, skip this step

![sign in to your account](https://user-images.githubusercontent.com/13558917/59391535-68075f80-8d29-11e9-98b6-9c4326b177f3.png)

4. After signing in, on the top-right menu bar, select your profile icon

5. In the profile menu, select **Account settings...**

![Profile Settings](https://user-images.githubusercontent.com/13558917/59391526-66d63280-8d29-11e9-86f4-20b3c2a43cc2.png)

6. In the **Account Settings** window, under **All Accounts**, ensure that the account associated with your Azure Subscription has been added
    - Note: If your Azure Subscription acount is not visible, click **Add an account...**

![Account Settings](https://user-images.githubusercontent.com/13558917/59391525-66d63280-8d29-11e9-9491-f8b838dede67.png)

7. On the Visual **Studio Solution Explorer**, right-click **DevDaysSpeakers.Functions**

8. On the right-click menu, select **Publish...**

![Publish](https://user-images.githubusercontent.com/13558917/59391532-68075f80-8d29-11e9-856b-9f40ec0cee97.png)

9. In the **Pick a publish target** window, make the following selections:
    - **Publish Target**: Azure Function App
    - **Create New**

11. In the **Pick a publish target** window, select **Publish**

![Create New Function](https://user-images.githubusercontent.com/13558917/59391529-676ec900-8d29-11e9-8c43-7460afcf5b78.png)

12. In the **Azure App Service** window, make the following selections:
    - **Name**: [YourLastName]DevDaysSpeakersFunction
    - **Subscription**: [Select your subscription]
    - **Resource Group**: [Click **New...**]
        - **New resource group name**: DevDaysSpeakers
    - **Hosting Plan**: [Click **New...**]
        - **Hosting Plan**: DevDaysSpeakersFunctionHostingPlan
        - **Location**: [Choose the location closest to you]
        - **Size**: Consumption
    - **Azure Storage**: [Click **New...**]
        - **Account Name**: [YourLastName]devdayspeakers
        - **Location**: [Choose the location closest to you]
        - **Account Type**: Standard - Locally Redundant Storage

13. In the **Azure App Service** window, click **Create**

![Create New Azure App Service](https://user-images.githubusercontent.com/13558917/59960601-bacfdc80-94ca-11e9-9901-0fb8b78e484d.png)

14. Stand by while the Function deploys to Azure

![Deploying](https://user-images.githubusercontent.com/13558917/59960602-bacfdc80-94ca-11e9-9a88-b8864f62539a.png)

15. Once the deployment has completed, in the **Output** pad, ensure it says **Publish: 1 succeeded**

![Publish Succeeded](https://user-images.githubusercontent.com/13558917/59391533-68075f80-8d29-11e9-9c57-1728913de4b8.png)

16. Ensure Visual Studio has automatically launched a browser, confirming that the Azure Functions App is 

### 2b. Publish Code to Azure Functions, Visual Studio for Mac

Let's publish the Azure Functions code we wrote to the Azure cloud.

The following steps are for Visual Studio for Mac. If you are using Visual Studio on PC, continue to **Step 3**.

1. On Visual Studio for Mac, on the top menu bar, select **Visual Studio** > **Account**

![Account](https://user-images.githubusercontent.com/13558917/59393954-ace3c400-8d32-11e9-9b4f-50ea0b104618.png)

2. In the **Account** window, sign in with your Microsoft account
    - **Note:** If you have already signed in, skip this step

![Sign In](https://user-images.githubusercontent.com/13558917/59393961-ad7c5a80-8d32-11e9-9d4e-fec5df8decd8.png)

3. In the **Account** window, ensure that the account associated with your Azure Subscription has been added
    - Note: If your Azure Subscription account is not visible, click **Add an account...**

4. In the **Visual Studio for Mac Solution Explorer**, right-click **DevDaysSpeakers.Functions** > **Publish** > **Publish to Azure...**

![Right-click Publish](https://user-images.githubusercontent.com/13558917/59393959-ace3c400-8d32-11e9-92f7-a6523131be45.png)

5. In the **Publish to Azure App Service** window, select your Azure account

6. In the **Publish to Azure App Service** window, select **New**

![New Function](https://user-images.githubusercontent.com/13558917/59393957-ace3c400-8d32-11e9-9fcc-3c7f22e636e1.png)

7. In the **New App Service** window, enter the following:

    - **App Service Name**: [YourLastName]DevDaysSpeakersFunction
    - **Subscription**: [Select your Azure subscription]
    - **Resource Group**:
        - [Click the **+** icon]
        - DevDaysSpeakers
    - **Service Plan**: Custom
    - **Plan Name**: DevDaysSpeakersFunctionPlan
    - **Region**: [Choose the location closest to you]
    - **Pricing**: Consumption

8. In the **New App Service** window, click **Next**

![Create new App Service on Azure](https://user-images.githubusercontent.com/13558917/59393958-ace3c400-8d32-11e9-8c31-302d30b6bc57.png)

9. In the **Configure Storage Account** window, enter the following:
    - **Storage Account**: Custom
    - **Account Name**: [YourLastName]devdaysspeakers
    - **Account Type**: Standard - Locally Redundant Storage

10. In the **Configure Storage Account** window, click **Create**

![Create new App Service](https://user-images.githubusercontent.com/13558917/60039566-50c85a80-96b7-11e9-8816-ca6028223d76.png)

11. Stand by while the Function is published to Azure

12. If the **Update Functions Runtime on Azure** popup appears, click **Yes**

13. In the **Publish** pad, ensure it says **Publish Succeeded**

![Publish Succeeded](https://user-images.githubusercontent.com/13558917/59394342-24feb980-8d34-11e9-92c1-d7f383703964.png)
 

### 3. Get Azure Functions URL

1. In an internet browser, open the [Azure Portal](https://portal.azure.com?WT.mc_id=devdayslab-github-bramin)

2. In the Azure Portal, on the left menu, click on the Resource Group icon

![Azure Resource Group](https://user-images.githubusercontent.com/13558917/60040905-54111580-96ba-11e9-907a-7faff1741239.png)

3. In the Resource Group window, click on **DevDaysSpeakers**

![DevDaysSpeakers](https://user-images.githubusercontent.com/13558917/60040802-190ee200-96ba-11e9-99c6-3ee47699aecb.png)

4. In the DevDaysSpeakers Resource Group, click on **[YourLastName]DevDaysSpeakersFunctions**

![Speakers Functions](https://user-images.githubusercontent.com/13558917/60040808-190ee200-96ba-11e9-9630-869475777cbc.png)

5. In the Azure Functions window, select **GetSpeakersFunction** 

![GetSpeakersFunction](https://user-images.githubusercontent.com/13558917/60040805-190ee200-96ba-11e9-8502-9c49d0c8bb3f.png)

6. In the **GetSpeakersFunction** window, click **Get function URL**

![Get Function URL](https://user-images.githubusercontent.com/13558917/60040804-190ee200-96ba-11e9-8d13-a3dcd049d68f.png)

7. In the **Get function URL** popup, select **Copy**

![Copy Functions URL](https://user-images.githubusercontent.com/13558917/60040800-18764b80-96ba-11e9-8071-3546924b8e54.png)

### 4. Update SpeakersViewModel Logic to Use AzureService 

1. In Visual Studio, open **DevDaysSpeakers** > **Services** > **AzureService.cs**

2. In the **AzureService.cs** editor, set the value of `getSpeakersFunctionUrl` to the actual URL copied from the Azure Portal

3. Build/run the app
