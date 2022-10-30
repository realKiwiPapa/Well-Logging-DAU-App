using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Text;
using System.Net;
using System.Configuration;
using Maticsoft.DBUtility;

namespace Logging_App.WebService
{
    /// <summary>
    /// FileDownload1 的摘要说明
    /// </summary>
    public class FileDownload1 : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        static string basicPath = ConfigurationManager.AppSettings["basicPath"];
        static string userId = ConfigurationManager.AppSettings["userId"];
        static string userPwd = ConfigurationManager.AppSettings["userPwd"];
        private FtpWebRequest ftpCreate(string requestUriString)
        {
            var request = (FtpWebRequest)FtpWebRequest.Create(requestUriString);
            request.Proxy = null;
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(userId, userPwd);//测试服务器196
            //request.Credentials = new NetworkCredential("LoggingAppUser", "Te123456st");//测试服务器193
            request.UseBinary = true;
            request.KeepAlive = false;
            //request.UsePassive = false;
            return request;
        }

        public void ProcessRequest(HttpContext context)
        {
            if (!new UserService().GetActiveUserRoles().Contains(ServiceEnums.UserRole.批量下载员))
                return;
            context.Response.ContentType = "application/octet-stream";
            decimal id = decimal.Parse(context.Request.Form["id"]);
            var dr = DbHelperOra.Query("SELECT B.FILENAME,C.SHA1,C.MD5,C.LENGTH,C.PATHID,C.PATHMAIN FROM SYS_PROCESSING_UPLOADFILE A,SYS_FILE_UPLOAD B ,SYS_UPLOAD C WHERE A.FILEID =B.FILEID AND B.UPLOADID =C.UPLOADID AND A.PROCESS_UPLOAD_ID=" + id).Tables[0].Rows[0];
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(dr.Field<string>("FILENAME")));
            context.Response.AddHeader("Content-Length", dr.Field<decimal>("LENGTH").ToString());

            //var path = FileUpload.PathList.FirstOrDefault(x => x.id == dr.Field<decimal>("PATHID")).name;
            //path = path.Remove(path.LastIndexOf('\\') + 1);
            //path += dr.Field<string>("PATHMAIN");

            //string fullname = string.Format("{0}\\{1}-{2}-{3}",
            //                                   path,
            //                                   dr.Field<string>("SHA1"),
            //                                   dr.Field<string>("MD5"),
            //                                   dr.Field<decimal>("LENGTH"));
            var path = basicPath + dr.Field<string>("PATHMAIN")+"/";

            int chunkSize = 10240;
            byte[] buffer = new byte[chunkSize];
            int read = 0;
            var request = ftpCreate(string.Format("{0}{1}-{2}-{3}", path, dr.Field<string>("SHA1"), dr.Field<string>("MD5"), dr.Field<decimal>("LENGTH")));
            var response = ((FtpWebResponse)request.GetResponse());
            using (var stream = response.GetResponseStream())
            {
                //context.Response.AddHeader("Content-Length", fileList[0].LENGTH.ToString());
                do
                {
                    read = stream.Read(buffer, 0, chunkSize);
                    context.Response.OutputStream.Write(buffer, 0, read);
                    context.Response.Flush();
                }
                while (read > 0 && context.Response.IsClientConnected);
            }
            response.Close();
            //using (var reader = new FileStream(fullname,FileMode.Open,FileAccess.Read))
            //{
            //    do
            //    {
            //        read = reader.Read(buffer, 0, chunkSize);
            //        context.Response.OutputStream.Write(buffer, 0, read);
            //        context.Response.Flush();
            //    }
            //    while (read > 0 && context.Response.IsClientConnected);
            //}
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}