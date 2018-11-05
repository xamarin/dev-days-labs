using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace ImageSearch.Droid
{
    public abstract class BaseActivity : AppCompatActivity
    {
        public Toolbar Toolbar { get; set; }

        protected abstract int LayoutResource { get; }
        protected int ActionBarIcon { set => Toolbar.SetNavigationIcon(value); }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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