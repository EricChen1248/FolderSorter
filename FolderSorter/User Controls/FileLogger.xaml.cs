using System.Windows.Controls;
using FolderSorter.Classes.DataClasses;

namespace FolderSorter.User_Controls
{
    /// <summary>
    /// Interaction logic for FileLogger.xaml
    /// </summary>
    public partial class FileLogger
    {
        public FileLogger()
        {
            InitializeComponent();
        }

        public void AddLog(MoveFileData data)
        {
            var entry = new User_Controls.FileLog(data);
            StackPanel.Children.Add(entry);
        }
    }
}
