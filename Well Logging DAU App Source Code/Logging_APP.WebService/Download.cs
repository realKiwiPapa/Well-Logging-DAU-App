using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using Logging_App.Utility;
using Maticsoft.DBUtility;

namespace Logging_App.WebService
{
    public class UpdateDownload : DownloadController
    {
        public static DownloadState Create(string filename)
        {
            return CreateTask<UpdateDownload>(AutoUpdate.VersionPath + filename);
        }
    }

    public class FileDownload : DownloadController
    {

        public static DownloadState Create(decimal fileid)
        {
            string sha1 = null, md5 = null, path = null;
            decimal length = 0;
            var dt = DbHelperOra.Query("select a.sha1,a.md5,a.length,a.pathid,a.pathmain from sys_upload a,sys_file_upload b where a.uploadid=b.uploadid and b.fileid=" + fileid).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                sha1 = dt.Rows[0].FieldEx<string>("SHA1");
                md5 = dt.Rows[0].FieldEx<string>("MD5");
                length = dt.Rows[0].FieldEx<decimal>("LENGTH");
                path = FileUpload.PathList.FirstOrDefault(o => o.id == dt.Rows[0].FieldEx<decimal>("PATHID")).name;

                path = path.Remove(path.LastIndexOf('\\') + 1);
                path += dt.Rows[0].FieldEx<string>("PATHMAIN");
            }
            return CreateTask<FileDownload>(string.Format("{0}\\{1}-{2}-{3}", path, sha1, md5, length));
        }
    }

    public class PluginDownload : DownloadController
    {
        public static DownloadState Create(string filename)
        {
            return CreateTask<UpdateDownload>(PluginManager.RootPath + filename);
        }
    }
}