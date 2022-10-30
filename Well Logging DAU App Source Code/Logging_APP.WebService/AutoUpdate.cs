using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Logging_App.WebService
{
    public class AutoUpdate
    {
        static AutoUpdate()
        {
            Initialize();
        }
        private static string _version;
        private static string _versionpath;
        public static string Version { get { return _version; } }
        public static string VersionPath { get { return _versionpath; } }
        public static readonly string RootPath = AppDomain.CurrentDomain.BaseDirectory + "update\\";
        public static List<UpdateData> UpdateDataList = new List<UpdateData>();

        public static void Initialize()
        {
            var dir = new DirectoryInfo(RootPath);
            decimal ver = 0, vertmp;
            DirectoryInfo di = null;
            DirectoryInfo[] sourcedir;
            foreach (var d in dir.GetDirectories())
            {
                decimal.TryParse(d.Name, out vertmp);
                sourcedir =d.GetDirectories("SourceDir", SearchOption.TopDirectoryOnly);
                if (ver < vertmp && sourcedir.Length==1 )
                {
                    ver = vertmp;
                    di = sourcedir[0];
                }
            }
            if (di == null || di.GetFiles().Length < 2)
            {
                _version = string.Empty;
                return;
            }
            _version = ver.ToString();
            _versionpath = di.FullName.ToLower();
            UpdateDataList.Clear();
            foreach (var fi in di.GetFiles())
            {
                if (fi.Extension.ToLower() == ".temp") continue;
                var fileStream = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 512);
                var sha1 = Utility.HashHelper.ComputeSHA1(fileStream, 256 * 1024);
                fileStream.Position = 0;
                var md5 = Utility.HashHelper.ComputeMD5(fileStream);
                UpdateDataList.Add(
                    new UpdateData
                    {
                        FilePath = fi.FullName.ToLower().Substring(_versionpath.Length),
                        SHA1 = sha1,
                        MD5 = md5,
                        Length = fi.Length
                    });
            }
        }

        public struct UpdateData
        {
            public string FilePath;
            public string SHA1;
            public string MD5;
            public long Length;
        }
    }
}