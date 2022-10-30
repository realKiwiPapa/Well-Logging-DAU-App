using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
namespace Logging_App
{

    public class WebServiceConfig
    {
        static WebServiceConfig()
        {
            WebRequest.DefaultWebProxy = null;
        }

        public static CookieContainer CookieContainer = new CookieContainer();
        public static int Timeout = 3600000*48;//360000*50
        //public static WebProxy Proxy = new WebProxy();
#if TestClient||REMOTE
        public static string BaseUrl = ConfigurationManager.AppSettings["remoteService"];
#else
        public static string BaseUrl = "http://localhost:9366/";
#endif

    }

    public class DataServiceHelper : DataService.DataService
    {
        public DataServiceHelper()
        {
#if (!DEBUG||REMOTE)
            this.Url = WebServiceConfig.BaseUrl + "DataService.asmx";
#endif
            this.CookieContainer = WebServiceConfig.CookieContainer;
            this.Timeout = WebServiceConfig.Timeout;
            //this.Proxy = WebServiceConfig.Proxy;
        }

    }

    public class FileServiceHelper : FileService.FileService
    {
        public FileServiceHelper()
        {
#if (!DEBUG||REMOTE)
            this.Url = WebServiceConfig.BaseUrl + "FileService.asmx";
#endif
            this.CookieContainer = WebServiceConfig.CookieContainer;
            this.Timeout = WebServiceConfig.Timeout;
            //this.Proxy = WebServiceConfig.Proxy;
        }

    }

    public class UserServiceHelper : UserService.UserService
    {
        public UserServiceHelper()
        {
#if (!DEBUG||REMOTE)
            this.Url = WebServiceConfig.BaseUrl + "UserService.asmx";
#endif
            this.CookieContainer = WebServiceConfig.CookieContainer;
            this.Timeout = WebServiceConfig.Timeout;
            //this.Proxy = WebServiceConfig.Proxy;
        }
    }

    public class WorkflowServiceHelper : WorkflowService.WorkflowService
    {
        public WorkflowServiceHelper()
        {
#if (!DEBUG||REMOTE)
            this.Url = WebServiceConfig.BaseUrl + "Workflow/WorkflowService.asmx";
#endif
            this.CookieContainer = WebServiceConfig.CookieContainer;
            this.Timeout = WebServiceConfig.Timeout;
            //this.Proxy = WebServiceConfig.Proxy;
        }

    }
}
