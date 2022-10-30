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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;

namespace Logging_App
{
    /// <summary>
    /// A1_Submit.xaml 的交互逻辑
    /// </summary>
    public partial class A1_Submit : Page
    {
        public A1_Submit()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Project01");
            dt.Columns.Add("Project02");
            dt.Columns.Add("Project03");
            dt.Columns.Add("Project04");
            dt.Columns.Add("Project05");
            dt.Columns.Add("Project06");
            for (int i = 1; i < 20; i++)
            {
                dt.Rows.Add("2012-2013", "测井技术分公司", "磨溪"+i.ToString(), "完成", "2015-11-" + i, "小张");
            }
            项目信息.ItemsSource = dt.DefaultView;
        }

        private void 项目信息_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
          
        }
    }
}
