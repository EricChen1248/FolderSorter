using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using FolderSorter.Classes;
using FolderSorter.Classes.DataClasses;

namespace FolderSorter.User_Controls
{
    /// <summary>
    /// Interaction logic for FileLog.xaml
    /// </summary>
    public partial class FileLog
    {
        private MoveFileData data;
        public FileLog(MoveFileData data)
        {
            InitializeComponent();
            this.data = data;

            var fullPath = data.Destination + "\\" + data.FileName;
            try
            {
                var destination = data.Destination.Split('\\');
                var destinationString = "";
                var i = destination.Length;
                while (destinationString.Length < 20)
                {
                    destinationString = destination[--i] + "\\" + destinationString;
                }

                FileNameLabel.Content = data.FileName;
                FileLocationLabel.Content = @"...\" + destinationString;
                FileTypeLabel.Content = data.FileType;
                FileImage.Source = Helper.FileToImageConverter(data.Destination + "\\" + data.FileName);
                FileTimeLabel.Content = DateTime.Parse(data.Time).ToLocalTime();
                return;
            }
            catch (FileNotFoundException)
            {
                Server.DeleteLog(data);
                FileLogger.Remove(this);
            }
            catch (ArgumentNullException)
            {
                FileTimeLabel.Content = "PARSE ERROR";
            }
        }

        private void RemoveLogBtn_Click(object sender, RoutedEventArgs e)
        {
            Server.DeleteLog(data);
            FileLogger.Remove(this);
        }

        private void FileLog_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(data.Destination + "\\" + data.FileName);
            }
            catch (Win32Exception)
            {
                MainWindow.Instance.MoveFileLog.RemoveLog(this);
            }
        }
        
    }
}
