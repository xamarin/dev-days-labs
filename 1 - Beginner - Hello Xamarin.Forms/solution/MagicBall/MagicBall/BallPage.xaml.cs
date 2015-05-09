using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MagicBall
{	
	public partial class BallPage : ContentPage
	{	
        string[] options = {" It is certain"
            , " It is decidedly so"
            , " Without a doubt"
            , " Yes definitely"
            , " You may rely on it"
            , " As I see it, yes"
            , " Most likely"
            , " Outlook good"
            , " Yes"
            , " Signs point to yes"

            , " Reply hazy try again"
            , " Ask again later"
            , " Better not tell you now"
            , " Cannot predict now"
            , " Concentrate and ask again"

            , " Don't count on it"
            , " My reply is no"
            , " My sources say no"
            , " Outlook not so good"
            , " Very doubtful "
        };

		public BallPage ()
		{
			InitializeComponent ();
		}

        void ButtonClicked (object sender, EventArgs args)
        {
            label.Text = options [new Random ().Next (options.Length - 1)];  
        }
	}
}

