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

namespace Logging_App
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public delegate void LoginSuccessHandler();
        public event LoginSuccessHandler LoginSuccessEvent;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login(loginname.Text, password.Password);
        }

        private void Login(string loginname, string password)
        {
            if (string.IsNullOrWhiteSpace(loginname)) return;
            using (UserServiceHelper userser = new UserServiceHelper())
            {
                var model = Utility.ModelHandler<Model.HS_USER>.FillModel(userser.Login(loginname, password));
                if (model != null && model.Count == 1)
                {
                    MyHomePage.ActiveUser.COL_LOGINNAME = model[0].COL_LOGINNAME;
                    MyHomePage.ActiveUser.COL_NAME = model[0].COL_NAME;
                    Application.Current.MainWindow.Title += string.Format("\t{0}({1})", model[0].COL_LOGINNAME, model[0].COL_NAME);
                    LoginSuccessEvent();
                }
                else
                { 
                    throw new Exception("用户账户或密码错误！");
                }
            }
        }

        private void loginWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            if (string.IsNullOrEmpty(MyHomePage.ActiveUser.COL_LOGINNAME)) Login("admin", "admin");
#endif
        }
    }
}
