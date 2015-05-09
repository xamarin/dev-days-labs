using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripExpenses.Models;
using TripExpenses.ViewModels;
using Xamarin.Forms;

namespace TripExpenses.Views
{
	public partial class ExpenseListPage
	{
		private ExpensesViewModel viewModel;

		public ExpenseListPage()
		{
			InitializeComponent();

			this.BindingContext = viewModel = new ExpensesViewModel();

			ToolbarItems.Add(new ToolbarItem
			  {
				  Name = "refresh",
				  Icon = "refresh.png",
				  Command = viewModel.LoadExpenses
			  });

			ExpenseList.ItemTapped += (sender, args) =>
			{
				if (ExpenseList.SelectedItem == null)
					return;

				var detailPage = new DetailsPage(args.Item as TripExpense);
				Navigation.PushAsync(detailPage);

				ExpenseList.SelectedItem = null;
			};

			ButtonNewExpense.Clicked += async (sender, args) =>
			{
				var detailPage = new DetailsPage(null);
				await Navigation.PushAsync(detailPage);
			};
		}


		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (!viewModel.Initialized)
				viewModel.LoadExpenses.Execute(null);
		}
	}
}
