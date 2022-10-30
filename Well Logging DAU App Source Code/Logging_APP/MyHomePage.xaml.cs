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
using System.Globalization;
using System.Data;
using System.Threading;

using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// MyHomePage.xaml 的交互逻辑
    /// </summary>
    public partial class MyHomePage : Page
    {
        public MyHomePage()
        {
            InitializeComponent();
        }

        public delegate void LogoutHandler();
        public event LogoutHandler LogoutEvent;

        public delegate void OpenTaskHandler(object page);
        public event OpenTaskHandler OpenTaskEvent;

        public static Model.HS_USER ActiveUser = new Model.HS_USER();

        private void LoadMyTask(object state = null)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var ds = dataser.GetMyTask();
                if (ds != null)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        DM_LOG_TASK.ItemsSource = ds.Tables["DM_LOG_TASK"].GetDefaultView();
                        DM_LOG_OPS_PLAN.ItemsSource = ds.Tables["DM_LOG_OPS_PLAN"].GetDefaultView();
                        小队施工信息.ItemsSource = ds.Tables["小队施工信息"].GetDefaultView();
                        测井现场提交信息.ItemsSource = ds.Tables["测井现场提交信息"].GetDefaultView();
                        解释处理作业.ItemsSource = ds.Tables["解释处理作业"].GetDefaultView();
                        归档入库.ItemsSource = ds.Tables["归档入库"].GetDefaultView();
                        接收到的数据.ItemsSource = ds.Tables["接收到的数据"].GetDefaultView();
                    }));
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            welcome.DataContext = ActiveUser;
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadMyTask));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            changePasswordWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (pbNew.Password != pbRepeat.Password) throw new Exception("新密码与重复密码不一致！");
            using (var userser = new UserServiceHelper())
            {
                if (userser.UserChangePassword(pbOld.Password, pbNew.Password))
                {
                    changePasswordWindow.Close();
                    MessageBox.Show(App.Current.MainWindow,"修改密码成功！");
                }
                else MessageBox.Show(App.Current.MainWindow,"修改密码失败！");
            }
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            MyHomePage.ActiveUser.COL_NAME = null;
            Application.Current.MainWindow.Title = Application.Current.MainWindow.Title.Split('\t')[0];
            LogoutEvent();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Expander1.IsExpanded = true;
            LoadMyTask();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;
            var link = e.OriginalSource as Hyperlink;
            if (dg == null || string.IsNullOrEmpty(dg.Name) || link == null || string.IsNullOrEmpty(link.NavigateUri.OriginalString)) return;
            object page = null;
            switch (dg.Name)
            {
                case "DM_LOG_TASK":
                    page = new Log_Task_Page { REQUISITION_CD = link.NavigateUri.OriginalString };
                    break;
                case "DM_LOG_OPS_PLAN":
                    page = new LOG_OPS_PLAN_Page { JOB_PLAN_CD = link.NavigateUri.OriginalString };
                    break;
                case "小队施工信息":
                    page = new 小队施工信息_Page { JOB_PLAN_CD = link.NavigateUri.OriginalString };
                    break;
                case "测井现场提交信息":
                    page = new 测井现场提交信息_Page { JOB_PLAN_CD = link.NavigateUri.OriginalString };
                    break;
                case "接收到的数据":
                    page = new 测井现场提交信息_Page { JOB_PLAN_CD = link.NavigateUri.OriginalString };
                    break;
                case "解释处理作业":
                    page = new 解释处理作业_Page { Process_Name = link.NavigateUri.OriginalString };
                    break;
                case "归档入库":
                    page = new 归档入库_Page { Process_Name = link.NavigateUri.OriginalString };
                    break;
            }
            if (page != null) OpenTaskEvent(page);
            return;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            e.Handled = true;
        }
    }

    [ValueConversion(typeof(GridLength), typeof(object))]
    public class GridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dv = value as DataView;
            if (dv != null && dv.Table.Rows.Count > 0)
                return new GridLength(0, GridUnitType.Star);
            else
                return new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
