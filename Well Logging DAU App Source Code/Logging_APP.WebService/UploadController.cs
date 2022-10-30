using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;

using Logging_App.Utility;

namespace Logging_App.WebService
{
    public abstract class UploadController
    {
        public struct UploadState
        {
            public int TaskID;
            /// <summary>
            /// 0 任务不存在
            /// 1 上传中
            /// 2 其他用户上传中
            /// 3 上传完成
            /// 4 校验错误
            /// 5 校验中
            /// </summary>
            public int State;
            public long Position;
            public double Progress;
        }

        private const int taskTimeOut = 180;
        private const int userTimeOut = 10;
        private const int sha1Size = 256 * 1024;
        private static Timer timer = null;
        private static List<UploadController> tasklist = new List<UploadController>();
        private static readonly Regex md5Regex = new Regex(@"^[A-Z\d]{32}$", RegexOptions.Compiled);
        private static readonly Regex sha1Regex = new Regex(@"^[A-Z\d]{40}$", RegexOptions.Compiled);
        private FileStream fileStream = null;
        private HashHelper hashHelper = null;
        protected abstract string UploadPath { get; }
        protected abstract bool FileExists(string filename);
        protected abstract void UploadCompleted(string filename);
        private IAsyncResult asyncComputeMD5 = null;
        #region
        private int _id;
        public int ID { get { return _id; } }
        private string _sha1;
        public string SHA1 { get { return _sha1; } }
        private string _md5;
        public string MD5 { get { return _md5; } }
        private long _length;
        public long Length { get { return _length; } }
        private int _state;
        public int State
        {
            get
            {
                if (_state == 5 && asyncComputeMD5 != null && asyncComputeMD5.IsCompleted)
                    ValidationFile();
                return _state;
            }
        }
        public long Position
        {
            get
            {

                if (fileStream == null) return _length;
                return fileStream.Position;
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

        public static UploadState CreateTask<T>(string sha1, string md5, long length) where T : UploadController, new()
        {
            lock (tasklist)
            {
                if (timer == null)
                {
                    timer = new Timer(taskTimeOut * 2000);
                    timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                    timer.AutoReset = true;
                    timer.Enabled = true;
                }
                var task = tasklist.Find(t => sha1 == t.SHA1 && md5 == t.MD5 && length == t.Length && t.GetType() == typeof(T));
                if (task == null)
                {
                    task = new T();
                    task.Initialize(sha1, md5, length);
                    if (task.State != 3) tasklist.Add(task);
                    return new UploadState { TaskID = task.ID, State = task.State, Position = task.Position };
                }
                return setUploadSate(task);
            }
        }

        public static UploadState GetUploadSate(int taskID)
        {
            return setUploadSate(tasklist.Find(t => t.ID == taskID));
        }

        public static UploadState Write(int taskID, long position, byte[] bData)
        {
            var task = tasklist.Find(t => t.ID == taskID);
            if (task == null) return new UploadState();
            int state = task.Write(position, bData);
            return new UploadState { TaskID = task.ID, State = state, Position = task.Position };
        }

        protected static string CreateDirPath(string path)
        {
            if (!path.EndsWith("\\"))
                path += "\\";
            if (!Directory.Exists(path))
            {
                try
                {
                    
                    Directory.CreateDirectory(path);
                }
                catch(Exception ex)
                {
                    ServiceUtils.ThrowSoapException("创建上传目录失败！\r\n" + path + "\r\n" + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "\r\n" + ex.ToString());
                }
            }
            return path;
        }

        protected static void ClearTimeOutTask()
        {
            lock (tasklist)
            {
                tasklist.FindAll(t => t.EndWriteTime.AddSeconds(taskTimeOut) <= DateTime.Now && t.State != 5).ForEach(
                t => { t.Dispose(); });
            }
        }

        protected void Initialize(string sha1, string md5, long length)
        {
            _id = this.GetHashCode();
            _sha1 = sha1;
            _md5 = md5;
            _length = length;
            ValidationAttribute();
            string filename = string.Format("{0}-{1}-{2}", SHA1, MD5, Length);
            if (FileExists(filename))
            {
                _state = 3;
                UploadCompleted("");
            }
            else
            {
                _state = 1;
                fileStream = new FileStream(UploadPath + filename + ".temp", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 1024 * 512);
                fileStream.Position = fileStream.Length;
            }
        }

        protected int Write(long position, byte[] bDate)
        {
            lock (this)
            {
                if (State == 3 || State == 5) return State;
                if (fileStream == null) return 0;
                if (position != fileStream.Position) return 2;
                fileStream.Write(bDate, 0, bDate.Length);
                _endwritetime = DateTime.Now;
                if (fileStream.Position == Length)
                {
                    _state = 5;
                    fileStream.Position = 0;
                    string sha1 = HashHelper.ComputeSHA1(fileStream, sha1Size);
                    if (sha1 != SHA1)
                    {
                        Dispose(true);
                        return 4;
                    }
                    fileStream.Position = 0;
                    hashHelper = new HashHelper(fileStream);
                    asyncComputeMD5 = hashHelper.AsyncComputeMD5();
                    return 5;
                }
                _state = 1;
                return 1;
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

        private static UploadState setUploadSate(UploadController task)
        {
            if (task == null) return new UploadState();
            int state = task.State;
            if (state == 1 && task.EndWriteTime.AddSeconds(userTimeOut) >= DateTime.Now)
                state = 2;
            return new UploadState { TaskID = task.ID, State = state, Position = task.Position, Progress = task.Progress };
        }

        private void ValidationFile()
        {
            if (hashHelper.Hash == MD5)
            {
                string filename = fileStream.Name;
                Dispose();
                UploadCompleted(filename);
                _state = 3;
            }
            else
            {
                Dispose(true);
                _state = 4;
            }
        }

        private void ValidationAttribute()
        {
            if (Length < 0)
                throw new ArgumentException("Length无效!");
            if (!md5Regex.IsMatch(MD5))
                throw new ArgumentException("MD5无效!");
            if (!sha1Regex.IsMatch(SHA1))
                throw new ArgumentException("SHA1无效!");
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClearTimeOutTask();
        }

    }
}