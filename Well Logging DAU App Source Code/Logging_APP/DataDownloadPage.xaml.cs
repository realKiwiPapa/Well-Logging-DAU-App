using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Threading;
using System.IO;
using System.Net;

namespace Logging_App
{
    /// <summary>
    /// DataDownloadPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataDownloadPage : Page
    {
        public DataDownloadPage()
        {
            InitializeComponent();
        }

        private void LoadDataList()
        {
            using (var dataser = new DataServiceHelper())
            {
                dataGrid1.ItemsSource = dataser.GetFileDownloadList(Well_Job_Name.Text, Well_Struct_Unit_Name.Text, Part_Units.Text, Process_Name.Text).Tables[0].DefaultView;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataList();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LoadDataList();
        }

        static UploadWindow window = null;
        Utility.DataCollection<DownloadController.DownloadTask> tasks = new Utility.DataCollection<DownloadController.DownloadTask>();
        string selectedPath;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rows = (dataGrid1.ItemsSource as DataView).Table.Select("CHOISE=true");
            if (rows.Length < 1)
            {
                MessageBox.Show("请选择数据！");
                return;
            }
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "选择文件保存目录";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = Path.Combine(fbd.SelectedPath, "数据批量下载" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                window = new UploadWindow();
                window.Title = "文件下载";
                window.Owner = App.Current.MainWindow;
                window.dataGrid.ItemsSource = tasks;
                ThreadPool.QueueUserWorkItem(Download, rows);
                window.ShowDialog();
            }
        }

        private void Download(object obj)
        {
            foreach (var row in obj as DataRow[])
            {
                string processName = row.Field<string>("PROCESS_NAME");
                using (var dataser = new DataServiceHelper())
                {
                    foreach (DataRow dr in dataser.GetFileDownloadDetails(row.Field<string>("PROCESS_ID")).Tables[0].Rows)
                    {
                        string filePath = dr.Field<string>("FILEPATH");
                        string fileName = dr.Field<string>("FILENAME");
                        if (filePath != null)
                        {
                            int index = filePath.ToLower().IndexOf(processName.ToLower());
                            if (index > -1)
                                filePath = filePath.Remove(0, index + processName.Length);
                            filePath = filePath.Trim('\\');
                        }
                        else
                        {
                            filePath = string.Empty;
                        }
                        var task = new DownloadController.DownloadTask();
                        try
                        {
                            HttpWebRequest httpRequest = WebRequest.Create(WebServiceConfig.BaseUrl + "FileDownload.ashx") as HttpWebRequest;
                            httpRequest.Method = "POST";
                            httpRequest.Timeout = 3600000 * 10;
                            string param = "id=" + Uri.EscapeDataString(dr.Field<decimal>("PROCESS_UPLOAD_ID").ToString());
                            httpRequest.ContentType = "application/x-www-form-urlencoded";
                            byte[] bs = Encoding.UTF8.GetBytes(param);
                            httpRequest.ContentLength = bs.Length;
                            httpRequest.CookieContainer = WebServiceConfig.CookieContainer;
                            using (Stream requestStream = httpRequest.GetRequestStream())
                            {
                                requestStream.Write(bs, 0, bs.Length);
                            }
                            var response = httpRequest.GetResponse();
                            task.Size = Utility.UnitConversion.HumanReadableByteCount(response.ContentLength);
                            string path = Path.Combine(selectedPath, row.Field<string>("WELL_JOB_NAME"), processName, filePath);
                            Directory.CreateDirectory(path);
                            string fullName = Path.Combine(path, fileName);
                            int i = 0;
                            while (File.Exists(fullName))
                            {
                                i += 1;
                                int index = fileName.LastIndexOf('.');
                                if (index > 0)
                                    fileName = fileName.Insert(index, "（" + i + "）");
                                else
                                    fileName += "（" + i + "）";
                                fullName = Path.Combine(path, fileName);
                            }
                            task.Name = fileName;
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                tasks.Add(task);
                                window.dataGrid.ScrollIntoView(task);
                            }));
                            using (var fs = new FileStream(fullName, FileMode.OpenOrCreate, FileAccess.Write,FileShare.None,102400))
                            {
                                fs.SetLength(0);
                                fs.Position = 0;
                                using (Stream responseStream = response.GetResponseStream())
                                {
                                    byte[] buffer = new byte[1024];
                                    int read = 0;
                                    task.Msg = "下载中";
                                    task.Length = response.ContentLength;
                                    do
                                    {
                                        read = responseStream.Read(buffer, 0, 1024);
                                        fs.Write(buffer, 0, read);
                                        //task.Progress = Math.Round(fs.Position * 1.0 / response.ContentLength * 100, 2, MidpointRounding.AwayFromZero);
                                        task.CompletedSizeNumber = fs.Position;
                                        task.CompletedSize = Utility.UnitConversion.HumanReadableByteCount(fs.Position);
                                    }
                                    while (read > 0);
                                    task.Msg = "下载完成";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            task.hasError = true;
                            task.Msg = "下载出错";
                        }
                    }
                }
            }
            this.Dispatcher.Invoke(new Action(() =>
            {
                window.UploadCompleted = true;
                window.Close();
            }));
            System.Diagnostics.Process.Start(selectedPath);
        }
    }
}
