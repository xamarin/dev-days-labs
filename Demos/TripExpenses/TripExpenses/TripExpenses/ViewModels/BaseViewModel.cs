using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TripExpenses.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

      private bool isBusy;
      public bool IsBusy
      {
        get { return isBusy;  }
        set
        {
          if (isBusy == value)
            return;

          isBusy = value;
          OnPropertyChanged("IsBusy");
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      protected void OnPropertyChanged(string propertyName)
      {
        if (PropertyChanged == null)
          return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
}
