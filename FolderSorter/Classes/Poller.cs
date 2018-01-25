using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using FolderSorter.Classes.DataClasses;

namespace FolderSorter.Classes
{
    public class Poller
    {
        private DispatcherTimer _timer = new DispatcherTimer();

        internal Poller()
        {
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var folder = new DirectoryInfo(MainWindow.DownloadPath).GetFiles().Where(x => (x.Attributes & FileAttributes.Hidden) == 0);
            foreach (var file in folder)
            {
                if (Helper.IsFileLocked(file))
                { 
                    continue;
                }

                FileSorter.SortFile(file);
            }
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public void Resume()
        {
            _timer.Start();
        }
        
    }
}
