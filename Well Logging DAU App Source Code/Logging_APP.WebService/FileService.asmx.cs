using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Oracle.DataAccess.Client;
using Maticsoft.DBUtility;

namespace Logging_App.WebService
{
    /// <summary>
    /// FileService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class FileService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public UploadController.UploadState CreateUploadTask(string sha1, string md5, long length, string tasktype)
        {
            switch (tasktype.ToLower())
            {
                case "fileupload":
                    return FileUpload.Create(sha1, md5, length);
                case "packupload":
                    return PackUpload.Create(sha1, md5, length);
                default:
                    throw new ArgumentException("tasktype无效");
            }
        }

        [WebMethod(EnableSession = true)]
        public UploadController.UploadState GetUploadState(int taskID)
        {
            return UploadController.GetUploadSate(taskID);
        }


        [WebMethod(EnableSession = true)]
        public UploadController.UploadState UploadWrite(int taskID, long position, byte[] bData)
        {
            return UploadController.Write(taskID, position, bData);
        }

        [WebMethod(EnableSession = true)]
        public DownloadController.DownloadState CreateDownloadTask(decimal? fileid, string filename, string tasktype)
        {
            switch (tasktype.ToLower())
            {
                case "updatedownload":
                    return UpdateDownload.Create(filename);
                case "filedownload":
                    return FileDownload.Create((decimal)fileid);
                case "plugindownload":
                    return PluginDownload.Create(filename);
                default:
                    throw new ArgumentException("tasktype无效");
            }
        }

        [WebMethod(EnableSession = true)]
        public DownloadController.DownloadState DownloadRead(int taskID, long position)
        {
            return DownloadController.Read(taskID, position);
        }

        [WebMethod(EnableSession = true)]
        public string GetVersion()
        {
            return AutoUpdate.Version;
        }

        [WebMethod(EnableSession = true)]
        public List<PluginManager.PluginInfo> GetPluginsInfo()
        {
            return PluginManager.PluginsInfo;
        }

        [WebMethod(EnableSession = true)]
        public List<AutoUpdate.UpdateData> GetUpdateData()
        {
            return AutoUpdate.UpdateDataList;
        }

        [WebMethod(EnableSession = true)]
        public void SaveUploadInfo(string sha1,string md5,long length,decimal pathid) {
            string pathmain = FileUpload.PathList[0].name;
            pathmain = pathmain.Substring(pathmain.LastIndexOf('\\') + 1);
            DbHelperOra.ExecuteSql("DECLARE v_cnt NUMBER;BEGIN SELECT COUNT(1) INTO v_cnt FROM sys_upload WHERE sha1=:SHA1 AND md5=:MD5 AND length=:LENGTH;if v_cnt>0 then update sys_upload set pathid=:PATHID WHERE sha1=:SHA1 AND md5=:MD5 AND length=:LENGTH; else INSERT INTO sys_upload(UPLOADID,SHA1,MD5,LENGTH,PATHID,PATHMAIN) values (UPLOADID_SEQ.NEXTVAL,:SHA1,:MD5,:LENGTH,:PATHID,:PATHMAIN);end if;end;",
                ServiceUtils.CreateOracleParameter(":SHA1", OracleDbType.Char, sha1),
                ServiceUtils.CreateOracleParameter(":MD5", OracleDbType.Varchar2, md5),
                ServiceUtils.CreateOracleParameter(":LENGTH", OracleDbType.Decimal, length),
                ServiceUtils.CreateOracleParameter(":PATHID", OracleDbType.Decimal, pathid),
                ServiceUtils.CreateOracleParameter(":PATHMAIN", OracleDbType.NVarchar2, pathmain)
                );
        }

        [WebMethod(EnableSession = true)]
        public decimal GetUploadID(string sha1, string md5, long length)
        {
            return (decimal)DbHelperOra.GetSingle("select uploadid from SYS_UPLOAD where sha1=:SHA1 and md5=:MD5 and length=:LENGTH",
                ServiceUtils.CreateOracleParameter(":SHA1", OracleDbType.Varchar2, sha1),
                ServiceUtils.CreateOracleParameter(":MD5", OracleDbType.Varchar2, md5),
                ServiceUtils.CreateOracleParameter(":LENGTH", OracleDbType.Decimal, length)
                );
        }
        [WebMethod(EnableSession = true)]
        public string GetFileName(decimal fileid)
        {
            return (string)DbHelperOra.GetSingle("select filename from SYS_FILE_UPLOAD where FILEID=" + fileid);
        }

        [WebMethod(EnableSession = true)]
        public decimal SaveFileUploadInfo(string filename, string sha1, string md5, long length, string filepath)
        {
            var parms = new System.Data.IDataParameter[6];
            parms[0] = ServiceUtils.CreateOracleParameter("v_sha1", OracleDbType.Varchar2, sha1);
            parms[1] = ServiceUtils.CreateOracleParameter("v_md5", OracleDbType.Varchar2, md5);
            parms[2] = ServiceUtils.CreateOracleParameter("v_length", OracleDbType.Decimal, length);
            parms[3] = ServiceUtils.CreateOracleParameter("v_filename", OracleDbType.NVarchar2, filename);
            parms[4] = ServiceUtils.CreateOracleParameter("v_filepath", OracleDbType.NVarchar2, filepath);
            parms[5] = new OracleParameter("v_fileid", OracleDbType.Decimal);
            parms[5].Direction = System.Data.ParameterDirection.Output;
            DbHelperOra.RunProcedure("savefileuploadinfo", parms, "data");
            return (decimal)(Oracle.DataAccess.Types.OracleDecimal)parms[5].Value;
        }

        [WebMethod(EnableSession = true)]
        public byte[] Export_上井解释登记卡(string process_id)
        {
            //井基本情况
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT A.PROCESS_NAME, ");
            SQL.Append("  B.WELL_JOB_NAME, ");
            SQL.Append("  B.WELL_SORT, ");
            SQL.Append("  C.BEOG_LOCATION, ");
            SQL.Append("  C.WELL_STRUCT_UNIT_NAME , ");
            SQL.Append("  C.PART_UNITS, ");
            SQL.Append("  D.TEAM_ORG_ID, ");
            SQL.Append("  D.LOG_SERIES_ID, ");
            SQL.Append("  D.P_PROCESS_SOFTWARE, ");
            SQL.Append("  D.P_SUPERVISOR , ");
            SQL.Append("  D.NOTE, ");
            SQL.Append("  D.FILE_NUMBER, ");
            SQL.Append("  TO_CHAR(D.LOG_START_TIME,'yyyy-MM-DD HH24:MI') LOG_START_TIME1, ");
            SQL.Append("  D.PROCESSOR , ");
            SQL.Append("  D.INTERPRETER , ");
            SQL.Append("  NVL(D.LOG_ORIGINALITY_DATA,0)+NVL(D.LOG_INTERPRET_REPORT,0)+NVL(LOG_INTERPRET_RESULT,0) FILE_SIZE, ");
            //SQL.Append("  E.MAX_WELL_DEVIATION , ");
            //SQL.Append("  E.MAX_WELL_DEVIATION_MD , ");
            SQL.Append("  0 MAP_TYPE_NUMBER ");
            SQL.Append("FROM DM_LOG_PROCESS A, ");
            SQL.Append("  COM_JOB_INFO B, ");
            SQL.Append("  COM_WELL_BASIC C, ");
            SQL.Append("  PRO_LOG_DATA_PUBLISH D ");
            //SQL.Append("  COM_WELLBORE_BASIC E ");
            SQL.Append("WHERE A.DRILL_JOB_ID=B.DRILL_JOB_ID ");
            SQL.Append("AND B.WELL_ID       =C.WELL_ID ");
            SQL.Append("AND A.PROCESS_ID    =D.PROCESS_ID ");
            //SQL.Append("AND C.WELL_ID       =E.WELL_ID(+) ");
            SQL.Append("AND A.PROCESS_ID    =:PROCESS_ID ");
            SQL.Append("AND ROWNUM          =1");
            var dt1 = DbHelperOra.Query(SQL.ToString(), ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];

            SQL.Clear();
            SQL.Append("SELECT * ");
            SQL.Append("FROM ");
            SQL.Append("  (SELECT B.LOG_TEAM_LEADER, ");
            SQL.Append("    B.LOG_OPERATOR__NAME, ");
            SQL.Append("    B.FIELD_INSPECTOR, ");
            //SQL.Append("    A.JOB_PLAN_CD, ");
            //SQL.Append("    A.LOG_DATA_ID, ");
            //SQL.Append("    FIRST_VALUE(C.CEMENT_PROPERTIES) OVER (ORDER BY C.UPDATE_DATE DESC NULLS LAST) CEMENT_PROPERTIES, ");
            SQL.Append("    FIRST_VALUE(C.CEMENT_PRE_TOP) OVER (ORDER BY C.UPDATE_DATE DESC NULLS LAST) CEMENT_PRE_TOP, ");
            SQL.Append("    FIRST_VALUE(C.CEMENT_HEIGHT) OVER (ORDER BY C.UPDATE_DATE DESC NULLS LAST) CEMENT_HEIGHT, ");
            SQL.Append("    FIRST_VALUE(C.CEMENT_DENSITY_MIN_VALUE ");
            SQL.Append("    || '-' ");
            SQL.Append("    || C.CEMENT_DENSITY_MAX_VALUE) OVER (ORDER BY C.UPDATE_DATE DESC NULLS LAST) CEMENT_DENSITY_VALUE, ");
            SQL.Append("    FIRST_VALUE(D.CASING_OUTSIZE ");
            SQL.Append("    || 'x' ");
            SQL.Append("    || D.PIPE_HEIGHT) OVER (ORDER BY D.UPDATE_DATE DESC NULLS LAST) CASING_OUTSIZE_LENGTH, ");
            SQL.Append("    E.PRELOGGING_INTERVAL, ");
            SQL.Append("    F.LOG_TYPE, ");
            SQL.Append("    F.MAXIMUM_SLOPE, ");
            SQL.Append("    F.MAXIMUM_SLOPE_DEPTH, ");
            SQL.Append("    TO_CHAR(G.LOG_START_TIME, 'yyyy-MM-DD HH24:MI') LOG_START_TIME, ");
            SQL.Append("    TO_CHAR(G.LOG_END_TIME, 'yyyy-MM-DD HH24:MI') LOG_END_TIME, ");
            SQL.Append("    G.LOST_TIME, ");
            SQL.Append("    FIRST_VALUE(H.BIT_SIZE ");
            SQL.Append("    || 'x' ");
            SQL.Append("    || H.BIT_DEP) OVER (ORDER BY H.UPDATE_DATE DESC NULLS LAST) BIT_SIZE_DEP, ");
            SQL.Append("    FIRST_VALUE(I.BOTTOM_TEMPERATURE) OVER (ORDER BY SUBMIT_DATE DESC NULLS LAST) BOTTOM_TEMPERATURE, ");
            SQL.Append("    FIRST_VALUE(decode(substr(I.MUD_RESITIVITY,1,1),'.','0'||I.MUD_RESITIVITY,I.MUD_RESITIVITY)||'/'||I.MUD_TEMERATURE) OVER (ORDER BY SUBMIT_DATE DESC NULLS LAST) MUD_RESITIVITY_TEMERATURE,");
            SQL.Append("    G.LOG_TOTAL_TIME , ");
            SQL.Append("    FIRST_VALUE(J.JTLT) OVER (ORDER BY J.UPDATE_DATE DESC NULLS LAST) JTLT, ");
            SQL.Append("    FIRST_VALUE(J.DRILL_FLU_VISC) OVER (ORDER BY J.UPDATE_DATE DESC NULLS LAST) DRILL_FLU_VISC , ");
            SQL.Append("    FIRST_VALUE(J.P_XN_MUD_DENSITY) OVER (ORDER BY J.UPDATE_DATE DESC NULLS LAST) P_XN_MUD_DENSITY ");

            SQL.Append("  FROM DM_LOG_SOURCE_DATA A, ");
            SQL.Append("    DM_LOG_WORK_PERSONNEL B, ");
            SQL.Append("    PRO_LOG_CEMENT C, ");
            SQL.Append("    PRO_LOG_CASIN D, ");
            SQL.Append("    DM_LOG_OPS_PLAN E, ");
            SQL.Append("    DM_LOG_BASE F, ");
            SQL.Append("    DM_LOG_WORK_ANING G, ");
            SQL.Append("    PRO_LOG_BIT_PROGRAM H, ");
            SQL.Append("    PRO_LOG_RAPID_INFO I , ");
            SQL.Append("    PRO_LOG_SLOP J ");
            SQL.Append("  WHERE A.JOB_PLAN_CD = B.JOB_PLAN_CD(+) ");
            SQL.Append("  AND A.JOB_PLAN_CD   = C.JOB_PLAN_CD(+) ");
            SQL.Append("  AND A.JOB_PLAN_CD   = D.JOB_PLAN_CD(+) ");
            SQL.Append("  AND A.JOB_PLAN_CD   = E.JOB_PLAN_CD(+) ");
            SQL.Append("  AND A.JOB_PLAN_CD   = F.JOB_PLAN_CD(+) ");
            SQL.Append("  AND A.JOB_PLAN_CD   = G.JOB_PLAN_CD(+) ");
            SQL.Append("  AND A.JOB_PLAN_CD   = H.JOB_PLAN_CD(+) ");
            SQL.Append("  AND A.JOB_PLAN_CD   = I.JOB_PLAN_CD(+) ");
            SQL.Append("  AND A.JOB_PLAN_CD   = J.JOB_PLAN_CD(+) ");
            SQL.Append("  AND A.LOG_DATA_ID   = :PROCESS_ID ");
            SQL.Append("  ) ");
            SQL.Append("WHERE ROWNUM = 1");
            var dt2 = DbHelperOra.Query(SQL.ToString(), ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
            //测井项目
            SQL.Clear();
            //SQL.Append("SELECT DM_LOG_ITEMS.LOGGING_NAME, ");
            //SQL.Append("  DM_LOG_ITEMS.SCALE, ");
            //SQL.Append("  DM_LOG_ITEMS.ST_DEP ");
            //SQL.Append("  ||'-' ");
            //SQL.Append("  ||DM_LOG_ITEMS.EN_DEP ST_EN_DEP ");
            //SQL.Append("FROM DM_LOG_ITEMS, ");
            //SQL.Append("  DM_LOG_SOURCE_DATA ");
            //SQL.Append("WHERE DM_LOG_SOURCE_DATA.JOB_PLAN_CD = DM_LOG_ITEMS.JOB_PLAN_CD ");
            //SQL.Append("AND DM_LOG_SOURCE_DATA.LOG_DATA_ID   = :PROCESS_ID");
            //SQL.Append("SELECT A.LOGGING_NAME, ");
            //SQL.Append("  A.SCALE, ");
            //SQL.Append("  A.INDOOR_RATING, ");
            //SQL.Append("  B.ST_EN_DEP ");
            //SQL.Append("FROM PRO_LOG_PROCESSING_CURVERATING A, ");
            //SQL.Append("  (SELECT DM_LOG_ITEMS.LOGGING_NAME, ");
            //SQL.Append("    MIN( DM_LOG_ITEMS.ST_DEP ) ");
            //SQL.Append("    ||'-' ");
            //SQL.Append("    || MAX( DM_LOG_ITEMS.EN_DEP ) ST_EN_DEP ");
            //SQL.Append("  FROM DM_LOG_ITEMS, ");
            //SQL.Append("    DM_LOG_SOURCE_DATA ");
            //SQL.Append("  WHERE DM_LOG_SOURCE_DATA.JOB_PLAN_CD = DM_LOG_ITEMS.JOB_PLAN_CD ");
            //SQL.Append("  AND DM_LOG_SOURCE_DATA.LOG_DATA_ID   = :PROCESS_ID ");
            //SQL.Append("  GROUP BY DM_LOG_ITEMS.LOGGING_NAME ");
            //SQL.Append("  ) B ");
            //SQL.Append("WHERE A.PROCESS_ID=:PROCESS_ID ");
            //SQL.Append("AND A.LOGGING_NAME=B.LOGGING_NAME(+)");
            SQL.Append("SELECT B.CURVE_NAME, ");
            SQL.Append("  C.PROCESSING_ITEM_NAME, ");
            SQL.Append("  A.SCALE, ");
            SQL.Append("  A.INDOOR_RATING, ");
            SQL.Append("  A.START_DEP, ");
            SQL.Append("  A.END_DEP ");
            SQL.Append("FROM PRO_LOG_PROCESSING_CURVERATING A, ");
            SQL.Append("  PKL_LOG_OPS_CURVE B, ");
            SQL.Append("  PKL_LOG_OPS_PROJECT C ");
            SQL.Append("WHERE A.PROCESS_ID      =:PROCESS_ID ");
            SQL.Append("AND A.CURVE_ID          =B.CURVE_ID(+) ");
            SQL.Append("AND A.PROCESSING_ITEM_ID=C.PROCESSING_ITEM_ID(+)");
            var dt3 = DbHelperOra.Query(SQL.ToString(), ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
            dt3.TableName = "ITEMS";
            ///地质、测井分层
            SQL.Clear();
            SQL.Append("SELECT A.FORMATION_NAME, ");
            SQL.Append("  A.ST_DEVIATED_DEP, ");
            SQL.Append("  A.EN_DEVIATED_DEP, ");
            SQL.Append("  B.BOTTOM_DEPTH ");
            SQL.Append("FROM COM_LOG_LAYER A, ");
            SQL.Append("  (SELECT A.STRAT_UNIT_NAME, ");
            SQL.Append("    MAX(A.BOTTOM_DEPTH) BOTTOM_DEPTH ");
            SQL.Append("  FROM COM_BASE_STRATA_LAYER2 A, ");
            SQL.Append("    DM_LOG_SOURCE_DATA B ");
            SQL.Append("  WHERE B.LOG_DATA_ID=:PROCESS_ID ");
            SQL.Append("  AND B.JOB_PLAN_CD  =A.JOB_PLAN_CD ");
            SQL.Append("  GROUP BY A.STRAT_UNIT_NAME ");
            SQL.Append("  ) B ");
            SQL.Append("WHERE PROCESS_ID    =:PROCESS_ID ");
            SQL.Append("AND A.FORMATION_NAME=B.STRAT_UNIT_NAME(+) ");
            SQL.Append("ORDER BY A.LAYERID");
            var dt4 = DbHelperOra.Query(SQL.ToString(), ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
            dt4.TableName = "LAYER";
            if (dt4.Rows.Count > 0 && dt4.Rows[dt4.Rows.Count - 1]["ST_DEVIATED_DEP"] == DBNull.Value)
                dt4.Rows[dt4.Rows.Count - 1]["ST_DEVIATED_DEP"] = dt4.Rows[dt4.Rows.Count - 1]["EN_DEVIATED_DEP"];
            dt4.Columns.Add("FORMATION_NAME1");
            //dt4.Columns.Add("ST_DEVIATED_DEP1", typeof(decimal));
            dt4.Columns.Add("EN_DEVIATED_DEP1", typeof(decimal));
            dt4.Columns.Add("BOTTOM_DEPTH1", typeof(decimal));
            if (dt4.Rows.Count > 1)
            {
                int maxRow = (int)Math.Ceiling(dt4.Rows.Count / 2M);
                for (int i = maxRow; i < dt4.Rows.Count; i++)
                {
                    var row1 = dt4.Rows[i - maxRow];
                    var row2 = dt4.Rows[i];
                    row1["FORMATION_NAME1"] = row2["FORMATION_NAME"];
                    //row1["ST_DEVIATED_DEP1"] = row2["ST_DEVIATED_DEP"];
                    row1["EN_DEVIATED_DEP1"] = row2["EN_DEVIATED_DEP"];
                    row1["BOTTOM_DEPTH1"] = row2["BOTTOM_DEPTH"];
                    row2.Delete();
                }
                dt4.AcceptChanges();
            }
            //综合解释成果表
            SQL.Clear();
            SQL.Append("SELECT LAY_ID, ");
            SQL.Append("  FORMATION_NAME, ");
            SQL.Append("  START_DEPTH ");
            SQL.Append("  ||'-' ");
            SQL.Append("  || END_DEPTH START_END_DEPTH, ");
            SQL.Append("  VALID_THICKNESS, ");
            SQL.Append("  POROSITY_MIN_VALUE ");
            SQL.Append("  ||'-' ");
            SQL.Append("  || POROSITY_MAX_VALUE POROSITY, ");
            SQL.Append("  WATER_SATURATION_MIN_VALUE ");
            SQL.Append("  ||'-' ");
            SQL.Append("  || WATER_SATURATION_MAX_VALUE WATER_SATURATION, ");
            SQL.Append("  EXPLAIN_CONCLUSION ");
            SQL.Append("FROM COM_LOG_RESULT ");
            SQL.Append("WHERE PROCESS_ID=:PROCESS_ID ");
            SQL.Append("ORDER BY to_number(LAY_ID) Nulls Last");
            var dt5 = DbHelperOra.Query(SQL.ToString(), ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
            dt5.TableName = "RESULT";
            //测井图件报送登记
            SQL.Clear();
            SQL.Append("SELECT ROWNUM, ");
            SQL.Append("  A.MAPS_CODING, ");
            SQL.Append("  C.MAPS_CHINESE_NAME, ");
            SQL.Append("  A.MAP_START_DEP ");
            SQL.Append("  ||'-' ");
            SQL.Append("  || A.MAP_END_DEP MAP_START_END_DEP, ");
            SQL.Append("  A.MAP_SCALE, ");
            SQL.Append("  A.MAP_DATA_NAME, ");
            SQL.Append("  A.P_PROCESS_SOFTWARE , ");
            SQL.Append("  B.PROCESSOR ");
            SQL.Append("FROM PRO_LOG_PROCESS_MAP A, ");
            SQL.Append("  PRO_LOG_DATA_PUBLISH B, ");
            SQL.Append("  PKL_LOG_OPS_MAP C ");
            SQL.Append("WHERE A.PROCESS_ID=:PROCESS_ID ");
            SQL.Append("AND A.PROCESS_ID  =B.PROCESS_ID ");
            SQL.Append("AND A.MAPS_CODING =C.MAPS_CODING ");
            SQL.Append("ORDER BY A.ROWID");
            var dt6 = DbHelperOra.Query(SQL.ToString(), ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
            dt6.TableName = "MAP";
            int xrmiCount = dt6.Select("MAPS_CODING like 'xrmi*'").Length;
            dt1.Rows[0]["MAP_TYPE_NUMBER"] = xrmiCount > 1 ? dt6.Rows.Count - xrmiCount + 1 : dt6.Rows.Count;
            using (FileStream stream = File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\上井解释登记卡.doc")))
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(stream);
                doc.MailMerge.Execute(dt1);
                doc.MailMerge.Execute(dt2);
                doc.MailMerge.ExecuteWithRegions(dt3);
                doc.MailMerge.ExecuteWithRegions(dt4);
                doc.MailMerge.ExecuteWithRegions(dt5);
                doc.MailMerge.ExecuteWithRegions(dt6);
                //doc.MailMerge.DeleteFields();
                MemoryStream ms = new MemoryStream();
                doc.Save(ms, Aspose.Words.SaveFormat.Doc);
                return ms.GetBuffer();
            }
        }

    }
}
