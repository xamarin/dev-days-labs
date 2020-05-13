using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;

namespace ImageSearch.Droid
{
    public abstract class BaseActivity : AppCompatActivity
    {
        protected abstract int LayoutResource { get; }
        Toolbar? Toolbar { get; set; }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [Android.Runtime.GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Acr.UserDialogs.UserDialogs.Init(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(LayoutResource);

            Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);

                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }
        }
    }
}