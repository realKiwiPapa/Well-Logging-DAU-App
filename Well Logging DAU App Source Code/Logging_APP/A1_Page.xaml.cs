using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Collections.ObjectModel;
using System.IO;
using Logging_App.Utility;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.Script.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Logging_App
{
    /// <summary>
    /// A1交互逻辑
    /// </summary>
    public partial class A1_Page : Page
    {
        public A1_Page()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                //初始化业主单位列表
                var PART_UNITSList = dataser.GetComboBoxList_甲方来电单位().Tables[0].GetDefaultView();
                PART_UNITS.DisplayMemberPath = "DEPARTMENT_REQUISITION_NAME";
                PART_UNITS.SelectedValuePath = "DEPARTMENT_REQUISITION_NAME";
                PART_UNITS.ItemsSource = PART_UNITSList;
            }
            LoadDataList(1);
            BuildTabItems();
        }

        private DataService.VIEW_REQUISITION_LIST selectedItem;

        public void LoadDataList(int page)
        {
            using (var dataser = new DataServiceHelper())
            {
                //REQUISITION_TYPE.DisplayMemberPath = "REQUISITION_TYPE_NAME";
                //REQUISITION_TYPE.SelectedValuePath = "REQUISITION_TYPE_ID";
                //REQUISITION_TYPE.ItemsSource = dataser.GetRequisitionTypes().Tables[0].GetDefaultView();
                int total = 0;
                dataGrid1.ItemsSource = dataser.GetDataPushListXY(text_well_name.Text, PART_UNITS.Text, START_TIME.Value, cbox_state.Text, cbox_state1.Text, page, out total).Tables[0].GetDefaultView();
                dataPager1.PageIndex = page;
                dataPager1.TotalCount = total;
                selectedItem = null;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LoadDataList(1);
        }

        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            int page = (int)e.CurrentPageIndex;
            LoadDataList(page);
        }

        private ObservableCollection<string> GetColumns(string extension, Stream stream)
        {
            IWorkbook workbook = null;
            ObservableCollection<string> columns = new ObservableCollection<string>();
            if (extension.Equals(".xls"))
                workbook = new HSSFWorkbook(stream);
            else if (extension.Equals(".xlsx"))
                workbook = new XSSFWorkbook(stream);
            else
                return null;

            ISheet sheet = workbook.GetSheetAt(0);
            //取列
            IRow row0 = sheet.GetRow(0);
            for (int i = 1; i < row0.LastCellNum; i++)
            {
                var clnName = row0.GetCell(i).ToString();
                columns.Add(clnName);
            }
            return columns;
        }

        private Regex reg = new Regex(@"[\u4e00-\u9fa5]{0,}\d{3}_[\u4e00-\u9fa5]{0,}");
        private Dictionary<string, string> viewDic = new Dictionary<string, string>();
        private void BuildTabItems()
        {
            //获取表头
            Dictionary<string, ObservableCollection<string>> clnDic = new Dictionary<string, ObservableCollection<string>>();
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var dt = dataser.GetFileTemplate().Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var filename = dt.Rows[i].FieldEx<string>("filename");
                    var extension = dt.Rows[i].FieldEx<string>("filefromat");
                    var view = dt.Rows[i].FieldEx<string>("relativeview");
                    var data = dt.Rows[i].FieldEx<byte[]>("filedata");
                    if (!reg.Match(filename).Success) continue;
                    if (data == null || data.Length == 0) continue;
                    var name = reg.Match(filename).Value;
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        var columns = GetColumns(extension, ms);
                        if (columns == null) continue;
                        viewDic.Add(name, view);
                        clnDic.Add(name, columns);
                    }
                }
            }

            //创建标签
            foreach (var name in clnDic.Keys)
            {
                var ti = new TabItem();
                ti.Header = name.Split('_')[1];
                ti.Tag = name;
                ti.GotFocus += new RoutedEventHandler(ti_GotFocus);
                var clnList = clnDic[name];
                var grid = BuildDataGrid(clnList);
                ti.Content = grid;
                tabControl1.Items.Add(ti);
            }
        }

        void ti_GotFocus(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem == null)
            {
                MessageBox.Show("没有选择数据！");
                return;
            }
            var ti = tabControl1.SelectedItem as TabItem;
            BindingData(ti, pList);
            //throw new NotImplementedException();
        }

        //private void BindingAllTabsData()
        //{
        //    if (dataGrid1.SelectedItem == null)
        //    {
        //        MessageBox.Show("没有选择数据！");
        //        return;
        //    }
        //    using (DataServiceHelper dataser = new DataServiceHelper())
        //    {
        //        foreach (var item in tabControl1.Items)
        //        {
        //            var ti = item as TabItem;
        //            var enumItem = (TabType)ti.Tag;
        //            var view = GetRelativeViewByItem(enumItem);
        //            if (string.IsNullOrEmpty(view)) return;
        //            var dt = dataser.GetA1DataByView(view, ModelHelper.SerializeObject(pList)).Tables[0];
        //            var grid = ti.Content as DataGrid;
        //            grid.ItemsSource = dt.GetDefaultView();
        //        }
        //    }
        //}

        private void BindingData(TabItem ti, List<string> pList)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                var name = ti.Tag.ToString();
                var view = viewDic[name];
                if (string.IsNullOrEmpty(view)) return;
                var grid = ti.Content as DataGrid;
                var dt = dataser.GetA1DataByView(view, ModelHelper.SerializeObject(pList)).Tables[0];

                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    var dgc = grid.Columns[i];
                    var clnName = dgc.Header.ToString().Trim();
                    var isExists = clnName.IndexOf("序号") > -1 || clnName.IndexOf("序次") > -1;
                    var index = isExists ? dt.Columns.IndexOf("序号") : dt.Columns.IndexOf(clnName);
                    if (index == -1)
                        dt.Columns.Add(clnName);
                    else
                        if (isExists)
                            dt.Columns[index].ColumnName = clnName;
                }
                dt.AcceptChanges();

                //var indexs = new List<int>();
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    var dc = dt.Columns[i];
                //    if (dc.ColumnName.Equals("PROCESS_ID") || dc.ColumnName.Equals("FILEPATH") || dc.ColumnName.Equals("DOCUMENT_FORMAT")) continue;
                //    var dgc = grid.Columns.AsEnumerable().Where(x => x.Header.ToString().Trim().Equals(dc.ColumnName)).FirstOrDefault();
                //    if (dgc == null)
                //        indexs.Add(i);
                //    //dt.Columns.RemoveAt(i);
                //}
                //for (int i = 0; i < indexs.Count; i++)
                //{
                //    dt.Columns.RemoveAt(indexs[i]);
                //}
                //dt.AcceptChanges();

                grid.ItemsSource = null;
                grid.ItemsSource = dt.GetDefaultView();
            }
        }

        private DataGrid BuildDataGrid(ObservableCollection<string> clnList)
        {
            Binding binding = null;
            DataGridTextColumn column = null;
            DataGrid grid = new DataGrid();
            grid.IsReadOnly = true;
            grid.ColumnWidth = DataGridLength.Auto;
            grid.AutoGenerateColumns = false;
            grid.CanUserAddRows = false;
            grid.CanUserDeleteRows = false;
            grid.CanUserSortColumns = false;
            grid.SelectionMode = DataGridSelectionMode.Single;
            foreach (var clnName in clnList)
            {
                binding = new Binding(clnName);
                binding.Mode = BindingMode.TwoWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                column = new DataGridTextColumn();
                column.Header = clnName;
                column.Binding = binding;
                grid.Columns.Add(column);
            }
            return grid;
        }

        private List<string> pList = new List<string>();
        private DataRowView[] drv = null;
        private void dataGrid1_MouseLeave(object sender, MouseEventArgs e)
        {
            var count = dataGrid1.SelectedItems.Count;
            if (count == 0) return;
            drv = new DataRowView[count];
            for (int i = 0; i < count; i++)
            {
                drv[i] = dataGrid1.SelectedItems[i] as DataRowView;
            }
            var num = drv.GroupBy(x => x.Row.FieldEx<string>("WELL_JOB_NAME")).Count();
            if (num > 1)
            {
                MessageBox.Show("请选择相同作业井名！");
                return;
            }

            pList.Clear();
            foreach (var dr in drv)
            {
                var process_id = dr.Row.FieldEx<string>("PROCESS_ID");
                pList.Add(process_id);
            }

            var item = tabControl1.SelectedItem as TabItem;
            item.Focus();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var ti = tabControl1.SelectedItem as TabItem;
            if (ti == null) return;
            var dv = (ti.Content as DataGrid).ItemsSource as DataView;
            if (dv == null) return;
            var sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Excel 99-2003|*.xls";
            sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.CheckPathExists = true;
            if ((bool)sfd.ShowDialog())
            {
                try
                {
                    using (DataServiceHelper dataser = new DataServiceHelper())
                    {
                        var dt = dataser.GetFileTemplate().Tables[0];
                        DataRow dr = dt.AsEnumerable().Where(x => x.FieldEx<string>("filename").Contains(ti.Header.ToString())).FirstOrDefault();
                        var data = dr["filedata"] as byte[];
                        if (data != null && data.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(data))
                            {
                                IWorkbook workbook =null;
                                var extension = dr["filefromat"];
                                if (extension.Equals(".xls"))
                                    workbook = new HSSFWorkbook(ms);
                                else
                                    workbook = new XSSFWorkbook(ms);
                                ISheet sheet = workbook.GetSheetAt(0);
                                int rowNum = sheet.LastRowNum + 1;
                                IRow row0 = sheet.GetRow(0);
                                for (int i = 0; i < dv.Table.Rows.Count; i++)
                                {
                                    IRow dataRow = sheet.CreateRow(i + rowNum);
                                    for (int j = 0; j < dv.Table.Columns.Count; j++)
                                    {
                                        var clnName = dv.Table.Columns[j].ColumnName;
                                        var cell = row0.Cells.Where(x => x.StringCellValue.Trim().Equals(clnName)).FirstOrDefault();
                                        if (cell != null)
                                        {
                                            dataRow.CreateCell(cell.ColumnIndex).SetCellValue(dv.Table.Rows[i][j].ToString());
                                        }
                                    }
                                }
                                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                                {
                                    workbook.Write(fs);
                                }
                                MessageBox.Show("导出成功!");
                            }
                        }
                        else
                            MessageBox.Show("模板文件不存在!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                }
            }
        }

        protected struct JsonObject
        {
            public string code { get; set; }
            public string msg { get; set; }
        }

        private void AppendMsg(string msg)
        {
            this.Dispatcher.Invoke(new Action(()=>{
                pushMsg.AppendText(msg);
            }));
        }

        private void PushData()
        {
            DataTable dt = null;
            string tableName = null;
            this.Dispatcher.Invoke(new Action(() =>
            {
                pushMsg.Clear();
                var ti = tabControl1.SelectedItem as TabItem;
                var grid = ti.Content as DataGrid;
                dt = (grid.ItemsSource as DataView).Table;
                tableName = ti.Tag.ToString().Split('_')[0];
            }));
            if (dt.Columns.Contains("FILEPATH"))
            {
                fileDic.Clear();
                tableName = "FileData";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var filename = dt.Rows[i].FieldEx<string>("DOCID") + '.' + dt.Rows[i].FieldEx<string>("DOCUMENT_FORMAT");
                    var filepath = dt.Rows[i].FieldEx<string>("FILEPATH");
                    fileDic.Add(filename, filepath);
                }
                if (fileDic.Count > 0)
                {
                    AppendMsg("提示:正在推送文件...");
                    using (DataServiceHelper dataser = new DataServiceHelper())
                    {
                        if (dataser.PushFileToA1(ModelHelper.SerializeObject(fileDic)))
                            AppendMsg("推送成功!\n");
                        else
                            AppendMsg("推送失败!\n");
                    }
                }
            }

            dt.TableName = tableName;
            var wellName = drv[0].Row.FieldEx<string>("WELL_JOB_NAME");
            DataSet dsData = new DataSet();
            dsData.Tables.Add(dt.Copy());
            var strYear = checkClient.GetProjectYear(wellName);
            var result = inputClient.DataImport(strYear, wellName, dsData);

            JavaScriptSerializer js = new JavaScriptSerializer();
            var joList = js.Deserialize<List<JsonObject>>(result);
            foreach (var obj in joList)
            {
                AppendMsg("代码:" + obj.code + "\n");
                var msg = obj.msg;
                if (msg.IndexOf("&") > -1)
                    msg = msg.Replace("&", "\n");
                if (msg.IndexOf("<br/>") > -1)
                    msg = msg.Replace("<br/>", "\n");
                AppendMsg("消息:" + msg + "\n");
            }
        }

        A1DataInputService.WellLogInputSoapClient inputClient = new A1DataInputService.WellLogInputSoapClient();
        A1DataCheckService.WellLogCheckSoapClient checkClient = new A1DataCheckService.WellLogCheckSoapClient();
        private Dictionary<string,string> fileDic = new Dictionary<string,string>();
        private void pushData_Click(object sender, RoutedEventArgs e)
        {
            var count = dataGrid1.SelectedItems.Count;
            if (count == 0)
            {
                MessageBox.Show("请选择推送井次!");
                return;
            }
            Task.Factory.StartNew(() => PushData()).ContinueWith(t => { var exp = t.Exception; });
            Thread.Sleep(100);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //var count = dataGrid1.SelectedItems.Count;
            //if (count == 0)
            //{
            //    MessageBox.Show("请选择推送井次!");
            //    return;
            //}

            //pushMsg.Clear();
            //var ti = tabControl1.SelectedItem as TabItem;
            //var grid = ti.Content as DataGrid;
            //var dt = (grid.ItemsSource as DataView).Table;
            //var tableName = ti.Tag.ToString().Split('_')[0];
            //if (dt.Columns.Contains("HASHNAME"))
            //{
            //    fileList.Clear();
            //    tableName = "FileData";
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        var hashName = dt.Rows[i].FieldEx<string>("HASHNAME");
            //        fileList.Add(hashName);
            //    }
            //    if (fileList.Count > 0)
            //    {
            //        using (DataServiceHelper dataser = new DataServiceHelper())
            //        {
            //            if (dataser.PushFileToA1(ModelHelper.SerializeObject(fileList)))
            //                pushMsg.AppendText("推送成功!\n");
            //            else
            //                pushMsg.AppendText("推送失败!\n");
            //        }
            //    }
            //}

            //dt.TableName = tableName;
            //var wellName = drv[0].Row.FieldEx<string>("WELL_JOB_NAME");
            //DataSet dsData = new DataSet();
            //dsData.Tables.Add(dt.Copy());
            //var strYear = checkClient.GetProjectYear(wellName);
            //var result = inputClient.DataImport(strYear, wellName, dsData);

            //JavaScriptSerializer js = new JavaScriptSerializer();
            //var joList = js.Deserialize<List<JsonObject>>(result);
            //foreach (var obj in joList)
            //{
            //    pushMsg.AppendText("代码:" + obj.code + "\n");
            //    var msg = obj.msg;
            //    if (msg.IndexOf("&") > -1)
            //        msg = msg.Replace("&", "\n");
            //    if (msg.IndexOf("<br/>") > -1)
            //        msg = msg.Replace("<br/>", "\n");
            //    pushMsg.AppendText("消息:" + msg + "\n");
            //}
        }
    }
}
