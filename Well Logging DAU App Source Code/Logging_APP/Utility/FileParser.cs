using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Controls;

using NPOI.SS.UserModel;
using NPOI.POIFS.FileSystem;

namespace Logging_App.Utility
{
    public static class FileParser
    {
        public static void ListToDataGrid(DataGrid m_MyDataGrid, string in_Name)
        {
            string MyInf;
            string[] split;
            m_MyDataGrid.AutoGenerateColumns = true;
            m_MyDataGrid.CanUserSortColumns = false;
            m_MyDataGrid.CanUserAddRows = false;
            m_MyDataGrid.IsReadOnly = true;
            if (File.Exists(in_Name))
            {
                using (StreamReader MyRead = new StreamReader(in_Name, Encoding.Default))
                {
                    DataTable dt = new DataTable();
                    while (MyRead.Peek() >= 0)
                    {
                        MyInf = MyRead.ReadLine();

                        if (!string.IsNullOrWhiteSpace(MyInf))
                        {
                            split = MyInf.Split(',');
                            for (int i = dt.Columns.Count + 1; i <= split.Length; i++) dt.Columns.Add(new DataColumn(i.ToString()));
                            dt.Rows.Add(split);
                        }
                    }
                    m_MyDataGrid.ItemsSource = dt.GetDefaultView();
                }
            }
        }

        private static readonly Regex regex = new Regex(@"[^\s]+", RegexOptions.Compiled);
        private static readonly Regex regex1 = new Regex(@"#\s+", RegexOptions.Compiled);
        public static void TxtToDataGrid(DataGrid m_MyDataGrid, string in_Name, int maxLine = int.MaxValue)
        {
            string MyInf;
            MatchCollection matchColl;
            m_MyDataGrid.AutoGenerateColumns = true;
            m_MyDataGrid.CanUserSortColumns = false;
            m_MyDataGrid.CanUserAddRows = false;
            m_MyDataGrid.IsReadOnly = true;
            if (File.Exists(in_Name))
            {
                using (StreamReader MyRead = new StreamReader(in_Name, Encoding.Default))
                {
                    DataTable dt = new DataTable();
                    while (MyRead.Peek() >= 0)
                    {
                        MyInf = MyRead.ReadLine();
                        MyInf = regex1.Replace(MyInf, "#");
                        if (!string.IsNullOrWhiteSpace(MyInf))
                        {
                            matchColl = regex.Matches(MyInf);
                            for (int i = dt.Columns.Count + 1; i <= matchColl.Count; i++) dt.Columns.Add(new DataColumn(i.ToString()));
                            dt.Rows.Add(
                                (
                                from match in matchColl.Cast<Match>()
                                select match.Value
                                ).ToArray()
                                );
                            if (dt.Rows.Count > maxLine) break;
                        }
                    }
                    m_MyDataGrid.ItemsSource = dt.GetDefaultView();
                }
            }
        }

        public static DataTable OleDbReadExcel(string in_Name)
        {
            var dt = new DataTable("ExcelData");
            using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + in_Name + ";Extended Properties='Excel 8.0;HDR=No;IMEX=1;'"))
            {
                connection.Open();
                DataTable sheetNames = connection.GetSchema("Tables");
                if (sheetNames.Rows.Count < 1) return dt;
                OleDbCommand command = new OleDbCommand("SELECT * FROM [" + sheetNames.Rows[0]["TABLE_NAME"] + "]", connection);
                OleDbDataReader reader = command.ExecuteReader();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dt.Columns.Add((i + 1).ToString());
                }
                while (reader.Read())
                {
                    var a = reader;
                    var obj = new object[a.FieldCount];
                    a.GetValues(obj);
                    dt.Rows.Add(obj);
                }
                reader.Close();
            }
            return dt;
        }

        public static DataTable NPOIReadExcel(string in_Name)
        {
            using (var fs = new FileStream(in_Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook wb = WorkbookFactory.Create(fs, ImportOption.TextOnly);
                DataTable dt = new DataTable("ExcelData");
                if (wb.NumberOfSheets > 0)
                {

                    var sheet = wb.GetSheetAt(0);
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null) continue;
                        for (int j = dt.Columns.Count + 1; j <= row.LastCellNum; j++) dt.Columns.Add(new DataColumn(j.ToString()));
                        DataRow dataRow = dt.NewRow();

                        (
                            from cell in row.Cells
                            //where cell.ColumnIndex < dt.Columns.Count
                            select dataRow[cell.ColumnIndex] =
                            cell.CellType == CellType.String ? cell.StringCellValue :
                            cell.CellType == CellType.Numeric ? cell.NumericCellValue.ToString() :
                            cell.CellType == CellType.Boolean ? cell.BooleanCellValue.ToString() :
                            cell.CellType == CellType.Formula ? cell.NumericCellValue.ToString() :
                            ""
                         ).Count();
                        dt.Rows.Add(dataRow);
                    }
                }
                return dt;
            }
        }

        public static void ExcelToDataGrid(DataGrid m_MyDataGrid, string in_Name)
        {
            if (!File.Exists(in_Name)) return;
            m_MyDataGrid.AutoGenerateColumns = true;
            m_MyDataGrid.CanUserSortColumns = false;
            m_MyDataGrid.CanUserAddRows = false;
            m_MyDataGrid.IsReadOnly = true;
            DataTable dt;
            try
            {
                dt = NPOIReadExcel(in_Name);
            }
            catch (NPOI.HSSF.OldExcelFormatException)
            {
                dt = OleDbReadExcel(in_Name);
            }
            m_MyDataGrid.ItemsSource = dt.GetDefaultView();
        }

        public static DataTable ExcelToDataTable(string in_Name, int columnNameLineNumber = 1)
        {
            DataTable dt;
            try
            {
                dt = NPOIReadExcel(in_Name);
            }
            catch (NPOI.HSSF.OldExcelFormatException)
            {
                dt = OleDbReadExcel(in_Name);
            }
            if (dt != null && dt.Rows.Count >= columnNameLineNumber)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string name = dt.Rows[columnNameLineNumber - 1].FieldEx<string>(i);
                    if (string.IsNullOrWhiteSpace(name)) break;
                    dt.Columns[i].ColumnName = name;
                }
                dt.Rows.Remove(dt.Rows[columnNameLineNumber - 1]);
            }
            return dt;
        }

        private static readonly Regex curveRegex = new Regex(@"DIMENSION\s+(?<DATA_INFO>\d+).*?UNIT\s+(?<CURVE_UNIT>.*?)\s+.*?SDEP\s+(?<CURVE_START_DEP>.*?)\s+EDEP\s+(?<CURVE_END_DEP>.*?)\s+RLEV\s+(?<CURVE_RLEV>.*?)\s+INDEX-2-NAME\s+.*?(?#<CURVE_CD>.*?)\s+UNIT\s+(?<CURVE_T_UNIT>.*?)\s+TYPE\s+(?<CURVE_DATA_TYPE>.*?)\s+LENGTH\s+(?<CURVE_DATA_LENGTH>.*?)\s+NPS\s+(?<CURVE_T_SAMPLE>.*?)\s+.*?MIN\s+(?<CURVE_T_MIN_VALUE>.*?)\s+MAX\s+(?<CURVE_T_MAX_VALUE>.*?)\s+RLEV\s+(?<CURVE_T_RELV>.*?)\s+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        public static Dictionary<string, string> ReadCurveInfo(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var dic = new Dictionary<string, string>();
                var fi = new FileInfo(filePath);
                var sr = fi.OpenText();
                var groups = curveRegex.Match(sr.ReadToEnd()).Groups;
                sr.Close();
                foreach (string s in curveRegex.GetGroupNames())
                {
                    if (s != "0")
                        dic.Add(s, groups[s].Value);
                }
                return dic;
            }
            return null;
        }

    }
}
