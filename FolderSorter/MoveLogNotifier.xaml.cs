using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using FolderSorter.Classes;
using FolderSorter.Classes.DataClasses;

namespace FolderSorter
{
    /// <summary>
    /// Interaction logic for MoveLogNotifier.xaml
    /// </summary>
    public partial class MoveLogNotifier : Window
    {
        private DispatcherTimer timer;
        private MoveFileData _data;
        public MoveLogNotifier()
        {
            InitializeComponent();
        }

        public MoveLogNotifier(MoveFileData data) : this()
        {
            _data = data;
            FileIconImage.Source = Helper.FileToImageConverter(Path.Combine(data.Destination, data.FileName));
            FileName.Content = data.FileName;
            FileType.Content = data.FileType;
            var destination = data.Destination.Split('\\');
            var destinationString = "";
            var i = destination.Length;
            while (destinationString.Length < 30)
            {
                destinationString = destination[--i] + "\\" + destinationString;
            }

            Destination.Content = destinationString;
            
            timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(5)};
            timer.Tick += (s, args) => StartClose();
            timer.Start();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }
        
        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", _data.Destination);
        }

        private void StartClose()
        {
            new TimedDispatcher(10, TimeSpan.FromMilliseconds(10), (s, args) => { Opacity -= 0.1; }, Close);
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
            timer.Stop();
            MoveNotifier.Remove(this);
        }
    }
}
