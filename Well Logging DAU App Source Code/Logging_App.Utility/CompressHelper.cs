using System;
using System.IO;
using System.IO.Compression;

namespace Logging_App.Utility
{
    public class CompressHelper
    {
        public static byte[] ReadStreamToBytes(Stream stream)
        {
            byte[] buffer = new byte[128 * 1024];
            int r = 0;
            int l = 0;
            long position = -1;
            if (stream.CanSeek)
            {
                position = stream.Position;
                stream.Position = 0;
            }
            int i = 0;
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                r = stream.Read(buffer, 0, buffer.Length);
                if (r > 0)
                {
                    l += r;
                    ms.Write(buffer, 0, r);
                    i++;
                }
                else
                {
                    break;
                }
            }
            byte[] bytes = new byte[l];
            ms.Position = 0;
            ms.Read(bytes, 0, (int)l);
            ms.Close();
            ms = null;
            if (position >= 0)
            {
                stream.Position = position;
            }
            return bytes;
        }
        public static byte[] GZipCompress(byte[] DATA)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream stream = new GZipStream(ms, CompressionMode.Compress, true);
            stream.Write(DATA, 0, DATA.Length);
            stream.Close();
            stream.Dispose();
            stream = null;
            byte[] buffer = ReadStreamToBytes(ms);
            ms.Close();
            ms.Dispose();
            ms = null;
            return buffer;
        }
        public static byte[] GZipDecompress(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            GZipStream stream = new GZipStream(ms, CompressionMode.Decompress);
            byte[] buffer = ReadStreamToBytes(stream);
            ms.Close();
            ms.Dispose();
            ms = null;
            stream.Close();
            stream.Dispose();
            stream = null;
            return buffer;
        }
        public static Stream GZipCompress(Stream DATA)
        {
            byte[] buffer = ReadStreamToBytes(DATA);
            MemoryStream ms = new MemoryStream();
            GZipStream stream = new GZipStream(ms, CompressionMode.Compress, true);
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();
            stream.Dispose();
            stream = null;
            if (ms.CanSeek)
            {
                ms.Position = 0;
            }
            return ms;
        }
        public static Stream GZipDecompress(Stream data)
        {
            byte[] buffer = ReadStreamToBytes(data);
            MemoryStream ms = new MemoryStream(buffer);
            GZipStream stream = new GZipStream(ms, CompressionMode.Decompress);
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
            return stream;
        }
        public static byte[] DeflateCompress(byte[] DATA)
        {
            MemoryStream ms = new MemoryStream();
            DeflateStream stream = new DeflateStream(ms, CompressionMode.Compress, true);
            stream.Write(DATA, 0, DATA.Length);
            stream.Close();
            stream.Dispose();
            stream = null;
            byte[] buffer = ReadStreamToBytes(ms);
            ms.Close();
            ms.Dispose();
            ms = null;
            return buffer;
        }
        public static byte[] DeflateDecompress(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            DeflateStream stream = new DeflateStream(ms, CompressionMode.Decompress);
            byte[] buffer = ReadStreamToBytes(stream);
            ms.Close();
            ms.Dispose();
            ms = null;
            stream.Close();
            stream.Dispose();
            stream = null;
            return buffer;
        }
        public static Stream DeflateCompress(Stream DATA)
        {
            byte[] buffer = ReadStreamToBytes(DATA);
            MemoryStream ms = new MemoryStream();
            DeflateStream stream = new DeflateStream(ms, CompressionMode.Compress, true);
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();
            stream.Dispose();
            stream = null;
            if (ms.CanSeek)
            {
                ms.Position = 0;
            }
            return ms;
        }
        public static Stream DeflateDecompress(Stream data)
        {
            byte[] buffer = ReadStreamToBytes(data);
            MemoryStream ms = new MemoryStream(buffer);
            DeflateStream stream = new DeflateStream(ms, CompressionMode.Decompress);
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
            return stream;
        }
    }

}
