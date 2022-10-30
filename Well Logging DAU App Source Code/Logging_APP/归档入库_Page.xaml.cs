using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using WinFroms = System.Windows.Forms;
using LogDataFormatChangeLib;
using Logging_App.Utility;
using Microsoft.Win32;

namespace Logging_App
{
    /// <summary>
    /// 归档入库_Page.xaml 的交互逻辑
    /// </summary>
    public partial class 归档入库_Page : Page
    {
        //MyLib m_MyLib = new MyLib();
        public 归档入库_Page()
        {
            InitializeComponent();
        }

        #region

        private static readonly Regex reg = new Regex(@"\s|#", RegexOptions.Compiled);
        private void SetComboxDefaultSelect(DataTable dt, ComboBox cbox, DependencyObject colParent)
        {
            if (cbox.SelectedIndex < 0) return;
            var dr = dt.Rows[cbox.SelectedIndex];
            var cboxs = colParent.VisualSearchAllChild<ComboBox>();
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                if (dr[i] != null)
                    cboxs.FindAll(c => c.Tag.ToString().ToUpper().IndexOf("|" + reg.Replace(dr[i].ToString().ToUpper(), "") + "|") > 0)
                        .ForEach(c => c.SelectedIndex = i);
            }
        }

        private void BindComBox(DataView dv, DependencyObject rowParent, DependencyObject colParent)
        {
            if (dv != null)
            {
                Dictionary<int, int> sum_col = new Dictionary<int, int>();
                Dictionary<int, int> sum_row = new Dictionary<int, int>();
                for (int i = 1; i <= dv.Table.Columns.Count; i++) sum_col.Add(i, i);
                for (int i = 1; i <= 100 && i <= dv.Table.Rows.Count; i++) sum_row.Add(i, i);
                BindComBox(sum_row, rowParent);
                BindComBox(sum_col, colParent);
            }
        }

        private void BindComBox(Dictionary<int, int> dic, DependencyObject comboxParent)
        {
            comboxParent.VisualSearchAllChild<ComboBox>().ForEach(c =>
            {
                c.DisplayMemberPath = "Key";
                c.SelectedValuePath = "Value";
                c.ItemsSource = dic;
            });
        }

        private object FieldConvert(object obj)
        {
            string str = obj as string;
            if (string.IsNullOrEmpty(str))
                return DBNull.Value;
            else
                return obj;
        }

        private DataTable ReadDataTable(int? startRow, DataView dv, DependencyObject comboxParent)
        {
            if (startRow == null) throw new ArgumentNullException("数据开始行", "不能为空！");
            if (dv == null) throw new ArgumentNullException("数据", "不能为空！");
            var dt = dv.Table;
            Regex reg = new Regex("-+", RegexOptions.Compiled);
            var dic = new Dictionary<string, int[]>();
            var _dt = new DataTable();
            _dt.TableName = "data";
            foreach (var cbox in comboxParent.VisualSearchAllChild<ComboBox>().FindAll(c => c.Tag != null))
            {
                string tagStr = cbox.Tag.ToString();
                var names = tagStr.Split('|')[0].Split('-');
                int selectIndex = cbox.SelectedIndex;
                string[] tempArr = tagStr.Split(':');
                Type dataType = tempArr.Length == 2 ? Type.GetType(tempArr[1]) : typeof(String);
                if (names.Length > 1)
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (selectIndex > -1) dic.Add(names[i], new int[] { (int)selectIndex, i });
                        _dt.Columns.Add(new DataColumn(names[i], dataType));
                    }
                }
                else
                {
                    if (selectIndex > -1) dic.Add(names[0], new int[] { (int)selectIndex });
                    _dt.Columns.Add(new DataColumn(names[0], dataType));
                }
            }

            for (int i = (int)startRow - 1; i < dt.Rows.Count; i++)
            {
                var dr = _dt.NewRow();
                foreach (var k in dic)
                {
                    var str = dt.Rows[i].FieldEx<string>(k.Value[0]);
                    if (string.IsNullOrEmpty(str))
                        dr[k.Key] = DBNull.Value;
                    else
                    {
                        if (k.Value.Length > 1)
                        {
                            var v = reg.Split(str);
                            if (v.Length > k.Value[1])
                            {
                                dr[k.Key] = FieldConvert(v[k.Value[1]]);
                            }
                            else dr[k.Key] = str;
                        }
                        else
                        {
                            dr[k.Key] = str;
                        }
                    }
                }
                _dt.Rows.Add(dr);
            }
            return _dt;
        }

        private DataTable ReadLayerDataTable(int? startRow, DataView dv, DependencyObject comboxParent)
        {
            if (startRow == null) throw new ArgumentNullException("数据开始行", "不能为空！");
            if (dv == null) throw new ArgumentNullException("数据", "不能为空！");
            var dt = dv.Table;
            Regex reg = new Regex("-+", RegexOptions.Compiled);
            var dic = new Dictionary<string, int[]>();
            var _dt = new DataTable();
            _dt.TableName = "data";
            foreach (var cbox in comboxParent.VisualSearchAllChild<System.Windows.Controls.ComboBox>().FindAll(c => c.Tag != null))
            {
                string tagStr = cbox.Tag.ToString();
                var names = tagStr.Split('|')[0].Split('-');
                int selectIndex = cbox.SelectedIndex;
                string[] tempArr = tagStr.Split(':');
                Type dataType = tempArr.Length == 2 ? Type.GetType(tempArr[1]) : typeof(String);
                if (names.Length > 1)
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (selectIndex > -1) dic.Add(names[i], new int[] { (int)selectIndex, i });
                        _dt.Columns.Add(new DataColumn(names[i], dataType));
                    }
                }
                else
                {
                    if (selectIndex > -1) dic.Add(names[0], new int[] { (int)selectIndex });
                    _dt.Columns.Add(new DataColumn(names[0], dataType));
                }
            }

            for (int i = startRow.Value - 1; i < dt.Rows.Count; i++)
            {
                var dr = _dt.NewRow();
                foreach (var k in dic)
                {
                    if (k.Key == "ST_DEVIATED_DEP")
                    {
                        if (i >= startRow.Value)
                            dr[k.Key] = FieldConvert(dt.Rows[i - 1][k.Value[0]]);
                        continue;
                    }
                    if (k.Key == "EN_DEVIATED_DEP")
                    {
                        //if (i < dt.Rows.Count - 1)
                        //    dr[k.Key] = FieldConvert(dt.Rows[i + 1][k.Value[0]]);
                        //else
                        dr[k.Key] = FieldConvert(dt.Rows[i][k.Value[0]]);
                        continue;
                    }
                    if (k.Value.Length > 1)
                    {
                        var str = dt.Rows[i].FieldEx<string>(k.Value[0]);
                        var v = reg.Split(str);
                        if (v.Length > k.Value[1])
                        {
                            dr[k.Key] = FieldConvert(v[k.Value[1]]);
                        }
                        else dr[k.Key] = FieldConvert(dt.Rows[i][k.Value[0]]);
                    }
                    else
                    {
                        dr[k.Key] = FieldConvert(dt.Rows[i][k.Value[0]]);
                    }
                }
                _dt.Rows.Add(dr);
            }
            return _dt;
        }
        /**
        private Utility.DataCollection<T> DataTableToModel<T>(DataTable dt, DependencyObject comboxParent) where T : Model.ModelBase, new()
        {
            Regex reg = new Regex("-+", RegexOptions.Compiled);
            var cboxList = new List<System.Windows.Controls.ComboBox>();
            VisualHelper.VisualSearchAllChild<System.Windows.Controls.ComboBox>(cboxList, comboxParent);
            var dic = new Dictionary<string, int[]>();
            var modelList = new Utility.DataCollection<T>();
            Type t = typeof(T);
            foreach (var cbox in cboxList)
            {
                var names = cbox.Name.Split('和');
                int selectIndex = cbox.SelectedValue == null ? -1 : cbox.SelectedIndex;
                if (selectIndex < 1) continue;
                if (names.Length > 2)
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (t.GetProperty(names[i]) != null)
                            dic.Add(names[i], new int[] { (int)selectIndex, i });
                    }
                }
                else
                {
                    dic.Add(names[0], new int[] { (int)selectIndex });
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                foreach (var k in dic)
                {
                    PropertyInfo propertyInfo = t.GetType().GetProperty(k.Key);
                    if (propertyInfo != null && dr[k.Value[0]] != DBNull.Value)
                    {
                        T m = new T();
                        if (k.Value.Length > 1)
                        {
                            var str = dr.FieldEx<string>(k.Value[0]);
                            var v = reg.Split(str);
                            if (v.Length <= k.Value[1])
                            {
                                propertyInfo.SetValue(t, v[k.Value[1]], null);
                            }
                        }
                        else
                        {
                            propertyInfo.SetValue(t, dr[k.Value[0]], null);
                        }
                        modelList.Add(m);
                    }
                }
            }
            return modelList;
        }
         */
        #endregion
        #region
        //
        //"输出综合成果解释数据表";
        //
        private void Load_tabItem1(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(textBox2.Text.ToString() + "\\com\\data\\"))
            {
                //m_MyLib.DelDirAllTesFile(textBox2.Text.ToString() + "\\com\\data\\", "~$");
                //string m_ProcessFileName2 = textBox2.Text.ToString() + "\\com\\data\\" +
                //m_MyLib.GetDirFileName(textBox2.Text.ToString() + "\\com\\data\\", "Result.List");
                //textBox1.Text = m_ProcessFileName2;
                //m_MyLib.ListToDataGridView(dataGrid2, m_ProcessFileName2);
            }
        }
        //
        //固井质量评价数据
        //
        private void Load_I2(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(textBox2.Text.ToString() + "\\vdl\\data\\"))
            {
                //m_MyLib.DelDirAllTesFile(textBox2.Text.ToString() + "\\vdl\\data\\", "~$");
                //string m_ProcessFileName2 = textBox2.Text.ToString() + "\\vdl\\data\\" +
                //      m_MyLib.GetDirFileName(textBox2.Text.ToString() + "\\vdl\\data", "CBL3-S-L.List");
                //textBox5.Text = m_ProcessFileName2;
                //m_MyLib.ListToDataGridView(dataGrid3, m_ProcessFileName2);
            }
        }
        //
        //测量井斜数据
        //
        private void Load_I3(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(textBox2.Text.ToString() + "\\bg"))
            {
                //m_MyLib.DelDirAllTesFile(textBox2.Text.ToString() + "\\bg", "~$");
                //string m_ProcessFileName2 = textBox2.Text.ToString() + "\\bg\\" +
                //      m_MyLib.GetDirFileName(textBox2.Text.ToString() + "\\bg\\", "井斜数据");
                //textBox7.Text = m_ProcessFileName2;
                //m_MyLib.CurveToDataGridView(dataGrid3, m_ProcessFileName2, 1);
            }
        }
        //
        //获取原始数据
        //
        private void Load_Item9(object sender, RoutedEventArgs e)
        {
            string[] m_H = { "文件名", "文件大小", "文件类型", "备注" };
            if (Directory.Exists(textBox2.Text.ToString()))
            {

                //DataGrid9.AutoGenerateColumns = true;
                //System.Data.DataTable dt = new System.Data.DataTable();
                //dt.Clear();
                ////DataGrid9.ItemsSource = dt.GetDefaultView();
                //for (int i = 0; i < 4; i++) dt.Columns.Add(new DataColumn(m_H[i]));
                //m_MyLib.NewDt = dt;
                //m_MyLib.AddFileToDataGridView(textBox2.Text.ToString(), "\\yssj");
                //DataGrid9.ItemsSource = m_MyLib.NewDt.GetDefaultView();
            }

        }

        //
        //获取井眼轨迹数据
        //
        private void tabItem6_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(textBox2.Text.ToString() + "\\bg"))
            {
                //m_MyLib.DelDirAllTesFile(textBox2.Text.ToString() + "\\bg", "~$");
                //string m_ProcessFileName2 = textBox2.Text.ToString() + "\\bg\\" +
                //      m_MyLib.GetDirFileName(textBox2.Text.ToString() + "\\bg", "井眼轨迹数据");
                //textBox6_1.Text = m_ProcessFileName2;
                // if (m_MyLib.GetFileNameSuffix(m_ProcessFileName2) == "XLS")
                //  m_MyLib.xlsToDataGridView(tabItem6_Grid, m_ProcessFileName2);
                //if (m_MyLib.GetFileNameSuffix(m_ProcessFileName2) == "TXT")
                //     m_MyLib.CurveToDataGridView(tabItem6_Grid, m_ProcessFileName2, 2);
            }
        }
        //
        //
        //
        private void Load_I4(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(textBox2.Text.ToString() + "\\com\\data\\"))
            {
                //m_MyLib.DelDirAllTesFile(textBox2.Text.ToString() + "\\com\\data\\", "~$");
                //string m_ProcessFileName2 = textBox2.Text.ToString() + "\\com\\data\\" +
                //      m_MyLib.GetDirFileName(textBox2.Text.ToString() + "\\com\\data\\", ".TXT");
                //textBox9.Text = m_ProcessFileName2;
                //m_MyLib.CurveToDataGridView(dataGrid5, m_ProcessFileName2, 7);
            }

        }
        //
        //
        //
        private void tabItem5_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(textBox2.Text.ToString() + "\\com\\data\\"))
            {
                //m_MyLib.DelDirAllTesFile(textBox2.Text.ToString() + "\\com\\data\\", "~$");
                //string m_ProcessFileName2 = textBox2.Text.ToString() + "\\com\\data\\" +
                //      m_MyLib.GetDirFileName(textBox2.Text.ToString() + "\\com\\data\\", ".TXT");
                //textBox9.Text = m_ProcessFileName2;
                //m_MyLib.CurveToDataGridView(dataGrid5, m_ProcessFileName2, 7);
            }

        }
        #endregion
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var m_DirDlg = new WinFroms.FolderBrowserDialog();
            m_DirDlg.SelectedPath = textBox2.Text;
            if (m_DirDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clearControls(tabControl1 as DependencyObject);

                FileInfo[] fInfo;
                textBox2.Text = m_DirDlg.SelectedPath;
                FileHelper.FileDelete(m_DirDlg.SelectedPath, "~", "$");

                #region 获取固井质量评价文件
                fInfo = FileHelper.GetFiles(m_DirDlg.SelectedPath + "\\vdl\\data", "*CBL3-S-L.List");
                if (fInfo != null && fInfo.Length > 0)
                    textBox5.Text = fInfo[0].FullName;
                #endregion

                #region 获取测量井斜文件
                fInfo = FileHelper.GetFiles(m_DirDlg.SelectedPath + "\\bg", "*井斜数据*.txt", "*井斜数据*.xls", "*井斜数据*.xlsx");
                if (fInfo != null && fInfo.Length > 0)
                    textBox7.Text = fInfo[0].FullName;
                #endregion

                #region 获取综合解释成果曲线文件
                fInfo = FileHelper.GetFiles(m_DirDlg.SelectedPath + "\\com\\data", "*.txt");
                if (fInfo != null && fInfo.Length > 0)
                {
                    textBox9.Text = fInfo[0].FullName;
                    if (fInfo.Length > 1)
                        MessageBox.Show(App.Current.MainWindow, "综合解释成果曲线数据有多个文本!", "提示");
                }
                #endregion

                #region 获取测井分层文件
                fInfo = FileHelper.GetFiles(m_DirDlg.SelectedPath + "\\com\\data", "*Layer.List");
                if (fInfo != null && fInfo.Length > 0)
                    textBox7_1.Text = fInfo[0].FullName;
                #endregion

                #region 获取井眼轨迹数据文件
                fInfo = FileHelper.GetFiles(m_DirDlg.SelectedPath + "\\bg", "*井眼轨迹*.txt", "*井眼轨迹*.xls", "*井眼轨迹*.xlsx");
                if (fInfo != null && fInfo.Length > 0)
                    textBox6_1.Text = fInfo[0].FullName;
                #endregion

                #region 载人测井图件数据成果人库文件
                IEnumerable<FileInfo> fileInfoList = FileHelper.GetDirFiles(m_DirDlg.SelectedPath, false,
                    s => !"bg,qt,yssj".Split(',').Contains(s),
                    s => s == "data");
                var dic = new Dictionary<string, string>();
                foreach (var f in fileInfoList)
                {
                    dic.Add(f.FullName, f.Name);
                }
                listBox1.DisplayMemberPath = "Value";
                listBox1.SelectedValuePath = "Key";
                listBox1.ItemsSource = dic;
                #endregion

                #region 获取成果大块数据
                FileToDataGrid(dataGrid8, fileInfoList);
                #endregion

                #region 获取原始大块数据
                fileInfoList = FileHelper.GetDirFiles(m_DirDlg.SelectedPath, false,
                    s => s == "yssj");
                FileToDataGrid(dataGrid9, fileInfoList);
                #endregion

                #region 获取成果文档
                fileInfoList = FileHelper.GetDirFiles(m_DirDlg.SelectedPath, false,
                    s => s == "bg");
                FileToDataGrid(dataGrid10, fileInfoList);
                #endregion

                #region 获取成果图片
                fileInfoList = FileHelper.GetDirFiles(m_DirDlg.SelectedPath, false,
                    s => !"bg,qt,yssj".Split(',').Contains(s),
                    s => "head,map".Split(',').Contains(s));
                FileToDataGrid(dataGrid11, fileInfoList);
                #endregion

                #region 获取其它文件
                fileInfoList = FileHelper.GetDirFiles(m_DirDlg.SelectedPath, false,
                    s => s == "qt");
                FileToDataGrid(dataGrid12, fileInfoList);
                #endregion
                ListBox2Data.ForEach(o => o.Foreground = Brushes.Black);
            }
        }

        private void FileToDataGrid(DataGrid dg, IEnumerable<FileInfo> fileInfoList)
        {
            string[] cols = { "FILENAME", "LENGTH", "EXTENSION", "FULLNAME", "PROCESSING_ITEM_ID" };
            if (dg.Name == "dataGrid10")
                cols = cols.Concat(new string[] { "A1_GUID", "DOCUMENT_TYPE", "DOCUMENT_WRITER", "DOCUMENT_VERIFIER", "DOCUMENT_COMPLETION_DATE" }).ToArray();
            DataTable dt = new DataTable();
            foreach (string col in cols)
            {
                dt.Columns.Add(col);
            }
            string dirName = string.Empty;
            if (dg.Name != "dataGrid8" && dg.Name != "dataGrid11")
                dirName = new DirectoryInfo(textBox2.Text).Name.ToLower();
            foreach (FileInfo fInfo in fileInfoList)
            {
                decimal? itemID = null;
                string tempDirName = dirName;
                if (tempDirName == string.Empty)
                    tempDirName = fInfo.DirectoryName.Remove(0, textBox2.Text.Length).Trim('\\').Split('\\')[0].ToLower();
                if (string.IsNullOrWhiteSpace(tempDirName)) break;
                foreach (DataRow dr in process_code.Table.Rows)
                {
                    foreach (string code in dr.FieldEx<string>("PROCESSING_ITEM_CODE").Split(';'))
                    {
                        if ((dirName == string.Empty && tempDirName == code.ToLower()) || tempDirName.IndexOf("#" + code.ToLower() + "_") > -1)
                        {
                            itemID = dr.FieldEx<decimal>("PROCESSING_ITEM_ID");
                            break;
                        }
                    }
                    if (itemID != null) break;
                }
                dt.Rows.Add(fInfo.Name, fInfo.Length, string.IsNullOrEmpty(fInfo.Extension) ? "" : fInfo.Extension.TrimStart('.').ToUpper(), fInfo.FullName, itemID);
            }
            dg.ItemsSource = dt.GetDefaultView();
        }

        //
        //文件导入综合成果解释数据
        //
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var m_MyDlg = new WinFroms.OpenFileDialog();
            m_MyDlg.Filter = "综合成果解释数据(*Result.List)|*Result.List";
            m_MyDlg.FileName = textBox1.Text;
            if (m_MyDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clearControls(sp1 as DependencyObject);
                textBox1.Text = m_MyDlg.FileName;
            }
        }

        //
        //文件导入页岩气解释成果数据
        //
        private void button12_Click(object sender, RoutedEventArgs e)
        {
            var m_MyDlg = new WinFroms.OpenFileDialog();
            m_MyDlg.Filter = "页岩气解释成果数据(*Result.List)|*Result.List";
            m_MyDlg.FileName = textBox12.Text;
            if (m_MyDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clearControls(sp7 as DependencyObject);
                textBox12.Text = m_MyDlg.FileName;
            }
        }

        //
        //文件导入固井质量评价数据
        //
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            var m_MyDlg = new WinFroms.OpenFileDialog();
            m_MyDlg.Filter = "固井质量评价数据(*CBL3-S-L.List)|*CBL3-S-L.List";
            m_MyDlg.FileName = textBox5.Text;
            if (m_MyDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clearControls(sp2 as DependencyObject);
                textBox5.Text = m_MyDlg.FileName;
            }
        }

        /// <summary>
        /// 文件导入测量井斜数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            var m_MyDlg = new WinFroms.OpenFileDialog();
            m_MyDlg.Filter = "井斜数据(.txt;.xls;.xlsx)|*井斜数据*.txt;*井斜数据*.xls;*井斜数据*.xlsx";
            m_MyDlg.FileName = textBox7.Text;
            if (m_MyDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clearControls(sp3 as DependencyObject);
                textBox7.Text = m_MyDlg.FileName;
            }
        }

        /// <summary>
        /// 井眼轨迹数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_1_Click(object sender, RoutedEventArgs e)
        {
            var m_MyDlg = new WinFroms.OpenFileDialog();
            m_MyDlg.Filter = "井眼轨迹数据(.txt;.xls;.xlsx)|*井眼轨迹*.txt;*井眼轨迹*.xls;*井眼轨迹*.xlsx";
            m_MyDlg.FileName = textBox6_1.Text;
            if (m_MyDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clearControls(sp6 as DependencyObject);
                textBox6_1.Text = m_MyDlg.FileName;
            }
        }
        /// <summary>
        /// 测井分层数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_1_Click(object sender, RoutedEventArgs e)
        {
            var m_MyDlg = new WinFroms.OpenFileDialog();
            m_MyDlg.Filter = "Layer.List(*Layer.List)|*Layer.List";
            m_MyDlg.FileName = textBox7_1.Text;
            if (m_MyDlg.ShowDialog() == WinFroms.DialogResult.OK)
            {
                clearControls(sp5 as DependencyObject);
                textBox7_1.Text = m_MyDlg.FileName;
            }
        }
        //
        //文件导入综合解释成果曲线数据
        //
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            var m_MyDlg = new WinFroms.OpenFileDialog();
            m_MyDlg.Filter = "综合解释成果曲线数据(*.txt;)|*.txt";
            m_MyDlg.FileName = textBox9.Text;
            if (m_MyDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clearControls(sp4 as DependencyObject);
                textBox9.Text = m_MyDlg.FileName;
            }
        }

        #region bak
        //
        //成果图片
        //
        private void tabItem11_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string[] m_H = { "文件名", "文件大小", "文件类型", "备注" };
            if (Directory.Exists(textBox2.Text.ToString()))
            {

                //DataGrid_tabItem11.AutoGenerateColumns = true;
                //System.Data.DataTable dt = new System.Data.DataTable();
                //dt.Clear();
                ////DataGrid_tabItem11.ItemsSource = dt.GetDefaultView();
                //for (int i = 0; i < 4; i++) dt.Columns.Add(new DataColumn(m_H[i]));
                //m_MyLib.NewDt = dt;
                //m_MyLib.AddFileToDataGridView(textBox2.Text.ToString(), "\\map");
                //DataGrid_tabItem11.ItemsSource = m_MyLib.NewDt.GetDefaultView();
            }
        }
        //
        //其它
        //
        private void tabItem12_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string[] m_H = { "文件名", "文件大小", "文件类型", "备注" };
            if (Directory.Exists(textBox2.Text.ToString()))
            {

                //DataGrid_tabItem12.AutoGenerateColumns = true;
                //System.Data.DataTable dt = new System.Data.DataTable();
                //dt.Clear();
                ////DataGrid_tabItem12.ItemsSource = dt.GetDefaultView();
                //for (int i = 0; i < 4; i++) dt.Columns.Add(new DataColumn(m_H[i]));
                //m_MyLib.NewDt = dt;
                //m_MyLib.AddFileToDataGridView(textBox2.Text.ToString(), "\\qt");
                //DataGrid_tabItem12.ItemsSource = m_MyLib.NewDt.GetDefaultView();
            }
        }
        //
        //成果大块文件
        //
        private void tabItem8_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string[] m_H = { "文件名", "文件大小", "文件类型", "备注" };
            if (Directory.Exists(textBox2.Text.ToString()))
            {

                //DataGrid_tabItem8.AutoGenerateColumns = true;
                //System.Data.DataTable dt = new System.Data.DataTable();
                //dt.Clear();
                ////DataGrid_tabItem8.ItemsSource = dt.GetDefaultView();
                //for (int i = 0; i < 4; i++) dt.Columns.Add(new DataColumn(m_H[i]));
                //m_MyLib.NewDt = dt;
                //m_MyLib.AddFileToDataGridView(textBox2.Text.ToString(), "\\qt");
                //DataGrid_tabItem8.ItemsSource = m_MyLib.NewDt.GetDefaultView();
            }
        }

        private void tabItem10_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string[] m_H = { "文件名", "文件大小", "文件类型", "备注" };
            if (Directory.Exists(textBox2.Text.ToString()))
            {

                //DataGrid_tabItem10.AutoGenerateColumns = true;
                //System.Data.DataTable dt = new System.Data.DataTable();
                //dt.Clear();
                ////DataGrid_tabItem10.ItemsSource = dt.GetDefaultView();
                //for (int i = 0; i < 4; i++) dt.Columns.Add(new DataColumn(m_H[i]));
                //m_MyLib.NewDt = dt;
                //m_MyLib.AddFileToDataGridView(textBox2.Text.ToString(), "\\bg");
                //DataGrid_tabItem10.ItemsSource = m_MyLib.NewDt.GetDefaultView();
            }
        }
        //
        //测井图件数据
        //
        private void tabItem7_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Directory.Exists(textBox2.Text.ToString()))
            {
                //listBox1.Items.Clear();
                comboBox17.Items.Clear();
                //m_MyLib.AddFileToListBox(listBox1, textBox2.Text.ToString(), "\\data");
                comboBox17.Items.Add("综合处理");
                comboBox17.Items.Add("固井处理");
                comboBox17.Items.Add("成像处理");
                comboBox17.Items.Add("核磁处理");

            }
        }
        //
        //鼠标双击选择入库成果文件
        //
        //string m_FileName; //后缀为.exe的文件，如"#磨溪12-121209.exe"
        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //m_FileName = listBox1.SelectedItem.ToString();
            //m_MyLib.GetFullDirName(textBox11, textBox2.Text.ToString(), m_FileName);
        }
        //
        //选择组合框中处理解释项目
        //
        [DllImport("AutoReadFid.dll", EntryPoint = "ReadFidExe", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReadFidExe(string m_FileName, string m_DirName);
        private void comboBox17_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem_MouseDoubleClick(null, null);
            //
            //获取#磨溪12-121209.exe的路径，如"E:\\jrx\\测试数据\\磨溪12_q\\moxi12#ZH_20121209(503.14-3249)\\com\\data"
            //
            //string m_DirName = m_MyLib.GetAllDirFileName(textBox2.Text, m_FileName);
            //
            //获取"#磨溪12-121209.exe"的全称，如"E:\\测试数据\\磨溪12_q\\moxi12#ZH_20121209(503.14-3249)\\com\\data\\#磨溪12-121209.exe"
            //
            //string m_FullFileName = m_DirName + "\\" + m_FileName;
            //
            //生成新的FID路径
            //
            //string m_ExDirName = m_DirName + "\\" + m_FileName.Remove(m_FileName.IndexOf('.'));
            //if (!Directory.Exists(m_ExDirName)) ReadFidExe(m_FullFileName, m_DirName);

            // tabItem7_DataGrid.Items.Clear();
            // string[] m_Fid_CurveName = m_MyLib.GetFid_CurveName(m_ExDirName);
            // int m_CurveNum = m_MyLib.GetFid_CurveNum(m_ExDirName);
            // string[] m_Tmp = { "aa", "bb" };
            // for (int i = 0; i < 10; i++) tabItem7_DataGrid.Items.Add(m_Tmp);
        }
        #endregion

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        //private bool SetWorkFlow(string target_loginname, int state)
        //{
        //    using (var dataser = new DataServiceHelper())
        //    {
        //        var row = dataGrid1.SelectedItem as DataRowView;
        //        if (dataser.SetWorkFlow(row.Row.FieldEx<string>("PROCESS_ID"), target_loginname, DataService.WorkFlowType.解释处理作业, state))
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
                var row = dataGrid1.SelectedItem as DataRowView;
                workFlowControl1.SubmitReview(w.LoginName, row.Row.FieldEx<string>("PROCESS_ID"));
            }

        }

        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            //var flowData = workFlowControl1.GetData();
            //var result = MessageBox.Show("是否通过审核？", "解释处理作业审核", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            //int state = -1;
            //if (result == MessageBoxResult.Yes) state = (int)Model.SYS_Enums.FlowState.审核通过;
            //if (result == MessageBoxResult.No) state = (int)Model.SYS_Enums.FlowState.审核未通过;
            //if (state > -1 && SetWorkFlow(flowData[flowData.Length - 1].SOURCE_LOGINNAME, state))
            //{
            //    MessageBox.Show("审核成功！");
            //}
            var row = dataGrid1.SelectedItem as DataRowView;
            workFlowControl1.Review("解释处理作业审核", row.Row.FieldEx<string>("PROCESS_ID"));
        }

        private void Menu3Item_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            workFlowControl1.CancelSubmitReview("小队施工信息审核取消提交审核", row.Row.FieldEx<string>("PROCESS_ID"));
        }

        private void Menu4Item_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            workFlowControl1.CancelReview("小队施工信息退回审核", row.Row.FieldEx<string>("PROCESS_ID"));
        }

        private DataView process_code = null;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ListBox2Data == null) ListBox2Data = CreateListBox2Data();
            //tabItem1.Tag = "综合";//综合解释成果数据
            //tabItem2.Tag = "固井";//固井质量评价数据
            //tabItem3.Tag = "综合";//测量井斜数据
            //tabItem4.Tag = "综合";//综合解释成果曲线数据
            //tabItem5.Tag = "综合";//测井分层数据
            //tabItem6.Tag = "综合";//井眼轨迹数据
            //tabItem7.Tag = "综合;固井";//测井图件数据
            //tabItem8.Tag = "综合;固井;页岩气;生产";//成果大块文件
            //tabItem9.Tag = "综合;固井;页岩气;生产";//原始大块数据
            //tabItem10.Tag = "综合;固井;页岩气;生产";//成果文档s
            //tabItem11.Tag = "综合;固井;页岩气;生产";//成果图片
            //tabItem12.Tag = "综合;固井;页岩气;生产";//其它
            listBox2.ItemsSource = ListBox2Data;
            //workFlowControl1.ClearData();
            stackPanel.Visibility = Visibility.Collapsed;

            using (var dataser = new DataServiceHelper())
            {
                viewExtensions = dataser.GetFileViewExtensions();
                process_code = dataser.GetData_解释处理项目编码().Tables[0].GetDefaultView();
                var dt = dataser.GetData_图件类型().Tables[0];
                var dr = dt.NewRow();
                dr[0] = "全部";
                dt.Rows.InsertAt(dr, 0);
                comboBox17.ItemsSource = dt.GetDefaultView();
                // comboBox17.ItemsSource = dataser.GetData_图件类型().Tables[0].GetDefaultView();
                var archiveItemCodes = dataser.GetComboBoxList_ArchiveItemCodes().Tables[0];
                foreach (DataRow row in archiveItemCodes.Rows)
                {
                    comboBox.Items.Add(new ComboBoxItem { Content = row["ITEM_NAME"], Tag = row["ARCHIVE_NAME"] });
                }
                DataView queryDate = dataser.GetComboxList_QueryDate().Tables[0].GetDefaultView();
                QUERY_DATE.DisplayMemberPath = "QUREY_DATE";
                QUERY_DATE.SelectedValuePath = "DATE_VALUE";
                QUERY_DATE.ItemsSource = queryDate;
                QUERY_DATE.Text = "最近一年";
            }
            LoadDataList(1);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            LoadDataList(1);
        }

        public string Process_Name = string.Empty;
        private void LoadDataList(int page)
        {
            using (var dataser = new DataServiceHelper())
            {
                int total = 0;
                dataGrid1.ItemsSource = dataser.GetDataList_解释处理作业(ModelHelper.SerializeObject(SearchPanel.DataContext), page, out total).Tables[0].GetDefaultView();
                //Counts.Content = "      归档项目：" + total;
                dataPager2.TotalCount = total;
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

        bool canViewFile = false;
        private void ChangeMenuState()
        {
            //save1.IsEnabled = false;
            //menu1.IsEnabled = false;
            //menu2.IsEnabled = false;
            canViewFile = false;
            var flowData = workFlowControl1.GetData();
            ListBox2Data.ForEach(o => o.Foreground = Brushes.Black);
            if (flowData != null && flowData.Length > 0)
            {
                //    if (flowData[flowData.Count - 1].SOURCE_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME && (flowData.Count == 1 || flowData[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.审核未通过))
                //    {
                //        save1.IsEnabled = true;
                //        menu1.IsEnabled = true;
                //    }
                //    if (flowData[0].TARGET_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME && flowData[0].FLOW_STATE == (int)Model.SYS_Enums.FlowState.提交审核)
                //        menu2.IsEnabled = true;
                if (flowData[flowData.Length - 1].SOURCE_LOGINNAME == MyHomePage.ActiveUser.COL_LOGINNAME)
                    canViewFile = true;
            }
            if (!canViewFile)
            {
                using (var userser = new UserServiceHelper())
                {
                    var roles = userser.GetActiveUserRoles();
                    if (roles.Contains(UserService.UserRole.系统管理员) || roles.Contains(UserService.UserRole.调度管理员) || roles.Contains(UserService.UserRole.解释管理员))
                        canViewFile = true;
                }
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1 || !e.AddedItems[0].Equals(dataGrid1.SelectedItem)) return;
            var row = dataGrid1.SelectedItem as DataRowView;
            if (row == null)
            {
                stackPanel.Visibility = Visibility.Collapsed;
                return;
            }
            curveDataUploadID = null;
            curveDataComboBoxsSetting = null;
            workFlowControl1.LoadData(row.Row.FieldEx<string>("PROCESS_ID"));
            textBox2.Clear();
            //initCurveVariables();
            if (listBox2.SelectedIndex == -1)
                listBox2.SelectedIndex = 0;
            else
                ExecTabItemLoadData((tabControl1.SelectedItem as TabItem).Name);
            stackPanel.Visibility = Visibility.Visible;

            ChangeMenuState();
        }

        public Dictionary<string, string> curveNameDic;
        private string analyticType = null;
        private string path = null;
        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            curveNameDic = new Dictionary<string, string>();
            var lbValue = listBox1.SelectedValue as string;
            textBox11.Text = lbValue;
            //var item_id = comboBox17.SelectedValue as decimal?;
            var map_chinese_name = comboBox17.SelectedValue as string;
            DataTable curveNames = null;
            var data = new DataCollection<Model.PRO_LOG_PROCESSING_CURVE_INDEX>();
            if (map_chinese_name != null && lbValue != null)
            {
                var extension = Path.GetExtension(lbValue).ToLower();
                using (var dataser = new DataServiceHelper()) curveNames = dataser.GetMapCurveInfo(map_chinese_name).Tables[0];

                if (curveNames != null && curveNames.Rows.Count > 0)
                {
                    path = FileHelper.ExtractFidPack(lbValue, e == null ? false : true);
                    if (!string.IsNullOrEmpty(path))
                    {
                        foreach (DataRow row in curveNames.Rows)
                        {
                            data.Add(new Model.PRO_LOG_PROCESSING_CURVE_INDEX
                            {
                                CURVE_CD = row.FieldEx<string>("CURVE_CD"),
                                CURVE_NAME = row.FieldEx<string>("CURVE_NAME"),
                                PROCESSING_ITEM_ID = row.FieldEx<decimal?>("PROCESSING_ITEM_ID"),
                                MAPS_CODING = row.FieldEx<string>("MAPS_CHINESE_NAME")
                            });
                        }
                        switch (extension)
                        {
                            case ".exe":
                                foreach (var f in FileHelper.GetDirFiles(path, true).FindAll(f => f.Name.ToUpper().IndexOf("I") == 0 && !string.IsNullOrEmpty(f.Extension)))
                                {
                                    curveNameDic.Add(f.Extension.TrimStart('.'), f.FullName);
                                }
                                analyticType = "exe";
                                break;
                            case ".xtf":
                                XtfFormat mm = new XtfFormat();
                                mm.m_FileName = lbValue;
                                mm.OpenXtfFile();
                                var m_CurveName = mm.GetFileCurveIn();
                                mm.m_CloseXtf();
                                foreach (var name in m_CurveName)
                                {
                                    curveNameDic.Add(name, lbValue);
                                }
                                analyticType = "xtf";
                                break;
                            default:
                                break;
                        }
                    }
                }
                curveNameCol.Visibility = Visibility.Collapsed;
                curveNameCol1.Visibility = Visibility.Visible;
                dataGrid7.ItemsSource = data;
            }
            else
            {
                var dv = dataGrid7.ItemsSource as DataView;
                if (dv != null)
                {
                    if (comboBox17.SelectedIndex < 1)
                        dv.RowFilter = null;
                    else
                        dv.RowFilter = "MAPS_CODING='" + map_chinese_name + "'";
                }
            }
        }

        public void OutFile(string m_FileName, byte[] bytes)
        {
            using (var fs = new FileStream(m_FileName, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        public void OutFile(string m_FileName, Array m_Buffer, float m_Dep, float m_Rlev, int m_Num)
        {
            string sign = " "; //元素之间分隔符号，此处设置为空格，可自行更改设置
            StreamWriter sw = new StreamWriter(m_FileName, true); //第一个参数是读取到流的文件
            int kk = 0;
            for (int i = 0; i < m_Buffer.Length - m_Num; i = i + m_Num)
            {
                string mm = (m_Dep + kk * m_Rlev).ToString();
                for (int j = 0; j < m_Num; j++) mm = mm + sign + m_Buffer.GetValue(i + j).ToString() + sign;
                sw.Write(mm);
                sw.WriteLine();
                kk++;
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        private void curveNameCbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbox = sender as System.Windows.Controls.ComboBox;
            var model = dataGrid7.SelectedItem as Model.PRO_LOG_PROCESSING_CURVE_INDEX;
            if (model == null)
                model = ((DataGridRow)((DependencyObject)cbox).VisualSearchParent<DataGridRow>()).Item as Model.PRO_LOG_PROCESSING_CURVE_INDEX;
            switch (analyticType)
            {
                case "exe":
                    string filepath = ((KeyValuePair<string, string>)cbox.SelectedItem).Value;
                    var dic = FileParser.ReadCurveInfo(filepath);
                    string curvedatapath = filepath.Substring(0, filepath.LastIndexOf("\\") + 1) + "D" + Path.GetFileName(filepath).Remove(0, 1);
                    var fInfo = new FileInfo(curvedatapath);
                    if (fInfo.Exists)
                    {
                        model.DATA_SIZE = fInfo.Length;
                        model.DATA_PATH = curvedatapath;
                    }
                    else
                    {
                        model.DATA_SIZE = 0;
                        model.DATA_PATH = null;
                    }
                    Type propertyType;
                    if (dic != null)
                        foreach (var k in dic)
                        {
                            PropertyInfo propertyInfo = model.GetType().GetProperty(k.Key);
                            if (propertyInfo != null)
                            {
                                propertyType = propertyInfo.PropertyType;
                                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    propertyType = propertyType.GetGenericArguments()[0];
                                propertyInfo.SetValue(model, Convert.ChangeType(k.Value, propertyType), null);
                            }
                        }
                    break;
                case "xtf":
                    var curveName = ((KeyValuePair<string, string>)cbox.SelectedItem).Key;
                    var filePath = ((KeyValuePair<string, string>)cbox.SelectedItem).Value;
                    var fi = new FileInfo(filePath);
                    if (fi.Exists)
                    {
                        XtfFormat xtf = new XtfFormat();
                        xtf.m_FileName = filePath;
                        xtf.OpenXtfFile();
                        var m_CurveIn = xtf.GetCurveIn(curveName);
                        model.CURVE_START_DEP = Convert.ToDecimal(m_CurveIn.curve_start_dep);
                        model.CURVE_END_DEP = Convert.ToDecimal(m_CurveIn.curve_end_dep);
                        model.CURVE_UNIT = new string(m_CurveIn.curve_unit);
                        model.CURVE_RLEV = Convert.ToDecimal(m_CurveIn.curve_rlev);
                        model.CURVE_T_SAMPLE = m_CurveIn.m_CurveD1Num;
                        model.CURVE_T_UNIT = new string(m_CurveIn.curve_t_unit);
                        model.CURVE_T_MAX_VALUE = Convert.ToDecimal(m_CurveIn.curve_t_max_value);
                        model.CURVE_T_MIN_VALUE = Convert.ToDecimal(m_CurveIn.curve_t_min_value);
                        model.CURVE_T_RELV = Convert.ToDecimal(m_CurveIn.curve_t_relv);
                        model.DATA_INFO = m_CurveIn.m_CurveDim;
                        var tempPath = Path.Combine(path.Substring(0, path.LastIndexOf("\\")), fi.Name + "." + curveName);
                        model.DATA_PATH = tempPath;

                        Array array = null;
                        byte[] buffer = null;
                        switch (m_CurveIn.curve_data_type[0])
                        {
                            case '4':
                                array = xtf.GetFloatCurveData(m_CurveIn.curve_name, m_CurveIn.curve_start_dep);
                                buffer = new byte[4 * array.Length];
                                for (int i = 0; i < array.Length; i++)
                                {
                                    byte[] _byte = BitConverter.GetBytes(Convert.ToSingle(array.GetValue(i)));
                                    _byte.CopyTo(buffer, i * 4);
                                }
                                model.CURVE_DATA_TYPE = "float";
                                break;
                            case '2':
                                array = xtf.GetInt16CurveData(m_CurveIn.curve_name, m_CurveIn.curve_start_dep);
                                buffer = new byte[4 * array.Length];
                                for (int i = 0; i < array.Length; i++)
                                {
                                    byte[] _byte = BitConverter.GetBytes(Convert.ToInt16(array.GetValue(i)));
                                    _byte.CopyTo(buffer, i * 4);
                                }
                                model.CURVE_DATA_TYPE = "int";
                                break;
                        }
                        OutFile(tempPath, buffer);
                        model.DATA_SIZE = new FileInfo(tempPath).Length;
                        xtf.m_CloseXtf();
                    }
                    else
                    {
                        model.DATA_SIZE = 0;
                        model.DATA_PATH = null;
                    }
                    break;
                default:
                    break;
            }
        }

        private void curveNameCbox_Initialized(object sender, EventArgs e)
        {
            var cbox = sender as System.Windows.Controls.ComboBox;
            if (cbox.ItemsSource != curveNameDic)
            {
                cbox.ItemsSource = curveNameDic;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileParser.ListToDataGrid(dataGrid2, textBox1.Text);
            BindComBox((dataGrid2.ItemsSource as DataView), (DependencyObject)rowGrid, (DependencyObject)colGrid);
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            FileParser.ListToDataGrid(dataGrid13, textBox12.Text);
            BindComBox((dataGrid13.ItemsSource as DataView), (DependencyObject)rowGrid6, (DependencyObject)colGrid6);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FileParser.ListToDataGrid(dataGrid3, textBox5.Text);
            BindComBox(dataGrid3.ItemsSource as DataView, (DependencyObject)rowGrid1, (DependencyObject)colGrid1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Path.GetExtension(textBox7.Text).ToUpper() == ".TXT")
                FileParser.TxtToDataGrid(dataGrid4, textBox7.Text);
            else
                FileParser.ExcelToDataGrid(dataGrid4, textBox7.Text);
            BindComBox(dataGrid4.ItemsSource as DataView, (DependencyObject)rowGrid2, (DependencyObject)colGrid2);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            FileParser.TxtToDataGrid(dataGrid5, textBox9.Text, File.ReadAllLines(textBox9.Text).Length);
            BindComBox(dataGrid5.ItemsSource as DataView, (DependencyObject)rowGrid3, (DependencyObject)colGrid3);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            FileParser.ListToDataGrid(dataGrid4_1, textBox7_1.Text);
            BindComBox(dataGrid4_1.ItemsSource as DataView, (DependencyObject)rowGrid4, (DependencyObject)colGrid4);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (Path.GetExtension(textBox6_1.Text).ToUpper() == ".TXT")
                FileParser.TxtToDataGrid(dataGrid6, textBox6_1.Text);
            else
                FileParser.ExcelToDataGrid(dataGrid6, textBox6_1.Text);
            BindComBox(dataGrid6.ItemsSource as DataView, (DependencyObject)rowGrid5, (DependencyObject)colGrid5);
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetComboxDefaultSelect((dataGrid2.ItemsSource as DataView).Table, comboBox1, colGrid);
        }

        private void comboBox4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetComboxDefaultSelect((dataGrid13.ItemsSource as DataView).Table, comboBox4, colGrid6);
        }

        private void comboBox1_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetComboxDefaultSelect((dataGrid3.ItemsSource as DataView).Table, comboBox1_1, colGrid1);
        }

        private void comboBox2_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetComboxDefaultSelect((dataGrid4.ItemsSource as DataView).Table, comboBox2_1, colGrid2);
        }

        private void comboBox3_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetComboxDefaultSelect((dataGrid5.ItemsSource as DataView).Table, comboBox3_1, colGrid3);
        }

        private void comboBox4_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetComboxDefaultSelect((dataGrid4_1.ItemsSource as DataView).Table, comboBox4_1, colGrid4);
        }

        private void comboBox6_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetComboxDefaultSelect((dataGrid6.ItemsSource as DataView).Table, comboBox6_1, colGrid5);
        }

        private void clearControls(DependencyObject rootControl)
        {
            rootControl.VisualSearchAllChild<System.Windows.Controls.TextBox>().ForEach(t => t.Clear());
            rootControl.VisualSearchAllChild<System.Windows.Controls.ComboBox>().ForEach(c => { if (c.Tag == null || c.Tag.ToString() != "skip")c.ItemsSource = null; });
            rootControl.VisualSearchAllChild<System.Windows.Controls.DataGrid>().ForEach(d => d.ItemsSource = null);
        }
        #region TabItem 数据保存

        private void save1_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            if (row == null) return;
            var tabItem = tabControl1.SelectedItem as TabItem;
            if (tabItem == null) return;
            bool? result = (bool?)this.GetType().InvokeMember(tabItem.Name + "_Save", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, this, new object[] { row.Row.FieldEx<string>("PROCESS_ID") });
            if (result == true)
            {
                ListBox2Data.Find(o => o.TabItem == tabItem).Foreground = Brushes.SpringGreen;
                if (tabItem.Name != "tabItem7" && tabItem.Name != "tabItem4") ExecTabItemLoadData(tabItem.Name);
                MessageBox.Show(App.Current.MainWindow, "保存成功！", tabItem.Header.ToString());
            }
            if (result == false)
                MessageBox.Show(App.Current.MainWindow, "保存失败！", tabItem.Header.ToString());
        }

        private bool? tabItem1_Save(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                dataser.SaveDataList_综合解释成果数据(process_id, ReadDataTable(comboBox2.SelectedValue as int?, (dataGrid2.ItemsSource as DataView), colGrid as DependencyObject));
                //tabItem1_LoadData(process_id);
                return true;
            }
        }

        private bool? tabItem2_Save(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                dataser.SaveDataList_固井质量评价数据(process_id, ReadDataTable(comboBox1_2.SelectedValue as int?, (dataGrid3.ItemsSource as DataView), colGrid1 as DependencyObject));
                //tabItem2_LoadData(process_id);
                return true;
            }
        }

        private bool? tabItem3_Save(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                dataser.SaveDataList_测量井斜数据(process_id, ReadDataTable(comboBox2_2.SelectedValue as int?, (dataGrid4.ItemsSource as DataView), colGrid2 as DependencyObject));
                return true;
            }
        }

        private bool? tabItem4_Save(string process_id)
        {
            /*
            Dictionary<string, int> dic = null;
            string md5 = null;
            string sha1 = null;
            long length = 0;
            if (File.Exists(textBox9.Text))
            {
                if (comboBox3_2.SelectedIndex == -1) throw new ArgumentNullException("数据开始行", "不能为空！");
                var task = new UploadController.UploadTask { FullName = textBox9.Text };
                new FileCacheUpload().BeginUpload(new DataCollection<UploadController.UploadTask> { task });
                if (task.hasError)
                {
                    return false;
                }
                md5 = task.MD5;
                sha1 = task.SHA1;
                length = task.Length;
                dic = new Dictionary<string, int>();
                dic.Add("StartLine", comboBox3_2.SelectedIndex + 1);
                (colGrid3 as DependencyObject).VisualSearchAllChild<ComboBox>(cbox => cbox.Tag != null)
                    .ForEach(cbox => dic.Add(cbox.Tag as string, cbox.SelectedIndex + 1));
            }
            using (var dataser = new DataServiceHelper())
            {
                dataser.SaveCurveDataCache(process_id, md5, sha1, length, ModelHelper.SerializeObject(dic));
                tabItem4_LoadData(process_id);
                return true;
            }
             * */

            var dt = GetCurveInfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.SaveCurveData(dt, process_id) > 0)
                    {
                        //initCurveVariables();
                        tabItem4_LoadData(process_id);
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        private bool? tabItem5_Save(string process_id)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            using (var dataser = new DataServiceHelper())
            {
                dataser.SaveDataList_测井分层数据(process_id, ReadLayerDataTable(comboBox4_2.SelectedValue as int?, (dataGrid4_1.ItemsSource as DataView), colGrid4 as DependencyObject));
                //COM_LOG_LAYER.ItemsSource = dataser.GetDataList_测井分层数据(row.Row.FieldEx<string>("PROCESS_ID")).Tables[0].GetDefaultView();
                //tabItem5_LoadData(process_id);
                return true;
            }
        }

        private bool? tabItem6_Save(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                dataser.SaveDataList_井眼轨迹数据(process_id, ReadDataTable(comboBox6_2.SelectedValue as int?, (dataGrid6.ItemsSource as DataView), colGrid5 as DependencyObject));
                //tabItem6_LoadData(process_id);
                return true;
            }
        }
        private bool? tabItem7_Save(string process_id)
        {
            //var itemid = comboBox17.SelectedValue;
            var map_chinese_name = comboBox17.SelectedValue as string;
            if (map_chinese_name == null)
            {
                MessageBox.Show(App.Current.MainWindow, "请选择图件编码!");
                return null;
            }
            var data = dataGrid7.ItemsSource as Utility.DataCollection<Model.PRO_LOG_PROCESSING_CURVE_INDEX>;
            if (data == null) return null;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].CURVE_START_DEP == null || data[i].CURVE_END_DEP == null)
                {
                    data.Remove(data[i]);
                    i--;
                }
            }
            var tasks = new Utility.DataCollection<UploadController.UploadTask>();
            using (var dataser = new DataServiceHelper())
            {
                foreach (var model in data)
                {
                    //只有三维曲线才上传
                    if (!string.IsNullOrWhiteSpace(model.DATA_PATH) && model.DATA_INFO > 2)
                    {
                        tasks.Add(new UploadController.UploadTask { FullName = model.DATA_PATH });
                    }
                }
                new FileUpload().BeginUpload(tasks);
                var taskList = tasks.ToList();
                if (taskList.Find(t => t.hasError) != null)
                {
                    MessageBox.Show(App.Current.MainWindow, "上传出错，保存失败！");
                    return null;
                }

                foreach (var model in data)
                {
                    var task = taskList.Find(t => t.FullName == model.DATA_PATH);
                    if (task != null)
                    {
                        model.FILEID = task.FileID;
                        model.DATA_STORAGE_WAY = 1;
                        model.BLOCK_NUMBER = 0;
                        model.BLOCK_SIZE = task.Length;
                        var dateDir = DateTime.Now.ToString("yyyyMM");
                        var filePath = string.Format("{0}\\{1}-{2}-{3}", dateDir, task.SHA1, task.MD5, task.Length);
                        model.CURVE_DATA = System.Text.Encoding.UTF8.GetBytes(filePath);
                    }
                    else if (!string.IsNullOrWhiteSpace(model.DATA_PATH))
                    {
                        byte[] fileData = GetFileData(model.DATA_PATH);
                        model.DATA_STORAGE_WAY = 0;
                        model.BLOCK_NUMBER = 0;
                        model.BLOCK_SIZE = fileData.Length;
                        model.CURVE_DATA = fileData;
                    }
                }

                //foreach (var model in data)
                //{
                //    if (!string.IsNullOrWhiteSpace(model.DATA_PATH))
                //    {
                //        byte[] fileData = GetFileData(model.DATA_PATH);
                //        model.BLOCK_NUMBER = 0;
                //        model.BLOCK_SIZE = fileData.Length;
                //        model.CURVE_DATA = fileData;
                //    }
                //}
                var datatable = Utility.ModelHandler<Model.PRO_LOG_PROCESSING_CURVE_INDEX>.FillDataTable(data.ToList());
                datatable.Columns.Remove("DATA_PATH");
                dataser.SaveProcessingCurveInfo(process_id, map_chinese_name, datatable);
                return true;
            }
        }

        private decimal SaveUploadInfo(string fullName)
        {
            int sha1Size = 256 * 1024;
            decimal fileid = 0;
            IAsyncResult result;
            Utility.HashHelper hashhelper;
            using (var fs = new FileStream(fullName, FileMode.Open, FileAccess.Read))
            {
                var fi = new FileInfo(fullName);
                string name = fi.Name;
                string path = fi.DirectoryName.Remove(0, fi.Directory.Root.Name.Length);
                long length = fs.Length;
                string sha1 = Utility.HashHelper.ComputeSHA1(fs, sha1Size); ;
                hashhelper = new Utility.HashHelper(fs);
                result = hashhelper.AsyncComputeMD5();
                while (!result.AsyncWaitHandle.WaitOne(300))
                {

                }
                string md5 = hashhelper.Hash;

                using (var fileser = new FileServiceHelper())
                {
                    fileser.SaveUploadInfo(sha1, md5, length, 0);
                    fileid = fileser.SaveFileUploadInfo(name, sha1, md5, length, path);
                }
            }
            return fileid;
        }

        private bool? SaveProcessDocument(string process_id, System.Windows.Controls.DataGrid datagrid, int filetype)
        {
            var dv = datagrid.ItemsSource as DataView;
            DataTable dt = new DataTable("data");
            dt.Columns.Add("DOCUMENT_FORMAT");
            dt.Columns.Add("DOCUMENT_NAME");
            dt.Columns.Add("DOCUMENT_TYPE");
            dt.Columns.Add("DOCUMENT_WRITER");
            dt.Columns.Add("DOCUMENT_VERIFIER");
            dt.Columns.Add("DOCUMENT_COMPLETION_DATE", typeof(DateTime));
            dt.Columns.Add("DOCUMENT_DATA_SIZE");
            dt.Columns.Add("FILEID");
            dt.Columns.Add("PROCESSING_ITEM_ID");
            dt.Columns.Add("A1_GUID");
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (!dv.Table.Columns.Contains("FULLNAME"))
                {
                    foreach (DataRow dr in dv.Table.Rows)
                    {
                        dt.Rows.Add(dr["EXTENSION"] == null ? DBNull.Value : dr["EXTENSION"],
                            dr["FILENAME"], dr["DOCUMENT_TYPE"],
                            dr["DOCUMENT_WRITER"], dr["DOCUMENT_VERIFIER"],
                            dr["DOCUMENT_COMPLETION_DATE"], dr["LENGTH"],
                            dr["FILEID"], dr["PROCESSING_ITEM_ID"], dr["A1_GUID"]);
                    }
                }
                else
                {
                    var tasks = new Utility.DataCollection<UploadController.UploadTask>();
                    foreach (DataRow dr in dv.Table.Rows)
                    {
                        tasks.Add(new UploadController.UploadTask { FullName = dr.FieldEx<string>("FULLNAME") });
                    }

                    new FileUpload().BeginUpload(tasks);
                    var taskList = tasks.ToList();
                    if (taskList.Find(t => t.hasError) != null)
                    {
                        MessageBox.Show(App.Current.MainWindow, "上传出错，保存失败！");
                        return null;
                    }
                    foreach (DataRow dr in dv.Table.Rows)
                    {
                        var task = taskList.Find(t => t.FullName == dr.FieldEx<string>("FULLNAME"));
                        if (task != null)
                        {
                            dt.Rows.Add(dr["EXTENSION"] == null ? DBNull.Value : dr["EXTENSION"],
                            dr["FILENAME"], dr["DOCUMENT_TYPE"],
                            dr["DOCUMENT_WRITER"], dr["DOCUMENT_VERIFIER"],
                            dr["DOCUMENT_COMPLETION_DATE"], dr["LENGTH"],
                            task.FileID, dr["PROCESSING_ITEM_ID"], dr["A1_GUID"]);
                        }
                    }
                }
                dataser.SaveProcessDocument(process_id, filetype, dt);
                return true;
            }
        }

        private bool? SaveProcessingFile(string process_id, System.Windows.Controls.DataGrid datagrid, int filetype)
        {
            var dv = datagrid.ItemsSource as DataView;
            if (!dv.Table.Columns.Contains("FULLNAME")) return null;
            var tasks = new Utility.DataCollection<UploadController.UploadTask>();
            DataTable dt = new DataTable("data");
            dt.Columns.Add("EXTENSION");
            dt.Columns.Add("FILEID");
            dt.Columns.Add("PROCESSING_ITEM_ID");
            using (var dataser = new DataServiceHelper())
            {
                //成果文档、图片、其它只保存文件信息不上传
                //if (filetype == 3 || filetype == 4 || filetype == 5)
                //if (false)
                //{
                //    dt.Columns.Add("PROCESS_UPLOAD_FILE", typeof(byte[]));
                //    foreach (DataRow dr in dv.Table.Rows)
                //    {
                //        string fullname = dr.FieldEx<string>("FULLNAME");
                //        decimal fileid = SaveUploadInfo(fullname);
                //        byte[] fileBytes = GetFileData(fullname);
                //        dt.Rows.Add(dr["EXTENSION"] == null ? DBNull.Value : dr["EXTENSION"], fileid, dr["PROCESSING_ITEM_ID"], fileBytes);
                //    }
                //}
                //else
                //{
                foreach (DataRow dr in dv.Table.Rows)
                {
                    tasks.Add(new UploadController.UploadTask { FullName = dr.FieldEx<string>("FULLNAME") });
                }

                new FileUpload().BeginUpload(tasks);
                var taskList = tasks.ToList();
                if (taskList.Find(t => t.hasError) != null)
                {
                    MessageBox.Show(App.Current.MainWindow, "上传出错，保存失败！");
                    return null;
                }
                foreach (DataRow dr in dv.Table.Rows)
                {
                    var task = taskList.Find(t => t.FullName == dr.FieldEx<string>("FULLNAME"));
                    if (task != null)
                    {
                        dt.Rows.Add(dr["EXTENSION"] == null ? DBNull.Value : dr["EXTENSION"], task.FileID, dr["PROCESSING_ITEM_ID"]);
                    }
                    //}
                }
                dataser.SaveProcessingFile(process_id, filetype, dt);
                return true;
            }
        }
        //文件转换成byte流数据
        private byte[] GetFileData(string fullName)
        {
            FileStream fs = File.OpenRead(fullName);
            byte[] buffer = new byte[fs.Length];
            int remaining = buffer.Length;
            int offset = 0;
            while (remaining > 0)
            {
                int read = fs.Read(buffer, offset, buffer.Length);
                remaining -= read;
                offset += read;
            }
            fs.Close();
            return buffer;
        }

        private bool? tabItem8_Save(string process_id)
        {
            bool? result = SaveProcessingFile(process_id, dataGrid8, 1);
            //if (result == true)
            //    tabItem8_LoadData(process_id);
            return result;
        }

        private bool? tabItem9_Save(string process_id)
        {
            return SaveProcessingFile(process_id, dataGrid9, 2);
        }

        private bool? tabItem10_Save(string process_id)
        {
            //return SaveProcessingFile(process_id, dataGrid10, 3);
            return SaveProcessDocument(process_id, dataGrid10, 3);
        }

        private bool? tabItem11_Save(string process_id)
        {
            return SaveProcessingFile(process_id, dataGrid11, 4);
        }

        private bool? tabItem12_Save(string process_id)
        {
            return SaveProcessingFile(process_id, dataGrid12, 5);
        }

        private bool? tabItem13_Save(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                dataser.SaveDataList_页岩气解释成果数据(process_id, ReadDataTable(comboBox5.SelectedValue as int?, (dataGrid13.ItemsSource as DataView), colGrid6 as DependencyObject));
                return true;
            }
        }
        #endregion
        private void viewFile(object sender, RoutedEventArgs e)
        {
            if (!canViewFile) return;
            var button = sender as System.Windows.Controls.Button;
            var rowView = button.DataContext as DataRowView;
            if (!rowView.Row.Table.Columns.Contains("FILEID"))
            {
                MessageBox.Show(App.Current.MainWindow, "数据还没有保存！");
                return;
            }
            ViewFile.View(new DownloadController.DownloadTask { FileID = rowView.Row.FieldEx<decimal>("FILEID"), Name = rowView.Row.FieldEx<string>("FILENAME") });
        }

        static string[] viewExtensions = "xls;xlsx;doc;docx;bmp;dib;jpg;jepg;jpe;jfif;gif;tif;tiff;png".Split(';');
        private void viewFileButton_Loaded(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (canViewFile && viewExtensions.Contains((button.Tag as string).ToLower()))
                button.Visibility = Visibility.Visible;
            else
                button.Visibility = Visibility.Collapsed;
        }

        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key)
            {
                var datagrid = sender as DataGrid;
                if (datagrid != null && datagrid.SelectedItems != null)
                {
                    var items = datagrid.SelectedItems;
                    while (items.Count > 0)
                    {
                        (datagrid.ItemsSource as DataView).Table.Rows.Remove((items[0] as DataRowView).Row);
                    }
                }
            }
        }

        private List<ListBoxItemModel> ListBox2Data = null;

        private List<ListBoxItemModel> CreateListBox2Data()
        {
            var list = new List<ListBoxItemModel>();
            foreach (TabItem item in tabControl1.Items)
            {
                list.Add(
                    new ListBoxItemModel
                    {
                        Name = item.Header.ToString(),
                        TabItem = item
                    });
            }
            return list;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox2 == null || e.AddedItems.Count == 0) return;
            var item = e.AddedItems[0] as ListBoxItem;
            string name = item.Tag as string;
            if (item.Content as string == "全部")
                listBox2.ItemsSource = ListBox2Data;
            else
                listBox2.ItemsSource = ListBox2Data.FindAll(o => name.IndexOf("," + o.Name + ",") >= 0);
            var a = listBox2.SelectedItem;
            if (listBox2.SelectedIndex == -1 && listBox2.Items.Count > 0) listBox2.SelectedIndex = 0;
        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            var model = e.AddedItems[0] as ListBoxItemModel;
            foreach (TabItem item in tabControl1.Items)
            {
                if (item == model.TabItem)
                {
                    item.Visibility = Visibility.Visible;
                    tabControl1.SelectedItem = item;
                    ExecTabItemLoadData(item.Name);
                }
                else
                    item.Visibility = Visibility.Collapsed;
            }
        }
        #region TabItem 数据载入
        private void ExecTabItemLoadData(string tabItemName)
        {
            var row = dataGrid1.SelectedItem as DataRowView;
            this.GetType().InvokeMember(tabItemName + "_LoadData", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, this, new object[] { row.Row.FieldEx<string>("PROCESS_ID") });
        }

        private void tabItem1_LoadData(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                COM_LOG_RESULT.ItemsSource = dataser.GetDataList_综合解释成果数据(process_id).Tables[0].GetDefaultView();
            }
        }
        private void tabItem2_LoadData(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                COM_LOG_CEMENT_EVALUATION_INF.ItemsSource = dataser.GetDataList_固井质量评价数据(process_id).Tables[0].GetDefaultView();
            }
        }
        private void tabItem3_LoadData(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                COM_LOG_DEV_AZI.ItemsSource = dataser.GetDataList_测量井斜数据(process_id).Tables[0].GetDefaultView();
            }
        }
        private decimal? curveDataUploadID = null;
        private Dictionary<string, int> curveDataComboBoxsSetting = null;
        private void tabItem4_LoadData(string process_id)
        {
            /*
            curveDataUploadID = null;
            curveDataComboBoxsSetting = null;
            using (var dataser = new DataServiceHelper())
            {
                var data = dataser.GetCurveDataCache(process_id).Tables[0];
                if (data != null && data.Rows.Count > 0)
                {
                    curveDataUploadID = data.Rows[0].FieldEx<decimal?>("UPLOADID");
                    curveDataComboBoxsSetting = ModelHelper.DeserializeObject(data.Rows[0].FieldEx<byte[]>("COMBOBOXS_SETTING")) as Dictionary<string, int>;
                }
                dataPager1.TotalCount = 0;
                GetCurveDataCachePage(1);
            }
            */
            initCurveVariables(dataPager1, COM_LOG_COM_CURVEDATA);
            if (GetCurveData(process_id, 1, dataPager1, COM_LOG_COM_CURVEDATA))
            {
                ReloadCurveData(1, dataPager1, COM_LOG_COM_CURVEDATA);
            }
        }
        private void tabItem5_LoadData(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                COM_LOG_LAYER.ItemsSource = dataser.GetDataList_测井分层数据(process_id).Tables[0].GetDefaultView();
            }
        }
        private void tabItem6_LoadData(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                COM_LOG_WELL_TRAJECTORY.ItemsSource = dataser.GetDataList_井眼轨迹数据(process_id).Tables[0].GetDefaultView();
            }
        }
        private bool isSaved()
        {
            var tabItem = tabControl1.SelectedItem as TabItem;
            return ListBox2Data.Find(o => o.TabItem == tabItem).Foreground == Brushes.SpringGreen;
        }
        private void tabItem7_LoadData(string process_id)
        {
            if (!string.IsNullOrEmpty(textBox2.Text) && !isSaved()) return;
            using (var dataser = new DataServiceHelper())
            {
                var curveInfoData = dataser.GetCurveInfo(process_id).Tables[0];
                dataGrid7.ItemsSource = curveInfoData.GetDefaultView();
                //if (curveInfoData != null && curveInfoData.Rows.Count > 0)
                //    comboBox17.SelectedValue = curveInfoData.Rows[0]["MAPS_CODING"];

                //else
                //    comboBox17.SelectedValue = null;
                comboBox17.SelectedIndex = 0;
            }
            curveNameCol.Visibility = Visibility.Visible;
            curveNameCol1.Visibility = Visibility.Collapsed;
        }
        private void LoadProcessingFile(string process_id, int fileType, DataGrid dg)
        {
            if (!string.IsNullOrEmpty(textBox2.Text) && !isSaved()) return;
            using (var dataser = new DataServiceHelper())
            {
                if (dg.Name != "dataGrid10")
                    dg.ItemsSource = dataser.GetProcessingFile(process_id, fileType).Tables[0].GetDefaultView();
                else
                    dg.ItemsSource = dataser.GetProcessDocument(process_id, fileType).Tables[0].GetDefaultView();
            }
        }
        private void tabItem8_LoadData(string process_id)
        {
            LoadProcessingFile(process_id, 1, dataGrid8);
        }
        private void tabItem9_LoadData(string process_id)
        {
            LoadProcessingFile(process_id, 2, dataGrid9);
        }
        private void tabItem10_LoadData(string process_id)
        {
            LoadProcessingFile(process_id, 3, dataGrid10);
        }
        private void tabItem11_LoadData(string process_id)
        {
            LoadProcessingFile(process_id, 4, dataGrid11);
        }
        private void tabItem12_LoadData(string process_id)
        {
            LoadProcessingFile(process_id, 5, dataGrid12);
        }
        private void tabItem13_LoadData(string process_id)
        {
            using (var dataser = new DataServiceHelper())
            {
                COM_LOG_RESULT1.ItemsSource = dataser.GetDataList_页岩气解释成果数据(process_id).Tables[0].GetDefaultView();
            }
        }
        private void dataPager1_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            //GetCurveDataCachePage((int)e.CurrentPageIndex);


            int page = (int)e.CurrentPageIndex;
            ReloadCurveData(page, dataPager1, COM_LOG_COM_CURVEDATA);
            pre_page = page;
        }

        private void GetCurveDataCachePage(int page)
        {
            COM_LOG_COM_CURVEDATA.ItemsSource = null;
            if (curveDataUploadID == null || curveDataComboBoxsSetting == null) return;
            if (!curveDataComboBoxsSetting.ContainsKey("StartLine")) return;
            int startLine = curveDataComboBoxsSetting["StartLine"];
            if (startLine < 1) return;
            using (var dataser = new DataServiceHelper())
            {
                int countLine = 0;
                var strArry = dataser.GetCurveDataCachePage((decimal)curveDataUploadID, startLine, page, out countLine);
                if (countLine < 1) return;
                dataPager1.TotalCount = countLine - startLine + 1;
                COM_LOG_COM_CURVEDATA.ItemsSource = ReadSringArry(strArry).GetDefaultView();
            }
        }

        private DataTable ReadSringArry(string[] strArry)
        {
            var _dt = new DataTable();
            if (strArry == null || strArry.Length == 0) return _dt;
            Regex reg = new Regex("-+", RegexOptions.Compiled);
            var dic = new Dictionary<string, int[]>();
            _dt.TableName = "data";
            foreach (var c in curveDataComboBoxsSetting)
            {
                if (c.Key == "StartLine") continue;
                var names = c.Key.Split('|')[0].Split('-');
                int selectIndex = c.Value;
                string[] tempArr = c.Key.Split(':');
                Type dataType = tempArr.Length == 2 ? Type.GetType(tempArr[1]) : typeof(String);
                if (names.Length > 1)
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (selectIndex > -1) dic.Add(names[i], new int[] { (int)selectIndex, i });
                        _dt.Columns.Add(new DataColumn(names[i], dataType));
                    }
                }
                else
                {
                    if (selectIndex > -1) dic.Add(names[0], new int[] { (int)selectIndex });
                    _dt.Columns.Add(new DataColumn(names[0], dataType));
                }
            }

            foreach (string line in strArry)
            {
                var matches = Regex.Matches(line, @"[^\s]+");
                var dr = _dt.NewRow();
                foreach (var k in dic)
                {
                    if (k.Value[0] == 0 || k.Value[0] > matches.Count) continue;
                    var str = matches[k.Value[0] - 1].Value;
                    if (string.IsNullOrEmpty(str))
                        dr[k.Key] = DBNull.Value;
                    else
                    {
                        if (k.Value.Length > 1)
                        {
                            var v = reg.Split(str);
                            if (v.Length > k.Value[1])
                            {
                                dr[k.Key] = FieldConvert(v[k.Value[1]]);
                            }
                            else dr[k.Key] = str;
                        }
                        else
                        {
                            dr[k.Key] = str;
                        }
                    }
                }
                _dt.Rows.Add(dr);
            }

            return _dt;
        }

        #endregion

        private void ProcessingItemComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var cbox = sender as ComboBox;
            if (cbox.ItemsSource != process_code)
                cbox.ItemsSource = process_code;
        }

        private class ListBoxItemModel : Model.ModelBase
        {
            private string name;
            public string Name
            {
                get { return name; }
                set
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
            public TabItem TabItem { get; set; }
            private Brush foreground = Brushes.Black;
            public Brush Foreground
            {
                get { return foreground; }
                set
                {
                    foreground = value;
                    NotifyPropertyChanged("Foreground");
                }
            }
        }
        private void downloadButton_Loaded(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (canViewFile && viewExtensions.Contains((button.Tag as string).ToLower()))
                button.Visibility = Visibility.Visible;
            else
                button.Visibility = Visibility.Collapsed;
        }


        private void Download_File(object sender, RoutedEventArgs e)
        {
            if (!canViewFile) return;
            var button = sender as System.Windows.Controls.Button;
            var rowView = button.DataContext as DataRowView;
            if (!rowView.Row.Table.Columns.Contains("FILEID"))
            {
                MessageBox.Show(App.Current.MainWindow, "数据还没有保存！");
                return;
            }
            string file_name = rowView.Row.FieldEx<string>("FILENAME");
            SaveFileDialog sf = new SaveFileDialog();
            sf.RestoreDirectory = true;
            sf.Filter = "所有|*.*";
            sf.FileName = file_name;
            if (sf.ShowDialog() == true)
            {
                byte[] fileBytes = rowView.Row.FieldEx<byte[]>("PROCESS_UPLOAD_FILE");
                FileStream fs = new FileStream(sf.FileName, FileMode.Create, FileAccess.Write);
                fs.Write(fileBytes, 0, fileBytes.Length);
                fs.Close();
                MessageBox.Show("保存成功!");
            }
        }

        //重新构建曲线datatable
        private DataTable GetCurveInfo()
        {
            int start_line = 0;
            float rlev = 0.125f;
            Dictionary<string, int[]> dic = null;
            var dv = dataGrid5.ItemsSource as DataView;
            for (int i = 0; i < dv.Table.Rows.Count; i++)
            {
                var dr = dv.Table.Rows[i];
                if (dr[0].Equals("RLEV"))
                {
                    if (Convert.ToSingle(dr[2]) != rlev)
                    {
                        MessageBox.Show("综合解释成果曲线数据\n请使用0.125米采样间距!!", "提示");
                        return null;
                    }
                    break;
                }
            }
            //取到combox组保存值
            if (File.Exists(textBox9.Text))
            {
                if (comboBox3_2.SelectedIndex == -1) throw new ArgumentNullException("数据开始行", "不能为空！");
                dic = new Dictionary<string, int[]>();
                start_line = comboBox3_2.SelectedIndex + 1;
                (colGrid3 as DependencyObject).VisualSearchAllChild<ComboBox>(cbox => cbox.Tag != null)
                    .ForEach(cbox =>
                    {
                        var names = cbox.Tag.ToString().Split('|')[0].Split('-');
                        int selectIndex = cbox.SelectedIndex + 1;
                        if (names.Length > 1)
                        {
                            for (int i = 0; i < names.Length; i++)
                            {
                                if (selectIndex > 0) dic.Add(names[i], new int[] { (int)selectIndex, i });
                            }
                        }
                        else
                        {
                            if (selectIndex > 0) dic.Add(names[0], new int[] { (int)selectIndex });
                        }
                    });
            }
            var curveHelper = new CurveDataHelper();
            return curveHelper.GetComLogCurveDT(dv.Table, dic, start_line);
        }

        private void ReloadCurveData(int page, Controls.DataPager dp, DataGrid dg)
        {
            int pagesize = dp.PageSize;
            int total = dp.TotalCount;
            int start_line = (page - 1) * pagesize + 1;
            int end_line = pagesize;

            if (dp.PageCount == 1 || page == dp.PageCount)
            {
                end_line = total;
            }
            else
            {
                end_line += start_line - 1;
            }

            if (page > pre_page)
            {
                start_dep += (page - pre_page) * pagesize * curve_rlev;
            }
            else if (page < pre_page)
            {
                start_dep -= (pre_page - page) * pagesize * curve_rlev;
            }

            BindingCurveData(start_line, end_line, start_dep, curve_rlev, dg);
        }

        private void BindingCurveData(int start_line, int end_line, decimal start_dep, decimal curve_rlev, DataGrid dg)
        {
            new_dt.Rows.Clear();
            //循环行数
            for (int i = start_line - 1; i < end_line; i++)
            {
                var _dr = new_dt.NewRow();

                //循环列数
                for (int j = 0; j < original_dt.Rows.Count; j++)
                {
                    if (dic_list[j][i] == -999)
                    {
                        _dr[j] = DBNull.Value;
                    }
                    else
                    {
                        _dr[j] = dic_list[j][i];
                    }
                    if (j == original_dt.Rows.Count - 1)
                    {
                        _dr[j + 1] = start_dep;
                        start_dep += curve_rlev;
                    }
                }
                new_dt.Rows.Add(_dr);
            }
            dg.ItemsSource = new_dt.GetDefaultView();
        }

        private void initCurveVariables(Controls.DataPager dp, DataGrid dg)
        {
            original_dt = null;
            new_dt = null;
            dic_list = null;
            start_dep = 0;
            curve_rlev = 0;
            pre_page = 1;
            dp.TotalCount = 0;

            new_dt = new DataTable();
            dic_list = new Dictionary<int, List<float>>();
            dg.ItemsSource = null;
        }

        DataTable original_dt = null;
        DataTable new_dt = null;
        decimal start_dep = 0;
        decimal curve_rlev = 0;
        int pre_page = 1;
        Dictionary<int, List<float>> dic_list = null;

        private bool GetCurveData(string process_id, int type, Controls.DataPager dp, DataGrid dg)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                if (type == 1)
                    original_dt = dataser.GetCurveData(process_id).Tables[0];
                else
                    original_dt = dataser.GetThreePressureData(process_id).Tables[0];

                if (original_dt != null && original_dt.Rows.Count > 0)
                {
                    new_dt = new DataTable();
                    dic_list = new Dictionary<int, List<float>>();
                    int total = original_dt.Rows[0].FieldEx<int>("CURVE_SAMPLE");
                    start_dep = original_dt.Rows[0].FieldEx<decimal>("CURVE_START_DEP");
                    curve_rlev = original_dt.Rows[0].FieldEx<decimal>("CURVE_RLEV");
                    int count = 0;
                    for (int i = 0; i < original_dt.Rows.Count; i++)
                    {
                        var _column = original_dt.Rows[i].FieldEx<string>("CURVE_CD");
                        var dgc = dg.Columns.Where(p => p.SortMemberPath.Equals(_column)).FirstOrDefault();
                        if (dgc != null) { count++; }
                        new_dt.Columns.Add(_column, typeof(decimal));

                        var curveBytes = original_dt.Rows[i].FieldEx<byte[]>("CURVE_DATA");
                        var curveData = new float[curveBytes.Length / 4];
                        for (int j = 0; j < curveBytes.Length / 4; j++)
                            curveData[j] = BitConverter.ToSingle(curveBytes, j * 4);
                        dic_list.Add(i, curveData.ToList());
                        //var list = ModelHelper.DeserializeObject(original_dt.Rows[i].FieldEx<byte[]>("CURVE_DATA")) as List<float>;
                        //dic_list.Add(i, list);
                    }
                    if (count == 0) { return false; }
                    new_dt.Columns.Add("DEP", typeof(decimal));
                    dp.TotalCount = total;
                    return true;
                }
            }
            return false;
        }

        private void button14_Click(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataser.TransOldProcessingCurveData();
                MessageBox.Show(DateTime.Now + ":转化完成!!");
            }
        }

        private void button15_Click(object sender, RoutedEventArgs e)
        {
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                dataser.TransOldComCurveData();
                MessageBox.Show(DateTime.Now + ":转化完成!!");
            }
        }

        private void dataPager2_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            int page = (int)e.CurrentPageIndex;
            LoadDataList(page);
        }

        private void button14_1_Click(object sender, RoutedEventArgs e)
        {
            var m_MyDlg = new WinFroms.OpenFileDialog();
            m_MyDlg.Filter = "三压力测井曲线(*.txt;)|*.txt";
            m_MyDlg.FileName = textBox14.Text;
            if (m_MyDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                clearControls(sp14 as DependencyObject);
                textBox14.Text = m_MyDlg.FileName;
            }
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            FileParser.TxtToDataGrid(dataGrid14, textBox14.Text, File.ReadAllLines(textBox14.Text).Length);
            BindComBox(dataGrid14.ItemsSource as DataView, (DependencyObject)rowGrid14, (DependencyObject)colGrid14);
        }

        private void comboBox14_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetComboxDefaultSelect((dataGrid14.ItemsSource as DataView).Table, comboBox14_1, colGrid14);
        }

        private void dataPager3_PageChanged(object sender, Controls.PageChangedEventArgs e)
        {
            int page = (int)e.CurrentPageIndex;
            ReloadCurveData(page, dataPager3, PRO_LOG_THRESS_PRESSURE);
            pre_page = page;
        }

        private DataTable GetThressPressureData()
        {
            //取到combox组保存值
            Dictionary<string, int[]> dic = null;
            int start_line = 0;
            if (File.Exists(textBox14.Text))
            {
                if (comboBox14_2.SelectedIndex == -1) throw new ArgumentNullException("数据开始行", "不能为空！");
                dic = new Dictionary<string, int[]>();
                start_line = comboBox14_2.SelectedIndex + 1;
                (colGrid14 as DependencyObject).VisualSearchAllChild<ComboBox>(cbox => cbox.Tag != null)
                    .ForEach(cbox =>
                    {
                        var names = cbox.Tag.ToString().Split('|')[0].Split('-');
                        int selectIndex = cbox.SelectedIndex + 1;
                        if (names.Length > 1)
                        {
                            for (int i = 0; i < names.Length; i++)
                            {
                                if (selectIndex > 0) dic.Add(names[i], new int[] { (int)selectIndex, i });
                            }
                        }
                        else
                        {
                            if (selectIndex > 0) dic.Add(names[0], new int[] { (int)selectIndex });
                        }
                    });
            }

            var dv = dataGrid14.ItemsSource as DataView;
            var curveHelper = new CurveDataHelper();
            return curveHelper.GetComLogCurveDT(dv.Table, dic, start_line);
        }

        private bool? tabItem14_Save(string process_id)
        {
            var dt = GetThressPressureData();
            if (dt != null && dt.Rows.Count > 0)
            {
                using (DataServiceHelper dataser = new DataServiceHelper())
                {
                    if (dataser.SaveThreePressureData(dt, process_id) > 0)
                    {
                        tabItem14_LoadData(process_id);
                        return true;
                    }
                }
            }
            return false;
        }

        private void tabItem14_LoadData(string process_id)
        {
            initCurveVariables(dataPager3, PRO_LOG_THRESS_PRESSURE);
            if (GetCurveData(process_id, 2, dataPager3, PRO_LOG_THRESS_PRESSURE))
            {
                ReloadCurveData(1, dataPager3, PRO_LOG_THRESS_PRESSURE);
            }
        }

        DataTable curveDT = new DataTable();
        Dictionary<int, Array> dic = new Dictionary<int, Array>();
        private void Show_Data(object sender, RoutedEventArgs e)
        {
            var dv = dataGrid7.SelectedItems as IList;
            var drv0 = dv[0] as DataRowView;
            if (drv0 == null)
            {
                MessageBox.Show("数据还没保存!!", "提示");
                return;
            }

            var rlev = drv0.Row.FieldEx<decimal>("CURVE_RLEV");
            //判断曲线采样间距一致
            for (int i = 0; i < dv.Count; i++)
            {
                var drv = dv[i] as DataRowView;
                var rlev1 = drv.Row.FieldEx<decimal>("CURVE_RLEV");
                var data_info = drv.Row.FieldEx<decimal>("DATA_INFO");
                var storage_way = drv.Row.FieldEx<decimal>("DATA_STORAGE_WAY");
                if (data_info > 2 || storage_way == 1)
                {
                    MessageBox.Show("三维曲线暂不支持显示!!", "提示");
                    return;
                }
                if (rlev != rlev1)
                {
                    MessageBox.Show("请选择采样间距相同的曲线!!", "提示");
                    return;
                }
            }

            dic.Clear();
            curveDT.Clear();
            curveDT.Columns.Clear();
            curveDT.Columns.Add("序号", typeof(decimal));
            curveDT.Columns.Add("DEP", typeof(decimal));
            var d_start = new decimal[dv.Count];
            var d_end = new decimal[dv.Count];
            var start_min = drv0.Row.FieldEx<decimal>("CURVE_START_DEP");
            var end_max = drv0.Row.FieldEx<decimal>("CURVE_END_DEP");
            for (int i = 0; i < dv.Count; i++)
            {
                var drv = dv[i] as DataRowView;
                var curve_cd = drv.Row.FieldEx<string>("CURVE_CD");
                var start_dep = drv.Row.FieldEx<decimal>("CURVE_START_DEP");
                var end_dep = drv.Row.FieldEx<decimal>("CURVE_END_DEP");
                var curve_id = drv.Row.FieldEx<decimal>("CURVEID");
                var type = drv.Row.FieldEx<string>("CURVE_DATA_TYPE");
                byte[] bytes = null;
                try
                {
                    using (DataServiceHelper dataser = new DataServiceHelper())
                    {
                        var dt = dataser.GetProcessingCurveData(curve_id).Tables[0];
                        bytes = dt.Rows[0].FieldEx<byte[]>("CURVE_DATA");
                    }
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("曲线数据太大,无法显示!!", "提示");
                    return;
                }

                if (bytes == null || bytes.Length == 0) continue;
                if (!string.IsNullOrWhiteSpace(type))
                {
                    if (type.ToLower().Equals("int"))
                    {
                        var curve_data = new int[bytes.Length / 4];
                        for (int j = 0; j < bytes.Length / 4; j++)
                            curve_data[j] = BitConverter.ToInt32(bytes, j * 4);
                        dic.Add(i, curve_data);
                    }
                    else if (type.ToLower().Equals("float"))
                    {
                        var curve_data = new float[bytes.Length / 4];
                        for (int j = 0; j < bytes.Length / 4; j++)
                            curve_data[j] = BitConverter.ToSingle(bytes, j * 4);
                        dic.Add(i, curve_data);
                    }
                    else if (type.ToLower().Equals("double"))
                    {
                        var curve_data = new double[bytes.Length / 8];
                        for (int j = 0; j < bytes.Length / 8; j++)
                            curve_data[j] = BitConverter.ToDouble(bytes, j * 8);
                        dic.Add(i, curve_data);
                    }

                    curveDT.Columns.Add(curve_cd, typeof(string));
                    if (start_min > start_dep) { start_min = start_dep; }
                    if (end_max < end_dep) { end_max = end_dep; }
                    d_start[i] = start_dep;
                    d_end[i] = end_dep;
                }
            }

            var sample = Math.Ceiling((end_max - start_min) / rlev);
            var idx = new int[dv.Count];
            for (int i = 0; i < d_start.Length; i++)
            {
                var _start = start_min;
                for (int j = 0; j <= sample; j++)
                {
                    if (_start >= d_start[i])
                    {
                        idx[i] = j;
                        break;
                    }
                    _start += rlev;
                }
            }

            for (int i = 0; i <= sample; i++)
            {
                var dr = curveDT.NewRow();
                for (int j = 0; j < curveDT.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        dr[j] = i + 1;
                    }
                    else if (j == 1)
                    {
                        dr[j] = start_min;
                    }
                    else
                    {
                        var start = d_start[j - 2];
                        var end = d_end[j - 2];
                        if ((start_min >= start) && (start_min <= end))
                        {
                            var index = i - idx[j - 2];
                            var maxidx = dic[j - 2].Length - 1;
                            if (index <= maxidx)
                                dr[j] = dic[j - 2].GetValue(index);
                            else
                                dr[j] = -9999;
                        }
                        else
                        {
                            dr[j] = -9999;
                        }
                    }
                }
                start_min += rlev;
                curveDT.Rows.Add(dr);
            }
            var cdw = new CurveDataWindow();
            cdw.datagrid1.ItemsSource = curveDT.DefaultView;
            cdw.ShowDialog();
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            SearchPanel.DataContext = new Model.PRO_LOG_DATA_PUBLISH();
        }

        private void DocumentTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var cbox = sender as ComboBox;
            using (DataServiceHelper dataser = new DataServiceHelper())
            {
                cbox.ItemsSource = dataser.GetData_文档类型().Tables[0].GetDefaultView();
            }
        }
    }
}
