using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FolderSorter.Classes;
using FolderSorter.Classes.DataClasses;

namespace FolderSorter
{
    /// <summary>
    /// Interaction logic for FileLog.xaml
    /// </summary>
    public partial class FileLog
    {
        public FileLog(MoveFileData data)
        {
            InitializeComponent();

            var fullPath = data.Destination + "\\" + data.FileName;
            try
            {
                var destination =
                    MainWindow.DownloadPath.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries).Last() + "\\" +
                    data.Destination.TrimStart(MainWindow.DownloadPath.ToCharArray());
                FileNameLabel.Content = data.FileName;
                FileLocationLabel.Content = @"...\" + destination;
                FileTypeLabel.Content = data.FileType;
                FileTimeLabel.Content = data.Time.ToLocalTime();
                FileImage.Source = Helper.FileToImageConverter(data.Destination + "\\" + data.FileName);
                return;
            }
            catch (FileNotFoundException)
            {
                // TODO : Log Debug Error;
            }
        }
    }
}
