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
using System.Data;
using System.Collections.ObjectModel;
using Logging_App.Utility;
using Logging_App.Model;

namespace Logging_App
{
    /// <summary>
    /// SelectUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectUserWindow1
    {
        public SelectUserWindow1()
        {
            InitializeComponent();
        }

        public UserService.UserRole UserRole;
        public string LoginName;
        public string LoginID;
        //允许选择多行
        public bool allowMultiple = false;

        private void userDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (allowMultiple)
            {
                userDataGrid.SelectionMode = DataGridSelectionMode.Extended;
            }
            this.Title = "选择" + Enum.GetName(typeof(UserService.UserRole), UserRole);
            using (var userser = new UserServiceHelper())
            {
                var dt = userser.GetUser(UserRole).Tables[0];
                //foreach (var row in dt.Select("COL_LOGINNAME='" + MyHomePage.ActiveUser.COL_LOGINNAME + "'"))
                //{
                //    dt.Rows.Remove(row);
                //}
                userDataGrid.ItemsSource = dt.GetDefaultView();
            }
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            if (!allowMultiple)
            {
                var row = userDataGrid.SelectedItem as DataRowView;
                if (row != null)
                {
                    LoginName = row.Row.FieldEx<string>("COL_LOGINNAME");
                }
            }
            else
            {
                int count = userDataGrid.SelectedItems.Count;
                if (count > 0)
                {
                    foreach (DataRowView dv in userDataGrid.SelectedItems)
                    {
                        //LoginName += dv.Row.Field<string>("COL_LOGINNAME") + "(" + dv.Row.Field<string>("COL_NAME") + "),";
                        LoginName += dv.Row.Field<string>("COL_NAME") + ",";
                        LoginID += dv.Row.Field<string>("COL_LOGINNAME") + ",";
                    }
                    LoginName = LoginName.Remove(LoginName.Length - 1, 1);
                    LoginID = LoginID.Remove(LoginID.Length - 1, 1);
                }
            }
            this.Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
