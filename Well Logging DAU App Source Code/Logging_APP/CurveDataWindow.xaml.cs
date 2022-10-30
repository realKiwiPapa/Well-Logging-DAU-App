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
using Logging_App.Utility;

namespace Logging_App
{
    /// <summary>
    /// CurveDataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CurveDataWindow : System.Windows.Window
    {
        public CurveDataWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var excelHelper = new ExcelHelper();
            var sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Excel(*XLS)|*.xls";
            sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.CheckPathExists = true;
            if ((bool)sfd.ShowDialog())
            {
                try
                {
                    excelHelper.SaveToExcel(sfd.FileName, datagrid1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                }
                finally
                {
                    this.Close();
                }
            }
        }
    }
}
