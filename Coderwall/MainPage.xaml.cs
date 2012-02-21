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

using Coderwall.Models;

namespace Coderwall
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();



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
            AppSettings AppSettings = new AppSettings();
            if (App.ViewModel.Username != AppSettings.UsernameSetting)
            {
                ApplicationBarIconButton MyWallButton = new ApplicationBarIconButton();
                MyWallButton.IconUri = new Uri("/Icons/appbar.people.png", UriKind.Relative);
                MyWallButton.Text = "My Wall";
                MyWallButton.Click += new EventHandler(MyWallButton_Click);
                ApplicationBar.Buttons.Add(MyWallButton);
            }

            ApplicationBarIconButton searchButton = new ApplicationBarIconButton();
            searchButton.IconUri = new Uri("/Icons/appbar.feature.search.rest.png", UriKind.Relative);
            searchButton.Text = "Search";
            searchButton.Click += new EventHandler(SearchButton_Click);
            ApplicationBar.Buttons.Add(searchButton);

            ApplicationBarIconButton settingsButton = new ApplicationBarIconButton();
            settingsButton.IconUri = new Uri("/Icons/appbar.feature.settings.rest.png", UriKind.Relative);
            settingsButton.Text = "Settings";
            settingsButton.Click += new EventHandler(SettingsButton_Click);
            ApplicationBar.Buttons.Add(settingsButton);

            ApplicationBarIconButton refreshButton = new ApplicationBarIconButton();
            refreshButton.IconUri = new Uri("/Icons/appbar.sync.rest.png", UriKind.Relative);
            refreshButton.Text = "Refresh";
            refreshButton.Click += new EventHandler(RefreshButton_Click);
            ApplicationBar.Buttons.Add(refreshButton);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                //Create Application Bar
                CreateApplicationBar();
                App.ViewModel.LoadData();
            }

        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml",UriKind.Relative));
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            AppSettings AppSettings = new AppSettings();
            if (App.ViewModel.Username == AppSettings.UsernameSetting)
                App.ViewModel.ShouldCache = true;
            else
                App.ViewModel.ShouldCache = false;

            App.ViewModel.GoBack = false;
            App.ViewModel.IgnoreCache = true;
            App.ViewModel.LoadData();
        }

        private void MyWallButton_Click(object sender, EventArgs e)
        {
            AppSettings AppSettings = new AppSettings();
            App.ViewModel.Username = AppSettings.UsernameSetting;
            App.ViewModel.IgnoreCache = false;
            App.ViewModel.ShouldCache = true;
            //Create Application Bar
            CreateApplicationBar();
            App.ViewModel.LoadData();
        }

        private void MainPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {

            double BoxWidth;
            if ((e.Orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
            {
                // Set view width to full width of screen
                MainPivot.Width = LayoutRoot.ActualWidth;
                MainPivot.HorizontalAlignment = HorizontalAlignment.Stretch;
                Header.Margin = new Thickness(20, 10, 20, 10);
                
                //Move Profile Stats
                Grid.SetRow(ProfileDetails, 1);
                Grid.SetColumn(ProfileDetails, 0);
                FullName.TextAlignment = TextAlignment.Center;
                About.TextAlignment = TextAlignment.Center;
                Avatar.Margin = new Thickness(0, 0, 0, 0);
                ProfileGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                ProfileGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);

                //Move Stat Boxes
                Grid.SetRow(Stat3, 1);
                Grid.SetColumn(Stat3, 0);
                Grid.SetRow(Stat4, 1);
                Grid.SetColumn(Stat4, 1);

                //Set Dimensions on Stat Boxes
                BoxWidth = (MainPivot.Width) / 2;
                StatsGrid.ColumnDefinitions[0].Width = new GridLength(0.5,GridUnitType.Star);
                StatsGrid.ColumnDefinitions[1].Width = new GridLength(0.5,GridUnitType.Star);
                StatsGrid.ColumnDefinitions[2].Width = new GridLength(0);
                StatsGrid.ColumnDefinitions[3].Width = new GridLength(0);

                StatsGrid.Height = 440;
                StatsGrid.RowDefinitions[1].Height = new GridLength(0.5,GridUnitType.Star);

            }

            // If not in the portrait mode, move buttonList content to a visible row and column.

            else
            {
                // Adjust View Width To Compensate for Application Bar showing
                if (ApplicationBar == null)
                    ApplicationBar = new ApplicationBar();

                MainPivot.Width = LayoutRoot.ActualWidth - ApplicationBar.DefaultSize;
                if ((e.Orientation & PageOrientation.LandscapeLeft) == PageOrientation.LandscapeLeft)
                {
                    MainPivot.HorizontalAlignment = HorizontalAlignment.Left;
                    Header.Margin = new Thickness(20,10,20,10);
                }
                else
                {
                    MainPivot.HorizontalAlignment = HorizontalAlignment.Right;
                    Header.Margin = new Thickness(ApplicationBar.DefaultSize, 10, 20, 10);
                }

                // Move Profile Stats
                Grid.SetRow(ProfileDetails, 0);
                Grid.SetColumn(ProfileDetails, 1);
                Avatar.Margin = new Thickness(0, 0, 10, 0);
                FullName.TextAlignment = TextAlignment.Left;
                About.TextAlignment = TextAlignment.Left;
                ProfileGrid.ColumnDefinitions[0].Width = new GridLength(1,GridUnitType.Auto);
                ProfileGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                // Move Stat Boxes
                Grid.SetRow(Stat3, 0);
                Grid.SetColumn(Stat3, 2);
                Grid.SetRow(Stat4, 0);
                Grid.SetColumn(Stat4, 3);

                //Set Dimensions on Stat Boxes
                StatsGrid.ColumnDefinitions[0].Width = new GridLength(0.25,GridUnitType.Star);
                StatsGrid.ColumnDefinitions[1].Width = new GridLength(0.25, GridUnitType.Star);
                StatsGrid.ColumnDefinitions[2].Width = new GridLength(0.25,GridUnitType.Star);
                StatsGrid.ColumnDefinitions[3].Width = new GridLength(0.25,GridUnitType.Star);

                StatsGrid.Height = 220;
                StatsGrid.RowDefinitions[1].Height = new GridLength(0);
                
            }
        }

    }
}