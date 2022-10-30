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
using System.ComponentModel;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

//using Logging_App.Model;
using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// Log_Task_Page.xaml 的交互逻辑
    /// </summary>
    public partial class Log_Task_Page : Page
    {
        public Log_Task_Page()
        {
            InitializeComponent();
        }

        public string REQUISITION_CD = string.Empty;

        private void LoadDataList(int page)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                int total = 0;
                dataGrid1.ItemsSource = dataser.GetTaskList(ModelHelper.SerializeObject(SearchLogTask.DataContext),page,out total).Tables[0].GetDefaultView();
                //Counts.Content = "      通知单：" + total;
                dataPager1.TotalCount = total;
                if (!string.IsNullOrEmpty(REQUISITION_CD))
                {
                    var drv = (from DataRowView item in dataGrid1.Items
                               where item.Row.FieldEx<string>("REQUISITION_CD") == REQUISITION_CD
                               select item).FirstOrDefault();
                    if (drv != null)
                    {
                        dataGrid1.ScrollIntoView(drv);
                        dataGrid1.SelectedItem = drv;
                    }
                    REQUISITION_CD = string.Empty;
                };
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {

                //DataView job_purpose = dataser.GetComboBoxList_作业目的().GetDefaultView();
                //JOB_PURPOSE.DisplayMemberPath = "JOB_CHINESE_NAME";
                //JOB_PURPOSE.SelectedValuePath = "JOB_ID";
                //JOB_PURPOSE.ItemsSource = job_purpose;
                //JOB_PURPOSE1.DisplayMemberPath = "JOB_CHINESE_NAME";
                //JOB_PURPOSE1.SelectedValuePath = "JOB_ID";
                //JOB_PURPOSE1.ItemsSource = job_purpose;

                //WELL_JOB_NAME.DisplayMemberPath = "WELL_JOB_NAME";
                //WELL_JOB_NAME.SelectedValuePath = "DRILL_JOB_ID";
                //WELL_JOB_NAME.ItemsSource = dataser.GetWellJobNameList().GetDefaultView();
                var department_requisition_data = dataser.GetComboBoxList_甲方来电单位().Tables[0].GetDefaultView();
                department_requisition.DisplayMemberPath = "DEPARTMENT_REQUISITION_NAME";
                department_requisition.SelectedValuePath = "DEPARTMENT_REQUISITION_ID";
                department_requisition.ItemsSource = department_requisition_data;
                //DEPARTMENT_REQUISITION.DisplayMemberPath = "DEPARTMENT_REQUISITION_NAME";
                //DEPARTMENT_REQUISITION.SelectedValuePath = "DEPARTMENT_REQUISITION_ID";
                //DEPARTMENT_REQUISITION.ItemsSource = department_requisition_data;
                market_classify.DisplayMemberPath = "MARKET_TYPE";
                market_classify.SelectedValuePath = "MARKET_TYPE_ID";
                market_classify.ItemsSource = dataser.GetComboBoxList_市场类型().Tables[0].GetDefaultView();

                complete_man.DisplayMemberPath = "EXEC_TASK_UNIT_NAME";
                complete_man.SelectedValuePath = "EXEC_TASK_UNIT_NAME";
                complete_man.ItemsSource = dataser.GetComboBoxList_执行单位().Tables[0].GetDefaultView();

                DataView requisitionType = dataser.GetRequisitionTypes().Tables[0].GetDefaultView();
                REQUISITION_TYPE1.DisplayMemberPath = "REQUISITION_TYPE_NAME";
                REQUISITION_TYPE1.SelectedValuePath = "REQUISITION_TYPE_ID";
                REQUISITION_TYPE1.ItemsSource = requisitionType;
                REQUISITION_TYPE2.DisplayMemberPath = "REQUISITION_TYPE_NAME";
                REQUISITION_TYPE2.SelectedValuePath = "REQUISITION_TYPE_ID";
                REQUISITION_TYPE2.ItemsSource = requisitionType;

                requisition_type.DisplayMemberPath = "REQUISITION_TYPE_NAME";
                requisition_type.SelectedValuePath = "REQUISITION_TYPE_ID";
                requisition_type.ItemsSource = requisitionType;

                DataView queryDate = dataser.GetComboxList_QueryDate().Tables[0].GetDefaultView();
                QUERY_DATE.DisplayMemberPath = "QUREY_DATE";
                QUERY_DATE.SelectedValuePath = "DATE_VALUE";
                QUERY_DATE.ItemsSource = queryDate;
                //默认选中
                QUERY_DATE.Text = "最近一年";
                
            }
            //dataGrid2.Visibility = Visibility.Collapsed;
            //DM_LOG_TASK.DataContext = new Model.DM_LOG_TASK();
            ResetData();
            LoadDataList(1);
        }

        /// <summary>
        /// 刷新dataGrid1
        /// </summary>
        /// <param name="DaTa"></param>
        private void dataGrid2_updata(List<Model.TreeModel> treelist)
        {
            var items = dataGrid2.ItemsSource as Utility.DataCollection<Model.DM_LOG_PREDICTED_ITEM>;

            if (items == null)
            {
                items = new DataCollection<Model.DM_LOG_PREDICTED_ITEM>();
                foreach (Model.TreeModel tree in treelist)
                {
                    items.AddNew(new Model.DM_LOG_PREDICTED_ITEM() { LOG_ITEM_ID = tree.ID, PREDICTED_LOGGING_NAME = tree.Name });
                }
                dataGrid2.ItemsSource = items;
            }
            else
            {
                for (int i = 0; i < items.Count; ++i)
                {
                    if (treelist.FirstOrDefault(o => o.ID == items[i].LOG_ITEM_ID) == null)
                    {
                        items.Remove(items[i]);
                        --i;
                    }

                }

                foreach (Model.TreeModel tree in treelist)
                {
                    if (items.FirstOrDefault(o => o.LOG_ITEM_ID == tree.ID) == null)
                    {
                        var item = items.RemovedData.FirstOrDefault(o => o.LOG_ITEM_ID == tree.ID);
                        if (item != null)
                        {
                            items.AddNew(item);
                            continue;
                        }
                        items.AddNew(new Model.DM_LOG_PREDICTED_ITEM() { LOG_ITEM_ID = tree.ID, PREDICTED_LOGGING_NAME = tree.Name });
                    }
                }
            }

            ///深度赋值
            decimal? dep = null, st_dep = null, en_dep = null;
            var depText = predicted_logging_interval.Text;
            var arry = depText.Split('-');
            if (arry.Length == 2)
            {
                decimal st_dep_temp, en_dep_temp;
                if (Decimal.TryParse(arry[0], out st_dep_temp) && Decimal.TryParse(arry[1], out en_dep_temp))
                {
                    st_dep = st_dep_temp;
                    en_dep = en_dep_temp;
                }
            }
            else
            {
                decimal dep_temp;
                if (Decimal.TryParse(depText, out dep_temp))
                    dep = dep_temp;
            }
            if (dep != null)
            {
                st_dep = dep;
                en_dep = dep;
            }
            if (st_dep != null && en_dep != null)
            {
                foreach (var item in items)
                {
                    if (item.PRE_ST_DEP == null) item.PRE_ST_DEP = st_dep;
                    if (item.PRE_EN_DEP == null) item.PRE_EN_DEP = en_dep;
                }
            }

            //dataGrid2.Visibility = Visibility.Visible;
            //dataGrid2.ItemsSource = DaTa.GetDefaultView();
        }

        private void but_预测项目_Click(object sender, RoutedEventArgs e)
        {
            var items = dataGrid2.ItemsSource as Utility.DataCollection<Model.DM_LOG_PREDICTED_ITEM>;

            预测项目_Window w = new 预测项目_Window();
            w.Title = "预测项目";
            if (items != null) w.DataContext = items.Select(o => o.LOG_ITEM_ID).ToArray();
            w.Owner = Application.Current.MainWindow;
            w.ChangeTextEvent += new ChangeTextHandler_预测项目(dataGrid2_updata);
            w.ShowDialog();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            var data_测井任务通知单 = DM_LOG_TASK.DataContext as Model.DM_LOG_TASK;
            //var row = WELL_JOB_NAME.SelectedItem as DataRowView;
            //if (row == null || string.IsNullOrWhiteSpace(row.Row.FieldEx<string>("WELL_JOB_NAME")))
            //{
            //    MessageBox.Show("请选择作业井名！");
            //    return;
            //}
            //data_测井任务通知单.WELL_JOB_NAME = row.Row.FieldEx<string>("WELL_JOB_NAME");
            if (WELL_JOB_NAME.Text == "")
            {
                MessageBox.Show(App.Current.MainWindow,"请选择作业井名！"); return;
            }
            string WELLJOBNAME = WELL_JOB_NAME.Text.Trim();
            data_测井任务通知单.WELL_JOB_NAME = WELLJOBNAME;
            var dataList_预测项目信息 = dataGrid2.ItemsSource as Utility.DataCollection<Model.DM_LOG_PREDICTED_ITEM>;
            List<Model.DM_LOG_PREDICTED_ITEM> dataChangedList_预测项目信息 = null;
            List<Model.DM_LOG_PREDICTED_ITEM> dataRemoveList_预测项目信息 = null;
            if (dataList_预测项目信息 != null)
            {
                dataChangedList_预测项目信息 = dataList_预测项目信息.ChangedData;
                dataRemoveList_预测项目信息 = dataList_预测项目信息.RemovedData.FindAll(o => o.PREDICTED_LOGGING_ITEMS_ID != null);
            }
            if (data_测井任务通知单.HasError() || (dataList_预测项目信息 != null && dataList_预测项目信息.HasError()))
            {
                MessageBox.Show(App.Current.MainWindow,"输入数据有误，请检查！");
                return;
            }

            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var tasks = new Utility.DataCollection<UploadController.UploadTask>();
                if (File.Exists(REQUISITION_SCANNING_FILENAME.Text))
                {
                    tasks.Add(new UploadController.UploadTask { FullName = REQUISITION_SCANNING_FILENAME.Text });
                }
                new FileUpload().BeginUpload(tasks);
                if (tasks.Count > 0 && tasks.ToList().Find(t => t.hasError) != null)
                {
                    MessageBox.Show(App.Current.MainWindow,"上传出错，保存失败！");
                    return;
                }
                if (tasks.Count > 0)
                    data_测井任务通知单.REQUISITION_SCANNING_FILEID = tasks[0].FileID;
                if (A12DATA_TO_CJ(WELLJOBNAME) && dataser.Save_测井任务通知单(
                    Utility.ModelHelper.SerializeObject(data_测井任务通知单),
                    Utility.ModelHelper.SerializeObject(dataChangedList_预测项目信息),
                    Utility.ModelHelper.SerializeObject(dataRemoveList_预测项目信息)
                    ))
                {

                    ResetData();
                    LoadDataList(1);
                    if (dataList_预测项目信息 != null)
                    {
                        dataList_预测项目信息.RemovedData.Clear();
                        dataList_预测项目信息.ChangedData.Clear();
                    }
                    MessageBox.Show(App.Current.MainWindow,"保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow,"保存失败！");
            }
            //this.Close();
            //保存后，授权给任务完成人
            //执行测井作业收集信息,并跳转到计划任务书管理
            //被授权的完成人登陆，进行下面操作
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid1.SelectedItem)) return;
            var dataRowView = dataGrid1.SelectedItem as DataRowView;
            editButton.IsChecked = true;
            //WELL_JOB_NAME.IsReadOnly = false;
            ChangeMenuState();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var model = ModelHandler<Model.DM_LOG_TASK>.FillModel(dataser.GetData_测井任务通知单(dataRowView.Row.FieldEx<string>("REQUISITION_CD")).Tables[0].Rows[0]);
                DM_LOG_TASK.DataContext = model;
                Utility.FileHelper.GetServerFileName(model.REQUISITION_SCANNING_FILEID, filename => REQUISITION_SCANNING_FILENAME.Text = filename);
                dataGrid2.ItemsSource = Utility.ModelHandler<Model.DM_LOG_PREDICTED_ITEM>.FillModel(dataser.GetPredictedItems(dataRowView.Row.FieldEx<decimal>("PREDICTED_LOGGING_ITEMS_ID").ToString()));
                var data = dataser.GetData_通知单引用参数(dataRowView.Row.FieldEx<string>("WELL_JOB_NAME"));
                COM_WELL_BASIC.DataContext = data.Tables["COM_WELL_BASIC"].GetDefaultView();
                COM_JOB_INFO.DataContext = data.Tables["COM_JOB_INFO"].GetDefaultView();
                COM_WELLBORE_BASIC.DataContext = data.Tables["COM_WELLBORE_BASIC"].GetDefaultView();
                WELL_JOB_NAME.Text = data.Tables["COM_JOB_INFO"].Rows[0]["WELL_JOB_NAME"].ToString();
            }
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            SearchLogTask.DataContext = new Model.DM_LOG_TASK();
        }

        private void addButton_Checked(object sender, RoutedEventArgs e)
        {
            dataGrid1.SelectedIndex = -1;
            if (DM_LOG_TASK != null)
            {
                ResetData();
            }
        }

        //private void WELL_JOB_NAME_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    DataRowView dataRowView = WELL_JOB_NAME.SelectedItem as DataRowView;
        //    if (dataRowView == null) return;
        //    using (DataServiceHelper dataser = new DataServiceHelper())
        //    {
        //        COM_WELL_BASIC.DataContext = dataser.GetWellBaseInfo(dataRowView.Row.FieldEx<string>("WELL_JOB_NAME"));
        //        COM_JOB_INFO.DataContext = dataser.GetComJobInfo(dataRowView.Row.FieldEx<string>("WELL_JOB_NAME"));
        //    }
        //}

        private void ResetData()
        {
            REQUISITION_SCANNING_FILENAME.Text = string.Empty;
            DM_LOG_TASK.DataContext = new Model.DM_LOG_TASK();
            dataGrid2.ItemsSource = null;
            addButton.IsChecked = true;
            //WELL_JOB_NAME.IsReadOnly = true;
            COM_JOB_INFO.DataContext = null;
            COM_WELL_BASIC.DataContext = null;
            //menu1.IsEnabled = false;
            //menuSave.IsEnabled = false;
            workFlowControl1.ClearData();
            COM_WELLBORE_BASIC.DataContext = null;
            PRO_LOG_TESTOIL.DataContext = null;
            PRO_LOG_SLOP.DataContext = null;
            PRO_LOG_BIT_PROGRAM.DataContext = null;//A12
            PRO_LOG_CASIN.DataContext = null;//A12
            PRO_LOG_PRODUCE.DataContext = null;//A12
            ContextMenu cm = (ContextMenu)dataGrid1.Resources["rowContextMenu"];
            cm.Visibility = (Visibility)new BooleanToVisibilityConverter().Convert(workFlowControl1.CanDelete, null, null, null);// 
            //using (UserServiceHelper userser = new UserServiceHelper())
            //{
            //    var roles = userser.GetActiveUserRoles();
            //    if (roles.Contains(UserService.UserRole.调度管理员))
            //        menuSave.IsEnabled = true;
            //    if (roles.Contains(UserService.UserRole.系统管理员))
            //        cm.Visibility = Visibility.Visible;
            //}
        }

        private void SearchLogTask_Click(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                ResetData();
                LoadDataList(1);
                //dataGrid1.ItemsSource = dataser.GetTaskList(ModelHelper.SerializeObject(SearchLogTask.DataContext)).Tables[0].GetDefaultView();
               // Counts.Content = "      通知单：" + dataGrid1.Items.Count.ToString();
            }
        }

        private void ChangeMenuState()
        {
            //menu1.IsEnabled = false;
            //menuSave.IsEnabled = false;
            var dataRowView = dataGrid1.SelectedItem as DataRowView;
            workFlowControl1.LoadData(dataRowView.Row.FieldEx<string>("REQUISITION_CD"));
            //var flowData = workFlowControl1.GetData();
            //using (UserServiceHelper userser = new UserServiceHelper())
            //{
            //    if (userser.GetActiveUserRoles().Contains(UserService.UserRole.调度管理员))
            //    {
            //        if (flowData == null || MyHomePage.ActiveUser.COL_LOGINNAME == flowData[0].TARGET_LOGINNAME)
            //        {
            //            menu1.IsEnabled = true;
            //            menuSave.IsEnabled = true;
            //        }
            //    }
            //}
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectUserWindow();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
            var data = DM_LOG_TASK.DataContext as Model.DM_LOG_TASK;
            //using (var dataser = new DataServiceHelper())
            //{
            //    if (window.UserModel != null && dataser.SetWorkFlow_通知单指派(data.REQUISITION_CD, window.UserModel.COL_LOGINNAME))
            //    {
            //        ChangeMenuState();
            //    }
            //}
            if (window.UserModel != null)
                workFlowControl1.AppointData(window.UserModel.COL_LOGINNAME,data.REQUISITION_CD );
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Utility.FileHelper.SelectFile(filename => REQUISITION_SCANNING_FILENAME.Text = filename);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var model = DM_LOG_TASK.DataContext as Model.DM_LOG_TASK;
            if (model.REQUISITION_SCANNING_FILEID == null) return;
            ViewFile.View(new DownloadController.DownloadTask { FileID = model.REQUISITION_SCANNING_FILEID });
        }

        private void rec_notice_time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (menuSave.IsEnabled && e.NewValue != null)
            {
                treatment_date_requisition.Value = ((DateTime)e.NewValue).AddMinutes(10);
            }
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid1.SelectedItem as DataRowView;
            string requisition_cd = dataRowView.Row.FieldEx<string>("REQUISITION_CD");
            if (MessageBox.Show(App.Current.MainWindow,"删除通知单会删除与之关联的计划任务书、测井作业收集信息、测井现场提交信息\n确定要删除“" + requisition_cd + "”吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogTask(requisition_cd))
                    {
                        ResetData();
                        LoadDataList(1);
                        MessageBox.Show(App.Current.MainWindow,"删除成功！");
                    }
                    else
                        MessageBox.Show(App.Current.MainWindow,"删除失败！");
                }
        }

        #region A12
        /// <summary>
        /// 获取保存井基本信息
        /// </summary>
        /// <param name="WELL_JOB_NAME"></param>
        /// <returns></returns>
        public bool A12DATA_TO_CJ(string WELL_JOB_NAME)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var or_well = dataser.GetComJobInfo(WELL_JOB_NAME).Tables[0];
                if (or_well != null && or_well.Rows.Count > 0) return true;
                else
                {
                    if (dataser.Save_ALL_WELL_DATA(WELL_JOB_NAME))
                    {
                        DRILL_JOB_ID.Text = dataser.GetData_COMJOBINFO(WELL_JOB_NAME).Tables[0].Rows[0]["DRILL_JOB_ID"].ToString();
                        return true;
                    }
                    return false;
                }
            }
        }
        public bool A12DATA_TO_CJ_LOG(string WELL_JOB_NAME)
        {
            return false;
        }
        private void WELL_JOB_NAME_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (addButton.IsChecked == true)
            {
                COM_WELL_BASIC_Window w = new COM_WELL_BASIC_Window();
                w.Title = "COM_WELL_BASIC";
                w.Owner = Application.Current.MainWindow;
                w.ChangeTextEvent += new ChangeTextHandler_井信息(COM_WELL_BASIC_Window);
                w.ShowDialog();
            }
        }
        private void COM_WELL_BASIC_Window(string WELL_JOB_NAMEs, string JOB_ID)
        {
            WELL_JOB_NAME.Text = WELL_JOB_NAMEs;
            using (var dataser = new DataServiceHelper())
            {
                if (dataser.Save_SYS_WELL_DATA(JOB_ID))
                {
                    var data_cache = dataser.GetList_SYS_WELL_CACHE_NAME(WELL_JOB_NAMEs).Tables[0];
                    COM_WELL_BASIC.DataContext = data_cache;
                    COM_JOB_INFO.DataContext = data_cache;
                    COM_WELLBORE_BASIC.DataContext = data_cache;
                    PRO_LOG_TESTOIL.DataContext = data_cache;
                    PRO_LOG_SLOP.DataContext = data_cache;
                    PRO_LOG_BIT_PROGRAM.DataContext = data_cache;//A12
                    PRO_LOG_CASIN.DataContext = data_cache;//A12
                    PRO_LOG_PRODUCE.DataContext = data_cache;//A12
                }
                else
                {
                    var data_cache = dataser.GetList_SYS_WELL_CACHE_NAME(WELL_JOB_NAMEs).Tables[0];
                    if (data_cache != null && data_cache.Rows.Count > 0)//读取缓存中信息，如果没用，说明这口井是自己创建，读取本地信息。
                    {
                        COM_WELL_BASIC.DataContext = data_cache;
                        COM_JOB_INFO.DataContext = data_cache;
                        COM_WELLBORE_BASIC.DataContext = data_cache;
                        PRO_LOG_TESTOIL.DataContext = data_cache;
                        PRO_LOG_SLOP.DataContext = data_cache;
                        PRO_LOG_BIT_PROGRAM.DataContext = data_cache;//A12
                        PRO_LOG_CASIN.DataContext = data_cache;//A12
                        PRO_LOG_PRODUCE.DataContext = data_cache;//A12
                    }
                    else
                    {
                        var data = dataser.GetData_通知单引用参数(WELL_JOB_NAMEs);
                        COM_WELL_BASIC.DataContext = data.Tables["COM_WELL_BASIC"].GetDefaultView();
                        COM_JOB_INFO.DataContext = data.Tables["COM_JOB_INFO"].GetDefaultView();
                        COM_WELLBORE_BASIC.DataContext = data.Tables["COM_WELLBORE_BASIC"].GetDefaultView();
                        DRILL_JOB_ID.Text = data.Tables["COM_JOB_INFO"].Rows[0]["DRILL_JOB_ID"].ToString();
                        //COM_WELL_BASIC.DataContext = dataser.GetWellBaseInfo(WELL_JOB_NAME);
                        //COM_JOB_INFO.DataContext = dataser.GetComJobInfo(WELL_JOB_NAME);
                    }
                }
            }
        }
        #endregion

        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            int page= (int)e.CurrentPageIndex;
            ResetData();
            LoadDataList(page);
        }
    }
}
