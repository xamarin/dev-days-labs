# Xamarin Test Cloud and Xamarin.UITest Lab

## PRIZE

When you are ready to upload to Test Cloud you will be awarded 10 Test Cloud hours to test any of your mobile applications!


This folder contains the start project for the Xamarin.UITest mini-hack at Evolve 2014. 

This mini-hack involves the following steps:

1. Log in to [Xamarin Test Cloud](http://testcloud.xamarin.com) to obtain your Test Cloud API Key and to get your Device ID.

2. Create a new NUnit project for the Xamarin.UITests and add a `TestFixture` to that project.

3. Submit the iOS application and the Xamarin.UITests to Test Cloud.


This solution includes a Xamarin.Android and Xamarin.iOS project, however we will only focus on the Xamarin.iOS project using Xamarin Studio. This mini-hack should work on Visual Studio with some slight variation from these instructions.

## About the Application

The application is a simple credit card validator with one rule:

* A valid credit number must have exactly 16 characters.

Here are some screenshots of the application running in iOS:

![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image10.png?raw=true)

There is a bash script included, `build.sh` that can be used to compile both projects from the command line. The `images` folder holds the bitmaps necessary for this README.

# Getting Started

## Get the Test Cloud API key and the Device ID

Before Xamarin Test Cloud will accept submissions you must log in to Xamarin Test Cloud to get an API key and a Device ID. You can find detailed 
instructions on [Xamarin's developer portal](http://developer.xamarin.com/guides/testcloud/submitting/#Getting_a_Test_Cloud_API_Key_and_Selecting_Devices). 

The list below is a quick summary:  

1. Log in to Xamarin Test Cloud

2. Create a new Test Run

3. Record the API Test key and the Device ID.


## Adding the Xamarin Test Cloud Agent to CreditCardValidation.iOS

In order for Xamarin.UITests to interact with the application, the Xamarin Test Cloud Agent  must be added to the iOS application. Android applications do not use the Xamarin Test Cloud Agent.

1. Right click on the **Components** folder of the CreditCardValidation.iOS project, and select **Get More Components**:

	![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image01.png?raw=true)

2. Search the Component Store using the keyword **Xamarin Test Cloud Agent**. The Test Cloud Agent component should show up:

	![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image02.png?raw=true)

3.  Double click on the component and add it to the project.

	![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image03.png?raw=true)

4. Finally, edit `AppDelegate.cs` so that it resembles the following code:

	```
	public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        #if DEBUG
        Xamarin.Calabash.Start();
        #endif

        window = new UIWindow(UIScreen.MainScreen.Bounds);

        viewController = new UINavigationController(new CreditCardValidationScreen());
        window.RootViewController = viewController;
        window.MakeKeyAndVisible();

        return true;
    }
	```	

5. Compile the Xamarin.iOS project and run it in the iOS simulator or on your iPhone if you have one. 


## Adding an NUnit Project

Now that the Xamarin Test Cloud Agent has been added to Xamarin.iOS project, we will need another project for the UITest code.

### If you are using OS X

1. Right click on the solution, and select **Add > New Project**

    ![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image04.png?raw=true)

2. From the **New Project** dialog select the **NUnit Library Project**:

    ![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image05.png?raw=true)

3. Name the project **CreditCardValidation.UITests** and click OK. The Solution Pad should resemble the following screenshot:

    ![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image06.png?raw=true) 

4. Next it is necessary to add the Xamarin.UITest NuGet package. Right click on the CreditCardValidation.UITests project, and select **Add > Add Packages...**:

    ![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image07.png)

5. Search NuGet for **UITest**, select the **Xamarin.UITest** package, and add it to the project:

    ![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image08.png?raw=true) 
    Once the package has been added, the Solution Pad should resemble the following screenshot:

    ![](https://github.com/xamarin/mini-hacks/blob/master/Test%20Cloud/images/image09.png?raw=true)

At this point we are ready to start adding the tests to CreditCardValidation.UITests.

### If you are using Windows

1. Add a new class library project named **CreditCardValidation.UITests** to your solution.

2. Add NUnit 2.6.3 to the project using NuGet.

3. Next it is necessary to add the Xamarin.UITest NuGet package. Right click on the CreditCardValidation.UITests project, and select **Add > Add Packages...**.

4. Search NuGet for **UITest**, select the **Xamarin.UITest** package, and add it to the project.


## Adding The Tests	


1. Edit the contents of the file `Tests.cs` so that it contains the 
following:

	```
    using System;
    using NUnit.Framework;
    using System.Reflection;
    using System.IO;
    using Xamarin.UITest.iOS;
    using Xamarin.UITest;
    using Xamarin.UITest.Queries;
    using System.Linq;
    
    namespace CreditCardValidation.UITests
    {
        [TestFixture]
        public class CreditCardValidationTests
        {
            static readonly Func<AppQuery, AppQuery> EditTextView = c => c.Marked("CreditCardTextField");
            static readonly Func<AppQuery, AppQuery> ValidateButton = c => c.Marked("ValidateButton");
            static readonly Func<AppQuery, AppQuery> ShortErrorMessage = c => c.Marked("ErrorMessagesTextField").Text("Credit card number is to short.");
            static readonly Func<AppQuery, AppQuery> LongErrorMessage = c => c.Marked("ErrorMessagesTextField").Text("Credit card number is to long.");
            static readonly Func<AppQuery, AppQuery> SuccessScreenNavBar = c => c.Class("UINavigationBar").Id("Valid Credit Card");
            static readonly Func<AppQuery, AppQuery> SuccessMessageLabel = c => c.Class("UILabel").Text("The credit card number is valid!");
    
            iOSApp _app;
    
            public string PathToIPA { get; set; }
    
            [TestFixtureSetUp]
            public void TestFixtureSetup()
            {
                string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                FileInfo fi = new FileInfo(currentFile);
                string dir = fi.Directory.Parent.Parent.Parent.FullName;
                PathToIPA = Path.Combine(dir, "CreditCardValidation.iOS", "bin", "iPhoneSimulator", "Debug", "CreditCardValidationiOS.app");
    
    
            }
    
            [SetUp]
            public void SetUp()
            {
                    _app = ConfigureApp.iOS.AppBundle(PathToIPA).ApiKey("YOUR_API_KEY_HERE").StartApp();
            }
    
            [Test]
            public void CreditCardNumber_ToShort_DisplayErrorMessage()
            {
                // Arrange - Nothing to do because the queries have already been initialized.
    
                // Act
                _app.EnterText(EditTextView, new string('9', 15));
                _app.Tap(ValidateButton);
    
                // Assert
                AppResult[] result = _app.Query(ShortErrorMessage);
                Assert.IsTrue(result.Any(), "The error message is not being displayed.");
            }
    
            [Test]
            public void CreditCardNumber_TooLong_DisplayErrorMessage()
            {
                // Arrange - There is nothing to do as the queries have already been created.
    
                // Act
                _app.EnterText(EditTextView, new string('9', 17));
                _app.Tap(ValidateButton);
    
                // Assert
                AppResult[] result = _app.Query(LongErrorMessage);
                Assert.IsTrue(result.Any(), "The error message is not being displayed.");
            }
    
            [Test]
            public void CreditCardNumber_CorrectSize_DisplaySuccessScreen()
            {
                // Arrange - Nothing to do, the queries have been defined.
    
                // Act - Enter a valid credit card number, tap Valid, and wait for the next screen to appear
                _app.EnterText(EditTextView, new string('9', 16));
                _app.Tap(ValidateButton);
    
                _app.WaitForElement(SuccessScreenNavBar, "Valid Credit Card Screen did not appear", TimeSpan.FromSeconds(5));
    
                // Assert - Make sure that the message is on the screen
                AppResult[] results = _app.Query(SuccessMessageLabel);
                Assert.IsTrue(results.Any(), "The success message was not displayed on the screen");
            }
        }
    }
	```

2. Run the tests locally. This ensures that everything is working before submitting to Test Cloud.


## Submit the Tests to Test Cloud

Build the project one more time, this time targeting the `Debug | iPhone` build configuration. This will ensure that an `IPA` is created at `./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa`.

Next using the API key and device ID from the start of this walkthrough, you can use the following bash script to compile the application and then submit it to Xamarin Test Cloud (notice that you have to add in your Test Cloud API key and your IOS Device ID):

    #!/bin/sh

    ### Add your Test Cloud API Key/iOS Device Id and uncomment the lines below
    # export TESTCLOUD_API_KEY=<SetYourAPIKeyHere>
    # export IOS_DEVICE_ID=<SetYourDeviceIdsHere>

    ### You shouldn't have to update these variables.
    export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa
    export ASSEMBLY_DIR=./CreditCardValidation.UITests/bin/Debug


    ### Remove the old directories
    rm -rf CreditCardValidation.Droid/obj
    rm -rf CreditCardValidation.Droid/bin
    rm -rf CreditCardValidation.iOS/obj
    rm -rf CreditCardValidation.iOS/bin
    rm -rf CreditCardValidation.UITest/bin
    rm -rf CreditCardValidation.UITest/obj

    ### iOS : compile a Debug build for the iPhone
    /Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./CreditCardValidation.sln


    ### This line submits the application and tests to Test Cloud.
    mono ./packages/Xamarin.UITest.0.6.5/tools/test-cloud.exe submit $IPA $TESTCLOUD_API_KEY --devices $IOS_DEVICE_ID --assembly-dir $ASSEMBLY_DIR

If you are using Windows and Visual Studio you can run from the test cloud folder in a command prompt:

    .\packages\Xamarin.UITest.0.6.5\tools\test-cloud.exe submit .\CreditCardValidation.iOS\bin\iPhone\Debug\CreditCardvalidationiOS-1.0.ipa --devices --series "master" --locale "en_US" --assembly-dir .\CreditCardValidation.UITests\bin\Debug

Run the bash script and patiently wait while the application is uploaded to Test Cloud and enqueued for testing. You should see output similar to the following:

    Negotiating file upload to Xamarin Test Cloud.
    Posting to https://testcloud.xamarin.com/ci/upload2

    Uploading Xamarin.UITest.dll ... Already uploaded.
    Uploading nunit.framework.dll ... Already uploaded.
    Uploading CreditCardValidationiOS-1.0.ipa... 100%
    Uploading CreditCardValidation.UITests.dll... 100%

    Upload complete.

    Status: Waiting for devices...
    Status: Waiting for devices...
    Status: Waiting for devices...
    Status: Waiting for devices...
    Status: Testing: 3 running, 1 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 4 running, 0 enqueued, 0 complete...
    Status: Testing: 3 running, 0 enqueued, 1 complete...
    Status: Testing: 3 running, 0 enqueued, 1 complete...
    Status: Testing: 1 running, 0 enqueued, 3 complete...
    Status: Finished

    Total scenarios: 3
    Passed: 3
    Failed: 0
    Skipped: 0

    Total steps: 3

    Test report: https://testcloud.xamarin.com/test/creditcardvalidation.ios_5ea3683f-8318-49cd-bbc4-8cc15a0bcb/

Once the test run is complete, check out the test report in your browser and see the successful tests!

# Next Steps

At this point, now that you've got tests written for iOS, why not try and write your own tests for the Xamarin.Android application and submit them to Xamarin Test Cloud?

If for some reason you are stuck, you can find a complete version of UITest over at  [mobile_samples/TestCloud/CreditCardValidation/CreditCardValidation-](https://github.com/xamarin/mobile-samples/tree/master/TestCloud/CreditCardValidation/CreditCardValidation-UITest) Github. All you have to do is update the TestFixture with your Test Cloud API Key.
