using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;

namespace Logging_App.Utility
{
    public class FtpHelper
    {
        private string ftpUserID;
        private string ftpPassword;
        private string ftpURI;

        /// <summary>
        /// 连接FTP
        /// </summary>
        /// <param name="FtpServerIP">FTP连接地址</param>
        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>
        /// <param name="FtpUserID">用户名</param>
        /// <param name="FtpPassword">密码</param>
        public FtpHelper()
        {
            ftpURI = ConfigurationManager.AppSettings["FtpURI"];
            ftpUserID = ConfigurationManager.AppSettings["FtpUserID"];
            ftpPassword = ConfigurationManager.AppSettings["FtpPassword"];
        }

        public FtpHelper(string target)
        {
            string uri = null;
            string uid = null;
            string pwd = null;
            if (target.Equals("A1"))
            {
                uri = "A1_URI";
                uid = "A1_UID";
                pwd = "A1_PWD";
            }
            if (target.Equals("XY"))
            {
                uri = "FtpURI";
                uid = "FtpUserID";
                pwd = "FtpPassword";
            }
            ftpURI = ConfigurationManager.AppSettings[uri];
            ftpUserID = ConfigurationManager.AppSettings[uid];
            ftpPassword = ConfigurationManager.AppSettings[pwd];
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="remoteFileName">上传的文件名</param>
        public void Upload(string dir, string remoteFileName, string filePath)
        {
            var fileInf = new FileInfo(filePath);
            var uri = ftpURI;
            if(!string.IsNullOrEmpty(dir))
                uri+=dir+"/";
            uri += remoteFileName;
            var reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.KeepAlive = false;
            reqFTP.UseBinary = true;
            reqFTP.UsePassive = false;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            var buff = new byte[buffLength];
            int contentLen;
            var fs = fileInf.OpenRead();
            try
            {
                var strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void Download(string filePath, string fileName)
        {
            try
            {
                var outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
                var reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                var response = (FtpWebResponse)reqFTP.GetResponse();
                var ftpStream = response.GetResponseStream();
                var cl = response.ContentLength;
                var bufferSize = 2048;
                int readCount;
                var buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList()
        {
            try
            {
                var result = new StringBuilder();
                var ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI));
                ftp.UseBinary = true;
                ftp.UsePassive = false;
                ftp.KeepAlive = false;
                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                var response = ftp.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                var line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前目录下所有的文件夹列表(仅文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetDirectoryList()
        {
            var drectory = GetFilesDetailList();
            if (drectory == null)
            {
                return null;
            }
            var m = string.Empty;
            foreach (var str in drectory)
            {
                var dirPos = str.IndexOf("<DIR>");
                if (dirPos > 0)
                {
                    /*判断 Windows 风格*/
                    m += str.Substring(dirPos + 5).Trim() + "\n";
                }
                else if (str.Trim().Substring(0, 1).ToUpper() == "D")
                {
                    /*判断 Unix 风格*/
                    var dir = str.Substring(54).Trim();
                    if (dir != "." && dir != "..")
                    {
                        m += dir + "\n";
                    }
                }
            }
            var n = new char[] { '\n' };
            return m.Split(n);
        }

        /// <summary>
        /// 获取当前目录下文件列表(仅文件)
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList(string mask, string dir)
        {
            try
            {
                var result = new StringBuilder();
                var uri=ftpURI;
                if (!string.IsNullOrEmpty(dir))
                    uri += dir;
                var reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                var response = reqFTP.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                var line = reader.ReadLine();
                while (line != null)
                {
                    if (mask.Trim() != string.Empty && mask.Trim() != "*.*")
                    {
                        var mask_ = mask.Substring(0, mask.IndexOf("*"));
                        if (line.Substring(0, mask_.Length) == mask_)
                        {
                            result.Append(line);
                            result.Append("\n");
                        }
                    }
                    else
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 判断当前目录下指定的子目录是否存在
        /// </summary>
        /// <param name="RemoteDirectoryName">指定的目录名</param>
        public bool DirectoryExist(string RemoteDirectoryName)
        {
            var dirList = GetDirectoryList();
            if (dirList != null)
            {
                foreach (var str in dirList)
                {
                    if (str.Trim() == RemoteDirectoryName.Trim())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断当前目录下指定的文件是否存在
        /// </summary>
        /// <param name="RemoteFileName">远程文件名</param>
        /// <param name="dir">远程文件目录</param>
        public bool FileExist(string dir, string RemoteFileName)
        {
            var fileList = GetFileList("*.*", dir);
            if (fileList != null)
            {
                foreach (var str in fileList)
                {
                    if (str.Trim() == RemoteFileName.Trim())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dir"></param>
        public void MakeDir(string dir)
        {
            try
            {
                var reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + dir));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.KeepAlive = false;
                var response = (FtpWebResponse)reqFTP.GetResponse();
                var ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteFile(string dir, string fileName)
        {
            try
            {
                var uri = ftpURI + dir + "/" + fileName;
                var reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.KeepAlive = false;
                var response = (FtpWebResponse)reqFTP.GetResponse();
                var ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
