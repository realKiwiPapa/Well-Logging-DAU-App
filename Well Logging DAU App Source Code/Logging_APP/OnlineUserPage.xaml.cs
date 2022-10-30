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

using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// OnlineUserPage.xaml 的交互逻辑
    /// </summary>
    public partial class OnlineUserPage : Page
    {
        public OnlineUserPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var userser = new UserServiceHelper())
            {
                var ds = userser.GetOnlineUser();
                if (ds != null)
                    dataGrid1.ItemsSource = ds.Tables[0].GetDefaultView();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Button_Click(null, null);
        }
    }
}
