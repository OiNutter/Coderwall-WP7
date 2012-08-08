using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;


namespace Coderwall.Models
{
    public class UserCache
    {
        public IsolatedStorageSettings CachedSettings { get; private set; }

        public UserCache()
        {
            CachedSettings = IsolatedStorageSettings.ApplicationSettings;
        }

        public void StoreUser(User CurrentUser)
        {
            DateTime ValidUntil = new DateTime();
            ValidUntil = DateTime.Now + (new TimeSpan(24, 0, 0));

            StoreValue("ValidUntil", ValidUntil);
            StoreValue("CachedUsername", CurrentUser.Username);
            StoreValue("CachedName", CurrentUser.Name);
            StoreValue("CachedLocation", CurrentUser.Location);
            StoreValue("CachedTitle", CurrentUser.Title);
            StoreValue("CachedCompany", CurrentUser.Company);
            StoreValue("CachedBadges", CurrentUser.Badges);
            StoreValue("CachedAccomplishments", CurrentUser.Accomplishments);
            StoreValue("CachedStats", CurrentUser.Stats);
            StoreValue("CachedEndorsements", CurrentUser.Endorsements);
            StoreValue("CachedThumbnail", CurrentUser.Thumbnail);
            StoreValue("CachedSpecialities", CurrentUser.Specialities);
          
        }

        private void StoreValue(string Key, Object Value)
        {
            if (!CachedSettings.Contains(Key))
                CachedSettings.Add(Key, Value);
            else
                CachedSettings[Key] = Value;
        }

    }


}
