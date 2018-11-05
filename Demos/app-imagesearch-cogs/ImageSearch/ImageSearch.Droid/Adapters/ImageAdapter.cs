using System;

using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using ImageSearch.ViewModel;

using Square.Picasso;

namespace ImageSearch.Droid.Adapters
{
    class ImageAdapter : RecyclerView.Adapter
    {
        readonly ImageSearchViewModel _viewModel;
        readonly Activity _activity;

        public ImageAdapter(Activity activity, ImageSearchViewModel viewModel)
        {
            _viewModel = viewModel;
            _activity = activity;

            _viewModel.Images.CollectionChanged += (sender, args) => _activity.RunOnUiThread(NotifyDataSetChanged);
        }

        event EventHandler<ImageAdapterClickEventArgs> ItemClick;
        event EventHandler<ImageAdapterClickEventArgs> ItemLongClick;

        public override int ItemCount => _viewModel.Images.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView;

            var id = Resource.Layout.item;

            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var holder = new ImageAdapterViewHolder(itemView, OnClick, OnLongClick);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _viewModel.Images[position];

            var imageAdapterViewHolder = holder  as ImageAdapterViewHolder;

            imageAdapterViewHolder.Caption.Text = item?.Name;

            Picasso.With(_activity).Load(item?.ContentUrl).Into(imageAdapterViewHolder.Image);
        }

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