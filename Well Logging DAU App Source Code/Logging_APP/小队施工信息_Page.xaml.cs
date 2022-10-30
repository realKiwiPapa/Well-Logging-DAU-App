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
using System.Data;
using System.IO;
using Logging_App.Utility;
using Microsoft.Win32;
using System.Web.Script.Serialization;
using System.Threading;

namespace Logging_App
{
    /// <summary>
    /// 小队施工信息_Page.xaml 的交互逻辑
    /// </summary>
    public partial class 小队施工信息_Page
    {
        public 小队施工信息_Page()
        {
            InitializeComponent();
        }

        public string JOB_PLAN_CD = string.Empty;
        private void ResetData()
        {
            DataRowView dataRowView = dataGrid1.SelectedItem as DataRowView;
            int selectedIndex = tabControl1.SelectedIndex;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataRowView == null)
                {
                    //DM_LOG_BASE.DataContext = new Model.DM_LOG_BASE();
                    //DM_LOG_WORK_ANING.DataContext = new Model.DM_LOG_WORK_ANING();
                    //DM_LOG_WORK_PERSONNEL.DataContext = new Model.DM_LOG_WORK_PERSONNEL();
                    //DM_LOG_UP_EQUIP.DataContext = new Model.DM_LOG_UP_EQUIP();

                    //JOB_PURPOSE1.DisplayMemberPath = "JOB_CHINESE_NAME";
                    //JOB_PURPOSE1.SelectedValuePath = "JOB_ID";
                    //JOB_PURPOSE1.ItemsSource = dataser.GetComboBoxList_作业目的().GetDefaultView();
                    //REQUISITION_TYPE.DisplayMemberPath = "LOG_ITEM_TYPE";
                    //REQUISITION_TYPE.SelectedValuePath = "LOG_ITEM_ID";
                    //REQUISITION_TYPE.ItemsSource = dataser.GetRequisitionTypes().GetDefaultView();
                    drill_statuse.DisplayMemberPath = "STATUS_NAME";
                    drill_statuse.SelectedValuePath = "STATUS_NAME";
                    drill_statuse.ItemsSource = dataser.GetComboBoxList_钻井状态().Tables[0].GetDefaultView();
                    job_id1.DisplayMemberPath = "JOB_CHINESE_NAME";
                    job_id1.SelectedValuePath = "JOB_ID";
                    job_id1.ItemsSource = dataser.GetJobIDlist().Tables[0].GetDefaultView();
                    log_type.DisplayMemberPath = "LOG_TYPE_NAME";
                    log_type.SelectedValuePath = "LOG_TYPE_NAME";
                    log_type.ItemsSource = dataser.GetLogType().Tables[0].GetDefaultView();
                    log_mode.DisplayMemberPath = "CONSTRUCTION_TECHNOLOGY_NAME";
                    log_mode.SelectedValuePath = "CONSTRUCTION_TECHNOLOGY_NAME";
                    log_mode.ItemsSource = dataser.GetLogMode().Tables[0].GetDefaultView();
                    log_series.DisplayMemberPath = "LOG_SERIES_NAME";
                    log_series.SelectedValuePath = "LOG_SERIES_NAME";
                    log_series.ItemsSource = dataser.GetComboBoxList_LogSeries().Tables[0].GetDefaultView();
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
                    tabControl1.IsEnabled = false;
                }
                else
                {
                    switch (selectedIndex)
                    {
                        case -1: break;
                        case 0:
                            {
                                DM_LOG_BASE.DataContext = null;
                                DM_LOG_WORK_ANING.DataContext = null;
                                DM_LOG_WORK_PERSONNEL.DataContext = null;
                                DM_LOG_UP_EQUIP.DataContext = null;

                                DataSet dataSet1 = dataser.GetData_小队施工基本信息(dataRowView["JOB_PLAN_CD"].ToString());

                                var model1 = Utility.ModelHandler<Model.DM_LOG_BASE>.FillModel(dataSet1.Tables["DM_LOG_BASE"]);
                                var model2 = Utility.ModelHandler<Model.DM_LOG_WORK_ANING>.FillModel(dataSet1.Tables["DM_LOG_WORK_ANING"]);
                                var model3 = Utility.ModelHandler<Model.DM_LOG_WORK_PERSONNEL>.FillModel(dataSet1.Tables["DM_LOG_WORK_PERSONNEL"]);
                                var model4 = Utility.ModelHandler<Model.DM_LOG_UP_EQUIP>.FillModel(dataSet1.Tables["DM_LOG_UP_EQUIP"]);
                                DM_LOG_BASE.DataContext = model1 == null ? new Model.DM_LOG_BASE() : model1[0];
                                DM_LOG_WORK_ANING.DataContext = model2 == null ? new Model.DM_LOG_WORK_ANING() : model2[0];
                                DM_LOG_WORK_PERSONNEL.DataContext = model3 == null ? new Model.DM_LOG_WORK_PERSONNEL() : model3[0];
                                DM_LOG_UP_EQUIP.DataContext = model4 == null ? new Model.DM_LOG_UP_EQUIP() : model4[0];
                                #region 初始值
                                var modelTemp = Utility.ModelHandler<Model.DM_LOG_OPS_PLAN>.FillModel(dataser.GetPlanData(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].Rows[0]);
                                if (string.IsNullOrWhiteSpace(job_purpose.Text))
                                {
                                    if (modelTemp.JOB_ID != null)
                                    {
                                        var rows = dataser.GetJobIDlist().Tables[0].Select("JOB_ID=" + (decimal)modelTemp.JOB_ID);
                                        if (rows.Length > 0)
                                            job_purpose.Text = rows[0].FieldEx<string>("JOB_CHINESE_NAME");
                                    }

                                }
                                if (received_inform_time.Value == null)
                                    received_inform_time.Value = modelTemp.RECEIVED_INFORM_TIME;
                                if (string.IsNullOrWhiteSpace(job_layer.Text))
                                    job_layer.Text = modelTemp.JOB_LAYER;
                                if (string.IsNullOrWhiteSpace(log_type.Text))
                                    log_type.Text = modelTemp.LOG_TYPE;
                                if (string.IsNullOrWhiteSpace(log_mode.Text))
                                    log_mode.Text = modelTemp.LOG_MODE;
                                if (requirements_time.Value == null)
                                    requirements_time.Value = modelTemp.REQUIREMENTS_TIME;
                                if (arrive_time.Value == null)
                                    arrive_time.Value = modelTemp.REQUIREMENTS_TIME;
                                if (Save1.IsEnabled)
                                {
                                    if (string.IsNullOrWhiteSpace(log_team_leader1.Text))
                                        log_team_leader1.Text = MyHomePage.ActiveUser.COL_NAME;
                                    if (string.IsNullOrWhiteSpace(log_operator__name1.Text))
                                        log_operator__name1.Text = MyHomePage.ActiveUser.COL_NAME;
                                }
                                if (string.IsNullOrWhiteSpace(team_org_id.Text))
                                    team_org_id.Text = modelTemp.LOG_TEAM_ID;
                                #endregion
                            }
                            break;
                        case 1:
                            {

                                //三交会
                                DM_LOG_THREE_CROSS.DataContext = null;
                                var model1 = Utility.ModelHandler<Model.DM_LOG_THREE_CROSS>.FillModel(dataser.GetData_三交会(dataRowView["JOB_PLAN_CD"].ToString()));
                                //ChangeSaveFileState(model1 != null && model1[0].ATTACH_FILE != null);
                                if (model1 != null)
                                {
                                    DM_LOG_THREE_CROSS.DataContext = model1[0];
                                    SaveFilesToLocal(CROSS_ATTACHINFO, CROSS_ATTACHFILE);
                                    GetFileListData(CROSS_FILELIST);
                                }
                                else
                                {
                                    DM_LOG_THREE_CROSS.DataContext = new Model.DM_LOG_THREE_CROSS();
                                    //DM_LOG_THREE_CROSS.DataContext = model1 == null ? new Model.DM_LOG_THREE_CROSS() : model1[0];
                                    MEETING_DATE1.Value = DateTime.Now;
                                    CROSS_FILELIST.ItemsSource = null;
                                    uploadTask = null;
                                    ClearTempFiles();
                                }
                            }
                            break;
                        case 2:
                            {
                                DM_LOG_CONTACT_WILL.DataContext = null;

                                var model1 = Utility.ModelHandler<Model.DM_LOG_CONTACT_WILL>.FillModel(dataser.GetData_多方联席会(dataRowView["JOB_PLAN_CD"].ToString()));
                                DM_LOG_CONTACT_WILL.DataContext = model1 == null ? new Model.DM_LOG_CONTACT_WILL() : model1[0];
                            }
                            break;
                        case 3:
                            {
                                //井场班前会
                                DM_LOG_CLASS_MEET.DataContext = null;
                                var model1 = Utility.ModelHandler<Model.DM_LOG_CLASS_MEET>.FillModel(dataser.GetData_井场班前会(dataRowView["JOB_PLAN_CD"].ToString()));
                                //DM_LOG_CLASS_MEET.DataContext = model1 == null ? new Model.DM_LOG_CLASS_MEET() : model1[0];
                                if (model1 != null)
                                {
                                    DM_LOG_CLASS_MEET.DataContext = model1[0];
                                    SaveFilesToLocal(MEET_ATTACHINFO, MEET_ATTACHFILE);
                                    GetFileListData(MEET_FILELIST);
                                }
                                else
                                {
                                    DM_LOG_CLASS_MEET.DataContext = new Model.DM_LOG_CLASS_MEET();
                                    MEETING_DATE2.Value = DateTime.Now;
                                    MEET_FILELIST.ItemsSource = null;
                                    uploadTask = null;
                                    ClearTempFiles();
                                }
                            }
                            break;
                        case 4:
                            {
                                dataGrid2.ItemsSource = dataser.GetDataList_项目明细(dataRowView["JOB_PLAN_CD"].ToString()).Tables[0].GetDefaultView();
                                log_series_name.ItemsSource = log_series.ItemsSource;
                                log_series_name.SelectedValuePath = "LOG_SERIES_NAME";
                                log_series_name.DisplayMemberPath = "LOG_SERIES_NAME";
                                DM_LOG_WORK_DETAILS.DataContext = null;
                                DM_LOG_WORK_HOLDUP_DETAILS.DataContext = null;
                                DM_LOG_RADIATION_STATUS.DataContext = null;
                                DM_LOG_DOWNHOLE_EQUIP.ItemsSource = null;
                                DM_LOG_ITEMS.ItemsSource = null;
                                uploadTask = null;
                                DM_LOG_ITEMS.ItemsSource = new Utility.DataCollection<Model.DM_LOG_ITEMS>();
                                DM_LOG_WORK_DETAILS.DataContext = new Model.DM_LOG_WORK_DETAILS();
                                DM_LOG_WORK_HOLDUP_DETAILS.DataContext = new Model.DM_LOG_WORK_HOLDUP_DETAILS();
                                DM_LOG_RADIATION_STATUS.DataContext = new Model.DM_LOG_RADIATION_STATUS();
                                DM_LOG_DOWNHOLE_EQUIP.ItemsSource = new Utility.DataCollection<Model.DM_LOG_DOWNHOLE_EQUIP>();
                                DM_LOG_ITEMS.ItemsSource = new Utility.DataCollection<Model.DM_LOG_ITEMS>();
                                addButton.IsChecked = true;
                                down_well_sequence.IsReadOnly = false;
                            }
                            break;
                        case 5:
                            {

                                DataTable dt = dataser.GetComboBoxList_下井趟次号(dataRowView["JOB_PLAN_CD"].ToString()).Tables[0];
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    comboBox1.DisplayMemberPath = "NAME";
                                    comboBox1.SelectedValuePath = "VALUE";
                                    comboBox1.ItemsSource = dt.GetDefaultView();
                                    comboBox1.SelectedIndex = 0;
                                }
                            }
                            break;
                        case 6:
                            {
                                //施工总结会
                                DM_LOG_JOB_SUMMAY.DataContext = null;
                                var model1 = Utility.ModelHandler<Model.DM_LOG_JOB_SUMMAY>.FillModel(dataser.GetData_施工总结会(dataRowView["JOB_PLAN_CD"].ToString()));
                                //DM_LOG_JOB_SUMMAY.DataContext = model1 == null ? new Model.DM_LOG_JOB_SUMMAY() : model1[0];
                                if (model1 != null)
                                {
                                    DM_LOG_JOB_SUMMAY.DataContext = model1[0];
                                    SaveFilesToLocal(SUMMARY_ATTACHINFO, SUMMARY_ATTACHFILE);
                                    GetFileListData(SUMMARY_FILELIST);
                                }
                                else
                                {
                                    DM_LOG_JOB_SUMMAY.DataContext = new Model.DM_LOG_JOB_SUMMAY();
                                    MEETING_DATE3.Value = DateTime.Now;
                                    SUMMARY_FILELIST.ItemsSource = null;
                                    uploadTask = null;
                                    ClearTempFiles();
                                }
                            }
                            break;
                    }
                    tabControl1.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// 弹出设备框，选择设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var models = DM_LOG_DOWNHOLE_EQUIP.ItemsSource as Utility.DataCollection<Model.DM_LOG_DOWNHOLE_EQUIP>;
            小队施工信息_设备列表_Window w = new 小队施工信息_设备列表_Window();
            w.Title = "小队施工信息_设备列表";
            if (models.Count > 0) w.DataContext = string.Join(",", models.Select(o => o.EQUIPID).ToArray());
            w.Owner = Application.Current.MainWindow;
            w.ChangeTextEvent += new ChangeTextHandler_设备列表(DM_LOG_DOWNHOLE_EQUIP_updata);
            w.ShowDialog();
        }
        /// <summary>
        /// 刷新DM_LOG_DOWNHOLE_EQUIP
        /// </summary>
        /// <param name="DaTa"></param>
        private void DM_LOG_DOWNHOLE_EQUIP_updata(DataRow[] dataRows)
        {
            var models = DM_LOG_DOWNHOLE_EQUIP.ItemsSource as Utility.DataCollection<Model.DM_LOG_DOWNHOLE_EQUIP>;
            var idChoises = dataRows.Select(o => o.FieldEx<string>("INSTRUMENT_ID")).ToArray();
            var resetModels = (from ms in models.RemovedData
                               where (idChoises).Contains(ms.EQUIPID)
                               select ms).ToList();
            for (int i = 0; i < resetModels.Count; ++i)
            {
                models.AddNew(resetModels[i]);
            }

            foreach (var row in from rows in dataRows.AsEnumerable()
                                where !(models.Select(o => o.EQUIPID).ToArray()).Contains(rows.FieldEx<string>("INSTRUMENT_ID"))
                                select rows)
            {
                models.AddNew(new Model.DM_LOG_DOWNHOLE_EQUIP()
                {
                    EQUIPID = row.FieldEx<string>("INSTRUMENT_ID"),
                    DEVICEACCOUNT_ID = row.FieldEx<string>("INSTRUMENT_ZBH"),
                    DEVICEACCOUNT_NAME = row.FieldEx<string>("INSTRUMENT_NAME"),
                    TEAM = row.FieldEx<string>("USE_TEAM"),
                    WORKING_STATE = "在用"
                });
            }
            var removeModels = (from ms in models
                                where !(idChoises).Contains(ms.EQUIPID)
                                select ms).ToList();
            for (int i = removeModels.Count - 1; i >= 0; --i)
            {
                models.Remove(removeModels[i]);
            }
        }
        /// <summary>
        /// 弹出预测项目，选择预测项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var items = DM_LOG_ITEMS.ItemsSource as Utility.DataCollection<Model.DM_LOG_ITEMS>;
            预测项目_Window w = new 预测项目_Window();
            w.Title = "测井项目";
            w.DataContext = items.Select(o => o.LOG_ITEM_ID).ToArray();
            w.Owner = Application.Current.MainWindow;
            w.ChangeTextEvent += new ChangeTextHandler_预测项目(DM_LOG_ITEMS_updata);
            w.ShowDialog();
        }
        /// <summary>
        /// 刷新DM_LOG_ITEMS
        /// </summary>
        /// <param name="DaTa"></param>
        private void DM_LOG_ITEMS_updata(List<Model.TreeModel> treelist)
        {
            var items = DM_LOG_ITEMS.ItemsSource as Utility.DataCollection<Model.DM_LOG_ITEMS>;

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
                    items.AddNew(new Model.DM_LOG_ITEMS() { LOG_ITEM_ID = tree.ID, LOGGING_NAME = tree.Name });
                }
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            小队施工信息_放射源_Window w = new 小队施工信息_放射源_Window();
            w.Title = "小队施工信息_放射源";
            w.Owner = Application.Current.MainWindow;
            w.ChangeTextEvent += new ChangeTextHandler_放射源(DM_LOG_RADIATION_STATUS_updata);
            w.ShowDialog();
        }
        private void DM_LOG_RADIATION_STATUS_updata(DataRow dataRow)
        {
            var model = DM_LOG_RADIATION_STATUS.DataContext as Model.DM_LOG_RADIATION_STATUS;
            model.RADIATION_CD = dataRow.FieldEx<string>("RADIATION_ID");
            model.RADIATION_NO = dataRow.FieldEx<string>("RADIATION_NO"); ;
            model.ELEMENT = dataRow.FieldEx<string>("ELEMENT"); ;
            model.ACTIVE = dataRow.FieldEx<string>("ACTIVE");
        }



        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid1.SelectedItem)) return;
            ChangeMenuState();
            ResetData();
        }

        private void ChangeMenuState()
        {
            ///menu1.IsEnabled = false;
            ///menu2.IsEnabled = false;
            ///menu3.IsEnabled = false;
            //Save1.IsEnabled = false;
            //Save2.IsEnabled = false;
            //Save3.IsEnabled = false;
            //Save4.IsEnabled = false;
            //Save5.IsEnabled = false;
            //Save6.IsEnabled = false;
            //Save7.IsEnabled = false;
            var row = dataGrid1.SelectedItem as DataRowView;
            //using (DataServiceHelper dataser = new DataServiceHelper())
            //{
            //    using(var userser=new UserServiceHelper())
            //    {
            //    var role = userser.GetActiveUserRoles();
            //    var flowData1 = Utility.ModelHandler<Model.SYS_WORK_FLOW>.FillModel(dataser.GetWorkFlow(row.Row.FieldEx<string>("JOB_PLAN_CD"), (int)Model.SYS_Enums.WorkFlowType.计划任务书));
            //    if (flowData1 == null) return;
            workFlowControl1.LoadData(row.Row.FieldEx<string>("JOB_PLAN_CD"));
            //var flowData2 = workFlowControl1.GetData();
            //if (flowData1[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.审核通过)
            //{
            //    if (flowData1[0].TARGET_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME && (flowData2 == null || flowData2[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.审核未通过))
            //    {
            //        Save1.IsEnabled = true;
            //        Save2.IsEnabled = true;
            //        Save3.IsEnabled = true;
            //        Save4.IsEnabled = true;
            //        Save5.IsEnabled = true;
            //        Save6.IsEnabled = true;
            //        Save7.IsEnabled = true;
            //    }
            //}
            //    }
            //}
        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0 && e.AddedItems.Count > 0 && e.AddedItems[0].Equals(tabControl1.SelectedItem))
                ResetData();
        }


        /// <summary>
        /// 保存基本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveBaseInfo_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid1.SelectedItem as DataRowView;
            Model.DM_LOG_BASE model_小队施工基本信息 = DM_LOG_BASE.DataContext as Model.DM_LOG_BASE; ;
            Model.DM_LOG_WORK_ANING model_小队施工时效 = DM_LOG_WORK_ANING.DataContext as Model.DM_LOG_WORK_ANING;
            Model.DM_LOG_WORK_PERSONNEL model_小队施工人员 = DM_LOG_WORK_PERSONNEL.DataContext as Model.DM_LOG_WORK_PERSONNEL;
            Model.DM_LOG_UP_EQUIP model_小队施工地面设备 = DM_LOG_UP_EQUIP.DataContext as Model.DM_LOG_UP_EQUIP;

            if (model_小队施工基本信息.HasError() || model_小队施工时效.HasError() || model_小队施工人员.HasError() || model_小队施工地面设备.HasError())
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            if (!(model_小队施工时效.ARRIVE_TIME < model_小队施工时效.RECEIVING_TIME && model_小队施工时效.RECEIVING_TIME < model_小队施工时效.HAND_TIME && model_小队施工时效.HAND_TIME < model_小队施工时效.LEAVE_TIME))
            {
                MessageBox.Show(App.Current.MainWindow, "实际到井时间<实际接井时间<交井时间<离开井场时间!!");
                return;
            }
            if (!(model_小队施工时效.ARRIVE_TIME <= model_小队施工时效.LOG_START_TIME && model_小队施工时效.LOG_START_TIME < model_小队施工时效.LOG_END_TIME && model_小队施工时效.LOG_END_TIME <= model_小队施工时效.HAND_TIME))
            {
                MessageBox.Show(App.Current.MainWindow, "实际接井时间<=测井开始时间<测井结束时间<=测井交井时间!!");
                return;
            }

            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                model_小队施工基本信息.JOB_PLAN_CD = dataRowView["JOB_PLAN_CD"].ToString();
                model_小队施工时效.REQUISITION_CD = dataRowView["REQUISITION_CD"].ToString();
                if (dataser.Save_小队施工基本信息(
                    Utility.ModelHelper.SerializeObject(model_小队施工基本信息),
                    Utility.ModelHelper.SerializeObject(model_小队施工时效),
                    Utility.ModelHelper.SerializeObject(model_小队施工人员),
                    Utility.ModelHelper.SerializeObject(model_小队施工地面设备)
                    ))
                {
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                    ResetData();
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }

        private void save多方联席会_Click(object sender, RoutedEventArgs e)
        {
            var model = DM_LOG_CONTACT_WILL.DataContext as Model.DM_LOG_CONTACT_WILL;
            DataRowView dataRowView = dataGrid1.SelectedItem as DataRowView;
            model.JOB_PLAN_CD = dataRowView["JOB_PLAN_CD"].ToString();
            if (model.HasError())
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Save_多方联席会(Utility.ModelHelper.SerializeObject(model)))
                {
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                    ResetData();
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }

        private void addButton_Checked(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null) ResetData();
        }

        private void save作业明细_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid1.SelectedItem as DataRowView;
            var model1 = DM_LOG_WORK_DETAILS.DataContext as Model.DM_LOG_WORK_DETAILS;
            model1.JOB_PLAN_CD = dataRowView["JOB_PLAN_CD"].ToString();
            var model2 = DM_LOG_WORK_HOLDUP_DETAILS.DataContext as Model.DM_LOG_WORK_HOLDUP_DETAILS;
            var model3 = DM_LOG_RADIATION_STATUS.DataContext as Model.DM_LOG_RADIATION_STATUS;
            var model4 = DM_LOG_ITEMS.ItemsSource as Utility.DataCollection<Model.DM_LOG_ITEMS>;
            var model5 = DM_LOG_DOWNHOLE_EQUIP.ItemsSource as Utility.DataCollection<Model.DM_LOG_DOWNHOLE_EQUIP>;
            if (model1.HasError() || model2.HasError() || model3.HasError() || model4.HasError() || model5.HasError())
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            if (model4==null || model4.Count < 1)
            {
                MessageBox.Show(App.Current.MainWindow, "测井项目不能为空！");
                return;
            }

            using (DataServiceHelper dataser = new DataServiceHelper())
            {

                if (dataser.Save_作业明细(
                     Utility.ModelHelper.SerializeObject(model1),
                     Utility.ModelHelper.SerializeObject(model2),
                     Utility.ModelHelper.SerializeObject(model3),
                     Utility.ModelHelper.SerializeObject(model4.ChangedData),
                     Utility.ModelHelper.SerializeObject(model4.RemovedData.FindAll(o => o.LOG_ITEM_ID != null)),
                     Utility.ModelHelper.SerializeObject(model5.ChangedData),
                     Utility.ModelHelper.SerializeObject(model5.RemovedData.FindAll(o => o.EQUIPID != null))
                     ))
                {
                    if (uploadTask != null && uploadTask.Count > 0)
                    {
                        new FileUpload().BeginUpload(uploadTask);
                        var taskList = uploadTask.ToList();
                        if (taskList.Find(t => t.hasError) == null)
                        {
                            MessageBox.Show(App.Current.MainWindow, "文件上传成功！");
                            //return;
                        }
                    }
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                    ResetData();
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        Utility.DataCollection<UploadController.UploadTask> uploadTask = null;
        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid2.SelectedItem)) return;
            editButton.IsChecked = true;
            DataRowView dataRowView = dataGrid2.SelectedItem as DataRowView;
            down_well_sequence.IsReadOnly = true;
            uploadTask = null;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                DataSet ds = dataser.GetData_项目明细(dataRowView["JOB_PLAN_CD"].ToString(), dataRowView["DOWN_WELL_SEQUENCE"].ToString());
                if (ds != null && ds.Tables.Count == 5)
                {
                    var model1 = Utility.ModelHandler<Model.DM_LOG_WORK_DETAILS>.FillModel(ds.Tables["DM_LOG_WORK_DETAILS"]);
                    var model2 = Utility.ModelHandler<Model.DM_LOG_WORK_HOLDUP_DETAILS>.FillModel(ds.Tables["DM_LOG_WORK_HOLDUP_DETAILS"]);
                    var model3 = Utility.ModelHandler<Model.DM_LOG_RADIATION_STATUS>.FillModel(ds.Tables["DM_LOG_RADIATION_STATUS"]);
                    var model4 = Utility.ModelHandler<Model.DM_LOG_ITEMS>.FillModel(ds.Tables["DM_LOG_ITEMS"]);
                    var model5 = Utility.ModelHandler<Model.DM_LOG_DOWNHOLE_EQUIP>.FillModel(ds.Tables["DM_LOG_DOWNHOLE_EQUIP"]);

                    DM_LOG_WORK_DETAILS.DataContext = model1 == null ? new Model.DM_LOG_WORK_DETAILS() : model1[0];
                    DM_LOG_WORK_HOLDUP_DETAILS.DataContext = model2 == null ? new Model.DM_LOG_WORK_HOLDUP_DETAILS() : model2[0];
                    DM_LOG_RADIATION_STATUS.DataContext = model3 == null ? new Model.DM_LOG_RADIATION_STATUS() : model3[0];
                    DM_LOG_ITEMS.ItemsSource = model4 == null ? new Utility.DataCollection<Model.DM_LOG_ITEMS>() : model4;
                    DM_LOG_DOWNHOLE_EQUIP.ItemsSource = model5 == null ? new Utility.DataCollection<Model.DM_LOG_DOWNHOLE_EQUIP>() : model5;
                }

            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0].Equals(comboBox1.SelectedItem))
            {
                DataRowView drv1 = dataGrid1.SelectedItem as DataRowView;
                DataRowView drv2 = comboBox1.SelectedItem as DataRowView;

                PRO_LOG_REMOTE_DIRECT.DataContext = null;
                PRO_LOG_PUNISH.DataContext = null;

                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    DataSet ds = dataser.GetData_专家指导(drv1["JOB_PLAN_CD"].ToString(), drv2["VALUE"].ToString());
                    var model1 = Utility.ModelHandler<Model.PRO_LOG_REMOTE_DIRECT>.FillModel(ds.Tables["PRO_LOG_REMOTE_DIRECT"]);
                    var model2 = Utility.ModelHandler<Model.PRO_LOG_PUNISH>.FillModel(ds.Tables["PRO_LOG_PUNISH"]);
                    PRO_LOG_REMOTE_DIRECT.DataContext = model1 == null ? new Model.PRO_LOG_REMOTE_DIRECT() : model1[0];
                    PRO_LOG_PUNISH.DataContext = model2 == null ? new Model.PRO_LOG_PUNISH() : model2[0];
                }
            }
        }

        private void save专家指导_Click(object sender, RoutedEventArgs e)
        {
            var model1 = PRO_LOG_REMOTE_DIRECT.DataContext as Model.PRO_LOG_REMOTE_DIRECT;
            var model2 = PRO_LOG_PUNISH.DataContext as Model.PRO_LOG_PUNISH;
            if (model1.DOWN_WELL_SEQUENCE == null)
            {
                DataRowView drv1 = dataGrid1.SelectedItem as DataRowView;
                DataRowView drv2 = comboBox1.SelectedItem as DataRowView;

                model1.JOB_PLAN_CD = drv1["JOB_PLAN_CD"].ToString();
                model1.DOWN_WELL_SEQUENCE = drv2.Row.FieldEx<decimal>("VALUE");
            }
            if (model1.HasError() || model2.HasError())
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Save_专家指导(
                     Utility.ModelHelper.SerializeObject(model1),
                     Utility.ModelHelper.SerializeObject(model2)))
                {
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                    ResetData();
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            SearchPanel.DataContext = new Model.LogPlanSearch();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("是", "1");
            dic.Add("否", "0");
            is_add.DisplayMemberPath = "Key";
            is_add.SelectedValuePath = "Value";
            is_add.ItemsSource = dic;

            if_success.DisplayMemberPath = "Key";
            if_success.SelectedValuePath = "Value";
            if_success.ItemsSource = dic;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dataGrid1.SelectedIndex = -1;
            tabControl1.SelectedIndex = 0;
            workFlowControl1.ClearData();
            ResetData();
            LoadDataList(1);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            dataGrid1.SelectedIndex = -1;
            addButton.IsChecked = true;
            using (var dataser = new DataServiceHelper())
            {
                DataView queryDate = dataser.GetComboxList_QueryDate().Tables[0].GetDefaultView();
                QUERY_DATE.DisplayMemberPath = "QUREY_DATE";
                QUERY_DATE.SelectedValuePath = "DATE_VALUE";
                QUERY_DATE.ItemsSource = queryDate;
                QUERY_DATE.Text = "最近一年";
            }
            //workFlowControl1.ClearData();
            ResetData();
            LoadDataList(1);
        }

        //private bool SetWorkFlow(string target_loginname, int state)
        //{
        //    using (var dataser = new DataServiceHelper())
        //    {
        //        var row = dataGrid1.SelectedItem as DataRowView;
        //        if (dataser.SetWorkFlow(row.Row.FieldEx<string>("JOB_PLAN_CD"), target_loginname, DataService.WorkFlowType.小队施工信息, state))
        //        {
        //            ChangeMenuState();
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private void MenuItem1_Click(object sender, RoutedEventArgs e)
        //{
        //    var w = new SelectUserWindow1();
        //    w.UserRole = UserService.UserRole.解释管理员;
        //    w.Owner = Application.Current.MainWindow;
        //    w.ShowDialog();
        //    if (!string.IsNullOrEmpty(w.LoginName))
        //    {
        //        if (SetWorkFlow(w.LoginName, 1)) MessageBox.Show("提交审核成功！");
        //    }
        //}

        //private void MenuItem2_Click(object sender, RoutedEventArgs e)
        //{
        //    var result = MessageBox.Show("是否通过审核？", "小队施工信息审核", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        //    int state = -1;
        //    if (result == MessageBoxResult.Yes) state = 2;
        //    if (result == MessageBoxResult.No) state = 0;
        //    if (state > -1 && SetWorkFlow(null, state))
        //        MessageBox.Show("审核成功！");
        //}

        //private void MenuItem3_Click(object sender, RoutedEventArgs e)
        //{
        //    var window = new SelectUserWindow();
        //    window.Owner = Application.Current.MainWindow;
        //    window.ShowDialog();
        //    using (var dataser = new DataServiceHelper())
        //    {
        //        var row = dataGrid1.SelectedItem as DataRowView;
        //        if (window.UserModel != null && dataser.SetWorkFlow(row.Row.FieldEx<string>("JOB_PLAN_CD"), window.UserModel.COL_LOGINNAME, DataService.WorkFlowType.解释处理作业, 0))
        //            MessageBox.Show("指派成功！");
        //    }
        //}


        private bool VilidataSave(int size)
        {
            int limit_size = 1024 * 1024 * 20;
            if (size > limit_size)
            {
                MessageBox.Show("文件总大小超过20M限制,保存失败!!", "提示");
                return false;
            }
            return true;
        }
        private void Save_三交会_Click(object sender, RoutedEventArgs e)
        {
            GetBlobData(CROSS_ATTACHINFO, CROSS_ATTACHFILE);
            var model = DM_LOG_THREE_CROSS.DataContext as Model.DM_LOG_THREE_CROSS;
            if (!VilidataSave(model.ATTACH_FILE.Length)) return;
            DataRowView dataRowView = dataGrid1.SelectedItem as DataRowView;
            model.JOB_PLAN_CD = dataRowView["JOB_PLAN_CD"].ToString();
            if (model.HasError())
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }

            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Save_三交会(Utility.ModelHelper.SerializeObject(model)))
                {
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                    ResetData();
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        private void Save_井场班前会_Click(object sender, RoutedEventArgs e)
        {
            GetBlobData(MEET_ATTACHINFO, MEET_ATTACHFILE);
            var model = DM_LOG_CLASS_MEET.DataContext as Model.DM_LOG_CLASS_MEET;
            if (!VilidataSave(model.ATTACH_FILE.Length)) return;
            DataRowView dataRowView = dataGrid1.SelectedItem as DataRowView;
            model.JOB_PLAN_CD = dataRowView["JOB_PLAN_CD"].ToString();
            if (model.HasError())
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Save_井场班前会(Utility.ModelHelper.SerializeObject(model)))
                {
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                    ResetData();
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }

        private void Save_施工总结会_Click(object sender, RoutedEventArgs e)
        {
            GetBlobData(SUMMARY_ATTACHINFO, SUMMARY_ATTACHFILE);
            var model = DM_LOG_JOB_SUMMAY.DataContext as Model.DM_LOG_JOB_SUMMAY;
            if (!VilidataSave(model.ATTACH_FILE.Length)) return;
            DataRowView dataRowView = dataGrid1.SelectedItem as DataRowView;
            model.JOB_PLAN_CD = dataRowView["JOB_PLAN_CD"].ToString();
            if (model.HasError())
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Save_施工总结会(Utility.ModelHelper.SerializeObject(model)))
                {
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                    ResetData();
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //DataTable datatable = null;
            System.Windows.Forms.FolderBrowserDialog m_DirDlg = new System.Windows.Forms.FolderBrowserDialog();
            if (m_DirDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName.Clear();
                uploadTask = new Utility.DataCollection<UploadController.UploadTask>();
                //DataTable dt = new DataTable();
                //dt.Columns.Add("DATA_NAME");
                //datatable = 
                GetPathInfo(m_DirDlg.SelectedPath, uploadTask);
                //for (int i = 0; i < datatable.Rows.Count; i++)
                //{
                //    FileName.Text += datatable.Rows[i]["DATA_NAME"].ToString() + "\n";
                //}
                foreach (var task in uploadTask)
                {
                    FileName.AppendText(task.Name + "\n");
                }
                FileName.Text.Trim('\n');
            }

        }

        public void GetPathInfo(string path, Utility.DataCollection<UploadController.UploadTask> tasks)
        {
            string[] fileNames = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);
            foreach (string file in fileNames)
            {
                //System.IO.FileInfo fi = new System.IO.FileInfo(file);
                //DataRow dr = dt.NewRow();
                //dr["DATA_NAME"] = fi.Name;
                //dt.Rows.Add(dr);
                tasks.Add(new UploadController.UploadTask { FullName = file });
            }
            foreach (string dir in directories) GetPathInfo(dir, tasks);

            //return dt;
        }

        private void FileName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var w = new FileListWindow();
            w.Owner = App.Current.MainWindow;
            //w.textBox = FileName;
            w.DataContext = uploadTask;
            w.control = FileName;
            w.Show();
        }

        private ComboBox _combox = null;
        private string save_path = AppDomain.CurrentDomain.BaseDirectory + "temp\\~~teamfile\\";
        private void Combox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _combox = sender as ComboBox;
            var w = new FileListWindow();
            w.Owner = App.Current.MainWindow;
            w.DataContext = uploadTask;
            w.control = _combox;
            w._team_deleget += new FileListWindow.TeamDeleget(FileListWindowClosed);
            w.Show();
        }

        public void GetPathInfo_1(string[] fileNames, Utility.DataCollection<UploadController.UploadTask> tasks)
        {
            tasks.Clear();
            foreach (string file in fileNames)
            {
                tasks.Add(new UploadController.UploadTask { FullName = file });
            }
        }

        private void ClearTempFiles()
        {
            if (!Directory.Exists(save_path))
            {
                Directory.CreateDirectory(save_path);
            }
            else
            {
                foreach (var item in Directory.EnumerateFileSystemEntries(save_path))
                {
                    File.Delete(item);
                }
            }
        }

        private void SaveFilesToLocal(Label attach_info, Label attach_file)
        {
            ClearTempFiles();
            if (attach_info.Content != null)
            {
                string fileinfo = attach_info.Content.ToString();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                List<AttachInfo> list = jss.Deserialize<List<AttachInfo>>(fileinfo);
                byte[] buffer = (byte[])attach_file.Content;
                foreach (var file in list)
                {
                    FileStream fs = new FileStream(save_path + file.F_NAME, FileMode.Create, FileAccess.Write);
                    fs.Write(buffer, file.F_START, file.F_SIZE);
                    fs.Close();
                }
            }
        }

        private void GetBlobData(Label attach_info, Label attach_file)
        {
            DirectoryInfo folder = new DirectoryInfo(save_path);
            FileInfo[] files = folder.GetFiles();

            int start = 0;
            long bufferLength = 0;
            for (int i = 0; i < files.Length; i++)
            {
                bufferLength += files[i].Length;
            }
            byte[] buffer = new byte[bufferLength];

            List<AttachInfo> list = new List<AttachInfo>();

            for (int i = 0; i < files.Length; i++)
            {
                FileStream fs = File.OpenRead(files[i].FullName);
                byte[] temp = new byte[fs.Length];
                int remaining = temp.Length;
                int offset = 0;
                while (remaining > 0)
                {
                    int read = fs.Read(temp, offset, temp.Length);
                    remaining -= read;
                    offset += read;
                }
                fs.Close();

                AttachInfo info = new AttachInfo();
                info.F_NAME = files[i].Name;
                info.F_START = start;
                info.F_SIZE = temp.Length;
                list.Add(info);

                temp.CopyTo(buffer, start);
                start += temp.Length;
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string json = jss.Serialize(list);
            attach_info.Content = json;
            attach_file.Content = buffer;
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.RestoreDirectory = true;
            op.Multiselect = true;
            op.Filter = "纯文本文档|*.txt|word文档|*.doc;*.docx|excel文档|*.xls;*.xlsx|所有|*.*";

            if (op.ShowDialog() == true)
            {
                for (int i = 0; i < op.SafeFileNames.Length; i++)
                {
                    File.Copy(op.FileNames[i], save_path + op.SafeFileNames[i], true);
                }
                ComboBox _current = GetFileListCombo();
                GetFileListData(_current);
            }
        }
        /*
        private void ChangeSaveFileState(Hyperlink download,bool isExits = false)
        {
            if (isExits)
            {
                download.IsEnabled = true;
            }
            else
            {
                download.IsEnabled = false;
            }
        }
         * */

        private void GetFileListData(ComboBox filelist)
        {
            DirectoryInfo folder = new DirectoryInfo(save_path);
            FileInfo[] files = folder.GetFiles();
            filelist.ItemsSource = files;
            filelist.SelectedValuePath = "Name";
            filelist.DisplayMemberPath = "Name";
            filelist.SelectedIndex = 0;
            GetListItemData();
        }

        private void GetListItemData()
        {
            if (uploadTask == null)
            {
                uploadTask = new Utility.DataCollection<UploadController.UploadTask>();
            }

            string[] paths = Directory.EnumerateFileSystemEntries(save_path).ToArray();
            GetPathInfo_1(paths, uploadTask);
        }

        private void FileListWindowClosed(List<string> list)
        {
            foreach (var f in list)
            {
                File.Delete(f);
            }
            GetFileListData(_combox);
        }

        private ComboBox GetFileListCombo()
        {
            ComboBox _current = null;
            int index = tabControl1.SelectedIndex;
            switch (index)
            {
                case 1:
                    _current = CROSS_FILELIST;
                    break;
                case 3:
                    _current = MEET_FILELIST;
                    break;
                case 6:
                    _current = SUMMARY_FILELIST;
                    break;
            }
            return _current;
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            ComboBox _current = GetFileListCombo();
            if (_current.SelectedIndex == -1)
            {
                MessageBox.Show("选择文件");
                return;
            }

            SaveFileDialog sf = new SaveFileDialog();
            sf.RestoreDirectory = true;
            sf.Filter = "纯文本文档|*.txt|word文档|*.doc;*.docx|excel文档|*.xls;*.xlsx|所有|*.*";
            sf.FileName = _current.SelectedValue.ToString();

            if (sf.FileName.Contains(".txt"))
            {
                sf.FilterIndex = 1;
            }
            else if (sf.FileName.Contains(".doc") || sf.FileName.Contains(".docx"))
            {
                sf.FilterIndex = 2;
            }
            else if (sf.FileName.Contains(".xls") || sf.FileName.Contains(".xlsx"))
            {
                sf.FilterIndex = 3;
            }
            else
            {
                sf.FilterIndex = 4;
            }

            if (sf.ShowDialog() == true)
            {
                File.Copy(save_path + sf.SafeFileName, sf.FileName, true);
                MessageBox.Show("保存成功！");
            }
        }

        private void LoadDataList(int page)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                int total = 0;
                dataGrid1.ItemsSource = dataser.GetPlanList(Utility.ModelHelper.SerializeObject(SearchPanel.DataContext), page, out total).Tables[0].GetDefaultView();
                combination_name.ItemsSource = dataser.GetComboBoxList_COMBINATION_NAME().Tables[0].GetDefaultView();
                dataPager1.TotalCount = total;
            }
        }

        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            int page = (int)e.CurrentPageIndex;
            LoadDataList(page);
        }

        private void log_time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (log_start_time.Value == null || log_end_time.Value == null)
            {
                return;
            }
            var ts1 = new TimeSpan(Convert.ToDateTime(log_start_time.Value).Ticks);
            var ts2 = new TimeSpan(Convert.ToDateTime(log_end_time.Value).Ticks);
            var ts = ts2.Subtract(ts1);
            log_total_time.Text = Math.Round(ts.TotalHours, 1).ToString();
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            小队施工信息_车辆_Window vehicle = new 小队施工信息_车辆_Window();
            vehicle.Owner = App.Current.MainWindow;
            vehicle.ShowDialog();
            if(!string.IsNullOrEmpty(vehicle.Vehicle_Plate)){
                var tb = sender as TextBox;
                tb.Text = vehicle.Vehicle_Plate;
            }
        }
    }
}
