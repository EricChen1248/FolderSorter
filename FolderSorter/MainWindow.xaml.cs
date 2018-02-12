using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FolderSorter.Classes;
using FolderSorter.Classes.DataClasses;
using System.Windows.Controls.Primitives;
using FolderSorter.Pages;

namespace FolderSorter
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow Instance;
        public static string DownloadPath => UserFolderAPI.GetPath(KnownFolder.Downloads);
        private static Poller _poller;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            
            InitTitleBar();
            InitFrame();
            FileSorter.Init();
            
            _poller = new Poller();
        }

        public void AddLog(MoveFileData data)
        {
            MoveFileLog.AddLog(data);
        }

        private void Window_OnClose(object sender, EventArgs eventArgs)
        {
            IconCreator.Dispose();
        }
        
        #region Title Bar

        private void InitTitleBar()
        {
            var restoreIfMove = false;

            var brush = SystemParameters.WindowGlassBrush.Clone();
            brush.Opacity = 0.3;
            OuterBorder.BorderBrush = brush;
            
            TitleBar.MouseDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    MaximizeBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    return;
                }

                if (WindowState == WindowState.Maximized)
                {
                    restoreIfMove = true;
                }

                DragMove();
            };

            TitleBar.MouseUp += (s, e) => restoreIfMove = false;

            TitleBar.MouseMove += (s, e) =>
            {
                if (!restoreIfMove) return;

                restoreIfMove = false;
                var mouseX = e.GetPosition(this).X;
                var width = RestoreBounds.Width;
                var x = mouseX - width / 2;

                if (x < 0)
                {
                    x = 0;
                }
                else if (x + width > Width)
                {
                    x = Width - width;
                }

                WindowState = WindowState.Normal;
                Left = x;
                Top = 0;
                DragMove();
            };
        }

        private void MaximizeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseBtn_OnClick(object sender, RoutedEventArgs eventArgs)
        {
            IconCreator.Show(WindowState);
            Hide();
        }

        private void MinimizeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        #endregion

        #region Border
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Rectangle border)) return;
            border.MouseMove += Border_MouseMove;
            border.MouseLeftButtonUp += Border_MouseLeftButtonUp;
            border.CaptureMouse();
        }
        
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Rectangle border)) return;
            border.MouseMove -= Border_MouseMove;
            border.MouseLeftButtonUp -= Border_MouseLeftButtonUp;
            border.ReleaseMouseCapture();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is Rectangle border))
                return;

            var width = e.GetPosition(this).X;
            var height = e.GetPosition(this).Y;
            border.CaptureMouse();

            if (border.Name.Contains("Top"))
            {
                height -= 5;
                Top += height;
                height = Height - height;
                if (height > 0)
                {
                    Height = height;
                }
            }

            else if (border.Name.Contains("Bottom"))
            {
                height += 5;
                if (height > 0)
                {
                    Height = height;
                }
            }

            if (border.Name.Contains("Left"))
            {
                width -= 5;
                Left += width;
                width = Width - width;
                if (width > 0)
                {
                    Width = width;
                }
            }

            else if (border.Name.Contains("Right"))
            {
                width += 5;
                if (width > 0)
                {
                    Width = width;
                }
            }
        }

        private void Border_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!(sender is Rectangle border))
                return;

            switch (border.Name)
            {
                case "TopBorder":
                    Cursor = Cursors.SizeNS;
                    break;
                case "BottomBorder":
                    Cursor = Cursors.SizeNS;
                    break;
                case "LeftBorder":
                    Cursor = Cursors.SizeWE;
                    break;
                case "RightBorder":
                    Cursor = Cursors.SizeWE;
                    break;
                case "TopLeftBorder":
                    Cursor = Cursors.SizeNWSE;
                    break;
                case "TopRightBorder":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "BottomLeftBorder":
                    Cursor = Cursors.SizeNESW;
                    break;
                case "BottomRightBorder":
                    Cursor = Cursors.SizeNWSE;
                    break;
            }
        }

        private void Border_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        #endregion

        #region Frame
        private void InitFrame()
        {
            MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            RuleCreatorFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

        }
        
        private void Frame_OnNavigated(object sender, NavigationEventArgs e)
        {
            if (!(sender is Frame frame))
                return;
            try
            {
                frame.RemoveBackEntry();
            }
            catch (InvalidOperationException)
            {
                // There isn't a back entry to remove the first time frame is navigated
            }
        }
        #endregion
        
        public void OpenRulesList()
        {
            Show();
            NewFloatingFrame(new RulesList());
        }

        public void NewFloatingFrame(Page page)
        {
            RuleCreatorGrid.Visibility = Visibility.Visible;
            RuleCreatorFrame.Navigate(page);
        }

        public void CloseFloatingFrame()
        {
            RuleCreatorGrid.Visibility = Visibility.Hidden;
            RuleCreatorFrame.Content = null;
        }
        private void RulesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenRulesList();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButton.Content = _poller.ToggleResume() ? "Pause Sorting" : "Resume Sorting";
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            MoveFileLog.ClearLogs();
        }
    }
}
