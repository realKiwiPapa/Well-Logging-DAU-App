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
    public delegate void ChangeTextHandler_设备列表(DataRow[] dataRows);
    /// <summary>
    /// 小队施工信息_设备列表_Window.xaml 的交互逻辑
    /// </summary>
    public partial class 小队施工信息_设备列表_Window
    {
        public event ChangeTextHandler_设备列表 ChangeTextEvent;

        public 小队施工信息_设备列表_Window()
        {
            InitializeComponent();
        }
        private static DataTable csvDataTable;

        private DataTable GetCsvData()
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                return dataser.GetDataList_井下仪器编码().Tables[0];
            }
            /**
            DataTable dt = new DataTable();
            string str = Properties.Resources.设备台帐;
            foreach (string s in str.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                string[] strs = s.Split(',');

                if (dt.Columns.Count > 0)
                {
                    strs[0] = "False";
                    dt.Rows.Add(strs);
                }
                else
                {
                    foreach (string s1 in strs)
                    {
                        dt.Columns.Add(s1);
                    }
                }
            }
            return dt;
             **/
        }

        private void BindComboBox(ComboBox cbox, string name)
        {
            DataTable tempDT = csvDataTable.GetDefaultView().ToTable(true, new string[] { name });
            DataView dv = tempDT.GetDefaultView();
            dv.RowFilter = name + "<>'' ";
            dv.Sort = name;
            DataTable dt = dv.ToTable();
            DataRow dr1 = dt.NewRow();
            dr1[0] = "全部";
            dt.Rows.InsertAt(dr1, 0);
            cbox.ItemsSource = dt.GetDefaultView();
            cbox.DisplayMemberPath = name;
            cbox.SelectedValuePath = name;
            cbox.SelectedValue = "全部";
        }

        private DataView dataView;

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            csvDataTable = GetCsvData();
            csvDataTable.Columns.Add(new DataColumn(){ColumnName="CHOISE",DataType=Type.GetType("System.Int32"),DefaultValue=0});
            dataView = csvDataTable.GetDefaultView();
            dataGrid1.ItemsSource = dataView;
            checkBox1.IsChecked = false;
            if (this.DataContext != null)
            {
                DataRow[] drs = csvDataTable.Select("INSTRUMENT_ID in (" + this.DataContext + ")");
                if (drs.Length > 0) checkBox1.IsChecked = true;
                foreach (DataRow dr in drs)
                {
                    dr["CHOISE"] = 1;
                }
            }
            //dataView.Sort = "CHOISE DESC,INSTRUMENT_NAME ASC";
            BindComboBox(comboBox1, "INSTRUMENT_NAME");
            BindComboBox(comboBox2, "INSTRUMENT_TYPE");
            BindComboBox(comboBox3, "USE_TEAM");
            comboBox1.SelectionChanged += comboBox_SelectionChanged;
            comboBox2.SelectionChanged += comboBox_SelectionChanged;
            comboBox3.SelectionChanged += comboBox_SelectionChanged;
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ChangeTextEvent(csvDataTable.Select("CHOISE=1"));
            this.Close();
        }

        private void SelectionChanged()
        {
            string 仪器名称 = comboBox1.SelectedValue.ToString();
            string 仪器类别 = comboBox2.SelectedValue.ToString();
            string 所在班组 = comboBox3.SelectedValue.ToString();
            string and1 = 仪器名称 == "全部" ? string.Empty : " and INSTRUMENT_NAME='" + 仪器名称 + "'";
            string and2 = 仪器类别 == "全部" ? string.Empty : " and INSTRUMENT_TYPE='" + 仪器类别 + "'";
            string and3 = 所在班组 == "全部" ? string.Empty : " and USE_TEAM='" + 所在班组 + "'";
            dataView.RowFilter = "1=1" + and1 + and2 + and3;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged();
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            stackPanel.IsEnabled = false;
            dataView.RowFilter = "CHOISE=1";
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            stackPanel.IsEnabled = true;
            SelectionChanged();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}