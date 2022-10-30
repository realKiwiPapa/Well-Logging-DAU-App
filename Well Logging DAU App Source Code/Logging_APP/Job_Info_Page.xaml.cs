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

using System.Data;
using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// Job_InfoPage.xaml 的交互逻辑
    /// </summary>
    public partial class Job_Info_Page : Page
    {
        public Job_Info_Page()
        {
            InitializeComponent();
        }

        private void LoadJobInfoList()
        {
            COM_JOB_INFO.DataContext = new Model.COM_JOB_INFO();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataGrid1.ItemsSource = dataser.GetJobInfoList(Utility.ModelHelper.SerializeObject(SearchJobInfo.DataContext)).Tables[0].GetDefaultView();
                well_id.ItemsSource = dataser.GetWellComboBoxList().Tables[0].GetDefaultView();
                well_type.ItemsSource = dataser.GetWellType().Tables[0].GetDefaultView();
                well_sort.ItemsSource = dataser.GetWellSort().Tables[0].GetDefaultView();
            }
            Button_NewJobInfo.IsChecked = true;
            Button_ChangeDrillState.IsChecked = true;
            Button_ChangeGEO_DES_ITEM.IsChecked = true;
            gb1.Visibility = gb2.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SearchJobInfo.DataContext = new Model.JobInfoSearch();
            LoadJobInfoList();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            if (row == null) return;
            well_id.IsEnabled = false;
            //well_job_name.IsReadOnly = true;
            string job_id = row.Row.FieldEx<string>("DRILL_JOB_ID");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                COM_JOB_INFO.DataContext = Utility.ModelHandler<Model.COM_JOB_INFO>.FillModel(dataser.GetJobInfo(job_id).Tables[0].Rows[0]);
                dataGrid2.ItemsSource = dataser.GetDrillStateList(job_id).Tables[0].GetDefaultView();
                dataGrid3.ItemsSource = dataser.GetGEO_DES_ItemList(job_id).Tables[0].GetDefaultView();
            }
            Button_ChangeJobInfo.IsChecked = true;
            Button_NewDrillState.IsChecked = true;
            Button_NewGEO_DES_ITEM.IsChecked = true;
            DM_LOG_DRILL_STATE.DataContext = new Model.DM_LOG_DRILL_STATE();
            DM_LOG_GEO_DES_ITEM.DataContext = new Model.DM_LOG_GEO_DES_ITEM();
            gb1.Visibility = gb2.Visibility = Visibility.Visible;
        }

        private void Button_NewJobInfo_Checked(object sender, RoutedEventArgs e)
        {
            if (COM_JOB_INFO != null) 
            {
                dataGrid1.SelectedIndex = -1;
                gb1.Visibility = gb2.Visibility = Visibility.Collapsed;
                COM_JOB_INFO.DataContext = new Model.COM_JOB_INFO();
                well_id.IsEnabled = true;
                well_job_name.IsReadOnly = false;
            }
        }

        private void MenuItemSaveJobInfo_Click(object sender, RoutedEventArgs e)
        {
            var model = COM_JOB_INFO.DataContext as Model.COM_JOB_INFO;
            if (model == null || model.HasError()) throw new Exception("参数有误，请检查！");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.SaveJobInfo(Utility.ModelHelper.SerializeObject(model)))
                {
                    LoadJobInfoList();
                    MessageBox.Show(App.Current.MainWindow,"保存成功！");
                }
                else
                    MessageBox.Show(App.Current.MainWindow,"保存失败！");
            }
        }

        private void SearchJobInfo_Click(object sender, RoutedEventArgs e)
        {
            LoadJobInfoList();
        }

        private void well_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && !well_job_name.IsReadOnly)
            {
                var row = e.AddedItems[0] as DataRowView;
                if (row != null)
                    well_job_name.Text = row.Row.FieldEx<string>("WELL_NAME");
            }
        }

        private void Button_NewDrillState_Checked(object sender, RoutedEventArgs e)
        {
            if (DM_LOG_DRILL_STATE != null)
            {
                DM_LOG_DRILL_STATE.DataContext = new Model.DM_LOG_DRILL_STATE();
            }
        }

        private void MenuItemSaveDrillState_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            if (row == null) return;
            string job_id = row.Row.FieldEx<string>("DRILL_JOB_ID");
            var model = DM_LOG_DRILL_STATE.DataContext as Model.DM_LOG_DRILL_STATE;
            if (model == null || model.HasError()) throw new Exception("参数有误，请检查！");
            model.DRILL_JOB_ID = job_id;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.SaveDirllState(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid2.ItemsSource = dataser.GetDrillStateList(model.DRILL_JOB_ID).Tables[0].GetDefaultView();
                    DM_LOG_DRILL_STATE.DataContext=new Model.DM_LOG_DRILL_STATE();
                     Button_NewDrillState.IsChecked = true;
                    MessageBox.Show(App.Current.MainWindow,"保存成功！");
                }
                else
                    MessageBox.Show(App.Current.MainWindow,"保存失败！");
            }
        }

        private void Button_NewGEO_DES_ITEM_Checked(object sender, RoutedEventArgs e)
        {
            if (DM_LOG_GEO_DES_ITEM != null)
            {
                DM_LOG_GEO_DES_ITEM.DataContext = new Model.DM_LOG_GEO_DES_ITEM();
            }
        }

        private void MenuItemSaveGEO_DES_ITEM_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            if (row == null) return;
            string job_id = row.Row.FieldEx<string>("DRILL_JOB_ID");
            var model = DM_LOG_GEO_DES_ITEM.DataContext as Model.DM_LOG_GEO_DES_ITEM;
            if (model == null || model.HasError()) throw new Exception("参数有误，请检查！");
            model.DRILL_JOB_ID = job_id;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.SaveGEO_DES_ITEM(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid3.ItemsSource = dataser.GetGEO_DES_ItemList(model.DRILL_JOB_ID).Tables[0].GetDefaultView();
                    DM_LOG_GEO_DES_ITEM.DataContext = new Model.DM_LOG_GEO_DES_ITEM();
                    Button_NewGEO_DES_ITEM.IsChecked = true;
                    MessageBox.Show(App.Current.MainWindow,"保存成功！");
                }
                else
                    MessageBox.Show(App.Current.MainWindow,"保存失败！");
            }
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = dataGrid2.SelectedItem as DataRowView;
            if (row == null) return;
            string drill_well_id = row.Row.FieldEx<string>("DRILL_WELL_ID");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                DM_LOG_DRILL_STATE.DataContext = Utility.ModelHandler<Model.DM_LOG_DRILL_STATE>.FillModel(dataser.GetDrillState(drill_well_id).Tables[0].Rows[0]);
            }
            Button_ChangeDrillState.IsChecked = true;
        }

        private void dataGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = dataGrid3.SelectedItem as DataRowView;
            if (row == null) return;
            string drill_well_id = row.Row.FieldEx<string>("DRILL_WELL_ID");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                DM_LOG_GEO_DES_ITEM.DataContext = Utility.ModelHandler<Model.DM_LOG_GEO_DES_ITEM>.FillModel(dataser.GetGEO_DES_Item(drill_well_id).Tables[0].Rows[0]);
            }
            Button_ChangeGEO_DES_ITEM.IsChecked = true;
        }
    }
}
