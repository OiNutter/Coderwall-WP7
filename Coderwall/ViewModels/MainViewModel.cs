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
        }

        /// <summary>
        /// A collection of Badges objects.
        /// </summary>
        public ObservableCollection<BadgeViewModel> Badges { get; private set; }

        public User CurrentUser { get; private set; }

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
                    foreach (BadgeObject badge in response.Data.Badges)
                    {
                        Badges.Add(new BadgeViewModel() { BadgeName = badge.Name, BadgeDescription = badge.Description, Badge = new System.Windows.Media.Imaging.BitmapImage(new Uri(badge.Badge,UriKind.Absolute)) });
                    }

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