using LinqToTwitter;
using TwitterSearch.Portable.Models;
using Refractored.Xam.TTS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterSearch.Portable.ViewModels
{
  public class TwitterViewModel
  {
    public ObservableCollection<Tweet> Tweets { get; set; }
    public bool IsBusy { get; set; }
    
    public TwitterViewModel()
    {
      Tweets = new ObservableCollection<Tweet>();
    }

    public async Task LoadTweetsCommand(string search)
    {
      if (IsBusy)
        return;

      IsBusy = true;
      try
      {

        Tweets.Clear();
        var auth = new ApplicationOnlyAuthorizer()
        {
          CredentialStore = new InMemoryCredentialStore
          {
            ConsumerKey = "ZTmEODUCChOhLXO4lnUCEbH2I",
            ConsumerSecret = "Y8z2Wouc5ckFb1a0wjUDT9KAI6DUat5tFNdmIkPLl8T4Nyaa2J",
          },
        };
        await auth.AuthorizeAsync();

        var twitterContext = new TwitterContext(auth);
       
        var queryResponse = await
          (from tweet in twitterContext.Search
           where tweet.Type == SearchType.Search &&
             tweet.Query == search &&
             tweet.Count == 100
           select tweet).SingleOrDefaultAsync();

        if (queryResponse == null || queryResponse.Statuses == null)
          return;


        var tweets =
          (from tweet in queryResponse.Statuses
           select new Tweet
           {
             StatusID = tweet.StatusID,
             ScreenName = tweet.User.ScreenNameResponse,
             Text = tweet.Text,
             CurrentUserRetweet = tweet.CurrentUserRetweet,
             CreatedAt = tweet.CreatedAt,
             Url = string.Format("https://m.twitter.com/{0}/status/{1}", tweet.User.ScreenNameResponse, tweet.StatusID),
             Image = (tweet.RetweetedStatus != null && tweet.RetweetedStatus.User != null ?
                            tweet.RetweetedStatus.User.ProfileImageUrl : tweet.User.ProfileImageUrl)
           }).ToList();

        foreach (var tweet in tweets)
        {
          Tweets.Add(tweet);
        }
      }
      catch (Exception ex)
      {
      }

      IsBusy = false;
    }


    public void Speak(int index)
    {
      var tweet = Tweets[index];

      CrossTextToSpeech.Current.Speak(tweet.Text);

    }
  }

}
