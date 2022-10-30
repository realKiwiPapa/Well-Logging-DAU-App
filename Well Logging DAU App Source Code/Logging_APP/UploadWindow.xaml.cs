using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Logging_App
{
    /// <summary>
    /// UploadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UploadWindow
    {
        public UploadWindow()
        {
            InitializeComponent();
        }

        public bool UploadCompleted = false;

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!UploadCompleted) e.Cancel = true;
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Title.Contains("上传"))
            {
                timeUsedCol.Visibility = Visibility.Visible;
                timeLeftCol.Visibility = Visibility.Visible;
                speedCol.Visibility = Visibility.Visible;
            }
        }
    }
}
