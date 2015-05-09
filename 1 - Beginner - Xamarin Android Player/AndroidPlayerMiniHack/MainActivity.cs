using System;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace AndroidPlayerMiniHack
{
	[Activity (Label = "Xamarin Android Player", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		RecyclerView recyclerView;
		RecyclerView.Adapter adapter;
		RecyclerView.LayoutManager layoutManager;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.main);

			var refreshbattery = FindViewById<Button> (Resource.Id.refresh_battery);
			var batterystatus = FindViewById<TextView> (Resource.Id.battery_status);
			var batterylevel = FindViewById<TextView> (Resource.Id.battery_level);

			refreshbattery.Click += (sender, e) => {
				var filter  = new IntentFilter(Intent.ActionBatteryChanged);
				var battery = RegisterReceiver(null, filter);
				int level   = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
				int scale   = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

				batterylevel.Text = string.Format("Current Charge: {0}%", Math.Floor (level * 100D / scale));

				// Are we charging / charged? works on phones, not emulators must check how.
				int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
				var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

					// How are we charging?
				var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
				var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
				var acCharge = chargePlug == (int)BatteryPlugged.Ac;
				var wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

				isCharging = (usbCharge || acCharge || wirelessCharge);
				if(!isCharging){
					batterystatus.Text = "Status: discharging";
				} else if(usbCharge){
					batterystatus.Text = "Status: charging via usb";
				} else if(acCharge){
					batterystatus.Text = "Status: charging via ac";
				} else if(wirelessCharge){
					batterystatus.Text = "Status: charging via wireless";
				}
			};
				
			recyclerView = FindViewById<RecyclerView> (Resource.Id.recycler_view);
			recyclerView.HasFixedSize = true;


			layoutManager = new LinearLayoutManager (this);
			recyclerView.SetLayoutManager (layoutManager);

			var items = new List<RecyclerItem> (100);
			for(int i = 0; i < 100; i++)
				items.Add(new RecyclerItem{Title = "Item: " + i});

			adapter = new RecyclerAdapter (items);
			recyclerView.SetAdapter (adapter);


			//TODO: FindViewById for new GPS TextView

			//TODO: FindViewById for new GPS Button Refresh

			//TODO: Add Click Handler to GetPositionAsync with async keyword
			/*
			 var getlocation = FindViewById<Button> (Resource.Id.REPLACE_WITH_ID);

				getlocation.Click += async (sender, e) => {
			    };
			 * /

			//In Click Handler
			//TODO: Create new geolcator and Call GetPositionAsync() from Geolocator Plugin
			//var locator = CrossGeolocator.Current;
			//locator.DesiredAccuracy = 50;
			 
			//TODO: Set Label to new location
			/*
			 var position = await locator.GetPositionAsync (timeout: 10000);

			 Console.WriteLine ("Position Latitude: {0}", position.Latitude);
			 Console.WriteLine ("Position Longitude: {0}", position.Longitude);
			*/
		}


		public class RecyclerItem : Java.Lang.Object
		{
			static int[] Ids = new int[]{Resource.Drawable.caterpiller, Resource.Drawable.flying_in_the_light_large, Resource.Drawable.jelly_fish_2, Resource.Drawable.lone_pine_sunset, Resource.Drawable.look_me_in_the_eye, Resource.Drawable.over_there, Resource.Drawable.rainbow, Resource.Drawable.rainbow, Resource.Drawable.sample1, Resource.Drawable.sample2};
			static int nextId;
			public RecyclerItem()
			{
				Image = Ids[nextId];
				nextId++;
				if (nextId >= Ids.Length)
					nextId = 0;
			}
			public string Title {get;set;}
			public int Image {get;set;}
		}

		public class ViewHolder : RecyclerView.ViewHolder
		{
			public TextView Text {get;set;}
			public ImageView Image {get;set;}

			public ViewHolder(View view) :
			base(view)
			{
				Text = view.FindViewById<TextView> (Resource.Id.textView);
				Image = view.FindViewById<ImageView> (Resource.Id.imageView);
			}
		}

		public class RecyclerAdapter : RecyclerView.Adapter, View.IOnClickListener
		{

			public List<RecyclerItem> Items { get; set; }
			/// <summary>
			/// Generice constructor but you would want ot base this on your data.
			/// </summary>
			/// <param name="items">Items.</param>
			public RecyclerAdapter(List<RecyclerItem> items)
			{
				this.Items = items;
			}

			/// <summary>
			/// Create a new view which is invoked by the layout manager
			/// </summary>
			/// <param name="parent">Parent.</param>
			/// <param name="viewType">View type.</param>
			public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
			{
				var view = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.recycleritem_card, null);
				view.SetOnClickListener (this);
				return new ViewHolder (view);
			}

			/// <summary>
			/// Replaces the content of the view. Invoked by the layout manager
			/// </summary>
			/// <param name="holder">Holder.</param>
			/// <param name="position">Position.</param>
			public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
			{
				var holder2 = holder as ViewHolder;
				holder2.Text.Text = Items [position].Title;
				holder2.Image.SetImageResource (Items [position].Image);
				holder2.ItemView.Tag = Items [position];
			}

			public override int ItemCount {
				get {
					return Items.Count;
				}
			}

			public void Add(RecyclerItem item) {
				var position = Items.Count;
				Items.Insert(position, item);
				NotifyItemInserted(position);
			}

			public void Remove(int position) {
				if (position < 0)
					return;

				Items.RemoveAt(position);
				NotifyItemRemoved(position);
			}

			public Action<View, RecyclerItem> OnItemClick { get; set; }
			public void OnClick (View v)
			{
				if (OnItemClick == null)
					return;

				OnItemClick (v, (RecyclerItem)v.Tag);
			}

		}
	}
}


