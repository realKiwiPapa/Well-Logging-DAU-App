using System;
using System.IO;
using System.Web;
using System.Web.Services.Protocols;
using System.Linq;

using log4net;
using log4net.Core;
using Logging_App.Utility;

namespace Logging_App.WebService
{
    public class SoapWatchExtension : SoapExtension
    {
        private Stream _originalStream;
        private Stream _workStream;
        private readonly static string[] exceptionMethod = "Login,GetVersion,GetUpdateData,CreateDownloadTask,DownloadRead,GetPluginsInfo".Split(',');
        public override Stream ChainStream(Stream stream)
        {
            _originalStream = stream;
            _workStream = new MemoryStream();
            return _workStream;
        }

        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {

        }

        private static ILog logger;
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.AfterSerialize:
                    if (message.Exception != null)
                    {
                        var userInfo = ServiceUtils.GetUserInfo(false);
                        if (userInfo != null)
                        {
                            ThreadContext.Properties["LoginName"] = userInfo.COL_LOGINNAME;
                        }
                        Exception ex;
                        if (message.Exception.InnerException == null)
                            ex = message.Exception;
                        else
                            ex = message.Exception.InnerException;
                        ThreadContext.Properties["UserIP"] = ServiceUtils.GetIP();
                        if (logger == null) logger = LogManager.GetLogger("ErrorLog");
                        logger.Error(null, ex);
                        ServiceUtils.ThrowSoapException(ex.Message);
                    }
                    CompressStream();
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    DecompressStream();
                    break;
                case SoapMessageStage.AfterDeserialize:
                    if (ServiceUtils.GetUserInfo(false) == null)
                    {
                        if (!exceptionMethod.Contains(message.MethodInfo.Name))
                            ServiceUtils.ThrowSoapException("没有登录，或登录已经失效，请重新登录！");
                    }
                    else OnlineUser.Call();
                    break;
            }
        }

        public void CompressStream()
        {
            //压缩 响应
            Stream stream = CompressHelper.GZipCompress(_workStream);
            byte[] buffer = CompressHelper.ReadStreamToBytes(stream);
            _originalStream.Write(buffer, 0, buffer.Length);
        }
        public void DecompressStream()
        {
            //解压 请求
            byte[] bytes = CompressHelper.ReadStreamToBytes(_originalStream);
            bytes = CompressHelper.GZipDecompress(bytes);
            _workStream.Write(bytes, 0, bytes.Length);
            _workStream.Position = 0;
        }
    }
}