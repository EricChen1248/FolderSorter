using System.Windows;
using FolderSorter.Classes;
using FolderSorter.User_Controls;

namespace FolderSorter.Pages
{
    /// <summary>
    /// Interaction logic for RulesList.xaml
    /// </summary>
    public partial class RulesList
    {
        public RulesList()
        {
            InitializeComponent();
            foreach (var rule in FileSorter.rules)
            {
                Logger.Children.Add(new RuleLog(rule));
            }

            FallBackLogger.Children.Add(new RuleLog(FileSorter.FallBackRule));
        }

        private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.CloseFloatingFrame();
        }

        private void NewRuleBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.NewFloatingFrame(new RuleCreatorPage());
        }
    }
}
