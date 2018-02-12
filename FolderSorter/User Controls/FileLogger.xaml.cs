using System;
using System.Collections.Generic;
using System.Diagnostics;
using FolderSorter.Classes;
using FolderSorter.Classes.DataClasses;

namespace FolderSorter.User_Controls
{
    /// <summary>
    /// Interaction logic for FileLogger.xaml
    /// </summary>
    public partial class FileLogger
    {
        private readonly Queue<FileLog> _pendingRemoveLogs = new Queue<FileLog>();

        public FileLogger()
        {
            InitializeComponent();
            var logs = Server.ReadLogs();
            foreach (var log in logs)
            {
                AddLog(log);
            }

            while (_pendingRemoveLogs.Count > 0)
            {
                RemoveLog(_pendingRemoveLogs.Dequeue());
            }
        }

        public void ClearLogs()
        {
            StackPanel.Children.RemoveRange(0, StackPanel.Children.Count);
            Server.ClearLog();
        }
        
        public void AddLog(MoveFileData data)
        {
            var entry = new FileLog(data);
            StackPanel.Children.Insert(0, entry);
        }

        public void RemoveLog(FileLog log)
        {
            StackPanel.Children.Remove(log);
        }

        public static void Remove(FileLog log)
        {
            try
            {
                MainWindow.Instance.MoveFileLog.RemoveLog(log);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e);
            }
        }

        public static void QueueRemove(FileLog log)
        {
            MainWindow.Instance.MoveFileLog._pendingRemoveLogs.Enqueue(log);
        }
    }
}
