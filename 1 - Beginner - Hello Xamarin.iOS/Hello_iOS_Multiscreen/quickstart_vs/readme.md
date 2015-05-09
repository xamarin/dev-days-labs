id:{d72e6230-c9ee-4bee-90ec-877d256821aa}  
title:Hello, iOS Multiscreen  
subtitle:Handling Navigation with Xamarin.iOS  
brief:In this two-part guide, we expand the Phoneword application we created in the Hello, iOS guide to handle a second screen. Along the way we’ll introduce the Model-View-Controller design pattern, implement our first iOS navigation, and develop a deeper understanding of iOS application structure and functionality.  
samplecode:[Hello, iOS](/samples/monotouch/Hello_iOS/)  
sdk:[iOS Human Interface Guidelines](http://developer.apple.com/library/ios/#documentation/UserExperience/Conceptual/MobileHIG/Introduction/Introduction.html)  
sdk:[iOS Provisioning Portal](https://developer.apple.com/ios/manage/overview/index.action)  

# Hello.iOS Multiscreen Quickstart

In the walkthrough part of this guide we’ll add a second screen to our Phoneword application to keep track of the history of numbers called using our app. The final application will have a second screen that displays the call history, as illustrated by the following screenshot:

 [ ![](Images/00.png)](Images/00.png)

In the [accompanying Deep Dive](http://developer.xamarin.com/guides/ios/getting_started/hello,_iOS_multiscreen/hello,_iOS_multiscreen_deepdive/), we’ll review what we’ve built and discuss architecture, navigation, and other new iOS concepts that we encounter along the way. Let’s get started!

 <a name="Requirements" class="injected"></a>


# Requirements

This guide resumes where the Hello, iOS document left off, and requires completion of the [Hello, iOS Quickstart](http://developer.xamarin.com/guides/ios/getting_started/hello,_iOS/). If you’d like to
jump directly into the Hello, iOS Multiscreen Quickstart, you can download the completed version of the Phoneword app from the [Hello, iOS sample](http://developer.xamarin.com/samples/monotouch/Hello_iOS/).


<ide name="vs">
<h1>Walkthrough – Navigation with Segues</h1>

<p>In this walkthrough we’ll add a Call History screen to our <b>Phoneword</b> application.</p>

<ol>

<li><p>Let’s open the <b>Phoneword</b> application in Visual Studio. If you haven’t completed the <a href="http://developer.xamarin.com/guides/ios/getting_started/hello,_iOS/">Hello, iOS walkthrough</a>, you can
<a href="http://developer.xamarin.com/guides/ios/getting_started/hello,_iOS/hello,_iOS_multiscreen_quickstart/Resources/Phoneword_iOS_Android_Multiscreen.zip">download the completed Phoneword application here</a>:</p>
<p><a href="Images/vs01.png" class=" fancybox"><img src="Images/vs01.png"></a></p>
</li>

<li><p>Let’s start by editing the user interface. Open the <span class="uiitem">MainStoryboard.storyboard</span> file from the <span class="uiitem">Solution Explorer</span>:</p>
<p><a href="Images/vs02.png" class=" fancybox"><img src="Images/vs02.png"></a></p>
</li>

<li><p>Next, let’s drag a <span class="uiitem">Navigation Controller</span> from the <span class="uiitem">Toolbox</span> onto the design surface:</p>
<p><a href="Images/vs03.png" class=" fancybox"><img src="Images/vs03.png"></a></p>
</li>

<li><p>Drag the <span class="uiitem">Sourceless Segue</span> (that’s the gray arrow to the left of the <span class="uiitem">Phoneword</span> Scene) from the <span class="uiitem">Phoneword</span> Scene
to the <span class="uiitem">Navigation Controller</span> to change the starting point of the application:</p>
<p><a href="Images/vs04.png" class=" fancybox"><img src="Images/vs04.png"></a></p>
</li>

<li><p>Select the <span class="uiitem">Root View Controller</span> by clicking on the black bar, and press <code>Delete</code> to remove it from the design surface. </p>
<p>Then, let’s move the <span class="uiitem">Phoneword</span> Scene next to the <span class="uiitem">Navigation Controller</span>:</p>
<p><a href="Images/vs05.png" class=" fancybox"><img src="Images/vs05.png"></a></p>
</li>

<li><p>Next, let’s set the <span class="uiitem">ViewController</span> as the Navigation Controller’s <code>Root View Controller</code>. Press down the
<code>Ctrl</code> key and click inside the <span class="uiitem">Navigation Controller</span>. A blue line should appear. Then, still holding down the <code>Ctrl</code>
 key, drag from the <span class="uiitem">Navigation Controller</span> to the <span class="uiitem">Phoneword</span> Scene and release. This is called <code>Ctrl-dragging</code>:</p>
<p><a href="Images/vs06.png" class=" fancybox"><img src="Images/vs06.png"></a></p>
</li>

<li><p>From the popover, let’s set the relationship to <span class="uiitem">Root</span>:</p>
<p><a href="Images/vs07.png" class=" fancybox"><img src="Images/vs07.png"></a></p>

<p><p>The <span class="uiitem">ViewController</span> is now our <span class="uiitem">Navigation Controller’s Root View Controller:</span></p>
<p><a href="Images/vs08.png" class=" fancybox"><img src="Images/vs08.png"></a></p>
</li>

<li><p>Double-click on the <span class="uiitem">Phoneword</span> screen’s <span class="uiitem">Title</span> bar and change the <span class="uiitem">Title</span> to "<code>Phoneword</code>":</p>
<p><a href="Images/vs09.png" class=" fancybox"><img src="Images/vs09.png"></a></p>
</li>

<li><p>Let’s drag a <span class="uiitem">Button</span> from the <span class="uiitem">Toolbox</span> and place it under the <span class="uiitem">Call Button</span>. Drag the
handles to make the new <span class="uiitem">Button</span> the same width as the <span class="uiitem">Call Button</span>:</p>
<p><a href="Images/vs10.png" class=" fancybox"><img src="Images/vs10.png"></a></p>
</li>

<li><p>In the <span class="uiitem">Properties Explorer</span>, change the <span class="uiitem">Name</span> of the <span class="uiitem">Button</span> to <code>CallHistoryButton</code>
 and change the <span class="uiitem">Title</span> to "<code>Call History</code>":</p>
<p><a href="Images/vs11.png" class=" fancybox"><img src="Images/vs11.png"></a></p>
</li>

<li><p>Next, let’s create the <span class="uiitem">Call History</span> screen. From the <span class="uiitem">Toolbox</span>, drag a <span class="uiitem">Table View Controller</span>
 onto the design surface:</p>
<p><a href="Images/vs12.png" class=" fancybox"><img src="Images/vs12.png"></a></p>
</li>

<li><p>Next, select the <span class="uiitem">Table View Controller</span> by clicking on the black bar at the bottom of the Scene. In the <span class="uiitem">Properties Explorer</span>,
change the <span class="uiitem">Table View Controller’s</span> class to <code>CallHistoryController</code> and press <code>Enter</code>:</p>
<p><a href="Images/vs13.png" class=" fancybox"><img src="Images/vs13.png"></a></p>

<p>The iOS Designer will generate a custom backing class called <code>CallHistoryController</code> to manage this screen’s Content View Hierarchy.
The <span class="uiitem">CallHistoryController.cs</span> file will appear in the <span class="uiitem">Solution Explorer</span>:</p>
<p><a href="Images/vs14.png" class=" fancybox"><img src="Images/vs14.png"></a></p>
</li>

<li><p>Let’s double-click on the <span class="uiitem">CallHistoryController.cs</span> file to open it and replace the contents with the following code:</p>
<pre><code class=" syntax brush-C#">using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace Phoneword_iOS
{
    public partial class CallHistoryController : UITableViewController
    {
        public List&lt;String&gt; PhoneNumbers { get; set; }

        static NSString callHistoryCellId = new NSString ("CallHistoryCell");

        public CallHistoryController (IntPtr handle) : base (handle)
        {
            TableView.RegisterClassForCellReuse (typeof(UITableViewCell), callHistoryCellId);
            TableView.Source = new CallHistoryDataSource (this);
            PhoneNumbers = new List&lt;string&gt; ();
        }

        class CallHistoryDataSource : UITableViewSource
        {
            CallHistoryController controller;

            public CallHistoryDataSource (CallHistoryController controller)
            {
                this.controller = controller;
            }

            // Returns the number of rows in each section of the table
            public override nint RowsInSection (UITableView tableView, nint section)
            {
                return controller.PhoneNumbers.Count;
            }
            //
            // Returns a table cell for the row indicated by row property of the NSIndexPath
            // This method is called multiple times to populate each row of the table.
            // The method automatically uses cells that have scrolled off the screen or creates new ones as necessary.
            //
            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell (CallHistoryController.callHistoryCellId);

                int row = indexPath.Row;
                cell.TextLabel.Text = controller.PhoneNumbers [row];
                return cell;
            }
        }
    }
}</code></pre>

<p>Save the application and build it to ensure there are no errors.</p>
</li>

<li><p>Let’s create a <code>Segue</code> (transition) between the <span class="uiitem">Phoneword</span> Scene and the <span class="uiitem">Call History</span> Scene.
In the <span class="uiitem">Phoneword Scene</span>, select the <span class="uiitem">Call History Button</span> and <code>Ctrl-drag</code> from the
<span class="uiitem">Button</span> to the <span class="uiitem">Call History</span> Scene:</p>
<p><a href="Images/vs15.png" class=" fancybox"><img src="Images/vs15.png"></a></p>

<p>From the <span class="uiitem">Action Segue</span> popover, select <span class="uiitem">Push</span>:</p>
<p><a href="Images/vs16.png" class=" fancybox"><img src="Images/vs16.png"></a></p>

<p>The iOS Designer will add a Segue between the two Scenes:</p>
<p><a href="Images/vs17.png" class=" fancybox"><img src="Images/vs17.png"></a></p>
</li>

<li><p>Let’s add a <span class="uiitem">Title</span> to the <span class="uiitem">Table View Controller</span> by selecting the black bar at the bottom of the Scene
and changing the <span class="uiitem">View Controller Title</span> to "<code>Call History</code>"" in the <span class="uiitem">Properties Explorer</span>:</p>
<p><a href="Images/vs18.png" class=" fancybox"><img src="Images/vs18.png"></a></p>
</li>

<li><p>If we run the application now, the <span class="uiitem">Call History Button</span> will open the <span class="uiitem">Call History</span> screen,
but the <code>Table View</code> will be empty because we have no code to keep track of phone numbers. Let’s add that functionality.</p>

<p>First, we need a way to store dialed phone numbers. We’re going to store it as a list of strings called <code>PhoneNumbers</code>.</p>

<p>To support lists, let’s add <code>System.Collections.Generic</code> to our <code>using directives</code> at the top of the file:</p>

<pre><code class=" syntax brush-C#">using System.Collections.Generic;</code></pre>
</li>

<li>Next, modify the <code>ViewController</code> class with the following code:

<pre><code class=" syntax brush-C#">namespace Phoneword_iOS
{
    public partial class ViewController : UIViewController
    {
        // translatedNumber was moved here from ViewDidLoad ()
        string translatedNumber = "";

        public List&lt;String&gt; PhoneNumbers { get; set; }

        public ViewController (IntPtr handle) : base (handle)
        {
            //initialize list of phone numbers called for Call History screen
            PhoneNumbers = new List&lt;String&gt; ();

        }
        // ViewDidLoad, etc...
	}
}</code></pre>

<p>Note that we’ve also moved <code>translatedNumber</code> from inside the <code>ViewDidLoad</code> method to a <code>class-level variable</code>.</p>
</li>

<li><p>Next, let’s modify our <code>CallButton</code> code to add dialed numbers to the list of phone numbers by calling
<code>PhoneNumbers.Add(translatedNumber)</code>. The full code will look like this:</p>

<pre><code class=" syntax brush-C#">// On "Call" button-up, try to dial a phone number
CallButton.TouchUpInside += (object sender, EventArgs e) =&gt; {

    //Store the phone number that we're dialing in PhoneNumbers
    PhoneNumbers.Add (translatedNumber);

    var url = new NSUrl ("tel:" + translatedNumber);

    if (!UIApplication.SharedApplication.OpenUrl (url)) {
        var av = new UIAlertView ("Not supported",
                 "Scheme 'tel:' is not supported on this device",
                  null,
                  "OK",
                  null);
        av.Show ();
    }
 };</code></pre>
</li>

<li><p>Finally, let’s add the following method to the <code>ViewController</code> class. This will go somewhere below <code>ViewDidLoad</code>:</p>

<pre><code class=" syntax brush-C#">public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
  {
     base.PrepareForSegue (segue, sender);

     // set the View Controller that’s powering the screen we’re
     // transitioning to

     var callHistoryContoller = segue.DestinationViewController as CallHistoryController;

     //set the Table View Controller’s list of phone numbers to the
     // list of dialed phone numbers

     if (callHistoryContoller != null) {
        callHistoryContoller.PhoneNumbers = PhoneNumbers;
     }
   }</code></pre>

<p>Save and build the application to make sure there are no errors.</p>
</li>

<li><p>Press the <span class="uiitem">Start</span> button to launch our application inside the <span class="uiitem">iOS Simulator</span>:</p>
<p><a href="Images/21.png" class=" fancybox"><img src="Images/21.png"></a>
<a href="Images/22.png" class=" fancybox"><img src="Images/22.png"></a></p>
</li>

</ol>

<p>Congratulations on completing your first multi-screen Xamarin.iOS application!</p>

<a name="Walkthrough_–_Code-Only_Navigation" class="injected"></a>
<h1>Walkthrough - Code-Only Navigation</h1>

<p>In the walkthrough above, we created navigation between the <span class="uiitem">Phoneword</span> and <span class="uiitem">Call History</span>
screens using a Storyboard Segue. Segues are great at handling simple navigation that requires moving from one screen to the next. Segues are not
good at handling more complex navigation – for example, conditional navigation when the second screen may be different depending on the user’s
behavior on the first screen. In this walkthrough, we’ll learn to handle navigation between screens programmatically.</p>

<ol>

<li><p>Let’s the <span class="uiitem">MainStoryboard.storyboard</span> file and remove the Segue between the <span class="uiitem">Phoneword</span>
Scene and the <span class="uiitem">Call History</span> Scene by selecting the Segue and pressing <span class="uiitem">Delete</span>:</p>
<p><a href="Images/vs19.png" class=" fancybox"><img src="Images/vs19.png"></a></p>

<p>This will remove the <span class="uiitem">Title Bar</span> and <span class="uiitem">Back Button</span> from the Call History screen.</p>
</li>

<li><p>Select the <code>CallHistoryController</code> by clicking on the black bar at the bottom of the Scene. In the <span class="uiitem">Properties Explorer</span>,
let’s set this View Controller’s <span class="uiitem">Storyboard ID</span> to <code>CallHistoryController</code>:</p>
<p><a href="Images/vs20.png" class=" fancybox"><img src="Images/vs20.png"></a></p>
</li>

<li><p>Next, in the <code>ViewController</code> class, let’s remove the <code>PrepareForSegue</code> method we added in the previous walkthrough.
We can comment out the entire method it by selecting the code, clicking the <span class="uiitem">Edit</span> Menu and choosing <span class="uiitem">Advanced</span> > <span class="uiitem">Comment Selection</span>:</p>
<p><a href="Images/vs21.png" class=" fancybox"><img src="Images/vs21.png"></a></p>
</li>

<li><p>Add the following code inside the <code>ViewDidLoad</code> method. This wires up the <code>CallHistoryButton</code> to perform the navigation to the next screen:</p>

<pre><code class=" syntax brush-C#">CallHistoryButton.TouchUpInside += (object sender, EventArgs e) =&gt;{
   // Launches a new instance of CallHistoryController
   CallHistoryController callHistory = this.Storyboard.InstantiateViewController ("CallHistoryController") as CallHistoryController;
   if (callHistory != null) {
      callHistory.PhoneNumbers = PhoneNumbers;
   this.NavigationController.PushViewController (callHistory, true);
   }
};</code></pre>

<p>Save and build the application to make sure there are no errors.</p>
</li>

<li><p>Press the <span class="uiitem">Start</span> button to launch our application inside the <span class="uiitem">iOS Simulator</span>. It should look
and behave exactly the same as the previous version:</p>
<p><a href="Images/21.png" class=" fancybox"><img src="Images/21.png"></a>
<a href="Images/22.png" class=" fancybox"><img src="Images/22.png"></a></p>
</li>

</ol>
</ide>

Our app can now handle navigation using both Storyboard Segues and in code. Now it’s time to dissect the tools and skills we just
learned in the [Hello, iOS Multiscreen Deep Dive](http://developer.xamarin.com/guides/ios/getting_started/hello,_iOS_multiscreen/hello,_iOS_multiscreen_deepdive/).