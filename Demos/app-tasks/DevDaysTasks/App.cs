using Xamarin.Forms;

namespace DevDaysTasks
{
	public class App : Application
	{
		public App ()
		{
            MainPage = new NavigationPage(new TodoListPage())
            {
                BarBackgroundColor = Color.FromHex("5ABAFF"),
                BarTextColor = Color.White
            };         
		}
	}
}

