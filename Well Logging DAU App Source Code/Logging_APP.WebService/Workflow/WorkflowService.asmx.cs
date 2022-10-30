using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using System.Data;
using Oracle.DataAccess.Client;

using Maticsoft.DBUtility;
using Logging_App.WebService.Workflow;
using Logging_App.Utility;

namespace Logging_App.WebService.Workflow
{
    /// <summary>
    /// WorkflowService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    [XmlInclude(typeof(Workflow.C测井任务通知单))]
    [XmlInclude(typeof(Workflow.C计划任务书))]
    [XmlInclude(typeof(Workflow.C测井作业收集信息))]
    [XmlInclude(typeof(Workflow.C测井现场提交信息))]
    [XmlInclude(typeof(Workflow.C解释处理作业))]
    public class WorkflowService : System.Web.Services.WebService
    {


        [WebMethod(EnableSession = true)]
        public ServiceEnums.WorkflowType[] GetActiveUserWorkflowTypes()
        {
            var dt = DbHelperOra.Query("select distinct flow_type from sys_work_flow where target_loginname=:LOGINNAME",
                new OracleParameter[] { ServiceUtils.CreateOracleParameter(":LOGINNAME", OracleDbType.Varchar2, ServiceUtils.GetUserInfo().COL_LOGINNAME) }).Tables[0];
            var list = new List<ServiceEnums.WorkflowType>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add((ServiceEnums.WorkflowType)dr.FieldEx<decimal>(0));
            }
            return list.ToArray();
        }


        [WebMethod(EnableSession = true)]
        public Controller GetWorkflowData(ServiceEnums.WorkflowType type, string obj_id1, string obj_id2)
        {
            return Controller.GetData(type, obj_id1, obj_id2);
        }

        [WebMethod(EnableSession = true)]
        public bool SubmitReview(ServiceEnums.WorkflowType type, string target_loginname, string obj_id1, string obj_id2)
        {
            return Controller.SubmitReview(type, target_loginname, obj_id1, obj_id2);
        }

        [WebMethod(EnableSession = true)]
        public bool Review(ServiceEnums.WorkflowType type, ServiceEnums.WorkflowState state, string obj_id1, string obj_id2, string message)
        {
            return Controller.Review(type, state, obj_id1, obj_id1, message);
        }

        [WebMethod(EnableSession = true)]
        public bool AppointData(ServiceEnums.WorkflowType type, string target_loginname, string obj_id1, string obj_id2)
        {
            return Controller.AppointData(type, target_loginname, obj_id1, obj_id1);
        }

        [WebMethod(EnableSession = true)]
        public bool CancelSubmitReview(ServiceEnums.WorkflowType type, string obj_id1)
        {
            return Controller.CancelSubmitReview(type, obj_id1);
        }

        [WebMethod(EnableSession = true)]
        public bool CancelReview(ServiceEnums.WorkflowType type, string obj_id1, string message)
        {
            return Controller.CancelReview(type, obj_id1, message);
        }

        //[WebMethod(EnableSession = true)]
        //public bool SetWorkFlow_通知单指派(string obj_id, string target_loginname)
        //{
        //    if (new UserService().GetActiveUserRoles().Contains(ServiceEnums.UserRole.调度管理员))
        //        return SetWorkFlow(obj_id, target_loginname, ServiceEnums.WorkflowType.测井任务通知单);
        //    return false;
        //}

        //[WebMethod(EnableSession = true)]
        //public bool SetWorkFlow_计划任务书指派(string obj_id, string target_loginname)
        //{
        //    return SetWorkFlow(obj_id, target_loginname, ServiceEnums.WorkflowType.计划任务书);
        //}

        //[WebMethod(EnableSession = true)]
        //public DataTable GetWorkFlow(string obj_id, int flow_type)
        //{
        //    return DbHelperOra.Query("SELECT A.*,B.COL_NAME AS SOURCE_NAME,C.COL_NAME AS TARGET_NAME FROM SYS_WORK_FLOW A,HS_USER B,HS_USER C WHERE A.OBJ_ID=:OBJ_ID AND A.FLOW_TYPE=:FLOW_TYPE AND B.COL_LOGINNAME (+)=A.SOURCE_LOGINNAME AND C.COL_LOGINNAME (+)=A.TARGET_LOGINNAME ORDER BY FLOW_ID DESC,A.FLOW_TIME DESC",
        //        new OracleParameter[] {
        //           ServiceUtils.CreateOracleParameter(":OBJ_ID", OracleDbType.Varchar2, obj_id) ,            
        //           ServiceUtils.CreateOracleParameter(":FLOW_TYPE", OracleType.Number, flow_type)
        //    });
        //}

        //[WebMethod(EnableSession = true)]
        //public bool SetLogPlanState(string obj_id, string target_loginname, int state)
        //{
        //    return SetWorkFlow(obj_id, target_loginname, ServiceEnums.WorkflowType.计划任务书, state);
        //}

        //[WebMethod(EnableSession = true)]
    }
}
