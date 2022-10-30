using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Logging_App
{
    public abstract class DownloadController
    {
        private delegate void AsyncDownloadCompleted(Utility.DataCollection<DownloadTask> tasks);
        protected abstract string TaskType { get; }
        protected abstract string DownloadPath { get; }
        protected abstract void DownloadCompleted(Utility.DataCollection<DownloadTask> tasks);
        public void BeginDownload(Utility.DataCollection<DownloadTask> tasks)
        {
            if (tasks == null || tasks.Count < 1) return;
            ThreadPool.QueueUserWorkItem(Download, tasks);
        }
        private void Download(object tasksobj)
        {
            var tasks = tasksobj as Utility.DataCollection<DownloadTask>;
            //var downloadCompleted = new AsyncDownloadCompleted(DownloadCompleted);
            FileStream fs = null;
            try
            {
                if (!Directory.Exists(DownloadPath))
                    Directory.CreateDirectory(DownloadPath);
                foreach (DownloadTask t in tasks)
                {
                    try
                    {
                        using (fs = new FileStream(DownloadPath + t.Name + ".temp", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 1024 * 1024))
                        {
                            fs.SetLength(0);
                            fs.Position = 0;
                            t.CompletedSize = "0 B";
                            using (var fileSer = new FileServiceHelper())
                            {
                                t.Msg = "下载中";
                                var task = fileSer.CreateDownloadTask(t.FileID, t.Name, TaskType);
                                //var length = task.Length;
                                t.Length = task.Length;
                                t.Size = Utility.UnitConversion.HumanReadableByteCount(t.Length); ;
                                while (true)
                                {
                                    if (task.State == 0) fileSer.CreateDownloadTask(t.FileID, t.Name, TaskType);
                                    if (task.State == 2)
                                    {
                                        t.hasError = true;
                                        t.Msg = "文件不存在";
                                        break;
                                    }
                                    //t.Progress = Math.Round(fs.Position * 1.0 / length * 100, 2, MidpointRounding.AwayFromZero);
                                    t.CompletedSizeNumber = fs.Position;
                                    t.CompletedSize = Utility.UnitConversion.HumanReadableByteCount(fs.Position);
                                    if (t.Length == fs.Position)
                                    {
                                        //t.CompletedSizeNumber = fs.Length;
                                        t.Msg = "下载完成";
                                        break;
                                    }
                                    task = fileSer.DownloadRead(task.TaskID, fs.Position);
                                    fs.Write(task.Data, 0, task.Data.Length);
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        t.Msg = ex.Message;
                        t.hasError = true;
                    }
                }

                //downloadCompleted.BeginInvoke(tasks, null, null);
            }
            catch
            {
                throw;
            }
            finally
            {
                DownloadCompleted(tasks);
            }
        }
        public class DownloadTask : Model.ModelBase
        {
            public bool hasError = false;
            public decimal? FileID { get; set; }
            public string Name
            {
                get;
                set;
            }
            public double Progress
            {
                get
                {
                    return Math.Round(CompletedSizeNumber * 1.0 / Length * 100, 2, MidpointRounding.AwayFromZero);
                }
            }
            public long Length { get; set; }

            private string _size;
            public string Size
            {
                get { return _size; }
                set
                {
                    _size = value;
                    NotifyPropertyChanged("Size");
                }
            }

            private string _completedsize;
            public string CompletedSize
            {
                get { return _completedsize; }
                set
                {
                    _completedsize = value;
                    NotifyPropertyChanged("CompletedSize");
                }
            }

            private long _completedSizeNumber;
            public long CompletedSizeNumber
            {
                get { return _completedSizeNumber; }
                set
                {
                    _completedSizeNumber = value;
                    NotifyPropertyChanged("Progress");
                }
            }

            private string _msg = "等待";
            public string Msg
            {
                get { return _msg; }
                set
                {
                    _msg = value;
                    NotifyPropertyChanged("Msg");
                }
            }
            public object Tag { get; set; }
        }
    }
}
