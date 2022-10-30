using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Logging_App.Utility
{
    public static class FileHelper
    {
        public static string GetServerFileName(decimal? fileid, Action<string> filename = null)
        {
            string name = ">未上传文件";
            string tmpName = name;
            if (fileid != null)
            {
                using (var fileser = new FileServiceHelper())
                {
                    tmpName = fileser.GetFileName((decimal)fileid);
                    if (!string.IsNullOrEmpty(tmpName))
                        name = tmpName;
                }
            }
            filename(name);
            return tmpName;
        }

        public static string SelectFile(Action<string> fullname = null)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.CheckFileExists = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (fullname != null) fullname(fileDialog.FileName);
                return fileDialog.FileName;
            }
            return null;
        }

        public class DirInfo
        {
            /// <summary>
            /// 文件总大小
            /// </summary>
            public long FileLength=0;
            /// <summary>
            /// 文件总个数
            /// </summary>
            public long FileNumber=0;
        }
        /// <summary>
        /// 获取目录的文件大小，文件个数信息
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="includeRootPathFiles">是否包含此目录下的文件</param>
        /// <param name="folderFilters">文件夹过滤器，按照目录层次依次排列string=DirectoryInfo.Name.ToLower()</param>
        public static DirInfo GetDirInfo(string dirPath, bool includeRootPathFile = false, params Predicate<string>[] folderFilters)
        {
            DirInfo dInfo = new DirInfo();
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            Predicate<string> filter = null;
            if (includeRootPathFile)
            {
                foreach (var f in dirInfo.GetFiles())
                {
                    if (f.Length > 0 && f.Name != "Thumbs.db")
                    {
                        dInfo.FileLength += f.Length;
                        dInfo.FileNumber += 1;
                    }
                }
            }

            if (folderFilters != null && folderFilters.Length > 0)
            {
                filter = folderFilters[0];
                folderFilters = (
                    from ff in folderFilters
                    where !ff.Equals(filter)
                    select ff
                    ).ToArray();
            }

            foreach (var d in dirInfo.GetDirectories())
            {
                string name = d.Name.ToLower();
                if (name.EndsWith(".temp"))
                    continue;
                if (filter == null || filter(name))
                {
                    var tmp=GetDirInfo(d.FullName, true, folderFilters);
                    dInfo.FileLength += tmp.FileLength;
                    dInfo.FileNumber += tmp.FileNumber;
                }
            }
            return dInfo;
        }

        /// <summary>
        /// 搜索文件，不包括当前目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPatterns"></param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(string path, params string[] searchPatterns)
        {
            return GetFiles(path, SearchOption.TopDirectoryOnly, searchPatterns);
        }

        public static FileInfo[] GetFiles(string path, SearchOption searchOption = SearchOption.TopDirectoryOnly, params string[] searchPatterns)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (searchPatterns == null || searchPatterns.Length == 0)
                    return dirInfo.GetFiles();
                else
                {
                    List<FileInfo> fInfoList = new List<FileInfo>();
                    foreach (string searchPattern in searchPatterns)
                    {
                        fInfoList.AddRange(dirInfo.GetFiles(searchPattern, searchOption));
                    }
                    return fInfoList.ToArray();
                }

            }
            return null;
        }

        public static void FileDelete(string path, params string[] searchPatterns)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                foreach (string searchPattern in searchPatterns)
                {
                    foreach (var fInfo in dirInfo.GetFiles(searchPattern, SearchOption.AllDirectories))
                    {
                        fInfo.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// 获取目录中的文件
        /// </summary>
        /// <param name="RootPath">目录路径</param>
        /// <param name="includeRootPathFiles">是否包含此目录下的文件</param>
        /// <param name="folderFilters">文件夹过滤器，按照目录层次依次排列string=DirectoryInfo.Name.ToLower()</param>
        public static IEnumerable<FileInfo> GetDirFiles(string rootPath, bool includeRootPathFile = false, params Predicate<string>[] folderFilters)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(rootPath);
            Predicate<string> filter = null;
            if (includeRootPathFile)
                foreach (FileInfo fi in dirInfo.GetFiles())
                {
                    if (fi.Length > 0 && fi.Name != "Thumbs.db")
                        yield return fi;
                }

            if (folderFilters != null && folderFilters.Length > 0)
            {
                filter = folderFilters[0];
                folderFilters = (
                    from ff in folderFilters
                    where !ff.Equals(filter)
                    select ff
                    ).ToArray();
            }

            foreach (var d in dirInfo.GetDirectories())
            {
                string name = d.Name.ToLower();
                if (name.EndsWith(".temp"))
                    continue;
                if (filter == null || filter(name))
                {
                    foreach (FileInfo fi in GetDirFiles(d.FullName, true, folderFilters))
                        yield return fi;
                }
            }
        }

        [DllImport("AutoReadFid.dll", EntryPoint = "ReadFidExe", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReadFidExe(string m_FileName, string m_DirName);
        public static string ExtractFidPack(string exeFileName, bool deleteOldFiles = true)
        {
            if (File.Exists(exeFileName))
            {
                bool tempFileExists = Directory.Exists(exeFileName + ".temp");
                if (deleteOldFiles || !tempFileExists)
                {
                    if (tempFileExists)
                    {
                        Directory.Delete(exeFileName + ".temp", true);
                    }
                    string path = Directory.CreateDirectory(exeFileName + ".temp").FullName;
                    ReadFidExe(exeFileName, path);
                    return path + "\\" + Path.GetFileNameWithoutExtension(exeFileName);
                }
                else
                    return exeFileName + ".temp\\" + Path.GetFileNameWithoutExtension(exeFileName);
            }
            return null;
        }
    }
}
