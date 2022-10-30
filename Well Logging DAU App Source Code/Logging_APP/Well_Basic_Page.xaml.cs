using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// Well_Basic_Page.xaml 的交互逻辑
    /// </summary>
    public partial class Well_Basic_Page : Page
    {
        public Well_Basic_Page()
        {
            InitializeComponent();
        }

        public void LoadWellList()
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataGrid1.ItemsSource = null;
                dataGrid2.ItemsSource = null;
                Button_NewWell.IsChecked = true;
                Button_NewWellbore.IsChecked = true;
                gbWellbore.IsEnabled = false;
                grid1.DataContext = new Model.COM_WELL_BASIC();
                DRILL_ENG_DES_FILENAME.Text = string.Empty;
                DRILL_GEO_DES_FILENAME.Text = string.Empty;
                dataGrid1.ItemsSource = dataser.SearchWellBasic(Utility.ModelHelper.SerializeObject(SearchWellBasic.DataContext)).Tables[0].GetDefaultView();
                //Counts.Content = "      井：" + dataGrid1.Items.Count.ToString();
            }
        }

        public void LoadWellboreList(string well_id)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataGrid2.ItemsSource = null;
                gbWellbore.IsEnabled = true;
                Button_NewWellbore.IsChecked = true;
                grid2.DataContext = new Model.COM_WELLBORE_BASIC() { WELL_ID = well_id };
                grid3.DataContext = new Model.COM_WELLSTRUCTURE_DATA();
                dataGrid2.ItemsSource = dataser.GetWellboreList(well_id).Tables[0].GetDefaultView();
            }
        }

        private void Button_NewWell_Checked(object sender, RoutedEventArgs e)
        {
            if (grid1 != null)
                grid1.DataContext = new Model.COM_WELL_BASIC();
        }


        private void SearchWellBasic_Click(object sender, RoutedEventArgs e)
        {
            LoadWellList();
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            SearchWellBasic.DataContext = new Model.COM_WELL_BASIC();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid1.SelectedItem)) return;
            var row = e.AddedItems[0] as DataRowView;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetWellBasic(row.Row.Field<string>("WELL_ID")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");
                var model = Utility.ModelHandler<Model.COM_WELL_BASIC>.FillModel(dataTable.Rows[0]);
                grid1.DataContext = model;
                Utility.FileHelper.GetServerFileName(model.DRILL_GEO_DES_FILEID, filename => DRILL_GEO_DES_FILENAME.Text = filename);
                Utility.FileHelper.GetServerFileName(model.DRILL_ENG_DES_FILEID, filename => DRILL_ENG_DES_FILENAME.Text = filename);
                Button_ChangeWell.IsChecked = true; ;
                LoadWellboreList(row.Row.Field<string>("WELL_ID"));
            }
        }

        private void MenuItemSaveWellBasic_Click(object sender, RoutedEventArgs e)
        {
            var model = grid1.DataContext as Model.COM_WELL_BASIC;
            if (model == null || model.HasError()) throw new Exception("参数有误，请检查！");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var tasks = new Utility.DataCollection<UploadController.UploadTask>();
                UploadController.UploadTask task1 = null;
                UploadController.UploadTask task2 = null;
                if (File.Exists(DRILL_ENG_DES_FILENAME.Text))
                {
                    task1 = new UploadController.UploadTask { FullName = DRILL_ENG_DES_FILENAME.Text };
                    tasks.Add(task1);
                }
                if (File.Exists(DRILL_GEO_DES_FILENAME.Text))
                {
                    task2 = new UploadController.UploadTask { FullName = DRILL_GEO_DES_FILENAME.Text };
                    tasks.Add(task2);
                }
                new FileUpload().BeginUpload(tasks);
                if (tasks.Count > 0 && tasks.ToList().Find(t => t.hasError) != null)
                {
                    MessageBox.Show(App.Current.MainWindow, "上传出错，保存失败！");
                    return;
                }
                if (task1 != null) model.DRILL_ENG_DES_FILEID = task1.FileID;
                if (task2 != null) model.DRILL_GEO_DES_FILEID = task2.FileID;
                if (dataser.SaveWellBasic(Utility.ModelHelper.SerializeObject(model)))
                {

                    MessageBox.Show(App.Current.MainWindow, "井信息保存成功！");
                    LoadWellList();
                }
                else
                    MessageBox.Show(App.Current.MainWindow, "井信息保存失败！");
            }
        }

        private void Button_NewWellbore_Checked(object sender, RoutedEventArgs e)
        {
            if (grid2 != null)
            {
                var row = dataGrid1.SelectedItem as DataRowView;
                if (row != null)
                {
                    grid2.DataContext = new Model.COM_WELLBORE_BASIC() { WELL_ID = row.Row.Field<string>("WELL_ID") };
                    grid3.DataContext = new Model.COM_WELLSTRUCTURE_DATA();
                }
            }
        }

        private void MenuItemSaveWellbore_Click(object sender, RoutedEventArgs e)
        {
            var model1 = grid2.DataContext as Model.COM_WELLBORE_BASIC;
            var model2 = grid3.DataContext as Model.COM_WELLSTRUCTURE_DATA;
            if (model1 == null || model1.HasError() || model2 == null || model2.HasError()) throw new Exception("参数有误，请检查！");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.SaveWellbore(Utility.ModelHelper.SerializeObject(model1), Utility.ModelHelper.SerializeObject(model2)))
                {
                    MessageBox.Show(App.Current.MainWindow, "井筒信息保存成功！");
                    LoadWellboreList(model1.WELL_ID);
                }
                else
                    MessageBox.Show(App.Current.MainWindow, "井筒信息保存失败！");
            }
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid2.SelectedItem)) return;
            var row = e.AddedItems[0] as DataRowView;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetWellbore(row.Row.Field<string>("WELLBORE_ID")).Tables[0];
                var dt = dataser.GetWellStructure(row.Row.Field<string>("WELLBORE_ID")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1 || dt == null || dt.Rows.Count < 1) throw new Exception("没有找到数据！");
                grid2.DataContext = null;
                grid3.DataContext = null;
                grid2.DataContext = Utility.ModelHandler<Model.COM_WELLBORE_BASIC>.FillModel(dataTable.Rows[0]);
                grid3.DataContext = Utility.ModelHandler<Model.COM_WELLSTRUCTURE_DATA>.FillModel(dt.Rows[0]);
                //if (grid3.DataContext == null) grid3.DataContext = new Model.COM_BASE_WELLSTRUCTURE();
                Button_ChangeWellbore.IsChecked = true;
                //wellboreTab.IsSelected = true;
            }
        }

        private void CanUserDelete()
        {
            ContextMenu cm = (ContextMenu)dataGrid1.Resources["rowContextMenu"];
            using (UserServiceHelper userser = new UserServiceHelper())
            {
                if (userser.GetActiveUserRoles().Contains(UserService.UserRole.系统管理员))
                {
                    cm.Visibility = Visibility.Visible;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                //初始化甲方单位列表
                var PART_UNITSList = dataser.GetComboBoxList_甲方来电单位().Tables[0].GetDefaultView();
                PART_UNITS.DisplayMemberPath = "DEPARTMENT_REQUISITION_NAME";
                PART_UNITS.SelectedValuePath = "DEPARTMENT_REQUISITION_NAME";
                PART_UNITS.ItemsSource = PART_UNITSList;
                PART_UNITS1.DisplayMemberPath = "DEPARTMENT_REQUISITION_NAME";
                PART_UNITS1.SelectedValuePath = "DEPARTMENT_REQUISITION_NAME";
                PART_UNITS1.ItemsSource = PART_UNITSList;
            }

            LoadWellList();
            CanUserDelete();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Utility.FileHelper.SelectFile(filename => DRILL_GEO_DES_FILENAME.Text = filename);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var model = grid1.DataContext as Model.COM_WELL_BASIC;
            if (model.DRILL_GEO_DES_FILEID == null) return;
            ViewFile.View(new DownloadController.DownloadTask { FileID = model.DRILL_GEO_DES_FILEID });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Utility.FileHelper.SelectFile(filename => DRILL_ENG_DES_FILENAME.Text = filename);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var model = grid1.DataContext as Model.COM_WELL_BASIC;
            if (model.DRILL_ENG_DES_FILEID == null) return;
            ViewFile.View(new DownloadController.DownloadTask { FileID = model.DRILL_ENG_DES_FILEID });
        }
        private void Del_Well_Basic(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid1.SelectedItem as DataRowView;
            string well_id = dataRowView.Row.FieldEx<string>("WELL_ID");
            if (MessageBox.Show("确定要删除井及关联数据？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteComWellBasic(well_id))
                    {
                        dataGrid1.ItemsSource = dataser.SearchWellBasic(Utility.ModelHelper.SerializeObject(SearchWellBasic.DataContext)).Tables[0].GetDefaultView();
                        //Counts.Content = "      井：" + dataGrid1.Items.Count.ToString();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
    }
}
