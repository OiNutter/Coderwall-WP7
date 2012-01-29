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

using RestSharp;

using Coderwall.Models;


namespace Coderwall
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Badges = new ObservableCollection<BadgeViewModel>();
            this.Accomplishments = new ObservableCollection<string>();
            this.Avatar = new System.Windows.Media.Imaging.BitmapImage(new Uri("https://secure.gravatar.com/avatar/1863cc77d322b858d85b6ff665749e63?s=200", UriKind.Absolute));
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
            client.BaseUrl = "http://api.coderwall.com";

            var request = new RestRequest();
            request.Resource = "oinutter.json?full=true";
            client.ExecuteAsync<User>(request, (response) =>
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    Debug.WriteLine(response.ErrorMessage);
                }
                else
                {
                    CurrentUser = response.Data;
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentUser"));
                    foreach (BadgeObject badge in CurrentUser.Badges)
                        Badges.Add(new BadgeViewModel() { BadgeName = badge.Name, BadgeDescription = badge.Description, Badge = new System.Windows.Media.Imaging.BitmapImage(new Uri(badge.Badge,UriKind.Absolute)) });

                    foreach (string accomplishment in CurrentUser.Accomplishments)
                        Accomplishments.Add(accomplishment);

                    if (Accomplishments.Count == 0)
                        Accomplishments.Add("You Have Not Entered Any Accomplishments");

                    Summary = CurrentUser.Title;

                    if (CurrentUser.Title != "" && CurrentUser.Company != "")
                        Summary += " at ";
                    
                    Summary += CurrentUser.Company;

                    if (Summary != "")
                        Summary += "\n";

                    Summary += CurrentUser.Location;
                
                    Specialities = string.Join(", ", CurrentUser.Specialities);

                    PropertyChanged(this, new PropertyChangedEventArgs("Summary"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Specialities"));
                   
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