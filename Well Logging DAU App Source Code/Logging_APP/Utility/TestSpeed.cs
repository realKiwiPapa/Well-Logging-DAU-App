using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Logging_App.Utility
{
    public class TestSpeed
    {
        private static void Upload_Request(string address, out int returnValue)
        {
            returnValue = 0;
            try
            {
                //时间戳 
                string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");

                //请求头部信息 
                StringBuilder sb = new StringBuilder();
                sb.Append("--");
                sb.Append(strBoundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"");
                sb.Append("file");
                sb.Append("\"; filename=\"");
                sb.Append("saveName");
                sb.Append("\"");
                sb.Append("\r\n");
                sb.Append("Content-Type: ");
                sb.Append("application/octet-stream");
                sb.Append("\r\n");
                sb.Append("\r\n");
                string strPostHeader = sb.ToString();
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);
                // 根据uri创建HttpWebRequest对象 
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
                httpReq.Method = "POST";
                httpReq.CookieContainer = WebServiceConfig.CookieContainer;
                var bs = new byte[512];
                int max = 512*4;
                //对发送的数据不使用缓存 
                httpReq.AllowWriteStreamBuffering = false;

                //设置获得响应的超时时间（300秒） 
                httpReq.Timeout = 6000000;
                httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
                long length = bs.Length * max + postHeaderBytes.Length + boundaryBytes.Length;
                httpReq.ContentLength = length;
                Stream postStream = httpReq.GetRequestStream();

                //发送请求头部消息 
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                while (returnValue < max)
                {
                    postStream.Write(bs, 0, 512);
                    returnValue++;
                }
                //添加尾部的时间戳 
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();
            }
            catch{}
        }

        public static int GetUploadSpeed()
        {
            int returnValue = 0;
            var stopwatch = new Stopwatch();
            try
            {
                ManualResetEvent wait = new ManualResetEvent(false);
                Thread work = new Thread(new ThreadStart(() =>
                {
                    stopwatch.Start();
                    Upload_Request(WebServiceConfig.BaseUrl + "TestSpeed.ashx", out returnValue);
                    wait.Set();
                }));
                work.Start();
                wait.WaitOne(5000);
                if (work.IsAlive) work.Abort();
            }
            catch { }
            stopwatch.Stop();
            return (int)(returnValue * 512D/stopwatch.ElapsedMilliseconds*1000D);
        }
    }
}
