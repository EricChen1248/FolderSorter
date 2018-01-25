using System;
using System.Collections.Generic;
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
using FolderSorter.Classes.DataClasses;

namespace FolderSorter
{
    /// <summary>
    /// Interaction logic for FileLogger.xaml
    /// </summary>
    public partial class FileLogger : UserControl
    {
        public FileLogger()
        {
            InitializeComponent();
        }

        public void AddLog(MoveFileData data)
        {
            var entry = new FileLog(data);
            StackPanel.Children.Add(entry);
        }
    }
}
