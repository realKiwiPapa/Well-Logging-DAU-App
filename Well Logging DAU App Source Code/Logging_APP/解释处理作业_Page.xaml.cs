using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.IO;

using Logging_App.Utility;
using Microsoft.Win32;

namespace Logging_App
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class 解释处理作业_Page : Page
    {

        public 解释处理作业_Page()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var m_DirDlg = new System.Windows.Forms.FolderBrowserDialog();
            m_DirDlg.SelectedPath = textBox1.Text;
            if (m_DirDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = m_DirDlg.SelectedPath;
                var strs = textBox1.Text.Split('\\');
                if (addButton.IsChecked == true) PROCESS_NAME.Text = strs[strs.Length - 1];
                var model = PRO_LOG_DATA_PUBLISH.DataContext as Model.PRO_LOG_DATA_PUBLISH;
                FileHelper.FileDelete(m_DirDlg.SelectedPath, "~", "$");
                model.LOG_ORIGINALITY_DATA = FileHelper.GetDirInfo(m_DirDlg.SelectedPath, false, s => s == "yssj").FileLength;
                model.LOG_INTERPRET_REPORT = FileHelper.GetDirInfo(m_DirDlg.SelectedPath, false, s => s == "bg").FileLength; ;
                model.LOG_INTERPRET_RESULT = FileHelper.GetDirInfo(m_DirDlg.SelectedPath, false, s => !"bg,qt,yysj".Split(',').Contains(s)).FileLength;
                model.FILE_NUMBER = FileHelper.GetDirInfo(m_DirDlg.SelectedPath, false).FileNumber;
                var itemModels = new Utility.DataCollection<Model.PRO_LOG_PROCESSING_ITEM>();
                var mapModels = new Utility.DataCollection<Model.PRO_LOG_PROCESS_MAP>();

                foreach (var dir in new DirectoryInfo(m_DirDlg.SelectedPath).GetDirectories())
                {
                    string dirName = dir.Name.ToLower();
                    if (!"bg,qt,yssj,pdf".Split(',').Contains(dir.Name.ToLower()))//
                    {
                        decimal? itemID = null;
                        foreach (DataRow dr in (comBox1.ItemsSource as DataView).Table.Rows)
                        {
                            if (dr["PROCESSING_ITEM_CODE"] == DBNull.Value) continue;
                            foreach (string code in dr.FieldEx<string>("PROCESSING_ITEM_CODE").Split(';'))
                            {
                                if (dirName == code.ToLower())
                                {
                                    itemID = dr.FieldEx<decimal>("PROCESSING_ITEM_ID");
                                    break;
                                }
                            }
                            if (itemID != null)
                            {
                                itemModels.Add(new Model.PRO_LOG_PROCESSING_ITEM
                                {
                                    PROCESSING_ITEM_ID = itemID.Value,
                                    P_PROCESS_SOFTWARE = p_process_software.Text,
                                    P_SUPERVISOR = p_supervisor.Text,
                                    PROCESSOR = processor.Text,
                                    INTERPRETER = processor.Text
                                });
                                break;
                            }
                        }
                    }
                    if (dirName == "pdf")
                    {
                        //FileHelper.GetDirFiles(System.IO.Path.Combine(m_DirDlg.SelectedPath, dirName),
                        //  false, s => s == "head")
                        FileHelper.GetDirFiles(dir.FullName, true).ForEach(f =>
                        {//"head,map".Split(',').Contains(s)
                            if (f.Extension.ToLower() == ".pdf")
                            {
                                string mapCoding = string.Empty;
                                decimal? startDEP = null;
                                decimal? endDEP = null;
                                decimal? itemId = null;
                                mapCoding = new Regex(@"_([^_]+)_").Match(f.Name).Groups[1].Value.ToLower();
                                var rows = from DataRowView drv in mapsCoding
                                           where drv.Row.FieldEx<string>("MAPS_CODING") == mapCoding
                                           select drv.Row;
                                if (rows.Count() > 0)
                                {
                                    mapCoding = rows.First().FieldEx<string>("MAPS_CODING");
                                    itemId = rows.First().FieldEx<decimal?>("PROCESSING_ITEM_ID");
                                }
                                else
                                {
                                    rows = from DataRowView drv in mapsCoding
                                           where drv.Row.FieldEx<string>("MAPS_CODING") == dirName
                                           select drv.Row;
                                    if (rows.Count() > 0)
                                    {
                                        mapCoding = rows.First().FieldEx<string>("MAPS_CODING");
                                        itemId = rows.First().FieldEx<decimal?>("PROCESSING_ITEM_ID");
                                    }
                                }
                                //(\d+\.?\d*)-(\d+\.?\d*)[^\.#\d]
                                var groups = new Regex(@"\((\d+\.?\d*)-(\d+\.?\d*)\)").Match(f.Name).Groups;
                                if (!string.IsNullOrEmpty(groups[1].Value))
                                {
                                    startDEP = Convert.ToDecimal(groups[1].Value);
                                    endDEP = Convert.ToDecimal(groups[2].Value);
                                }

                                //FileStream fs = File.OpenRead(f.FullName);
                                //byte[] file_data = new byte[fs.Length];
                                //int remaining = file_data.Length;
                                //int offset = 0;
                                //while (remaining > 0)
                                //{
                                //    int read = fs.Read(file_data, offset, file_data.Length);
                                //    remaining -= read;
                                //    offset += read;
                                //}
                                //fs.Close();

                                mapModels.Add(new Model.PRO_LOG_PROCESS_MAP
                                {
                                    PROCESSING_ITEM_ID = itemId,
                                    MAP_DATA_NAME = f.Name,
                                    MAPS_CODING = mapCoding,
                                    MAP_START_DEP = startDEP,
                                    MAP_END_DEP = endDEP,
                                    MAP_PDF_SIZE = f.Length,
                                    P_PROCESS_SOFTWARE = p_process_software.Text,
                                    MAP_PDF_DATA=null
                                    //MAP_PDF_DATA = file_data
                                });
                            }
                        });
                    }
                }
                PRO_LOG_PROCESSING_ITEM.ItemsSource = itemModels;
                PRO_LOG_PROCESS_MAP.ItemsSource = mapModels;
            }

        }

        private Utility.DataCollection<Model.DM_LOG_SOURCE_DATA> sourceData = new Utility.DataCollection<Model.DM_LOG_SOURCE_DATA>();
        private DataView mapsCoding;
        private DataView qualityTypes = null;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dataser = new DataServiceHelper())
            {
                
                WELL_JOB_NAME.DisplayMemberPath = "WELL_JOB_NAME";
                WELL_JOB_NAME.SelectedValuePath = "DRILL_JOB_ID";
                WELL_JOB_NAME.ItemsSource = dataser.GetProcessWellJob().Tables[0].GetDefaultView();
                var code = dataser.GetData_解释处理项目编码().Tables[0].GetDefaultView();
                comBox1.ItemsSource = code;
                comBox2.ItemsSource = code;
                log_series_name.ItemsSource = dataser.GetComboBoxList_LogSeries().Tables[0].GetDefaultView();
                log_data_format.ItemsSource = dataser.GetComboBoxList_DataFormat().Tables[0].GetDefaultView();
                qualityTypes = dataser.GetComboBoxList_CurveQualityTypes().Tables[0].GetDefaultView();
                curveName.ItemsSource = dataser.GetAllCurveNames().Tables[0].GetDefaultView();
                mapsCoding = dataser.GetData_解释图件编码().Tables[0].GetDefaultView();
                interpret_center.ItemsSource = dataser.GetComboBoxList_InterpretCenter().Tables[0].GetDefaultView();
                log_series_id.ItemsSource = dataser.GetComboBoxList_LogSeries().Tables[0].GetDefaultView();
                acceptance_way_name.ItemsSource = dataser.GetComboBoxList_AcceptanceWay().Tables[0].GetDefaultView();
                p_process_software.ItemsSource = dataser.GetComboBoxList_ProcessSoftware().Tables[0].GetDefaultView();
                sourceList.DisplayMemberPath = "JOB_PLAN_CD";
                sourceList.ValueMemberPath = "JOB_PLAN_CD";
                sourceList.SelectedMemberPath = "Selected";
                DataView queryDate = dataser.GetComboxList_QueryDate().Tables[0].GetDefaultView();
                QUERY_DATE.DisplayMemberPath = "QUREY_DATE";
                QUERY_DATE.SelectedValuePath = "DATE_VALUE";
                QUERY_DATE.ItemsSource = queryDate;
                QUERY_DATE.Text = "最近一年";
                addButton.IsChecked = false;
                addButton.IsChecked = true;
                LoadDataList(1);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            LoadDataList(1);
            addButton.IsChecked = true;
        }

        public string Process_Name = string.Empty;
        private void LoadDataList(int page)
        {
            using (var dataser = new DataServiceHelper())
            {
                int total = 0;
                dataGrid1.ItemsSource = dataser.GetDataList_解释处理作业(ModelHelper.SerializeObject(SearchPanel.DataContext),page,out total).Tables[0].GetDefaultView();
                //Counts.Content = "      解释处理作业：" + total;
                dataPager1.TotalCount = total;
                if (!string.IsNullOrEmpty(Process_Name))
                {
                    var drv = (from DataRowView item in dataGrid1.Items
                               where item.Row.FieldEx<string>("PROCESS_NAME") == Process_Name
                               select item).FirstOrDefault();
                    if (drv != null)
                    {
                        dataGrid1.ScrollIntoView(drv);
                        dataGrid1.SelectedItem = drv;
                    }
                    Process_Name = string.Empty;
                };
            }
        }

        private void LoadSourceData()
        {
            using (var dataser = new DataServiceHelper())
            {
                if (WELL_JOB_NAME.SelectedItem != null)
                    sourceData = Utility.ModelHandler<Model.DM_LOG_SOURCE_DATA>.FillModel(dataser.GetProcessDataJobSource((WELL_JOB_NAME.SelectedItem as DataRowView).Row.FieldEx<string>("DRILL_JOB_ID")));
                else
                    sourceData.Clear();
                foreach (DataRow row in sourceDT.Rows)
                {
                    var model = sourceData.ToList().Find(o => o.JOB_PLAN_CD == row.FieldEx<string>("JOB_PLAN_CD"));
                    if (model == null)
                    {
                        if (sourceData.Count == 0)
                        {
                            model = new Model.DM_LOG_SOURCE_DATA { JOB_PLAN_CD = row.FieldEx<string>("JOB_PLAN_CD") };
                            sourceData.AddNew(model);
                        }
                    }
                    if (model != null) model.Selected = true;
                }
                sourceList.ItemsSource = sourceData;
            }
        }

        private void WELL_JOB_NAME_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSourceData();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            var selectedSourceData = sourceData.ToList().FindAll(m => m.Selected == true);
            if (selectedSourceData.Count < 1) throw new ArgumentException("请选择数据源！");
            var model1 = DM_LOG_PROCESS.DataContext as Model.DM_LOG_PROCESS;
            var model2 = PRO_LOG_DATA_PUBLISH.DataContext as Model.PRO_LOG_DATA_PUBLISH;
            var model3 = PRO_LOG_PROCESSING_ITEM.ItemsSource as DataCollection<Model.PRO_LOG_PROCESSING_ITEM>;
            var model4 = PRO_LOG_PROCESS_MAP.ItemsSource as DataCollection<Model.PRO_LOG_PROCESS_MAP>;
            var model5 = PRO_LOG_PROCESSING_CURVERATING.ItemsSource as DataCollection<Model.PRO_LOG_PROCESSING_CURVERATING>;
            var model6 = PRO_LOG_ORIGINAL_MAP.ItemsSource as DataCollection<Model.PRO_LOG_ORIGINAL_MAP>;
            if (model1 == null || model2 == null || model3 == null || model4 == null || model5 == null||model6==null || model1.HasError() || model2.HasError() || model3.HasError() || model4.HasError() || model5.HasError()||model6.HasError())
                throw new ArgumentException("输入数据有误，请检查！");

            using (var dataser = new DataServiceHelper())
            {
                if (dataser.Save_解释处理作业(
                    ModelHelper.SerializeObject(model1),
                    ModelHelper.SerializeObject(selectedSourceData),
                    ModelHelper.SerializeObject(model2),
                    ModelHandler<Model.PRO_LOG_PROCESSING_ITEM>.FillDataTable(model3.ToList()),
                    ModelHandler<Model.PRO_LOG_PROCESS_MAP>.FillDataTable(model4.ToList()),
                    ModelHandler<Model.PRO_LOG_PROCESSING_CURVERATING>.FillDataTable(model5.ToList()),
                    ModelHandler<Model.PRO_LOG_ORIGINAL_MAP>.FillDataTable(model6.ToList())
                    ))
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
            ResetData();
            LoadDataList(1);
        }

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetData()
        {
            //Save1.IsEnabled = true;
            Menu1.IsEnabled = false;
            WELL_JOB_NAME.Visibility = Visibility.Visible;
            well_job_name.Visibility = Visibility.Collapsed;
            button3.Visibility = Visibility.Collapsed;
            DM_LOG_PROCESS.DataContext = new Model.DM_LOG_PROCESS();
            PRO_LOG_DATA_PUBLISH.DataContext = new Model.PRO_LOG_DATA_PUBLISH();
            PRO_LOG_PROCESSING_ITEM.ItemsSource = new DataCollection<Model.PRO_LOG_PROCESSING_ITEM>();
            PRO_LOG_PROCESS_MAP.ItemsSource = new DataCollection<Model.PRO_LOG_PROCESS_MAP>();
            PRO_LOG_PROCESSING_CURVERATING.ItemsSource = new DataCollection<Model.PRO_LOG_PROCESSING_CURVERATING>();
            PRO_LOG_ORIGINAL_MAP.ItemsSource = new DataCollection<Model.PRO_LOG_ORIGINAL_MAP>();
            sourceData.Clear();
            dataGrid1.SelectedIndex = -1;
            workFlowControl1.ClearData();
            textBox1.Clear();
            ContextMenu cm = (ContextMenu)dataGrid1.Resources["rowContextMenu"];
            cm.Visibility = (Visibility)new BooleanToVisibilityConverter().Convert(workFlowControl1.CanDelete, null, null, null);
            //using (UserServiceHelper userser = new UserServiceHelper())
            //{
            //    using (DataServiceHelper dataser = new DataServiceHelper())
            //    {
            //        WELL_JOB_NAME.ItemsSource = dataser.GetProcessWellJob().GetDefaultView();
            //    }
            //    if (userser.GetActiveUserRoles().Contains(UserService.UserRole.系统管理员))
            //        cm.Visibility = Visibility.Visible;
            //}
        }

        private void ChangeMenuState()
        {
            //Save1.IsEnabled = false;
            //var flowData = workFlowControl1.GetData();
            //if (flowData == null || flowData.Count == 0) return;
            //if (flowData[flowData.Count - 1].SOURCE_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME && (flowData.Count == 1 || flowData[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.审核未通过))
            //    Save1.IsEnabled = true;
        }

        private void addButton_Checked(object sender, RoutedEventArgs e)
        {
            if (WELL_JOB_NAME == null) return;
            ResetData();
        }

        private DataTable sourceDT = new DataTable();
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid1.SelectedItem)) return;
            var row = dataGrid1.SelectedItem as DataRowView;
            if (row == null)
            {
                ResetData();
                return;
            }
            Menu1.IsEnabled = true;
            editButton.IsChecked = true;
            using (var dataser = new DataServiceHelper())
            {
                var ds = dataser.GetData_解释处理作业(row.Row.FieldEx<string>("PROCESS_ID"));
                workFlowControl1.LoadData(row.Row.FieldEx<string>("PROCESS_ID"));
                sourceDT = ds.Tables["DM_LOG_SOURCE_DATA"];
                well_job_name.Text = row.Row.FieldEx<string>("WELL_JOB_NAME");
                WELL_JOB_NAME.Visibility = Visibility.Collapsed;
                well_job_name.Visibility = Visibility.Visible;
                DM_LOG_PROCESS.DataContext = ModelHandler<Model.DM_LOG_PROCESS>.FillModel(ds.Tables["DM_LOG_PROCESS"])[0];
                PRO_LOG_DATA_PUBLISH.DataContext = ModelHandler<Model.PRO_LOG_DATA_PUBLISH>.FillModel(ds.Tables["PRO_LOG_DATA_PUBLISH"])[0];
                PRO_LOG_PROCESSING_ITEM.ItemsSource = ModelHandler<Model.PRO_LOG_PROCESSING_ITEM>.FillModel(ds.Tables["PRO_LOG_PROCESSING_ITEM"]);
                PRO_LOG_PROCESS_MAP.ItemsSource = ModelHandler<Model.PRO_LOG_PROCESS_MAP>.FillModel(ds.Tables["PRO_LOG_PROCESS_MAP"]);
                PRO_LOG_PROCESSING_CURVERATING.ItemsSource = ModelHandler<Model.PRO_LOG_PROCESSING_CURVERATING>.FillModel(ds.Tables["PRO_LOG_PROCESSING_CURVERATING"]);
                PRO_LOG_ORIGINAL_MAP.ItemsSource=ModelHandler<Model.PRO_LOG_ORIGINAL_MAP>.FillModel(ds.Tables["PRO_LOG_ORIGINAL_MAP"]);
                if (PRO_LOG_PROCESSING_ITEM.ItemsSource == null) PRO_LOG_PROCESSING_ITEM.ItemsSource = new DataCollection<Model.PRO_LOG_PROCESSING_ITEM>();
                if (PRO_LOG_PROCESS_MAP.ItemsSource == null) PRO_LOG_PROCESS_MAP.ItemsSource = new DataCollection<Model.PRO_LOG_PROCESS_MAP>();
                if (PRO_LOG_PROCESSING_CURVERATING.ItemsSource == null) PRO_LOG_PROCESSING_CURVERATING.ItemsSource = new DataCollection<Model.PRO_LOG_PROCESSING_CURVERATING>();
                if (PRO_LOG_ORIGINAL_MAP.ItemsSource == null) PRO_LOG_ORIGINAL_MAP.ItemsSource = new DataCollection<Model.PRO_LOG_ORIGINAL_MAP>();
            }
            ChangeMenuState();
            LoadSourceData();
            button3.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = PRO_LOG_PROCESSING_ITEM.ItemsSource as Utility.DataCollection<Model.PRO_LOG_PROCESSING_ITEM>;
            var w = new 解释处理项目编码_Window();
            w.DataContext = comBox1.ItemsSource;
            w.Owner = App.Current.MainWindow;
            w.LoadData(PRO_LOG_PROCESSING_ITEM.ItemsSource);
            w.ShowDialog();
            var data = w.GetData();
            if (data != null)
            {
                model.RemoveAll(m => !data.Contains(m.PROCESSING_ITEM_ID));

                var data1 = model.Select(m => m.PROCESSING_ITEM_ID);
                (from d in data
                 where !data1.Contains(d)
                 select d).ForEach(d => model.Add(
                     new Model.PRO_LOG_PROCESSING_ITEM
                     {
                         PROCESSING_ITEM_ID = d,
                         P_PROCESS_SOFTWARE = p_process_software.Text,
                         P_SUPERVISOR = p_supervisor.Text,
                         PROCESSOR = processor.Text,
                         INTERPRETER = processor.Text
                     }));

                PRO_LOG_PROCESSING_ITEM.ItemsSource = model;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var data = PRO_LOG_PROCESS_MAP.ItemsSource as DataCollection<Model.PRO_LOG_PROCESS_MAP>;
            data.Add(new Model.PRO_LOG_PROCESS_MAP { P_PROCESS_SOFTWARE = p_process_software.Text });
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var cbox = sender as ComboBox;
            if (cbox.ItemsSource != mapsCoding)
            {
                cbox.ItemsSource = mapsCoding;
            }
        }

        private void PRO_LOG_PROCESS_MAP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PRO_LOG_PROCESS_MAP.SelectedIndex > -1) mapDel.IsEnabled = true;
        }

        private void mapDel_Click(object sender, RoutedEventArgs e)
        {
            var item = PRO_LOG_PROCESS_MAP.SelectedItem as Model.PRO_LOG_PROCESS_MAP;
            var data = PRO_LOG_PROCESS_MAP.ItemsSource as DataCollection<Model.PRO_LOG_PROCESS_MAP>;
            if (item != null)
                if (data.Remove(item))
                    mapDel.IsEnabled = false;
        }

        private void ProcessingItemComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var cbox = sender as ComboBox;
            if (cbox.ItemsSource != comBox1.ItemsSource)
                cbox.ItemsSource = comBox1.ItemsSource;
        }

        private void QualityTypesComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var cbox = sender as ComboBox;
            if (cbox.ItemsSource != qualityTypes)
                cbox.ItemsSource = qualityTypes;
        }
        /// <summary>
        /// 刷新DM_LOG_ITEMS
        /// </summary>
        /// <param name="DaTa"></param>
        /**
        private void PRO_LOG_PROCESSING_CURVERATING_updata(List<Model.TreeModel> treelist)
        {
            var items = PRO_LOG_PROCESSING_CURVERATING.ItemsSource as Utility.DataCollection<Model.PRO_LOG_PROCESSING_CURVERATING>;

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
                    items.AddNew(new Model.PRO_LOG_PROCESSING_CURVERATING() { LOG_ITEM_ID = tree.ID, LOGGING_NAME = tree.Name });
                }
            }
        }
        private void ratingAdd_Click(object sender, RoutedEventArgs e)
        {
            //var data = PRO_LOG_PROCESSING_CURVERATING.ItemsSource as DataCollection<Model.PRO_LOG_PROCESSING_CURVERATING>;
            //data.Add(new Model.PRO_LOG_PROCESSING_CURVERATING());
            var items = PRO_LOG_PROCESSING_CURVERATING.ItemsSource as Utility.DataCollection<Model.PRO_LOG_PROCESSING_CURVERATING>;
            预测项目_Window w = new 预测项目_Window();
            w.Title = "测井项目";
            if (items != null) w.DataContext = items.Select(o => o.LOG_ITEM_ID).ToArray();
            w.Owner = Application.Current.MainWindow;
            w.ChangeTextEvent += new ChangeTextHandler_预测项目(PRO_LOG_PROCESSING_CURVERATING_updata);
            w.ShowDialog();
        }
        
        private void ratingDel_Click(object sender, RoutedEventArgs e)
        {
            var item = PRO_LOG_PROCESSING_CURVERATING.SelectedItem as Model.PRO_LOG_PROCESSING_CURVERATING;
            var data = PRO_LOG_PROCESSING_CURVERATING.ItemsSource as DataCollection<Model.PRO_LOG_PROCESSING_CURVERATING>;
            if (item != null)
                if (data.Remove(item))
                    ratingDel.IsEnabled = false;
        }
*/
        private void PRO_LOG_PROCESSING_CURVERATING_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (PRO_LOG_PROCESSING_CURVERATING.SelectedIndex > -1) ratingDel.IsEnabled = true;
        }

        private void sourceList_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            if (!Save1.IsEnabled) return;
            object item = null;
            if (e.IsSelected)
                item = e.Item;
            else if (sourceList.SelectedItems.Count > 0)
                item = sourceList.SelectedItems[sourceList.SelectedItems.Count - 1];
            if (item != null)
            {
                var model = item as Model.DM_LOG_SOURCE_DATA;
                using (var dataser = new DataServiceHelper())
                {
                    var dataSet = dataser.GetDefaultValue_解释处理作业(model.JOB_PLAN_CD);
                    if (dataSet.Tables["DM_LOG_BASE"].Rows.Count > 0)
                    {
                        if (log_total_time.DecValue == null)
                            log_total_time.DecValue = dataSet.Tables["DM_LOG_BASE"].Rows[0].FieldEx<decimal?>("log_total_time");
                        if (lost_time.DecValue == null)
                            lost_time.DecValue = dataSet.Tables["DM_LOG_BASE"].Rows[0].FieldEx<decimal?>("lost_time");
                        if (string.IsNullOrWhiteSpace(team_org_id.Text))
                            team_org_id.Text = dataSet.Tables["DM_LOG_BASE"].Rows[0].FieldEx<string>("team_org_id");
                        if (string.IsNullOrWhiteSpace(log_series_id.Text))
                            log_series_id.Text = dataSet.Tables["DM_LOG_BASE"].Rows[0].FieldEx<string>("log_series_id");
                    }
                    if (string.IsNullOrWhiteSpace(p_supervisor.Text))
                    {
                        //p_supervisor.Text = dataser.GetWorkFlow(model.JOB_PLAN_CD, 4).Select("FLOW_STATE=0", "FLOW_ID DESC")[0].FieldEx<string>("SOURCE_NAME");
                        var data = workFlowControl1.QueryData(WorkflowService.WorkflowType.测井现场提交信息, model.JOB_PLAN_CD).DataList;
                        if (data != null && data.Length > 0)
                            p_supervisor.Text = data.FirstOrDefault(d => d.FLOW_STATE == WorkflowService.WorkflowState.数据指派).SOURCE_LOGINNAME;
                    }

                    //if (PRO_LOG_PROCESSING_CURVERATING.Items.Count == 0)
                    //    PRO_LOG_PROCESSING_CURVERATING.ItemsSource = ModelHandler<Model.PRO_LOG_PROCESSING_CURVERATING>.FillModel(dataSet.Tables["DM_LOG_ITEMS"]);
                }
            }
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid1.SelectedItem as DataRowView;
            string process_id = dataRowView.Row.FieldEx<string>("PROCESS_ID");
            if (MessageBox.Show(App.Current.MainWindow, "删除解释处理作业删除与之关联的归档入库\n确定要删除“" + dataRowView.Row.FieldEx<string>("PROCESS_NAME") + "”吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogProcess(process_id))
                    {
                        ResetData();
                        LoadDataList(1);
                        MessageBox.Show(App.Current.MainWindow, "删除成功！");
                    }
                    else
                        MessageBox.Show(App.Current.MainWindow, "删除失败！");
                }
        }

        private void menu1_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            var sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.FileName = row.Row.FieldEx<string>("PROCESS_NAME") + "_上井解释登记卡";
            sfd.DefaultExt = "doc";
            sfd.Filter = "上井解释登记卡(*.doc)|*.doc";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            using (var fileser = new FileServiceHelper())
            {
                var data = fileser.Export_上井解释登记卡(row.Row.FieldEx<string>("PROCESS_ID"));
                if (data.Length > 0)
                {
                    File.WriteAllBytes(sfd.FileName, data);
                    Utility.WinShell.OpenFile(sfd.FileName);
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var model = PRO_LOG_PROCESSING_CURVERATING.ItemsSource as Utility.DataCollection<Model.PRO_LOG_PROCESSING_CURVERATING>;
            var w = new Window_测井曲线编码();
            w.DataContext = curveName.ItemsSource;
            w.Owner = App.Current.MainWindow;
            w.LoadData(PRO_LOG_PROCESSING_CURVERATING.ItemsSource, comBox1.ItemsSource);
            w.ShowDialog();
            var data = w.GetData();
            if (data != null)
            {
                model.RemoveAll(m => data.Find(o => o.ID.Value == m.CURVE_ID) == null);

                var data1 = model.Select(m => m.CURVE_ID);
                (from d in data
                 where !data1.Contains(d.ID.Value)
                 select d).ForEach(d => model.Add(new Model.PRO_LOG_PROCESSING_CURVERATING
                 {
                     CURVE_ID = d.ID.Value,
                     PROCESSING_ITEM_ID = d.Item_id,
                     SCENE_RATING = "优",
                     INDOOR_RATING = "优"
                 }));

                PRO_LOG_PROCESSING_CURVERATING.ItemsSource = model;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var w = new WELL_JOB_NAME_Window();
            w.Owner = Application.Current.MainWindow;
            w.ShowDialog();
            if (string.IsNullOrEmpty(w.Drill_Job_ID)) return;
            var row = dataGrid1.SelectedItem as DataRowView;
            if (row == null) return;
            using (var dataser = new DataServiceHelper())
            {
                if (dataser.UpdateDrillID(row.Row.FieldEx<decimal>("PROCESS_ID"), w.Drill_Job_ID))
                {
                    LoadDataList(1);
                    addButton.IsChecked = true;
                    MessageBox.Show("成功！");
                }
                else
                    MessageBox.Show("失败！");
            }
        }

        //private void Button_Click_4(object sender, RoutedEventArgs e)
        //{
        //    var button = sender as System.Windows.Controls.Button;
        //    var model = button.DataContext as Model.PRO_LOG_PROCESS_MAP;
        //    if (string.IsNullOrEmpty(model.MAPID))
        //    {
        //        MessageBox.Show(App.Current.MainWindow, "数据还没有保存！");
        //        return;
        //    }
        //    else
        //    {
        //        string filename = model.MAP_DATA_NAME;
        //        if (!string.IsNullOrEmpty(filename)) {
        //            MessageBox.Show("文件不存在");
        //            return;
        //        }

        //        byte[] filedata = model.MAP_PDF_DATA;
        //        SaveFileDialog sf = new SaveFileDialog();
        //        sf.RestoreDirectory = true;
        //        sf.Filter = "所有|*.*";
        //        sf.FileName = filename;
        //        if (sf.ShowDialog() == true)
        //        {
        //            FileStream fs = new FileStream(sf.FileName, FileMode.Create, FileAccess.Write);
        //            fs.Write(filedata, 0, filedata.Length);
        //            fs.Close();
        //            MessageBox.Show("保存成功!");
        //        }
        //    }
        //}

        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            int page = (int)e.CurrentPageIndex;
            LoadDataList(page);
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            SearchPanel.DataContext = new Model.PRO_LOG_DATA_PUBLISH();
        }

        private void PRO_LOG_ORIGINAL_MAP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PRO_LOG_ORIGINAL_MAP.SelectedIndex > -1) omapDel.IsEnabled = true;
        }

        private void omapAdd_Click(object sender, RoutedEventArgs e)
        {
            var data = PRO_LOG_ORIGINAL_MAP.ItemsSource as DataCollection<Model.PRO_LOG_ORIGINAL_MAP>;
            data.Add(new Model.PRO_LOG_ORIGINAL_MAP { REWORK_NUM=0,REVIEWER="现场评级人",MAP_DATE=DateTime.Now});
        }

        private void omapDel_Click(object sender, RoutedEventArgs e)
        {
            var item = PRO_LOG_ORIGINAL_MAP.SelectedItem as Model.PRO_LOG_ORIGINAL_MAP;
            var data = PRO_LOG_ORIGINAL_MAP.ItemsSource as DataCollection<Model.PRO_LOG_ORIGINAL_MAP>;
            if (item != null)
                if (data.Remove(item))
                    omapDel.IsEnabled = false;
        }
    }
}
