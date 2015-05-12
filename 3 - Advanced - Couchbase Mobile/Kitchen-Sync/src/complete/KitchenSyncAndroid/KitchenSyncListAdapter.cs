using System;
using System.Collections.Generic;

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

using KitchenSyncShared;

namespace KitchenSyncAndroid {
    public class KitchenSyncListAdapter : ArrayAdapter<Task> {
		public KitchenSyncListAdapter (Context context, int resource, int textViewResourceId, IList<Task> tasks) :
        base(context, resource, textViewResourceId, tasks)
		{
		}
		
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
            if(convertView == null) {
                LayoutInflater vi = (LayoutInflater)parent.Context.GetSystemService(Android.Content.Context.LayoutInflaterService);
                convertView = vi.Inflate(Resource.Layout.list_item, null);
            }

            Task task;
            try {
                TextView label = convertView.FindViewById<TextView>(Resource.Id.label);
                task = GetItem(position);
                if(task == null) {
                    return convertView;
                }

                label.Text = task.Text;
                label.SetTextColor(Color.White);
                CheckBox checkbox = convertView.FindViewById<CheckBox>(Resource.Id.item_checked);
                checkbox.Checked = task.Checked;
            } catch(Exception e) {
                Log.Error("KitchenSyncListAdapter", "Error {0}", e);
            }
			
            return convertView;
		}
	}
}