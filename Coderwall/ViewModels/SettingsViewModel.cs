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

using Coderwall.Models;

namespace Coderwall.ViewModels
{
    public class SettingsViewModel
    {

        public SettingsViewModel()
        {
            this.AppSettings = new AppSettings();
        }

        public AppSettings AppSettings { get; private set; }

    }
}
