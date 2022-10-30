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

namespace Logging_App
{
    /// <summary>
    /// editWindow.xaml 的交互逻辑
    /// </summary>
    public partial class editWindow
    {
        public editWindow()
        {
            InitializeComponent();
        }

        public bool isBtnOk=false;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            isBtnOk = true;
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
