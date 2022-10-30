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
using System.Management;//在引用中添加dll
using System.IO;
using System.Data;
using Logging_App.Utility;
using System.Collections.ObjectModel;

namespace Logging_App
{
    /// <summary>
    /// Dvd_Info_Page.xaml 的交互逻辑
    /// </summary>
    public partial class Log_Dvd_Info_Page: Page
    {
        public Log_Dvd_Info_Page()
        {
            InitializeComponent();
        }

        private void LoadDataList(int page)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                int total=0;
                dataGrid1.ItemsSource = dataser.GetDvdInfoList(ModelHelper.SerializeObject(search_panel.DataContext), page, out total).Tables[0].GetDefaultView();
                dataPager1.TotalCount = total;
                DVDINFO.DataContext = null;
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = dataGrid1.SelectedItem as DataRowView;
            if (item == null) return;
            string process_id = item.Row.FieldEx<string>("PROCESS_ID");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dt = dataser.GetDvdInfo(process_id).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DVDINFO.DataContext = ModelHandler<Model.DM_LOG_DVD_INFO>.FillModel(dt.Rows[0]);
                }
                else
                {
                    var model = new Model.DM_LOG_DVD_INFO();
                    model.PROCESS_NAME=item.Row.FieldEx<string>("PROCESS_NAME");
                    model.COPY_DVD_MAN = MyHomePage.ActiveUser.COL_LOGINNAME;
                    model.COPY_DVD_DATE = DateTime.Now;
                    DVDINFO.DataContext = model;
                }
            }
            //Button_ChangeJobInfo.IsChecked = true;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            LoadDataList(1);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitCD();
            search_panel.DataContext = new Model.DM_LOG_DVD_INFO();
            LoadDataList(1);
        }
        //
        //获取机器中盘符
        //
        public void InitCD()
        {
             ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT  *  From  Win32_LogicalDisk ");
             ManagementObjectCollection queryCollection = query.Get();
             foreach (ManagementObject mo in queryCollection)
                disk_no.Items.Add(mo["Name"].ToString());
        }
        private void Save_DvdInfo(object sender, RoutedEventArgs e)
        {
            var item = dataGrid1.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("请选择一个解释处理作业!!");
                return;
            }
            string process_id = item.Row.FieldEx<string>("PROCESS_ID");
            var model = DVDINFO.DataContext as Model.DM_LOG_DVD_INFO;
            if (model == null || model.HasError()) throw new Exception("参数有误，请检查！");
            //if (!model.PROCESS_NAME.Equals(model.DVD_DIR_NAME))
            //{
            //    MessageBox.Show("解释处理作业名与光盘目录名不一致!!");
            //    return;
            //}
            //string m_CdL = "川测发（CG）" + DateTime.Today.Year.ToString() + "-" + DVD_N.Text.ToString() + "-04/01";//光盘编号
            if (string.IsNullOrEmpty(model.PROCESS_ID))
            {
                model.DVD_NUMBER = "川测发（CG）" + DateTime.Today.Year.ToString() + "-" + model.DVD_NUMBER + "-04/01";
            }
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.SaveDvdInfo(ModelHelper.SerializeObject(model),process_id)>0)
                {
                    LoadDataList(1);
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }
        }

        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            int page = (int)e.CurrentPageIndex;
            LoadDataList(page);
        }

        private void disk_no_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Product> lists = new List<Product>();
            int m_ItemNum = 0;
            cd_pathname.ItemsSource = null;
            var mm = disk_no.Items[disk_no.SelectedIndex].ToString();
            try
            {
                foreach (string MyDir in Directory.GetDirectories(mm))  //井目录
                {
                    foreach (string MyDir1 in Directory.GetDirectories(MyDir))  //测井项目
                    {
                        var m_ItemName = new System.IO.FileInfo(MyDir1);
                        Product pt = new Product();
                        pt.Id = m_ItemNum++;
                        pt.Value = m_ItemName.Name;
                        lists.Add(pt);
                        if (m_ItemName.Name.Equals(Item.Text.ToString().Trim()))
                            cd_pathname.SelectedIndex = m_ItemNum;
                    }
                }
            }
            catch { }

            //cd_pathname.SelectedValuePath = "Id";   //程序内部维护的值
            //cd_pathname.DisplayMemberPath = "Value";  //显示的内容
            cd_pathname.ItemsSource = lists;  //数据源
            //cd_pathname.SelectedIndex = 0;  //默认选中值
        }
    }
    class Product   //声明类
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
