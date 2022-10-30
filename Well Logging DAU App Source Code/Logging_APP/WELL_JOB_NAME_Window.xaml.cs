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
    /// <summary>
    /// COM_WELL_BASIC.xaml 的交互逻辑
    /// </summary>
    public partial class WELL_JOB_NAME_Window
    {

        public WELL_JOB_NAME_Window()
        {
            InitializeComponent();
        }

        private string drill_Job_ID;
        public string Drill_Job_ID
        {
            get { return drill_Job_ID; }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var item = dataGrid1.SelectedItem as DataRowView;
            if (item == null || string.IsNullOrWhiteSpace(item.Row.FieldEx<string>("WELL_JOB_NAME"))) return;
            drill_Job_ID= item.Row.FieldEx<string>("JOB_ID");
            this.Close();
        }

        DataView dataView;
        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataView = dataser.GetDataList_井名().Tables[0].GetDefaultView();
                dataGrid1.ItemsSource = dataView;
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
                dataView = dataser.GetDataList_井名().Tables[0].GetDefaultView();
            }
            dataGrid1.ItemsSource = dataView;
            txt井名.Text = "";
            txt作业井名.Text = "";
        }
    }
}
