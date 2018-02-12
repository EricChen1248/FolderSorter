using System;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace FolderSorter.Classes
{
    public class Poller
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _timerStarted = true;
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

        /// <summary>
        /// Toggled poller between pause and resume, true when resumed
        /// </summary>
        /// <returns>true when resumed</returns>
        public bool ToggleResume()
        {
            if (_timerStarted)
            {
                Pause();
                return false;
            }
            Resume();
            return true;
        }

        private void Pause()
        {
            _timer.Stop();
            _timerStarted = false;
        }

        private void Resume()
        {
            _timer.Start();
            _timerStarted = true;
        }
        
    }
}
