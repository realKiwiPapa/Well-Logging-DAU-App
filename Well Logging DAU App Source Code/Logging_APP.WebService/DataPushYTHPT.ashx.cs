using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Data;
using Oracle.DataAccess.Client;
using Maticsoft.DBUtility;

namespace Logging_App.WebService
{
    /// <summary>
    /// DataPush 的摘要说明
    /// </summary>
    public class DataPushYTHPT : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private static bool isRunning = false;
        private HttpContext _context;
        private StringBuilder logBuilder = null;
        private void WriteLine(string msg)
        {
            _context.Response.Write(msg);
            _context.Response.Flush();
            if (logBuilder != null)
            {
                logBuilder.Append(msg + "\r\n");
            }

        }

        private void WriteLog(string msg)
        {
            WriteLine("正在推送：" + msg + "。。。");
        }

        public void ProcessRequest(HttpContext context)
        {
            _context = context;
            if (isRunning)
            {
                WriteLine("一个推送任务正在进行！");
                return;
            }
            isRunning = true;
            try
            {
                string process_id = context.Request.Form["process_id"];
                if (!new UserService().GetActiveUserRoles().Contains(ServiceEnums.UserRole.调度管理员))
                {
                    WriteLine("没有权限！");
                    return;
                }
                var dt = DbHelperOra.Query("select A.PROCESS_NAME,B.DRILL_JOB_ID,B.WELL_ID,E.JOB_PLAN_CD,E.REQUISITION_CD from DM_LOG_PROCESS a,COM_JOB_INFO b,dm_log_source_data d,dm_log_ops_plan e where A.DRILL_JOB_ID=B.DRILL_JOB_ID and A.PROCESS_ID=D.LOG_DATA_ID and D.JOB_PLAN_CD=E.JOB_PLAN_CD and A.PROCESS_ID=" + process_id).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    WriteLine("参数不正确！");
                    return;
                }
                var row = dt.Rows[0];
                string user_name = context.Request.Form["active_user"];
                string process_name = row.Field<string>("PROCESS_NAME");
                string drill_job_id = row.Field<string>("DRILL_JOB_ID");
                string well_id = row.Field<string>("WELL_ID");
                string job_plan_cd = row.Field<string>("JOB_PLAN_CD");
                string requisition_cd = row.Field<string>("REQUISITION_CD");

                DateTime StartTime = DateTime.Now;

                logBuilder = new StringBuilder();
                logBuilder.Append("-".PadLeft(20, '-') + "\r\n");
                logBuilder.Append(ServiceUtils.GetUserInfo().COL_LOGINNAME + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "\r\n");
                bool isExist;
                StringBuilder strSql = new StringBuilder();
                WriteLine("开始推送：" + process_name);
                #region 井基本数据
                WriteLog("井基本数据");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_WELL_BASIC where rownum<2 and WELL_ID=:WELL_ID", ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, well_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_WELL_BASIC where WELL_ID=:WELL_ID", ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, well_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_WELL_BASIC(");
                    strSql.Append("WELL_ID,SITE_ID,WELL_NAME,WELL_LEGAL_NAME,WELL_UWI,WELL_STRUCT_UNIT_DESCRIPTION,WELL_FIELD_UNIT_NAME,WELL_STRUCT_UNIT_NAME,BEOG_LOCATION,STRUCTURAL_LOCATION,TRAVERSE_LINE_LOCATION,SURVEY_X_AXIS,SURVEY_Y_AXIS,GROUND_ELEVATION,SYSTEM_DATUM_OFFSET,RANGER_X_AXIS,RANGER_Y_AXIS,MAGNETIC_DECLINATION,CASING_TOP_DEPTH,WELL_HEAD_LONGITUDE,WELL_HEAD_LATITUDE,LOC_COUNTRY,LOC_STATE,P_LOC_CITY,LOC_COUNTY,DRILL_GEO_DES,DRILL_ENG_DES,PART_UNITS,WELL_HEAD_FLANGE,DRILL_ENG_DES_FILEID,DRILL_GEO_DES_FILEID,WELL_DRILL_CONDITION,BX_HEIGHT");
                    strSql.Append(") values (");
                    strSql.Append(":WELL_ID,:SITE_ID,:WELL_NAME,:WELL_LEGAL_NAME,:WELL_UWI,:WELL_STRUCT_UNIT_DESCRIPTION,:WELL_FIELD_UNIT_NAME,:WELL_STRUCT_UNIT_NAME,:BEOG_LOCATION,:STRUCTURAL_LOCATION,:TRAVERSE_LINE_LOCATION,:SURVEY_X_AXIS,:SURVEY_Y_AXIS,:GROUND_ELEVATION,:SYSTEM_DATUM_OFFSET,:RANGER_X_AXIS,:RANGER_Y_AXIS,:MAGNETIC_DECLINATION,:CASING_TOP_DEPTH,:WELL_HEAD_LONGITUDE,:WELL_HEAD_LATITUDE,:LOC_COUNTRY,:LOC_STATE,:P_LOC_CITY,:LOC_COUNTY,:DRILL_GEO_DES,:DRILL_ENG_DES,:PART_UNITS,:WELL_HEAD_FLANGE,:DRILL_ENG_DES_FILEID,:DRILL_GEO_DES_FILEID,:WELL_DRILL_CONDITION,:BX_HEIGHT");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":WELL_ID", "WELL_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SITE_ID", "SITE_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_NAME", "WELL_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_LEGAL_NAME", "WELL_LEGAL_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_UWI", "WELL_UWI", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_STRUCT_UNIT_DESCRIPTION", "WELL_STRUCT_UNIT_DESCRIPTION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_FIELD_UNIT_NAME", "WELL_FIELD_UNIT_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_STRUCT_UNIT_NAME", "WELL_STRUCT_UNIT_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BEOG_LOCATION", "BEOG_LOCATION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":STRUCTURAL_LOCATION", "STRUCTURAL_LOCATION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":TRAVERSE_LINE_LOCATION", "TRAVERSE_LINE_LOCATION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":SURVEY_X_AXIS", "SURVEY_X_AXIS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SURVEY_Y_AXIS", "SURVEY_Y_AXIS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":GROUND_ELEVATION", "GROUND_ELEVATION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SYSTEM_DATUM_OFFSET", "SYSTEM_DATUM_OFFSET", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RANGER_X_AXIS", "RANGER_X_AXIS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RANGER_Y_AXIS", "RANGER_Y_AXIS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAGNETIC_DECLINATION", "MAGNETIC_DECLINATION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CASING_TOP_DEPTH", "CASING_TOP_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WELL_HEAD_LONGITUDE", "WELL_HEAD_LONGITUDE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_HEAD_LATITUDE", "WELL_HEAD_LATITUDE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOC_COUNTRY", "LOC_COUNTRY", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOC_STATE", "LOC_STATE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_LOC_CITY", "P_LOC_CITY", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOC_COUNTY", "LOC_COUNTY", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DRILL_GEO_DES", "DRILL_GEO_DES", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":DRILL_ENG_DES", "DRILL_ENG_DES", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":PART_UNITS", "PART_UNITS", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_HEAD_FLANGE", "WELL_HEAD_FLANGE", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":DRILL_ENG_DES_FILEID", "DRILL_ENG_DES_FILEID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DRILL_GEO_DES_FILEID", "DRILL_GEO_DES_FILEID", OracleDbType.Decimal),

                        ServiceUtils.CreateOracleParameter(":WELL_DRILL_CONDITION", "WELL_DRILL_CONDITION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BX_HEIGHT", "BX_HEIGHT", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 井筒基本数据
                WriteLog("井筒基本数据");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_WELLBORE_BASIC where rownum<2 and WELL_ID=:WELL_ID", ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, well_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_WELLBORE_BASIC where WELL_ID=:WELL_ID", ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, well_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_WELLBORE_BASIC(");
                    strSql.Append("WELL_ID,WELLBORE_ID,WELLBORE_WUI,P_WELLBORE_ID,WELLBORE_NAME,PURPOSE,MAX_WELL_DEVIATION,MAX_WELL_DEVIATION_MD,DESIGN_VERTICAL_TVD,WELLBORE_PRODUCTION_DATE,DEFLECTION_POINT_MD,MD,VERTICAL_WELL_TVD,PLUGBACK_MD,PLUGBACK_TVD,TRUE_PLUGBACKTOTAL_DEPTH,DESIGN_HORIZON,DESIGN_MD,ACTUAL_HORIZON,BTM_X_COORDINATE,BTM_Y_COORDINATE,WELLBORE_ID_A1,WELLBORE_ID_A7");
                    strSql.Append(") values (");
                    strSql.Append(":WELL_ID,:WELLBORE_ID,:WELLBORE_WUI,:P_WELLBORE_ID,:WELLBORE_NAME,:PURPOSE,:MAX_WELL_DEVIATION,:MAX_WELL_DEVIATION_MD,:DESIGN_VERTICAL_TVD,:WELLBORE_PRODUCTION_DATE,:DEFLECTION_POINT_MD,:MD,:VERTICAL_WELL_TVD,:PLUGBACK_MD,:PLUGBACK_TVD,:TRUE_PLUGBACKTOTAL_DEPTH,:DESIGN_HORIZON,:DESIGN_MD,:ACTUAL_HORIZON,:BTM_X_COORDINATE,:BTM_Y_COORDINATE,:WELLBORE_ID_A1,:WELLBORE_ID_A7");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":WELL_ID", "WELL_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":WELLBORE_ID", "WELLBORE_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":WELLBORE_WUI", "WELLBORE_WUI", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_WELLBORE_ID", "P_WELLBORE_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":WELLBORE_NAME", "WELLBORE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PURPOSE", "PURPOSE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAX_WELL_DEVIATION", "MAX_WELL_DEVIATION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAX_WELL_DEVIATION_MD", "MAX_WELL_DEVIATION_MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DESIGN_VERTICAL_TVD", "DESIGN_VERTICAL_TVD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WELLBORE_PRODUCTION_DATE", "WELLBORE_PRODUCTION_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":DEFLECTION_POINT_MD", "DEFLECTION_POINT_MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MD", "MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":VERTICAL_WELL_TVD", "VERTICAL_WELL_TVD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PLUGBACK_MD", "PLUGBACK_MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PLUGBACK_TVD", "PLUGBACK_TVD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":TRUE_PLUGBACKTOTAL_DEPTH", "TRUE_PLUGBACKTOTAL_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DESIGN_HORIZON", "DESIGN_HORIZON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DESIGN_MD", "DESIGN_MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ACTUAL_HORIZON", "ACTUAL_HORIZON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BTM_X_COORDINATE", "BTM_X_COORDINATE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BTM_Y_COORDINATE", "BTM_Y_COORDINATE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WELLBORE_ID_A1", "WELLBORE_ID_A1", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELLBORE_ID_A7", "WELLBORE_ID_A7", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 测井作业基本信息
                WriteLog("测井作业基本信息");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_JOB_INFO where rownum<2 and DRILL_JOB_ID=:DRILL_JOB_ID", ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, drill_job_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_JOB_INFO where DRILL_JOB_ID=:DRILL_JOB_ID", ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, drill_job_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_JOB_INFO(");
                    strSql.Append("DRILL_JOB_ID,WELL_ID,WELL_ID_A7,WELL_ID_A1,WELL_JOB_NAME,ACTIVITY_NAME,FIELD,WELL_THE_MARKET,WELL_TYPE,WELL_SORT,TRUE_COMPLETION_FORMATION,COMPLETE_METHOD,JOB_PURPOSE,DRILL_UNIT,DESIGN_HORIZON,DESIGN_MD,WELLDONE_DEP,START_WELL_DATE,END_WELL_DATE,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":DRILL_JOB_ID,:WELL_ID,:WELL_ID_A7,:WELL_ID_A1,:WELL_JOB_NAME,:ACTIVITY_NAME,:FIELD,:WELL_THE_MARKET,:WELL_TYPE,:WELL_SORT,:TRUE_COMPLETION_FORMATION,:COMPLETE_METHOD,:JOB_PURPOSE,:DRILL_UNIT,:DESIGN_HORIZON,:DESIGN_MD,:WELLDONE_DEP,:START_WELL_DATE,:END_WELL_DATE,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", "DRILL_JOB_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_ID", "WELL_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":WELL_ID_A7", "WELL_ID_A7", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_ID_A1", "WELL_ID_A1", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", "WELL_JOB_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ACTIVITY_NAME", "ACTIVITY_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FIELD", "FIELD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_THE_MARKET", "WELL_THE_MARKET", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_TYPE", "WELL_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_SORT", "WELL_SORT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":TRUE_COMPLETION_FORMATION", "TRUE_COMPLETION_FORMATION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":COMPLETE_METHOD", "COMPLETE_METHOD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PURPOSE", "JOB_PURPOSE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DRILL_UNIT", "DRILL_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DESIGN_HORIZON", "DESIGN_HORIZON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DESIGN_MD", "DESIGN_MD", OracleDbType.Decimal),

                        ServiceUtils.CreateOracleParameter(":WELLDONE_DEP", "WELLDONE_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":START_WELL_DATE", "START_WELL_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":END_WELL_DATE", "END_WELL_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 测井任务通知单
                WriteLog("测井任务通知单");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_TASK where rownum<2 and REQUISITION_CD=:REQUISITION_CD", ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, requisition_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_TASK where REQUISITION_CD=:REQUISITION_CD", ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, requisition_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_TASK(");
                    strSql.Append("REQUISITION_CD,REQUISITION_TYPE,PREDICTED_LOGGING_DATE,PREDICTED_LOGGING_INTERVAL,PREDICTED_LOGGING_ITEMS_ID,DEPARTMENT_REQUISITION,PERSON_EQUISITION,REC_NOTICE_TIME,RECIPIENT_REQUISITION,COMPLETE_MAN,TREATMENT_DATE_REQUISITION,MARKET_IDENTIFICATION,MARKET_CLASSIFY,VERIFIER,BRIEF_DESCRIPTION_CONTENT,REQUISITION_SCANNING,NOTE,DRILL_STATUSE,WELL_ID,DRILL_JOB_ID,WELL_JOB_NAME,REQUISITION_SCANNING_FILEID");
                    strSql.Append(") values (");
                    strSql.Append(":REQUISITION_CD,:REQUISITION_TYPE,:PREDICTED_LOGGING_DATE,:PREDICTED_LOGGING_INTERVAL,:PREDICTED_LOGGING_ITEMS_ID,:DEPARTMENT_REQUISITION,:PERSON_EQUISITION,:REC_NOTICE_TIME,:RECIPIENT_REQUISITION,:COMPLETE_MAN,:TREATMENT_DATE_REQUISITION,:MARKET_IDENTIFICATION,:MARKET_CLASSIFY,:VERIFIER,:BRIEF_DESCRIPTION_CONTENT,:REQUISITION_SCANNING,:NOTE,:DRILL_STATUSE,:WELL_ID,:DRILL_JOB_ID,:WELL_JOB_NAME,:REQUISITION_SCANNING_FILEID");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_TYPE", "REQUISITION_TYPE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_DATE", "PREDICTED_LOGGING_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_INTERVAL", "PREDICTED_LOGGING_INTERVAL", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_ITEMS_ID", "PREDICTED_LOGGING_ITEMS_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DEPARTMENT_REQUISITION", "DEPARTMENT_REQUISITION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PERSON_EQUISITION", "PERSON_EQUISITION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REC_NOTICE_TIME", "REC_NOTICE_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":RECIPIENT_REQUISITION", "RECIPIENT_REQUISITION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":COMPLETE_MAN", "COMPLETE_MAN", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":TREATMENT_DATE_REQUISITION", "TREATMENT_DATE_REQUISITION", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":MARKET_IDENTIFICATION", "MARKET_IDENTIFICATION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MARKET_CLASSIFY", "MARKET_CLASSIFY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":VERIFIER", "VERIFIER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BRIEF_DESCRIPTION_CONTENT", "BRIEF_DESCRIPTION_CONTENT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_SCANNING", "REQUISITION_SCANNING", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":DRILL_STATUSE", "DRILL_STATUSE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_ID", "WELL_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", "DRILL_JOB_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_JOB_NAME", "WELL_JOB_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_SCANNING_FILEID", "REQUISITION_SCANNING_FILEID", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 计划任务书
                WriteLog("计划任务书");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_OPS_PLAN where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_OPS_PLAN where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_OPS_PLAN(");
                    strSql.Append("REQUISITION_CD,JOB_PLAN_CD,RECEIVED_INFORM_TIME,REQUIREMENTS_TIME,JOB_ID,JOB_LAYER,LOG_TYPE,LOG_MODE,PREPARE_PERSON,VERIFIER,APPROVER,PREPARE_DATE,PLAN_CONTENT_SCANNING,NOTE,LOG_TEAM_ID,PRELOGGING_INTERVAL,PLAN_CONTENT_SCANNING_FILEID");
                    strSql.Append(") values (");
                    strSql.Append(":REQUISITION_CD,:JOB_PLAN_CD,:RECEIVED_INFORM_TIME,:REQUIREMENTS_TIME,:JOB_ID,:JOB_LAYER,:LOG_TYPE,:LOG_MODE,:PREPARE_PERSON,:VERIFIER,:APPROVER,:PREPARE_DATE,:PLAN_CONTENT_SCANNING,:NOTE,:LOG_TEAM_ID,:PRELOGGING_INTERVAL,:PLAN_CONTENT_SCANNING_FILEID");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RECEIVED_INFORM_TIME", "RECEIVED_INFORM_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":REQUIREMENTS_TIME", "REQUIREMENTS_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":JOB_ID", "JOB_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":JOB_LAYER", "JOB_LAYER", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_TYPE", "LOG_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_MODE", "LOG_MODE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PREPARE_PERSON", "PREPARE_PERSON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":VERIFIER", "VERIFIER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":APPROVER", "APPROVER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PREPARE_DATE", "PREPARE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":PLAN_CONTENT_SCANNING", "PLAN_CONTENT_SCANNING", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_TEAM_ID", "LOG_TEAM_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PRELOGGING_INTERVAL", "PRELOGGING_INTERVAL", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PLAN_CONTENT_SCANNING_FILEID", "PLAN_CONTENT_SCANNING_FILEID", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 解释处理作业
                WriteLog("解释处理作业");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_PROCESS where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_PROCESS where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_PROCESS(");
                    strSql.Append("PROCESS_ID,LOG_DATA_ID,WELL_ID,DRILL_JOB_ID,PROCESS_NAME,PROCESS_CODE,PROCESS_START_DATE,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":PROCESS_ID,:LOG_DATA_ID,:WELL_ID,:DRILL_JOB_ID,:PROCESS_NAME,:PROCESS_CODE,:PROCESS_START_DATE,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_DATA_ID", "LOG_DATA_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_ID", "WELL_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", "DRILL_JOB_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESS_NAME", "PROCESS_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESS_CODE", "PROCESS_CODE", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":PROCESS_START_DATE", "PROCESS_START_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 地层分层数据
                WriteLog("地层分层数据");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_BASE_STRATA_LAYER2 where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_BASE_STRATA_LAYER2 where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_COM_BASE_STRATA_LAYER2(");
                    strSql.Append("SEQ_NO,JOB_PLAN_CD,REQUISITION_CD,STRAT_UNIT_NAME,STRAT_UNIT_S_NAME,AGE_CODE,STRATUM_CODE,STRAT_RANK,PARENT_STRAT_RANK,ROCK_DESCRIBE,BOTTOM_DEPTH,VERTICAL_DEPTH,BOTTOM_HEIGHT,VERTICAL_THICKNESS,SLANT_THICNESS,RELATIONS,DATE_TYPE,P_SCHEME_DESC,ROW_STATE,CREATE_ORG,CREATE_USER,UPDATE_ORG,UPDATE_USER,CREATE_DATE,UPDATE_DATE,REMARK");
                    strSql.Append(") values (");
                    strSql.Append(":SEQ_NO,:JOB_PLAN_CD,:REQUISITION_CD,:STRAT_UNIT_NAME,:STRAT_UNIT_S_NAME,:AGE_CODE,:STRATUM_CODE,:STRAT_RANK,:PARENT_STRAT_RANK,:ROCK_DESCRIBE,:BOTTOM_DEPTH,:VERTICAL_DEPTH,:BOTTOM_HEIGHT,:VERTICAL_THICKNESS,:SLANT_THICNESS,:RELATIONS,:DATE_TYPE,:P_SCHEME_DESC,:ROW_STATE,:CREATE_ORG,:CREATE_USER,:UPDATE_ORG,:UPDATE_USER,:CREATE_DATE,:UPDATE_DATE,:REMARK");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                        ServiceUtils.CreateOracleParameter(":SEQ_NO", "SEQ_NO", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":STRAT_UNIT_NAME", "STRAT_UNIT_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":STRAT_UNIT_S_NAME", "STRAT_UNIT_S_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":AGE_CODE", "AGE_CODE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":STRATUM_CODE", "STRATUM_CODE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":STRAT_RANK", "STRAT_RANK", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PARENT_STRAT_RANK", "PARENT_STRAT_RANK", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ROCK_DESCRIBE", "ROCK_DESCRIBE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BOTTOM_DEPTH", "BOTTOM_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":VERTICAL_DEPTH", "VERTICAL_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BOTTOM_HEIGHT", "BOTTOM_HEIGHT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":VERTICAL_THICKNESS", "VERTICAL_THICKNESS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SLANT_THICNESS", "SLANT_THICNESS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RELATIONS", "RELATIONS", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATE_TYPE", "DATE_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_SCHEME_DESC", "P_SCHEME_DESC", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ROW_STATE", "ROW_STATE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CREATE_ORG", "CREATE_ORG", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CREATE_USER", "CREATE_USER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UPDATE_ORG", "UPDATE_ORG", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UPDATE_USER", "UPDATE_USER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CREATE_DATE", "CREATE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", "UPDATE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":REMARK", "REMARK", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 固井质量分段评价数据
                WriteLog("固井质量分段评价数据");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_LOG_CEMENT_EVAL_INF where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_LOG_CEMENT_EVALUATION_INF where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_LOG_CEMENT_EVAL_INF(");
                    strSql.Append("CEMENTID,PROCESS_ID,ST_DEP,EN_DEP,MAX_CBL,MIN_CBL,MEA_CBL,RESULT,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":CEMENTID,:PROCESS_ID,:ST_DEP,:EN_DEP,:MAX_CBL,:MIN_CBL,:MEA_CBL,:RESULT,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CEMENTID", "CEMENTID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ST_DEP", "ST_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":EN_DEP", "EN_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAX_CBL", "MAX_CBL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MIN_CBL", "MIN_CBL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MEA_CBL", "MEA_CBL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RESULT", "RESULT", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 综合成果数据
                /*
                WriteLog("综合成果数据");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_LOG_COM_CURVEDATA where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_LOG_COM_CURVEDATA where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_LOG_COM_CURVEDATA(");
                    strSql.Append("CURVEDATAID,PROCESS_ID,DEP,SP,GR,CAL,DEV,DAZ,AC,CNL,DEN,PE,RT,RXO,SGR,CGR,U,TH,K");
                    strSql.Append(") values (");
                    strSql.Append(":CURVEDATAID,:PROCESS_ID,:DEP,:SP,:GR,:CAL,:DEV,:DAZ,:AC,:CNL,:DEN,:PE,:RT,:RXO,:SGR,:CGR,:U,:TH,:K");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CURVEDATAID", "CURVEDATAID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DEP", "DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SP", "SP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":GR", "GR", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CAL", "CAL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DEV", "DEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DAZ", "DAZ", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":AC", "AC", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CNL", "CNL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DEN", "DEN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PE", "PE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RT", "RT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RXO", "RXO", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SGR", "SGR", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CGR", "CGR", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":U", "U", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":TH", "TH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":K", "K", OracleDbType.Decimal)
              );
                }
                 */
                #endregion
                #region 小队现场提交数据
                WriteLog("小队现场提交数据");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_LOG_DEV_AZI where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_LOG_DEV_AZI where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_LOG_DEV_AZI(");
                    strSql.Append("DEV_ID,PROCESS_ID,DEP,INCLNATION,AZIMUTH,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":DEV_ID,:PROCESS_ID,:DEP,:INCLNATION,:AZIMUTH,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":DEV_ID", "DEV_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DEP", "DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INCLNATION", "INCLNATION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":AZIMUTH", "AZIMUTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 分层表
                WriteLog("分层表");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_LOG_LAYER where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_LOG_LAYER where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_LOG_LAYER(");
                    strSql.Append("LAYERID,PROCESS_ID,FORMATION_NAME,ST_DEVIATED_DEP,EN_DEVIATED_DEP,ST_VERTICAL_DEP,EN_VERTICAL_DEP");
                    strSql.Append(") values (");
                    strSql.Append(":LAYERID,:PROCESS_ID,:FORMATION_NAME,:ST_DEVIATED_DEP,:EN_DEVIATED_DEP,:ST_VERTICAL_DEP,:EN_VERTICAL_DEP");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":LAYERID", "LAYERID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FORMATION_NAME", "FORMATION_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ST_DEVIATED_DEP", "ST_DEVIATED_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":EN_DEVIATED_DEP", "EN_DEVIATED_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ST_VERTICAL_DEP", "ST_VERTICAL_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":EN_VERTICAL_DEP", "EN_VERTICAL_DEP", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 解释成果表
                WriteLog("解释成果表");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_LOG_RESULT where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_LOG_RESULT where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_LOG_RESULT(");
                    strSql.Append("RESULTID,PROCESS_ID,LAY_ID,FORMATION_NAME,START_DEPTH,END_DEPTH,VALID_THICKNESS,EXPLAIN_CONCLUSION,POROSITY_MIN_VALUE,POROSITY_MAX_VALUE,WATER_SATURATION_MIN_VALUE,WATER_SATURATION_MAX_VALUE,PENETRATE_RATE,GR_MIN_VALUE,GR_MAX_VALUE,SOUNDWAVE_MIN_VALUE,SOUNDWAVE_MAX_VALUE,DENSITY_MIN_VALUE,DENSITY_MAX_VALUE,NEUTRON_MIN_VALUE,NEUTRON_MAX_VALUE,RESISITANCE_MIN_VALUE,RESISITANCE_MAX_VALUE,QRESISITANCE_MIN_VALUE,QRESISITANCE_MAX_VALUE,INDUCTION120_MIN_VALUE,INDUCTION120_MAX_VALUE,INDUCTION30_MIN_VALUE,INDUCTION30_MAX_VALUE,STANDBY_PARAM1,STANDBY_PARAM2,STANDBY_PARAM3,STANDBY_PARAM4,STANDBY_PARAM5,STANDBY_PARAM6,STANDBY_PARAM7,STANDBY_PARAM8,SP_MIN_VALUE,SP_MAX_VALUE,RT1_MIN_VALUE,RT1_MAX_VALUE,SH_MIN_VALUE,SH_MAX_VALUE,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":RESULTID,:PROCESS_ID,:LAY_ID,:FORMATION_NAME,:START_DEPTH,:END_DEPTH,:VALID_THICKNESS,:EXPLAIN_CONCLUSION,:POROSITY_MIN_VALUE,:POROSITY_MAX_VALUE,:WATER_SATURATION_MIN_VALUE,:WATER_SATURATION_MAX_VALUE,:PENETRATE_RATE,:GR_MIN_VALUE,:GR_MAX_VALUE,:SOUNDWAVE_MIN_VALUE,:SOUNDWAVE_MAX_VALUE,:DENSITY_MIN_VALUE,:DENSITY_MAX_VALUE,:NEUTRON_MIN_VALUE,:NEUTRON_MAX_VALUE,:RESISITANCE_MIN_VALUE,:RESISITANCE_MAX_VALUE,:QRESISITANCE_MIN_VALUE,:QRESISITANCE_MAX_VALUE,:INDUCTION120_MIN_VALUE,:INDUCTION120_MAX_VALUE,:INDUCTION30_MIN_VALUE,:INDUCTION30_MAX_VALUE,:STANDBY_PARAM1,:STANDBY_PARAM2,:STANDBY_PARAM3,:STANDBY_PARAM4,:STANDBY_PARAM5,:STANDBY_PARAM6,:STANDBY_PARAM7,:STANDBY_PARAM8,:SP_MIN_VALUE,:SP_MAX_VALUE,:RT1_MIN_VALUE,:RT1_MAX_VALUE,:SH_MIN_VALUE,:SH_MAX_VALUE,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":RESULTID", "RESULTID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LAY_ID", "LAY_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FORMATION_NAME", "FORMATION_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", "START_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", "END_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":VALID_THICKNESS", "VALID_THICKNESS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":EXPLAIN_CONCLUSION", "EXPLAIN_CONCLUSION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":POROSITY_MIN_VALUE", "POROSITY_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":POROSITY_MAX_VALUE", "POROSITY_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WATER_SATURATION_MIN_VALUE", "WATER_SATURATION_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WATER_SATURATION_MAX_VALUE", "WATER_SATURATION_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PENETRATE_RATE", "PENETRATE_RATE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":GR_MIN_VALUE", "GR_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":GR_MAX_VALUE", "GR_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SOUNDWAVE_MIN_VALUE", "SOUNDWAVE_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SOUNDWAVE_MAX_VALUE", "SOUNDWAVE_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DENSITY_MIN_VALUE", "DENSITY_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DENSITY_MAX_VALUE", "DENSITY_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NEUTRON_MIN_VALUE", "NEUTRON_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NEUTRON_MAX_VALUE", "NEUTRON_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RESISITANCE_MIN_VALUE", "RESISITANCE_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RESISITANCE_MAX_VALUE", "RESISITANCE_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":QRESISITANCE_MIN_VALUE", "QRESISITANCE_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":QRESISITANCE_MAX_VALUE", "QRESISITANCE_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INDUCTION120_MIN_VALUE", "INDUCTION120_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INDUCTION120_MAX_VALUE", "INDUCTION120_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INDUCTION30_MIN_VALUE", "INDUCTION30_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INDUCTION30_MAX_VALUE", "INDUCTION30_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STANDBY_PARAM1", "STANDBY_PARAM1", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STANDBY_PARAM2", "STANDBY_PARAM2", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STANDBY_PARAM3", "STANDBY_PARAM3", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STANDBY_PARAM4", "STANDBY_PARAM4", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STANDBY_PARAM5", "STANDBY_PARAM5", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STANDBY_PARAM6", "STANDBY_PARAM6", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STANDBY_PARAM7", "STANDBY_PARAM7", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STANDBY_PARAM8", "STANDBY_PARAM8", OracleDbType.Decimal),

                        ServiceUtils.CreateOracleParameter(":SP_MIN_VALUE", "SP_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SP_MAX_VALUE", "SP_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RT1_MIN_VALUE", "RT1_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RT1_MAX_VALUE", "RT1_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SH_MIN_VALUE", "SH_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SH_MAX_VALUE", "SH_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 井眼轨迹数据
                WriteLog("井眼轨迹数据");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_LOG_WELL_TRAJECTORY where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_LOG_WELL_TRAJECTORY where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_LOG_WELL_TRAJECTORY(");
                    strSql.Append("TRAJECTORYID,PROCESS_ID,MD,INCLNATION,AZIMUTH,E_ELEMENT,N_ELEMENT,TVD,ALL_MOV,P_XN_CLOSURE_AZIMUTH,P_XN_CLOSED_DISTANCE,DOG_LEG,NOTE,TYPE");
                    strSql.Append(") values (");
                    strSql.Append(":TRAJECTORYID,:PROCESS_ID,:MD,:INCLNATION,:AZIMUTH,:E_ELEMENT,:N_ELEMENT,:TVD,:ALL_MOV,:P_XN_CLOSURE_AZIMUTH,:P_XN_CLOSED_DISTANCE,:DOG_LEG,:NOTE,:TYPE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":TRAJECTORYID", "TRAJECTORYID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MD", "MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INCLNATION", "INCLNATION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":AZIMUTH", "AZIMUTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":E_ELEMENT", "E_ELEMENT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":N_ELEMENT", "N_ELEMENT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":TVD", "TVD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ALL_MOV", "ALL_MOV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_XN_CLOSURE_AZIMUTH", "P_XN_CLOSURE_AZIMUTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_XN_CLOSED_DISTANCE", "P_XN_CLOSED_DISTANCE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DOG_LEG", "DOG_LEG", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":TYPE", "TYPE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 井身结构数据
                WriteLog("井身结构数据");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_WELLSTRUCTURE_DATA where rownum<2 and WELL_ID=:WELL_ID", ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, well_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_WELLSTRUCTURE_DATA where WELL_ID=:WELL_ID", ServiceUtils.CreateOracleParameter(":WELL_ID", OracleDbType.Varchar2, well_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_WELLSTRUCTURE_DATA(");
                    strSql.Append("WELLSTRUCTURE_ID,WELL_ID,WELLBORE_ID,NO,CASING_NAME,MD,HORIZON,BORE_SIZE,CASING_OD,BOINGTOOL_BOTTOM_DEPTH,SETTING_DEPTH,CHOKE_COIL_DEPTH,STAGE_COLLAR1_DEPTH,STAGE_COLLAR2_DEPTH,FIRST_CEMENT_TOP,SECOND_CEMENT_TOP,THIRD_CEMENT_TOP,CEMENT_METHOD,ARTIFICIAL_WELL_BOTTOM_DEPTH");
                    strSql.Append(") values (");
                    strSql.Append(":WELLSTRUCTURE_ID,:WELL_ID,:WELLBORE_ID,:NO,:CASING_NAME,:MD,:HORIZON,:BORE_SIZE,:CASING_OD,:BOINGTOOL_BOTTOM_DEPTH,:SETTING_DEPTH,:CHOKE_COIL_DEPTH,:STAGE_COLLAR1_DEPTH,:STAGE_COLLAR2_DEPTH,:FIRST_CEMENT_TOP,:SECOND_CEMENT_TOP,:THIRD_CEMENT_TOP,:CEMENT_METHOD,:ARTIFICIAL_WELL_BOTTOM_DEPTH");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":WELLSTRUCTURE_ID", "WELLSTRUCTURE_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WELL_ID", "WELL_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":WELLBORE_ID", "WELLBORE_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":NO", "NO", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CASING_NAME", "CASING_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MD", "MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":HORIZON", "HORIZON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BORE_SIZE", "BORE_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CASING_OD", "CASING_OD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BOINGTOOL_BOTTOM_DEPTH", "BOINGTOOL_BOTTOM_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SETTING_DEPTH", "SETTING_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CHOKE_COIL_DEPTH", "CHOKE_COIL_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STAGE_COLLAR1_DEPTH", "STAGE_COLLAR1_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":STAGE_COLLAR2_DEPTH", "STAGE_COLLAR2_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":FIRST_CEMENT_TOP", "FIRST_CEMENT_TOP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SECOND_CEMENT_TOP", "SECOND_CEMENT_TOP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":THIRD_CEMENT_TOP", "THIRD_CEMENT_TOP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CEMENT_METHOD", "CEMENT_METHOD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ARTIFICIAL_WELL_BOTTOM_DEPTH", "ARTIFICIAL_WELL_BOTTOM_DEPTH", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 小队施工_基本信息
                WriteLog("小队施工_基本信息");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_BASE where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_BASE where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_BASE(");
                    strSql.Append("BASEID,JOB_PLAN_CD,JOB_PURPOSE,COLLABORATION_DEPARTMENT,CONSTRUCTION_ORG,RECEIVED_INFORM_TIME,P_XN_DRILL_DEPTH,P_XN_LOG_DEPTH,JOB_LAYER,JOB_PROFILE_SERIES,MAXIMUM_SLOPE,MAXIMUM_SLOPE_DEPTH,MUD_RESITIVITY,MUD_TEMERATURE,BOTTOM_TEMPERATURE,BOTTOM_DEP,WELLBORE_FLUID,START_DEPTH,END_DEPTH,IS_COMPLETE,LOG_TYPE,LOG_MODE,INSTRUMENT_COUNT,INSTRUMENT_SUCCESS,WAVE_SELECTION,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":BASEID,:JOB_PLAN_CD,:JOB_PURPOSE,:COLLABORATION_DEPARTMENT,:CONSTRUCTION_ORG,:RECEIVED_INFORM_TIME,:P_XN_DRILL_DEPTH,:P_XN_LOG_DEPTH,:JOB_LAYER,:JOB_PROFILE_SERIES,:MAXIMUM_SLOPE,:MAXIMUM_SLOPE_DEPTH,:MUD_RESITIVITY,:MUD_TEMERATURE,:BOTTOM_TEMPERATURE,:BOTTOM_DEP,:WELLBORE_FLUID,:START_DEPTH,:END_DEPTH,:IS_COMPLETE,:LOG_TYPE,:LOG_MODE,:INSTRUMENT_COUNT,:INSTRUMENT_SUCCESS,:WAVE_SELECTION,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":BASEID", "BASEID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PURPOSE", "JOB_PURPOSE", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":COLLABORATION_DEPARTMENT", "COLLABORATION_DEPARTMENT", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":CONSTRUCTION_ORG", "CONSTRUCTION_ORG", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":RECEIVED_INFORM_TIME", "RECEIVED_INFORM_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":P_XN_DRILL_DEPTH", "P_XN_DRILL_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_XN_LOG_DEPTH", "P_XN_LOG_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":JOB_LAYER", "JOB_LAYER", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PROFILE_SERIES", "JOB_PROFILE_SERIES", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":MAXIMUM_SLOPE", "MAXIMUM_SLOPE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAXIMUM_SLOPE_DEPTH", "MAXIMUM_SLOPE_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MUD_RESITIVITY", "MUD_RESITIVITY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MUD_TEMERATURE", "MUD_TEMERATURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BOTTOM_TEMPERATURE", "BOTTOM_TEMPERATURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BOTTOM_DEP", "BOTTOM_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WELLBORE_FLUID", "WELLBORE_FLUID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", "START_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", "END_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":IS_COMPLETE", "IS_COMPLETE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_TYPE", "LOG_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_MODE", "LOG_MODE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":INSTRUMENT_COUNT", "INSTRUMENT_COUNT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INSTRUMENT_SUCCESS", "INSTRUMENT_SUCCESS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WAVE_SELECTION", "WAVE_SELECTION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 井场班会
                WriteLog("井场班会");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_CLASS_MEET where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_CLASS_MEET where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_CLASS_MEET(");
                    strSql.Append("CLASS_MEET_ID,JOB_PLAN_CD,HOST,WRITER,MEETING_DATE,MEETING_TIME,PARTICIPANTS,SUMMARY_ITEM1,SUMMARY_ITEM2,SUMMARY_ITEM3,SUMMARY_ITEM6,SUMMARY_ITEM4,SUMMARY_ITEM5,NOTE,ATTACH_INFO,ATTACH_FILE");
                    strSql.Append(") values (");
                    strSql.Append(":CLASS_MEET_ID,:JOB_PLAN_CD,:HOST,:WRITER,:MEETING_DATE,:MEETING_TIME,:PARTICIPANTS,:SUMMARY_ITEM1,:SUMMARY_ITEM2,:SUMMARY_ITEM3,:SUMMARY_ITEM6,:SUMMARY_ITEM4,:SUMMARY_ITEM5,:NOTE,:ATTACH_INFO,:ATTACH_FILE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CLASS_MEET_ID", "CLASS_MEET_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":HOST", "HOST", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":WRITER", "WRITER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":MEETING_DATE", "MEETING_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":MEETING_TIME", "MEETING_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PARTICIPANTS", "PARTICIPANTS", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM1", "SUMMARY_ITEM1", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM2", "SUMMARY_ITEM2", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM3", "SUMMARY_ITEM3", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM6", "SUMMARY_ITEM6", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM4", "SUMMARY_ITEM4", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM5", "SUMMARY_ITEM5", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),

                        ServiceUtils.CreateOracleParameter(":ATTACH_INFO", "ATTACH_INFO", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ATTACH_FILE", "ATTACH_FILE", OracleDbType.Blob)
              );
                }
                #endregion
                #region 多方联席会
                WriteLog("多方联席会");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_CONTACT_WILL where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_CONTACT_WILL where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_CONTACT_WILL(");
                    strSql.Append("CONTACTID,JOB_PLAN_CD,A_PUNISH,RRSIDE_SITE_PUNISH,DRILL_CREW_CHIEF,LOG_TEAM_LEADER,LOG_OPERATOR__NAME,LOG_SAFE_NAME,LOG_LAND_LEADER,LOG_COMPANY_PUNISH,LOG_COMPANY_LEADER,MEETING_SUM,WRITER,MEETING_DATE,MEETING_TIME,NOTE,B_PUNISH,MUD_CREW_CHIEF,SOLP_CREW_CHIEF,LOG_INTERPRETATION");
                    strSql.Append(") values (");
                    strSql.Append(":CONTACTID,:JOB_PLAN_CD,:A_PUNISH,:RRSIDE_SITE_PUNISH,:DRILL_CREW_CHIEF,:LOG_TEAM_LEADER,:LOG_OPERATOR__NAME,:LOG_SAFE_NAME,:LOG_LAND_LEADER,:LOG_COMPANY_PUNISH,:LOG_COMPANY_LEADER,:MEETING_SUM,:WRITER,:MEETING_DATE,:MEETING_TIME,:NOTE,:B_PUNISH,:MUD_CREW_CHIEF,:SOLP_CREW_CHIEF,:LOG_INTERPRETATION");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CONTACTID", "CONTACTID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":A_PUNISH", "A_PUNISH", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":RRSIDE_SITE_PUNISH", "RRSIDE_SITE_PUNISH", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":DRILL_CREW_CHIEF", "DRILL_CREW_CHIEF", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":LOG_TEAM_LEADER", "LOG_TEAM_LEADER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":LOG_OPERATOR__NAME", "LOG_OPERATOR__NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_SAFE_NAME", "LOG_SAFE_NAME", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":LOG_LAND_LEADER", "LOG_LAND_LEADER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":LOG_COMPANY_PUNISH", "LOG_COMPANY_PUNISH", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":LOG_COMPANY_LEADER", "LOG_COMPANY_LEADER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":MEETING_SUM", "MEETING_SUM", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":WRITER", "WRITER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":MEETING_DATE", "MEETING_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":MEETING_TIME", "MEETING_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":B_PUNISH", "B_PUNISH", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":MUD_CREW_CHIEF", "MUD_CREW_CHIEF", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SOLP_CREW_CHIEF", "SOLP_CREW_CHIEF", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":LOG_INTERPRETATION", "LOG_INTERPRETATION", OracleDbType.Char)
              );
                }
                #endregion
                #region 小队施工_井下设备
                WriteLog("小队施工_井下设备");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_DOWNHOLE_EQUIP where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_DOWNHOLE_EQUIP where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_DOWNHOLE_EQUIP(");
                    strSql.Append("JOB_PLAN_CD,EQUIPID,DEVICEACCOUNT_NAME,DEVICEACCOUNT_ID,TEAM,WORKING_STATE,FAULT_DESCRIPTION,DOWN_WELL_SEQUENCE");
                    strSql.Append(") values (");
                    strSql.Append(":JOB_PLAN_CD,:EQUIPID,:DEVICEACCOUNT_NAME,:DEVICEACCOUNT_ID,:TEAM,:WORKING_STATE,:FAULT_DESCRIPTION,:DOWN_WELL_SEQUENCE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":EQUIPID", "EQUIPID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DEVICEACCOUNT_NAME", "DEVICEACCOUNT_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DEVICEACCOUNT_ID", "DEVICEACCOUNT_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":TEAM", "TEAM", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WORKING_STATE", "WORKING_STATE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FAULT_DESCRIPTION", "FAULT_DESCRIPTION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", "DOWN_WELL_SEQUENCE", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 钻井动态
                WriteLog("钻井动态");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_DRILL_STATE where rownum<2 and DRILL_JOB_ID=:DRILL_JOB_ID", ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, drill_job_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_DRILL_STATE where DRILL_JOB_ID=:DRILL_JOB_ID", ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, drill_job_id)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_DRILL_STATE(");
                    strSql.Append("DRILL_WELL_ID,DRILL_JOB_ID,LAST_WEEK_MD,CURRENT_WEEK_MD,CURRENT_LAYER,CREATE_DATE,CREATE_MAN,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":DRILL_WELL_ID,:DRILL_JOB_ID,:LAST_WEEK_MD,:CURRENT_WEEK_MD,:CURRENT_LAYER,:CREATE_DATE,:CREATE_MAN,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":DRILL_WELL_ID", "DRILL_WELL_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", "DRILL_JOB_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LAST_WEEK_MD", "LAST_WEEK_MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURRENT_WEEK_MD", "CURRENT_WEEK_MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURRENT_LAYER", "CURRENT_LAYER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":CREATE_DATE", "CREATE_DATE", OracleDbType.Date),
                        ServiceUtils.CreateOracleParameter(":CREATE_MAN", "CREATE_MAN", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 钻井地质设计预测项目
                WriteLog("钻井地质设计预测项目");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_GEO_DES_ITEM where rownum<2 and DRILL_JOB_ID=:DRILL_JOB_ID", ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, drill_job_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_GEO_DES_ITEM where DRILL_JOB_ID=:DRILL_JOB_ID", ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", OracleDbType.Varchar2, drill_job_id)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_GEO_DES_ITEM(");
                    strSql.Append("DRILL_WELL_ID,DRILL_JOB_ID,LOG_MD,LOG_INTERVAL,LOG_LAYER,LOG_ITEM,LOG_SCALE,CREATE_DATE,CREATE_MAN,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":DRILL_WELL_ID,:DRILL_JOB_ID,:LOG_MD,:LOG_INTERVAL,:LOG_LAYER,:LOG_ITEM,:LOG_SCALE,:CREATE_DATE,:CREATE_MAN,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":DRILL_WELL_ID", "DRILL_WELL_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", "DRILL_JOB_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_MD", "LOG_MD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOG_INTERVAL", "LOG_INTERVAL", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":LOG_LAYER", "LOG_LAYER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":LOG_ITEM", "LOG_ITEM", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_SCALE", "LOG_SCALE", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":CREATE_DATE", "CREATE_DATE", OracleDbType.Date),
                        ServiceUtils.CreateOracleParameter(":CREATE_MAN", "CREATE_MAN", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 测井项目
                WriteLog("测井项目");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_ITEMS where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_ITEMS where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_ITEMS(");
                    strSql.Append("JOB_PLAN_CD,DOWN_WELL_SEQUENCE,LOGGING_NAME,ITEM_FLAG,ST_DEP,EN_DEP,SCALE,RLEV,NOTE,LOG_ITEM_ID");
                    strSql.Append(") values (");
                    strSql.Append(":JOB_PLAN_CD,:DOWN_WELL_SEQUENCE,:LOGGING_NAME,:ITEM_FLAG,:ST_DEP,:EN_DEP,:SCALE,:RLEV,:NOTE,:LOG_ITEM_ID");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", "DOWN_WELL_SEQUENCE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOGGING_NAME", "LOGGING_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ITEM_FLAG", "ITEM_FLAG", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ST_DEP", "ST_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":EN_DEP", "EN_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SCALE", "SCALE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RLEV", "RLEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_ITEM_ID", "LOG_ITEM_ID", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 作业总结
                WriteLog("作业总结");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_JOB_SUMMAY where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_JOB_SUMMAY where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_JOB_SUMMAY(");
                    strSql.Append("JOB_SUMMARY_ID,JOB_PLAN_CD,MEETING_DATE,MEETING_TIME,HOST,WRITER,PARTICIPANTS,SUMMARY_ITEM1,SUMMARY_ITEM2,SUMMARY_ITEM3,SUMMARY_ITEM6,SUMMARY_ITEM4,SUMMARY_ITEM5,NOTE,ATTACH_INFO,ATTACH_FILE");
                    strSql.Append(") values (");
                    strSql.Append(":JOB_SUMMARY_ID,:JOB_PLAN_CD,:MEETING_DATE,:MEETING_TIME,:HOST,:WRITER,:PARTICIPANTS,:SUMMARY_ITEM1,:SUMMARY_ITEM2,:SUMMARY_ITEM3,:SUMMARY_ITEM6,:SUMMARY_ITEM4,:SUMMARY_ITEM5,:NOTE,:ATTACH_INFO,:ATTACH_FILE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":JOB_SUMMARY_ID", "JOB_SUMMARY_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MEETING_DATE", "MEETING_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":MEETING_TIME", "MEETING_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":HOST", "HOST", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":WRITER", "WRITER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":PARTICIPANTS", "PARTICIPANTS", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM1", "SUMMARY_ITEM1", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM2", "SUMMARY_ITEM2", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM3", "SUMMARY_ITEM3", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM6", "SUMMARY_ITEM6", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM4", "SUMMARY_ITEM4", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM5", "SUMMARY_ITEM5", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),

                        ServiceUtils.CreateOracleParameter(":ATTACH_INFO", "ATTACH_INFO", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ATTACH_FILE", "ATTACH_FILE", OracleDbType.Blob)
              );
                }
                #endregion
                #region 预测项目信息
                WriteLog("预测项目信息");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_dm_log_task a,CQY_DM_LOG_PREDICTED_ITEM b where A.PREDICTED_LOGGING_ITEMS_ID=B.PREDICTED_LOGGING_ITEMS_ID and ROWNUM<2 and a.requisition_cd=:REQUISITION_CD", ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, requisition_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select b.* from dm_log_task a,DM_LOG_PREDICTED_ITEM b where A.PREDICTED_LOGGING_ITEMS_ID=B.PREDICTED_LOGGING_ITEMS_ID and a.requisition_cd=:REQUISITION_CD", ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, requisition_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_PREDICTED_ITEM(");
                    strSql.Append("PREDICTED_LOGGING_ITEMS_ID,PREDICTED_LOGGING_NAME,PRE_ST_DEP,PRE_EN_DEP,PRE_SCALE,NOTE,LOG_ITEM_ID");
                    strSql.Append(") values (");
                    strSql.Append(":PREDICTED_LOGGING_ITEMS_ID,:PREDICTED_LOGGING_NAME,:PRE_ST_DEP,:PRE_EN_DEP,:PRE_SCALE,:NOTE,:LOG_ITEM_ID");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_ITEMS_ID", "PREDICTED_LOGGING_ITEMS_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PREDICTED_LOGGING_NAME", "PREDICTED_LOGGING_NAME", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":PRE_ST_DEP", "PRE_ST_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PRE_EN_DEP", "PRE_EN_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PRE_SCALE", "PRE_SCALE", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_ITEM_ID", "LOG_ITEM_ID", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 小队施工_放射源使用情况
                WriteLog("小队施工_放射源使用情况");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_RADIATION_STATUS where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_RADIATION_STATUS where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_RADIATION_STATUS(");
                    strSql.Append("JOB_PLAN_CD,RADIATION_NO,RADIATION_CD,ELEMENT,ACTIVE,LOAD_PERSON,UNLOAD_PERSON,UNDER_WELL_TIME,MAX_WORK_PRESSURE,MAX_WORK_TEMPERATURE,REPLACE_SOURCEPACKING,REPLACE_SOURCEPACKING_NUM,REPLACE_DATE,REPLACE_PLACE,REPLACE_PERSON,RADIATIONID,DOWN_WELL_SEQUENCE");
                    strSql.Append(") values (");
                    strSql.Append(":JOB_PLAN_CD,:RADIATION_NO,:RADIATION_CD,:ELEMENT,:ACTIVE,:LOAD_PERSON,:UNLOAD_PERSON,:UNDER_WELL_TIME,:MAX_WORK_PRESSURE,:MAX_WORK_TEMPERATURE,:REPLACE_SOURCEPACKING,:REPLACE_SOURCEPACKING_NUM,:REPLACE_DATE,:REPLACE_PLACE,:REPLACE_PERSON,:RADIATIONID,:DOWN_WELL_SEQUENCE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RADIATION_NO", "RADIATION_NO", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RADIATION_CD", "RADIATION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ELEMENT", "ELEMENT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ACTIVE", "ACTIVE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOAD_PERSON", "LOAD_PERSON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UNLOAD_PERSON", "UNLOAD_PERSON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UNDER_WELL_TIME", "UNDER_WELL_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAX_WORK_PRESSURE", "MAX_WORK_PRESSURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAX_WORK_TEMPERATURE", "MAX_WORK_TEMPERATURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":REPLACE_SOURCEPACKING", "REPLACE_SOURCEPACKING", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REPLACE_SOURCEPACKING_NUM", "REPLACE_SOURCEPACKING_NUM", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":REPLACE_DATE", "REPLACE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":REPLACE_PLACE", "REPLACE_PLACE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REPLACE_PERSON", "REPLACE_PERSON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RADIATIONID", "RADIATIONID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", "DOWN_WELL_SEQUENCE", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 解释作业数据源
                WriteLog("解释作业数据源");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_SOURCE_DATA where rownum<2 and LOG_DATA_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_SOURCE_DATA where LOG_DATA_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_SOURCE_DATA(");
                    strSql.Append("LOG_DATA_ID,JOB_PLAN_CD,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":PROCESS_ID,:JOB_PLAN_CD,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":PROCESS_ID", "LOG_DATA_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 三交会
                WriteLog("三交会");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_THREE_CROSS where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_THREE_CROSS where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_THREE_CROSS(");
                    strSql.Append("THREE_CROSS_ID,JOB_PLAN_CD,MEETING_DATE,MEETING_TIME,HOST,WRITER,PARTICIPANTS,SUMMARY_ITEM1,SUMMARY_ITEM2,SUMMARY_ITEM3,SUMMARY_ITEM6,SUMMARY_ITEM4,SUMMARY_ITEM5,NOTE,ATTACH_INFO,ATTACH_FILE");
                    strSql.Append(") values (");
                    strSql.Append(":THREE_CROSS_ID,:JOB_PLAN_CD,:MEETING_DATE,:MEETING_TIME,:HOST,:WRITER,:PARTICIPANTS,:SUMMARY_ITEM1,:SUMMARY_ITEM2,:SUMMARY_ITEM3,:SUMMARY_ITEM6,:SUMMARY_ITEM4,:SUMMARY_ITEM5,:NOTE,:ATTACH_INFO,:ATTACH_FILE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":THREE_CROSS_ID", "THREE_CROSS_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MEETING_DATE", "MEETING_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":MEETING_TIME", "MEETING_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":HOST", "HOST", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":WRITER", "WRITER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":PARTICIPANTS", "PARTICIPANTS", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM1", "SUMMARY_ITEM1", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM2", "SUMMARY_ITEM2", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM3", "SUMMARY_ITEM3", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM6", "SUMMARY_ITEM6", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM4", "SUMMARY_ITEM4", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":SUMMARY_ITEM5", "SUMMARY_ITEM5", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),

                        ServiceUtils.CreateOracleParameter(":ATTACH_INFO", "ATTACH_INFO", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ATTACH_FILE", "ATTACH_FILE", OracleDbType.Blob)
              );
                }
                #endregion
                #region 小队施工_地面设备
                WriteLog("小队施工_地面设备");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_UP_EQUIP where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_UP_EQUIP where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_UP_EQUIP(");
                    strSql.Append("REQUISITION_CD,JOB_PLAN_CD,TEAM_ORG_ID,LOG_SERIES_ID,WINCH_LICENCEPLATE,WINCHC_ABLE_LENGTH,LOGGINGTRUCK_LICENSEPLATE");
                    strSql.Append(") values (");
                    strSql.Append(":REQUISITION_CD,:JOB_PLAN_CD,:TEAM_ORG_ID,:LOG_SERIES_ID,:WINCH_LICENCEPLATE,:WINCHC_ABLE_LENGTH,:LOGGINGTRUCK_LICENSEPLATE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":TEAM_ORG_ID", "TEAM_ORG_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_SERIES_ID", "LOG_SERIES_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WINCH_LICENCEPLATE", "WINCH_LICENCEPLATE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WINCHC_ABLE_LENGTH", "WINCHC_ABLE_LENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOGGINGTRUCK_LICENSEPLATE", "LOGGINGTRUCK_LICENSEPLATE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 小队施工_时效
                WriteLog("小队施工_时效");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_WORK_ANING where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_WORK_ANING where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_WORK_ANING(");
                    strSql.Append("ANINGID,REQUISITION_CD,JOB_PLAN_CD,RECEIVED_INFORM_TIME,REQUIREMENTS_TIME,ARRIVE_TIME,RECEIVING_TIME,HAND_TIME,LEAVE_TIME,WAIT_TIME,LOG_START_TIME,LOG_END_TIME,LOG_TOTAL_TIME,LOST_TIME,WINCH_RUNNING_TIME,DEPARTURE_POINT_,UNILATERAL_DISTANCE,DEPARTURE_POINT_TIME,RETURN_POINT,RETURN_POINT_TIME");
                    strSql.Append(") values (");
                    strSql.Append(":ANINGID,:REQUISITION_CD,:JOB_PLAN_CD,:RECEIVED_INFORM_TIME,:REQUIREMENTS_TIME,:ARRIVE_TIME,:RECEIVING_TIME,:HAND_TIME,:LEAVE_TIME,:WAIT_TIME,:LOG_START_TIME,:LOG_END_TIME,:LOG_TOTAL_TIME,:LOST_TIME,:WINCH_RUNNING_TIME,:DEPARTURE_POINT_,:UNILATERAL_DISTANCE,:DEPARTURE_POINT_TIME,:RETURN_POINT,:RETURN_POINT_TIME");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":ANINGID", "ANINGID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RECEIVED_INFORM_TIME", "RECEIVED_INFORM_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":REQUIREMENTS_TIME", "REQUIREMENTS_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":ARRIVE_TIME", "ARRIVE_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":RECEIVING_TIME", "RECEIVING_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":HAND_TIME", "HAND_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":LEAVE_TIME", "LEAVE_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":WAIT_TIME", "WAIT_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOG_START_TIME", "LOG_START_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":LOG_END_TIME", "LOG_END_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":LOG_TOTAL_TIME", "LOG_TOTAL_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOST_TIME", "LOST_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WINCH_RUNNING_TIME", "WINCH_RUNNING_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DEPARTURE_POINT_", "DEPARTURE_POINT_", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UNILATERAL_DISTANCE", "UNILATERAL_DISTANCE", OracleDbType.Decimal),

                        ServiceUtils.CreateOracleParameter(":DEPARTURE_POINT_TIME", "DEPARTURE_POINT_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":RETURN_POINT", "RETURN_POINT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RETURN_POINT_TIME", "RETURN_POINT_TIME", OracleDbType.TimeStamp)
              );
                }
                #endregion
                #region 小队施工_作业明细
                WriteLog("小队施工_作业明细");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_WORK_DETAILS where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_WORK_DETAILS where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_WORK_DETAILS(");
                    strSql.Append("DETAILSID,JOB_PLAN_CD,BLOCK_NUNBER,LOG_ORIGIANL_DATA_ID,DOWN_WELL_SEQUENCE,IS_ADD,COMBINATION_NAME,START_TIME,END_TIME,WORK_HOURS,IF_SUCCESS,MEASURE_WELL_TO,MEASURE_WELL_FROM,WELL_SECTION,POWERSUPPLY_VOLTAGE,CABLEHEAD_VOLTAGE,\"CURRENT\",LARGEST_CABLE_TENSION,LARGEST_CABLEHEAD_TENSION,DATA_NAME,DATA_SIZE,DATA_BLOCK_NUM,NOTE,FILENAME,LOG_SERIES_NAME");
                    strSql.Append(") values (");
                    strSql.Append(":DETAILSID,:JOB_PLAN_CD,:BLOCK_NUNBER,:LOG_ORIGIANL_DATA_ID,:DOWN_WELL_SEQUENCE,:IS_ADD,:COMBINATION_NAME,:START_TIME,:END_TIME,:WORK_HOURS,:IF_SUCCESS,:MEASURE_WELL_TO,:MEASURE_WELL_FROM,:WELL_SECTION,:POWERSUPPLY_VOLTAGE,:CABLEHEAD_VOLTAGE,:CURRENT1,:LARGEST_CABLE_TENSION,:LARGEST_CABLEHEAD_TENSION,:DATA_NAME,:DATA_SIZE,:DATA_BLOCK_NUM,:NOTE,:FILENAME,:LOG_SERIES_NAME");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":DETAILSID", "DETAILSID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BLOCK_NUNBER", "BLOCK_NUNBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOG_ORIGIANL_DATA_ID", "LOG_ORIGIANL_DATA_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", "DOWN_WELL_SEQUENCE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":IS_ADD", "IS_ADD", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":COMBINATION_NAME", "COMBINATION_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":START_TIME", "START_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":END_TIME", "END_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":WORK_HOURS", "WORK_HOURS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":IF_SUCCESS", "IF_SUCCESS", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":MEASURE_WELL_TO", "MEASURE_WELL_TO", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MEASURE_WELL_FROM", "MEASURE_WELL_FROM", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WELL_SECTION", "WELL_SECTION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":POWERSUPPLY_VOLTAGE", "POWERSUPPLY_VOLTAGE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CABLEHEAD_VOLTAGE", "CABLEHEAD_VOLTAGE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURRENT1", "CURRENT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LARGEST_CABLE_TENSION", "LARGEST_CABLE_TENSION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LARGEST_CABLEHEAD_TENSION", "LARGEST_CABLEHEAD_TENSION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DATA_NAME", "DATA_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE", "DATA_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DATA_BLOCK_NUM", "DATA_BLOCK_NUM", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FILENAME", "FILENAME", OracleDbType.Clob),
                        ServiceUtils.CreateOracleParameter(":LOG_SERIES_NAME","LOG_SERIES_NAME",OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 小队施工_遇阻情况
                WriteLog("小队施工_遇阻情况");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_WORK_HOLDUP_DETAILS where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_WORK_HOLDUP_DETAILS where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_WORK_HOLDUP_DETAILS(");
                    strSql.Append("HOLDUPID,JOB_PLAN_CD,DOWN_WELL_SEQUENCE,HOLDUP_DATE,HOLDUP_MD,HOLDUP_TYPE,WB_OBSTRUCTION_ID,OBSTRUCTION_DESC,TOOL_DIAMETER");
                    strSql.Append(") values (");
                    strSql.Append(":HOLDUPID,:JOB_PLAN_CD,:DOWN_WELL_SEQUENCE,:HOLDUP_DATE,:HOLDUP_MD,:HOLDUP_TYPE,:WB_OBSTRUCTION_ID,:OBSTRUCTION_DESC,:TOOL_DIAMETER");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":HOLDUPID", "HOLDUPID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", "DOWN_WELL_SEQUENCE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":HOLDUP_DATE", "HOLDUP_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":HOLDUP_MD", "HOLDUP_MD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":HOLDUP_TYPE", "HOLDUP_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WB_OBSTRUCTION_ID", "WB_OBSTRUCTION_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":OBSTRUCTION_DESC", "OBSTRUCTION_DESC", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":TOOL_DIAMETER", "TOOL_DIAMETER", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 小队施工_人员
                WriteLog("小队施工_人员");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_DM_LOG_WORK_PERSONNEL where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from DM_LOG_WORK_PERSONNEL where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_DM_LOG_WORK_PERSONNEL(");
                    strSql.Append("PERSONNEL_ID,JOB_PLAN_CD,LOG_TEAM_LEADER,CONTACT_TELEPHONE,LOG_OPERATOR__NAME,SOURCE_OPERATOR_NAME,LOG_SUPERVISION_NAME,OTHER_CONSTRUCTION_PERSONS,FIELD_INSPECTOR,INDOOR_INSPECTOR,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":PERSONNEL_ID,:JOB_PLAN_CD,:LOG_TEAM_LEADER,:CONTACT_TELEPHONE,:LOG_OPERATOR__NAME,:SOURCE_OPERATOR_NAME,:LOG_SUPERVISION_NAME,:OTHER_CONSTRUCTION_PERSONS,:FIELD_INSPECTOR,:INDOOR_INSPECTOR,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":PERSONNEL_ID", "PERSONNEL_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_TEAM_LEADER", "LOG_TEAM_LEADER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CONTACT_TELEPHONE", "CONTACT_TELEPHONE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_OPERATOR__NAME", "LOG_OPERATOR__NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":SOURCE_OPERATOR_NAME", "SOURCE_OPERATOR_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_SUPERVISION_NAME", "LOG_SUPERVISION_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":OTHER_CONSTRUCTION_PERSONS", "OTHER_CONSTRUCTION_PERSONS", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FIELD_INSPECTOR", "FIELD_INSPECTOR", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":INDOOR_INSPECTOR", "INDOOR_INSPECTOR", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 钻头参数
                WriteLog("钻头参数");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_BIT_PROGRAM where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_BIT_PROGRAM where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_BIT_PROGRAM(");
                    strSql.Append("BITID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,BIT_SIZE,BIT_DEP,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":BITID,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:BIT_SIZE,:BIT_DEP,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":BITID", "BITID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", "UPDATE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":BIT_SIZE", "BIT_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BIT_DEP", "BIT_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 小队施工_套管参数
                WriteLog("小队施工_套管参数");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_CASIN where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_CASIN where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_CASIN(");
                    strSql.Append("CASINID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,CASING_NAME,CASING_TYPE,CASING_OUTSIZE,CASING_QUANTITY,CASING_THREAD_TYPE,CASING_GRADE,PIPE_THICKNESS,P_XN_CASING_LENGTH,ACCUMULATIVE_LENGTH,RUNNING_BOTTOM_DEPTH,SCREW_ON_TORQUE,CENTERING_DEVICE_POSITION,CENTERING_DEVICE_NO,CENTERING_DEVICE_TYPE,CENTERING_DEVICE_SIZE,BASETUBE_BORESIZE,BASETUBE_OUTSIZE,BASETUBE_UNITNUMBER,BASETUBE_TYPE,BASETUBE_SLOTWIDTH,BASETUBE_POREDIAMETER,SIEVETUBE_SLOTWIDTH,SIEVETUBE_SLOTLENGTH,PIPE_HEIGHT,CURRENT_LOG_FILL,CURRENT_LOG_PLUGBACK,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":CASINID,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:CASING_NAME,:CASING_TYPE,:CASING_OUTSIZE,:CASING_QUANTITY,:CASING_THREAD_TYPE,:CASING_GRADE,:PIPE_THICKNESS,:P_XN_CASING_LENGTH,:ACCUMULATIVE_LENGTH,:RUNNING_BOTTOM_DEPTH,:SCREW_ON_TORQUE,:CENTERING_DEVICE_POSITION,:CENTERING_DEVICE_NO,:CENTERING_DEVICE_TYPE,:CENTERING_DEVICE_SIZE,:BASETUBE_BORESIZE,:BASETUBE_OUTSIZE,:BASETUBE_UNITNUMBER,:BASETUBE_TYPE,:BASETUBE_SLOTWIDTH,:BASETUBE_POREDIAMETER,:SIEVETUBE_SLOTWIDTH,:SIEVETUBE_SLOTLENGTH,:PIPE_HEIGHT,:CURRENT_LOG_FILL,:CURRENT_LOG_PLUGBACK,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CASINID", "CASINID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", "UPDATE_DATE", OracleDbType.Date),
                        ServiceUtils.CreateOracleParameter(":CASING_NAME", "CASING_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CASING_TYPE", "CASING_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CASING_OUTSIZE", "CASING_OUTSIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CASING_QUANTITY", "CASING_QUANTITY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CASING_THREAD_TYPE", "CASING_THREAD_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CASING_GRADE", "CASING_GRADE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PIPE_THICKNESS", "PIPE_THICKNESS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_XN_CASING_LENGTH", "P_XN_CASING_LENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ACCUMULATIVE_LENGTH", "ACCUMULATIVE_LENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":RUNNING_BOTTOM_DEPTH", "RUNNING_BOTTOM_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SCREW_ON_TORQUE", "SCREW_ON_TORQUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_POSITION", "CENTERING_DEVICE_POSITION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_NO", "CENTERING_DEVICE_NO", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_TYPE", "CENTERING_DEVICE_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CENTERING_DEVICE_SIZE", "CENTERING_DEVICE_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BASETUBE_BORESIZE", "BASETUBE_BORESIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BASETUBE_OUTSIZE", "BASETUBE_OUTSIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BASETUBE_UNITNUMBER", "BASETUBE_UNITNUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BASETUBE_TYPE", "BASETUBE_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BASETUBE_SLOTWIDTH", "BASETUBE_SLOTWIDTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BASETUBE_POREDIAMETER", "BASETUBE_POREDIAMETER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SIEVETUBE_SLOTWIDTH", "SIEVETUBE_SLOTWIDTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SIEVETUBE_SLOTLENGTH", "SIEVETUBE_SLOTLENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PIPE_HEIGHT", "PIPE_HEIGHT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURRENT_LOG_FILL", "CURRENT_LOG_FILL", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURRENT_LOG_PLUGBACK", "CURRENT_LOG_PLUGBACK", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2)
              );
                }
                #endregion
                #region 小队施工-固井参数
                WriteLog("小队施工-固井参数");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_CEMENT where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_CEMENT where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_CEMENT(");
                    strSql.Append("CEMENTID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,CEMENT_PROPERTIES,CEMENT_DENSITY_MAX_VALUE,CEMENTED_QUANTITY,CASING_SHOE_DEPTH,CEMENT_PRE_TOP,CEMENT_HEIGHT,CEMENT_WELL_DATE,OPEN_WELL_DATE,DISTANCE_TUBING_AND_BUSHING,CASING_TOP_SPACING,CEMENT_TYPE,CEMENT_DENSITY_MIN_VALUE");
                    strSql.Append(") values (");
                    strSql.Append(":CEMENTID,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:CEMENT_PROPERTIES,:CEMENT_DENSITY_MAX_VALUE,:CEMENTED_QUANTITY,:CASING_SHOE_DEPTH,:CEMENT_PRE_TOP,:CEMENT_HEIGHT,:CEMENT_WELL_DATE,:OPEN_WELL_DATE,:DISTANCE_TUBING_AND_BUSHING,:CASING_TOP_SPACING,:CEMENT_TYPE,:CEMENT_DENSITY_MIN_VALUE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CEMENTID", "CEMENTID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", "UPDATE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":CEMENT_PROPERTIES", "CEMENT_PROPERTIES", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CEMENT_DENSITY_MAX_VALUE", "CEMENT_DENSITY_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CEMENTED_QUANTITY", "CEMENTED_QUANTITY", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CASING_SHOE_DEPTH", "CASING_SHOE_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CEMENT_PRE_TOP", "CEMENT_PRE_TOP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CEMENT_HEIGHT", "CEMENT_HEIGHT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CEMENT_WELL_DATE", "CEMENT_WELL_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":OPEN_WELL_DATE", "OPEN_WELL_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":DISTANCE_TUBING_AND_BUSHING", "DISTANCE_TUBING_AND_BUSHING", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CASING_TOP_SPACING", "CASING_TOP_SPACING", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CEMENT_TYPE", "CEMENT_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CEMENT_DENSITY_MIN_VALUE", "CEMENT_DENSITY_MIN_VALUE", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 小队施工_取心参数
                WriteLog("小队施工_取心参数");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_CORE where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_CORE where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_CORE(");
                    strSql.Append("COREID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,FORMATION_NAME,START_DEPTH,END_DEPTH,CORE_WORD_DESCRIPTION,CORE_GR_DATA,CORE_PICTURE_DESCRIPTION,CORE_RECOVERY");
                    strSql.Append(") values (");
                    strSql.Append(":COREID,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:FORMATION_NAME,:START_DEPTH,:END_DEPTH,:CORE_WORD_DESCRIPTION,:CORE_GR_DATA,:CORE_PICTURE_DESCRIPTION,:CORE_RECOVERY");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":COREID", "COREID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", "UPDATE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":FORMATION_NAME", "FORMATION_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", "START_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", "END_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CORE_WORD_DESCRIPTION", "CORE_WORD_DESCRIPTION", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":CORE_GR_DATA", "CORE_GR_DATA", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":CORE_PICTURE_DESCRIPTION", "CORE_PICTURE_DESCRIPTION", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":CORE_RECOVERY", "CORE_RECOVERY", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 解释处理_数据发布
                WriteLog("解释处理_数据发布");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_DATA_PUBLISH where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_DATA_PUBLISH where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_DATA_PUBLISH(");
                    strSql.Append("DATA_PUBLISH_ID,PROCESS_ID,INTERPRET_CENTER,LOG_START_TIME,LOG_TOTAL_TIME,LOST_TIME,TEAM_ORG_ID,LOG_SERIES_ID,PRO_START_TIME,P_TOTAL_TIME,P_PROCESS_SOFTWARE,PROCESSOR,INTERPRETER,P_SUPERVISOR,RESULT_MAP_TYPE,LOG_ORIGINALITY_DATA,LOG_INTERPRET_REPORT,LOG_INTERPRET_RESULT,NOTE,FILE_NUMBER,P_SCENE_RATING,P_INDOOR_RATING,ACCEPTANCE_WAY_NAME");
                    strSql.Append(") values (");
                    strSql.Append(":DATA_PUBLISH_ID,:PROCESS_ID,:INTERPRET_CENTER,:LOG_START_TIME,:LOG_TOTAL_TIME,:LOST_TIME,:TEAM_ORG_ID,:LOG_SERIES_ID,:PRO_START_TIME,:P_TOTAL_TIME,:P_PROCESS_SOFTWARE,:PROCESSOR,:INTERPRETER,:P_SUPERVISOR,:RESULT_MAP_TYPE,:LOG_ORIGINALITY_DATA,:LOG_INTERPRET_REPORT,:LOG_INTERPRET_RESULT,:NOTE,:FILE_NUMBER,:P_SCENE_RATING,:P_INDOOR_RATING,:ACCEPTANCE_WAY_NAME");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":DATA_PUBLISH_ID", "DATA_PUBLISH_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":INTERPRET_CENTER", "INTERPRET_CENTER", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":LOG_START_TIME", "LOG_START_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":LOG_TOTAL_TIME", "LOG_TOTAL_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOST_TIME", "LOST_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":TEAM_ORG_ID", "TEAM_ORG_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_SERIES_ID", "LOG_SERIES_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PRO_START_TIME", "PRO_START_TIME", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":P_TOTAL_TIME", "P_TOTAL_TIME", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_PROCESS_SOFTWARE", "P_PROCESS_SOFTWARE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESSOR", "PROCESSOR", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":INTERPRETER", "INTERPRETER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_SUPERVISOR", "P_SUPERVISOR", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RESULT_MAP_TYPE", "RESULT_MAP_TYPE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOG_ORIGINALITY_DATA", "LOG_ORIGINALITY_DATA", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOG_INTERPRET_REPORT", "LOG_INTERPRET_REPORT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":LOG_INTERPRET_RESULT", "LOG_INTERPRET_RESULT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FILE_NUMBER", "FILE_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_SCENE_RATING", "P_SCENE_RATING", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_INDOOR_RATING", "P_INDOOR_RATING", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":ACCEPTANCE_WAY_NAME","ACCEPTANCE_WAY_NAME",OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 解释处理曲线评级
                WriteLog("解释处理曲线评级");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_PROCESS_CURVERATE where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_PROCESSING_CURVERATING where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_PROCESS_CURVERATE(");
                    strSql.Append("PROCESS_ID,PROCESSING_ITEM_ID,SCALE,RLEV,SCENE_RATING,INDOOR_RATING,CURVE_ID,START_DEP,END_DEP,WHY");
                    strSql.Append(") values (");
                    strSql.Append(":PROCESS_ID,:PROCESSING_ITEM_ID,:SCALE,:RLEV,:SCENE_RATING,:INDOOR_RATING,:CURVE_ID,:START_DEP,:END_DEP,:WHY");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SCALE", "SCALE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RLEV", "RLEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SCENE_RATING", "SCENE_RATING", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":INDOOR_RATING", "INDOOR_RATING", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_ID", "CURVE_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":START_DEP", "START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":END_DEP", "END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WHY","WHY",OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 解释处理_图件
                WriteLog("解释处理_图件");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_PROCESS_MAP where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_PROCESS_MAP where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_PROCESS_MAP(");
                    strSql.Append("MAPID,PROCESS_ID,MAPS_CODING,MAP_START_DEP,MAP_END_DEP,MAP_SCALE,MAP_DATA_NAME,MAP_PDF_SIZE,MAP_PDF_DATA,P_PROCESS_SOFTWARE,MAP_OUT_DATE,MAP_PERSON,MAP_NUMBER,MAP_TEMPLATE_SIZE,MAP_TEMPLATE_DATA,MAP_VERIFIER,PROCESSING_ITEM_ID");
                    strSql.Append(") values (");
                    strSql.Append(":MAPID,:PROCESS_ID,:MAPS_CODING,:MAP_START_DEP,:MAP_END_DEP,:MAP_SCALE,:MAP_DATA_NAME,:MAP_PDF_SIZE,:MAP_PDF_DATA,:P_PROCESS_SOFTWARE,:MAP_OUT_DATE,:MAP_PERSON,:MAP_NUMBER,:MAP_TEMPLATE_SIZE,:MAP_TEMPLATE_DATA,:MAP_VERIFIER,:PROCESSING_ITEM_ID");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":MAPID", "MAPID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAPS_CODING", "MAPS_CODING", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_START_DEP", "MAP_START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_END_DEP", "MAP_END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_SCALE", "MAP_SCALE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_DATA_NAME", "MAP_DATA_NAME", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_PDF_SIZE", "MAP_PDF_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_PDF_DATA", "MAP_PDF_DATA", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":P_PROCESS_SOFTWARE", "P_PROCESS_SOFTWARE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_OUT_DATE", "MAP_OUT_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":MAP_PERSON", "MAP_PERSON", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":MAP_NUMBER", "MAP_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_TEMPLATE_SIZE", "MAP_TEMPLATE_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAP_TEMPLATE_DATA", "MAP_TEMPLATE_DATA", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":MAP_VERIFIER", "MAP_VERIFIER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 解释处理_处理项目曲线
                /*
                WriteLog("解释处理_处理项目曲线");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_PROCESSING_CURVE where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_PROCESSING_CURVE where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_PROCESSING_CURVE(");
                    strSql.Append("CURVEID,PROCESS_ID,PROCESSING_ITEM_ID,CURVE_NAME,CURVE_START_DEP,CURVE_END_DEP,CURVE_RLEV,CURVE_UNIT,CURVE_T_SAMPLE,CURVE_T_UNIT,CURVE_T_MAX_VALUE,CURVE_T_MIN_VALUE,CURVE_T_RELV,CURVE_DATA_TYPE,CURVE_DATA_LENGTH,DATA_PROPERTY,DATA_INFO,P_CURVESOFTWARE_NAME,DATA_STORAGE_WAY,DATA_SIZE,FILE_STORAGE_PATH,FILE_DATA,CURVE_NOTE,NOTE,FILEID,CURVE_CD");
                    strSql.Append(") values (");
                    strSql.Append(":CURVEID,:PROCESS_ID,:PROCESSING_ITEM_ID,:CURVE_NAME,:CURVE_START_DEP,:CURVE_END_DEP,:CURVE_RLEV,:CURVE_UNIT,:CURVE_T_SAMPLE,:CURVE_T_UNIT,:CURVE_T_MAX_VALUE,:CURVE_T_MIN_VALUE,:CURVE_T_RELV,:CURVE_DATA_TYPE,:CURVE_DATA_LENGTH,:DATA_PROPERTY,:DATA_INFO,:P_CURVESOFTWARE_NAME,:DATA_STORAGE_WAY,:DATA_SIZE,:FILE_STORAGE_PATH,:FILE_DATA,:CURVE_NOTE,:NOTE,:FILEID,:CURVE_CD");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CURVEID", "CURVEID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_NAME", "CURVE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_START_DEP", "CURVE_START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_END_DEP", "CURVE_END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_RLEV", "CURVE_RLEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_UNIT", "CURVE_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_SAMPLE", "CURVE_T_SAMPLE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_UNIT", "CURVE_T_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_MAX_VALUE", "CURVE_T_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_MIN_VALUE", "CURVE_T_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_RELV", "CURVE_T_RELV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_TYPE", "CURVE_DATA_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_LENGTH", "CURVE_DATA_LENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DATA_PROPERTY", "DATA_PROPERTY", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATA_INFO", "DATA_INFO", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_CURVESOFTWARE_NAME", "P_CURVESOFTWARE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATA_STORAGE_WAY", "DATA_STORAGE_WAY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE", "DATA_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":FILE_STORAGE_PATH", "FILE_STORAGE_PATH", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FILE_DATA", "FILE_DATA", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":CURVE_NOTE", "CURVE_NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FILEID", "FILEID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_CD", "CURVE_CD", OracleDbType.Varchar2)
              );
                }
                 */
                #endregion
                #region 解释处理_处理项目曲线_INDEX表
                WriteLog("解释处理_处理项目曲线");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_PROC_CURVE_INDEX where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_PROCESSING_CURVE_INDEX where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_PROC_CURVE_INDEX(");
                    strSql.Append("CURVEID,PROCESS_ID,PROCESSING_ITEM_ID,CURVE_NAME,CURVE_START_DEP,CURVE_END_DEP,CURVE_RLEV,CURVE_UNIT,CURVE_T_SAMPLE,CURVE_T_UNIT,CURVE_T_MAX_VALUE,CURVE_T_MIN_VALUE,CURVE_T_RELV,CURVE_DATA_TYPE,CURVE_DATA_LENGTH,DATA_PROPERTY,DATA_INFO,P_CURVESOFTWARE_NAME,DATA_STORAGE_WAY,DATA_SIZE,CURVE_NOTE,NOTE,CURVE_CD");
                    strSql.Append(") values (");
                    strSql.Append(":CURVEID,:PROCESS_ID,:PROCESSING_ITEM_ID,:CURVE_NAME,:CURVE_START_DEP,:CURVE_END_DEP,:CURVE_RLEV,:CURVE_UNIT,:CURVE_T_SAMPLE,:CURVE_T_UNIT,:CURVE_T_MAX_VALUE,:CURVE_T_MIN_VALUE,:CURVE_T_RELV,:CURVE_DATA_TYPE,:CURVE_DATA_LENGTH,:DATA_PROPERTY,:DATA_INFO,:P_CURVESOFTWARE_NAME,:DATA_STORAGE_WAY,:DATA_SIZE,:CURVE_NOTE,:NOTE,:CURVE_CD");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CURVEID", "CURVEID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_NAME", "CURVE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_START_DEP", "CURVE_START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_END_DEP", "CURVE_END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_RLEV", "CURVE_RLEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_UNIT", "CURVE_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_SAMPLE", "CURVE_T_SAMPLE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_UNIT", "CURVE_T_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_MAX_VALUE", "CURVE_T_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_MIN_VALUE", "CURVE_T_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_T_RELV", "CURVE_T_RELV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_TYPE", "CURVE_DATA_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_LENGTH", "CURVE_DATA_LENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DATA_PROPERTY", "DATA_PROPERTY", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATA_INFO", "DATA_INFO", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_CURVESOFTWARE_NAME", "P_CURVESOFTWARE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATA_STORAGE_WAY", "DATA_STORAGE_WAY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE", "DATA_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_NOTE", "CURVE_NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_CD", "CURVE_CD", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 解释处理_处理项目曲线_DATA表
                isExist = DbHelperOraYTHPT.Exists("SELECT COUNT(1) FROM CQY_PRO_LOG_PROC_CURVE_DATA A,CQY_PRO_LOG_PROC_CURVE_INDEX B WHERE ROWNUM<2 AND A.CURVEID=B.CURVEID and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("SELECT A.* FROM PRO_LOG_PROCESSING_CURVE_DATA A,PRO_LOG_PROCESSING_CURVE_INDEX B WHERE A.CURVEID=B.CURVEID and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_PROC_CURVE_DATA(");
                    strSql.Append("CURVEID,BLOCK_NUMBER,BLOCK_SIZE,CURVE_DATA");
                    strSql.Append(") values (");
                    strSql.Append(":CURVEID,:BLOCK_NUMBER,:BLOCK_SIZE,:CURVE_DATA");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CURVEID", "CURVEID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BLOCK_NUMBER", "BLOCK_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BLOCK_SIZE", "BLOCK_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA",null, OracleDbType.Blob)//屏蔽大字段
              );
                }
                #endregion
                #region 综合解释成果曲线_INDEX表
                WriteLog("综合解释成果曲线");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_LOG_COM_CURVE_INDEX where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from COM_LOG_COM_CURVE_INDEX where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_LOG_COM_CURVE_INDEX(");
                    strSql.Append("CURVEID,PROCESS_ID,PROCESSING_ITEM_ID,CURVE_NAME,CURVE_CD,CURVE_START_DEP,CURVE_END_DEP,CURVE_RLEV,CURVE_UNIT,CURVE_SAMPLE,CURVE_MAX_VALUE,CURVE_MIN_VALUE,CURVE_DATA_TYPE,CURVE_DATA_LENGTH,P_CURVESOFTWARE_NAME,DATA_SIZE,CURVE_NOTE,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":CURVEID,:PROCESS_ID,:PROCESSING_ITEM_ID,:CURVE_NAME,:CURVE_CD,:CURVE_START_DEP,:CURVE_END_DEP,:CURVE_RLEV,:CURVE_UNIT,:CURVE_SAMPLE,:CURVE_MAX_VALUE,:CURVE_MIN_VALUE,:CURVE_DATA_TYPE,:CURVE_DATA_LENGTH,:P_CURVESOFTWARE_NAME,:DATA_SIZE,:CURVE_NOTE,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CURVEID", "CURVEID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_NAME", "CURVE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_CD", "CURVE_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_START_DEP", "CURVE_START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_END_DEP", "CURVE_END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_RLEV", "CURVE_RLEV", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_UNIT", "CURVE_UNIT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_SAMPLE", "CURVE_SAMPLE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_MAX_VALUE", "CURVE_MAX_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_MIN_VALUE", "CURVE_MIN_VALUE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_TYPE", "CURVE_DATA_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA_LENGTH", "CURVE_DATA_LENGTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_CURVESOFTWARE_NAME", "P_CURVESOFTWARE_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE", "DATA_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_NOTE", "CURVE_NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 综合解释成果曲线_DATA表
                isExist = DbHelperOraYTHPT.Exists("SELECT COUNT(1) FROM CQY_COM_LOG_COM_CURVE_DATA A,CQY_COM_LOG_COM_CURVE_INDEX B WHERE ROWNUM<2 AND A.CURVEID=B.CURVEID and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("SELECT A.* FROM COM_LOG_COM_CURVE_DATA A,COM_LOG_COM_CURVE_INDEX B WHERE A.CURVEID=B.CURVEID and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_COM_LOG_COM_CURVE_DATA(");
                    strSql.Append("CURVEID,BLOCK_NUMBER,BLOCK_SIZE,CURVE_DATA");
                    strSql.Append(") values (");
                    strSql.Append(":CURVEID,:BLOCK_NUMBER,:BLOCK_SIZE,:CURVE_DATA");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":CURVEID", "CURVEID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BLOCK_NUMBER", "BLOCK_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BLOCK_SIZE", "BLOCK_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CURVE_DATA", "CURVE_DATA", OracleDbType.Blob)
              );
                }
                #endregion
                #region 解释处理_处理项目
                WriteLog("解释处理_处理项目 ");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_PROCESSING_ITEM where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_PROCESSING_ITEM where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_PROCESSING_ITEM(");
                    strSql.Append("ITEMID,PROCESS_ID,PROCESSING_ITEM_ID,P_CURVE_NUMBER,P_PROCESS_SOFTWARE,P_WELL_INTERVAL,DATA_NAME,PROCESSOR,P_START_DATE,SCALE,INTERPRETER,P_SUPERVISOR,NOTE,LOG_SERIES_NAME,LOG_DATA_FORMAT");
                    strSql.Append(") values (");
                    strSql.Append(":ITEMID,:PROCESS_ID,:PROCESSING_ITEM_ID,:P_CURVE_NUMBER,:P_PROCESS_SOFTWARE,:P_WELL_INTERVAL,:DATA_NAME,:PROCESSOR,:P_START_DATE,:SCALE,:INTERPRETER,:P_SUPERVISOR,:NOTE,:LOG_SERIES_NAME,:LOG_DATA_FORMAT");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":ITEMID", "ITEMID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_CURVE_NUMBER", "P_CURVE_NUMBER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":P_PROCESS_SOFTWARE", "P_PROCESS_SOFTWARE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_WELL_INTERVAL", "P_WELL_INTERVAL", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DATA_NAME", "DATA_NAME", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":PROCESSOR", "PROCESSOR", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_START_DATE", "P_START_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":SCALE", "SCALE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":INTERPRETER", "INTERPRETER", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_SUPERVISOR", "P_SUPERVISOR", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_SERIES_NAME","LOG_SERIES_NAME",OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_DATA_FORMAT","LOG_DATA_FORMAT",OracleDbType.Varchar2)
              );
                }
                #endregion
                #region 小队施工_生产参数
                WriteLog("小队施工_生产参数");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_PRODUCE where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_PRODUCE where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_PRODUCE(");
                    strSql.Append("WORKID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,OIL_SHOE_DEPTH,WELL_FLUID_TYPE,OIL_LINE_INNER_DIAMETER,BELLMOUTH_DEPTH,CHOKE,SUL_HYD,WELLHEAD_PRESS,WELLBOTTOM_PRESS,WELLHEAD_FLANGE_TYPE,WATER_PRO_PER_DAY,AIR_DAY_PRODUCTION,OIL_PRO_PER_DAY,CASING_PRESSURE,OIL_PRESSURE,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":WORKID,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:OIL_SHOE_DEPTH,:WELL_FLUID_TYPE,:OIL_LINE_INNER_DIAMETER,:BELLMOUTH_DEPTH,:CHOKE,:SUL_HYD,:WELLHEAD_PRESS,:WELLBOTTOM_PRESS,:WELLHEAD_FLANGE_TYPE,:WATER_PRO_PER_DAY,:AIR_DAY_PRODUCTION,:OIL_PRO_PER_DAY,:CASING_PRESSURE,:OIL_PRESSURE,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":WORKID", "WORKID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", "UPDATE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":OIL_SHOE_DEPTH", "OIL_SHOE_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WELL_FLUID_TYPE", "WELL_FLUID_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":OIL_LINE_INNER_DIAMETER", "OIL_LINE_INNER_DIAMETER", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BELLMOUTH_DEPTH", "BELLMOUTH_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CHOKE", "CHOKE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SUL_HYD", "SUL_HYD", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WELLHEAD_PRESS", "WELLHEAD_PRESS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WELLBOTTOM_PRESS", "WELLBOTTOM_PRESS", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":WELLHEAD_FLANGE_TYPE", "WELLHEAD_FLANGE_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":WATER_PRO_PER_DAY", "WATER_PRO_PER_DAY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":AIR_DAY_PRODUCTION", "AIR_DAY_PRODUCTION", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":OIL_PRO_PER_DAY", "OIL_PRO_PER_DAY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CASING_PRESSURE", "CASING_PRESSURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":OIL_PRESSURE", "OIL_PRESSURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2)
              );
                }
                #endregion
                #region 小队施工_测井监督
                WriteLog("小队施工_测井监督");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_PUNISH where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_PUNISH where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_PUNISH(");
                    strSql.Append("PUNISHID,JOB_PLAN_CD,REASON,PUNISH_DATE,PUNISH_SUGGESTION,IS_RECTIFY,REVISE_DATE,LOG_SUPERVISION_ID,NOTE,DOWN_WELL_SEQUENCE");
                    strSql.Append(") values (");
                    strSql.Append(":PUNISHID,:JOB_PLAN_CD,:REASON,:PUNISH_DATE,:PUNISH_SUGGESTION,:IS_RECTIFY,:REVISE_DATE,:LOG_SUPERVISION_ID,:NOTE,:DOWN_WELL_SEQUENCE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":PUNISHID", "PUNISHID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REASON", "REASON", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":PUNISH_DATE", "PUNISH_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":PUNISH_SUGGESTION", "PUNISH_SUGGESTION", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":IS_RECTIFY", "IS_RECTIFY", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":REVISE_DATE", "REVISE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":LOG_SUPERVISION_ID", "LOG_SUPERVISION_ID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", "DOWN_WELL_SEQUENCE", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 小队施工_现场快速解释基本信息
                WriteLog("小队施工_现场快速解释基本信息");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_RAPID_INFO where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_RAPID_INFO where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_RAPID_INFO(");
                    strSql.Append("REQUISITION_CD,DRILL_JOB_ID,JOB_PLAN_CD,LOG_TEAM,LOG_MODE,LOG_SERVER_ID,BOTTOM_DEP,BOTTOM_TEMPERATURE,MUD_RESITIVITY,MUD_TEMERATURE,SUBMIT_MAN,SUBMIT_DATE,NOTE,BOTTOM_PRESSURE,GROUND_TEMPERATURE");
                    strSql.Append(") values (");
                    strSql.Append(":REQUISITION_CD,:DRILL_JOB_ID,:JOB_PLAN_CD,:LOG_TEAM,:LOG_MODE,:LOG_SERVER_ID,:BOTTOM_DEP,:BOTTOM_TEMPERATURE,:MUD_RESITIVITY,:MUD_TEMERATURE,:SUBMIT_MAN,:SUBMIT_DATE,:NOTE,:BOTTOM_PRESSURE,:GROUND_TEMPERATURE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DRILL_JOB_ID", "DRILL_JOB_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_TEAM", "LOG_TEAM", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_MODE", "LOG_MODE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":LOG_SERVER_ID", "LOG_SERVER_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BOTTOM_DEP", "BOTTOM_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BOTTOM_TEMPERATURE", "BOTTOM_TEMPERATURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MUD_RESITIVITY", "MUD_RESITIVITY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MUD_TEMERATURE", "MUD_TEMERATURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SUBMIT_MAN", "SUBMIT_MAN", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":SUBMIT_DATE", "SUBMIT_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":BOTTOM_PRESSURE","BOTTOM_PRESSURE",OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":GROUND_TEMPERATURE", "GROUND_TEMPERATURE", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 小队施工_现场快速解释成果表
                WriteLog("小队施工_现场快速解释成果表");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_RAPID_RESULTS where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_RAPID_RESULTS where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_RAPID_RESULTS(");
                    strSql.Append("DATA_ID,REQUISITION_CD,JOB_PLAN_CD,RAPID_RESULTS_TYPE,START_DEP,END_DEP,DATA,DATA_SIZE,NOTE,FILENAME,FILEID");
                    strSql.Append(") values (");
                    strSql.Append(":DATA_ID,:REQUISITION_CD,:JOB_PLAN_CD,:RAPID_RESULTS_TYPE,:START_DEP,:END_DEP,:DATA,:DATA_SIZE,:NOTE,:FILENAME,:FILEID");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":DATA_ID", "DATA_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RAPID_RESULTS_TYPE", "RAPID_RESULTS_TYPE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":START_DEP", "START_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":END_DEP", "END_DEP", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DATA", "DATA", OracleDbType.Blob),
                        ServiceUtils.CreateOracleParameter(":DATA_SIZE", "DATA_SIZE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FILENAME", "FILENAME", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":FILEID", "FILEID", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 小队施工_远程指导
                WriteLog("小队施工_远程指导");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_REMOTE_DIRECT where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_REMOTE_DIRECT where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_REMOTE_DIRECT(");
                    strSql.Append("DIRECTID,JOB_PLAN_CD,DOWN_WELL_SEQUENCE,EXPERTISE,EXPERT_LEADER,DIRECT_DATE,DIRECT_MEANS,EXPERTISE_RECEIVE_MAN,EXPERTISE_EFFECT,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":DIRECTID,:JOB_PLAN_CD,:DOWN_WELL_SEQUENCE,:EXPERTISE,:EXPERT_LEADER,:DIRECT_DATE,:DIRECT_MEANS,:EXPERTISE_RECEIVE_MAN,:EXPERTISE_EFFECT,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":DIRECTID", "DIRECTID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":DOWN_WELL_SEQUENCE", "DOWN_WELL_SEQUENCE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":EXPERTISE", "EXPERTISE", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":EXPERT_LEADER", "EXPERT_LEADER", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":DIRECT_DATE", "DIRECT_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":DIRECT_MEANS", "DIRECT_MEANS", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":EXPERTISE_RECEIVE_MAN", "EXPERTISE_RECEIVE_MAN", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":EXPERTISE_EFFECT", "EXPERTISE_EFFECT", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2)
              );
                }
                #endregion
                #region 小队施工_钻井液参数
                WriteLog("小队施工_钻井液参数");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_SLOP where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_SLOP where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_SLOP(");
                    strSql.Append("MUDID,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,JTLT,SLOP_PROPERTIES,P_XN_MUD_DENSITY,SLOP_PERSENT,SLOP_PH,DRILL_FLU_VISC,SLOP_TEMPERATURE,SLOP_RESISTIVITY,DRILLING_FLUID_SALINITY,MUD_FILTRATE_SALINITY,CAKE_DENSITY,API_FILTER_LOSS");
                    strSql.Append(") values (");
                    strSql.Append(":MUDID,:JOB_PLAN_CD,:REQUISITION_CD,:UPDATE_DATE,:JTLT,:SLOP_PROPERTIES,:P_XN_MUD_DENSITY,:SLOP_PERSENT,:SLOP_PH,:DRILL_FLU_VISC,:SLOP_TEMPERATURE,:SLOP_RESISTIVITY,:DRILLING_FLUID_SALINITY,:MUD_FILTRATE_SALINITY,:CAKE_DENSITY,:API_FILTER_LOSS");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":MUDID", "MUDID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":UPDATE_DATE", "UPDATE_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":JTLT", "JTLT", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":SLOP_PROPERTIES", "SLOP_PROPERTIES", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":P_XN_MUD_DENSITY", "P_XN_MUD_DENSITY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SLOP_PERSENT", "SLOP_PERSENT", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SLOP_PH", "SLOP_PH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DRILL_FLU_VISC", "DRILL_FLU_VISC", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SLOP_TEMPERATURE", "SLOP_TEMPERATURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":SLOP_RESISTIVITY", "SLOP_RESISTIVITY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":DRILLING_FLUID_SALINITY", "DRILLING_FLUID_SALINITY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MUD_FILTRATE_SALINITY", "MUD_FILTRATE_SALINITY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CAKE_DENSITY", "CAKE_DENSITY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":API_FILTER_LOSS", "API_FILTER_LOSS", OracleDbType.Decimal)
              );
                }
                #endregion
                #region 井试油参数
                WriteLog("井试油参数");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_PRO_LOG_TESTOIL where rownum<2 and JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from PRO_LOG_TESTOIL where JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                    strSql.Append("insert into CQY_PRO_LOG_TESTOIL(");
                    strSql.Append("TESTOILID,JOB_PLAN_CD,REQUISITION_CD,TEST_DATE,LAYER_NAME,START_DEPTH,END_DEPTH,OUTPUT_OIL_EVERY_DAY,OUTPUT_WATER_EVERY_DAY,OUTPUT_GAS_EVERY_DAY,HYDROSULFIDE,FORMATION_PRESSURE,CHOKE,OIL_PRESSURE,CASING_PRESSURE,TEST_CONCLUSION,DETAILED_DESCRIPTION_TEST,NOTE");
                    strSql.Append(") values (");
                    strSql.Append(":TESTOILID,:JOB_PLAN_CD,:REQUISITION_CD,:TEST_DATE,:LAYER_NAME,:START_DEPTH,:END_DEPTH,:OUTPUT_OIL_EVERY_DAY,:OUTPUT_WATER_EVERY_DAY,:OUTPUT_GAS_EVERY_DAY,:HYDROSULFIDE,:FORMATION_PRESSURE,:CHOKE,:OIL_PRESSURE,:CASING_PRESSURE,:TEST_CONCLUSION,:DETAILED_DESCRIPTION_TEST,:NOTE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":TESTOILID", "TESTOILID", OracleDbType.Char),
                        ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", "JOB_PLAN_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":REQUISITION_CD", "REQUISITION_CD", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":TEST_DATE", "TEST_DATE", OracleDbType.TimeStamp),
                        ServiceUtils.CreateOracleParameter(":LAYER_NAME", "LAYER_NAME", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":START_DEPTH", "START_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":END_DEPTH", "END_DEPTH", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":OUTPUT_OIL_EVERY_DAY", "OUTPUT_OIL_EVERY_DAY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":OUTPUT_WATER_EVERY_DAY", "OUTPUT_WATER_EVERY_DAY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":OUTPUT_GAS_EVERY_DAY", "OUTPUT_GAS_EVERY_DAY", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":HYDROSULFIDE", "HYDROSULFIDE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":FORMATION_PRESSURE", "FORMATION_PRESSURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CHOKE", "CHOKE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":OIL_PRESSURE", "OIL_PRESSURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":CASING_PRESSURE", "CASING_PRESSURE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":TEST_CONCLUSION", "TEST_CONCLUSION", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":DETAILED_DESCRIPTION_TEST", "DETAILED_DESCRIPTION_TEST", OracleDbType.NVarchar2),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.NVarchar2)
              );
                }
                #endregion
                #region 页岩气解释成果数据
                WriteLog("页岩气解释成果数据");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_COM_LOG_RESULT a, CQY_WL_ACH_SHALE_GAS_INTER b where A.RESULTID=B.RESULTID and rownum <2 and A.PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select b.* from COM_LOG_RESULT a,WL_ACH_SHALE_GAS_INTER b where A.RESULTID=B.RESULTID and A.PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_WL_ACH_SHALE_GAS_INTER(");
                    strSql.Append("SHALE_GAS_INTER_ID,RESULTID,ORGANIC_CARBON_CONTEN_MIN,ORGANIC_CARBON_CONTEN_MAX,ALL_GAS_CONTENT_MIN,ALL_GAS_CONTENT_MAX,ADSORBED_GAS_CONTENT_MIN,ADSORBED_GAS_CONTENT_MAX,FREE_GAS_CONTENT_MIN,FREE_GAS_CONTENT_MAX,BRITTLENESS_INDEX_MIN,BRITTLENESS_INDEX_MAX,KEROGEN_MIN,KEROGEN_MAX,MAIN_STRESS_MAX_MIN,MAIN_STRESS_MAX_MAX,MAIN_STRESS_MAX_DIRECT_MIN,MAIN_STRESS_MAX_DIRECT_MAX,MAIN_STRESS_MIN_MIN,MAIN_STRESS_MIN_MAX,GAP_PRESS_MIN,GAP_PRESS_MAX,NOTE,INDUCTION_DEEP_MIN,INDUCTION_DEEP_MAX,INDUCTION_MEDIUM_MIN,INDUCTION_MEDIUM_MAX,INDUCTION_SHALLOW_MIN,INDUCTION_SHALLOW_MAX");
                    strSql.Append(") values (");
                    strSql.Append(":SHALE_GAS_INTER_ID,:RESULTID,:ORGANIC_CARBON_CONTEN_MIN,:ORGANIC_CARBON_CONTEN_MAX,:ALL_GAS_CONTENT_MIN,:ALL_GAS_CONTENT_MAX,:ADSORBED_GAS_CONTENT_MIN,:ADSORBED_GAS_CONTENT_MAX,:FREE_GAS_CONTENT_MIN,:FREE_GAS_CONTENT_MAX,:BRITTLENESS_INDEX_MIN,:BRITTLENESS_INDEX_MAX,:KEROGEN_MIN,:KEROGEN_MAX,:MAIN_STRESS_MAX_MIN,:MAIN_STRESS_MAX_MAX,:MAIN_STRESS_MAX_DIRECT_MIN,:MAIN_STRESS_MAX_DIRECT_MAX,:MAIN_STRESS_MIN_MIN,:MAIN_STRESS_MIN_MAX,:GAP_PRESS_MIN,:GAP_PRESS_MAX,:NOTE,:INDUCTION_DEEP_MIN,:INDUCTION_DEEP_MAX,:INDUCTION_MEDIUM_MIN,:INDUCTION_MEDIUM_MAX,:INDUCTION_SHALLOW_MIN,:INDUCTION_SHALLOW_MAX");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":SHALE_GAS_INTER_ID", "SHALE_GAS_INTER_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":RESULTID", "RESULTID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ORGANIC_CARBON_CONTEN_MIN", "ORGANIC_CARBON_CONTEN_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ORGANIC_CARBON_CONTEN_MAX", "ORGANIC_CARBON_CONTEN_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ALL_GAS_CONTENT_MIN", "ALL_GAS_CONTENT_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ALL_GAS_CONTENT_MAX", "ALL_GAS_CONTENT_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ADSORBED_GAS_CONTENT_MIN", "ADSORBED_GAS_CONTENT_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":ADSORBED_GAS_CONTENT_MAX", "ADSORBED_GAS_CONTENT_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":FREE_GAS_CONTENT_MIN", "FREE_GAS_CONTENT_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":FREE_GAS_CONTENT_MAX", "FREE_GAS_CONTENT_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BRITTLENESS_INDEX_MIN", "BRITTLENESS_INDEX_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":BRITTLENESS_INDEX_MAX", "BRITTLENESS_INDEX_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":KEROGEN_MIN", "KEROGEN_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":KEROGEN_MAX", "KEROGEN_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAIN_STRESS_MAX_MIN", "MAIN_STRESS_MAX_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAIN_STRESS_MAX_MAX", "MAIN_STRESS_MAX_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAIN_STRESS_MAX_DIRECT_MIN", "MAIN_STRESS_MAX_DIRECT_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAIN_STRESS_MAX_DIRECT_MAX", "MAIN_STRESS_MAX_DIRECT_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAIN_STRESS_MIN_MIN", "MAIN_STRESS_MIN_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":MAIN_STRESS_MIN_MAX", "MAIN_STRESS_MIN_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":GAP_PRESS_MIN", "GAP_PRESS_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":GAP_PRESS_MAX", "GAP_PRESS_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":NOTE", "NOTE", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":INDUCTION_DEEP_MIN", "INDUCTION_DEEP_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INDUCTION_DEEP_MAX", "INDUCTION_DEEP_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INDUCTION_MEDIUM_MIN", "INDUCTION_MEDIUM_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INDUCTION_MEDIUM_MAX", "INDUCTION_MEDIUM_MAX", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INDUCTION_SHALLOW_MIN", "INDUCTION_SHALLOW_MIN", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":INDUCTION_SHALLOW_MAX", "INDUCTION_SHALLOW_MAX", OracleDbType.Decimal)

              );
                }
                #endregion
                #region 上传信息表
                /*
                WriteLog("上传信息表");
                try
                {
                    var maxid = DbHelperOraYTHPT.GetSingle("select max(uploadid) from CQY_SYS_UPLOAD");
                    if (maxid != null)
                    {
                        strSql.Clear();
                        dt = DbHelperOra.Query("select * from SYS_UPLOAD where uploadid >" + (decimal)maxid).Tables[0];
                        strSql.Append("insert into CQY_SYS_UPLOAD(");
                        strSql.Append("UPLOADID,SHA1,MD5,LENGTH,PATHID,PATHMAIN");
                        strSql.Append(") values (");
                        strSql.Append(":UPLOADID,:SHA1,:MD5,:LENGTH,:PATHID,:PATHMAIN");
                        strSql.Append(") ");
                        DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                               ServiceUtils.CreateOracleParameter(":UPLOADID", "UPLOADID", OracleDbType.Decimal),
                            ServiceUtils.CreateOracleParameter(":SHA1", "SHA1", OracleDbType.Char),
                            ServiceUtils.CreateOracleParameter(":MD5", "MD5", OracleDbType.Varchar2),
                            ServiceUtils.CreateOracleParameter(":LENGTH", "LENGTH", OracleDbType.Decimal),
                            ServiceUtils.CreateOracleParameter(":PATHID", "PATHID", OracleDbType.Decimal),
                            ServiceUtils.CreateOracleParameter(":PATHMAIN", "PATHMAIN", OracleDbType.Varchar2)
                  );
                    }
                    maxid = DbHelperOraYTHPT.GetSingle("select max(fileid) from CQY_SYS_FILE_UPLOAD");
                    if (maxid != null)
                    {
                        strSql.Clear();
                        dt = DbHelperOra.Query("select * from SYS_FILE_UPLOAD where FILEID>" + (decimal)maxid).Tables[0];
                        strSql.Append("insert into CQY_SYS_FILE_UPLOAD(");
                        strSql.Append("FILEID,UPLOADID,FILENAME,PATHID");
                        strSql.Append(") values (");
                        strSql.Append(":FILEID,:UPLOADID,:FILENAME,:PATHID");
                        strSql.Append(") ");
                        DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                               ServiceUtils.CreateOracleParameter(":FILEID", "FILEID", OracleDbType.Decimal),
                            ServiceUtils.CreateOracleParameter(":UPLOADID", "UPLOADID", OracleDbType.Decimal),
                            ServiceUtils.CreateOracleParameter(":FILENAME", "FILENAME", OracleDbType.NVarchar2),
                            ServiceUtils.CreateOracleParameter(":PATHID", "PATHID", OracleDbType.Decimal)
                  );
                    }
                }
                catch { };
                 */
                #endregion
                #region 井、通知单文件信息入库
                List<int> fileList = new List<int>();
                dt = DbHelperOra.Query("select DRILL_ENG_DES_FILEID,DRILL_GEO_DES_FILEID from com_well_basic where well_id=:well_id",
                   ServiceUtils.CreateOracleParameter(":well_id", OracleDbType.Char, well_id)).Tables[0];
                var eng_filedid = Convert.ToString(dt.Rows[0][0]);
                var geo_fileid = Convert.ToString(dt.Rows[0][1]);
                if (!string.IsNullOrWhiteSpace(eng_filedid)) fileList.Add(Convert.ToInt32(eng_filedid));
                if (!string.IsNullOrWhiteSpace(geo_fileid)) fileList.Add(Convert.ToInt32(geo_fileid));

                var req_scan_id = DbHelperOra.GetSingle("select REQUISITION_SCANNING_FILEID from dm_log_task where REQUISITION_CD=:REQUISITION_CD",
                    ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, requisition_cd));
                if (!string.IsNullOrWhiteSpace(Convert.ToString(req_scan_id))) fileList.Add(Convert.ToInt32(req_scan_id));

                var plan_dt = DbHelperOra.Query("select PLAN_CONTENT_SCANNING_FILEID from DM_LOG_OPS_PLAN  where REQUISITION_CD='" + requisition_cd + "'").Tables[0];
                var result_dt = DbHelperOra.Query("select FILEID  from PRO_LOG_RAPID_RESULTS  where REQUISITION_CD='" + requisition_cd + "'").Tables[0];
                var original_dt = DbHelperOra.Query("select FILEID  from PRO_LOG_RAPID_ORIGINAL_DATA where JOB_PLAN_CD like '" + requisition_cd + "%'").Tables[0];
                if (plan_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < plan_dt.Rows.Count; i++)
                    {
                        var fileid = Convert.ToString(plan_dt.Rows[i][0]);
                        if (!string.IsNullOrWhiteSpace(fileid)) fileList.Add(Convert.ToInt32(fileid));
                    }
                }
                if (result_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < result_dt.Rows.Count; i++)
                    {
                        var fileid = Convert.ToString(result_dt.Rows[i][0]);
                        if (!string.IsNullOrWhiteSpace(fileid)) fileList.Add(Convert.ToInt32(fileid));
                    }
                }
                if (original_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < original_dt.Rows.Count; i++)
                    {
                        var fileid = Convert.ToString(original_dt.Rows[i][0]);
                        if (!string.IsNullOrWhiteSpace(fileid)) fileList.Add(Convert.ToInt32(fileid));
                    }
                }

                for (int i = 0; i < fileList.Count; i++)
                {
                    dt = DbHelperOra.Query("select a.*,b.fileid,b.filename,b.pathid as pathid_1 from sys_upload a,sys_file_upload b where b.fileid=:FILEID and b.uploadid=a.uploadid",
                        ServiceUtils.CreateOracleParameter(":FILEID", OracleDbType.Decimal, fileList[i])).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        DbHelperOraYTHPT.ExecuteSql("DECLARE v_cnt NUMBER;v_cnt1 NUMBER;BEGIN SELECT COUNT(1) INTO v_cnt FROM CQY_SYS_UPLOAD WHERE rownum<2 and uploadid=:UPLOADID;SELECT COUNT(1) INTO v_cnt1 FROM CQY_SYS_FILE_UPLOAD WHERE rownum<2 and fileid=:FILEID;if v_cnt=0 then INSERT INTO CQY_SYS_UPLOAD(UPLOADID,SHA1,MD5,LENGTH,PATHID,PATHMAIN) values (:UPLOADID,:SHA1,:MD5,:LENGTH,:PATHID,:PATHMAIN);else UPDATE CQY_SYS_UPLOAD SET sha1=:SHA1,md5=:MD5,length=:LENGTH,pathid=:PATHID,pathmain=:PATHMAIN where uploadid=:UPLOADID;end if;if v_cnt1=0 then INSERT INTO CQY_SYS_FILE_UPLOAD(FILEID,UPLOADID,FILENAME,PATHID) values(:FILEID,:UPLOADID,:FILENAME,:PATHID_1);else UPDATE CQY_SYS_FILE_UPLOAD SET uploadid=:UPLOADID,filename=:FILENAME,pathid=:PATHID_1 where fileid=:FILEID;end if;end;",
                                ServiceUtils.CreateOracleParameter(":SHA1", OracleDbType.Char, dt.Rows[0].Field<string>("sha1")),
                                ServiceUtils.CreateOracleParameter(":MD5", OracleDbType.Varchar2, dt.Rows[0].Field<string>("md5")),
                                ServiceUtils.CreateOracleParameter(":LENGTH", OracleDbType.Decimal, dt.Rows[0].Field<decimal>("length")),
                                ServiceUtils.CreateOracleParameter(":FILEID", OracleDbType.Decimal, fileList[i]),
                                ServiceUtils.CreateOracleParameter(":UPLOADID", OracleDbType.Decimal, dt.Rows[0].Field<decimal>("uploadid")),
                                ServiceUtils.CreateOracleParameter(":PATHID", OracleDbType.Decimal, dt.Rows[0].Field<decimal>("pathid")),
                                ServiceUtils.CreateOracleParameter(":PATHMAIN", OracleDbType.Varchar2, dt.Rows[0].Field<string>("pathmain")),
                                ServiceUtils.CreateOracleParameter(":FILENAME", OracleDbType.NVarchar2, dt.Rows[0].Field<string>("filename")),
                                ServiceUtils.CreateOracleParameter(":PATHID_1", OracleDbType.Decimal, dt.Rows[0].Field<decimal?>("pathid_1")));
                    }
                }
                #endregion
                #region 解释处理作业文件信息
                dt = DbHelperOra.Query("select a.*,b.fileid,b.filename,b.pathid as pathid_1 from sys_upload a,sys_file_upload b,sys_processing_uploadfile c where c.process_id=:process_id and c.fileid=b.fileid and b.uploadid=a.uploadid",
                    ServiceUtils.CreateOracleParameter(":process_id", OracleDbType.Varchar2, process_id)).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DbHelperOraYTHPT.ExecuteSql("DECLARE v_cnt NUMBER;v_cnt1 NUMBER;BEGIN SELECT COUNT(1) INTO v_cnt FROM CQY_SYS_UPLOAD WHERE rownum<2 and uploadid=:UPLOADID;SELECT COUNT(1) INTO v_cnt1 FROM CQY_SYS_FILE_UPLOAD WHERE rownum<2 and fileid=:FILEID;if v_cnt=0 then INSERT INTO CQY_SYS_UPLOAD(UPLOADID,SHA1,MD5,LENGTH,PATHID,PATHMAIN) values (:UPLOADID,:SHA1,:MD5,:LENGTH,:PATHID,:PATHMAIN);else UPDATE CQY_SYS_UPLOAD SET sha1=:SHA1,md5=:MD5,length=:LENGTH,pathid=:PATHID,pathmain=:PATHMAIN where uploadid=:UPLOADID;end if;if v_cnt1=0 then INSERT INTO CQY_SYS_FILE_UPLOAD(FILEID,UPLOADID,FILENAME,PATHID) values(:FILEID,:UPLOADID,:FILENAME,:PATHID_1);else UPDATE CQY_SYS_FILE_UPLOAD SET filename=:FILENAME,pathid=:PATHID_1 where fileid=:FILEID;end if;end;",
                            ServiceUtils.CreateOracleParameter(":SHA1", OracleDbType.Char, dt.Rows[i].Field<string>("sha1")),
                            ServiceUtils.CreateOracleParameter(":MD5", OracleDbType.Varchar2, dt.Rows[i].Field<string>("md5")),
                            ServiceUtils.CreateOracleParameter(":LENGTH", OracleDbType.Decimal, dt.Rows[i].Field<decimal>("length")),
                            ServiceUtils.CreateOracleParameter(":FILEID", OracleDbType.Decimal, dt.Rows[i].Field<decimal>("fileid")),
                            ServiceUtils.CreateOracleParameter(":UPLOADID", OracleDbType.Decimal, dt.Rows[i].Field<decimal>("uploadid")),
                            ServiceUtils.CreateOracleParameter(":PATHID", OracleDbType.Decimal, dt.Rows[i].Field<decimal>("pathid")),
                            ServiceUtils.CreateOracleParameter(":PATHMAIN", OracleDbType.Varchar2, dt.Rows[i].Field<string>("pathmain")),
                            ServiceUtils.CreateOracleParameter(":FILENAME", OracleDbType.NVarchar2, dt.Rows[i].Field<string>("filename")),
                            ServiceUtils.CreateOracleParameter(":PATHID_1", OracleDbType.Decimal, dt.Rows[i].Field<decimal>("pathid_1")));
                }
                #endregion
                #region 解释处理归档文件信息
                WriteLog("解释处理归档文件信息");
                isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_SYS_PROCESSING_UPLOADFILE where rownum<2 and PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id));
                if (!isExist)
                {
                    strSql.Clear();
                    dt = DbHelperOra.Query("select * from SYS_PROCESSING_UPLOADFILE where PROCESS_ID=:PROCESS_ID", ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, process_id)).Tables[0];
                    strSql.Append("insert into CQY_SYS_PROCESSING_UPLOADFILE(");
                    strSql.Append("PROCESS_ID,EXTENSION,FILETYPE,FILEID,PROCESSING_ITEM_ID,PROCESS_UPLOAD_ID,PROCESS_UPLOAD_FILE");
                    strSql.Append(") values (");
                    strSql.Append(":PROCESS_ID,:EXTENSION,:FILETYPE,:FILEID,:PROCESSING_ITEM_ID,:PROCESS_UPLOAD_ID,:PROCESS_UPLOAD_FILE");
                    strSql.Append(") ");
                    DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                           ServiceUtils.CreateOracleParameter(":PROCESS_ID", "PROCESS_ID", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":EXTENSION", "EXTENSION", OracleDbType.Varchar2),
                        ServiceUtils.CreateOracleParameter(":FILETYPE", "FILETYPE", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":FILEID", "FILEID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESSING_ITEM_ID", "PROCESSING_ITEM_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_UPLOAD_ID", "PROCESS_UPLOAD_ID", OracleDbType.Decimal),
                        ServiceUtils.CreateOracleParameter(":PROCESS_UPLOAD_FILE", "PROCESS_UPLOAD_FILE", OracleDbType.Blob)
              );
                }
                #endregion
                #region 流程状态
                pushWorkFlowData(0, requisition_cd);
                pushWorkFlowData(1, job_plan_cd);
                pushWorkFlowData(4, job_plan_cd);
                pushWorkFlowData(5, process_id);
                #endregion
                DbHelperOra.ExecuteSql("Declare v_cnt NUMBER;BEGIN SELECT count(1) into v_cnt from DI_LOG_PUSH where rownum<2 and NAME=:NAME;if v_cnt>0 then UPDATE DI_LOG_PUSH SET OPERATOR=:OPERATOR,LOG_TIME=systimestamp where NAME=:NAME;else INSERT INTO DI_LOG_PUSH(ID,NAME,OPERATOR) VALUES(DATA_PUSH_ID_SEQ.NEXTVAL,:NAME,:OPERATOR);end if;END;",
                        new OracleParameter[]{
                           ServiceUtils.CreateOracleParameter(":NAME",OracleDbType.Varchar2,process_name),
                           ServiceUtils.CreateOracleParameter(":OPERATOR",OracleDbType.Varchar2,user_name)
                       });

                DateTime EndTime = DateTime.Now;
                var TotalTime = ExecDateDiff(StartTime, EndTime).ToString();
                WriteLine("数据推送完成！" + "耗时：" + TotalTime);
            }
            catch (Exception ex)
            {
                WriteLine("推送数据出错：" + ex.Message);
                var inEx = ex.InnerException;
                while (inEx != null)
                {
                    logBuilder.Append(ex.InnerException.Message + "\r\n");
                    inEx = inEx.InnerException;
                }
            }
            finally
            {
                try
                {
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile//DataPushLog//YTHPT" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", logBuilder.ToString());
                }
                catch (Exception e) { }
                isRunning = false;
            }
        }

        private void pushWorkFlowData(int type, string obj_id)
        {
            StringBuilder strSql = new StringBuilder();
            bool isExist = DbHelperOraYTHPT.Exists("select count(1) from CQY_SYS_WORK_FLOW_NOW where rownum<2 and OBJ_ID=:OBJ_ID and flow_type=" + type, ServiceUtils.CreateOracleParameter(":OBJ_ID", OracleDbType.Varchar2, obj_id));
            if (!isExist)
            {
                var dt = DbHelperOra.Query("select * from SYS_WORK_FLOW_NOW where OBJ_ID=:OBJ_ID and flow_type=" + type, ServiceUtils.CreateOracleParameter(":OBJ_ID", OracleDbType.Varchar2, obj_id)).Tables[0];
                strSql.Append("insert into CQY_SYS_WORK_FLOW_NOW(");
                strSql.Append("OBJ_ID,SOURCE_LOGINNAME,TARGET_LOGINNAME,FLOW_TYPE,FLOW_STATE,FLOW_TIME,FLOW_ID,MESSAGE");
                strSql.Append(") values (");
                strSql.Append(":OBJ_ID,:SOURCE_LOGINNAME,:TARGET_LOGINNAME,:FLOW_TYPE,:FLOW_STATE,:FLOW_TIME,:FLOW_ID,:MESSAGE");
                strSql.Append(") ");
                DbHelperOraYTHPT.InsertDataTable(strSql.ToString(), dt,
                                       ServiceUtils.CreateOracleParameter(":OBJ_ID", "OBJ_ID", OracleDbType.Varchar2),
                    ServiceUtils.CreateOracleParameter(":SOURCE_LOGINNAME", "SOURCE_LOGINNAME", OracleDbType.Varchar2),
                    ServiceUtils.CreateOracleParameter(":TARGET_LOGINNAME", "TARGET_LOGINNAME", OracleDbType.Varchar2),
                    ServiceUtils.CreateOracleParameter(":FLOW_TYPE", "FLOW_TYPE", OracleDbType.Decimal),
                    ServiceUtils.CreateOracleParameter(":FLOW_STATE", "FLOW_STATE", OracleDbType.Decimal),
                    ServiceUtils.CreateOracleParameter(":FLOW_TIME", "FLOW_TIME", OracleDbType.TimeStamp),
                    ServiceUtils.CreateOracleParameter(":FLOW_ID", "FLOW_ID", OracleDbType.Decimal),
                    ServiceUtils.CreateOracleParameter(":MESSAGE", "MESSAGE", OracleDbType.NVarchar2)
          );
            }
        }

        private class Sequence
        {
            private long counter = 0;
            private long baseValue = DateTime.Now.Ticks / 100000 * 100000;
            public string NextValue
            {
                get
                {
                    counter += 1;
                    return (baseValue + counter).ToString();
                }
            }
        }

        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="dateBegin">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>返回(秒)单位，比如: 0:00:21</returns>
        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            return ts3.ToString("g").Substring(0, 7);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}