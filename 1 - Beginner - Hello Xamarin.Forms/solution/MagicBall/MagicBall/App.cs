using System;
using Xamarin.Forms;

namespace MagicBall
{
    public class App
    {
        public static Page GetMainPage ()
        {	
            return new BallPage ();
        }
    }
}

