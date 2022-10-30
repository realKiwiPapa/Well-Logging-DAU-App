using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Logging_App
{
    public class UpdateDownload : DownloadController
    {
        static string rootPath = AppDomain.CurrentDomain.BaseDirectory;
        static AutoUpdateWindow autoUpdateWindow = null;
        private static string version;
        public static string Version { get { return version; } }
        public static void Update()
        {
            var bytedata = new byte[20];
            var exefile = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            var fs = new FileStream(exefile, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 512);
            fs.Position = fs.Length - 20;
            fs.Read(bytedata, 0, 20);
            version = Encoding.ASCII.GetString(bytedata);
            string ver;
            using (var fileser = new FileServiceHelper())
            {
                ver = fileser.GetVersion();
                if (string.IsNullOrEmpty(ver) || version == ver.PadRight(20, ' ')) return;
                version = ver;
                var data = fileser.GetUpdateData();
                var tasks = new Utility.DataCollection<DownloadTask>();
                foreach (var d in data)
                {
                    tasks.Add(new DownloadTask { Name = d.FilePath });
                }

                foreach (var fileinfo in new DirectoryInfo(rootPath).GetFiles("*.old", SearchOption.AllDirectories))
                {
                    fileinfo.Delete();
                }
                if (tasks == null || tasks.Count < 1) return;
                autoUpdateWindow = new AutoUpdateWindow();
                autoUpdateWindow.dataGrid.ItemsSource = tasks;
                new UpdateDownload().BeginDownload(tasks);
                autoUpdateWindow.ShowDialog();
            }

        }

        protected override string TaskType
        {
            get { return "UpdateDownload"; }
        }

        protected override string DownloadPath
        {
            get { return rootPath; }
        }
        protected override void DownloadCompleted(Utility.DataCollection<DownloadController.DownloadTask> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.Length != task.CompletedSizeNumber) continue;
                string fullname = rootPath + task.Name;
                try
                {
                    File.Delete(fullname);
                }
                catch
                {
                    File.Move(fullname, fullname + ".old");
                }
                File.Move(fullname + ".temp", fullname);
                if (fullname.ToLower().EndsWith(".exe"))
                {
                    using (var fs = new FileStream(fullname, FileMode.Append, FileAccess.Write, FileShare.Read, 1024 * 512))
                    {
                        fs.Write(Encoding.ASCII.GetBytes(Version.PadRight(20, ' ')), 0, 20);
                    }
                }
            }
            autoUpdateWindow.UpdateCompleted();
        }
    }

    public class ViewFile : DownloadController
    {
        static ViewFileWindow vfw = null;
        static string rootPath = AppDomain.CurrentDomain.BaseDirectory + "temp\\~~viewfile";
        string downloadPath = string.Format("{0}\\{1}\\", rootPath, Path.GetRandomFileName());
        protected override string DownloadPath
        {
            get { return downloadPath; }
        }

        protected override string TaskType
        {
            get { return "FileDownload"; }
        }

        public static void View(DownloadController.DownloadTask task)
        {
            if (task.FileID == null) return;
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                using (var fileser = new FileServiceHelper())
                {
                    task.Name = fileser.GetFileName((decimal)task.FileID);
                }
            }
            try
            {
                Directory.Delete(rootPath, true);
            }
            catch { }
            vfw = new ViewFileWindow();
            vfw.Owner = App.Current.MainWindow;
            vfw.DataContext = task;
            new ViewFile().BeginDownload(new Utility.DataCollection<DownloadTask> { task });
            vfw.ShowDialog();
        }
        protected override void DownloadCompleted(Utility.DataCollection<DownloadController.DownloadTask> tasks)
        {
            vfw.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        if (tasks[0].hasError)
                        {
                            MessageBox.Show(tasks[0].Msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        string fullname = DownloadPath + tasks[0].Name;
                        try
                        {
                            File.Delete(fullname);
                            File.Move(fullname + ".temp", fullname);
                        }
                        catch { }

                        Utility.WinShell.OpenFile(fullname);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "文件打开失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        vfw.Dispatcher.Invoke(new Action(() => { vfw.Close(); }));
                    }
                }));
        }
    }

    public class FileDownload : DownloadController
    {
        static UploadWindow window = null;
        protected override string TaskType
        {
            get { return "FileDownload"; }
        }

        public static void Start(Utility.DataCollection<DownloadTask> tasks)
        {
            if (tasks == null || tasks.Count < 1) return;
            tasks.RemoveAll(t => t.FileID == null || t.FileID < 1);
            if (tasks.Count == 0) return;
            using (var fileser = new FileServiceHelper())
            {
                foreach (var task in tasks)
                {
                    if (string.IsNullOrWhiteSpace(task.Name))
                    {
                        task.Name = fileser.GetFileName((decimal)task.FileID);
                    }
                }
            }
            tasks.RemoveAll(t => string.IsNullOrWhiteSpace(t.Name));
            if (tasks.Count == 0) return;

            var sbd = new System.Windows.Forms.FolderBrowserDialog();
            sbd.ShowNewFolderButton = true;
            sbd.Description = "选择文件保存目录";
            if (sbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                window = new UploadWindow();
                window.Title = "文件下载";
                window.Owner = App.Current.MainWindow;
                window.dataGrid.ItemsSource = tasks;
                var fd = new FileDownload();
                fd.downloadPath = sbd.SelectedPath + "\\";
                fd.BeginDownload(tasks);
                window.ShowDialog();
            }
        }

        public string downloadPath;
        protected override string DownloadPath
        {
            get { return downloadPath; }
        }

        protected override void DownloadCompleted(Utility.DataCollection<DownloadController.DownloadTask> tasks)
        {
            window.Dispatcher.Invoke(new Action(() =>
            {
                window.UploadCompleted = true;
                foreach (var task in tasks)
                {
                    if (task.hasError) continue;
                    try
                    {
                        File.Delete(DownloadPath + task.Name);
                        File.Move(DownloadPath + task.Name + ".temp", DownloadPath + task.Name);
                    }
                    catch { }
                }
                Process.Start(DownloadPath);
            }));
        }
    }

    public class PluginDownload : DownloadController
    {
        public static readonly string RootPath = AppDomain.CurrentDomain.BaseDirectory + "plugins\\";
        static AutoUpdateWindow autoUpdateWindow = null;
        private static FileService.PluginInfo[] pluginsInfo;
        public static FileService.PluginInfo[] PluginsInfo { get { return pluginsInfo; } }
        public static void Update()
        {
            var dir = new DirectoryInfo(RootPath);
            if (!dir.Exists)
                dir.Create();
            using (var fileser = new FileServiceHelper())
            {
                pluginsInfo = fileser.GetPluginsInfo();
            }
            foreach (var d in dir.GetDirectories())
            {
                if (pluginsInfo.FirstOrDefault(x => x.Name == d.Name) == null)
                    try
                    {
                        d.Delete(true);
                    }
                    catch { }
            }
            var tasks = new Utility.DataCollection<DownloadTask>();
            foreach (var plugin in pluginsInfo)
            {
                var pluginDir = new DirectoryInfo(RootPath + plugin.Name);
                if (!pluginDir.Exists) pluginDir.Create();
                foreach (var file in pluginDir.GetFiles())
                {
                    if (plugin.ItemsInfo.FirstOrDefault(x => x.Name == file.Name) == null)
                        file.Delete();
                }
                foreach (var item in plugin.ItemsInfo)
                {
                    var fileInfo = new FileInfo(RootPath + plugin.Name + "\\" + item.Name);
                    if (fileInfo.Exists && fileInfo.LastWriteTime == item.LastWriteTime) continue;
                    tasks.Add(new DownloadTask { Name = plugin.Name + "\\" + item.Name, Tag = item.LastWriteTime });
                }
            }
            if (tasks == null || tasks.Count < 1) return;
            autoUpdateWindow = new AutoUpdateWindow();
            autoUpdateWindow.Title = "扩展更新中";
            autoUpdateWindow.dataGrid.ItemsSource = tasks;
            new PluginDownload().BeginDownload(tasks);
            autoUpdateWindow.ShowDialog();
        }

        protected override string TaskType
        {
            get { return "PluginDownload"; }
        }

        protected override string DownloadPath
        {
            get { return RootPath; }
        }
        protected override void DownloadCompleted(Utility.DataCollection<DownloadController.DownloadTask> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.Length != task.CompletedSizeNumber) continue;
                string fullname = RootPath + task.Name;
                try
                {
                    File.Delete(fullname);
                }
                catch
                {
                    File.Move(fullname, fullname + ".old");
                }
                File.Move(fullname + ".temp", fullname);
                File.SetLastWriteTime(fullname, (DateTime)task.Tag);
            }
            autoUpdateWindow.UpdateCompleted();
        }
    }
}
