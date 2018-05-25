using Xamarin.Forms;

namespace DevDaysTasks
{
	public class App : Application
	{
		public App ()
		{
            MainPage = new NavigationPage(new TodoList())
            {
                BarBackgroundColor = Color.FromHex("#5ABAFF"),
                BarTextColor = Color.White
            };         
		}
	}
}

