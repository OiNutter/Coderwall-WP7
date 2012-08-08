using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Coderwall
{

    public class BadgeViewModel : INotifyPropertyChanged
    {

        private string _badgeName;
        /// <summary>
        /// The name of the badge
        /// </summary>
        /// <returns></returns>
        public string BadgeName
        {
            get
            {
                return _badgeName;
            }
            set
            {
                if (value != _badgeName)
                {
                    _badgeName = value;
                    NotifyPropertyChanged("BadgeName");
                }
            }
        }
        private ImageSource _badge;
        /// <summary>
        /// The sprite containing the badge image
        /// </summary>
        /// 
        public ImageSource Badge
        {
            get
            {
                return _badge;
            }
            set
            {
                if (value != _badge)
                {
                    _badge = value;
                    NotifyPropertyChanged("Badge");
                }
            }
        }
        private string _badgeDescription;
        /// <summary>
        /// The summary of what the badge is for
        /// </summary>
        public string BadgeDescription
        {
            get
            {
                return _badgeDescription;
            }
            set
            {
                if (value != _badgeDescription)
                {
                    _badgeDescription = value;
                    NotifyPropertyChanged("BadgeDescription");
                }
            }
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
