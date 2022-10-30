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
using Logging_App.Utility;

namespace Logging_App
{
    public delegate void ChangeTextHandler_井信息(string WELL_JOB_NAME, string JOB_ID);
    /// <summary>
    /// COM_WELL_BASIC.xaml 的交互逻辑
    /// </summary>
    public partial class COM_WELL_BASIC_Window
    {
        public event ChangeTextHandler_井信息 ChangeTextEvent;

        public COM_WELL_BASIC_Window()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var item = dataGrid1.SelectedItem as DataRowView;
            if (item == null || string.IsNullOrWhiteSpace(item.Row.FieldEx<string>("WELL_JOB_NAME"))) return;
            string JOB_ID = null;
            if (comboBox1.SelectedIndex == 1) JOB_ID = item.Row.FieldEx<string>("JOB_ID");
            ChangeTextEvent(item.Row.FieldEx<string>("WELL_JOB_NAME"), JOB_ID);
            this.Close();
        }

        DataView dataView;
        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataView = dataser.GetDataList_井名().Tables[0].GetDefaultView();
                dataGrid1.ItemsSource = dataView;
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string 井名 = txt井名.Text;
            string 作业井名 = txt作业井名.Text;
            string and1 = 井名 == "" ? string.Empty : " and WELL_NAME like '%" + 井名 + "%'";
            string and2 = 作业井名 == "" ? string.Empty : " and WELL_JOB_NAME like '%" + 作业井名 + "%'";
            dataView.RowFilter = "1=1" + and1 + and2;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var data9库 = dataser.GetDataList_井名().Tables[0];
                if (comboBox1.SelectedIndex == 1)//中心库
                {
                    var data12 = dataser.GetDataList_A12井名().Tables[0];
                    for (int i = 0; i < data12.Rows.Count; i++)
                    {
                        data12.Rows[i]["WELL_NAME"] = data12.Rows[i]["WELL_NAME"].ToString().Replace("井", "");
                        data12.Rows[i]["WELL_JOB_NAME"] = data12.Rows[i]["WELL_JOB_NAME"].ToString().Replace("井", "");
                        for (int j = 0; j < data9库.Rows.Count; j++)
                        {
                            if (data9库.Rows[j]["WELL_NAME"].ToString() == data12.Rows[i]["WELL_NAME"].ToString())
                            {
                                data12.Rows[i].Delete();
                                break;
                            }
                        }
                    }
                    dataView = data12.GetDefaultView();
                }
                else
                {
                    dataView = data9库.GetDefaultView();
                }
            }
            dataGrid1.ItemsSource = dataView;
            txt井名.Text = "";
            txt作业井名.Text = "";
        }
    }
}
