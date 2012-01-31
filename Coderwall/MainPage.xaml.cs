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

using System.Diagnostics;

namespace Coderwall
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //Create Application Bar
            CreateApplicationBar();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void CreateApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.BackgroundColor = Color.FromArgb(255, 200, 60, 50);
            ApplicationBar.ForegroundColor = Colors.White;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // Add Buttons
            ApplicationBarIconButton searchButton = new ApplicationBarIconButton();
            searchButton.IconUri = new Uri("/Icons/appbar.feature.search.rest.png", UriKind.Relative);
            searchButton.Text = "Search";
            searchButton.Click += new EventHandler(SearchButton_Click);
            ApplicationBar.Buttons.Add(searchButton);

            ApplicationBarIconButton settingsButton = new ApplicationBarIconButton();
            settingsButton.IconUri = new Uri("/Icons/appbar.feature.settings.rest.png", UriKind.Relative);
            settingsButton.Text = "Settings";
            settingsButton.Click += new EventHandler(Settings_Click);
            ApplicationBar.Buttons.Add(settingsButton);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml",UriKind.Relative));
        }

    }
}