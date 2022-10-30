using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Logging_App.WebService
{
    public class DataCache
    {
        static readonly string path = AppDomain.CurrentDomain.BaseDirectory + "DataCache\\";

        [SerializableAttribute]
        public struct LineIndex
        {
            public int StartLine { get; set; }
            public int EndLine { get; set; }
            public long Position { get; set; }
        }

        public static List<LineIndex> GetFileLineIndex(string fullname)
        {
            string filename = path + Path.GetFileName(fullname) + ".LineIndex";
            if (!File.Exists(filename)) CreateFileLineIndex(fullname);
            return Utility.ModelHelper.DeserializeObject(File.ReadAllBytes(filename)) as List<LineIndex>;
        }

        public static void CreateFileLineIndex(string fullname)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            int bufferSize = 1024 * 1024;
            byte[] bytes = new byte[bufferSize];
            var LineIndexList = new List<LineIndex>();
            using (var fs = new FileStream(fullname, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
            {
                int count;
                int num = 0;
                byte b;
                bool isNullOrEmpty = true;
                long pos = 0;
                byte endbyte = 10;
                while ((count = fs.Read(bytes, 0, bytes.Length)) > 0)
                {
                    int i = 0;
                    int offset = 0;
                    var lineIndex = new LineIndex { StartLine = 0 };
                    isNullOrEmpty = true;
                    while (true)
                    {
                        if (i == count)
                        {
                            if (!isNullOrEmpty)
                            {
                                if (endbyte == 10 || endbyte == 13) num++;
                                endbyte = bytes[count - 1];
                            }
                            else
                            {
                                endbyte = 10;
                                if (lineIndex.StartLine == 0) break;
                            }
                            if (LineIndexList.Count == 1 && lineIndex.StartLine == 0) break;
                            if (lineIndex.StartLine == 0) lineIndex.StartLine = 1;
                            lineIndex.EndLine = num;
                            LineIndexList.Add(lineIndex);
                            break;
                        }
                        b = bytes[i];
                        switch (b)
                        {
                            case 10:
                            case 13:
                                if (b == 13 && i < count - 1 && bytes[i + 1] == 10)
                                {
                                    i++;
                                }
                                if (endbyte != 10 && endbyte != 13)
                                {
                                    offset = i;
                                    endbyte = 10;
                                    i++;
                                    continue;
                                }
                                if (!isNullOrEmpty)
                                {
                                    num++;
                                    isNullOrEmpty = true;
                                    if (lineIndex.StartLine == 0)
                                    {
                                        lineIndex.StartLine = num;
                                        lineIndex.Position = pos + offset;
                                    }
                                }
                                else
                                {
                                    offset = i;
                                }
                                break;
                            default:
                                if (b > 32 && b != 127)
                                    isNullOrEmpty = false;
                                break;
                        }
                        i++;
                    }
                    pos = fs.Position;
                }
                if (LineIndexList.Count > 0)
                    File.WriteAllBytes(path + Path.GetFileName(fullname) + ".LineIndex", Utility.ModelHelper.SerializeObject(LineIndexList));
            }
        }

    }
}