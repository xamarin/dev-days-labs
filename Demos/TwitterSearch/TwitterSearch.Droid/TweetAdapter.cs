using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TwitterSearch.Portable.ViewModels;

namespace TwitterSearch.Droid
{
  public class TweetAdapter : BaseAdapter
  {
    TwitterViewModel viewModel;
    Activity context;
    public TweetAdapter(Activity context, TwitterViewModel viewModel)
    {
      this.context = context;
      this.viewModel = viewModel;
    }
    public override int Count
    {
      get { return viewModel.Tweets.Count; }
    }

    public override Java.Lang.Object GetItem(int position)
    {
      return position;

    }

    public override long GetItemId(int position)
    {
      return position;
    }

    public override View GetView(int position, View convertView, ViewGroup parent)
    {
      var tweet = viewModel.Tweets[position];
      var view = convertView;
      if (view == null)
      {
        view = context.LayoutInflater.Inflate(Resource.Layout.tweet, null);
      }

      var name = view.FindViewById<TextView>(Resource.Id.name);
      var time = view.FindViewById<TextView>(Resource.Id.time);
      var tweetText = view.FindViewById<TextView>(Resource.Id.tweet);
      var photo = view.FindViewById<ImageView>(Resource.Id.photo);

      Koush.UrlImageViewHelper.SetUrlDrawable(photo, tweet.Image);
      name.Text = tweet.DisplayName;
      tweetText.Text = tweet.Text;
      time.Text = tweet.Date;

      return view;
    }
  }
}