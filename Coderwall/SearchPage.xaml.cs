using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Coderwall
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
            CreateApplicationBar();
        }

        private void CreateApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.BackgroundColor = Color.FromArgb(255,138,203,239);
            ApplicationBar.ForegroundColor = Colors.White;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // Add Buttons
            ApplicationBarIconButton SearchButton = new ApplicationBarIconButton();
            SearchButton.IconUri = new Uri("/Icons/appbar.feature.search.rest.png", UriKind.Relative);
            SearchButton.Text = "Search";
            SearchButton.Click += new EventHandler(SearchButton_Click);
            ApplicationBar.Buttons.Add(SearchButton);

            ApplicationBarIconButton CancelButton = new ApplicationBarIconButton();
            CancelButton.IconUri = new Uri("/Icons/appbar.cancel.rest.png", UriKind.Relative);
            CancelButton.Text = "Cancel";
            CancelButton.Click += new EventHandler(CancelButton_Click);
            ApplicationBar.Buttons.Add(CancelButton);

        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            App.ViewModel.IgnoreCache = true;
            App.ViewModel.ShouldCache = false;
            App.ViewModel.Username = SearchBox.Text;
            App.ViewModel.GoBack = true;
            App.ViewModel.ForceReload();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SearchBox_GotFocus(object sender, EventArgs e)
        {
            SolidColorBrush ForegroundBrush = new SolidColorBrush();
            ForegroundBrush.Color = Colors.Black;
            SearchBox.Foreground = ForegroundBrush;

            SolidColorBrush BackgroundBrush = new SolidColorBrush();
            BackgroundBrush.Color = Colors.White;
            SearchBox.Background = BackgroundBrush;
        }
    }
}