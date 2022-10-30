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
using System.IO;
using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// 测井现场提交信息_Page.xaml 的交互逻辑
    /// </summary>
    public partial class 测井现场提交信息_Page
    {
        string Path_解释成果;
        string Path_原始数据;

        public 测井现场提交信息_Page()
        {
            InitializeComponent();
        }

        private void But_现场快速解释成果表_Click(object sender, RoutedEventArgs e)
        {
            var m_DirDlg = new System.Windows.Forms.FolderBrowserDialog();
            Path_解释成果 = m_DirDlg.SelectedPath;
            if (m_DirDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var models = new Utility.DataCollection<Model.PRO_LOG_RAPID_RESULTS>();
                GetPathInfo_成果(m_DirDlg.SelectedPath, models);
                PRO_LOG_RAPID_RESULTS.ItemsSource = models;
            }
        }

        public void GetPathInfo_成果(string path, Utility.DataCollection<Model.PRO_LOG_RAPID_RESULTS> models)
        {
            string[] fileNames = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);
            foreach (string file in fileNames)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                var model = new Model.PRO_LOG_RAPID_RESULTS();
                model.FILENAME = fi.Name;
                model.FullName = fi.FullName;
                if (fi.Name.IndexOf("Layer.List") > 0)
                {
                    model.RAPID_RESULTS_TYPE = "分层数据";
                }
                else if (fi.Name.IndexOf("Result.List") > 0)
                {
                    model.RAPID_RESULTS_TYPE = "解释成果数据";
                }
                else if (fi.Name.Contains("井斜"))
                {
                    model.RAPID_RESULTS_TYPE = "井斜数据";
                }
                else if (fi.Name.IndexOf("CBL3-S-L.List") > 0)
                {
                    model.RAPID_RESULTS_TYPE = "固井质量解释";
                }
                else if (fi.Name.IndexOf(".bmp") > 0 || fi.Name.IndexOf(".jpg") > 0 || fi.Name.IndexOf(".tiff") > 0 || fi.Name.IndexOf(".gif") > 0 || fi.Name.IndexOf(".pcx") > 0 || fi.Name.IndexOf(".tga") > 0
                    || fi.Name.IndexOf(".exif") > 0 || fi.Name.IndexOf(".fpx") > 0 || fi.Name.IndexOf(".svg") > 0 || fi.Name.IndexOf(".psd") > 0 || fi.Name.IndexOf(".cdr") > 0 || fi.Name.IndexOf(".pcd") > 0
                    || fi.Name.IndexOf(".dxf") > 0 || fi.Name.IndexOf(".ufo") > 0 || fi.Name.IndexOf(".eps") > 0 || fi.Name.IndexOf(".ai") > 0 || fi.Name.IndexOf(".raw") > 0)
                {
                    model.RAPID_RESULTS_TYPE = "图片";
                }
                else
                {
                    model.RAPID_RESULTS_TYPE = "测井数据";
                }
                model.DATA_SIZE = fi.Length;
                models.Add(model);
            }
            foreach (string dir in directories) GetPathInfo_成果(dir, models);

            // return models;
        }

        private void But_选择原始数据目录_Click(object sender, RoutedEventArgs e)
        {
            var m_DirDlg = new System.Windows.Forms.FolderBrowserDialog();
            Path_原始数据 = m_DirDlg.SelectedPath;
            if (m_DirDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var models = new Utility.DataCollection<Model.PRO_LOG_RAPID_ORIGINAL_DATA>();
                GetPathInfo_原始(m_DirDlg.SelectedPath, models);
                PRO_LOG_RAPID_ORIGINAL_DATA.ItemsSource = models;
            }
        }

        public void GetPathInfo_原始(string path, Utility.DataCollection<Model.PRO_LOG_RAPID_ORIGINAL_DATA> models)
        {
            string[] fileNames = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);
            foreach (string file in fileNames)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                var model = new Model.PRO_LOG_RAPID_ORIGINAL_DATA();
                model.DATA_NAME = fi.Name;
                model.DATA_SIZE = fi.Length;
                model.FullName = fi.FullName;
                models.Add(model);
            }
            foreach (string dir in directories) GetPathInfo_原始(dir, models);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //string Paths="";
            ////检查是否存在目的目录
            //if (!Directory.Exists("D:\\DATA"))
            //    Directory.CreateDirectory("D:\\DATA");
            ////File.Copy(源文件地址,目标地址, true（为true是覆盖同名文件）); 
            //if (Path_解释成果 != null)
            //{
            //    for (int i = 0; i < dataGrid1.Items.Count; i++)
            //    {
            //        DataRowView selectItem = dataGrid1.Items[i] as DataRowView;
            //        if (selectItem.Row["RAPID_RESULTS_TYPE"].ToString() != "")
            //        {
            //            Paths=Path_解释成果+selectItem.Row["RAPID_RESULTS_TYPE"].ToString();
            //            File.Copy(Paths, selectItem.Row["RAPID_RESULTS_TYPE"].ToString());
            //        }
            //    }
            //    //File.Copy(
            //    Logging_App.Model.ZipClass zip = new Model.ZipClass();
            //    zip.ZipFileFromDirectory("D:\\DATA", "C:\\", 6);
            //}


            //this.Close();
            //初始化时，作业小队到钻井液温度直接从前面读过来。
        }

        public string JOB_PLAN_CD = string.Empty;
        private void LoadDataList(int page)
        {
            dataViewPanel.IsEnabled = false;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                int total = 0;
                dataGrid.ItemsSource = dataser.GetPlanList(Utility.ModelHelper.SerializeObject(SearchPanel.DataContext), page, out total).Tables[0].GetDefaultView();
                dataPager1.TotalCount = total;
                job_id1.DisplayMemberPath = "JOB_CHINESE_NAME";
                job_id1.SelectedValuePath = "JOB_ID";
                job_id1.ItemsSource = dataser.GetJobIDlist().Tables[0].GetDefaultView();
                if (!string.IsNullOrEmpty(JOB_PLAN_CD))
                {
                    var drv = (from DataRowView item in dataGrid.Items
                               where item.Row.FieldEx<string>("JOB_PLAN_CD") == JOB_PLAN_CD
                               select item).FirstOrDefault();
                    if (drv != null)
                    {
                        dataGrid.ScrollIntoView(drv);
                        dataGrid.SelectedItem = drv;
                    }
                    JOB_PLAN_CD = string.Empty;
                };
            }
        }

        private void LoadData()
        {
            var row = dataGrid.SelectedItem as DataRowView;
            if (row == null) return;
            var model = Utility.ModelHandler<Model.DM_LOG_OPS_PLAN>.FillModel(row.Row);
            if (model == null) return;
            dataViewPanel.IsEnabled = true;
            using (var dataser = new DataServiceHelper())
            {
                var ds = dataser.GetData_测井现场提交信息(model.JOB_PLAN_CD);
                if (ds != null && ds.Tables.Count == 3)
                {
                    var model1 = Utility.ModelHandler<Model.PRO_LOG_RAPID_INFO>.FillModel(ds.Tables["PRO_LOG_RAPID_INFO"]);
                    var model2 = Utility.ModelHandler<Model.PRO_LOG_RAPID_RESULTS>.FillModel(ds.Tables["PRO_LOG_RAPID_RESULTS"]);
                    var model3 = Utility.ModelHandler<Model.PRO_LOG_RAPID_ORIGINAL_DATA>.FillModel(ds.Tables["PRO_LOG_RAPID_ORIGINAL_DATA"]);
                    PRO_LOG_RAPID_INFO.DataContext = model1 == null ? new Model.PRO_LOG_RAPID_INFO() : model1[0];
                    PRO_LOG_RAPID_RESULTS.ItemsSource = model2 == null ? null : model2;
                    PRO_LOG_RAPID_ORIGINAL_DATA1.DataContext = model3 == null ? new Model.PRO_LOG_RAPID_ORIGINAL_DATA() : model3[0];
                    PRO_LOG_RAPID_ORIGINAL_DATA.ItemsSource = model3 == null ? null : model3;
                    ChangeMenuState();
                    #region 初始值
                    var modelTemp = Utility.ModelHandler<Model.DM_LOG_OPS_PLAN>.FillModel(dataser.GetPlanData(model.JOB_PLAN_CD).Tables[0].Rows[0]);
                    DataSet dataSet1 = dataser.GetData_小队施工基本信息(model.JOB_PLAN_CD);
                    var modelTemp1 = Utility.ModelHandler<Model.DM_LOG_BASE>.FillModel(dataSet1.Tables["DM_LOG_BASE"]);
                    var modelTemp2 = Utility.ModelHandler<Model.DM_LOG_UP_EQUIP>.FillModel(dataSet1.Tables["DM_LOG_UP_EQUIP"]);

                    if (string.IsNullOrWhiteSpace(log_team.Text))
                        log_team.Text = modelTemp.LOG_TEAM_ID;
                    if (string.IsNullOrWhiteSpace(log_mode.Text) && modelTemp1 != null)
                        log_mode.Text = modelTemp1[0].LOG_MODE;
                    if (string.IsNullOrWhiteSpace(log_server_id.Text) && modelTemp2 != null)
                        log_server_id.Text = modelTemp2[0].LOG_SERIES_ID;
                    if (string.IsNullOrWhiteSpace(log_mode.Text))
                        log_mode.Text = modelTemp.LOG_MODE;
                    if (Save1.IsEnabled)
                    {
                        if (string.IsNullOrWhiteSpace(submit_man.Text))
                            submit_man.Text = MyHomePage.ActiveUser.COL_NAME;
                        if (submit_date.Value == null)
                            submit_date.Value = DateTime.Now;
                    }
                    #endregion
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            workFlowControl1.ClearData();
            using (var dataser = new DataServiceHelper())
            {
                DataView queryDate = dataser.GetComboxList_QueryDate().Tables[0].GetDefaultView();
                QUERY_DATE.DisplayMemberPath = "QUREY_DATE";
                QUERY_DATE.SelectedValuePath = "DATE_VALUE";
                QUERY_DATE.ItemsSource = queryDate;
                QUERY_DATE.Text = "最近一年";
            }
            LoadDataList(1);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid.SelectedItem)) return;
            LoadData();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            workFlowControl1.ClearData();
            PRO_LOG_RAPID_INFO.DataContext = null;
            PRO_LOG_RAPID_RESULTS.ItemsSource = null;
            PRO_LOG_RAPID_ORIGINAL_DATA1.DataContext = null;
            PRO_LOG_RAPID_ORIGINAL_DATA.ItemsSource = null;
            LoadDataList(1);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid.SelectedItem as DataRowView;
            if (row == null) return;
            var model = Utility.ModelHandler<Model.DM_LOG_OPS_PLAN>.FillModel(row.Row);
            if (model == null) return;

            var model_PRO_LOG_RAPID_INFO = PRO_LOG_RAPID_INFO.DataContext as Model.PRO_LOG_RAPID_INFO;
            var model_PRO_LOG_RAPID_ORIGINAL_DATA1 = PRO_LOG_RAPID_ORIGINAL_DATA1.DataContext as Model.PRO_LOG_RAPID_ORIGINAL_DATA;
            var modelList1 = PRO_LOG_RAPID_RESULTS.ItemsSource as Utility.DataCollection<Model.PRO_LOG_RAPID_RESULTS>;
            var modelList2 = PRO_LOG_RAPID_ORIGINAL_DATA.ItemsSource as Utility.DataCollection<Model.PRO_LOG_RAPID_ORIGINAL_DATA>;
            List<Model.PRO_LOG_RAPID_RESULTS> modelList_PRO_LOG_RAPID_RESULTS = null;
            if (modelList1 != null) modelList_PRO_LOG_RAPID_RESULTS = modelList1.ToList();
            List<Model.PRO_LOG_RAPID_ORIGINAL_DATA> modelList_PRO_LOG_RAPID_ORIGINAL_DATA = null;
            if (modelList2 != null) modelList_PRO_LOG_RAPID_ORIGINAL_DATA = modelList2.ToList();
            if (model_PRO_LOG_RAPID_INFO.HasError() || model_PRO_LOG_RAPID_ORIGINAL_DATA1.HasError() || (modelList1 != null && modelList1.HasError()) || (modelList2 != null && modelList2.HasError()))
                throw new ArgumentException("输入数据有误，请检查！");
            model_PRO_LOG_RAPID_INFO.REQUISITION_CD = model.REQUISITION_CD;
            using (var dataser = new DataServiceHelper())
            {
                var dt1 = dataser.GetWorkDetailsFileNames(model.JOB_PLAN_CD).Tables[0];
                var list1 = new List<string>();
                foreach (var arr in dt1.AsEnumerable().Select(r => r.FieldEx<string>("filename").Trim('\n').Split('\n')))
                {
                    list1.AddRange(arr);
                }
                var list2 = new List<string>();
                if (modelList_PRO_LOG_RAPID_ORIGINAL_DATA != null) list2 = modelList_PRO_LOG_RAPID_ORIGINAL_DATA.Select(m => m.DATA_NAME).ToList();
                if (list1.Count == 0 && list2.Count > 0)
                {
                    MessageBox.Show(App.Current.MainWindow, "无需上传原始数据文件！");
                    return;
                }
                var missingFiles = list1.FindAll(s => list2.Find(s1 => s1.ToLower() == s.ToLower()) == null);
                var excessFiles = list2.FindAll(s => list1.Find(s1 => s1.ToLower() == s.ToLower()) == null);
                if (missingFiles.Count > 0 || excessFiles.Count > 0)
                {
                    var w = new editWindow();
                    w.Title = "文件名不一致";
                    w.Owner = App.Current.MainWindow;
                    w.textbox1.Clear();
                    w.textbox1.IsReadOnly = true;
                    w.textbox1.TextWrapping = TextWrapping.NoWrap;
                    w.textbox1.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    w.textbox1.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    w.textbox1.AppendText("原始数据文件和小队施工-作业明细上的文件名不一致，请核对后再提交！\n");
                    if (missingFiles.Count > 0)
                    {
                        w.textbox1.AppendText(string.Format(">>>>>缺少的文件({0})\n", missingFiles.Count));
                        missingFiles.ForEach(s => w.textbox1.AppendText(s + "\n"));
                    }
                    if (excessFiles.Count > 0)
                    {
                        w.textbox1.AppendText(string.Format(">>>>>多余的文件({0})\n", excessFiles.Count));
                        excessFiles.ForEach(s => w.textbox1.AppendText(s + "\n"));
                    }
                    w.ShowDialog();
                    return;
                }
                var tasks = new Utility.DataCollection<UploadController.UploadTask>();
                if (modelList_PRO_LOG_RAPID_RESULTS != null)
                    foreach (var m in modelList_PRO_LOG_RAPID_RESULTS)
                    {
                        if (!string.IsNullOrEmpty(m.FullName) && tasks.ToList().Find(t => t.FullName == m.FullName) == null)
                            tasks.Add(new UploadController.UploadTask { FullName = m.FullName });
                    }
                if (modelList_PRO_LOG_RAPID_ORIGINAL_DATA != null)
                    foreach (var m in modelList_PRO_LOG_RAPID_ORIGINAL_DATA)
                    {
                        if (!string.IsNullOrEmpty(m.FullName) && tasks.ToList().Find(t => t.FullName == m.FullName) == null)
                            tasks.Add(new UploadController.UploadTask { FullName = m.FullName });
                    }
                new FileUpload().BeginUpload(tasks);
                var taskList = tasks.ToList();
                if (taskList.Find(t => t.hasError) != null)
                {
                    MessageBox.Show(App.Current.MainWindow, "上传出错，保存失败！");
                    return;
                }
                if (modelList_PRO_LOG_RAPID_RESULTS != null)
                    foreach (var m in modelList_PRO_LOG_RAPID_RESULTS)
                    {
                        var task = taskList.Find(t => t.FullName == m.FullName);
                        if (task != null)
                        {
                            m.FILEID = task.FileID;
                        }
                    }
                if (modelList_PRO_LOG_RAPID_ORIGINAL_DATA != null)
                    foreach (var m in modelList_PRO_LOG_RAPID_ORIGINAL_DATA)
                    {
                        var task = taskList.Find(t => t.FullName == m.FullName);
                        if (task != null)
                        {
                            m.FILEID = task.FileID;
                        }
                    }
                if (dataser.Save_测井现场提交信息(
                    model.JOB_PLAN_CD, Utility.ModelHelper.SerializeObject(model_PRO_LOG_RAPID_INFO),
                    Utility.ModelHelper.SerializeObject(modelList_PRO_LOG_RAPID_RESULTS),
                    Utility.ModelHelper.SerializeObject(model_PRO_LOG_RAPID_ORIGINAL_DATA1),
                    Utility.ModelHelper.SerializeObject(modelList_PRO_LOG_RAPID_ORIGINAL_DATA)
                    ))
                {
                    LoadData();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }

        private void ChangeMenuState()
        {
            //menu1.IsEnabled = false;
            //menu2.IsEnabled = false;
            //menu3.IsEnabled = false;
            //Save1.IsEnabled = false;
            var row = dataGrid.SelectedItem as DataRowView;
            //using (DataServiceHelper dataser = new DataServiceHelper())
            //{
            //    using (var userser = new UserServiceHelper())
            //    {
            //        var role = userser.GetActiveUserRoles();
            //        var flowData1 = Utility.ModelHandler<Model.SYS_WORK_FLOW>.FillModel(dataser.GetWorkFlow(row.Row.FieldEx<string>("JOB_PLAN_CD"), (int)Model.SYS_Enums.WorkFlowType.计划任务书));
            workFlowControl1.LoadData(row.Row.FieldEx<string>("JOB_PLAN_CD"));
            //    if (flowData1 == null) return;
            //    var flowData2 = workFlowControl1.GetData();
            //    if (flowData1[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.审核通过)
            //    {
            //        if (flowData1[0].TARGET_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME && (flowData2 == null || flowData2[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.审核未通过))
            //        {
            //            Save1.IsEnabled = true;
            //            menu1.IsEnabled = true;
            //        }
            //        if (flowData2 == null) return;
            //        if (flowData2[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.提交审核 && flowData2[0].TARGET_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME)
            //            menu2.IsEnabled = true;
            //        if (flowData2[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.审核通过 && flowData2[0].SOURCE_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME)
            //            menu3.IsEnabled = true;
            //    }
            //}
            //}
        }

        //private bool SetWorkFlow(string target_loginname, int state)
        //{
        //    using (var dataser = new DataServiceHelper())
        //    {
        //        var row = dataGrid.SelectedItem as DataRowView;
        //        if (dataser.SetWorkFlow(row.Row.FieldEx<string>("JOB_PLAN_CD"), target_loginname, DataService.WorkFlowType.测井现场提交信息, state))
        //        {
        //            ChangeMenuState();
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            var w = new SelectUserWindow1();
            w.UserRole = UserService.UserRole.解释管理员;
            w.Owner = System.Windows.Application.Current.MainWindow;
            w.ShowDialog();
            if (!string.IsNullOrEmpty(w.LoginName))
            {
                //if (SetWorkFlow(w.LoginName, (int)Model.SYS_Enums.FlowState.提交审核))
                //{
                //    System.Windows.MessageBox.Show("提交审核成功！");
                //}
                var row = dataGrid.SelectedItem as DataRowView;
                workFlowControl1.SubmitReview(w.LoginName, row.Row.FieldEx<string>("JOB_PLAN_CD"));
            }

        }

        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            //var flowData = workFlowControl1.GetData();
            //var result = System.Windows.MessageBox.Show("是否通过审核？", "小队施工信息审核", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            //int state = -1;
            //if (result == MessageBoxResult.Yes) state = (int)Model.SYS_Enums.FlowState.审核通过;
            //if (result == MessageBoxResult.No) state = (int)Model.SYS_Enums.FlowState.审核未通过;
            //if (state > -1 && SetWorkFlow(flowData[flowData.Length - 1].SOURCE_LOGINNAME, state))
            //{
            //    System.Windows.MessageBox.Show("审核成功！");
            //}
            var row = dataGrid.SelectedItem as DataRowView;
            workFlowControl1.Review("小队施工信息审核", row.Row.FieldEx<string>("JOB_PLAN_CD"));
        }

        private void MenuItem3_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectUserWindow();
            window.IncludedMyself = true;
            window.Owner = System.Windows.Application.Current.MainWindow;
            window.ShowDialog();
            using (var dataser = new DataServiceHelper())
            {
                var row = dataGrid.SelectedItem as DataRowView;
                //if (window.UserModel != null && dataser.SetWorkFlow(row.Row.FieldEx<string>("JOB_PLAN_CD"), window.UserModel.COL_LOGINNAME, DataService.WorkFlowType.测井现场提交信息, (int)Model.SYS_Enums.FlowState.数据指派))
                //{
                //    System.Windows.MessageBox.Show("指派成功！");
                //    ChangeMenuState();
                //}
                if (window.UserModel != null)
                    workFlowControl1.AppointData(window.UserModel.COL_LOGINNAME, row.Row.FieldEx<string>("JOB_PLAN_CD"));
            }
        }

        private void Menu4Item_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid.SelectedItem as DataRowView;
            workFlowControl1.CancelSubmitReview("小队施工信息审核取消提交审核", row.Row.FieldEx<string>("JOB_PLAN_CD"));
        }

        private void Menu5Item_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid.SelectedItem as DataRowView;
            workFlowControl1.CancelReview("小队施工信息退回审核", row.Row.FieldEx<string>("JOB_PLAN_CD"));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var items = PRO_LOG_RAPID_RESULTS.SelectedItems;
            var tasks = new Utility.DataCollection<DownloadController.DownloadTask>();
            foreach (Model.PRO_LOG_RAPID_RESULTS model in items)
            {
                tasks.Add(new DownloadController.DownloadTask { FileID = model.FILEID, Name = model.FILENAME });
            }
            FileDownload.Start(tasks);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var items = PRO_LOG_RAPID_ORIGINAL_DATA.SelectedItems;
            var tasks = new Utility.DataCollection<DownloadController.DownloadTask>();
            foreach (Model.PRO_LOG_RAPID_ORIGINAL_DATA model in items)
            {
                tasks.Add(new DownloadController.DownloadTask { FileID = model.FILEID, Name = model.DATA_NAME });
            }
            FileDownload.Start(tasks);
        }

        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            workFlowControl1.ClearData();
            PRO_LOG_RAPID_INFO.DataContext = null;
            PRO_LOG_RAPID_RESULTS.ItemsSource = null;
            PRO_LOG_RAPID_ORIGINAL_DATA1.DataContext = null;
            PRO_LOG_RAPID_ORIGINAL_DATA.ItemsSource = null;
            int page = (int)e.CurrentPageIndex;
            LoadDataList(page);
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            SearchPanel.DataContext = new Model.LogPlanSearch();
        }
    }
}
