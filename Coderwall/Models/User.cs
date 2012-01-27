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

using System.Collections.Generic;

namespace Coderwall.Models
{
    public class User
    {
        public string Username{ get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public int Endorsements { get; set; }
        public List<BadgeObject> Badges { get; set; }
        public List<string> Accomplishments { get; set; }
        public List<Object> Stats { get; set; }
        public List<string> Specialities { get; set; }
    }
}
