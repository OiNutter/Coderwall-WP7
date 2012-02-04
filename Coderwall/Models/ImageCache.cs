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
    public class ImageCache
    {
        private IsolatedStorageFile Cache { get; set; }

        public ImageCache()
        {
            Cache = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public StoreImage(){

        }

    }
}
