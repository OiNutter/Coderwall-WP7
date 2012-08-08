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
using System.ComponentModel;

using System.Diagnostics;

using Coderwall.Models;
using Coderwall.ViewModels;

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
            App.ViewModel.PropertyChanged += new PropertyChangedEventHandler(CurrentUserChanged);

        }

        private void CurrentUserChanged(object sender, PropertyChangedEventArgs Event)
        {
            if (Event.PropertyName == "CurrentUser")
            {
                for(int i=StatsGrid.Children.Count-1;i>=0;i--){
                    StatsGrid.Children.RemoveAt(i);
                }

                int row = 0;
                int col = 0;
                Grid StatItem;
                SolidColorBrush BgBrush;
                Viewbox NumberBox;
                Viewbox DescriptionBox;
                TextBlock NumberText;
                TextBlock DescriptionText;
                if (App.ViewModel.CurrentUser.Stats != null)
                {
                    for (int j = 0; j < App.ViewModel.CurrentUser.Stats.Count; j++)
                    {
                        StatItem = new Grid();
                        BgBrush = new SolidColorBrush();
                        BgBrush.Color = Color.FromArgb(255,138,203,239);
                        StatItem.Background = BgBrush;
                        StatItem.Margin = new Thickness(7.5);

                        //Add Number
                        NumberBox = new Viewbox();

                        NumberText = new TextBlock();
                        NumberText.Text = App.ViewModel.CurrentUser.Stats[j].Number.ToString();
                        NumberText.TextAlignment = TextAlignment.Center;
                        NumberText.HorizontalAlignment = HorizontalAlignment.Stretch;
                        NumberText.TextTrimming = TextTrimming.None;
                        NumberText.VerticalAlignment = VerticalAlignment.Top;
                        NumberText.Style = (Style)Application.Current.Resources["PhoneTextBlockBase"];
                        NumberText.FontSize = (double)Application.Current.Resources["PhoneFontSizeHuge"];
                        NumberText.FontFamily = (FontFamily)Application.Current.Resources["PhoneFontFamilyLight"];
                        NumberText.Margin = new Thickness(12, -45, 12, 0);

                        NumberBox.Child = NumberText;
                        StatItem.Children.Add(NumberBox);

                        //Add Description
                        DescriptionBox = new Viewbox();
                        DescriptionBox.StretchDirection = StretchDirection.DownOnly;
                        DescriptionBox.VerticalAlignment = VerticalAlignment.Bottom;
                        DescriptionBox.HorizontalAlignment = HorizontalAlignment.Left;

                        DescriptionText = new TextBlock();
                        DescriptionText.Text = App.ViewModel.CurrentUser.Stats[j].Description;
                        DescriptionText.TextAlignment = TextAlignment.Left;
                        DescriptionText.HorizontalAlignment = HorizontalAlignment.Left;
                        DescriptionText.TextTrimming = TextTrimming.None;
                        DescriptionText.VerticalAlignment = VerticalAlignment.Bottom;
                        DescriptionText.Style = (Style)Application.Current.Resources["PhoneTextSubtleStyle"];
                        DescriptionText.Margin = new Thickness(7.5);

                        DescriptionBox.Child = DescriptionText;
                        StatItem.Children.Add(DescriptionBox);

                        StatsGrid.Children.Add(StatItem);
                        Grid.SetColumn(StatItem, col);
                        Grid.SetRow(StatItem, row);
                        col = col + 1;

                        //Set up any required row spanning
                        if (App.ViewModel.CurrentUser.Stats.Count < 3 && j == 0)
                        {
                            Grid.SetColumnSpan(StatItem, 2);
                            col = 2;
                        }

                        if (col >= 2)
                        {
                            col = 0;
                            row = row + 1;
                        }
                    }
                }

                StatItem = new Grid();
                BgBrush = new SolidColorBrush();
                BgBrush.Color = Color.FromArgb(255,138,203,239);
                StatItem.Background = BgBrush;
                StatItem.Margin = new Thickness(7.5);

                //Add Number
                NumberBox = new Viewbox();

                NumberText = new TextBlock();
                NumberText.Text = App.ViewModel.CurrentUser.Endorsements.ToString();
                NumberText.TextAlignment = TextAlignment.Center;
                NumberText.HorizontalAlignment = HorizontalAlignment.Stretch;
                NumberText.TextTrimming = TextTrimming.None;
                NumberText.VerticalAlignment = VerticalAlignment.Top;
                NumberText.Style = (Style)Application.Current.Resources["PhoneTextBlockBase"];
                NumberText.FontSize = (double)Application.Current.Resources["PhoneFontSizeHuge"];
                NumberText.FontFamily = (FontFamily)Application.Current.Resources["PhoneFontFamilyLight"];
                NumberText.Margin = new Thickness(12, -45, 12, 0);

                NumberBox.Child = NumberText;
                StatItem.Children.Add(NumberBox);

                //Add Description
                DescriptionBox = new Viewbox();
                DescriptionBox.StretchDirection = StretchDirection.DownOnly;
                DescriptionBox.VerticalAlignment = VerticalAlignment.Bottom;
                DescriptionBox.HorizontalAlignment = HorizontalAlignment.Left;

                DescriptionText = new TextBlock();
                DescriptionText.Text = "Endorsements";
                DescriptionText.TextAlignment = TextAlignment.Left;
                DescriptionText.HorizontalAlignment = HorizontalAlignment.Left;
                DescriptionText.TextTrimming = TextTrimming.None;
                DescriptionText.VerticalAlignment = VerticalAlignment.Bottom;
                DescriptionText.Style = (Style)Application.Current.Resources["PhoneTextSubtleStyle"];
                DescriptionText.Margin = new Thickness(7.5);

                DescriptionBox.Child = DescriptionText;
                StatItem.Children.Add(DescriptionBox);

                StatsGrid.Children.Add(StatItem);
                Grid.SetColumn(StatItem,col);
                Grid.SetRow(StatItem, row);

                if (App.ViewModel.CurrentUser.Stats == null || App.ViewModel.CurrentUser.Stats.Count < 2)
                    Grid.SetColumnSpan(StatItem, 2);

                if (App.ViewModel.CurrentUser.Stats == null)
                {
                    StatsGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                    StatsGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Star);
                    StatsGrid.Height = 220;
                }
                else
                {
                    StatsGrid.RowDefinitions[0].Height = new GridLength(0.5, GridUnitType.Star);
                    StatsGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
                    StatsGrid.Height = 440;
                }
            }
        }

        private void CreateApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.BackgroundColor = Color.FromArgb(255,138,203,239);
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

                // Move Stat Boxes
                int col = 0;
                int row = 0;
                for (int i = 0; i < StatsGrid.Children.Count; i++)
                {
                    Grid.SetRow((FrameworkElement)StatsGrid.Children.ElementAt(i), row);
                    Grid.SetColumn((FrameworkElement)StatsGrid.Children.ElementAt(i), col);
                    Grid.SetColumnSpan((FrameworkElement)StatsGrid.Children.ElementAt(i), 1);
                    col = col + 1;

                    if ((StatsGrid.Children.Count < 4 && i == 0) || (StatsGrid.Children.Count < 3 && i == 1))
                    {
                        Grid.SetColumnSpan((FrameworkElement)StatsGrid.Children.ElementAt(i), 2);
                        col = 2;
                    }

                    if (col >= 2)
                    {
                        col = 0;
                        row = row + 1;
                    }

                }

                //Set Dimensions on Stat Boxes
                BoxWidth = (MainPivot.Width) / 2;
                StatsGrid.ColumnDefinitions[0].Width = new GridLength(0.5,GridUnitType.Star);
                StatsGrid.ColumnDefinitions[1].Width = new GridLength(0.5,GridUnitType.Star);
                StatsGrid.ColumnDefinitions[2].Width = new GridLength(0);
                StatsGrid.ColumnDefinitions[3].Width = new GridLength(0);

                if (StatsGrid.Children.Count > 1)
                {
                    StatsGrid.Height = 440;
                    StatsGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
                }
                else
                {
                    StatsGrid.Height = 220;
                    StatsGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Star);
                }

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
                int col = 0;
                for (int i = 0; i < StatsGrid.Children.Count; i++)
                {
                    Grid.SetRow((FrameworkElement)StatsGrid.Children.ElementAt(i),0);
                    Grid.SetColumn((FrameworkElement)StatsGrid.Children.ElementAt(i), col);
                    Grid.SetColumnSpan((FrameworkElement)StatsGrid.Children.ElementAt(i), 1);
                    col = col + 1;

                }
                
                //Set Dimensions on Stat Boxes
                for (int j = 0; j < StatsGrid.ColumnDefinitions.Count; j++)
                {
                    double ColWidth;
                    
                    if (j < StatsGrid.Children.Count)
                        ColWidth = (double)(1.0 / StatsGrid.Children.Count);
                    else
                        ColWidth = 0;

                    StatsGrid.ColumnDefinitions[j].Width = new GridLength(ColWidth, GridUnitType.Star);
                }
               
                StatsGrid.Height = 220;
                StatsGrid.RowDefinitions[1].Height = new GridLength(0);
                
            }
        }

    }
}