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

namespace Logging_App.Controls
{
    /// <summary>
    /// MyMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyMessageBox
    {
        public MyMessageBox()
        {
            InitializeComponent();
        }

        private MessageBoxResult result = MessageBoxResult.None;


        public MessageBoxResult Result
        {
            get { return result; }
        }

        private void Caption_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Height = 290 + TextContent.ActualHeight;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.Yes;
            this.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.No;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
