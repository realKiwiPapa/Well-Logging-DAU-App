using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging_App.WebService
{
    /// <summary>
    /// TestSpeed 的摘要说明
    /// </summary>
    public class TestSpeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var files=context.Request.Files;
            if (files.Count > 0)
            {
                //files[0].SaveAs(@"d:\test.txt");
            }
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
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