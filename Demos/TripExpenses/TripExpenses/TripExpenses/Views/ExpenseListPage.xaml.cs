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

			AddItem.Command = new Command (() => {
				var detailPage = new DetailsPage (null);
				Navigation.PushAsync (detailPage);
			});


			ExpenseList.ItemTapped += (sender, args) =>
			{
				if (ExpenseList.SelectedItem == null)
					return;

				var detailPage = new DetailsPage(args.Item as TripExpense);
				Navigation.PushAsync(detailPage);

				ExpenseList.SelectedItem = null;
			};
				

		}

		public void OnDelete (object sender, EventArgs e) {
			var mi = ((MenuItem)sender);
			viewModel.DeleteExpense.Execute (mi.CommandParameter);
		}


		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (!viewModel.Initialized)
				viewModel.LoadExpenses.Execute(null);
		}
	}
}