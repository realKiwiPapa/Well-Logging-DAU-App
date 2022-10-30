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
    /// A1_Check.xaml 的交互逻辑
    /// </summary>
    public partial class A1_Check : Page
    {
        public A1_Check()
        {
            InitializeComponent();
        }

        private void 项目信息_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //mainMenu.Visibility = Visibility.Visible;
          //  tabControl1.Visibility = Visibility.Hidden;
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
            for (int i = 1; i < 100; i++)
            {
                dt.Rows.Add("2012-2013", "测井技术分公司", "磨溪" + i.ToString(), "完成", "2015-11-" + i, "小张");
            }
            项目信息.ItemsSource = dt.DefaultView;
        }

        private void mainMenu_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem TV = (TreeViewItem)mainMenu.SelectedItem;
            if (TV.Header.ToString() == "测井任务")
            {
                tabItem1.Visibility = Visibility.Visible;
                tabItem1.Focus();
            }
            else if (TV.Header.ToString() == "测井项目")
            {
                tabItem2.Visibility = Visibility.Visible;
                tabItem2.Focus();
            }
            else if (TV.Header.ToString() == "测井遇阻遇卡")
            {
                tabItem3.Visibility = Visibility.Visible;
                tabItem3.Focus();
            }
            else if (TV.Header.ToString() == "井斜测量数据")
            {
                tabItem4.Visibility = Visibility.Visible;
                tabItem4.Focus();
            }
            else if (TV.Header.ToString() == "页岩气解释成果")
            {
                tabItem5.Visibility = Visibility.Visible;
                tabItem5.Focus();
            }
            else if (TV.Header.ToString() == "井筒力学")
            {
                tabItem6.Visibility = Visibility.Visible;
                tabItem6.Focus();
            }
            else if (TV.Header.ToString() == "测井曲线")
            {
                tabItem7.Visibility = Visibility.Visible;
                tabItem7.Focus();
            }
            else if (TV.Header.ToString() == "成果图件")
            {
                tabItem8.Visibility = Visibility.Visible;
                tabItem8.Focus();
            }
            else if (TV.Header.ToString() == "固井质量总评价")
            {
                tabItem9.Visibility = Visibility.Visible;
                tabItem9.Focus();
            }
            else if (TV.Header.ToString() == "固井质量分段评价")
            {
                tabItem10.Visibility = Visibility.Visible;
                tabItem10.Focus();
            }
            else if (TV.Header.ToString() == "测井文档")
            {
                tabItem11.Visibility = Visibility.Visible;
                tabItem11.Focus();
            }
            else
            {
            }
        }
    }
}