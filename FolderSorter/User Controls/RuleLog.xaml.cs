using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using FolderSorter.Classes;
using FolderSorter.Pages;

namespace FolderSorter.User_Controls
{
    /// <summary>
    /// Interaction logic for RuleLog.xaml
    /// </summary>
    public partial class RuleLog
    {
        private readonly Rule _rule;
        internal RuleLog(Rule rule)
        {
            _rule = rule;
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

            if (rule.FallBack)
            {
                DeleteRuleBtn.IsEnabled = false;
                var path = DeleteRuleBtn.Content as Path;
                path.Stroke = new SolidColorBrush(Colors.Transparent);
            }
        }


        private bool _settingsClicked;
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            if (_settingsClicked == false)
            {
                _settingsClicked = true;
                SettingsButton.Background = new SolidColorBrush(Color.FromArgb(1, 70, 70, 70));
                var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            _settingsClicked = false;
            SettingsButton.Background = new SolidColorBrush(Color.FromArgb(0,0,0,0));
            
            MainWindow.Instance.NewFloatingFrame(new RuleCreatorPage(_rule));
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _settingsClicked = false;
            SettingsButton.Background = new SolidColorBrush(Color.FromArgb(0,0,0,0));
            (sender as DispatcherTimer)?.Stop(); 
        }

        private void DeleteRuleBtn_Click(object sender, RoutedEventArgs e)
        {
            FileSorter.DeleteRule(_rule);
            MainWindow.Instance.NewFloatingFrame(new RulesList());
        }
    }
}
