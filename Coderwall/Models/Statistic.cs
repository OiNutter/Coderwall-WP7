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
using System.Text.RegularExpressions;

namespace Coderwall.Models
{
    public class Statistic
    {
        public string Name { get; set; }
        public int Number { get; set; }

        private string _description;
        public string Description {
            get
            {
                return _description;
            }
            set
            {
                _description = Regex.Replace(value, @"\w+", (m) =>
                {
                    string tmp = m.Value;
                    return char.ToUpper(tmp[0]) + tmp.Substring(1, tmp.Length - 1).ToLower();
                });
            }
        }
    }
}
