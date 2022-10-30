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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using Logging_App.Utility;
using System.Threading.Tasks;
using System.Threading;

namespace Logging_App
{
    /// <summary>
    /// FileRecoveyPage.xaml 的交互逻辑
    /// </summary>
    public partial class FileRecoveyPage : Page
    {
        private HashHelper hashhelper;
        private IAsyncResult result;
        private const int sha1Size = 256 * 1024;
        private List<FileInfomation> fileList = new List<FileInfomation>();
        public FileRecoveyPage()
        {
            InitializeComponent();
        }

        protected class FileInfomation
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string FullName { get; set; }
            public string HashName { get; set; }
            public int Length { get; set; }
        }

        private enum LogType
        {
            恢复文件 = 0,
            数据库校对 = 1,
            目录校对 = 2
        }

        private void WriteLog(TextBox target, LogType lt, string msg, bool isWrite = true)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                target.AppendText(msg + "\n");
                target.ScrollToEnd();
            }));
            if (isWrite)
            {
                string logname = string.Empty;
                switch ((int)lt)
                {
                    case 0:
                        logname = "recovery_log.txt";
                        break;
                    case 1:
                        logname = "compare_db_log.txt";
                        break;
                    case 2:
                        logname = "compare_file_log.txt";
                        break;
                    default:
                        break;
                }
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + logname, msg + "\r\n");
            }
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath.Text))
            {
                MessageBox.Show(App.Current.MainWindow, "路径不能为空！！");
                return;
            }
            var tasks = new DataCollection<UploadController.UploadTask>();
            Task.Factory.StartNew(() => UploadFiles(tasks)).ContinueWith(t => { var exp = t.Exception; });
            Thread.Sleep(100);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private List<FileInfomation> GetAllFiles(DirectoryInfo dir)
        {
            var files = dir.GetFiles();
            foreach (var fi in files)
            {
                using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024))
                {
                    var sha1 = HashHelper.ComputeSHA1(fs, sha1Size);
                    fs.Position = 0;
                    hashhelper = new HashHelper(fs);
                    result = hashhelper.AsyncComputeMD5();
                    while (!result.AsyncWaitHandle.WaitOne(300)) { }
                    var md5 = hashhelper.Hash;
                    fileList.Add(new FileInfomation
                    {
                        FileName = fi.Name,
                        FullName = fi.FullName,
                        FilePath = fi.DirectoryName.Remove(0, fi.Directory.Root.Name.Length),
                        HashName = string.Format("{0}-{1}-{2}", sha1, md5, fi.Length),
                        Length = (int)fi.Length
                    });
                }
            }
            var allDir = dir.GetDirectories();
            foreach (var d in allDir)
            {
                GetAllFiles(d);
            }
            return fileList;
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            var folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                filePath.Text = folder.SelectedPath;
            else
                filePath.Text = string.Empty;
        }

        private void UploadFiles(DataCollection<UploadController.UploadTask> tasks)
        {
            DirectoryInfo base_dir = null;
            this.Dispatcher.Invoke(new Action(() =>
            {
                base_dir = new DirectoryInfo(filePath.Text);
            }));
            var well_dir = base_dir.GetDirectories();
            fileList.Clear();
            using (var dataser = new DataServiceHelper())
            {
                //井目录
                foreach (var well in well_dir)
                {
                    //目录下所有文件
                    var list = GetAllFiles(well);
                    //数据库井下所有文件
                    var dt = dataser.GetAllProcessFileByWellName(well.Name).Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var filestate = dt.Rows[i].FieldEx<decimal>("filestate");
                        var process_name = dt.Rows[i].FieldEx<string>("process_name");
                        var fileid = dt.Rows[i].FieldEx<decimal>("fileid");
                        var filename = dt.Rows[i].FieldEx<string>("filename");
                        var hashname = dt.Rows[i].FieldEx<string>("hashname");
                        var length = dt.Rows[i].FieldEx<decimal>("length");
                        var filepath = dt.Rows[i].FieldEx<string>("filepath");
                        var fi = list.Where(x => x.HashName.Equals(hashname) || (x.FileName.Equals(filename) && x.Length == length)).FirstOrDefault();
                        string msg = string.Empty;
                        if (fi == null)
                        {
                            msg = string.Format("提示:数据库：井({0})-井次({1})-文件({2},{3}),恢复目录中无此文件！！", well.Name, process_name, filename, length);
                        }
                        else
                        {
                            //如果服务器上存在文件
                            if (filestate > 0 && fi.FilePath.Equals(filepath)) continue;
                            if (tasks.Count(x => x.FullName.Equals(fi.FullName)) > 0) continue;
                            tasks.Add(new UploadController.UploadTask { OldFileID = fileid, FullName = fi.FullName });
                        }
                        if (!string.IsNullOrEmpty(msg))
                        {
                            WriteLog(textBox1, LogType.恢复文件, msg);
                        }
                    }
                }
            }
            if (tasks.Count > 0)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    new FileUpload().BeginUpload(tasks);
                }));
                var taskList = tasks.ToList();
                if (taskList.Find(t => t.hasError) != null)
                {
                    ShowMsgBox("上传出错！！");
                    return;
                }

                using (var dataser = new DataServiceHelper())
                {
                    //更新sys_processing_fileupload，清理旧数据
                    var fileIds = tasks.ToDictionary(k => k.OldFileID, v => v.FileID);
                    if (fileIds.Count > 0)
                        dataser.UpdateProcessFileInfo(ModelHelper.SerializeObject(fileIds));
                }
                ShowMsgBox("文件恢复完成！！");
            }
            else
            {
                ShowMsgBox("没有需要恢复的文件！！");
            }
        }

        private void ShowMsgBox(string msg)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                MessageBox.Show(App.Current.MainWindow, msg);
            }));
        }

        DataTable dt1 = null;
        DataTable dt2 = null;
        private void Compare_Click(object sender, RoutedEventArgs e)
        {
            var tag = Convert.ToInt32((sender as Button).Tag);
            using (var dataser = new DataServiceHelper())
            {
                if (tag == 1)
                {
                    textBox2.Clear();
                    dt1 = dataser.CompareDataFiles(1).Tables[0];
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        var well_name = dt1.Rows[i].FieldEx<string>("Well_Name");
                        var process_name = dt1.Rows[i].FieldEx<string>("Process_Name");
                        var file_name = dt1.Rows[i].FieldEx<string>("File_Name");
                        string msg = string.Empty;
                        if (process_name == null)
                        {
                            msg = string.Format("提示：数据库：井({0})-文件({1}),对应上传目录中无此文件！！", well_name, file_name);
                        }
                        else
                            msg = string.Format("提示：数据库：井({0})-井次({1})-文件({2}),对应上传目录中无此文件！！", well_name, process_name, file_name);
                        WriteLog(textBox2, LogType.数据库校对, msg);
                    }
                }
                else
                {
                    textBox3.Clear();
                    dt2 = dataser.CompareDataFiles(2).Tables[0];
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        var file_name = dt2.Rows[i].FieldEx<string>("File_Name");
                        var msg = string.Format("提示:上传目录中文件:{0},对应数据库中无此文件信息！！", file_name);
                        WriteLog(textBox3, LogType.目录校对, msg);
                    }
                }
                MessageBox.Show(App.Current.MainWindow, "校对完成！！");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var tag = Convert.ToInt32((sender as Button).Tag);
            using (var dataser = new DataServiceHelper())
            {
                if (tag == 1)
                {
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        var result = MessageBox.Show("确定要删除数据库中无效的文件信息？", "提示", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            if (dt1.Rows.Count > 0)
                                dataser.DelSysUpload(dt1);
                            MessageBox.Show("删除完成！！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("数据库中不存在无效的文件信息！！");
                    }
                }
                else
                {
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        var result = MessageBox.Show("确定要删除上传目录中无效的文件？", "提示", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            if (dt2.Rows.Count > 0)
                                dataser.DelUploadFile(dt2);
                            MessageBox.Show("删除完成！！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("上传目录中不存在无效的文件！！");
                    }
                }
            }
        }
    }
}
