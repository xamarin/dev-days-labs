using TwitterSearch.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TwitterSearch.WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
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

      Progress.Visibility = System.Windows.Visibility.Hidden;
    }

    private void Tweets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (Tweets.SelectedItem == null)
        return;

      System.Diagnostics.Process.Start(viewModel.Tweets[Tweets.SelectedIndex].Url);
    }
  }
}
