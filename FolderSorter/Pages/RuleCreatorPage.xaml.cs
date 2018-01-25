using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            FolderBox.Text = dialog.SelectedPath;
        }

        private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.CloseRuleCreator();
        }
    }
}
