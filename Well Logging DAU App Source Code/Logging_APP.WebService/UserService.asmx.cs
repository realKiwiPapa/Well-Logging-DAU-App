using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Data;
using Oracle.DataAccess.Client;
using Logging_App.Utility;
using Maticsoft.DBUtility;

namespace Logging_App.WebService
{
    /// <summary>
    /// UserService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class UserService : System.Web.Services.WebService
    {


        [WebMethod(EnableSession = true)]
        public DataSet GetDeptList()
        {
            return DbHelperOra.Query("select * from sys_dept");
        }

        //[WebMethod(EnableSession = true)]
        //public decimal AddDept(byte[] dept)
        //{
        //    var dept_model = Utility.ModelHelper.DeserializeObject(dept) as Model.SYS_DEPT;
        //    if (dept_model == null) return 0;
        //    decimal dept_id = (decimal)DbHelperOra.GetSingle("select SYS_DEPT_SEQUENCE.nextval from dual");
        //    if (DbHelperOra.ExecuteSql("INSERT INTO sys_dept(dept_id,name,parent_id,dept_type) values(:dept_id,:name,:parent_id,:dept_type)",
        //        new OracleParameter[]{
        //            ServiceUtils.CreateOracleParameter(":dept_id",OracleType.Number,dept_id),
        //            ServiceUtils.CreateOracleParameter(":name",OracleType.NVarChar,dept_model.NAME),
        //            ServiceUtils.CreateOracleParameter(":parent_id",OracleType.Number,dept_model.PARENT_ID),
        //            ServiceUtils.CreateOracleParameter(":dept_type",OracleType.Number,dept_model.DEPT_TYPE)
        //        }
        //        ) > 0)
        //        return dept_id;
        //    else
        //        return 0;
        //}

        //[WebMethod(EnableSession = true)]
        //public bool ChangeDept(byte[] dept)
        //{
        //    var dept_model = Utility.ModelHelper.DeserializeObject(dept) as Model.SYS_DEPT;
        //    if (dept_model == null) return false;
        //    if (DbHelperOra.ExecuteSql("update sys_dept set name=:name,parent_id=:parent_id,dept_type=:dept_type where dept_id=:dept_id",
        //        new OracleParameter[]{
        //            ServiceUtils.CreateOracleParameter(":dept_id",OracleType.Number,dept_model.DEPT_ID),
        //            ServiceUtils.CreateOracleParameter(":name",OracleType.NVarChar,dept_model.NAME),
        //            ServiceUtils.CreateOracleParameter(":parent_id",OracleType.Number,dept_model.PARENT_ID),
        //            ServiceUtils.CreateOracleParameter(":dept_type",OracleType.Number,dept_model.DEPT_TYPE)
        //        }
        //        ) > 0)
        //        return true;
        //    else
        //        return false;
        //}

        //    [WebMethod(EnableSession = true)]
        //    public bool CreateUser(byte[] user)
        //    {
        //        var userModel = Utility.ModelHelper.DeserializeObject(user) as Model.SYS_USER;
        //        if (DbHelperOra.Exists("select 1 from sys_user where username=:USERNAME",
        //new OracleParameter[] { ServiceUtils.CreateOracleParameter(":USERNAME", OracleDbType.Varchar2, userModel.USERNAME) }))
        //        {
        //            ServiceUtils.ThrowSoapException("用户账户已经存在！");
        //        }
        //        if (DbHelperOra.ExecuteSql("insert into sys_user(user_id,username,fullname,dept_id,password) values (SYS_USER_SEQUENCE.nextval,:USERNAME,:FULLNAME,:DEPT_ID,:PASSWORD)",
        //            new OracleParameter[]{
        //                ServiceUtils.CreateOracleParameter(":USERNAME",OracleDbType.Varchar2,userModel.USERNAME),
        //                ServiceUtils.CreateOracleParameter(":FULLNAME",OracleType.NVarChar,userModel.FULLNAME),
        //                ServiceUtils.CreateOracleParameter(":DEPT_ID",OracleType.Number,userModel.DEPT_ID),
        //                ServiceUtils.CreateOracleParameter(":PASSWORD",OracleDbType.Varchar2,ServiceUtils.GetHashString(userModel.PASSWORD))
        //            }) > 0) return true;
        //        return false;
        //    }

        [WebMethod(EnableSession = true)]
        public DataSet GetDeptUser(decimal deptid)
        {
            return DbHelperOra.Query("select user_id,username,dept_id,fullname,create_time from sys_user where dept_id=" + deptid);
        }

        //[WebMethod(EnableSession = true)]
        //public bool MoveUser(decimal dept_id, decimal[] userids)
        //{
        //    if (userids == null || userids.Length < 1) return false;
        //    if (DbHelperOra.ExecuteSql(string.Format("update sys_user set dept_id={0} where user_id in({1})", dept_id, string.Join(",", userids))) > 0) return true;
        //    return false;
        //}

        //[WebMethod(EnableSession = true)]
        //public bool ChangeUserName(decimal user_id, string username)
        //{
        //    if (DbHelperOra.ExecuteSql("update sys_user set fullname=:USERNAME where user_id=" + user_id,
        //        new OracleParameter[]{
        //            ServiceUtils.CreateOracleParameter(":USERNAME",OracleDbType.Varchar2,username)
        //        }
        //        ) > 0) return true;
        //    return false;
        //}

        [WebMethod(EnableSession = true)]
        public bool ChangePassword(decimal user_id, string pw)
        {
            if (!GetActiveUserRoles().Contains(ServiceEnums.UserRole.系统管理员))
                return false;
            return changeUserPassword(user_id, pw);
        }

        private bool changeUserPassword(decimal user_id, string pw)
        {
            if (DbHelperOra.ExecuteSql("update hs_user set col_pword=:PW where col_id=" + user_id,
                new OracleParameter[]{
                    ServiceUtils.CreateOracleParameter(":PW",OracleDbType.Varchar2,ServiceUtils.GetHashString(pw))
                }
                ) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public ServiceEnums.UserRole[] GetUserRole(string loginName)
        {
            var dt = DbHelperOra.Query("select distinct col_roletype from hs_role where col_loginname=:LOGINNAME",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, loginName) }).Tables[0];
            var list = new List<ServiceEnums.UserRole>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add((ServiceEnums.UserRole)dr.FieldEx<decimal>(0));
            }
            return list.ToArray();
        }

        [WebMethod(EnableSession = true)]
        public bool ChangeUserRole(string loginName, ServiceEnums.UserRole[] roles)
        {
            if (!GetActiveUserRoles().Contains(ServiceEnums.UserRole.系统管理员))
                return false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("begin ");
            strSql.Append("delete hs_role where col_loginname=:LOGINNAME;");
            if (roles != null)
            {
                foreach (int i in roles)
                {
                    strSql.Append("insert into hs_role(col_loginname,col_roletype) values(:LOGINNAME," + i + ");");
                }
            }
            strSql.Append(" end;");
            if (DbHelperOra.ExecuteSqlTran(strSql.ToString(),
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, loginName) }) > 0) return true;
            return false;
        }

        [WebMethod(EnableSession = true)]
        public DataSet Login(string loginName, string password)
        {/*
            var dt = new DataTable();
            dt.Columns.Add("PROJECT_ID");
            dt.Columns.Add("X_VALUES", typeof(decimal));
            for (int i = 0; i < 10; i++)
                dt.Rows.Add(i.ToString(), i+1);
            dt.Rows.Add("aaaa", 1000000);
            int i1 = DbHelperOra.InsertDataTable("insert into PC_PROJECT_ASSOC(project_id,x_values) values(:PROJECT_ID,:X_VALUES)", dt,
                 ServiceUtils.CreateOracleParameter(":PROJECT_ID", "PROJECT_ID", OracleDbType.Varchar2),
                 ServiceUtils.CreateOracleParameter(":X_VALUES", "X_VALUES", OracleDbType.Decimal));
            var dic=new Dictionary<string,OracleParameter[]>();
           
            dic.Add("delete from COM_LOG_RESULT where PROCESS_ID=:PROCESS_ID",new OracleParameter[]{ ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2,"375")});
            for (int i = 0; i < 10; i++)
            {
                dic.Add("/1*" + i + "*1/insert into com_log_result(RESULTID,PROCESS_ID)values(RESULTID_SEQ.nextval,:PROCESS_ID)", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":PROCESS_ID", OracleDbType.Varchar2, "375") });
                dic.Add("/1*" + i + "*1/insert into WL_ACH_SHALE_GAS_INTER(RESULTID,SHALE_GAS_INTER_ID)values(RESULTID_SEQ.currval,SHALE_GAS_INTER_ID_SEQ.nextval)",null);
            } dic.Add("/1*11*1/insert into WL_ACH_SHALE_GAS_INTER(RESULTID,SHALE_GAS_INTER_ID)values(121445,SHALE_GAS_INTER_ID_SEQ.nextval)", null);
            DbHelperOra.ExecuteSqlTran(dic);*/
            string strSql;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, loginName));
            password = ServiceUtils.GetHashString(password);
            strSql = "select col_id,col_loginname,col_name from hs_user where col_loginname=:LOGINNAME and col_pword";
            if (password == null)
            {
                strSql += " is null";
            }
            else
            {
                strSql += "=:PASSWORD";
                parameters.Add(ServiceUtils.CreateOracleParameter(":PASSWORD", OracleDbType.Varchar2, password));
            }
            var ds = DbHelperOra.Query(strSql, parameters.ToArray());
            if (ds.Tables[0].Rows.Count == 1)
            {
                ServiceUtils.SetUserInfo(Utility.ModelHandler<Model.HS_USER>.FillModel(ds.Tables[0].Rows[0]));
                ServiceUtils.WriteVisitLog();
            }
            return ds;
        }

        [WebMethod(EnableSession = true)]
        public ServiceEnums.UserRole[] GetActiveUserRoles()
        {
            return GetUserRole(ServiceUtils.GetUserInfo().COL_LOGINNAME);
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetOnlineUser()
        {
            return OnlineUser.GetDataTable();
        }

        //[WebMethod(EnableSession = true)]
        //public DataSet GetDataRight(string data_id)
        //{
        //    return DbHelperOra.Query("select NAME,b.rights_type_id as RIGHTS_TYPE_ID,WIRTEUSER,CHECKUSER,STATE from sys_data_rights a,sys_data_rights_type b where a.rights_type_id (+)=b.rights_type_id and a.data_id(+)=:dataid order by b.rights_type_id", new OracleParameter[] { ServiceUtils.CreateOracleParameter(":dataid", OracleDbType.Varchar2, data_id) });
        //}

        //[WebMethod(EnableSession = true)]
        //public bool SetDataRight(string dataID, byte[] bytesData)
        //{
        //    var models = Utility.ModelHelper.DeserializeObject(bytesData) as List<Model.SYS_DATA_RIGHTS>;
        //    if (string.IsNullOrWhiteSpace(dataID) || models == null || models.Count < 1) return false;

        //    var strSql = new StringBuilder();
        //    List<OracleParameter> parameters = new List<OracleParameter>();
        //    strSql.Append("begin ");
        //    parameters.Add(ServiceUtils.CreateOracleParameter(":DATA_ID", OracleDbType.Varchar2, dataID));
        //    for (int i = 0; i < models.Count; i++)
        //    {
        //        if (models[i].CHECKUSER == models[i].WIRTEUSER) ServiceUtils.ThrowSoapException("用户不能相同!");
        //        if (models[i].STATE == null)
        //        {
        //            strSql.Append("insert into SYS_DATA_RIGHTS(");
        //            strSql.Append("DATA_ID,RIGHTS_TYPE_ID,WIRTEUSER,CHECKUSER");
        //            strSql.Append(") values (");
        //            strSql.Append(":DATA_ID,:RIGHTS_TYPE_ID" + i + ",:WIRTEUSER" + i + ",:CHECKUSER" + i);
        //            strSql.Append(") ;");
        //        }
        //        else
        //        {
        //            strSql.Append("update SYS_DATA_RIGHTS set ");
        //            strSql.Append(" DATA_ID = :DATA_ID , ");
        //            strSql.Append(" WIRTEUSER = :WIRTEUSER" + i + ", ");
        //            strSql.Append(" CHECKUSER = :CHECKUSER" + i + "  ");
        //            strSql.Append(" where DATA_ID=:DATA_ID and RIGHTS_TYPE_ID=:RIGHTS_TYPE_ID" + i + ";");
        //        }
        //        parameters.AddRange(new OracleParameter[] {
        //           ServiceUtils.CreateOracleParameter(":RIGHTS_TYPE_ID"+i, OracleType.Number, models[i].RIGHTS_TYPE_ID) ,            
        //           ServiceUtils.CreateOracleParameter(":WIRTEUSER"+i, OracleDbType.Varchar2, models[i].WIRTEUSER) ,            
        //           ServiceUtils.CreateOracleParameter(":CHECKUSER"+i, OracleDbType.Varchar2, models[i].CHECKUSER) });
        //    }
        //    strSql.Append("end;");
        //    if (DbHelperOra.ExecuteSqlTran(strSql.ToString(), parameters.ToArray()) > 0) return true;
        //    return false;
        //}

        [WebMethod(EnableSession = true)]
        public bool UserChangePassword(string pwOld, string pwNew)
        {
            var userInfo = ServiceUtils.GetUserInfo();
            var dt = Login(userInfo.COL_LOGINNAME, pwOld).Tables[0];
            if (dt == null || dt.Rows.Count < 1) ServiceUtils.ThrowSoapException("旧密码错误！");
            return changeUserPassword(userInfo.COL_ID, pwNew);
        }

        [WebMethod(EnableSession = true)]
        public int SyncUser()
        {
            if (!GetActiveUserRoles().Contains(ServiceEnums.UserRole.系统管理员))
                return 0;
            return SyncUserClass.Do();
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetHSView()
        {
            return DbHelperOra.Query("SELECT COL_ID,COL_NAME FROM HS_VIEW WHERE COL_DISABLED=0 ORDER BY COL_ITEMINDEX,COL_NAME");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetHSGroup()
        {
            return DbHelperOra.Query("SELECT COL_ID,COL_NAME FROM HS_GROUP WHERE COL_DISABLED=0 ORDER BY COL_ITEMINDEX,COL_NAME");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetHSRelation()
        {
            return DbHelperOra.Query("SELECT COL_HSITEMTYPE,COL_HSITEMID,COL_DHSITEMID,COL_DHSITEMTYPE FROM HS_RELATION WHERE COL_DISABLED=0 AND COL_DHSITEMTYPE=2");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetHSUser(decimal hsItemID, decimal hsItemType)
        {
            return DbHelperOra.Query("SELECT A.COL_ID,A.COL_LOGINNAME,A.COL_NAME FROM HS_USER A,HS_RELATION B WHERE A.COL_DISABLED=0 AND A.COL_ID=B.COL_DHSITEMID AND B.COL_DHSITEMTYPE=1 AND B.COL_HSITEMID=" + hsItemID + " AND B.COL_HSITEMTYPE=" + hsItemType + " ORDER BY A.COL_ITEMINDEX,A.COL_LOGINNAME");
        }

        [WebMethod(EnableSession = true)]
        public DataSet GetUser(ServiceEnums.UserRole role)
        {
            return DbHelperOra.Query("select b.col_loginname,b.col_name,c.col_name group_name from hs_role a,hs_user b,hs_group c,hs_relation d where a.col_loginname=b.col_loginname and c.col_id=d.col_hsitemid and b.col_id=d.col_dhsitemid and d.col_hsitemtype=2 and d.col_dhsitemtype=1 and a.col_roletype=" + (int)role + " order by b.col_itemindex,b.col_loginname");
        }
    }
}
