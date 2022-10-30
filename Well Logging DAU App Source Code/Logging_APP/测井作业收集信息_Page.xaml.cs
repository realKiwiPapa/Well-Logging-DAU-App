using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// 测井作业收集信息_Page.xaml 的交互逻辑
    /// </summary>
    public partial class 测井作业收集信息_Page
    {
        public 测井作业收集信息_Page()
        {
            InitializeComponent();
        }

        private void LoadDataList(DataServiceHelper dataser, int page)
        {
            int total = 0;
            dataGrid.ItemsSource = dataser.GetPlanList(Utility.ModelHelper.SerializeObject(SearchPanel.DataContext), page, out total).Tables[0].GetDefaultView();
            dataPager1.TotalCount = total;
        }
        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            save1.IsEnabled = false;
            tabControl1.IsEnabled = false;
            
            ResetData();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                //dataGrid.ItemsSource = dataser.GetPlanList(Utility.ModelHelper.SerializeObject(SearchPanel.DataContext)).Tables[0].GetDefaultView();
                job_id1.DisplayMemberPath = "JOB_CHINESE_NAME";
                job_id1.SelectedValuePath = "JOB_ID";
                job_id1.ItemsSource = dataser.GetJobIDlist().Tables[0].GetDefaultView();
                DataView queryDate = dataser.GetComboxList_QueryDate().Tables[0].GetDefaultView();
                QUERY_DATE.DisplayMemberPath = "QUREY_DATE";
                QUERY_DATE.SelectedValuePath = "DATE_VALUE";
                QUERY_DATE.ItemsSource = queryDate;
                QUERY_DATE.Text = "最近一年";
                var slopProperties = dataser.GetComboBoxList_SlopProperties().Tables[0].GetDefaultView();
                jtlt.ItemsSource = slopProperties;
                SLOP_PROPERTIES.ItemsSource = slopProperties;
                LoadDataList(dataser, 1);
            }
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void ResetData()
        {
            PRO_LOG_CORE.DataContext = null;
            PRO_LOG_CORE.DataContext = new Model.PRO_LOG_CORE();
            PRO_LOG_TESTOIL.DataContext = null;
            PRO_LOG_TESTOIL.DataContext = new Model.PRO_LOG_TESTOIL();
            PRO_LOG_SLOP.DataContext = null;
            PRO_LOG_SLOP.DataContext = new Model.PRO_LOG_SLOP();
            PRO_LOG_PRODUCE.DataContext = null;
            PRO_LOG_PRODUCE.DataContext = new Model.PRO_LOG_PRODUCE();
            PRO_LOG_CASIN.DataContext = null;
            PRO_LOG_CASIN.DataContext = new Model.PRO_LOG_CASIN();
            PRO_LOG_CEMENT.DataContext = null;
            PRO_LOG_CEMENT.DataContext = new Model.PRO_LOG_CEMENT();
            PRO_LOG_BIT_PROGRAM.DataContext = null;
            PRO_LOG_BIT_PROGRAM.DataContext = new Model.PRO_LOG_BIT_PROGRAM();
            DM_LOG_LOGGING_INTERPRETATION.DataContext = null;
            DM_LOG_LOGGING_INTERPRETATION.DataContext = new Model.DM_LOG_LOGGING_INTERPRETATION();
            COM_BASE_STRATA_LAYER2.DataContext = null;
            COM_BASE_STRATA_LAYER2.DataContext = new Model.COM_BASE_STRATA_LAYER2();

            Nowtime1.Value = Nowtime2.Value = Nowtime3.Value = Nowtime4.Value = Nowtime5.Value = Nowtime6.Value = Nowtime7.Value = Nowtime8.Value = DateTime.Now;
            dataGrid1.ItemsSource = dataGrid2.ItemsSource = dataGrid3.ItemsSource = dataGrid4.ItemsSource = dataGrid5.ItemsSource = dataGrid6.ItemsSource = dataGrid7.ItemsSource = dataGrid8.ItemsSource = dataGrid9.ItemsSource = null;
            comboxip01.SelectedIndex = comboxip02.SelectedIndex = comboxip03.SelectedIndex = comboxip04.SelectedIndex = comboxip05.SelectedIndex = comboxip06.SelectedIndex = comboxip07.SelectedIndex = comboxip08.SelectedIndex = comboxip09.SelectedIndex = 0;
        }
        //取芯参数PRO_LOG_CORE
        //井试油参数PRO_LOG_TESTOIL
        //钻井液参数PRO_LOG_SLOP
        //生产参数PRO_LOG_PRODUCE
        //套管参数PRO_LOG_CASIN
        //固井参数PRO_LOG_CEMENT
        //钻头程序PRO_LOG_BIT_PROGRAM
        //录井解释成果DM_LOG_LOGGING_INTERPRETATION
        //地层分层数据COM_BASE_STRATA_LAYER2
        #region  Button_New_Checked
        private void Button_New_PRO_LOG_CORE_Checked(object sender, RoutedEventArgs e)
        {
            if (PRO_LOG_CORE != null)
            {
                PRO_LOG_CORE.DataContext = new Model.PRO_LOG_CORE();
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                JOB_PLAN_CD1.Text = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
                Nowtime1.Value = DateTime.Now;
            }
        }
        private void Button_New_PRO_LOG_TESTOIL_Checked(object sender, RoutedEventArgs e)
        {
            if (PRO_LOG_TESTOIL != null)
            {
                PRO_LOG_TESTOIL.DataContext = new Model.PRO_LOG_TESTOIL();
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                JOB_PLAN_CD2.Text = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
                Nowtime2.Value = DateTime.Now;
            }
        }
        private void Button_New_PRO_LOG_SLOP_Checked(object sender, RoutedEventArgs e)
        {
            if (PRO_LOG_SLOP != null)
            {
                PRO_LOG_SLOP.DataContext = new Model.PRO_LOG_SLOP();
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                JOB_PLAN_CD3.Text = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
                Nowtime3.Value = DateTime.Now;
            }
        }
        private void Button_New_PRO_LOG_PRODUCE_Checked(object sender, RoutedEventArgs e)
        {
            if (PRO_LOG_PRODUCE != null)
            {
                PRO_LOG_PRODUCE.DataContext = new Model.PRO_LOG_PRODUCE();
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                JOB_PLAN_CD4.Text = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
                Nowtime4.Value = DateTime.Now;
            }
        }
        private void Button_New_PRO_LOG_CASIN_Checked(object sender, RoutedEventArgs e)
        {
            if (PRO_LOG_CASIN != null)
            {
                PRO_LOG_CASIN.DataContext = new Model.PRO_LOG_CASIN();
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                JOB_PLAN_CD5.Text = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
                Nowtime5.Value = DateTime.Now;
            }
        }
        private void Button_New_PRO_LOG_CEMENT_Checked(object sender, RoutedEventArgs e)
        {
            if (PRO_LOG_CEMENT != null)
            {
                PRO_LOG_CEMENT.DataContext = new Model.PRO_LOG_CEMENT();
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                JOB_PLAN_CD6.Text = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
                Nowtime6.Value = DateTime.Now;
            }
        }
        private void Button_New_PRO_LOG_BIT_PROGRAM_Checked(object sender, RoutedEventArgs e)
        {
            if (PRO_LOG_BIT_PROGRAM != null)
            {
                PRO_LOG_BIT_PROGRAM.DataContext = new Model.PRO_LOG_BIT_PROGRAM();
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                JOB_PLAN_CD7.Text = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
                Nowtime7.Value = DateTime.Now;
            }
        }
        private void Button_New_DM_LOG_LOGGING_INTERPRETATION_Checked(object sender, RoutedEventArgs e)
        {
            if (DM_LOG_LOGGING_INTERPRETATION != null)
            {
                DM_LOG_LOGGING_INTERPRETATION.DataContext = new Model.DM_LOG_LOGGING_INTERPRETATION();
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                JOB_PLAN_CD8.Text = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
                Nowtime8.Value = DateTime.Now;
            }
        }
        private void Button_New_COM_BASE_STRATA_LAYER2_Checked(object sender, RoutedEventArgs e)
        {
            if (COM_BASE_STRATA_LAYER2 != null)
            {
                COM_BASE_STRATA_LAYER2.DataContext = new Model.COM_BASE_STRATA_LAYER2();
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                JOB_PLAN_CD9.Text = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
            }
        }
        #endregion

        #region dataGrid
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid.SelectedItem)) return;
            tabControl1.IsEnabled = true;
            save1.IsEnabled = true;
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                ResetData();
                dataGrid1.ItemsSource = dataser.GetDataGList_取心参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                dataGrid2.ItemsSource = dataser.GetDataGList_井试油参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                dataGrid3.ItemsSource = dataser.GetDataGList_钻井液参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                dataGrid4.ItemsSource = dataser.GetDataGList_生产参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                dataGrid5.ItemsSource = dataser.GetDataGList_套管参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                dataGrid6.ItemsSource = dataser.GetDataGList_固井参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                dataGrid7.ItemsSource = dataser.GetDataGList_钻头程序(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                dataGrid8.ItemsSource = dataser.GetDataGList_录井解释成果(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                dataGrid9.ItemsSource = dataser.GetDataGList_地层分层数据2(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();

                string JPCD = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD").ToString().Trim();
                JOB_PLAN_CD1.Text = JPCD;
                JOB_PLAN_CD2.Text = JPCD;
                JOB_PLAN_CD3.Text = JPCD;
                JOB_PLAN_CD4.Text = JPCD;
                JOB_PLAN_CD5.Text = JPCD;
                JOB_PLAN_CD6.Text = JPCD;
                JOB_PLAN_CD7.Text = JPCD;
                JOB_PLAN_CD8.Text = JPCD;
                JOB_PLAN_CD9.Text = JPCD;

                Nowtime1.Value = Nowtime2.Value = Nowtime3.Value = Nowtime4.Value = Nowtime5.Value = Nowtime6.Value = Nowtime7.Value = Nowtime8.Value = DateTime.Now;
            }
        }
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid1.SelectedItem)) return;
            DataRowView dataRowView = dataGrid1.SelectedItem as DataRowView;
            PRO_LOG_CORE.DataContext = null;
            PRO_LOG_CORE.DataContext = new Model.PRO_LOG_CORE();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetData_取心参数(dataRowView.Row.FieldEx<string>("COREID")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");
                PRO_LOG_CORE.DataContext = Utility.ModelHandler<Model.PRO_LOG_CORE>.FillModel(dataTable.Rows[0]);
                Button_Change_PRO_LOG_CORE.IsChecked = true;
                Nowtime1.Value = DateTime.Now;
            }
        }
        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid2.SelectedItem)) return;
            DataRowView dataRowView = dataGrid2.SelectedItem as DataRowView;
            PRO_LOG_TESTOIL.DataContext = null;
            PRO_LOG_TESTOIL.DataContext = new Model.PRO_LOG_TESTOIL();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetData_井试油参数(dataRowView.Row.FieldEx<string>("TESTOILID")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");
                PRO_LOG_TESTOIL.DataContext = Utility.ModelHandler<Model.PRO_LOG_TESTOIL>.FillModel(dataTable.Rows[0]);
                Button_Change_PRO_LOG_TESTOIL.IsChecked = true;
                Nowtime2.Value = DateTime.Now;
            }
        }
        private void dataGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid3.SelectedItem)) return;
            DataRowView dataRowView = dataGrid3.SelectedItem as DataRowView;
            PRO_LOG_SLOP.DataContext = null;
            PRO_LOG_SLOP.DataContext = new Model.PRO_LOG_SLOP();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetData_钻井液参数(dataRowView.Row.FieldEx<string>("MUDID")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");
                PRO_LOG_SLOP.DataContext = Utility.ModelHandler<Model.PRO_LOG_SLOP>.FillModel(dataTable.Rows[0]);
                Button_Change_PRO_LOG_SLOP.IsChecked = true;
                Nowtime3.Value = DateTime.Now;
            }
        }
        private void dataGrid4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid4.SelectedItem)) return;
            DataRowView dataRowView = dataGrid4.SelectedItem as DataRowView;
            PRO_LOG_PRODUCE.DataContext = null;
            PRO_LOG_PRODUCE.DataContext = new Model.PRO_LOG_PRODUCE();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetData_生产参数(dataRowView.Row.FieldEx<string>("WORKID")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");
                PRO_LOG_PRODUCE.DataContext = Utility.ModelHandler<Model.PRO_LOG_PRODUCE>.FillModel(dataTable.Rows[0]);
                Button_Change_PRO_LOG_PRODUCE.IsChecked = true;
                Nowtime4.Value = DateTime.Now;
            }
        }
        private void dataGrid5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid5.SelectedItem)) return;
            DataRowView dataRowView = dataGrid5.SelectedItem as DataRowView;
            PRO_LOG_CASIN.DataContext = null;
            PRO_LOG_CASIN.DataContext = new Model.PRO_LOG_CASIN();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetData_套管参数(dataRowView.Row.FieldEx<string>("CASINID")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");
                PRO_LOG_CASIN.DataContext = Utility.ModelHandler<Model.PRO_LOG_CASIN>.FillModel(dataTable.Rows[0]);
                Button_Change_PRO_LOG_CASIN.IsChecked = true;
                Nowtime5.Value = DateTime.Now;
            }
        }
        private void dataGrid6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid6.SelectedItem)) return;
            DataRowView dataRowView = dataGrid6.SelectedItem as DataRowView;
            PRO_LOG_CEMENT.DataContext = null;
            PRO_LOG_CEMENT.DataContext = new Model.PRO_LOG_CEMENT();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetData_固井参数(dataRowView.Row.FieldEx<string>("CEMENTID")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");
                PRO_LOG_CEMENT.DataContext = Utility.ModelHandler<Model.PRO_LOG_CEMENT>.FillModel(dataTable.Rows[0]);
                Button_Change_PRO_LOG_CEMENT.IsChecked = true;
                Nowtime6.Value = DateTime.Now;
            }
        }
        private void dataGrid7_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid7.SelectedItem)) return;
            DataRowView dataRowView = dataGrid7.SelectedItem as DataRowView;
            PRO_LOG_BIT_PROGRAM.DataContext = null;
            PRO_LOG_BIT_PROGRAM.DataContext = new Model.PRO_LOG_BIT_PROGRAM();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetData_钻头程序(dataRowView.Row.FieldEx<string>("BITID")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");
                PRO_LOG_BIT_PROGRAM.DataContext = Utility.ModelHandler<Model.PRO_LOG_BIT_PROGRAM>.FillModel(dataTable.Rows[0]);
                Button_Change_PRO_LOG_BIT_PROGRAM.IsChecked = true;
                Nowtime7.Value = DateTime.Now;
            }
        }
        private void dataGrid8_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid8.SelectedItem)) return;
            DataRowView dataRowView = dataGrid8.SelectedItem as DataRowView;
            DM_LOG_LOGGING_INTERPRETATION.DataContext = null;
            DM_LOG_LOGGING_INTERPRETATION.DataContext = new Model.DM_LOG_LOGGING_INTERPRETATION();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetData_录井解释成果(dataRowView.Row.FieldEx<string>("INTERPRETATION_CD")).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");//
                DM_LOG_LOGGING_INTERPRETATION.DataContext = Utility.ModelHandler<Model.DM_LOG_LOGGING_INTERPRETATION>.FillModel(dataTable.Rows[0]);
                Button_Change_DM_LOG_LOGGING_INTERPRETATION.IsChecked = true;
                Nowtime8.Value = DateTime.Now;
            }
        }
        private void dataGrid9_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid9.SelectedItem)) return;
            DataRowView dataRowView = dataGrid9.SelectedItem as DataRowView;
            COM_BASE_STRATA_LAYER2.DataContext = null;
            COM_BASE_STRATA_LAYER2.DataContext = new Model.COM_BASE_STRATA_LAYER2();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dataTable = dataser.GetData_地层分层数据(Convert.ToInt32(dataRowView.Row.FieldEx<decimal>("SEQ_NO"))).Tables[0];
                if (dataTable == null || dataTable.Rows.Count < 1) throw new Exception("没有找到数据！");
                COM_BASE_STRATA_LAYER2.DataContext = Utility.ModelHandler<Model.COM_BASE_STRATA_LAYER2>.FillModel(dataTable.Rows[0]);
                Button_Change_COM_BASE_STRATA_LAYER2.IsChecked = true;
            }
        }
        #endregion
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.IsEnabled = false;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                //dataGrid.ItemsSource = dataser.GetPlanList(Utility.ModelHelper.SerializeObject(SearchPanel.DataContext)).Tables[0].GetDefaultView();
                LoadDataList(dataser, 1);
            }
        }

        private void But_文件导入_Click(object sender, RoutedEventArgs e)
        {
            var m_DirDlg = new System.Windows.Forms.FolderBrowserDialog();
            m_DirDlg.ShowDialog();
        }
        #region Save
        private void Save_取芯参数_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
            if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
            //if (dataGrid1.ItemsSource == null) MessageBox.Show("为空！");
            //dataGrid1.Items.Refresh();

            //var dataList_取心参数 = dataGrid1.ItemsSource as Utility.DataCollection<Model.PRO_LOG_CORE>;
            //List<Model.PRO_LOG_CORE> dataChangedList_取心参数 = null;
            //List<Model.PRO_LOG_CORE> dataRemoveList_取心参数 = null;
            //if (dataList_取心参数 != null)
            //{
            //    dataChangedList_取心参数 = dataList_取心参数.ChangedData;
            //    dataRemoveList_取心参数 = dataList_取心参数.RemovedData.FindAll(o => o.COREID != null);
            //}
            //if (dataList_取心参数 != null && dataList_取心参数.HasError())
            //{
            //    MessageBox.Show("输入数据有误，请检查！");
            //    return;
            //}
            Model.PRO_LOG_CORE model = PRO_LOG_CORE.DataContext as Model.PRO_LOG_CORE;
            if (model == null)
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            model.REQUISITION_CD = dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Savedata_取心参数(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid1.ItemsSource = dataser.GetDataGList_取心参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        private void Save_井试油参数_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
            if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
            Model.PRO_LOG_TESTOIL model = PRO_LOG_TESTOIL.DataContext as Model.PRO_LOG_TESTOIL;
            if (model == null)
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            model.REQUISITION_CD = dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Savedata_井试油参数(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid2.ItemsSource = dataser.GetDataGList_井试油参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        private void Save_钻井液参数_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
            if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
            Model.PRO_LOG_SLOP model = PRO_LOG_SLOP.DataContext as Model.PRO_LOG_SLOP;
            if (model == null)
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            model.REQUISITION_CD = dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Savedata_钻井液参数(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid3.ItemsSource = dataser.GetDataGList_钻井液参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        private void Save_生产参数_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
            if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
            Model.PRO_LOG_PRODUCE model = PRO_LOG_PRODUCE.DataContext as Model.PRO_LOG_PRODUCE;
            if (model == null)
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            model.REQUISITION_CD = dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Savedata_生产参数(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid4.ItemsSource = dataser.GetDataGList_生产参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        private void Save_套管参数_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
            if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
            Model.PRO_LOG_CASIN model = PRO_LOG_CASIN.DataContext as Model.PRO_LOG_CASIN;
            if (model == null)
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            model.REQUISITION_CD = dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Savedata_套管参数(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid5.ItemsSource = dataser.GetDataGList_套管参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        private void Save_固井参数_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
            if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
            Model.PRO_LOG_CEMENT model = PRO_LOG_CEMENT.DataContext as Model.PRO_LOG_CEMENT;
            if (model == null)
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            model.REQUISITION_CD = dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Savedata_固井参数(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid6.ItemsSource = dataser.GetDataGList_固井参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        private void Save_钻头程序_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
            if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
            Model.PRO_LOG_BIT_PROGRAM model = PRO_LOG_BIT_PROGRAM.DataContext as Model.PRO_LOG_BIT_PROGRAM;
            if (model == null)
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            model.REQUISITION_CD = dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Savedata_钻头程序(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid7.ItemsSource = dataser.GetDataGList_钻头程序(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        private void Save_录井解释成果_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
            if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
            Model.DM_LOG_LOGGING_INTERPRETATION model = DM_LOG_LOGGING_INTERPRETATION.DataContext as Model.DM_LOG_LOGGING_INTERPRETATION;
            if (model == null)
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            model.REQUISITION_CD = dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Savedata_录井解释成果(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid8.ItemsSource = dataser.GetDataGList_录井解释成果(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        private void Save_地层分层数据_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
            //string Selected = dataRowView.Row.FieldEx<string>("JOB_PLAN_CD");
            if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
            Model.COM_BASE_STRATA_LAYER2 model = COM_BASE_STRATA_LAYER2.DataContext as Model.COM_BASE_STRATA_LAYER2;
            if (model == null)
            {
                MessageBox.Show(App.Current.MainWindow, "输入数据有误，请检查！");
                return;
            }
            model.REQUISITION_CD = dataRowView.Row.FieldEx<string>("REQUISITION_CD");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (dataser.Savedata_地层分层数据2(Utility.ModelHelper.SerializeObject(model)))
                {
                    dataGrid9.ItemsSource = dataser.GetDataGList_地层分层数据2(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                    MessageBox.Show(App.Current.MainWindow, "保存成功！");
                }
                else MessageBox.Show(App.Current.MainWindow, "保存失败！");
            }
        }
        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fDlg = new System.Windows.Forms.OpenFileDialog();
            fDlg.Title = "选择地层分层数据";
            fDlg.Filter = "地层分层(xls;.xlsx)|*.xls;*.xlsx";
            if (fDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                DataTable dt = Utility.FileParser.ExcelToDataTable(fDlg.FileName);
                using (var dataser = new DataServiceHelper())
                {
                    if (dataser.Import_地层分层数据2(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD"), dataRowView.Row.FieldEx<string>("REQUISITION_CD"), dt))
                    {
                        dataGrid9.ItemsSource = dataser.GetDataGList_地层分层数据2(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show(App.Current.MainWindow, "导入成功");
                        return; ;
                    }
                }
                MessageBox.Show(App.Current.MainWindow, "导入失败！");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var fDlg = new System.Windows.Forms.OpenFileDialog();
            fDlg.Title = "选择录井解释成果数据";
            fDlg.Filter = "录井解释成果(xls;.xlsx)|*.xls;*.xlsx";
            if (fDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                DataTable dt = Utility.FileParser.ExcelToDataTable(fDlg.FileName);
                using (var dataser = new DataServiceHelper())
                {
                    if (dataser.Import_录井解释成果(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD"), dataRowView.Row.FieldEx<string>("REQUISITION_CD"), dt))
                    {
                        dataGrid8.ItemsSource = dataser.GetDataGList_录井解释成果(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show(App.Current.MainWindow, "导入成功");
                        return; ;
                    }
                }
                MessageBox.Show(App.Current.MainWindow, "导入失败！");
            }
        }
        #region 删除
        private void Del_Log_Core(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid1.SelectedItem as DataRowView;
            string core_id = dataRowView.Row.FieldEx<string>("COREID");
            if (MessageBox.Show("确定要删除当前选定的数据吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogCore(core_id))
                    {
                        dataGrid1.ItemsSource = dataser.GetDataGList_取心参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
        private void Del_Log_TestOil(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid2.SelectedItem as DataRowView;
            string test_oil_id = dataRowView.Row.FieldEx<string>("TESTOILID");
            if (MessageBox.Show("确定要删除当前选定的数据吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogTestOil(test_oil_id))
                    {
                        dataGrid2.ItemsSource = dataser.GetDataGList_井试油参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
        private void Del_Log_Slop(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid3.SelectedItem as DataRowView;
            string mud_id = dataRowView.Row.FieldEx<string>("MUDID");
            if (MessageBox.Show("确定要删除当前选定的数据吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogSlop(mud_id))
                    {
                        dataGrid3.ItemsSource = dataser.GetDataGList_钻井液参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
        private void Del_Log_Produce(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid4.SelectedItem as DataRowView;
            string work_id = dataRowView.Row.FieldEx<string>("WORKID");
            if (MessageBox.Show("确定要删除当前选定的数据吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogProduce(work_id))
                    {
                        dataGrid4.ItemsSource = dataser.GetDataGList_生产参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
        private void Del_Log_Casin(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid5.SelectedItem as DataRowView;
            string casin_id = dataRowView.Row.FieldEx<string>("CASINID");
            if (MessageBox.Show("确定要删除当前选定的数据吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogCasin(casin_id))
                    {
                        dataGrid5.ItemsSource = dataser.GetDataGList_套管参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
        private void Del_Log_Cement(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid6.SelectedItem as DataRowView;
            string cement_id = dataRowView.Row.FieldEx<string>("CEMENTID");
            if (MessageBox.Show("确定要删除当前选定的数据吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogCement(cement_id))
                    {
                        dataGrid6.ItemsSource = dataser.GetDataGList_固井参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
        private void Del_Log_Bit_Program(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid7.SelectedItem as DataRowView;
            string bit_id = dataRowView.Row.FieldEx<string>("BITID");
            if (MessageBox.Show("确定要删除当前选定的数据吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogBitProgram(bit_id))
                    {
                        dataGrid7.ItemsSource = dataser.GetDataGList_钻头程序(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
        private void Del_Log_Logging_Interpretation(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid8.SelectedItem as DataRowView;
            string interpretation_cd = dataRowView.Row.FieldEx<string>("INTERPRETATION_CD");
            if (MessageBox.Show("确定要删除当前选定的数据吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteLogLoggingInterpretation(interpretation_cd))
                    {
                        dataGrid8.ItemsSource = dataser.GetDataGList_录井解释成果(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
        private void Del_Com_Base_Strata_Layer2(object sender, RoutedEventArgs e)
        {
            var dataRowView = dataGrid9.SelectedItem as DataRowView;
            int seq_no = int.Parse(dataRowView.Row.FieldEx<string>("SEQ_NO"));
            if (MessageBox.Show("确定要删除当前选定的数据吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.DeleteComBaseStrataLayer2(seq_no))
                    {
                        dataGrid9.ItemsSource = dataser.GetDataGList_地层分层数据2(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show("删除成功！");
                    }
                }
            }
        }
        #endregion

        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            tabControl1.IsEnabled = false;
            int page = (int)e.CurrentPageIndex;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                LoadDataList(dataser, page);
            }
        }

        #region 从其它服务器导入
        private void But_其他导入_1(object sender, RoutedEventArgs e)
        {
            if (comboxip01.SelectedIndex == 1)
            {
                comboxip01.SelectedIndex = 0;
                #region 取芯数据
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.Save_A12取芯数据(dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString(), dataRowView.Row.FieldEx<string>("JOB_PLAN_CD"), dataRowView.Row.FieldEx<string>("WELL_JOB_NAME")))
                    {
                        dataGrid1.ItemsSource = dataser.GetDataGList_取心参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show(App.Current.MainWindow, "导入成功！");
                    }
                    else
                        MessageBox.Show(App.Current.MainWindow, "无数据可导入！");
                }
                #endregion
            }
            else if (comboxip01.SelectedIndex == 0)
            {
                return;
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "无数据可导入!!!");
            }
        }

        private void But_其他导入_2(object sender, RoutedEventArgs e)
        {
            if (comboxip02.SelectedIndex == 1)
            {
                comboxip02.SelectedIndex = 0;
                MessageBox.Show(App.Current.MainWindow, "暂无数据可导入!!!");
            }
            else
                return;
        }

        private void But_其他导入_3(object sender, RoutedEventArgs e)
        {
            if (comboxip03.SelectedIndex == 1)
            {
                comboxip03.SelectedIndex = 0;
                #region 钻井液参数
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.Save_A12钻井液全性能(dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString(), dataRowView.Row.FieldEx<string>("JOB_PLAN_CD"), dataRowView.Row.FieldEx<string>("WELL_JOB_NAME")))
                    {
                        dataGrid3.ItemsSource = dataser.GetDataGList_钻井液参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show(App.Current.MainWindow, "导入成功！");
                    }
                    else
                        MessageBox.Show(App.Current.MainWindow, "无数据可导入！");
                }
                #endregion
            }
            else if (comboxip03.SelectedIndex == 0)
            {
                return;
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "无数据可导入!!!");
            }
        }

        private void But_其他导入_4(object sender, RoutedEventArgs e)
        {
            if (comboxip04.SelectedIndex == 1)
            {
                comboxip04.SelectedIndex = 0;
                MessageBox.Show(App.Current.MainWindow, "暂无数据可导入!!!");
            }
            else
                return;
        }

        private void But_其他导入_5(object sender, RoutedEventArgs e)
        {
            if (comboxip05.SelectedIndex == 1)
            {
                comboxip05.SelectedIndex = 0;
                #region 套管参数
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.Save_A12套管参数(dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString(), dataRowView.Row.FieldEx<string>("JOB_PLAN_CD"), dataRowView.Row.FieldEx<string>("WELL_JOB_NAME")))
                    {
                        dataGrid5.ItemsSource = dataser.GetDataGList_套管参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show(App.Current.MainWindow, "导入成功！");
                    }
                    else
                        MessageBox.Show(App.Current.MainWindow, "无数据可导入！");
                }
                #endregion
            }
            else if (comboxip05.SelectedIndex == 0)
            {
                return;
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "无数据可导入!!!");
            }
        }

        private void But_其他导入_6(object sender, RoutedEventArgs e)
        {
            if (comboxip06.SelectedIndex == 1)
            {
                comboxip06.SelectedIndex = 0;
                #region 固井参数
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.Save_A12固井参数(dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString(), dataRowView.Row.FieldEx<string>("JOB_PLAN_CD"), dataRowView.Row.FieldEx<string>("WELL_JOB_NAME")))
                    {
                        dataGrid6.ItemsSource = dataser.GetDataGList_固井参数(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show(App.Current.MainWindow, "导入成功！");
                    }
                    else
                        MessageBox.Show(App.Current.MainWindow, "无数据可导入！");
                }
                #endregion
            }
            else if (comboxip06.SelectedIndex == 0)
            {
                return;
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "无数据可导入!!!");
            }
        }

        private void But_其他导入_7(object sender, RoutedEventArgs e)
        {
            if (comboxip07.SelectedIndex == 1)
            {
                comboxip07.SelectedIndex = 0;
                #region 钻头尺寸
                DataRowView dataRowView = dataGrid.SelectedItem as DataRowView;
                if (dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == null || dataRowView.Row.FieldEx<string>("JOB_PLAN_CD") == "") return;
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.Save_A12钻头使用情况信息(dataRowView.Row.FieldEx<string>("REQUISITION_CD").ToString(), dataRowView.Row.FieldEx<string>("JOB_PLAN_CD"), dataRowView.Row.FieldEx<string>("WELL_JOB_NAME")))
                    {
                        dataGrid7.ItemsSource = dataser.GetDataGList_钻头程序(dataRowView.Row.FieldEx<string>("JOB_PLAN_CD")).Tables[0].GetDefaultView();
                        MessageBox.Show(App.Current.MainWindow, "导入成功！");
                    }
                    else
                        MessageBox.Show(App.Current.MainWindow, "无数据可导入！");
                }
                #endregion
            }
            else if (comboxip07.SelectedIndex == 0)
            {
                return;
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "无数据可导入!!!");
            }
        }

        private void But_其他导入_8(object sender, RoutedEventArgs e)
        {
            if (comboxip08.SelectedIndex == 1)
            {
                comboxip08.SelectedIndex = 0;
                MessageBox.Show(App.Current.MainWindow, "暂无数据可导入!!!");
            }
            else
                return;
        }

        private void But_其他导入_9(object sender, RoutedEventArgs e)
        {
            if (comboxip09.SelectedIndex == 1)
            {
                comboxip09.SelectedIndex = 0;
                MessageBox.Show(App.Current.MainWindow, "暂无数据可导入!!!");
            }
            else
                return;
        }
        #endregion

        private void Page_Initialized(object sender, EventArgs e)
        {
            SearchPanel.DataContext = new Model.LogPlanSearch();
        }
    }
}