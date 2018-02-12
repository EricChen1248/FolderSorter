using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using FolderSorter.Classes;

namespace FolderSorter.Pages
{
    /// <summary>
    /// Interaction logic for RuleCreatorPage.xaml
    /// </summary>
    public partial class RuleCreatorPage
    {
        private readonly Dictionary<string, RuleType> _ruleTypeDictionary = new Dictionary<string, RuleType>();
        private Rule _rule;
        private string Destination;
        public RuleCreatorPage(Rule rule) : this()
        {
            _rule = rule;
            NameBox.Text = rule.Name;
            Destination = rule.DestinationFolder;
            UpdateFolderBox();
            
            SortBox.Text = rule.Data.Aggregate((x, y) => x + "\n" + y);
            TypeCombo.SelectedIndex = (int) rule.Type;
            CreateButton.Content = "Update";
        }
        
        public RuleCreatorPage()
        {
            InitializeComponent();

            foreach (RuleType type in Enum.GetValues(typeof(RuleType)))
            {
                var str = GetDescription(type);
                _ruleTypeDictionary.Add(str, type);
                TypeCombo.Items.Add(str);
            }

            TypeCombo.SelectedIndex = 0;
            Destination = UserFolderAPI.GetPath(KnownFolder.Downloads) + "\\Others";
            UpdateFolderBox();

        }
        private static string GetDescription(RuleType type)
        {
            var fi = type.GetType().GetField(type.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : type.ToString();
        }

        private void BrowseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
            {
                ShowNewFolderButton = true,
                UseDescriptionForTitle = true,
                Description = @"Choose Location",
                SelectedPath = Path.Combine(UserFolderAPI.GetPath(KnownFolder.Downloads), "Others")
            };
            
            dialog.ShowDialog();
            Destination = dialog.SelectedPath;
            
            UpdateFolderBox();
        }

        private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.NewFloatingFrame(new RulesList());
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Destination = Destination.TrimEnd('\\');
            Destination = Destination.TrimEnd('/');
            if (Directory.Exists(Destination) == false)
            {
                var results = MessageBox.Show("Directory does not seem to be valid or does not exists, try create a directory?", "Invalid File Path" , MessageBoxButton.YesNo);
                if (results != MessageBoxResult.Yes) return;
                try
                {
                    Directory.CreateDirectory(Destination);
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show("Path is not a valid Windows File Path");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        $"Contact developer with the following information: \nFile Path: {Destination}\nException: {exception.Message}");
                }

                return;
            }
            if (_rule != null)
            {
                _rule.ChangeName(NameBox.Text);
                _rule.MoveLocation(Destination);
                _rule.Type = (RuleType) TypeCombo.SelectedIndex;
                _rule.UpdateData(SortBox.Text.Split('\n').ToList());
                FileSorter.UpdateRules();
            }
            else
            {
                FileSorter.AddRule(
                    new Rule(
                        NameBox.Text,
                        Destination,
                        SortBox.Text.Split('\n').ToList(),
                        (RuleType) TypeCombo.SelectedIndex));
            }
            
            MainWindow.Instance.NewFloatingFrame(new RulesList());
        }

        private void UpdateFolderBox()
        {
            var destination = Destination.Split('\\');
            var destinationString = "";
            var i = destination.Length;
            while (destinationString.Length < 20)
            {
                destinationString = destination[--i] + "\\" + destinationString;
            }

            FolderBox.Text = "...\\" + destinationString;
        }
    }
}
