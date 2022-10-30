using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Oracle.DataAccess.Client;

using Maticsoft.DBUtility;

namespace Logging_App.WebService
{
    public class FileUpload : UploadController, IConfigurationSectionHandler
    {
        public struct uploadPath
        {
            public decimal id;
            public string name;
            public bool used;
        }

        public static List<uploadPath> PathList = new List<uploadPath>();
        static string dirpath;
        static uploadPath uploadpath;
        protected override string UploadPath
        {
            get { return dirpath; }
        }

        static FileUpload()
        {
            ConfigurationManager.GetSection("FileUpload");
            if (PathList.Count < 1) throw new ArgumentNullException("上传路径不能为空");
            if (PathList.Exists(o => o.used))
                uploadpath = PathList.Find(o => o.used);
            else
                uploadpath = PathList[0];
            dirpath = CreateDirPath(uploadpath.name);
        }


        public static UploadState Create(string sha1, string md5, long length)
        {
            //string uploadPath = FileUpload.PathList[0].name;
            //string pathmain = uploadPath.Substring(uploadPath.LastIndexOf('\\') + 1);
            //if (pathmain == DateTime.Now.ToString("yyyyMM"))
            //{
            //    FileUpload.Create(sha1, md5, length);
            //    //FileUpload x = new FileUpload();
            //}
            return CreateTask<FileUpload>(sha1, md5, length);
        }

        protected override void UploadCompleted(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
                File.Move(filename, filename.Substring(0, filename.Length - 5));
            string uploadPath = FileUpload.PathList[0].name;
            string pathmain = uploadPath.Substring(uploadPath.LastIndexOf('\\') + 1);
            //删除其它目录相同文件
            var o_pathmain = DbHelperOra.GetSingle("select pathmain from sys_upload where sha1=:SHA1 and md5=:MD5 and length=:LENGTH",
                                ServiceUtils.CreateOracleParameter(":SHA1", OracleDbType.Char, SHA1),
                                ServiceUtils.CreateOracleParameter(":MD5", OracleDbType.Char, MD5),
                                ServiceUtils.CreateOracleParameter(":LENGTH", OracleDbType.Decimal, Length)) as string;
            if (o_pathmain != null && !o_pathmain.Equals(pathmain))
            {
                var basepath = uploadPath.Replace(pathmain, o_pathmain);
                var filepath = string.Format("{0}\\{1}-{2}-{3}", basepath, SHA1, MD5, Length);
                if (File.Exists(filepath))
                    File.Delete(filepath);
            }

            DbHelperOra.ExecuteSql("DECLARE v_cnt NUMBER;BEGIN SELECT COUNT(1) INTO v_cnt FROM sys_upload WHERE sha1=:SHA1 AND md5=:MD5 AND length=:LENGTH;if v_cnt>0 then update sys_upload set pathid=:PATHID,pathmain=:PATHMAIN WHERE sha1=:SHA1 AND md5=:MD5 AND length=:LENGTH; else INSERT INTO sys_upload(UPLOADID,SHA1,MD5,LENGTH,PATHID,PATHMAIN) values (UPLOADID_SEQ.NEXTVAL,:SHA1,:MD5,:LENGTH,:PATHID,:PATHMAIN);end if;end;",
                ServiceUtils.CreateOracleParameter(":SHA1", OracleDbType.Char, SHA1),
                ServiceUtils.CreateOracleParameter(":MD5", OracleDbType.Char, MD5),
                ServiceUtils.CreateOracleParameter(":LENGTH", OracleDbType.Decimal, Length),
                ServiceUtils.CreateOracleParameter(":PATHID", OracleDbType.Decimal, uploadpath.id),
                ServiceUtils.CreateOracleParameter(":PATHMAIN", OracleDbType.NVarchar2, pathmain)
                );
        }

        protected override bool FileExists(string filename)
        {
            foreach (var path in PathList)
            {
                var dir = new DirectoryInfo(path.name);
                if (dir.Exists && dir.GetFiles(filename, SearchOption.TopDirectoryOnly).Length > 0)
                {
                    uploadpath = path;
                    return true;
                }
            }
            return false;
        }

        object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
        {
            decimal _id;
            string _name;
            bool _used;
            XmlNode xnode;
            foreach (XmlNode xn in section)
            {
                if (xn.Name.ToLower() == "path")
                {
                    _id = decimal.Parse(xn.SelectSingleNode("@id").InnerText);
                    _name = xn.SelectSingleNode("@name").InnerText.ToLower();
                    if (string.IsNullOrWhiteSpace(_name)) continue;
                    //if (_name.StartsWith("\\")) _name = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + _name;
                    if (PathList.Exists(o => o.id == _id)) continue;
                    xnode = xn.SelectSingleNode("@used");
                    _used = false;
                    if (xnode != null)
                        bool.TryParse(xnode.InnerText, out _used);
                    //PathList.Add(new uploadPath { id = _id, name = _name + "\\" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute, used = _used });
                    PathList.Add(new uploadPath { id = _id, name = _name + "\\" + DateTime.Now.ToString("yyyyMM"), used = _used });
                }
            }
            return null;
        }
    }

    public class PackUpload : UploadController
    {
        static object synclock = new object();
        static string path = CreateDirPath(AutoUpdate.RootPath);
        protected override string UploadPath
        {
            get
            {
                return path;
            }
        }

        public PackUpload()
        {
            if (ServiceUtils.GetUserInfo().COL_LOGINNAME.ToLower() != "admin")
                ServiceUtils.ThrowSoapException("没有权限");
        }

        public static UploadState Create(string sha1, string md5, long length)
        {
            lock (synclock)
            {
                return CreateTask<PackUpload>(sha1, md5, length);
            }
        }

        protected override void UploadCompleted(string filename)
        {
            string fullpath = path + DateTime.Now.Ticks;
            var process = new System.Diagnostics.Process();
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            //process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "bin\\lessmsi.exe";
            process.StartInfo.Arguments = "x \"" + filename + "\" \"" + fullpath + "\"\\";
            process.Start();
            process.WaitForExit();
            process.Close();
            var dirinfo = new DirectoryInfo(fullpath + "\\SourceDir");
            if (dirinfo.Exists && dirinfo.GetFiles().Length > 1)
                AutoUpdate.Initialize();
        }

        protected override bool FileExists(string filename)
        {
            var dir = new DirectoryInfo(path);
            if (dir.Exists && dir.GetFiles(filename, SearchOption.TopDirectoryOnly).Length > 0)
                return true;
            return false;
        }
    }
}