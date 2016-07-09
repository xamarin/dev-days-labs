using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using ImageSearch.ViewModel;
using Android.App;
using Square.Picasso;

namespace ImageSearch.Droid.Adapters
{
    class ImageAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ImageAdapterClickEventArgs> ItemClick;
        public event EventHandler<ImageAdapterClickEventArgs> ItemLongClick;
        ImageSearchViewModel viewModel;
        Activity activity;

        public ImageAdapter(Activity activity, ImageSearchViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;

            this.viewModel.Images.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.item;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new ImageAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = viewModel.Images[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ImageAdapterViewHolder;
            holder.Caption.Text = item.Title;
            Picasso.With(activity).Load(item.ImageLink).Into(holder.Image);
        }

        public override int ItemCount => viewModel.Images.Count;

        void OnClick(ImageAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ImageAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ImageAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public TextView Caption { get; private set; }

        public ImageAdapterViewHolder(View itemView, Action<ImageAdapterClickEventArgs> clickListener,
                            Action<ImageAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            Caption = itemView.FindViewById<TextView>(Resource.Id.textView);
            itemView.Click += (sender, e) => clickListener(new ImageAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ImageAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class ImageAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}