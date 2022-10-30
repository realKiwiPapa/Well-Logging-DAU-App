using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Web;
using System.Xml.Linq;

namespace Logging_App.Utility
{
    public class ExcelHelper
    {
        private Excel.Application _excelApp = null;
        private Excel.Workbooks _books = null;
        private Excel._Workbook _book = null;
        private Excel.Sheets _sheets = null;
        private Excel._Worksheet _sheet = null;
        private Excel.Range _range = null;
        private Excel.Font _font = null;
        // Optional argument variable
        private object _optionalValue = Missing.Value;

        private void ReleaseCOM(object pObj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pObj);
            }
            catch
            {
                throw new Exception("释放资源时发生错误！");
            }
            finally
            {
                pObj = null;
            }
        }

        public void SaveToExcel(string excelName, DataGrid datagrid)
        {
            try
            {
                if (datagrid.HasItems)
                {
                    if (datagrid.Items.Count != 0)
                    {
                        Mouse.SetCursor(Cursors.Wait);
                        CreateExcelRef();
                        FillSheet(datagrid);
                        SaveExcel(excelName);
                        Mouse.SetCursor(Cursors.Arrow);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("生成Excel文件时发生错误!!");
            }
            finally
            {
                ReleaseCOM(_sheet);
                ReleaseCOM(_sheets);
                ReleaseCOM(_book);
                ReleaseCOM(_books);
                ReleaseCOM(_excelApp);
            }
        }

        private void SaveExcel(string excelName)
        {
            _excelApp.Visible = true;
            //保存为Office2003和Office2007都兼容的格式
            _book.SaveAs(excelName, 56, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //_book.Save();
            //_excelApp.Quit();

        }

        /// <summary>
        /// 将数据填充到内存Excel的工作表
        /// </summary>
        /// <param name="datagrid"></param>
        private void FillSheet(DataGrid datagrid)
        {
            object[] header = CreateHeader(datagrid);
            WriteData(header, datagrid);
        }


        private void WriteData(object[] header, DataGrid datagrid)
        {
            object[,] objData = new object[datagrid.Items.Count, header.Length];
            List<object> cells = new List<object>(header.Length);

            var dt = (datagrid.ItemsSource as DataView).Table;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                var dr = dt.Rows[j];
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    cells.Add(dr[k]);
                }
                for (int i = 0; i < header.Length; i++)
                {
                    objData[j, i] = cells[i];
                }
                cells.Clear();
            }
            AddExcelRows("A2", datagrid.Items.Count, header.Length, objData);
            AutoFitColumns("A1", datagrid.Items.Count + 1, header.Length);
        }


        private void AutoFitColumns(string startRange, int rowCount, int colCount)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.Columns.AutoFit();
        }


        private object[] CreateHeader(DataGrid datagrid)
        {

            List<object> objHeaders = new List<object>();
            for (int n = 0; n < datagrid.Columns.Count; n++)
            {
                objHeaders.Add(datagrid.Columns[n].Header);
            }

            var headerToAdd = objHeaders.ToArray();
            //工作表的单元是从“A1”开始
            AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
            SetHeaderStyle();

            return headerToAdd;
        }

        /// 将表头加粗显示
        private void SetHeaderStyle()
        {
            _font = _range.Font;
            _font.Bold = true;
        }

        /// 将数据填充到Excel工作表的单元格中
        private void AddExcelRows(string startRange, int rowCount, int colCount, object values)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.set_Value(_optionalValue, values);
        }

        /// 创建一个Excel程序实例
        private void CreateExcelRef()
        {
            _excelApp = new Excel.Application();
            _books = (Excel.Workbooks)_excelApp.Workbooks;
            _book = (Excel._Workbook)(_books.Add(_optionalValue));
            _sheets = (Excel.Sheets)_book.Worksheets;
            _sheet = (Excel._Worksheet)(_sheets.get_Item(1));
        }

    }
}
