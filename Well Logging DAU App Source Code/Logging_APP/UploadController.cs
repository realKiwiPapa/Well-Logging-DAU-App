using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows;
using Logging_App.Utility;

namespace Logging_App
{
    public abstract class UploadController
    {
        private const int sha1Size = 256 * 1024;
        private UploadWindow uploadWindow = null;
        private static int bufferSize = 0;
        private delegate void AsyncUploadCompleted(UploadTask task);
        protected abstract string TaskType { get; }
        protected abstract void UploadCompleted(UploadTask task);
        public void BeginUpload(Utility.DataCollection<UploadTask> tasks)
        {
            if (tasks == null || tasks.Count < 1) return;
            uploadWindow = new UploadWindow();
            uploadWindow.Owner = App.Current.MainWindow;
            uploadWindow.dataGrid.ItemsSource = tasks;
            ThreadPool.QueueUserWorkItem(Upload, tasks);
            uploadWindow.ShowDialog();
        }
        private void Upload(object tasksobj)
        {
            var tasks = tasksobj as Utility.DataCollection<UploadTask>;
            if (bufferSize < 1)
            {
                int speed = Utility.TestSpeed.GetUploadSpeed();
                if (speed > 1024 * 1024)
                    bufferSize = 1024 * 1024;
                else if (speed < 5 * 1024)
                    bufferSize = 5 * 1024;
                else
                    bufferSize = speed;
                //System.Windows.MessageBox.Show(bufferSize.ToString(), "测试");
            }
            //try
            //{
            //var uploadCompleted = new AsyncUploadCompleted(UploadCompleted);
            IAsyncResult result;
            //FileStream fs = null;
            byte[] bData = new byte[bufferSize];
            Utility.HashHelper hashhelper;
            bool retry = false;
            foreach (UploadTask t in tasks)
            {
                do
                {
                    try
                    {
                        using (var fs = new FileStream(t.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024))
                        {
                            var fi = new FileInfo(t.FullName);
                            t.path = fi.DirectoryName.Remove(0, fi.Directory.Root.Name.Length);
                            t.CompletedSize = "0 B";
                            t.Size = Utility.UnitConversion.HumanReadableByteCount(fs.Length);
                            t.Length = fs.Length;
                            t.Msg = "计算中";
                            t.SHA1 = Utility.HashHelper.ComputeSHA1(fs, sha1Size);
                            fs.Position = 0;
                            hashhelper = new Utility.HashHelper(fs);
                            result = hashhelper.AsyncComputeMD5();
                            
                            while (!result.AsyncWaitHandle.WaitOne(300))
                            {
                                t.CompletedSizeNumber = fs.Position;
                                //t.Progress = Math.Round(fs.Position * 1.0 / fs.Length * 100, 2, MidpointRounding.AwayFromZero); //hashhelper.Progress;
                            }
                            t.MD5 = hashhelper.Hash;
                            do
                            {
                                try
                                {
                                    t.Msg = "上传中";
                                    t.CompletedSizeNumber = 0;
                                    fs.Position = 0;
                                    DateTime startTime;
                                    DateTime beginTime;
                                    TimeSpan timeUsed;
                                    //TimeSpan tempTime;
                                    TimeSpan timeLeft;
                                    long uploadCount = 0;
                                    double speed;
                                    using (var fileSer = new FileServiceHelper())
                                    {
                                        var task = fileSer.CreateUploadTask(t.SHA1, t.MD5, fs.Length, TaskType);
                                        startTime = DateTime.Now;
                                        beginTime = startTime;
                                        while (true)
                                        {
                                            if (task.State == 0 || task.State == 4) task = fileSer.CreateUploadTask(t.SHA1, t.MD5, fs.Length, TaskType);
                                            if (task.Position != fs.Position)
                                                fs.Position = task.Position;
                                            if (task.State != 5) t.CompletedSize = Utility.UnitConversion.HumanReadableByteCount(fs.Position);
                                            //t.Progress = Math.Round(fs.Position * 1.0 / fs.Length * 100, 2, MidpointRounding.AwayFromZero);
                                            t.CompletedSizeNumber = fs.Position;
                                            if (task.State == 3)
                                            {
                                                t.CompletedSizeNumber = fs.Length;
                                                t.Msg = "上传完成";
                                                //uploadCompleted.BeginInvoke(t, null, null);
                                                UploadCompleted(t);
                                                break;
                                            }
                                            if (task.State == 2 || task.State == 5)
                                            {
                                                if (task.State == 5)
                                                {
                                                    t.Msg = "校验中";
                                                }
                                                Thread.Sleep(1000);
                                                task = fileSer.GetUploadState(task.TaskID);
                                                continue;
                                            }
                                            int count = fs.Read(bData, 0, bData.Length);
                                            if (count < bData.Length)
                                            {
                                                byte[] b = new byte[count];
                                                Buffer.BlockCopy(bData, 0, b, 0, count);
                                                task = fileSer.UploadWrite(task.TaskID, task.Position, b);
                                            }
                                            else
                                                task = fileSer.UploadWrite(task.TaskID, task.Position, bData);
                                            uploadCount += count;
                                            if ((DateTime.Now - beginTime).TotalSeconds >= 1)
                                            {
                                                timeUsed = DateTime.Now - startTime;
                                                if (timeUsed.Days > 0)
                                                    t.TimeUsed = timeUsed.ToString(@"d\.hh\:mm\:ss");
                                                else
                                                    t.TimeUsed = timeUsed.ToString(@"hh\:mm\:ss");
                                                speed = uploadCount / timeUsed.TotalSeconds;
                                                timeLeft = TimeSpan.FromSeconds((fs.Length - fs.Position) / speed);
                                                if (timeLeft.Days > 0)
                                                    t.TimeLeft = timeLeft.ToString(@"d\.hh\:mm\:ss");
                                                else
                                                    t.TimeLeft = timeLeft.ToString(@"hh\:mm\:ss");
                                                t.Speed = Utility.UnitConversion.HumanReadableByteCount((int)(speed)) + "/S";
                                                beginTime = DateTime.Now;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    retry = false;
                                    if (UploadErrorMessageBox.Show(uploadWindow, t.Name + "\n" + ex.Message) == MessageBoxResult.Yes)
                                        retry = true;
                                    else
                                    {
                                        t.Msg = ex.Message;
                                        t.hasError = true;
                                        goto end;
                                    }
                                }
                            }
                            while (retry);
                        }
                    }
                    catch (Exception ex)
                    {
                        retry = false;
                        if (UploadErrorMessageBox.Show(uploadWindow, t.Name + "\n" + ex.Message) == MessageBoxResult.Yes)
                            retry = true;
                        else
                        {
                            t.Msg = ex.Message;
                            t.hasError = true;
                            goto end;
                        }
                    }
                }
                while (retry);
            }
        //}
        //finally
        //{
        end:
            uploadWindow.UploadCompleted = true;
            //if (tasks.ToList().Find(t => t.hasError) == null)
            uploadWindow.Dispatcher.Invoke(new Action(() =>
            {
                uploadWindow.Close();
            }));
            //}
        }
        public class UploadTask : Model.ModelBase
        {
            public string SHA1;
            public string MD5;
            public string path;

            //private long _length;
            public long Length { get; set; }
            //{
            //    get { return _length; }
            //    set
            //    {
            //        _length = value;
            //        NotifyPropertyChanged("Length");
            //    }
            //}

            public decimal FileID { get; set; }
            public decimal OldFileID { get; set; }

            public bool hasError = false;
            private string _name;
            public string Name
            {
                get { return _name; }
            }

            private string _fullname;
            public string FullName
            {
                get { return _fullname; }
                set
                {
                    _fullname = value;
                    NotifyPropertyChanged("FullName");
                    _name = Path.GetFileName(_fullname);
                    NotifyPropertyChanged("Name");
                }
            }

            //private double _progress;
            public double Progress
            {
                get
                {
                    return Math.Round(CompletedSizeNumber * 1.0 / Length * 100, 2, MidpointRounding.AwayFromZero);
                }
                //set
                //{
                //    _progress = value;
                //    NotifyPropertyChanged("Progress");
                //}
            }

            //private long _sizeNumber;
            //public long SizeNumber
            //{
            //    get { return _sizeNumber; }
            //    set
            //    {
            //        _sizeNumber = value;
            //        NotifyPropertyChanged("SizeNumber");
            //    }
            //}

            private long _completedSizeNumber;
            public long CompletedSizeNumber
            {
                get { return _completedSizeNumber; }
                set
                {
                    _completedSizeNumber = value;
                    //NotifyPropertyChanged("CompletedSizeNumber");
                    NotifyPropertyChanged("Progress");
                }
            }

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

            private string _timeUsed;
            public string TimeUsed
            {
                get { return _timeUsed; }
                set
                {
                    _timeUsed = value;
                    NotifyPropertyChanged("TimeUsed");
                }
            }
            private string _timeLeft;
            public string TimeLeft
            {
                get { return _timeLeft; }
                set
                {
                    _timeLeft = value;
                    NotifyPropertyChanged("TimeLeft");
                }
            }
            private string _speed;
            public string Speed
            {
                get { return _speed; }
                set
                {
                    _speed = value;
                    NotifyPropertyChanged("Speed");
                }
            }
        }
    }
}
