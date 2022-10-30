using System;
using System.IO;
using System.Web;
using System.Web.Services.Protocols;

using Logging_App.Utility;

namespace Logging_App
{
    public class SoapWatchExtension : SoapExtension
    {
        private Stream _originalStream;
        private Stream _workStream;
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

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.AfterSerialize:
                    CompressStream();
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    DecompressStream();
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