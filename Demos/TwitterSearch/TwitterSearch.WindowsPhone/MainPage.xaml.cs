using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TwitterSearch.WindowsPhone.Resources;
using TwitterSearch.Portable.ViewModels;

namespace TwitterSearch.WindowsPhone
{
  public partial class MainPage : PhoneApplicationPage
  {
    // Constructor
    public MainPage()
    {
      InitializeComponent();
      viewModel = new TwitterViewModel();
    }
    private TwitterViewModel viewModel;
    private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
    {
      Progress.Visibility = System.Windows.Visibility.Visible;

      await viewModel.LoadTweetsCommand(TextBoxSearch.Text);
      Tweets.ItemsSource = viewModel.Tweets;

      Progress.Visibility = System.Windows.Visibility.Collapsed;
    }

    private void Tweets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if(Tweets.SelectedItem == null)
        return;

      viewModel.Speak(Tweets.SelectedIndex);
    }

    
  }
}