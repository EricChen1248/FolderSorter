using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace FolderSorter.Classes
{
    internal static class IconCreator
    {
        private static MainWindow MainWindow => (System.Windows.Application.Current.MainWindow as MainWindow);
        private static readonly NotifyIcon _notifyIcon;
        private static WindowState _originalWindowState;
        static IconCreator()
        {
            var contextMenu = GenerateContextMenu();
            _notifyIcon = new NotifyIcon
            {
                BalloonTipText = @"The app has been minimised. Double click the tray icon to show.",
                BalloonTipTitle = @"Folder Sorter",
                Text = @"Folder Sorter",
                Icon = Properties.Resources.TrayIcon,
                ContextMenu = contextMenu
            };
            _notifyIcon.DoubleClick += (s, e) =>
            {
                MainWindow.Show();
                MainWindow.ShowInTaskbar = true;
                _notifyIcon.Visible = false;
                MainWindow.WindowState = _originalWindowState;
            };
        }
        

        private static ContextMenu GenerateContextMenu()
        {
            var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Exit", (s, e) => MainWindow.Close());
            return contextMenu;
        }

        public static void Dispose()
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }

        public static void Show(WindowState currentWindowState)
        {
            _originalWindowState = currentWindowState;
            _notifyIcon.Visible = true;
            _notifyIcon.ShowBalloonTip(2000);
        }
    }
}
