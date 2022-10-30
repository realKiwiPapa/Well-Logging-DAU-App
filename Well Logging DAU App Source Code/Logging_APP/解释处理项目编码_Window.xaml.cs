using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// 解释处理项目编码_Window.xaml 的交互逻辑
    /// </summary>
    public partial class 解释处理项目编码_Window
    {
        public 解释处理项目编码_Window()
        {
            InitializeComponent();
        }

        private bool isOk = false;
        public class ListItemData:Model.ModelBase
        {
            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; NotifyPropertyChanged("Name"); }
            }
            private decimal _value;
            public decimal Value
            {
                get { return _value; }
                set { _value = value; NotifyPropertyChanged("Value"); }
            }
            private bool selected = false;
            public bool Selected
            {
                get { return selected; }
                set { selected = value; NotifyPropertyChanged("Selected"); }
            }
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            isOk = false;
        }

        public void LoadData(object data)
        {
            var dv = this.DataContext as DataView;
            if (dv == null) return;
            var list = new List<ListItemData>();
            var model = data as Utility.DataCollection<Model.PRO_LOG_PROCESSING_ITEM>;
            foreach (DataRow row in dv.Table.Rows)
            {
                var d = new ListItemData
                {
                    Name = row.FieldEx<string>("PROCESSING_ITEM_NAME"),
                    Value = row.FieldEx<decimal>("PROCESSING_ITEM_ID")
                };
                if (model != null && model.FirstOrDefault(m => m.PROCESSING_ITEM_ID == row.FieldEx<decimal>("PROCESSING_ITEM_ID")) != null)
                    d.Selected = true;
                list.Add(d);
            }

            listBox.SelectedMemberPath = "Selected";
            listBox.DisplayMemberPath = "Name";
            listBox.ValueMemberPath = "Value";
            listBox.ItemsSource = list;

        }

        public decimal[] GetData()
        {
            if (!isOk) return null;
            var list = listBox.ItemsSource as List<ListItemData>;
            return (from item in list
                    where item.Selected
                    select item.Value).ToArray();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            isOk = true;
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
