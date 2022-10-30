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
    public partial class DataPushYTHPT : Page
    {
        public DataPushYTHPT()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                //初始化业主单位列表
                var PART_UNITSList = dataser.GetComboBoxList_甲方来电单位().Tables[0].GetDefaultView();
                PART_UNITS.DisplayMemberPath = "DEPARTMENT_REQUISITION_NAME";
                PART_UNITS.SelectedValuePath = "DEPARTMENT_REQUISITION_NAME";
                PART_UNITS.ItemsSource = PART_UNITSList;
                //DataView queryDate = dataser.GetComboxList_QueryDate().Tables[0].GetDefaultView();
                //QUERY_DATE.DisplayMemberPath = "QUREY_DATE";
                //QUERY_DATE.SelectedValuePath = "DATE_VALUE";
                //QUERY_DATE.ItemsSource = queryDate;
                //QUERY_DATE.Text = "最近一年";
            }
            LoadDataList(1);
        }

        private DataService.VIEW_REQUISITION_LIST selectedItem;

        public void LoadDataList(int page)
        {
            using (var dataser = new DataServiceHelper())
            {
                //REQUISITION_TYPE.DisplayMemberPath = "REQUISITION_TYPE_NAME";
                //REQUISITION_TYPE.SelectedValuePath = "REQUISITION_TYPE_ID";
                //REQUISITION_TYPE.ItemsSource = dataser.GetRequisitionTypes().Tables[0].GetDefaultView();
                int total = 0;
                dataGrid1.ItemsSource = dataser.GetDataPushListYTHPT(text_well_name.Text, PART_UNITS.Text, START_TIME.Value, cbox_state.Text, cbox_state1.Text, page, out total).Tables[0].GetDefaultView();
                dataPager1.PageIndex = page;
                dataPager1.TotalCount = total;
                selectedItem = null;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LoadDataList(1);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem == null)
            {
                MessageBox.Show("没有选择数据！");
                return;
            }
            using (var dataser = new DataServiceHelper())
            {
                var process_name = ((DataRowView)dataGrid1.SelectedItem).Row.Field<string>("PROCESS_NAME");
                if (dataser.VilidateDataPush(process_name))
                {
                    var result = MessageBox.Show("选定的解释处理作业已推送!\n\n是否继续?", "提示", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No) return;
                }
            }
            ThreadPool.QueueUserWorkItem(call, dataGrid1.SelectedItem);
            button2.IsEnabled = false;
            textBox1.Clear();
        }

        private void call(object selectedItem)
        {
            try
            {
                HttpWebRequest httpRequest = WebRequest.Create(WebServiceConfig.BaseUrl + "DataPushYTHPT.ashx") as HttpWebRequest;
                httpRequest.Method = "POST";
                httpRequest.Timeout = 1000000;
                httpRequest.ReadWriteTimeout = 3600000 * 2;

                string param = "process_id=" + ((DataRowView)selectedItem).Row.Field<string>("PROCESS_ID");
                param += "&active_user=" + MyHomePage.ActiveUser.COL_NAME;
                //param += "&well_name=" + Uri.EscapeDataString(selectedItem.WELL_JOB_NAME);
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
                    LoadDataList(1);
                    button2.IsEnabled = true;
                }));
            }
        }

        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            int page = (int)e.CurrentPageIndex;
            LoadDataList(page);
        }
    }
}
