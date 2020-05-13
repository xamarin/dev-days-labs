using System;
using Android.App;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AsyncAwaitBestPractices;
using ImageSearch.ViewModel;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using Square.Picasso;

namespace ImageSearch.Droid.Adapters
{
    class ImageAdapter : RecyclerView.Adapter
    {
        readonly WeakEventManager<ImageAdapterClickEventArgs> _itemClickEventManager = new WeakEventManager<ImageAdapterClickEventArgs>();
        readonly WeakEventManager<ImageAdapterClickEventArgs> _itemLongClickEventManager = new WeakEventManager<ImageAdapterClickEventArgs>();

        readonly ImageSearchViewModel _viewModel;
        readonly Activity _activity;

        public ImageAdapter(Activity activity, ImageSearchViewModel viewModel)
        {
            _viewModel = viewModel;
            _activity = activity;

            _viewModel.Images.CollectionChanged += (sender, args) => _activity.RunOnUiThread(NotifyDataSetChanged);
        }

        event EventHandler<ImageAdapterClickEventArgs> ItemClick
        {
            add => _itemClickEventManager.AddEventHandler(value);
            remove => _itemClickEventManager.RemoveEventHandler(value);
        }

        event EventHandler<ImageAdapterClickEventArgs> ItemLongClick
        {
            add => _itemLongClickEventManager.AddEventHandler(value);
            remove => _itemLongClickEventManager.RemoveEventHandler(value);
        }

        public override int ItemCount => _viewModel.Images.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.item;

            var itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            return new ImageAdapterViewHolder(itemView, OnClick, OnLongClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ImageObject? item = _viewModel.Images[position];

            var imageAdapterViewHolder = (ImageAdapterViewHolder)holder;

            imageAdapterViewHolder.Caption.Text = item?.Name;

            Picasso.With(_activity).Load(item?.ContentUrl).Into(imageAdapterViewHolder.Image);
        }

        void OnClick(ImageAdapterClickEventArgs args) => _itemClickEventManager.HandleEvent(this, args, nameof(ItemClick));
        void OnLongClick(ImageAdapterClickEventArgs args) => _itemLongClickEventManager.HandleEvent(this, args, nameof(ItemLongClick));
    }

    public class ImageAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageAdapterViewHolder(View itemView, Action<ImageAdapterClickEventArgs> clickListener,
                            Action<ImageAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            Caption = itemView.FindViewById<TextView>(Resource.Id.textView);

            itemView.Click += (sender, e) => clickListener(new ImageAdapterClickEventArgs(itemView, AdapterPosition));
            itemView.LongClick += (sender, e) => longClickListener(new ImageAdapterClickEventArgs(itemView, AdapterPosition));
        }

        public ImageView Image { get; }
        public TextView Caption { get; }
    }

    public class ImageAdapterClickEventArgs : EventArgs
    {
        public ImageAdapterClickEventArgs(View view, int position) => (View, Position) = (view, position);

        public View View { get; }
        public int Position { get; }
    }
}
