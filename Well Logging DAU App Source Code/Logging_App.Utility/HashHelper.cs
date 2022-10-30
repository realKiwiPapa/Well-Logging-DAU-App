using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

using OpenSSL.Crypto;

namespace Logging_App.Utility
{ 
    public class HashHelper
    {
        private string _hash;
        public string Hash { get { return _hash; } }
        private Stream inputStream;

        public HashHelper(Stream inputStream)
        {
            this.inputStream = inputStream;
        }

        private delegate void AsyncCompute(MessageDigest md);

        public static string ComputeMD5(Stream inputStream)
        {
            return ComputeHash(inputStream, MessageDigest.MD5);
        }
        public static string ComputeMD5(Stream inputStream, long count)
        {
            return ComputeHash(inputStream, MessageDigest.MD5, count);
        }

        public IAsyncResult AsyncComputeMD5()
        {
            return AsyncComputeHash(MessageDigest.MD5);
        }

        public IAsyncResult AsyncComputeHash(MessageDigest md)
        {
            var async = new AsyncCompute(asyncComputeHash);
            return async.BeginInvoke( md, null, null);
        }

        public static string ComputeSHA1(Stream inputStream)
        {
            return ComputeHash(inputStream, MessageDigest.SHA1);
        }

        public static string ComputeSHA1(Stream inputStream, long count)
        {
            return ComputeHash(inputStream, MessageDigest.SHA1, count);
        }

        protected void asyncComputeHash(MessageDigest md)
        {
           _hash= ComputeHash(inputStream, md);
        }

        public static string ComputeHash<T>(byte[] buffer) where T : HashAlgorithm
        {
          return BitConverter.ToString(HashAlgorithm.Create(typeof(T).FullName).ComputeHash(buffer)).Replace("-","");
        }

        public static string ComputeHash(Stream inputStream, MessageDigest md)
        {
            using (MessageDigestContext ctx = new MessageDigestContext(md))
            {
                int num;
                byte[] buffer = new byte[0x10000];
                ctx.Init();
                do
                {
                    num = inputStream.Read(buffer, 0, buffer.Length);
                    if (num > 0)
                        ctx.Update(buffer, (uint)num);
                }
                while (num > 0);
                return BitConverter.ToString(ctx.DigestFinal()).Replace("-", "");
            }
        }

        public static string ComputeHash(Stream inputStream, MessageDigest md, long count)
        {
            using (MessageDigestContext ctx = new MessageDigestContext(md))
            {
                int num;
                long totalHashCount = 0;
                byte[] buffer = new byte[0x1000];
                ctx.Init();
                do
                {
                    int readCount = buffer.Length;
                    if (count - totalHashCount < buffer.Length)
                    {
                        readCount = (int)(count - totalHashCount);
                    }
                    num = inputStream.Read(buffer, 0, readCount);
                    if (num > 0)
                    {
                        ctx.Update(buffer, (uint)num);
                        totalHashCount += num;
                    }
                }
                while (num > 0);
                return BitConverter.ToString(ctx.DigestFinal()).Replace("-", "");
            }
        }
    }
}
