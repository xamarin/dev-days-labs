using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripExpenses.Models;
using TripExpenses.ViewModels;
using Xamarin.Forms;
using TripExpenses.UI;

namespace TripExpenses.Views
{
	public partial class DetailsPage
	{
		
		public DetailViewModel ViewModel
		{
			get { return BindingContext as DetailViewModel; }
			set { BindingContext = value; }
		}

		public DetailsPage(TripExpense expense)
		{
			InitializeComponent();
			Type type = typeof(MultiTriggerConverter);
			foreach (var item in DetailViewModel.Categories)
				PickerCategory.Items.Add(item);

			ViewModel = new DetailViewModel(expense, Navigation);
		}
	}
}
