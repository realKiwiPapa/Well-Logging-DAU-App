using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
using System.Text.RegularExpressions;

using Maticsoft.DBUtility;

namespace Logging_App.WebService
{
    public static class ServiceUtils
    {

        public static OracleParameter CreateOracleParameter(string name, string sourceColumn, OracleDbType oracleType)
        {
            OracleParameter parameter = new OracleParameter(name, oracleType);
            parameter.SourceColumn = sourceColumn;
            return parameter;
        }

        public static OracleParameter CreateOracleParameter(string name, OracleDbType oracleType, object value = null)
        {
            value = value == null ? DBNull.Value : value;
            OracleParameter parameter = new OracleParameter(name, value);
            parameter.OracleDbType = oracleType;
            return parameter;
        }

        public static void SetUserInfo(Model.HS_USER userModel)
        {
            HttpContext context = HttpContext.Current;
            ClearSession();
            context.Session.Timeout = 30;
            context.Session["userinfo"] = userModel;
            context.Session["logintime"] = DateTime.Now;
        }

        public static Model.HS_USER GetUserInfo(bool throwErr = true)
        {
            HttpContext context = HttpContext.Current;
            object user;
            if (context.Session != null && (user = context.Session["userinfo"]) != null)
            {
                return (Model.HS_USER)user;
            }
            if (throwErr) ThrowSoapException("没有登录，或登录已经失效，请重新登录！");
            return null;
        }

        public static DateTime GetUserLoginTime(bool throwErr = true)
        {
            HttpContext context = HttpContext.Current;
            object time;
            if (context.Session != null && (time = context.Session["logintime"]) != null)
            {
                try
                {
                    return System.Convert.ToDateTime(time);
                }
                catch { }

            }
            if (throwErr) ThrowSoapException("没有登录，或登录已经失效，请重新登录！");
            return DateTime.MinValue;
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public static string GetSessionID(bool throwErr = true)
        {
            HttpContext context = HttpContext.Current;
            string sessionID;
            if (context.Session != null && (sessionID = context.Session.SessionID) != null)
            {
                return sessionID;
            }
            if (throwErr) ThrowSoapException("没有登录，或登录已经失效，请重新登录！");
            return null;
        }

        public static void WriteVisitLog()
        {
#if !DEBUG
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SYS_USER_VISIT_LOG(");
            strSql.Append("COL_LOGINNAME,IP");
            strSql.Append(") values (");
            strSql.Append(":COL_LOGINNAME,:IP");
            strSql.Append(")");
            DbHelperOra.ExecuteSqlTran(strSql.ToString(), new OracleParameter[] {
                ServiceUtils.CreateOracleParameter(":COL_LOGINNAME", OracleDbType.Varchar2, GetUserInfo().COL_LOGINNAME) , 
                ServiceUtils.CreateOracleParameter(":IP",  OracleDbType.Varchar2, GetIP())
            });
#endif
        }

        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static string GetIP()
        {
            string result = String.Empty;
            HttpContext context = HttpContext.Current;
            result = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(result))
            {
                result = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (String.IsNullOrEmpty(result))
            {
                result = context.Request.UserHostAddress;
            }
            if (String.IsNullOrEmpty(result) || !IsIP(result))
            {
                return "0.0.0.0";
            }
            return result;
        }


        public static void ThrowSoapException(string faultstring, string falutcode = "Client")
        {
            HttpContext context = HttpContext.Current;
            var xmlstr = new StringBuilder();
            xmlstr.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlstr.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            xmlstr.Append("<soap:Body><soap:Fault>");
            xmlstr.Append("<faultcode>soap:{0}</faultcode>");
            xmlstr.Append("<faultstring>{1}</faultstring>");
            xmlstr.Append("<detail /></soap:Fault></soap:Body></soap:Envelope>");
            context.Response.ContentType = "text/xml; charset=utf-8";
            context.Response.Clear();
            var stream = Utility.CompressHelper.GZipCompress(Encoding.UTF8.GetBytes(string.Format(xmlstr.ToString(), falutcode.xmlEscape(), faultstring.xmlEscape())));
            //byte[] buffer = Utility.CompressHelper.ReadStreamToBytes(stream);
            //_originalStream.Write(buffer, 0, buffer.Length);
            context.Response.BinaryWrite(stream);
            //context.Response.Write(string.Format(xmlstr.ToString(), falutcode.xmlEscape(), faultstring.xmlEscape()));
            context.Response.End();
        }

        public static string xmlEscape(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return System.Security.SecurityElement.Escape(obj.ToString());
        }

        public static string GetHashString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                var shaM = new System.Security.Cryptography.SHA256Managed();
                str = BitConverter.ToString(shaM.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty);
                shaM.Clear();
            }
            return str;
        }
    }
}