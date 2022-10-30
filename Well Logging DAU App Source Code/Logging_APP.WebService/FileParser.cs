using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Text;

namespace Logging_App.WebService
{
    public static class FileParser
    {
        private static readonly Regex regex = new Regex(@"[^\s]+", RegexOptions.Compiled);
        private static readonly Regex regex1 = new Regex(@"#\s+", RegexOptions.Compiled);
        public static DataTable TxtToDataTable(string in_Name, int maxLine = int.MaxValue)
        {
            string MyInf;
            MatchCollection matchColl;
            if (File.Exists(in_Name))
            {
                using (StreamReader MyRead = new StreamReader(in_Name, Encoding.Default))
                {
                    DataTable dt = new DataTable();
                    while (MyRead.Peek() >= 0)
                    {
                        MyInf = MyRead.ReadLine();
                        MyInf = regex1.Replace(MyInf, "#");
                        if (!string.IsNullOrWhiteSpace(MyInf))
                        {
                            matchColl = regex.Matches(MyInf);
                            for (int i = dt.Columns.Count + 1; i <= matchColl.Count; i++) dt.Columns.Add(new DataColumn(i.ToString()));
                            dt.Rows.Add(
                                (
                                from match in matchColl.Cast<Match>()
                                select match.Value
                                ).ToArray()
                                );
                            if (dt.Rows.Count > maxLine) break;
                        }
                    }
                    return dt;
                }
            }
            return null;
        }
    }
}