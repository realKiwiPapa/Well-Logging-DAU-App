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
using System.Threading;

namespace Logging_App
{
    /// <summary>
    /// Auto_Update_Window.xaml 的交互逻辑
    /// </summary>
    public partial class AutoUpdateWindow
    {
        public AutoUpdateWindow()
        {
            InitializeComponent();
        }

        public void UpdateCompleted()
        {
            ThreadPool.QueueUserWorkItem(UpdateCompleted);
        }

        private void UpdateCompleted(object obj)
        {
            int seconds = 5;
            string title = string.Empty;
            Dispatcher.Invoke(new Action(() =>
            {
                button1.IsEnabled = true;
                title = this.Title;
            }));
            string str = "重新启动";
            if (title != "程序更新中")
                str = "关闭窗口";
            for (int i = 0; i < seconds; i++)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    button1.Content = string.Format("{0}({1})", str, seconds - i);
                }));
                Thread.Sleep(1000);
            }

            Dispatcher.Invoke(new Action(() => { button1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button1)); }));

        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!button1.IsEnabled || this.Title == "程序更新中")
                e.Cancel = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (this.Title != "程序更新中")
                this.Close();
            else
            {
                System.Windows.Forms.Application.Restart();
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}
