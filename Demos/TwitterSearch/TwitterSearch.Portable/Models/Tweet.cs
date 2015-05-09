using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterSearch.Portable.Models
{
  public class Tweet
  {
    public Tweet()
    {
    }

    public string DisplayName
    {
      get { return ScreenName + " at " + Date; }
    }

    public string Text { get; set; }

    public ulong StatusID { get; set; }

    public string ScreenName { get; set; }



    public string Date { get { return CreatedAt.ToString("g"); } }
    public string RTCount { get { return CurrentUserRetweet == 0 ? string.Empty : CurrentUserRetweet + " RT"; } }

    public string Image { get; set; }

    public string Url { get; set; }

    public DateTime CreatedAt
    {
      get;
      set;
    }

    public ulong CurrentUserRetweet
    {
      get;
      set;
    }
  }
}
