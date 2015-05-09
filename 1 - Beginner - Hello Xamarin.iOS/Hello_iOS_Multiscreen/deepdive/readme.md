# Hello, iOS Multiscreen Deep Dive

In the Quickstart walkthrough, we built and ran our first multi-screen Xamarin.iOS application. Now it’s time to develop a 
deeper understanding of iOS navigation and architecture.

In this guide we introduce the *Model, View, Controller (MVC)* pattern and its role in iOS architecture and navigation. 
Then we dive into the Navigation Controller and learn to use it to provide a familiar navigation experience in iOS.
 
<a name="Model_View_Controller" class="injected"></a>
## Model View Controller (MVC)

In the [Hello, iOS](http://developer.xamarin.com/guides/ios/getting_started/hello,_iOS/) tutorial, we learned that iOS applications have only one *Window* that View Controllers are in charge of loading their *Content View Hierarchies* into the Window. In the second 
Phoneword walkthrough, we added a second screen to our application and passed some data – a list of phone numbers – 
between the two screens, as illustrated by the diagram below:

 [ ![](Images/08.png)](Images/08.png)

In our example, data was collected in the first screen, passed from the first View Controller to the second, and displayed by 
the second screen. This separation of screens, View Controllers, and data follows the *Model, View, Controller (MVC)* pattern. In 
the next few sections, we discuss the benefits of the pattern, its components, and how we use it in our Phoneword application.

### Benefits of the MVC Pattern

Model-View-Controller is a *design pattern* – a reusable architectural solution to a common problem or use case in code. MVC is an 
architecture for applications with a *Graphical User Interface (GUI)*. It assigns objects in the application one of three roles - 
the *Model* (data or application logic), the *View* (user interface), and the *Controller* (code behind). The diagram 
below illustrates the relationships between the three pieces of the MVC pattern and the user:

 [ ![](Images/00.png)](Images/00.png)

The MVC pattern is useful because it provides logical separation between different parts of a GUI application and makes it easier for us to 
reuse code and views. Let’s jump in and take a look at each of the three roles in more detail.

<div class="note"><strong>Note</strong>: The MVC pattern is loosely analogous to the structure of ASP.NET pages or WPF applications. In these examples, 
the View is the component that is actually responsible for describing the UI and corresponds to the ASPX (HTML) page in ASP.NET or to XAML in a WPF 
application. The Controller is the component that is responsible for managing the View, which corresponds to the code-behind in ASP.NET or WPF.
</div>

### Model

The Model object is typically an application-specific representation of data that is to be displayed or entered into View. The Model is often 
loosely defined - for example, in our <span class="uiitem">Phoneword_iOS</span> app, the list of phone numbers (represented as a list of strings) is the Model. If we were 
building a cross-platform application, we could choose to share the <span class="uiitem">PhonewordTranslator</span> code between our iOS and Android applications. We could 
think of that shared code as the Model as well.

MVC is completely agnostic of the *data persistence* and *access* of the Model. In other words, MVC doesn’t care what our data looks like or how it’s stored, 
only how the data is *represented*. For example, we could choose to store our data in a SQL database, or persist it in some cloud storage mechanism, or simply use a `List<string>`. For MVC purposes, only the data representation itself is included in the pattern.

In some cases, the Model portion of the MVC may be empty. For example, we might choose to add some static pages to our app explaining how the phone translator 
works, why we built it, and how to get in touch with us to report bugs. These app screens would still be created using Views and Controllers, but they would 
not have any real Model data.

<div class="note"><strong>Note</strong>: In some literature, the Model portion of the MVC pattern can refer to the entire application backend, not just 
the data that is displayed on the UI. In this guide we use a modern interpretation of the Model, but the distinction is not particularly important.
</div>

### View

A View is the component that’s responsible for rendering the user interface. In nearly all platforms that use the MVC pattern, the user interface is composed of a hierarchy of views. We can think of a View in MVC as a view hierarchy with a single view – known as the root view - at the top of the hierarchy and any number of child views (known as or subviews) below it. In iOS, a screen’s Content View Hierarchy corresponds to the View component in MVC.

### Controller

The Controller object is the component that wires everything together and is represented in iOS by the `UIViewController`. We can think of the Controller as 
the backing code for a screen or a set of views. The Controller is responsible for listening for requests from the user and returning the appropriate view 
hierarchy. It listens to requests from the View (button clicks, text input, etc.) and performs the appropriate processing, View modification, and reloading 
of the View. The Controller is also responsible for creating or retrieving the Model from whatever backing data store exists in the application, and 
populating the View with its data.

Controllers can also manage other Controllers. For example, one Controller might load another Controller if it needs to display a different screen, or manage a stack 
of Controllers to monitor their order and the transitions between them. In the next section, we’ll see an example of a Controller that manages other Controllers as we 
introduce a special type of iOS View Controller called a *Navigation Controller*.

## Navigation Controller

In the Phoneword application, we used a *Navigation Controller* to help manage navigation between multiple screens. The Navigation Controller is a 
specialized `UIViewController` represented by the `UINavigationController` class. Instead of managing a single Content View Hierarchy, the 
Navigation Controller manages other View Controllers, as well as its own special Content View Hierarchy in the form of a navigation 
toolbar that includes a title, back button, and other optional features.

The Navigation Controller is common in iOS applications and provides navigation for staple iOS applications like the <span class="uiitem">Settings</span> app, 
as illustrated by the screenshot below:

 [ ![](Images/01.png)](Images/01.png)

The Navigation Controller serves three primary functions:

-  **Provides Hooks for Forward Navigation** – The Navigation Controller uses a hierarchal navigation metaphor where Content View Hierarchies are  *pushed* onto a  *navigation stack* . You can think of a navigation stack as a stack of playing cards, in which only the top most card is visible, as illustrated by the diagram below:  [ ![](Images/02.png)](Images/02.png)

 
-  **Optionally Provides a Back Button** - When we push a new item onto the navigation stack, the title bar can automatically display a  *back button* that allows the user to navigate backwards. Pressing the back button  *pops* the current View Controller off the navigation stack, and loads the previous Content View Hierarchy into the Window:  [ ![](Images/03.png)](Images/03.png)

 
-  **Provides a Title Bar** – The top portion of the  <span class="uiitem">Navigation Controller</span> is called the  *Title Bar* . It’s responsible for displaying the View Controller title, as illustrated by the diagram below:  [ ![](Images/04.png)](Images/04.png)

 


### Root View Controller

A <span class="uiitem">Navigation Controller</span> doesn’t manage a Content View Hierarchy, so it has nothing to display on its own. 
Instead, a <span class="uiitem">Navigation Controller</span> is paired with a *Root View Controller*:

 [ ![](Images/05.png)](Images/05.png)

The Root View Controller represents the first View Controller in the <span class="uiitem">Navigation Controller’s</span> stack, and the Root View Controller’s 
Content View Hierarchy is the first Content View Hierarchy to be loaded into the Window. If we want to put our entire application on the 
Navigation Controller’s stack, we can move the Sourceless Segue to the <span class="uiitem">Navigation Controller</span> and set our first screen’s View Controller as the 
Root View Controller, like we did in the Phoneword app:

 [ ![](Images/06.png)](Images/06.png)

### Additional Navigation Options

The <span class="uiitem">Navigation Controller</span> is a common way of handling navigation in iOS, but it is not the only option. A [Tab Bar Controller](http://developer.xamarin.com/guides/ios/user_interface/creating_tabbed_applications) 
can split an application into different functional areas; a [Split View Controller](http://developer.xamarin.com/recipes/ios/content_controls/split_view/use_split_view_to_show_two_controllers) can create Master/Detail Views; 
and a [Flyout Navigation Controller](http://components.xamarin.com/view/flyoutnavigation) creates navigation that the user can swipe in from the side. All of these can be 
combined with a <span class="uiitem">Navigation Controller</span> for an intuitive way to present content.

## Handling Transitions

In the Phoneword walkthrough, we handled the transition between the two View Controllers in two different ways – first with a Storyboard Segue and 
then programmatically. Let’s explore both these options in more detail.

### PrepareForSegue

When we add a Segue with a <span class="uiitem">Show</span> action to the Storyboard, we instruct iOS to push the second View Controller onto the 
Navigation Controller’s stack:

 [ ![](Images/09.png)](Images/09.png)

Adding a Segue to the Storyboard is enough to create a simple transition between screens. If we want to pass data between View Controllers, 
we have to override the `PrepareForSegue` method and handle the data ourselves:

```
public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
{
    base.PrepareForSegue (segue, sender);
    ...
}
```

iOS calls `PrepareForSegue` right before the transition occurs and passes the Segue that we created in the Storyboard into the method. 
At this point, we have to manually set the Segue’s destination View Controller. The following code gets a handle to the Destination View Controller and casts it to the proper class - CallHistoryController, in this case:

```
CallHistoryController callHistoryContoller = segue.DestinationViewController as CallHistoryController;
```

Finally, we pass the list of phone numbers (the Model) from the `ViewController` to the `CallHistoryController` by setting 
the `PhoneHistory` property of the `CallHistoryController` to the list of dialed phone numbers:

```
callHistoryContoller.PhoneNumbers = PhoneNumbers;
```

The complete code for passing data using a Segue is as follows:

```
public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
{
    base.PrepareForSegue (segue, sender);
        
    var callHistoryContoller = segue.DestinationViewController as CallHistoryController;
        
    if (callHistoryContoller != null) {
         callHistoryContoller.PhoneNumbers = PhoneNumbers;
    }
 }
```

### Navigation Without Segues

Transitioning from the first View Controller to the second in code is the same process as with a Segue, but several steps have to be done manually. 
First, we use `this.NavigationController` to get a reference to the Navigation Controller whose stack we are currently on. Then, we use the Navigation 
Controller’s `PushViewController` method to manually push the next View Controller onto the stack, passing in the View Controller and an option to animate 
the transition (we set this to `true`).

The following code handles the transition from the Phoneword screen to the Call History screen:

```
this.NavigationController.ShowViewController (callHistory, true);
```

Before we can transition to the next View Controller, we have to instantiate it manually from the Storyboard by calling `this.Storyboard.InstantiateViewController` 
and passing in the Storyboard ID of the `CallHistoryController`:

```
CallHistoryController callHistory = 
this.Storyboard.InstantiateViewController 
("CallHistoryController") as CallHistoryController;
```

Finally, we pass the list of phone numbers (the Model) from the `ViewController` to the `CallHistoryController` by setting the `PhoneHistory` 
property of the `CallHistoryController` to the list of dialed phone numbers, just like we did when we handled the transition with a Segue:

```
callHistory.PhoneNumbers = PhoneNumbers;
```

The complete code for the programmatic transition is as follows:

```
CallHistoryButton.TouchUpInside += (object sender, EventArgs e) => {
    // Launches a new instance of CallHistoryController
    CallHistoryController callHistory = this.Storyboard.InstantiateView   Controller ("CallHistoryController") as CallHistoryController;
    if (callHistory != null) {
callHistory.PhoneNumbers = PhoneNumbers;
     this.NavigationController.ShowViewController (callHistory, true);
    }
};
```

## Additional Concepts Introduced in Phoneword

The Phoneword application introduced several concepts not covered in this guide. These concepts include:

-  **Automatic Creation of View Controllers** – When we enter a class name for the View Controller in the  <span class="uiitem">Properties Pad</span> , the iOS designer checks if that class exists and then generates the View Controller backing class for us. For more information on this and other iOS designer features, refer to the  [Introduction to the iOS Designer](http://developer.xamarin.com/guides/ios/user_interface/designer/introduction) guide. 
-  **Table View Controller** – The  `CallHistoryController` is a Table View Controller. A Table View Controller contains a Table View, the most common layout and data display tool in iOS. Tables are beyond the scope of this guide. For more information on Table View Controllers, please refer to the  [Working with Tables and Cells](/guides/ios/user_interface/tables) guide. 
-   **Storyboard ID** – Setting the Storyboard ID creates a View Controller class in Objective-C containing the code-behind for the View Controller in the Storyboard. We use the Storyboard ID to find the Objective-C class and instantiate the View Controller in the Storyboard. For more information on Storyboard IDs, please refer to the  [Introduction to Storyboards](http://developer.xamarin.com/guides/ios/user_interface/introduction_to_storyboards) guide. 


## Summary

Congratulations, you’ve completed your first multi-screen iOS application!

In this guide we introduced the MVC pattern and used it to create a multi-screened application. We also explored Navigation Controllers and their 
role in powering iOS navigation. You now have the solid foundation you need to start developing your own Xamarin.iOS applications.

Next, let’s learn to build cross-platform applications with Xamarin with the [Introduction to Mobile Development](http://developer.xamarin.com/guides/cross-platform/getting_started/introduction_to_mobile_development/) and [Building Cross-Platform Applications](http://developer.xamarin.com/guides/cross-platform/application_fundamentals/building_cross_platform_applications/) guides.
