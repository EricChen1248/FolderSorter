using System;
using System.Windows.Threading;

namespace FolderSorter.Classes
{
    public class TimedDispatcher
    {
        private int _totalTicks;
        private Action _stopAction;
        public TimedDispatcher(int totalTicks, TimeSpan interval, EventHandler lambda)
        {
            var timer = new DispatcherTimer {Interval = interval};
            timer.Tick += lambda;
            timer.Tick += Timer_Tick;
            
            _totalTicks = totalTicks;
            timer.Start();
        }

        public TimedDispatcher(int totalTicks, TimeSpan interval, EventHandler lambda, Action onDeathCallback) : this(totalTicks, interval, lambda)
        {
            _stopAction = onDeathCallback;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            --_totalTicks;
            if (_totalTicks == 0)
            {
                (sender as DispatcherTimer)?.Stop();
                _stopAction?.Invoke();

            }
        }
    }
}
