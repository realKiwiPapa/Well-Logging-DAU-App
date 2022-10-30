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
    public delegate void ChangeTextHandler_放射源(DataRow dataRow);
    /// <summary>
    /// 小队施工信息_放射源_Window.xaml 的交互逻辑
    /// </summary>
    public partial class 小队施工信息_放射源_Window
    {
        public event ChangeTextHandler_放射源 ChangeTextEvent;

        public 小队施工信息_放射源_Window()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var item = dataGrid1.SelectedItem as DataRowView;
            if (item == null) return;
            ChangeTextEvent(item.Row);
            this.Close();
        }

        DataView dataView;
        DataTable dataTable;
        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataTable = dataser.GetDataList_放射源编码().Tables[0];
                //dataTable.Columns.Add(new DataColumn() { ColumnName = "CHOISE", DataType = Type.GetType("System.Int32"), DefaultValue = 0 });
            }
            dataView = dataTable.GetDefaultView();
            dataGrid1.ItemsSource = dataView;
            BindComboBox(comboBox1, "RADIATION_NAME");
            BindComboBox(comboBox2, "USE_TEAM");
            comboBox1.SelectionChanged += comboBox_SelectionChanged;
            comboBox2.SelectionChanged += comboBox_SelectionChanged;
        }
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
        private void SelectionChanged()
        {
            string RADIATION_NAME = comboBox1.SelectedValue.ToString();
            string and1 = RADIATION_NAME == "全部" ? string.Empty : " and RADIATION_NAME='" + RADIATION_NAME + "'";
            string USE_TEAM = comboBox2.SelectedValue.ToString();
            string and2 = USE_TEAM == "全部" ? string.Empty : " and USE_TEAM='" + USE_TEAM + "'";
            dataView.RowFilter = "1=1" + and1 + and2;
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
