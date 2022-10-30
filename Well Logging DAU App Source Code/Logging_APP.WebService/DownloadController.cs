using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Timers;

namespace Logging_App.WebService
{
    public abstract class DownloadController
    {
        public struct DownloadState
        {
            public int TaskID;
            /// <summary>
            /// 0 任务不存在
            /// 1 下载中
            /// 2 文件不存在
            /// 3 下载完成
            /// </summary>
            public int State;
            public byte[] Data;
            public long Length;
        }
        private const int taskTimeOut = 20;
        private static Timer timer = null;
        private static List<DownloadController> tasklist = new List<DownloadController>();
        private FileStream fileStream = null;
        private byte[] byteData = new byte[1024 * 30];
        #region
        private int _id;
        public int ID { get { return _id; } }
        private long _length;
        public long Length { get { return _length; } }
        private int _state;
        public int State
        {
            get
            {
                return _state;
            }
        }
        private DateTime _endwritetime = DateTime.Now;
        public DateTime EndWriteTime
        {
            get
            {
                return _endwritetime;
            }
        }
        public double Progress;
        #endregion

        public static DownloadState CreateTask<T>(string fullname) where T : DownloadController, new()
        {
            if (timer == null)
            {
                timer = new Timer(taskTimeOut * 2000);
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.AutoReset = true;
                timer.Enabled = true;
            }
            var task = new T();
            task.Initialize(fullname);
            tasklist.Add(task);
            return new DownloadState { TaskID = task.ID, State = task.State,Length=task.Length };
        }

        protected static void ClearTimeOutTask()
        {
            lock (tasklist)
            {
                tasklist.FindAll(t => t.EndWriteTime.AddSeconds(taskTimeOut) <= DateTime.Now).ForEach(
                t => t.Dispose());
            }
        }

        protected void Initialize(string fullname)
        {
            _id = this.GetHashCode();
            _state = 1;
            if (!File.Exists(fullname))
            {
                _state = 2;
                return;
            }
            fileStream = new FileStream(fullname, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 512);
            _length = fileStream.Length;
            fileStream.Position = 0;
        }

        public static DownloadState Read(int taskID, long position)
        {
            var task = tasklist.Find(t => t.ID == taskID);
            if (task == null) return new DownloadState();
            var bdata = task.Read(position);
            return new DownloadState { TaskID = task.ID, State = task.State, Data = bdata };
        }

        protected byte[] Read(long position)
        {
            lock (this)
            {
                if (position >= fileStream.Length)
                {
                    _state = 3;
                    return null;
                }
                fileStream.Position = position;
                int count = fileStream.Read(byteData, 0, byteData.Length);
                _endwritetime = DateTime.Now;
                _state = 1;
                if (count < byteData.Length)
                {
                    byte[] b = new byte[count];
                    Buffer.BlockCopy(byteData, 0, b, 0, count);
                    return b;
                }
                else
                    return byteData;
            }
        }

        protected void Dispose(bool delfile = false)
        {
            lock (this)
            {
                if (fileStream == null) return;
                string filename = fileStream.Name;
                fileStream.Flush();
                fileStream.Close();
                fileStream.Dispose();
                fileStream = null;
                if (delfile && File.Exists(filename))
                    File.Delete(filename);
                tasklist.Remove(this);
            }
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClearTimeOutTask();
        }

    }
}