using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Diagnostics;
using Oracle.DataAccess.Client;

using Maticsoft.DBUtility;

namespace Logging_App.WebService.Workflow
{
    public class WorkflowData : Model.ModelBase
    {
        public string SOURCE_LOGINNAME { get; set; }
        public string SOURCE_NAME { get; set; }
        public string TARGET_LOGINNAME { get; set; }
        public string TARGET_NAME { get; set; }
        //public decimal FLOW_TYPE { get; set; }
        public ServiceEnums.WorkflowState FLOW_STATE { get; set; }
        public DateTime FLOW_TIME { get; set; }
        public string MESSAGE { get; set; }
    }
    public abstract class Controller
    {
        private static Dictionary<ServiceEnums.WorkflowType, Type> classDic = new Dictionary<ServiceEnums.WorkflowType, Type>();
        private static Dictionary<ServiceEnums.WorkflowState, PropertyInfo> propertyDic = new Dictionary<ServiceEnums.WorkflowState, PropertyInfo>();
        private string _activeUserLoginName = string.Empty;
        private ServiceEnums.UserRole[] _activeUserRoles;
        private string _obj_id1 = string.Empty;
        private string _obj_id2 = string.Empty;

        protected static readonly ServiceEnums.WorkflowState[] CanSaveState = new ServiceEnums.WorkflowState[]
        {
            ServiceEnums.WorkflowState.审核未通过,
            ServiceEnums.WorkflowState.取消提交审核,
            ServiceEnums.WorkflowState.退回审核,
            ServiceEnums.WorkflowState.新建
        };

        protected string ActiveUserLoginName
        {
            get
            {
                if (string.IsNullOrEmpty(_activeUserLoginName))
                    _activeUserLoginName = ServiceUtils.GetUserInfo().COL_LOGINNAME;
                return _activeUserLoginName;
            }
        }
        protected ServiceEnums.UserRole[] ActiveUserRoles
        {
            get
            {
                if (_activeUserRoles == null)
                    _activeUserRoles = new UserService().GetActiveUserRoles();
                return _activeUserRoles;
            }
        }
        protected string Obj_id1 { get { return _obj_id1; } }
        protected string Obj_id2 { get { return _obj_id2; } }



        protected Dictionary<string, List<WorkflowData>> cacheData = new Dictionary<string, List<WorkflowData>>();

        protected abstract bool submitReview(string target_loginname);
        protected abstract bool review(ServiceEnums.WorkflowState state);
        protected abstract bool appointData(string target_loginname);

        public static bool SubmitReview(ServiceEnums.WorkflowType type, string target_loginname, string obj_id1, string obj_id2)
        {
            var m = (Controller)Activator.CreateInstance(classDic[type], trimID(obj_id1), trimID(obj_id2));
            if (!m.CanSubmitReview) return false;
            return SetWorkFlow(m.Obj_id1, target_loginname, type, ServiceEnums.WorkflowState.提交审核);
        }

        public static bool Review(ServiceEnums.WorkflowType type, ServiceEnums.WorkflowState state, string obj_id1 = "", string obj_id2 = "", string message = "")
        {
            if (state == ServiceEnums.WorkflowState.审核通过 || state == ServiceEnums.WorkflowState.审核未通过)
            {
                var m = (Controller)Activator.CreateInstance(classDic[type], trimID(obj_id1), trimID(obj_id2));
                if (m.CanReview)
                {
                    if (SetWorkFlow(m.Obj_id1, m.DataList.Last().SOURCE_LOGINNAME, type, state, message))
                    {
                        WorkflowData data;
                        if(type==ServiceEnums.WorkflowType.测井现场提交信息&&state==ServiceEnums.WorkflowState.审核通过&&(data=m.DataList.Find(o=>o.FLOW_STATE==ServiceEnums.WorkflowState.数据指派))!=null)
                        {
                            SetWorkFlow(m.Obj_id1, data.TARGET_LOGINNAME, type, ServiceEnums.WorkflowState.数据指派, "", false, data.SOURCE_LOGINNAME);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool AppointData(ServiceEnums.WorkflowType type, string target_loginname, string obj_id1, string obj_id2)
        {
            var m = (Controller)Activator.CreateInstance(classDic[type], trimID(obj_id1), trimID(obj_id2));
            if (m.CanAppointData)
            {
                return SetWorkFlow(m.Obj_id1, target_loginname, type, ServiceEnums.WorkflowState.数据指派);
            }
            return false;
        }

        public static bool CancelSubmitReview(ServiceEnums.WorkflowType type, string obj_id1)
        {
            var m = (Controller)Activator.CreateInstance(classDic[type], trimID(obj_id1), "");
            if (m.CanCancelSubmitReview)
            {
                return SetWorkFlow(m.Obj_id1, m.DataList[0].TARGET_LOGINNAME, type, ServiceEnums.WorkflowState.取消提交审核);
            }
            return false;
        }

        public static bool CancelReview(ServiceEnums.WorkflowType type, string obj_id1, string message)
        {
            var m = (Controller)Activator.CreateInstance(classDic[type], trimID(obj_id1), "");
            if (m.CanCancelReview)
            {
                return SetWorkFlow(m.Obj_id1, m.DataList[m.DataList.Count-1].SOURCE_LOGINNAME, type, ServiceEnums.WorkflowState.退回审核, message);
            }
            return false;
        }

        protected abstract bool canSave();
        protected abstract bool canAppointData();
        protected abstract bool canSubmitReview();
        protected abstract bool canReview();
        protected abstract bool canDelete();
        protected abstract bool canCancelSubmitReview();
        protected abstract bool canCancelReview();
        public bool CanSave
        {
            get
            {
                if (ServiceUtils.GetUserInfo().COL_LOGINNAME == "admin") return true;
                return canSave();
            }
            set { }
        }
        public bool CanAppointData
        {
            get
            {
                return canAppointData();
            }
            set { }
        }
        public bool CanSubmitReview
        {
            get
            {
                return canSubmitReview();
            }
            set { }
        }
        public bool CanReview
        {
            get
            {
                return canReview();
            }
            set { }
        }
        public bool CanDelete
        {
            get
            {
                return canDelete();
            }
            set { }
        }

        public bool CanCancelSubmitReview
        {
            get
            {
                return canCancelSubmitReview();
            }
            set { }
        }

        public bool CanCancelReview
        {
            get
            {
                return canCancelReview();
            }
            set { }
        }

        public List<WorkflowData> DataList;

        private static string trimID(string id)
        {
            return id == null ? "" : id;
        }

        static Controller()
        {
            Type type = typeof(ServiceEnums.WorkflowType);
            foreach (ServiceEnums.WorkflowType v in Enum.GetValues(type))
            {

                var obj = type.GetField(v.ToString()).GetCustomAttributes(typeof(Workflow.ClassAttribute), false);
                if (obj.Length > 0)
                    classDic.Add(v, ((Workflow.ClassAttribute)obj[0]).Type);
            }
            var properties = typeof(Controller).GetProperties();
            type = typeof(ServiceEnums.WorkflowState);
            foreach (ServiceEnums.WorkflowState v in Enum.GetValues(type))
            {
                var obj = type.GetField(v.ToString()).GetCustomAttributes(typeof(Workflow.PropertyAttribute), false);
                if (obj.Length > 0)
                {
                    var name = ((Workflow.PropertyAttribute)obj[0]).Name;
                    var property = properties.FirstOrDefault(p => p.Name == name);
                    if (property == null)
                        Debug.Fail(name + " 未定义的Property！");
                    else
                        propertyDic.Add(v, property);
                }
            }
        }

        public Controller()
        {
        }

        public Controller(string obj_id1, string obj_id2)
        {
            _obj_id1 = trimID(obj_id1);
            _obj_id2 = trimID(obj_id2);
            DataList = queryData(obj_id1);
        }

        protected List<WorkflowData> queryData(string obj_id)
        {
            if (classDic.ContainsValue(this.GetType()))
            {
                return queryData(obj_id, classDic.First(d => d.Value == this.GetType()).Key);
            }
            return null;
        }

        protected List<WorkflowData> queryData(string obj_id, ServiceEnums.WorkflowType type)
        {
            if (string.IsNullOrWhiteSpace(obj_id)) return null;
            if (cacheData.ContainsKey(obj_id + type))
                return cacheData[obj_id + type];
            var dt = DbHelperOra.Query("SELECT A.source_loginname,a.target_loginname,a.flow_state,a.flow_time,a.message,B.COL_NAME AS SOURCE_NAME,C.COL_NAME AS TARGET_NAME FROM SYS_WORK_FLOW A,HS_USER B,HS_USER C WHERE A.OBJ_ID=:OBJ_ID AND A.FLOW_TYPE=:FLOW_TYPE AND B.COL_LOGINNAME (+)=A.SOURCE_LOGINNAME AND C.COL_LOGINNAME (+)=A.TARGET_LOGINNAME ORDER BY FLOW_ID DESC,A.FLOW_TIME DESC",
    new OracleParameter[] {
			       ServiceUtils.CreateOracleParameter(":OBJ_ID", OracleDbType.Varchar2, obj_id) ,            
                   ServiceUtils.CreateOracleParameter(":FLOW_TYPE", OracleDbType.Decimal, (int)type)
            });
            var data = Utility.ModelHandler<WorkflowData>.FillModel(dt);
            List<WorkflowData> list = null;
            if (data != null && data.Count > 0)
                list = data.ToList();
            cacheData.Add(obj_id + type, list);
            return list;
        }

        public static Controller GetData(ServiceEnums.WorkflowType type, string obj_id1, string obj_id2)
        {
            return (Controller)Activator.CreateInstance(classDic[type], trimID(obj_id1), trimID(obj_id2));
        }

        public static void ValidateSave<T>(string obj_id1 = "", string obj_id2 = "") where T : Controller, new()
        {
            if (!((Controller)Activator.CreateInstance(typeof(T), trimID(obj_id1), trimID(obj_id2))).CanSave)
                ServiceUtils.ThrowSoapException("不能进行此操作！");
        }

        public static void ValidateDelete()
        {
            if (ServiceUtils.GetUserInfo().COL_LOGINNAME != "admin")
                ServiceUtils.ThrowSoapException("不能进行此操作！");
        }
        public static void ValidateDelete<T>(string obj_id1, string obj_id2 = "") where T : Controller, new()
        {
            if (!((Controller)Activator.CreateInstance(typeof(T), trimID(obj_id1), trimID(obj_id2))).CanDelete)
                ServiceUtils.ThrowSoapException("不能进行此操作！");
        }

        public static void Validate(ServiceEnums.WorkflowType type, ServiceEnums.WorkflowState state, string obj_id1, string obj_id2)
        {
            if (classDic.ContainsKey(type) && propertyDic.ContainsKey(state))
            {
                if (!(bool)propertyDic[state].GetValue(Activator.CreateInstance(classDic[type], trimID(obj_id1), trimID(obj_id2)), null))
                    ServiceUtils.ThrowSoapException("不能进行此操作！");
            }
            throw new ArgumentException("无效的参数！");
        }

        internal static bool SetWorkFlow(string obj_id, string target_loginname, ServiceEnums.WorkflowType type, ServiceEnums.WorkflowState state, string message = "", bool sendMsg = true, string source_logginname = "")
        {
            if (state != ServiceEnums.WorkflowState.新建 && string.IsNullOrWhiteSpace(target_loginname)) return false;
            if (string.IsNullOrWhiteSpace(source_logginname))
                source_logginname = ServiceUtils.GetUserInfo().COL_LOGINNAME;
            var strSql = new System.Text.StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            strSql.Append("    INSERT");
            strSql.Append("    INTO SYS_WORK_FLOW");
            strSql.Append("      (");
            strSql.Append("        OBJ_ID,");
            strSql.Append("        SOURCE_LOGINNAME,");
            strSql.Append("        TARGET_LOGINNAME,");
            strSql.Append("        FLOW_TYPE,");
            strSql.Append("        FLOW_STATE,");
            strSql.Append("        FLOW_ID,");
            strSql.Append("        MESSAGE");
            strSql.Append("      )");
            strSql.Append("      VALUES");
            strSql.Append("      (");
            strSql.Append("        :OBJ_ID,");
            strSql.Append("        :SOURCE_LOGINNAME,");
            strSql.Append("        :TARGET_LOGINNAME,");
            strSql.Append("        :FLOW_TYPE,");
            strSql.Append("        :FLOW_STATE,");
            strSql.Append("        FLOW_ID_SEQ.NEXTVAL,");
            strSql.Append("        :MESSAGE");
            strSql.Append("      )");
            parameters.AddRange(new OracleParameter[] {
                   ServiceUtils.CreateOracleParameter(":OBJ_ID", OracleDbType.Varchar2, obj_id) ,            
                   ServiceUtils.CreateOracleParameter(":SOURCE_LOGINNAME", OracleDbType.Varchar2,source_logginname ) ,
                   ServiceUtils.CreateOracleParameter(":TARGET_LOGINNAME", OracleDbType.Varchar2, target_loginname),
                   ServiceUtils.CreateOracleParameter(":FLOW_TYPE", OracleDbType.Decimal, (int)type),
                   ServiceUtils.CreateOracleParameter(":FLOW_STATE", OracleDbType.Decimal,(int)state),
                   ServiceUtils.CreateOracleParameter(":MESSAGE", OracleDbType.NVarchar2,message)
            });
            if (DbHelperOra.ExecuteSql(strSql.ToString(), parameters.ToArray()) > 0&&sendMsg)
            {
                try
                {
                    string subject = System.Configuration.ConfigurationManager.AppSettings["AppName"] + "通知";
                    string content = subject + "\n";
                    string taskType, taskName;
                    taskName = obj_id;
                    switch (type)
                    {
                        case ServiceEnums.WorkflowType.小队施工信息:
                        case ServiceEnums.WorkflowType.测井现场提交信息:
                            taskType = "小队施工信息&测井现场提交信息";
                            break;
                        case ServiceEnums.WorkflowType.解释处理作业:
                        case ServiceEnums.WorkflowType.归档入库:
                            taskType = "解释处理作业&归档入库";
                            taskName = DbHelperOra.GetSingle("select process_name from dm_log_process where process_id=" + obj_id) as string;
                            break;
                        default:
                            taskType = type.ToString();
                            break;
                    }
                    content += "您有新的消息,请注意查收！\n";
                    content += "类型：" + taskType + "\n";
                    content += "名称：" + taskName + "\n";
                    content += "操作人：" + string.Format("{0}({1})", ServiceUtils.GetUserInfo().COL_NAME, ServiceUtils.GetUserInfo().COL_LOGINNAME) + "\n";
                    content += "操作：" + (state).ToString() + "\n";
                    content += "操作时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n";
                    BigAntSDK.SendMessage(target_loginname, content, content);
                }
                catch { }
                return true;
            }
            return false;
        }
    }
}