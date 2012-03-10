﻿using System;
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
            IgnoreCache = false;
            ShouldCache = true;
            Username = "";
            GoBack = false;
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

        public bool IgnoreCache { get; set; }

        public bool ShouldCache { get; set; }

        public bool GoBack { get; set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void ForceReload()
        {
            IsDataLoaded = false;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            UserCache CurrentCached = new UserCache();
            IsolatedStorageSettings CachedUser = CurrentCached.CachedSettings;
            Badges = new ObservableCollection<BadgeViewModel>();
            Accomplishments = new ObservableCollection<string>();

            if (!IgnoreCache && (CachedUser.Contains("ValidUntil") && (DateTime)CachedUser["ValidUntil"] > DateTime.Now))
            {
                LoadUserFromCache(CachedUser);
            }
            else
            {

                RestClient client = new RestClient();
                client.BaseUrl = "http://coderwall.com";

                RestRequest request = new RestRequest();
                Random Random = new Random((int)DateTime.Now.Ticks);
                request.Resource = Username + ".json?full=true&rand=" + Random.Next(1,100000000);
                client.ExecuteAsync<User>(request, (response) =>
                {

                    if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                            MessageBox.Show("Sorry, there seems to be a problem finding the requested user. Please check you typed the username correctly and that the user exists");
                        else
                            MessageBox.Show("Sorry, we seem to be having problems loading the requested user. Please try again in a few minutes");

                        if (GoBack)
                        {
                            Microsoft.Phone.Controls.PhoneApplicationFrame Root = (Microsoft.Phone.Controls.PhoneApplicationFrame)Application.Current.RootVisual;
                            Root.GoBack();
                        }
                        else
                        {
                            LoadUserFromCache(CachedUser);
                        }
                           

                    }
                    else if (response.ResponseStatus == ResponseStatus.TimedOut)
                    {
                        MessageBox.Show("Sorry, we couldn't connect to the server. Please check your are connected to the internet");
                    }
                    else
                    {
                        CurrentUser = response.Data;
                        ProcessUser(CurrentUser, false);
                        if (ShouldCache)
                        {
                            CurrentCached.StoreUser(CurrentUser);
                            IgnoreCache = false;
                        }
                        this.IsDataLoaded = true;
                    }

                });

            }
        }

        private void LoadUserFromCache(IsolatedStorageSettings CachedUser)
        {
            CurrentUser = new User()
            {
                Username = (string)CachedUser["CachedUsername"],
                Name = (string)CachedUser["CachedName"],
                Title = (string)CachedUser["CachedTitle"],
                Company = (string)CachedUser["CachedCompany"],
                Location = (string)CachedUser["CachedLocation"],
                Specialities = (List<string>)CachedUser["CachedSpecialities"],
                Badges = (List<BadgeObject>)CachedUser["CachedBadges"],
                Accomplishments = (List<string>)CachedUser["CachedAccomplishments"],
                Stats = (List<Statistic>)CachedUser["CachedStats"],
                Endorsements = (int)CachedUser["CachedEndorsements"],
                Thumbnail = (string)CachedUser["CachedThumbnail"]
            };

            ProcessUser(CurrentUser, true);
        }

        private void ProcessUser(User ThisUser,bool Cached){

            ImageCache ImageCache = new ImageCache();

            foreach (BadgeObject badge in ThisUser.Badges)
            {
                Uri BadgeUri = new Uri(badge.Badge, UriKind.Absolute);
                ImageSource BadgeSource;
                if (Cached && ImageCache.ImageExists(BadgeUri))
                {
                    BadgeSource = ImageCache.LoadImage(BadgeUri);
                }
                else
                {
                    BitmapImage BadgeBitmap = new BitmapImage(BadgeUri);
                    BadgeSource = BadgeBitmap;
                    if (ShouldCache)
                        BadgeBitmap.ImageOpened += new EventHandler<RoutedEventArgs>(ImageCache.StoreImage);
                }

                Badges.Add(
                    new BadgeViewModel()
                    {
                        BadgeName = badge.Name,
                        BadgeDescription = badge.Description,
                        Badge = BadgeSource
                    }
               );
            }
            

            foreach (string accomplishment in ThisUser.Accomplishments)
                Accomplishments.Add(accomplishment);

                if (Accomplishments.Count == 0)
                    Accomplishments.Add("You Have Not Entered Any Accomplishments");

                Summary = ThisUser.Title;

                if (ThisUser.Title != null && ThisUser.Company != null)
                    Summary += " at ";
                    
                Summary += ThisUser.Company;

                if (Summary != "")
                    Summary += "\n";

                Summary += ThisUser.Location;
                
                Specialities = string.Join(", ", ThisUser.Specialities);

                Uri AvatarUri = new Uri(ThisUser.Thumbnail + "?s=200", UriKind.Absolute);
                if (Cached && ImageCache.ImageExists(AvatarUri))
                {
                    Avatar = ImageCache.LoadImage(AvatarUri);
                }
                else
                {
                    BitmapImage AvatarBitmap = new BitmapImage(AvatarUri);
                    Avatar = AvatarBitmap;
                    if (ShouldCache)
                        AvatarBitmap.ImageOpened += new EventHandler<RoutedEventArgs>(ImageCache.StoreImage);

                }

                PropertyChanged(this, new PropertyChangedEventArgs("CurrentUser"));
                PropertyChanged(this, new PropertyChangedEventArgs("Summary"));
                PropertyChanged(this, new PropertyChangedEventArgs("Specialities"));
                PropertyChanged(this, new PropertyChangedEventArgs("Avatar"));
                PropertyChanged(this, new PropertyChangedEventArgs("Badges"));
                PropertyChanged(this, new PropertyChangedEventArgs("Accomplishments"));
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