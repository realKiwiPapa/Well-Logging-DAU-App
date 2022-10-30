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
using System.Threading;

namespace Logging_App
{
    /// <summary>
    /// UploadErrorMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class UploadErrorMessageBox
    {
        public UploadErrorMessageBox()
        {
            InitializeComponent();
        }
        static int timeoutSeconds = 30;
        static System.Media.SoundPlayer sp = new System.Media.SoundPlayer(Properties.Resources.notify);
        MessageBoxResult result = MessageBoxResult.Cancel;

        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            MessageBoxResult result = MessageBoxResult.Cancel;
            owner.Dispatcher.Invoke(new Action(() =>
            {
                var box = new UploadErrorMessageBox();
                box.textBox1.Text = messageBoxText;
                box.Owner = owner;
                box.ShowDialog();
                result = box.result;
            }));
            return result;
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            sp.PlayLooping();
            ThreadPool.QueueUserWorkItem(timeoutEvent);
        }

        private void timeoutEvent(object obj)
        {
            Dispatcher.Invoke(new Action(() => { button1.IsEnabled = true; }));
            for (int i = 0; i < timeoutSeconds; i++)
            {
                Dispatcher.Invoke(new Action(() => { button1.Content = string.Format("重试({0})", timeoutSeconds - i); }));
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(new Action(() => { button1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button1)); }));

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.result = MessageBoxResult.Yes;
            this.Close();
        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            sp.Stop();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.result = MessageBoxResult.Cancel;
            this.Close();
        }
    }
}
