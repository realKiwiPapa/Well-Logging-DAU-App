using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Text;
using Logging_App.Model;

namespace Logging_App.Utility
{
    public class CurveDataHelper
    {
        private static readonly Regex regex = new Regex(@"[^\s]+", RegexOptions.Compiled);
        private static readonly Regex regex1 = new Regex(@"#\s+", RegexOptions.Compiled);
        private DataTable TxtToDataTable(string in_Name, int maxLine = int.MaxValue)
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
                    //File.Delete(in_Name);
                    return dt;
                }
            }
            return null;
        }

        private DataTable CreateCurveDT(string tblName, object cls)
        {
            var new_dt = new DataTable(tblName);
            var pi = cls.GetType().GetProperties();
            for (int i = 0; i < pi.Length; i++)
            {
                var colType = pi[i].PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                new_dt.Columns.Add(pi[i].Name, colType);
            }
            return new_dt;
        }

        private Dictionary<string, int[]> GetComboSetting(byte[] combosetting, out int start_line)
        {
            var setting = ModelHelper.DeserializeObject(combosetting) as Dictionary<string, int>;
            var dic = new Dictionary<string, int[]>();
            start_line = setting["StartLine"];
            foreach (var c in setting)
            {
                if (c.Key == "StartLine") continue;
                var names = c.Key.Split('|')[0].Split('-');
                int selectIndex = c.Value;
                string[] tempArr = c.Key.Split(':');
                Type dataType = tempArr.Length == 2 ? Type.GetType(tempArr[1]) : typeof(String);
                if (names.Length > 1)
                {
                    for (int j = 0; j < names.Length; j++)
                    {
                        if (selectIndex > 0) dic.Add(names[j], new int[] { (int)selectIndex, j });
                    }
                }
                else
                {
                    if (selectIndex > 0) dic.Add(names[0], new int[] { (int)selectIndex });
                }
            }
            return dic;
        }

        public DataTable GetProcessingCurveDT(DataTable oldDT, string mainPath)
        {
            var curvedt = CreateCurveDT("processingcurve", new PRO_LOG_PROCESSING_CURVE_INDEX());
            for (int i = 0; i < oldDT.Rows.Count; i++)
            {
                var dr = curvedt.NewRow();
                dr["CURVE_NAME"] = oldDT.Rows[i]["CURVE_NAME"];
                dr["CURVE_START_DEP"] = oldDT.Rows[i]["CURVE_START_DEP"];
                dr["CURVE_END_DEP"] = oldDT.Rows[i]["CURVE_END_DEP"];
                dr["CURVE_RLEV"] = oldDT.Rows[i]["CURVE_RLEV"];
                dr["CURVE_UNIT"] = oldDT.Rows[i]["CURVE_UNIT"];
                //dr["CURVE_T_SAMPLE"] = oldDT.Rows[i]["CURVE_T_SAMPLE"];
                dr["CURVE_T_UNIT"] = oldDT.Rows[i]["CURVE_T_UNIT"];
                //dr["CURVE_T_MAX_VALUE"] = oldDT.Rows[i]["CURVE_T_MAX_VALUE"];
                //dr["CURVE_T_MIN_VALUE"] = oldDT.Rows[i]["CURVE_T_MIN_VALUE"];
                //dr["CURVE_T_RELV"] = oldDT.Rows[i]["CURVE_T_RELV"];
                dr["CURVE_DATA_TYPE"] = oldDT.Rows[i]["CURVE_DATA_TYPE"];
                dr["CURVE_DATA_LENGHT"] = oldDT.Rows[i]["CURVE_DATA_LENGTH"];
                dr["DATA_PROPERTY"] = oldDT.Rows[i]["DATA_PROPERTY"];
                dr["DATA_INFO"] = oldDT.Rows[i]["DATA_INFO"];
                dr["P_CURVESOFTWARE_NAME"] = oldDT.Rows[i]["P_CURVESOFTWARE_NAME"];
                dr["DATA_STORAGE_WAY"] = oldDT.Rows[i]["DATA_STORAGE_WAY"];
                dr["CURVE_NOTE"] = oldDT.Rows[i]["CURVE_NOTE"];
                dr["NOTE"] = oldDT.Rows[i]["NOTE"];
                dr["CURVE_CD"] = oldDT.Rows[i]["CURVE_CD"];
                dr["DATA_SIZE"] = oldDT.Rows[i]["DATA_SIZE"];

                var sha1 = oldDT.Rows[i].FieldEx<string>("SHA1");
                if (sha1 != null)
                {
                    var md5 = oldDT.Rows[i].FieldEx<string>("MD5");
                    var length = oldDT.Rows[i].FieldEx<decimal>("LENGTH");
                    var filePath = string.Format("{0}\\{1}-{2}-{3}", mainPath, sha1, md5, length);
                    if (File.Exists(filePath))
                    {
                        var fs = File.OpenRead(filePath);
                        var buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        dr["BLOCK_NUMBER"] = 0;
                        dr["BLOCK_SIZE"] = buffer.Length;
                        dr["CURVE_DATA"] = buffer;
                        fs.Close();
                        //File.Delete(filePath);
                    }
                    else
                    {
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "tj_err.txt", "未找到文件:" + filePath + "\r\n");
                    }
                }
                curvedt.Rows.Add(dr);
            }
            return curvedt;
        }

        public DataTable GetComLogCurveDT(string filePath, byte[] combosetting)
        {
            int start_line = -1;
            var dic = GetComboSetting(combosetting, out start_line);
            var oldDT = TxtToDataTable(filePath);
            return GetComLogCurveDT(oldDT, dic, start_line);
        }

        public DataTable GetComLogCurveDT(DataTable old_dt, Dictionary<string, int[]> dic, int start_line)
        {
            if (old_dt != null)
            {
                var new_dt = CreateCurveDT("comlogcurve", new COM_LOG_COM_CURVE_INDEX());
                float start_dep = 0;
                float end_dep = 0;
                float curve_max = 0;
                float curve_min = 0;
                float defalut = -999.0f;
                var column_values = new List<float>();
                foreach (var k in dic)
                {
                    column_values.Clear();
                    if (k.Value[0] > old_dt.Columns.Count) continue;
                    for (int m = start_line - 1; m < old_dt.Rows.Count; m++)
                    {
                        float _value = defalut;
                        if (k.Value[0] > 0)
                        {
                            if (old_dt.Rows[m][k.Value[0] - 1] == DBNull.Value)
                            {
                                _value = defalut;
                            }
                            else
                            {
                                _value = Convert.ToSingle(old_dt.Rows[m][k.Value[0] - 1].ToString());
                            }
                        }
                        column_values.Add(_value);
                    }

                    curve_max = column_values.Max();
                    curve_min = column_values.Min();
                    if (k.Key.Equals("DEP"))
                    {
                        start_dep = curve_min;
                        end_dep = curve_max;
                    }
                    else
                    {
                        var dr = new_dt.NewRow();
                        dr["CURVE_CD"] = k.Key;
                        dr["CURVE_START_DEP"] = start_dep;
                        dr["CURVE_END_DEP"] = end_dep;
                        dr["CURVE_RLEV"] = (end_dep - start_dep) / column_values.Count;
                        //dr["CURVE_UNIT"]
                        dr["CURVE_SAMPLE"] = column_values.Count;
                        dr["CURVE_MAX_VALUE"] = curve_max;
                        dr["CURVE_MIN_VALUE"] = curve_min;
                        dr["CURVE_DATA_TYPE"] = "float";
                        dr["CURVE_DATA_LENGTH"] = 4;
                        byte[] buffer = new byte[4 * column_values.Count];
                        for (int i = 0; i < column_values.Count; i++)
                        {
                            byte[] _byte = BitConverter.GetBytes(column_values[i]);
                            _byte.CopyTo(buffer, i * 4);
                        }
                        dr["DATA_SIZE"] = buffer.Length;
                        dr["BLOCK_NUMBER"] = 0;
                        dr["BLOCK_SIZE"] = buffer.Length;
                        dr["CURVE_DATA"] = buffer;
                        new_dt.Rows.Add(dr);
                    }
                }
                return new_dt;
            }
            return null;
        }
    }
}