using System;
using System.Collections.Generic;
using System.Windows;
using FolderSorter.Classes.DataClasses;

namespace FolderSorter.Classes
{
    public static class MoveNotifier
    {
        private static readonly LinkedList<MoveLogNotifier> _logs = new LinkedList<MoveLogNotifier>();

        public static void Remove(MoveLogNotifier log)
        {
            LinkedListNode<MoveLogNotifier>[] currentNode = {_logs.Find(log)};
            while (currentNode[0]?.Previous != null)
            {
                var _ = new TimedDispatcher(20, TimeSpan.FromMilliseconds(1), (sender, args) =>
                {
                    currentNode[0].Value.Top += 5;
                });
                currentNode[0] = currentNode[0].Previous;
            }

            _logs.Remove(log);
        }

        public static void Add(MoveFileData data)

        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            var log = new MoveLogNotifier(data);
            log.Left = desktopWorkingArea.Right - log.Width - 20;
            log.Top = desktopWorkingArea.Bottom - log.Height - 20;
            foreach (var moveLogNotifier in _logs)
            {
                var _ = new TimedDispatcher(20, TimeSpan.FromMilliseconds(1), (sender, args) =>
                {
                    moveLogNotifier.Top -= 5;
                });

            }

            _logs.AddLast(log);
            log.Show();

        }
    }
}
