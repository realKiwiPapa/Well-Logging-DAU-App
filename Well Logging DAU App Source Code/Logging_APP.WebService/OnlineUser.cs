using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;

using Logging_App.Utility;

namespace Logging_App.WebService
{
    public class OnlineUser
    {
        private static DataTable OnlineUserTable = null;
        private static object dataTableLock = new object();

        public static void Call()
        {
            int timeOut = 30;
            lock (dataTableLock)
            {
                if (OnlineUserTable == null || !OnlineUserTable.Columns.Contains("STARTTIME"))
                {
                    if (OnlineUserTable != null) OnlineUserTable.Dispose();
                    OnlineUserTable = new DataTable();
                    OnlineUserTable.Columns.Add(new DataColumn("SESSIONID", typeof(String)));
                    OnlineUserTable.Columns.Add(new DataColumn("STARTTIME", typeof(DateTime)));
                    OnlineUserTable.Columns.Add(new DataColumn("ENDTIME", typeof(DateTime)));
                    OnlineUserTable.Columns.Add(new DataColumn("USERNAME", typeof(String)));
                    OnlineUserTable.Columns.Add(new DataColumn("IP", typeof(String)));
                }
                DataRow[] drs = OnlineUserTable.Select("ENDTIME < '" + DateTime.Now.AddMinutes(0 - timeOut) + "'");
                foreach (DataRow dr in drs)
                {
                    OnlineUserTable.Rows.Remove(dr);
                }
                string user = ServiceUtils.GetUserInfo().COL_LOGINNAME;
                string ip = ServiceUtils.GetIP();
                string sessionID = ServiceUtils.GetSessionID();
                DateTime startTime=ServiceUtils.GetUserLoginTime();
                drs = OnlineUserTable.Select("USERNAME='" + user+"'");
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        if (dr.FieldEx<string>("SESSIONID") != sessionID && dr.FieldEx<DateTime>("STARTTIME") > startTime)
                        {
                            ServiceUtils.ClearSession();
                            ServiceUtils.ThrowSoapException("没有登录，或登录已经失效，请重新登录！");
                        }
                        dr["SESSIONID"] = sessionID;
                        dr["STARTTIME"] = startTime;
                        dr["ENDTIME"] = DateTime.Now;
                        //dr["USERNAME"] = user;
                        dr["IP"] = ip;
                    }
                }
                else
                {
                    DataRow newRow = OnlineUserTable.NewRow();
                    newRow["SESSIONID"] = sessionID;
                    newRow["STARTTIME"] = DateTime.Now;
                    newRow["ENDTIME"] = DateTime.Now;
                    newRow["USERNAME"] = user;
                    newRow["IP"] = ip;
                    OnlineUserTable.Rows.Add(newRow);
                }
            }
        }

        /// <summary>
        /// 获取time分钟内的用户
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DataSet GetDataTable(int time = 5)
        {
            if (OnlineUserTable == null) return null;
            var dr=OnlineUserTable.Select("ENDTIME >= '" + DateTime.Now.AddMinutes(-time) + "'");
            if(dr.Length<1)return null;
            var dt=dr.CopyToDataTable();
            dt.TableName = "OnlineUser";
            dt.Columns.Remove("SESSIONID");
            DataView dv = dt.DefaultView;
            dv.Sort = "ENDTIME DESC";
            var ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }
    }
}
