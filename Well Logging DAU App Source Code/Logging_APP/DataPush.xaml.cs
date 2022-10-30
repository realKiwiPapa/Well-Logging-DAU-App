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
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading;
using Logging_App.Model;
using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// 数据同步.xaml 的交互逻辑
    /// </summary>
    public partial class DataPush : Page
    {
        public DataPush()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataList();
        }

        private DataService.VIEW_REQUISITION_LIST selectedItem;

        public void LoadDataList()
        {
            using (var dataser = new DataServiceHelper())
            {
                REQUISITION_TYPE.DisplayMemberPath = "REQUISITION_TYPE_NAME";
                REQUISITION_TYPE.SelectedValuePath = "REQUISITION_TYPE_ID";
                REQUISITION_TYPE.ItemsSource = dataser.GetRequisitionTypes().Tables[0].GetDefaultView();
                dataGrid1.ItemsSource = dataser.GetDataPushList(text_well_name.Text, text_REQUISITION_CD.Text).OrderBy(x => x.REQUISITION_CD);
                selectedItem = null;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LoadDataList();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem == null)
            {
                MessageBox.Show("没有选择数据！");
                return;
            }
            selectedItem = dataGrid1.SelectedItem as DataService.VIEW_REQUISITION_LIST;
            ThreadPool.QueueUserWorkItem(call);
            button2.IsEnabled = false;
            textBox1.Clear();
        }

        private void call(object state)
        {
            try
            {
                HttpWebRequest httpRequest = WebRequest.Create(WebServiceConfig.BaseUrl + "DataPush.ashx") as HttpWebRequest;
                httpRequest.Method = "POST";
                httpRequest.Timeout = 3600000 * 10;

                string param = "cd=" + Uri.EscapeDataString(selectedItem.REQUISITION_CD);
                param += "&well_name=" + Uri.EscapeDataString(selectedItem.WELL_JOB_NAME);
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                byte[] bs = Encoding.UTF8.GetBytes(param);
                httpRequest.ContentLength = bs.Length;
                httpRequest.CookieContainer = WebServiceConfig.CookieContainer;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bs, 0, bs.Length);
                requestStream.Close();
                Stream responseStream = httpRequest.GetResponse().GetResponseStream();
                int bytesRead;
                byte[] buff = new byte[10240];
                string msg;
                while (true)
                {
                    bytesRead = responseStream.Read(buff, 0, 1024);
                    if (bytesRead > 1)
                    {
                        msg = Encoding.UTF8.GetString(buff, 0, bytesRead) + "\n";
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            textBox1.AppendText(msg);
                            textBox1.ScrollToEnd();
                        }));
                    }
                    if (bytesRead == 0) break;
                }
                responseStream.Close();
            }
            finally
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    LoadDataList();
                    button2.IsEnabled = true;
                }));
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var window= new Window_A1();
            window.Owner = App.Current.MainWindow;
            window.Title = "A1审核";
            window.Content = new A1_Check();
            //window.FrameA1.Navigate();
            window.ShowDialog();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            var window = new Window_A1();
            window.Owner = App.Current.MainWindow;
            window.Title = "A1提交";
            window.Content = new A1_Submit();
            window.ShowDialog();
        }
    }
}
