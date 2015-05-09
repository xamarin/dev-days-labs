# Hello, Android Quickstart

In this walkthrough, we create an application that translates an
alphanumeric phone number entered by the user into a numeric phone
number, and then calls that number. The final application looks like
this:

[ ![](Images/intro-app-examples-sml.png)](Images/intro-app-examples.png)

Let's get started!

# Requirements

Xamarin.Android works with any of the following setups:

-  Latest version of Xamarin Studio on OS X Mountain Lion and above.
-  Latest version of Xamarin Studio on Windows 7 and above.
-  Windows 7 and above with Visual Studio 2010 Professional or higher.

This walkthrough assumes that the latest version of Xamarin.Android is 
installed and running on your platform of choice. For a guide to 
installing Xamarin.Android, refer to the 
[Xamarin.Android Installation](http://developer.xamarin.com/guides/android/getting_started/installation/) guides. 
Before we get started, please download and unzip the 
[Xamarin App Icons & Launch Screens](http://developer.xamarin.com/guides/android/getting_started/hello,android/Resources/XamarinAppIconsAndLaunchImages.zip) 
set. 



# Configuring Emulators


Android has several options for emulators. The standard Android 
emulator is the simplest to set up but runs slowly. Xamarin recommends 
that you use the high-performance 
[Xamarin Android Player](http://developer.xamarin.com/guides/android/getting_started/installation/android-player). 
If you are not using the Xamarin Android Player, you should configure 
your emulator to use hardware acceleration. Instructions for 
configuring hardware acceleration are available in the 
[Accelerating Android Emulators](/guides/android/getting_started/installation/accelerating_android_emulators) 
guide. 

<a name="Walkthrough" class="injected"></a>

# Walkthrough

<ol>
<ide name="xs">

    <li>
      <p>
        Let's launch Xamarin Studio from the <span class="uiitem">Applications</span>
        folder or from <span class="uiitem">Spotlight</span>. This opens the start
        page:
      </p>
      <p>
        <a href="Images/xs/01-launch.png" class=" fancybox"><img src=
                "Images/xs/01-launch-sml.png"></a>
      </p>

    </li>
    <li>
      <p>
        Click <span class="uiitem">New Solution...</span> to create a new project:
      </p>
      <p>
        <img src="Images/xs/02-new-solution.png">
      </p>
    </li>
    <li>
      <p>
        In the <span class="uiitem">Choose a template for your new project</span> dialog, 
        let's click <span class="uiitem">Android &gt; App</span> and select the 
        <span class="uiitem">Android App</span> template. Click <span class="uiitem">Next</span>.
      </p>
      <p>
        <a href="Images/xs/03-choose-template.png" class=" fancybox"><img src=
                "Images/xs/03-choose-template-sml.png"></a>
      </p>
    </li>
    <li>
      <p>
        In the <span class="uiitem">Configure your Android app</span> dialog, 
        we'll name the new app <code>Phoneword</code> and click <span class="uiitem">Next</span>:
      </p>
      <p>
        <a href="Images/xs/04-configure-app.png" class=" fancybox"><img src=
                "Images/xs/04-configure-app-sml.png"></a>
      </p>
    </li>

    <li>
      <p>
        In the <span class="uiitem">Configure your new project</span> dialog, 
        we'll leave the Solution and Project names set to <code>Phoneword</code>
        and click <span class="uiitem">Create</span> to create the project:
      </p>
      <p>
        <a href="Images/xs/05-configure-project.png" class=" fancybox"><img src=
                "Images/xs/05-configure-project-sml.png"></a>
      </p>
    </li>
</ide>

<ide name="xs">

    <li>
      <p>
        After the new project is created, let&lsquo;s expand the <span class=
        "uiitem">Resources</span> folder and then the <span class="uiitem">layout</span>
        folder in the <span class="uiitem">Solution</span> pad. Double-click
        <b>Main.axml</b> to open it in the Android Designer. This is the layout file for
        our screen:
      </p>
      <p>
        <a href="Images/xs/06-open-layout.png" class=" fancybox"><img src=
                "Images/xs/06-open-layout-sml.png"></a>
      </p>
    </li>
</ide>

<ide name="xs">

    <li>
      <p>
        Let&lsquo;s select the <b>Hello World, Click Me!</b> <span class=
        "uiitem">Button</span> on the design surface and press the <kbd>Delete</kbd> key to
        remove it. From the <span class="uiitem">Toolbox</span> (the area on the right),
        enter <code>text</code> into the search field and drag a <span class="uiitem">Text
        (Large)</span> widget onto the design surface (the area in the center):
      </p>
      <p>
        <a href="Images/xs/07-large-text.png" class=" fancybox"><img src=
                "Images/xs/07-large-text-sml.png"></a>
      </p>
    </li>

</ide>
    
<ide name="xs">

    <li>
      <p>
        With the <span class="uiitem">Text (Large)</span> widget selected on the design
        surface, we can use the <span class="uiitem">Properties</span> pad to change the
        <code>Text</code> property of the <span class="uiitem">Text
        (Large)</span> widget to <code>Enter a Phoneword:</code> as seen below:
      </p>
      <p>
        <a href="Images/xs/08-enter-a-phoneword.png" class=" fancybox"><img src=
                "Images/xs/08-enter-a-phoneword-sml.png"></a>
      </p>
      <div class="note">
        <p>
          <strong>Note</strong>: You can bring up the 
          <span class="uiitem">Properties</span> pad or 
          <span class="uiitem">Toolbox</span> at any time by navigating to
          <span class="uiitem">View &gt; Pads</span>.
        </p>
      </div>
    </li>
</ide>

<li>

  <p>
    Next, let&lsquo;s drag a <span class="uiitem">Plain Text</span> widget from the
    <span class="uiitem">Toolbox</span> to the design surface and place it underneath the
    <span class="uiitem">Text (Large)</span> widget. Notice that we can use the search
    field to help locate widgets by name:
  </p>

<ide name="xs">

  <p>
    <a href="Images/xs/09-plain-text.png" class="fancybox"><img src=
            "Images/xs/09-plain-text-sml.png"></a>
  </p>
</ide>
</li>

<ide name="xs">

    <li>
      <p>
        With the <span class="uiitem">Plain Text</span> widget selected on the design
        surface, we can use the <span class="uiitem">Properties</span> pad to change the
        <code>Id</code> property of the <span class="uiitem">Plain
        Text</span> widget to <code>@+id/PhoneNumberText</code> and change the 
        <code>Text</code> property to <code>1-855-XAMARIN</code>:
      </p>
      <p>
        <a href="Images/xs/10-add-properties.png" class=" fancybox"><img src=
                "Images/xs/10-add-properties-sml.png"></a>
      </p>
    </li>
</ide>

<li>

  <p>
    Let&lsquo;s drag a <span class="uiitem">Button</span> from the <span class=
    "uiitem">Toolbox</span> to the design surface and place it underneath the <span class=
    "uiitem">Plain Text</span> widget:
  </p>
<ide name="xs">

  <p>
    <a href="Images/xs/11-drag-button.png" class="fancybox"><img src=
            "Images/xs/11-drag-button-sml.png"></a>
  </p>
</ide>
<ide name="vs">

  <p>
    <a href="Images/vs/09-drag-button.png" class="fancybox"><img src=
            "Images/vs/09-drag-button-sml.png"></a>
  </p>
</ide>
</li>

<ide name="xs">

    <li>
      <p>
        With the <span class="uiitem">Button</span> selected on the design surface, we can
        use the <span class="uiitem">Properties</span> pad to change the 
        <code>Id</code> property of the <span class="uiitem">Button</span> to
        <code>@+id/TranslateButton</code> and change the 
        <code>Text</code> property to <code>Translate</code>:
      </p>
      <p>
        <a href="Images/xs/12-translate-button.png" class=" fancybox"><img src=
                "Images/xs/12-translate-button-sml.png"></a>
      </p>
    </li>
</ide>
<li>

  <p>
    Next, let&lsquo;s drag a second <span class="uiitem">Button</span> from the
    <span class="uiitem">Toolbox</span> to the design surface and place it underneath the
    <span class="uiitem">Translate</span> button:
  </p>

<ide name="xs">

  <p>
    <a href="Images/xs/13-drag-call-button.png" class="fancybox"><img src=
            "Images/xs/13-drag-call-button-sml.png"></a>
  </p>
</ide>
<ide name="vs">

  <p>
    <a href="Images/vs/11-drag-call-button.png" class="fancybox"><img src=
            "Images/vs/11-drag-call-button-sml.png"></a>
  </p>
</ide>
</li>

<ide name="xs">

    <li>
      <p>
        With the <span class="uiitem">Button</span> selected on the design surface, we can
        use the <span class="uiitem">Properties</span> pad to change the 
        <code>Id</code> property of the <span class="uiitem">Button</span> to
        <code>@+id/CallButton</code> and change the <code>Text</code>
        property to <code>Call</code>:
      </p>
      <p>
        <a href="Images/xs/14-call-properties.png" class=" fancybox"><img src=
                "Images/xs/14-call-properties-sml.png"></a>
      </p>
      <p>
        Let's save our work by pressing <kbd>&#8984; + S</kbd>.
      </p>
    </li>
    <li>
      <p>
        Now, let&lsquo;s add some code to translate phone numbers from alphanumeric to
        numeric. We&lsquo;ll add a new file to the project by clicking on the gear
        icon next to the <span class="uiitem">Phoneword</span> project in the <span class=
        "uiitem">Solution</span> pad and choosing <span class="uiitem">Add &gt; New
        File...</span>:
      </p>
      <p>
        <a href="Images/xs/15-add-new-file.png" class=" fancybox"><img src=
                "Images/xs/15-add-new-file-sml.png"></a>
      </p>
    </li>
</ide>

<ide name="xs">

    <li>
      <p>
        In the <span class="uiitem">New File</span> dialog, let&lsquo;s select <span class=
        "uiitem">General &gt; Empty Class</span>, name the new file
        <b>PhoneTranslator</b>, and click <span class="uiitem">New</span>:
      </p>
      <p>
        <a href="Images/xs/16-add-class.png" class=" fancybox"><img src=
                "Images/xs/16-add-class-sml.png"></a>
      </p>
    </li>
</ide>

<li>

  <p>
    This creates a new empty C# class for us. Let&lsquo;s remove all of the template 
    code and replace it with the following code:
  </p>

<pre>
<code class=" syntax brush-C#">using System.Text;
using System;

namespace Core
{
    public static class PhonewordTranslator
    {
        public static string ToNumber(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "";
            else
                raw = raw.ToUpperInvariant();

            var newNumber = new StringBuilder();
            foreach (var c in raw)
            {
                if (" -0123456789".Contains(c))
                    newNumber.Append(c);
                else {
                    var result = TranslateToNumber(c);
                    if (result != null)
                        newNumber.Append(result);
                }
                // otherwise we've skipped a non-numeric char
            }
            return newNumber.ToString();
        }
        static bool Contains (this string keyString, char c)
        {
            return keyString.IndexOf(c) &gt;= 0;
        }
        static int? TranslateToNumber(char c)
        {
            if ("ABC".Contains(c))
                return 2;
            else if ("DEF".Contains(c))
                return 3;
            else if ("GHI".Contains(c))
                return 4;
            else if ("JKL".Contains(c))
                return 5;
            else if ("MNO".Contains(c))
                return 6;
            else if ("PQRS".Contains(c))
                return 7;
            else if ("TUV".Contains(c))
                return 8;
            else if ("WXYZ".Contains(c))
                return 9;
            return null;
        }
    }
}</code></pre>

<ide name="xs">

  <p>
    Let's save the changes to the <b>PhoneTranslator.cs</b> file by choosing 
    <span class="uiitem">File &gt; Save</span> (or by pressing <kbd>&#8984; + S</kbd>),
    then close the file.
  </p>
   
</ide>
</li>

<ide name="xs">

    <li>
      <p>
        Next we&lsquo;re going to add code to wire up the user interface. Let&lsquo;s add
        the backing code into the <code>MainActivity</code> class. Double-click
        <b>MainActivity.cs</b> in the <span class="uiitem">Solution Pad</span> to open it:
      </p>
      <p>
        <a href="Images/xs/18-mainactivity.png" class=" fancybox"><img src=
                "Images/xs/18-mainactivity-sml.png"></a>
      </p>
    </li>
</ide>

<li>

  <p>
    We begin by wiring up the <span class="uiitem">Translate</span> button. In the
    <code>MainActivity</code> class, find the <code>OnCreate</code> method. We'll add our
    button code inside <code>OnCreate</code>, below the <code>base.OnCreate(bundle)</code>
    and <code>SetContentView (Resource.Layout.Main)</code> calls. Remove the template code
    so that the <code>OnCreate</code> method resembles the following code:
  </p>
   

<pre>
<code class=" syntax brush-C#">using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phoneword
{
    [Activity (Label = "Phoneword", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Our code will go here
        }
    }
}</code></pre>
</li>

<li>

  <p>
    Next, we need to get a reference to the controls that we created in the layout file
    with the Android Designer. Let's add the following code inside the
    <code>OnCreate</code> method:
  </p>

<pre>
<code class=" syntax brush-c#">// Get our UI controls from the loaded layout:
EditText phoneNumberText = FindViewById&lt;EditText&gt;(Resource.Id.PhoneNumberText);
Button translateButton = FindViewById&lt;Button&gt;(Resource.Id.TranslateButton);
Button callButton = FindViewById&lt;Button&gt;(Resource.Id.CallButton);</code></pre>
</li>

<li>

  <p>
    Now let's add code that responds to user presses of the <span class=
    "uiitem">Translate</span> button. Add the following code below the control definitions
    inside the <code>OnCreate</code> method:
  </p>
<pre>
<code class=" syntax brush-C#">// Disable the "Call" button
callButton.Enabled = false;

// Add code to translate number
string translatedNumber = string.Empty;

translateButton.Click += (object sender, EventArgs e) =&gt;
{
    // Translate user’s alphanumeric phone number to numeric
    translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
    if (String.IsNullOrWhiteSpace(translatedNumber))
    {
        callButton.Text = "Call";
        callButton.Enabled = false;
    }
    else
    {
        callButton.Text = "Call " + translatedNumber;
        callButton.Enabled = true;
    }
};</code></pre>
</li>

<li>

  <p>
    Next let&lsquo;s add code that responds to user presses of the <span class=
    "uiitem">Call</span> button. We&lsquo;ll place the following code below the code for
    the <span class="uiitem">Translate</span> button:
  </p>

<pre>
<code class=" syntax brush-C#">callButton.Click += (object sender, EventArgs e) =&gt;
{
    // On "Call" button click, try to dial phone number.
    var callDialog = new AlertDialog.Builder(this);
    callDialog.SetMessage("Call " + translatedNumber + "?");
    callDialog.SetNeutralButton("Call", delegate {
           // Create intent to dial phone
           var callIntent = new Intent(Intent.ActionCall);
           callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
           StartActivity(callIntent);
       });
    callDialog.SetNegativeButton("Cancel", delegate { });

    // Show the alert dialog to the user and wait for response.
    callDialog.Show();
};</code></pre>
</li>

<ide name="xs">

    <li>
      <p>
        Finally, let&lsquo;s give our application permission to place a phone call. Open
        the project options by right-clicking <span class="uiitem">Phoneword</span> in the
        <span class="uiitem">Solution</span> pad and selecting <span class=
        "uiitem">Options</span>:
      </p>
      <p>
        <a href="Images/xs/23a-project-options.png" class=" fancybox"><img src=
                "Images/xs/23a-project-options.png"></a>
      </p>
      <p>
        In the <span class="uiitem">Project Options</span> dialog, 
        select <span class= "uiitem">Build &gt; Android 
        Application</span>. In the <span class= "uiitem">Required 
        Permissions</span> section, enable the 
        <span class="uiitem">CallPhone</span> permission: 
      </p>
      <p>
        <a href="Images/xs/23b-call-phone.png" class=" fancybox"><img src=
                "Images/xs/23b-call-phone-sml.png"></a>
      </p>
    </li>
   
</ide>

<ide name="xs">

    <li>
      <p>
        Let's save our work and build the application by selecting <span class=
        "uiitem">Build &gt; Build All</span> (or by pressing <kbd>&#8984; + B</kbd>). If
        our application compiles, we will get a success message at the top of 
        Xamarin Studio:
      </p>
      <p>
        <img src="Images/vs/23-rebuild-all-succeeded.png">
      </p>
      <p>
        If there are errors, we can go through the previous steps and correct any mistakes
        until the application builds successfully.
      </p>
    </li>
</ide>

<li>

  <p>
    We now have a working application &ndash; it&lsquo;s time to add the finishing touches!
    Let&lsquo;s start by editing the <code>Label</code> for our <code>MainActivity</code>.
    The <code>Label</code> is what Android displays at the top of the screen to let users
    know where they are in the application. At the top of the <code>MainActivity</code>
    class, change the <code>Label</code> to <code>Phone Word</code> as seen here:
  </p>
<pre>
<code class=" syntax brush-C#">namespace Phoneword
{
    [Activity (Label = "Phone Word", MainLauncher = true)]
    public class MainActivity : Activity
    {
        ...
    }
}</code></pre>
</li>

<ide name="xs">

    <li>
      <p>
        Next, let's set the application icon. First, let's open the downloaded and unzipped <a href=
        "/guides/android/getting_started/hello,android/Resources/XamarinAppIconsAndLaunchImages.zip">
        Xamarin App Icons set</a>. Next, let's expand the <span class=
        "uiitem">drawable-hdpi</span> folder under <span class="uiitem">Resources</span>
        and remove the existing <b>Icon.png</b> by right-clicking it and selecting
        <span class="uiitem">Remove</span>:
      </p>
      <p>
        <a href="Images/xs/25a-remove-icon.png" class=" fancybox"><img src=
                "Images/xs/25a-remove-icon-sml.png"></a>
      </p>
      <p>
        When the following dialog box is displayed, select <span class="uiitem">Delete</span>:
      </p>
      <p>
        <a href="Images/xs/25b-delete-icon.png" class=" fancybox"><img src=
                "Images/xs/25b-delete-icon-sml.png"></a>

      </p>
    </li>
    <li>
      <p>
        Next, let's right-click the <span class="uiitem">drawable-hdpi</span> folder
        and select <span class="uiitem">Add &gt; Add Files</span>:
      </p>
      <p>
        <a href="Images/xs/26-add-files.png" class=" fancybox"><img src=
                "Images/xs/26-add-files-sml.png"></a>
      </p>
    </li>
    <li>
      <p>
        From the selection dialog, let's navigate to the unzipped Xamarin App Icons
        directory and open the <span class="uiitem">drawable-hdpi</span> folder. Select 
        <b>Icon.png</b>:
      </p>
      <p>
        <a href="Images/xs/27-xamarin-icons.png" class=" fancybox"><img src=
                "Images/xs/27-xamarin-icons-sml.png"></a>
      </p>
    </li>
    <li>
      <p>
        In the <span class="uiitem">Add File to Folder</span> dialog box, select 
        <span class="uiitem">Copy the file into the
        directory</span> and click <span class="uiitem">OK</span>:
      </p>
      <p>
        <img src="Images/xs/28-copy-to-directory.png">
      </p>
    </li>
    <li>
      <p>
        Repeat these steps for each of the <span class="uiitem">drawable-*</span> folders
        until the contents of the <span class="uiitem">drawable-*</span> Xamarin App Icons 
        folders are copied to their counterpart <span class="uiitem">drawable-*</span> folders 
        in the <b>Phoneword</b> project:
      </p>
      <p>
        <a href="Images/xs/29-resource-folders.png" class=" fancybox"><img src=
                "Images/xs/29-resource-folders-sml.png"></a>
      </p>
      <p>
        These folders provide different resolutions of the icon so that it renders correctly 
        on different devices with different screen densities.
      </p>
    </li>
   
</ide>

<ide name="xs">

    <li>
      <p>
        Finally, we can test our application by deploying it to an Android emulator. If you
        have not yet configured your emulator, please see <a href=
        "http://developer.xamarin.com/guides/android/getting_started/installation/android-player/">
        Xamarin Android Player</a> for setup instructions. 
        In this example, we have installed the <span class="uiitem">Nexus 4 (KitKat)</span> 
        (Android 4.4.2, API Level 19) virtual device and we have started it from the 
        Xamarin Android Player <span class="uiitem">Device Manager</span> console:
      </p>
      <p>
         <a href="Images/xs/32a-start-xap-virtual-device.png" class=" fancybox"><img src=
                 "Images/xs/32a-start-xap-virtual-device-sml.png"></a>
      </p>
      <p>
        In Xamarin Studio, select this virtual device (under 
        <span class="uiitem">Virtual Devices</span>) and click the play button
        in the upper left corner:
      </p>
      <p>
        <a href="Images/xs/32b-select-virtual-device.png" class=" fancybox"><img src=
                "Images/xs/32b-select-virtual-device-sml.png"></a>
      </p>
      <p>
        As shown in this screenshot, we have selected the <span class="uiitem">Nexus 4
        (KitKat) (API 19)</span> virtual device that is running in the Xamarin Android
        Player.
      </p>
    </li>
   
</ide>

<ide name="xs">

    <li>
      <p>
        After Xamarin Studio loads the application into the virtual device, the <b>Phoneword</b> 
        app is automatically started. The screenshots below
        illustrate the <b>Phoneword</b> application running in the Xamarin Android Player.
        The icons that we installed are displayed next to the <span class="uiitem">Phone Word</span> 
        label that we configured in <code>MainActivity</code>. 
        Clicking the <span class="uiitem">Translate</span> button
        updates the text of the <span class="uiitem">Call</span> button, and clicking the
        <span class="uiitem">Call</span> button causes the call dialog to appear as shown
        on the right:
      </p>
      <p>
        <a href="Images/xs/31-phoneword-result.png" class="fancybox"><img src=
                "Images/xs/31-phoneword-result-sml.png"></a>
      </p>
    </li>
   
</ide>

</ol>

Congratulations on completing your first Xamarin.Android application! Now it's time to 
dissect the tools and skills we just learned in the 
[Hello, Android Deep Dive](/guides/android/getting_started/hello,android/hello,android_deepdive/).
