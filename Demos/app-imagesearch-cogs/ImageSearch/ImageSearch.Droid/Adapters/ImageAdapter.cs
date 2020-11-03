using System;
using Android.App;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AsyncAwaitBestPractices;
using ImageSearch.ViewModel;
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

            var itemView = LayoutInflater.From(parent.Context)?.Inflate(id, parent, false) ?? throw new NullReferenceException($"Cannot Inflate {nameof(Resource.Layout.item)}");

            return new ImageAdapterViewHolder(itemView, OnClick, OnLongClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = _viewModel.Images[position];

            var imageAdapterViewHolder = (ImageAdapterViewHolder)holder;

            imageAdapterViewHolder.Caption.Text = item?.Name;

            Picasso.Get().Load(item?.ContentUrl).Into(imageAdapterViewHolder.Image);
        }

        void OnClick(ImageAdapterClickEventArgs args) => _itemClickEventManager.RaiseEvent(this, args, nameof(ItemClick));
        void OnLongClick(ImageAdapterClickEventArgs args) => _itemLongClickEventManager.RaiseEvent(this, args, nameof(ItemLongClick));
    }

    class ImageAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageAdapterViewHolder(View itemView, Action<ImageAdapterClickEventArgs> clickListener,
                            Action<ImageAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.imageView) ?? throw new NullReferenceException($"Cannot find {nameof(Resource.Id.imageView)}");
            Caption = itemView.FindViewById<TextView>(Resource.Id.textView) ?? throw new NullReferenceException($"Cannot find {nameof(Resource.Id.textView)}");

            itemView.Click += (sender, e) => clickListener(new ImageAdapterClickEventArgs(itemView, AdapterPosition));
            itemView.LongClick += (sender, e) => longClickListener(new ImageAdapterClickEventArgs(itemView, AdapterPosition));
        }

        public ImageView Image { get; }
        public TextView Caption { get; }
    }

    class ImageAdapterClickEventArgs : EventArgs
    {
        public ImageAdapterClickEventArgs(View view, int position) => (View, Position) = (view, position);

        public View View { get; }
        public int Position { get; }
    }
}
