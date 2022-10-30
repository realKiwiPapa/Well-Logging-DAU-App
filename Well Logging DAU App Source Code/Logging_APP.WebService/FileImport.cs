using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Text;

using Maticsoft.DBUtility;

namespace Logging_App.WebService
{
    public static class FileImport
    {
        private static object GetField<T>(this DataRow dr, string columnName)
        {
            if (dr.Table.Columns.Contains(columnName) && dr[columnName] != null)
            {
                string v = dr[columnName].ToString();
                if (typeof(T) == typeof(decimal))
                {
                    decimal d;
                    if (decimal.TryParse(v, out d))
                        return d;
                    else
                        return DBNull.Value;
                }
                return v;
            }
            else
                return DBNull.Value;
        }


        private static object lockLayer = new Object();
        /// <summary>
        /// 地层分层数据导入
        /// </summary>
        public static bool ImprotLayer(string job_plan_cd, string requistion_cd, DataTable data)
        {
            if (data == null || data.Rows.Count < 1 || !data.Columns.Contains("底部井深") || !data.Columns.Contains("地层单位名称")) return false;
            lock (lockLayer)
            {
                StringBuilder strSql = new StringBuilder();
                List<OracleParameter> parameters = new List<OracleParameter>();
                bool insert = false;
                strSql.Append("begin ");
                DataTable dt = DbHelperOra.Query("SELECT SEQ_NO,BOTTOM_DEPTH FROM COM_BASE_STRATA_LAYER2 WHERE COM_BASE_STRATA_LAYER2.JOB_PLAN_CD = :JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    object obj = data.Rows[i].GetField<decimal>("底部井深");
                    if (obj == DBNull.Value) continue;
                    var rows = dt.Select("BOTTOM_DEPTH=" + obj);

                    if (rows.Count() > 0)
                    {
                        strSql.Append("update COM_BASE_STRATA_LAYER2 set ");
                        strSql.Append(" STRAT_UNIT_NAME = :STRAT_UNIT_NAME" + i + " , ");
                        strSql.Append(" VERTICAL_DEPTH = :VERTICAL_DEPTH" + i + " , ");
                        strSql.Append(" VERTICAL_THICKNESS = :VERTICAL_THICKNESS" + i + " , ");
                        strSql.Append(" SLANT_THICNESS = :SLANT_THICNESS" + i + " , ");
                        strSql.Append(" REMARK = :REMARK" + i);
                        strSql.Append(" where SEQ_NO=" + rows.First()["SEQ_NO"] + ";");

                    }
                    else
                    {
                        insert = true;
                        strSql.Append("insert into COM_BASE_STRATA_LAYER2(");
                        strSql.Append("SEQ_NO,JOB_PLAN_CD,REQUISITION_CD,STRAT_UNIT_NAME,BOTTOM_DEPTH,VERTICAL_DEPTH,VERTICAL_THICKNESS,SLANT_THICNESS,REMARK");
                        strSql.Append(") values (");
                        strSql.Append("SEQ_NO_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,:STRAT_UNIT_NAME" + i + ",:BOTTOM_DEPTH" + i + ",:VERTICAL_DEPTH" + i + ",:VERTICAL_THICKNESS" + i + ",:SLANT_THICNESS" + i + ",:REMARK" + i);
                        strSql.Append(");");
                        parameters.Add(ServiceUtils.CreateOracleParameter(":BOTTOM_DEPTH" + i, OracleDbType.Decimal,obj));
                    }

                    parameters.AddRange(new OracleParameter[] {                      
                        ServiceUtils.CreateOracleParameter(":STRAT_UNIT_NAME"+ i, OracleDbType.Varchar2,data.Rows[i].GetField<string>("地层单位名称")) ,                   
                        ServiceUtils.CreateOracleParameter(":VERTICAL_DEPTH"+ i, OracleDbType.Decimal,data.Rows[i].GetField<decimal>("底部垂深")) ,            
                        ServiceUtils.CreateOracleParameter(":VERTICAL_THICKNESS"+ i, OracleDbType.Decimal,data.Rows[i].GetField<decimal>("垂厚")) ,     
                        ServiceUtils.CreateOracleParameter(":SLANT_THICNESS"+ i, OracleDbType.Decimal,data.Rows[i].GetField<decimal>("斜厚")) ,            
                        ServiceUtils.CreateOracleParameter(":REMARK"+ i, OracleDbType.Varchar2,data.Rows[i].GetField<string>("备注"))
                    });
                }
                if (insert)
                {
                    parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                    parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, requistion_cd));
                }
                strSql.Append(" end;");
                if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
                return false;
            }
        }

        private static object lockLoggingInterpretation = new object();
        /// <summary>
        /// 录井解释成果导入
        /// </summary>
        /// <param name="job_plan_cd"></param>
        /// <param name="requistion_cd"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool ImportLoggingInterpretation(string job_plan_cd, string requistion_cd, DataTable data)
        {
            if (data == null || data.Rows.Count < 1 || !data.Columns.Contains("顶界井深") || !data.Columns.Contains("底界井深")) return false;
            lock (lockLoggingInterpretation)
            {
                StringBuilder strSql = new StringBuilder();
                List<OracleParameter> parameters = new List<OracleParameter>();
                bool insert = false;
                strSql.Append("begin ");
                DataTable dt = DbHelperOra.Query("SELECT INTERPRETATION_CD,WELL_TOP_DEPTH,WELL_BOTTOM_DEPTH FROM DM_LOG_LOGGING_INTERPRETATION WHERE JOB_PLAN_CD=:JOB_PLAN_CD", ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd)).Tables[0];
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    object obj = data.Rows[i].GetField<decimal>("顶界井深");
                    object obj1 = data.Rows[i].GetField<decimal>("底界井深");
                    if (obj == DBNull.Value || obj1 == DBNull.Value) continue;
                    var rows = dt.Select("WELL_TOP_DEPTH=" + obj + " and WELL_BOTTOM_DEPTH=" + obj1);
                    if (rows.Count() > 0)
                    {
                        strSql.Append("update DM_LOG_LOGGING_INTERPRETATION set ");
                        strSql.Append(" UPDATE_DATE = systimestamp, ");
                        strSql.Append(" HORIZON = :HORIZON" + i + " , ");
                        strSql.Append(" THICKNESS = :THICKNESS" + i + " , ");
                        strSql.Append(" YSMC = :YSMC" + i + " , ");
                        strSql.Append(" YSMS = :YSMS" + i + " , ");
                        strSql.Append(" YGYS = :YGYS" + i + " , ");
                        strSql.Append(" YGJB = :YGJB" + i + " , ");
                        strSql.Append(" DLYGFXZ = :DLYGFXZ" + i + " , ");
                        strSql.Append(" DLYGJB = :DLYGJB" + i + " , ");
                        strSql.Append(" DISPLAY_TYPE = :DISPLAY_TYPE" + i + " , ");
                        strSql.Append(" DISPLAY_GENERAY = :DISPLAY_GENERAY" + i);
                        strSql.Append(" where INTERPRETATION_CD=" + rows.First()["INTERPRETATION_CD"] + ";");

                    }
                    else
                    {
                        insert = true;
                        strSql.Append("insert into DM_LOG_LOGGING_INTERPRETATION(");
                        strSql.Append("INTERPRETATION_CD,JOB_PLAN_CD,REQUISITION_CD,UPDATE_DATE,HORIZON,WELL_TOP_DEPTH,WELL_BOTTOM_DEPTH,THICKNESS,YSMC,YSMS,YGYS,YGJB,DLYGFXZ,DLYGJB,DISPLAY_TYPE,DISPLAY_GENERAY");
                        strSql.Append(") values (");
                        strSql.Append("INTERPRETATION_CD_SEQ.nextval,:JOB_PLAN_CD,:REQUISITION_CD,systimestamp,:HORIZON" + i + ",:WELL_TOP_DEPTH" + i + ",:WELL_BOTTOM_DEPTH" + i + ",:THICKNESS" + i + ",:YSMC" + i + ",:YSMS" + i + ",:YGYS" + i + ",:YGJB" + i + ",:DLYGFXZ" + i + ",:DLYGJB" + i + ",:DISPLAY_TYPE" + i + ",:DISPLAY_GENERAY" + i);
                        strSql.Append(");");
                        parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_TOP_DEPTH" + i, OracleDbType.Decimal, obj));
                        parameters.Add(ServiceUtils.CreateOracleParameter(":WELL_BOTTOM_DEPTH" + i, OracleDbType.Decimal, obj1));
                    }

                    parameters.AddRange(new OracleParameter[] {                            
                        ServiceUtils.CreateOracleParameter(":HORIZON"+ i, OracleDbType.Char, data.Rows[i].GetField<string>("层位")) ,            
                        ServiceUtils.CreateOracleParameter(":THICKNESS"+ i, OracleDbType.Decimal, data.Rows[i].GetField<decimal>("厚度")) ,            
                        ServiceUtils.CreateOracleParameter(":YSMC"+ i, OracleDbType.NVarchar2, data.Rows[i].GetField<string>("岩石名称")) ,            
                        ServiceUtils.CreateOracleParameter(":YSMS"+ i, OracleDbType.NVarchar2, data.Rows[i].GetField<string>("岩石描述")) ,            
                        ServiceUtils.CreateOracleParameter(":YGYS"+ i, OracleDbType.NVarchar2, data.Rows[i].GetField<string>("荧光颜色")) ,            
                        ServiceUtils.CreateOracleParameter(":YGJB"+ i, OracleDbType.NVarchar2, data.Rows[i].GetField<string>("荧光级别")) ,            
                        ServiceUtils.CreateOracleParameter(":DLYGFXZ"+ i, OracleDbType.Decimal, data.Rows[i].GetField<decimal>("定量荧光分析值")) ,            
                        ServiceUtils.CreateOracleParameter(":DLYGJB"+ i, OracleDbType.Char, data.Rows[i].GetField<string>("定量荧光级别")) ,            
                        ServiceUtils.CreateOracleParameter(":DISPLAY_TYPE"+ i, OracleDbType.Varchar2, data.Rows[i].GetField<string>("显示类型")) ,            
                        ServiceUtils.CreateOracleParameter(":DISPLAY_GENERAY"+ i, OracleDbType.NVarchar2,data.Rows[i].GetField<string>("显示情况"))             
                    });
                }
                if (insert)
                {
                    parameters.Add(ServiceUtils.CreateOracleParameter(":JOB_PLAN_CD", OracleDbType.Varchar2, job_plan_cd));
                    parameters.Add(ServiceUtils.CreateOracleParameter(":REQUISITION_CD", OracleDbType.Varchar2, requistion_cd));
                }
                strSql.Append(" end;");
                if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
                return false;
            }
        }
    }
}