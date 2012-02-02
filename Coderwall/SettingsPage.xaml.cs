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

using Coderwall.ViewModels;

namespace Coderwall
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private SettingsViewModel SettingsModel;

        public SettingsPage()
        {
            InitializeComponent();
            CreateApplicationBar();
            this.SettingsModel = new SettingsViewModel();
            this.DataContext = SettingsModel;
        }

        private void CreateApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.BackgroundColor = Color.FromArgb(255, 200, 60, 50);
            ApplicationBar.ForegroundColor = Colors.White;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // Add Buttons
            ApplicationBarIconButton OKButton = new ApplicationBarIconButton();
            OKButton.IconUri = new Uri("/Icons/appbar.check.rest.png", UriKind.Relative);
            OKButton.Text = "Confirm";
            OKButton.Click += new EventHandler(OKButton_Click);
            ApplicationBar.Buttons.Add(OKButton);

            ApplicationBarIconButton CancelButton = new ApplicationBarIconButton();
            CancelButton.IconUri = new Uri("/Icons/appbar.cancel.rest.png", UriKind.Relative);
            CancelButton.Text = "Cancel";
            CancelButton.Click += new EventHandler(CancelButton_Click);
            ApplicationBar.Buttons.Add(CancelButton);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            SettingsModel.AppSettings.UsernameSetting = TextBoxUserName.Text;
            App.ViewModel.Username = TextBoxUserName.Text;
            App.ViewModel.LoadData();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void TextBoxUserName_GotFocus(object sender, EventArgs e)
        {
            SolidColorBrush ForegroundBrush = new SolidColorBrush();
            ForegroundBrush.Color = Colors.White;
            TextBoxUserName.Foreground = ForegroundBrush;

            SolidColorBrush BackgroundBrush = new SolidColorBrush();
            BackgroundBrush.Color = Color.FromArgb(255,171,156,115);
            TextBoxUserName.Background = BackgroundBrush;
        }
    }
}