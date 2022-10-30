using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Logging_App.Model;

namespace Logging_App
{
    /// <summary>
    /// Window_测井曲线编码.xaml 的交互逻辑
    /// </summary>
    public partial class Window_测井曲线编码
    {
        public Window_测井曲线编码()
        {
            InitializeComponent();
        }

        private bool isOk = false;
        public class TreeData : TreeModel
        {
            private decimal item_id;
            public decimal Item_id
            {
                get { return item_id; }
                set { item_id = value; NotifyPropertyChanged("Item_id"); }
            }
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            isOk = false;
        }

        public void LoadData(object data,object data1)
        {
            var dv = this.DataContext as DataView;
            var dv1 = data1 as DataView;
            if (dv == null||dv.Count<1||dv1==null||dv1.Count<1) return;
            var model = new ObservableCollection<TreeData>();
            var model2 = data as Utility.DataCollection<Model.PRO_LOG_PROCESSING_CURVERATING>;
            foreach (DataRow row in dv1.Table.Rows)
            {
                var t = new TreeData
                {
                    Name=row.FieldEx<string>("PROCESSING_ITEM_NAME"),
                    IsChecked=false,
                    TreeData=Visibility.Collapsed
                };
                foreach (DataRow dr in dv.Table.Select("PROCESSING_ITEM_ID=" + row["PROCESSING_ITEM_ID"]))
                {
                    bool ischecked=false;
                    if (model2 != null && model2.FirstOrDefault(o => o.CURVE_ID == dr.FieldEx<decimal>("CURVE_ID")) != null)
                    {
                        ischecked = true;
                        t.IsExpanded = true;
                    }
                    var t1 = new TreeData
                    {
                        Name = dr.FieldEx<string>("CURVE_NAME"),
                        IsChecked = ischecked,
                        Icon="/Images/Icons/folder.png",
                        Item_id=dr.FieldEx<decimal>("PROCESSING_ITEM_ID"),
                        ID=dr.FieldEx<decimal>("CURVE_ID"),
                        TreeData = Visibility.Visible
                    };
                    t.Children.Add(t1);
                }
                if (t.Children.Count > 0)
                    model.Add(t);
            }
            myTree.ItemsSource = model;
        }

        public List<TreeData> GetData()
        {
            if (!isOk) return null;
            var list =  new List<TreeData>();
            foreach (TreeData td in myTree.ItemsSource)
            {
                foreach (TreeData td1 in td.Children)
                {
                    if (td1.IsChecked)
                        list.Add(td1);
                }
            }
            return list;
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
