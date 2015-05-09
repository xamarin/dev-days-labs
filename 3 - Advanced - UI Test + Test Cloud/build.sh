#!/bin/sh

### Add your Test Cloud API Key/iOS Device Id and uncomment the line below
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
