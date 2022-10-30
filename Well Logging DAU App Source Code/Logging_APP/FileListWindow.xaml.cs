using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logging_App.Utility;
using System.Threading;

namespace Logging_App
{
    /// <summary>
    /// 解释处理项目编码_Window.xaml 的交互逻辑
    /// </summary>
    public partial class FileListWindow
    {
        public FileListWindow()
        {
            InitializeComponent();
        }

        //public TextBox textBox = null;
        public Control control = null;

        public class ListItemData : Model.ModelBase
        {
            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; NotifyPropertyChanged("Name"); }
            }

            public string FullName { get; set; }
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
        Utility.DataCollection<UploadController.UploadTask> tasks = null;
        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (control != null)
            {
                TextBox textBox = null;
                string str = null;
                if (control is TextBox)
                {
                    textBox = control as TextBox;
                    str = textBox.Text;
                }
                tasks = this.DataContext as Utility.DataCollection<UploadController.UploadTask>;
                var list = new DataCollection<ListItemData>();
                if (tasks != null && tasks.Count > 0)
                {
                    foreach (var task in tasks)
                    {
                        var item = new ListItemData { Name = task.Name, FullName = task.FullName };
                        list.Add(item);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(str))
                {
                    str = str.Trim();
                    foreach (string name in str.Split('\n'))
                    {
                        var item = new ListItemData { Name = name };
                        list.Add(item);
                    }
                }

                listBox.SelectedMemberPath = "Selected";
                listBox.DisplayMemberPath = "Name";
                listBox.ValueMemberPath = "Value";
                listBox.ItemsSource = list;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (control != null && control is TextBox)
            {
                TextBox textBox = control as TextBox;
                textBox.Clear();
                var list = listBox.ItemsSource as DataCollection<ListItemData>;
                if (tasks != null && tasks.Count > 0)
                    tasks.Clear();
                if (list != null && list.Count > 0)
                {
                    foreach (ListItemData item in list)
                    {
                        textBox.AppendText(item.Name + "\n");
                        if (!string.IsNullOrEmpty(item.FullName))
                            tasks.Add(new UploadController.UploadTask { FullName = item.FullName });
                    }
                }
            }
            else if (control != null && control is ComboBox)
            {
                var list = listBox.ItemsSource as DataCollection<ListItemData>;
                if (list != null)
                {
                    var lidList = list.RemovedData;
                    if (lidList.Count > 0)
                    {
                        List<string> paths = new List<string>();
                        foreach (var item in lidList)
                        {
                            tasks.RemoveAll(x => x.Name == item.Name);
                            paths.Add(item.FullName);
                        }
                        _team_deleget(paths);
                    }
                }
            }
            this.Close();
        }

        public delegate void TeamDeleget(List<string> list);
        public TeamDeleget _team_deleget;

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var list = listBox.ItemsSource as DataCollection<ListItemData>;
            if (list != null)
                list.RemoveAll(o => o.Selected);
        }
    }
}
