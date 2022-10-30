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
    public delegate void ChangeTextHandler_车辆(DataRow dataRows);
    /// <summary>
    /// 小队施工信息_车辆_Window.xaml 的交互逻辑
    /// </summary>
    public partial class 小队施工信息_车辆_Window
    {
        public event ChangeTextHandler_车辆 ChangeTextEvent;

        public 小队施工信息_车辆_Window()
        {
            InitializeComponent();
        }

        public string Vehicle_Plate=string.Empty;
        private void BindComboBox(ComboBox cbox, string name)
        {
            DataTable tempDT = dataTable.GetDefaultView().ToTable(true, new string[] { name });
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
        private DataTable dataTable;

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataTable= dataser.GetDataList_车辆信息().Tables[0];
            }
            dataView = dataTable.GetDefaultView();
            dataGrid1.ItemsSource = dataView;
            BindComboBox(comboBox1, "VEHICLE_TYPE");
            BindComboBox(comboBox3, "TEAM_ORG");
            comboBox1.SelectionChanged += comboBox_SelectionChanged;
            comboBox3.SelectionChanged += comboBox_SelectionChanged;
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var item = dataGrid1.SelectedItem as DataRowView;
            if (item == null) return;
            Vehicle_Plate = item.Row.FieldEx<string>("VEHICLE_PLATE");
            //ChangeTextEvent(item.Row);
            this.Close();
        }

        private void SelectionChanged()
        {
            string vehicle_type = comboBox1.SelectedValue.ToString();
            string team_org = comboBox3.SelectedValue.ToString();
            string and1 = vehicle_type == "全部" ? string.Empty : " and VEHICLE_TYPE='" + vehicle_type + "'";
            string and3 = team_org == "全部" ? string.Empty : " and TEAM_ORG='" + team_org + "'";
            dataView.RowFilter = "1=1" + and1  + and3;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}