using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace XamarinInsights
{
	public class BaseModel: INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public BaseModel ()
		{

		}

		internal bool ProcPropertyChanged<T> (ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			return PropertyChanged.SetProperty (this, ref currentValue, newValue, propertyName);
		}

		internal void ProcPropertyChanged (string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
			}
		}
	}

	public static class BaseNotify
	{

		public static bool SetProperty<T> (this PropertyChangedEventHandler handler, object sender, ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals (currentValue, newValue))
				return false;
			currentValue = newValue;

			if (handler == null)
				return true;

			handler.Invoke (sender, new PropertyChangedEventArgs (propertyName));
			return true;
		}
	}
}

