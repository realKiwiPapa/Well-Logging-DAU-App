using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client;

using log4net;
using Maticsoft.DBUtility;
using Logging_App.Utility;

namespace Logging_App.WebService
{
    public class SyncUserClass
    {
        private static DataTable view;
        private static DataTable group;
        private static DataTable relation;
        private static DataTable user;
        private static StringBuilder logStringBuilder = new StringBuilder();
        private static ILog logger;
        private static readonly List<string> skipusers = new List<string> { "admin" };
        private static object syncLock = new object();

        private enum HsItemType : int
        {
            User = 1,
            Group = 2,
            View = 4
        }
        private enum Disabled : int
        {
            /// <summary>
            /// 记录已经被使用过
            /// </summary>
            Used = -1,
            False = 0,
            True = 1

        }

        public static int Do()
        {
            lock (syncLock)
            {
                view = DbHelperOra.Query("SELECT COL_ID,COL_NAME,COL_DISABLED,COL_ITEMINDEX FROM HS_VIEW ORDER BY COL_ID").Tables[0];
                group = DbHelperOra.Query("SELECT COL_ID,COL_NAME,COL_DISABLED,COL_ITEMINDEX FROM HS_GROUP ORDER BY COL_ID").Tables[0];
                relation = DbHelperOra.Query("SELECT COL_ID,COL_HSITEMTYPE,COL_HSITEMID,COL_DHSITEMID,COL_DHSITEMTYPE,COL_DISABLED FROM HS_RELATION ORDER BY COL_ID").Tables[0];
                user = DbHelperOra.Query("SELECT COL_ID,COL_LOGINNAME,COL_NAME,COL_DISABLED,COL_ITEMINDEX FROM HS_USER WHERE Col_ID > 0 ORDER BY COL_ID").Tables[0];
                DbHelperSQL.connectionString = PubConstant.GetConnectionString("SyncUserConnectionString");
                var view_jrx = DbHelperSQL.Query("SELECT Col_ID,Col_Name,Col_ItemIndex FROM hs_View WHERE Col_Type = 1 AND Col_IsDelete=0 ORDER BY Col_ID").Tables[0];
                foreach (DataRow row in view_jrx.Rows)
                {
                    var rows = view.Select("COL_NAME='" + row.FieldEx<string>("Col_Name") + "' AND COL_DISABLED<>" + (int)Disabled.Used);
                    if (rows.Length > 0)
                    {
                        if (rows[0].FieldEx<decimal>("COL_ITEMINDEX") != row.FieldEx<int>("Col_ItemIndex") || rows[0].FieldEx<decimal>("COL_DISABLED") == (int)Disabled.True)
                        {
                            if (DbHelperOra.ExecuteSql("UPDATE HS_VIEW SET COL_ITEMINDEX=" + row.FieldEx<int>("Col_ItemIndex") + ",COL_DISABLED=" + (int)Disabled.False + " WHERE COL_ID=" + rows[0].FieldEx<decimal>("COL_ID")) > 0)
                            {
                                logStringBuilder.Append("更新View：" + rows[0].FieldEx<string>("COL_NAME") + "\r\n");
                            }
                        }
                        rows[0]["COL_DISABLED"] = (int)Disabled.Used;
                        SyncUser(row.FieldEx<int>("Col_ID"), rows[0].FieldEx<decimal>("COL_ID"), HsItemType.View);
                        SyncGroup(row.FieldEx<int>("Col_ID"), rows[0].FieldEx<decimal>("COL_ID"), HsItemType.View);
                    }
                    else
                    {
                        var col_id = (decimal)DbHelperOra.GetSingle("select HS_VIEW_COL_ID_SEQ.nextval from dual");
                        if (DbHelperOra.ExecuteSql("INSERT INTO HS_VIEW(COL_ID,COL_NAME,COL_ITEMINDEX) VALUES(" + col_id + ",:NAME," + row.FieldEx<int>("Col_ItemIndex") + ")",
                            new OracleParameter[]{
                            ServiceUtils.CreateOracleParameter(":NAME",OracleDbType.Varchar2,row.FieldEx<string>("Col_Name"))
                        }) > 0)
                        {
                            logStringBuilder.Append("添加View：" + row.FieldEx<string>("Col_Name") + "\r\n");
                            SyncUser(row.FieldEx<int>("Col_ID"), col_id, HsItemType.View);
                            SyncGroup(row.FieldEx<int>("Col_ID"), col_id, HsItemType.View);
                        }

                    }
                }

                int num = logStringBuilder.Length;
                if (num > 0)
                {
                    if (logger == null) logger = LogManager.GetLogger("SyncUserLog");
                    logger.Info(logStringBuilder);
                    logStringBuilder.Clear();
                }
                SyncDisabled();
                return num;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hsItemID_jrx">jrx父对象ID</param>
        /// <param name="hsItemID">父对象ID</param>
        /// <param name="hsItemType">父对象类型</param>
        private static void SyncUser(decimal hsItemID_jrx, decimal hsItemID, HsItemType hsItemType)
        {
            int num = 0;
            var user_jrx = DbHelperSQL.Query("SELECT a.Col_LoginName,a.Col_Name,a.Col_ItemIndex FROM hs_User a,hs_Relation b WHERE b.Col_HsItemType = " + (int)hsItemType + " AND b.Col_HsItemID = " + (int)hsItemID_jrx + " AND a.Col_Disabled=0 AND a.Col_IsDelete=0 AND b.Col_DHsItemType = " + (int)HsItemType.User + " AND b.Col_DHsItemID = a.Col_ID ORDER BY a.Col_ID").Tables[0];
            foreach (DataRow row in user_jrx.Rows)
            {
                if (skipusers.Exists(o => o == row.FieldEx<string>("Col_LoginName"))) continue;
                var rows = user.Select("COL_LOGINNAME='" + row.FieldEx<string>("Col_LoginName") + "'");//+ "' AND COL_DISABLED<>" + (int)Disabled.Used
                if (rows.Length > 0)
                {

                    if (rows[0].FieldEx<string>("COL_NAME") != row.FieldEx<string>("Col_Name") || rows[0].FieldEx<decimal>("COL_ITEMINDEX") != row.FieldEx<int>("Col_ItemIndex") || rows[0].FieldEx<decimal>("COL_DISABLED") == (int)Disabled.True)
                    {
                        num = DbHelperOra.ExecuteSql("UPDATE HS_USER SET COL_NAME=:NAME,COL_ITEMINDEX=" + row.FieldEx<int>("Col_ItemIndex") + ",COL_DISABLED=" + (int)Disabled.False + " WHERE COL_ID=" + rows[0].FieldEx<decimal>("COL_ID"),
                            new OracleParameter[]{
                            ServiceUtils.CreateOracleParameter(":NAME",OracleDbType.Varchar2,rows[0].FieldEx<string>("COL_NAME"))
                            });

                    }
                    rows[0]["COL_DISABLED"] = (int)Disabled.Used;
                    if (num > 0 || SyncRelation(hsItemID, hsItemType, rows[0].FieldEx<decimal>("Col_ID"), HsItemType.User) > 0)
                    {
                        logStringBuilder.Append("更新User：" + rows[0].FieldEx<string>("COL_NAME") + "(" + rows[0].FieldEx<string>("COL_LOGINNAME") + ")\r\n");
                    }
                }
                else
                {
                    var col_id = (decimal)DbHelperOra.GetSingle("select HS_USER_COL_ID_SEQ.nextval from dual");
                    num = DbHelperOra.ExecuteSql("INSERT INTO HS_USER(COL_ID,COL_LOGINNAME,COL_NAME,COL_ITEMINDEX) VALUES(" + col_id + ",:LOGINNAME,:NAME," + row.FieldEx<int>("Col_ItemIndex") + ")",
                        new OracleParameter[]{
                            ServiceUtils.CreateOracleParameter(":LOGINNAME",OracleDbType.Varchar2,row.FieldEx<string>("Col_LoginName")),
                            ServiceUtils.CreateOracleParameter(":NAME",OracleDbType.Varchar2,row.FieldEx<string>("Col_Name"))
                        });
                    if (num > 0 && SyncRelation(hsItemID, hsItemType, col_id, HsItemType.User) > 0)
                    {
                        user.Rows.Add(col_id, row.FieldEx<string>("Col_LoginName"), row.FieldEx<string>("Col_Name"), (int)Disabled.Used, row.FieldEx<int>("Col_ItemIndex"));
                        logStringBuilder.Append("添加User：" + row.FieldEx<string>("Col_Name") + "(" + row.FieldEx<string>("Col_LoginName") + ")\r\n");
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hsItemID_jrx">jrx父对象ID</param>
        /// <param name="hsItemID">父对象ID</param>
        /// <param name="hsItemType">父对象类型</param>
        private static void SyncGroup(decimal hsItemID_jrx, decimal hsItemID, HsItemType hsItemType)
        {
            int num = 0;
            var group_jrx = DbHelperSQL.Query("SELECT a.Col_ID,a.Col_Name,a.Col_ItemIndex FROM hs_Group a,hs_Relation b WHERE b.Col_DHsItemID = a.Col_ID AND b.Col_DHsItemType = 2 AND b.Col_HsItemID = " + (int)hsItemID_jrx + " AND b.Col_HsItemType = " + (int)hsItemType + " AND b.Col_RelType = 1 AND a.col_IsDelete=0 ORDER BY a.Col_ID").Tables[0];
            foreach (DataRow row in group_jrx.Rows)
            {
                var rows = group.Select("COL_NAME='" + row.FieldEx<string>("Col_Name") + "' AND COL_DISABLED<>" + (int)Disabled.Used);
                if (rows.Length > 0)
                {
                    if (rows[0].FieldEx<decimal>("COL_ITEMINDEX") != row.FieldEx<int>("Col_ItemIndex") || rows[0].FieldEx<decimal>("COL_DISABLED") == (int)Disabled.True)
                    {
                        num = DbHelperOra.ExecuteSql("UPDATE HS_GROUP SET COL_ITEMINDEX=" + row.FieldEx<int>("Col_ItemIndex") + ",COL_DISABLED=" + (int)Disabled.False + " WHERE COL_ID=" + rows[0].FieldEx<decimal>("COL_ID"));

                    }
                    rows[0]["COL_DISABLED"] = (int)Disabled.Used;
                    if (num > 0 || SyncRelation(hsItemID, hsItemType, rows[0].FieldEx<decimal>("Col_ID"), HsItemType.Group) > 0)
                    {
                        logStringBuilder.Append("更新Group：" + rows[0].FieldEx<string>("COL_NAME") + "\r\n");
                    }
                    SyncUser(row.FieldEx<int>("Col_ID"), rows[0].FieldEx<decimal>("COL_ID"), HsItemType.Group);
                    SyncGroup(row.FieldEx<int>("Col_ID"), rows[0].FieldEx<decimal>("COL_ID"), HsItemType.Group);
                }
                else
                {
                    var col_id = (decimal)DbHelperOra.GetSingle("select HS_GROUP_COL_ID_SEQ.nextval from dual");
                    num = DbHelperOra.ExecuteSql("INSERT INTO HS_GROUP(COL_ID,COL_NAME,COL_ITEMINDEX) VALUES(" + col_id + ",:NAME," + row.FieldEx<int>("Col_ItemIndex") + ")",
                        new OracleParameter[]{
                            ServiceUtils.CreateOracleParameter(":NAME",OracleDbType.Varchar2,row.FieldEx<string>("Col_Name"))
                        });
                    if (num > 0 && SyncRelation(hsItemID, hsItemType, col_id, HsItemType.Group) > 0)
                    {
                        logStringBuilder.Append("添加Group：" + row.FieldEx<string>("Col_Name") + "\r\n");
                        SyncUser(row.FieldEx<int>("Col_ID"), col_id, HsItemType.Group);
                        SyncGroup(row.FieldEx<int>("Col_ID"), col_id, HsItemType.Group);
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hsItemID">父对象ID</param>
        /// <param name="hsItemType">父对象类型</param>
        /// <param name="dHsItemID">子对象ID</param>
        /// <param name="dHsItemType">子对象类型</param>
        private static int SyncRelation(decimal hsItemID, HsItemType hsItemType, decimal dHsItemID, HsItemType dHsItemType)
        {
            var rows = relation.Select("COL_HSITEMTYPE=" + (int)hsItemType + " AND COL_HSITEMID=" + hsItemID + " AND COL_DHSITEMID=" + dHsItemID + " AND COL_DHSITEMTYPE=" + (int)dHsItemType + " AND COL_DISABLED<>" + (int)Disabled.Used);
            if (rows.Length > 0)
            {
                rows[0]["COL_DISABLED"] = (int)Disabled.Used;
            }
            else
            {
                return DbHelperOra.ExecuteSql("INSERT INTO HS_RELATION(COL_ID,COL_HSITEMTYPE,COL_HSITEMID,COL_DHSITEMID,COL_DHSITEMTYPE) VALUES(HS_RELATION_COL_ID_SEQ.nextval," + (int)hsItemType + "," + hsItemID + "," + dHsItemID + "," + (int)dHsItemType + ")");
            }
            return 0;
        }

        private static void SyncDisabled()
        {
            var rows = view.Select("COL_DISABLED<>" + (int)Disabled.Used);
            string ids;
            if (rows.Length > 0)
            {
                ids = string.Join(",", rows.ToArray().Select(t => t.FieldEx<decimal>("COL_ID")).ToArray());
                if (ids.Length > 0)
                {
                    DbHelperOra.ExecuteSql("UPDATE HS_VIEW SET COL_DISABLED=1 WHERE COL_ID IN (" + ids + ")");
                }
            }
            rows = group.Select("COL_DISABLED<>" + (int)Disabled.Used);
            if (rows.Length > 0)
            {
                ids = string.Join(",", rows.ToArray().Select(t => t.FieldEx<decimal>("COL_ID")).ToArray());
                if (ids.Length > 0)
                {
                    DbHelperOra.ExecuteSql("UPDATE HS_GROUP SET COL_DISABLED=1 WHERE COL_ID IN (" + ids + ")");
                }
            }
            rows = user.Select("COL_DISABLED<>" + (int)Disabled.Used);
            if (rows.Length > 0)
            {
                ids = string.Join(",", rows.ToArray().Select(t => t.FieldEx<decimal>("COL_ID")).ToArray());
                if (ids.Length > 0)
                {
                    DbHelperOra.ExecuteSql("UPDATE HS_USER SET COL_DISABLED=1 WHERE COL_ID IN (" + ids + ")");
                }
            }
            rows = relation.Select("COL_DISABLED<>" + (int)Disabled.Used);
            if (rows.Length > 0)
            {
                ids = string.Join(",", rows.ToArray().Select(t => t.FieldEx<decimal>("COL_ID")).ToArray());
                if (ids.Length > 0)
                {
                    DbHelperOra.ExecuteSql("DELETE HS_RELATION WHERE COL_ID IN (" + ids + ")");
                }
            }
        }
    }
}