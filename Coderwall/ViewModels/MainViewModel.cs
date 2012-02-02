using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using RestSharp;

using Coderwall.Models;

namespace Coderwall.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {

        }

        /// <summary>
        /// A collection of Badges objects.
        /// </summary>
        public ObservableCollection<BadgeViewModel> Badges { get; private set; }

        public ObservableCollection<string> Accomplishments { get; private set; }

        public object Endorsements { get; private set; }

        public User CurrentUser { get; private set; }

        public ImageSource Avatar { get; private set; }

        public string Summary { get; private set; }

        public string Specialities { get; private set; }

        public string Username { get; set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            var client = new RestClient();
            client.BaseUrl = "http://coderwall.com";

            Badges = new ObservableCollection<BadgeViewModel>();
            Accomplishments = new ObservableCollection<string>();

            var request = new RestRequest();
            request.Resource = Username + ".json?full=true";
            client.ExecuteAsync<User>(request, (response) =>
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    Debug.WriteLine(response.ErrorMessage);
                }
                else
                {
                    CurrentUser = response.Data;

                    foreach (BadgeObject badge in CurrentUser.Badges)
                        Badges.Add(new BadgeViewModel() { BadgeName = badge.Name, BadgeDescription = badge.Description, Badge = new System.Windows.Media.Imaging.BitmapImage(new Uri(badge.Badge,UriKind.Absolute)) });

                    foreach (string accomplishment in CurrentUser.Accomplishments)
                        Accomplishments.Add(accomplishment);

                    if (Accomplishments.Count == 0)
                        Accomplishments.Add("You Have Not Entered Any Accomplishments");

                    Summary = CurrentUser.Title;

                    if (CurrentUser.Title != null && CurrentUser.Company != null)
                        Summary += " at ";
                    
                    Summary += CurrentUser.Company;

                    if (Summary != "")
                        Summary += "\n";

                    Summary += CurrentUser.Location;
                
                    Specialities = string.Join(", ", CurrentUser.Specialities);

                    Avatar = new System.Windows.Media.Imaging.BitmapImage(new Uri(CurrentUser.Thumbnail+"?s=200", UriKind.Absolute));

                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentUser"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Summary"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Specialities"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Avatar"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Badges"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Accomplishments"));
                   
                    this.IsDataLoaded = true;
                }
            });
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        

    }
}