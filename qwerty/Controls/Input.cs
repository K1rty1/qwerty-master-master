using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace qwerty.Controls
{
    public class Input:TextBox
    {
        public string Placeholder { get; set; }
        public Input()
        {
            Loaded += AddText;
            GotFocus += RemoveText;
        }
        private void RemoveText(object sender,System.Windows.RoutedEventArgs e)
        {
            if (Text==Placeholder)
            {
                Text = "";
            }
        }
        private void AddText(object sender, System.Windows.RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Text))
            {
                Text = Placeholder;
            }
        }
    }
}
