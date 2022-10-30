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
using Logging_App.Utility;
using System.Data;

namespace Logging_App
{
    /// <summary>
    /// LOG_OPS_PLAN_Page.xaml 的交互逻辑
    /// </summary>
    public partial class LOG_OPS_PLAN_Page : Page
    {
        public LOG_OPS_PLAN_Page()
        {
            InitializeComponent();
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            Model.DM_LOG_OPS_PLAN data_测井任务计划书 = DM_LOG_OPS_PLAN.DataContext as Model.DM_LOG_OPS_PLAN;
            if (string.IsNullOrWhiteSpace(data_测井任务计划书.REQUISITION_CD))
            {
                MessageBox.Show(App.Current.MainWindow,"请选择通知单编码！");
                return;
            }
            if (data_测井任务计划书.HasError())
            {
                MessageBox.Show(App.Current.MainWindow,"输入数据有误，请检查！");
                return;
            }

            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var tasks = new Utility.DataCollection<UploadController.UploadTask>();
                if (File.Exists(PLAN_CONTENT_SCANNING_FILENAME.Text))
                {//FileID = (decimal)data_测井任务计划书.PLAN_CONTENT_SCANNING_FILEID,
                    tasks.Add(new UploadController.UploadTask {  FullName = PLAN_CONTENT_SCANNING_FILENAME.Text });
                }
                new FileUpload().BeginUpload(tasks);
                if (tasks.Count > 0 && tasks.ToList().Find(t => t.hasError) != null)
                {
                    MessageBox.Show(App.Current.MainWindow,"上传出错，保存失败！");
                    return;
                }
                if (tasks.Count > 0) data_测井任务计划书.PLAN_CONTENT_SCANNING_FILEID = tasks[0].FileID;
                if (dataser.Save_测井任务计划书(Utility.ModelHelper.SerializeObject(data_测井任务计划书)))
                {
                    //try
                    //{
                    //    dataser.Save_ALL_JOB_DATA(data_测井任务计划书.REQUISITION_CD, data_测井任务计划书.REQUISITION_CD + "-" + data_测井任务计划书.LOG_TEAM_ID);
                    //}
                    //catch
                    //{ }
                    ResetData(1);
                    MessageBox.Show(App.Current.MainWindow,"保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow,"保存失败！");
            }


            //this.Close();
            //通知单编码，获取测井任务通知单管理表格中的通知单编码，成下拉选择形式。
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            // Model.DM_LOG_OPS_PLAN model = new Model.DM_LOG_OPS_PLAN();
            //grid1.DataContext = model;
            // DM_LOG_OPS_PLAN.DataContext = model;
            SearchPanel.DataContext = new Model.LogPlanSearch();
        }

        private void ChangeMenuState()
        {
            //menu1.IsEnabled = false;
            //menu2.IsEnabled = false;
            //menuSave.IsEnabled = false;
            //menuSave.IsEnabled = true;
            var model = grid1.DataContext as Model.DM_LOG_OPS_PLAN;
            //if (string.IsNullOrEmpty(model.REQUISITION_CD)) return;
            //menuSave.IsEnabled = false;
            var dataRowView = dataGrid1.SelectedItem as DataRowView;
            //using (DataServiceHelper dataser = new DataServiceHelper())
            //{
            //    using (UserServiceHelper userser = new UserServiceHelper())
            //    {
            //        var role = userser.GetActiveUserRoles();
            //        var flowData1 = Utility.ModelHandler<Model.SYS_WORK_FLOW>.FillModel(dataser.GetWorkFlow(model.REQUISITION_CD, (int)Model.SYS_Enums.WorkFlowType.测井任务通知单));
            workFlowControl1.LoadData(model.JOB_PLAN_CD, model.REQUISITION_CD);
            //var flowData2 = workFlowControl1.GetData();
            //if (flowData1[0].TARGET_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME && !role.Contains(UserService.UserRole.调度管理员) && (flowData2 == null || flowData2[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.审核未通过))
            //{
            //    menuSave.IsEnabled = true;
            //    menu1.IsEnabled = true;
            //}

            //if (role.Contains(UserService.UserRole.调度管理员) && flowData2 != null && flowData2[0].TARGET_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME && flowData2[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.提交审核)
            //{
            //    menu1.IsEnabled = true;
            //    menu2.IsEnabled = true;
            //}
            // }
            //}
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid1.SelectedItem)) return;
            PRO_LOG_BIT_PROGRAM.DataContext = null;//A12
            PRO_LOG_CASIN.DataContext = null;//A12
            PRO_LOG_SLOP.DataContext = null;//A12
            PRO_LOG_PRODUCE.DataContext = null;//A12
            COM_WELLBORE_BASIC.DataContext = null;//A12
            COM_WELL_BASIC.DataContext = null;//A12

            var dataRowView = dataGrid1.SelectedItem as DataRowView;
            if (dataRowView == null) return;
            editButton.IsChecked = true;
            requisition_cd.Visibility = Visibility.Collapsed;
            requisition_cd1.Visibility = Visibility.Visible;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var model = Utility.ModelHandler<Model.DM_LOG_OPS_PLAN>.FillModel(dataser.GetPlanData(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].Rows[0]);
                grid1.DataContext = model;
                Utility.FileHelper.GetServerFileName(model.PLAN_CONTENT_SCANNING_FILEID, filename => PLAN_CONTENT_SCANNING_FILENAME.Text = filename);
                requisition_cd1.Text = model.REQUISITION_CD;
                DM_LOG_OPS_PLAN.DataContext = model;
                var dt = dataser.GetPredictedItems(dataRowView["PREDICTED_LOGGING_ITEMS_ID"].ToString()).Tables[0];
                dataGrid2.ItemsSource = dt == null ? null : dt.GetDefaultView();

                var data_cache = dataser.GetList_SYS_WELL_CACHE_TASK(dataRowView.Row.FieldEx<string>("REQUISITION_CD")).Tables[0];
                if (data_cache != null && data_cache.Rows.Count > 0)
                {
                    PRO_LOG_BIT_PROGRAM.DataContext = data_cache;//A12
                    PRO_LOG_CASIN.DataContext = data_cache;//A12
                    PRO_LOG_SLOP.DataContext = data_cache;//A12
                    PRO_LOG_PRODUCE.DataContext = data_cache;//A12
                    COM_WELLBORE_BASIC.DataContext = data_cache;//A12
                    COM_WELL_BASIC.DataContext = data_cache;//A12
                }
                else
                {
                    var data = dataser.GetData_计划任务书参数显示(dataRowView.Row.FieldEx<string>("REQUISITION_CD"));
                    COM_WELL_BASIC.DataContext = data.Tables["COM_WELL_BASIC"].GetDefaultView();
                    COM_WELLBORE_BASIC.DataContext = data.Tables["COM_WELLBORE_BASIC"].GetDefaultView();
                    PRO_LOG_SLOP.DataContext = data.Tables["PRO_LOG_SLOP"].GetDefaultView();
                    PRO_LOG_BIT_PROGRAM.DataContext = data.Tables["PRO_LOG_BIT_PROGRAM"].GetDefaultView();
                    PRO_LOG_CASIN.DataContext = data.Tables["PRO_LOG_CASIN"].GetDefaultView();
                    PRO_LOG_PRODUCE.DataContext = data.Tables["PRO_LOG_PRODUCE"].GetDefaultView();
                }
            }
            ChangeMenuState();
            setDefaultValue(dataRowView.Row.FieldEx<string>("REQUISITION_CD"));
        }

        private void addButton_Checked(object sender, RoutedEventArgs e)
        {
            dataGrid1.SelectedIndex = -1;
            if (grid1 == null) return;
            ResetData(1);
        }

        private void ResetData(int page)
        {
            PLAN_CONTENT_SCANNING_FILENAME.Text = string.Empty;
            requisition_cd1.Visibility = Visibility.Collapsed;
            requisition_cd.Visibility = Visibility.Visible;
            addButton.IsChecked = true;
            Model.DM_LOG_OPS_PLAN model = new Model.DM_LOG_OPS_PLAN();
            dataGrid2.ItemsSource = null;
            grid1.DataContext = model;
            DM_LOG_OPS_PLAN.DataContext = model;
            PRO_LOG_BIT_PROGRAM.DataContext = null;//A12
            PRO_LOG_CASIN.DataContext = null;//A12
            PRO_LOG_SLOP.DataContext = null;//A12
            PRO_LOG_PRODUCE.DataContext = null;//A12
            COM_WELLBORE_BASIC.DataContext = null;//A12
            COM_WELL_BASIC.DataContext = null;//A12
            //menu1.IsEnabled = false;
            //menu2.IsEnabled = false;
            //menuSave.IsEnabled = true;
            workFlowControl1.ClearData();
            LoadPlanList(page);
        }

        public string JOB_PLAN_CD = string.Empty;
        private void LoadPlanList(int page)
        {
            if (SearchPanel.DataContext is Model.LogPlanSearch)
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    int total = 0;
                    dataGrid1.ItemsSource = dataser.GetPlanList(Utility.ModelHelper.SerializeObject(SearchPanel.DataContext),page,out total).Tables[0].GetDefaultView();
                    //Counts.Content = "      任务书：" + total;
                    dataPager1.TotalCount = total;
                    if (!string.IsNullOrEmpty(JOB_PLAN_CD))
                    {
                        var drv = (from DataRowView item in dataGrid1.Items
                                   where item.Row.FieldEx<string>("JOB_PLAN_CD") == JOB_PLAN_CD
                                   select item).FirstOrDefault();
                        if (drv != null)
                        {
                            dataGrid1.ScrollIntoView(drv);
                            dataGrid1.SelectedItem = drv;
                        }
                        JOB_PLAN_CD = string.Empty;
                    };
                }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                requisition_cd.DisplayMemberPath = "REQUISITION_CD";
                requisition_cd.SelectedValuePath = "REQUISITION_CD";
                requisition_cd.ItemsSource = dataser.GetRequisitionCdList().Tables[0].GetDefaultView();
                var jobList = dataser.GetJobIDlist().Tables[0].GetDefaultView();
                job_id.DisplayMemberPath = "JOB_CHINESE_NAME";
                job_id.SelectedValuePath = "JOB_ID";
                job_id.ItemsSource = jobList;
                job_id1.DisplayMemberPath = "JOB_CHINESE_NAME";
                job_id1.SelectedValuePath = "JOB_ID";
                job_id1.ItemsSource = jobList;
                DataView logIteam = dataser.GetTeamList().Tables[0].GetDefaultView();
                log_team_id.DisplayMemberPath = "TEAM_ORG_ID";
                log_team_id.SelectedValuePath = "TEAM_ORG_ID";
                log_team_id.ItemsSource = logIteam;
                log_type.ItemsSource = dataser.GetLogType().Tables[0].GetDefaultView();
                log_mode.ItemsSource = dataser.GetLogMode().Tables[0].GetDefaultView();
                //LOG_TEAM_ID.DisplayMemberPath = "TEAM_ORG_ID";
                //LOG_TEAM_ID.SelectedValuePath = "TEAM_ORG_ID";
                //LOG_TEAM_ID.ItemsSource = logIteam;

                DataView queryDate = dataser.GetComboxList_QueryDate().Tables[0].GetDefaultView();
                QUERY_DATE.DisplayMemberPath = "QUREY_DATE";
                QUERY_DATE.SelectedValuePath = "DATE_VALUE";
                QUERY_DATE.ItemsSource = queryDate;
                QUERY_DATE.Text = "最近一年";
            }
            ResetData(1);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ResetData(1);
        }

        private void Menu1Item_Click(object sender, RoutedEventArgs e)
        {
            var w = new SelectUserWindow1();
            w.UserRole = UserService.UserRole.调度管理员;
            w.Owner = System.Windows.Application.Current.MainWindow;
            w.ShowDialog();
            if (string.IsNullOrEmpty(w.LoginName)) return;
            var data = grid1.DataContext as Model.DM_LOG_OPS_PLAN;
            //using (DataServiceHelper dataser = new DataServiceHelper())
            //{
            //    if (dataser.SetLogPlanState(data.JOB_PLAN_CD, w.LoginName, (int)Model.SYS_Enums.FlowState.提交审核))
            //        MessageBox.Show("提交审核成功！");
            //    ChangeMenuState();
            //}
            if (workFlowControl1.SubmitReview(w.LoginName, data.JOB_PLAN_CD, data.REQUISITION_CD))
                ChangeMenuState();
        }

        private void Menu2Item_Click(object sender, RoutedEventArgs e)
        {
            var data = grid1.DataContext as Model.DM_LOG_OPS_PLAN;
            //var flowData = workFlowControl1.GetData();
            //using (DataServiceHelper dataser = new DataServiceHelper())
            //{
            //var result = MessageBox.Show("是否通过审核？", "任务计划书审核", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            //int state = -1;
            //if (result == MessageBoxResult.Yes) state = (int)Model.SYS_Enums.FlowState.审核通过;
            //if (result == MessageBoxResult.No) state = (int)Model.SYS_Enums.FlowState.审核未通过;
            //if (state > -1 && dataser.SetLogPlanState(data.JOB_PLAN_CD, flowData[flowData.Length - 1].SOURCE_LOGINNAME, state))
            //    MessageBox.Show("审核成功！");
            //ChangeMenuState();
            //}
            workFlowControl1.Review("任务计划书审核", data.JOB_PLAN_CD);
            ChangeMenuState();
        }

        private void Menu3Item_Click(object sender, RoutedEventArgs e)
        {
            var data = grid1.DataContext as Model.DM_LOG_OPS_PLAN;
            workFlowControl1.CancelSubmitReview("小队施工信息审核取消提交审核", data.JOB_PLAN_CD);
        }

        private void Menu4Item_Click(object sender, RoutedEventArgs e)
        {
            var data = grid1.DataContext as Model.DM_LOG_OPS_PLAN;
            workFlowControl1.CancelReview("小队施工信息退回审核", data.JOB_PLAN_CD);
        }

        private void setDefaultValue(string requisition_cd, bool forceUpdate = false)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var modelTemp = Utility.ModelHandler<Model.DM_LOG_TASK>.FillModel(dataser.GetData_测井任务通知单(requisition_cd).Tables[0].Rows[0]);
                if (string.IsNullOrWhiteSpace(prelogging_interval.Text) || forceUpdate)
                    prelogging_interval.Text = modelTemp.PREDICTED_LOGGING_INTERVAL;
                if (string.IsNullOrWhiteSpace(approver.Text) || forceUpdate)
                    approver.Text = modelTemp.VERIFIER;
                if (string.IsNullOrWhiteSpace(verifier.Text) || forceUpdate)
                    verifier.Text = modelTemp.VERIFIER;
                if (received_inform_time.Value == null || forceUpdate)
                {
                    //received_inform_time.Value = dataser.GetWorkFlow(requisition_cd, 0).Select("FLOW_STATE=0", "FLOW_ID DESC")[0].FieldEx<DateTime>("FLOW_TIME");
                    var data = workFlowControl1.QueryData(WorkflowService.WorkflowType.测井任务通知单, requisition_cd).DataList;
                    if (data != null && data.Length > 0)
                        received_inform_time.Value = data.FirstOrDefault(d => d.FLOW_STATE == WorkflowService.WorkflowState.数据指派).FLOW_TIME;

                }
                if (requirements_time.Value == null || forceUpdate)
                    requirements_time.Value = modelTemp.PREDICTED_LOGGING_DATE;
            }
        }

        private void requisition_cd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                var row = e.AddedItems[0] as DataRowView;
                setDefaultValue(row.Row.FieldEx<string>("REQUISITION_CD"), true);
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    var dt = dataser.GetPredictedItems(row["PREDICTED_LOGGING_ITEMS_ID"].ToString()).Tables[0];
                    dataGrid2.ItemsSource = dt == null ? null : dt.GetDefaultView();

                    var data_cache = dataser.GetList_SYS_WELL_CACHE_TASK(row.Row.ItemArray[0].ToString()).Tables[0];
                    if (data_cache != null && data_cache.Rows.Count > 0)
                    {
                        PRO_LOG_BIT_PROGRAM.DataContext = data_cache;//A12
                        PRO_LOG_CASIN.DataContext = data_cache;//A12
                        PRO_LOG_SLOP.DataContext = data_cache;//A12
                        PRO_LOG_PRODUCE.DataContext = data_cache;//A12
                        COM_WELLBORE_BASIC.DataContext = data_cache;//A12
                        COM_WELL_BASIC.DataContext = data_cache;//A12
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Utility.FileHelper.SelectFile(filename => PLAN_CONTENT_SCANNING_FILENAME.Text = filename);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var model = grid1.DataContext as Model.DM_LOG_OPS_PLAN;
            if (model.PLAN_CONTENT_SCANNING_FILEID == null) return;
            ViewFile.View(new DownloadController.DownloadTask { FileID = model.PLAN_CONTENT_SCANNING_FILEID });
        }

        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            int page = (int)e.CurrentPageIndex;
            ResetData(page);
        }
    }
}
