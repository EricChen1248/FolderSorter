using System;
using System.Linq;
using System.Windows.Controls;
using FolderSorter.Classes;

namespace FolderSorter.User_Controls
{
    /// <summary>
    /// Interaction logic for RuleLog.xaml
    /// </summary>
    public partial class RuleLog
    {
        private Rule rule;
        internal RuleLog(Rule rule)
        {
            this.rule = rule;
            InitializeComponent();
            RuleNameLabel.Content = rule.Name;

            var destination = rule.DestinationFolder.Split('\\');
            var destinationString = "";
            var i = destination.Length;
            while (destinationString.Length < 20)
            {
                destinationString = destination[--i] + "\\" + destinationString;
            }

            DestinationLabel.Content = "...\\" + destinationString;
        }

        private void Settings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            rule
        }
    }
}
