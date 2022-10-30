using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Data;

namespace Logging_App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //ThreadPool.QueueUserWorkItem(new WaitCallback(initializeServer));
        }
        static object a;
        private void initializeServer(object state)
        {
            using (UserServiceHelper userser = new UserServiceHelper())
            {
                try
                {
                    a = userser.Login("", "");
                }
                catch { }
            }
            using (var wfser = new WorkflowServiceHelper())
            {
                try
                {
                    a = wfser.GetActiveUserWorkflowTypes();
                }
                catch { }
            }

            using (var dataser = new DataServiceHelper())
            {
                try
                {
                    a = dataser.GetLogType();
                }
                catch { }
            }
        }


        private static LoginPage loginPage = new LoginPage();

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
#if TestClient
            this.Title += "(Beta)";
#endif
            this.Title = this.Title + "-" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            MainFrame.Navigate(loginPage);
            loginPage.LoginSuccessEvent += new LoginPage.LoginSuccessHandler(LoginSuccess);
        }

        private MainPage mainPage = null;

        private void LoginSuccess()
        {
            mainPage = new MainPage();
            MainFrame.Navigate(mainPage);
            mainPage.Loaded += new RoutedEventHandler(mainPage_Loaded);
        }

        private void mainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var mainPage = sender as MainPage;
            var page = mainPage.homeFrame.Content;
            if (page is MyHomePage)
            {
                ((MyHomePage)page).LogoutEvent += new MyHomePage.LogoutHandler(Logout);
                ((MyHomePage)page).OpenTaskEvent += new MyHomePage.OpenTaskHandler(OpenTask);
            }
        }

        private void OpenTask(object page)
        {
            if (mainPage != null) mainPage.OpenTask(page);
        }

        private void Logout()
        {
            MainFrame.Navigate(loginPage);
        }

        private void BaseWindow_Initialized(object sender, EventArgs e)
        {
            PluginDownload.Update();
#if !DEBUG
            UpdateDownload.Update();
#endif
        }
    }
}
