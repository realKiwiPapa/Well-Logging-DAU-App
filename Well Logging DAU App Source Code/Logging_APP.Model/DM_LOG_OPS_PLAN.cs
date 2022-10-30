using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    /// <summary>
    /// 计划任务书 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_OPS_PLAN : ModelBase
    {
        private string _requisition_cd;
        /// <summary>
        /// 通知单编码
        /// </summary>	
        [Required]
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string REQUISITION_CD
        {
            get { return _requisition_cd; }
            set { _requisition_cd = value; }
        }
        private string _job_plan_cd;
        /// <summary>
        /// 作业计划书编号
        /// </summary>	
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string JOB_PLAN_CD
        {
            get { return _job_plan_cd; }
            set { _job_plan_cd = value; }
        }
        private DateTime? _received_inform_time;
        /// <summary>
        /// 接通知时间
        /// </summary>	
        public DateTime? RECEIVED_INFORM_TIME
        {
            get { return _received_inform_time; }
            set { _received_inform_time = value; }
        }
        private DateTime? _requirements_time;
        /// <summary>
        /// 要求到井时间
        /// </summary>	
        public DateTime? REQUIREMENTS_TIME
        {
            get { return _requirements_time; }
            set { _requirements_time = value; }
        }
        private decimal? _job_id;
        /// <summary>
        /// 作业目的ID
        /// </summary>	
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? JOB_ID
        {
            get { return _job_id; }
            set { _job_id = value; }
        }
        private string _job_layer;
        /// <summary>
        /// 作业层位
        /// </summary>	
        [OracleStringLength(MaximumLength = 120, CharUsed = CharUsedType.Char)]
        public string JOB_LAYER
        {
            get { return _job_layer; }
            set { _job_layer = value; }
        }
        private decimal? _predicted_logging_items_id;
        /// <summary>
        /// 预测项目
        /// </summary>	
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? PREDICTED_LOGGING_ITEMS_ID
        {
            get { return _predicted_logging_items_id; }
            set { _predicted_logging_items_id = value; }
        }
        private string _log_type;
        /// <summary>
        /// 测井类型
        /// </summary>	
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_TYPE
        {
            get { return _log_type; }
            set { _log_type = value; }
        }
        private string _log_mode;
        /// <summary>
        /// 测井方式
        /// </summary>	
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_MODE
        {
            get { return _log_mode; }
            set { _log_mode = value; }
        }
        private string _prepare_person;
        /// <summary>
        /// 编制人
        /// </summary>	
        [OracleStringLength(MaximumLength = 16, CharUsed = CharUsedType.Byte)]
        public string PREPARE_PERSON
        {
            get { return _prepare_person; }
            set { _prepare_person = value; }
        }
        private string _verifier;
        /// <summary>
        /// 审核人
        /// </summary>	
        [OracleStringLength(MaximumLength = 16, CharUsed = CharUsedType.Byte)]
        public string VERIFIER
        {
            get { return _verifier; }
            set { _verifier = value; }
        }
        private string _approver;
        /// <summary>
        /// 批准人
        /// </summary>	
        [OracleStringLength(MaximumLength = 16, CharUsed = CharUsedType.Byte)]
        public string APPROVER
        {
            get { return _approver; }
            set { _approver = value; }
        }
        private DateTime? _prepare_date;
        /// <summary>
        /// 编制日期
        /// </summary>	
        public DateTime? PREPARE_DATE
        {
            get { return _prepare_date; }
            set { _prepare_date = value; }
        }
        private byte[] _plan_content_scanning;
        /// <summary>
        /// 计划书内容扫描件
        /// </summary>	
        public byte[] PLAN_CONTENT_SCANNING
        {
            get { return _plan_content_scanning; }
            set { _plan_content_scanning = value; }
        }
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>	
        [OracleStringLength(MaximumLength = 400, CharUsed = CharUsedType.Byte)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; }
        }
        private string _log_team_id;
        /// <summary>
        /// 测井小队ID
        /// </summary>	
        [Required]
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_TEAM_ID
        {
            get { return _log_team_id; }
            set { _log_team_id = value; }
        }
        private string _prelogging_interval;
        /// <summary>
        /// 测井井段
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string PRELOGGING_INTERVAL
        {
            get { return _prelogging_interval; }
            set { _prelogging_interval = value; NotifyPropertyChanged("PRELOGGING_INTERVAL"); }
        }

        private decimal? _plan_content_scanning_fileid;
        /// <summary>
        /// 计划书内容扫描件
        /// </summary>	
        public decimal? PLAN_CONTENT_SCANNING_FILEID
        {
            get { return _plan_content_scanning_fileid; }
            set { _plan_content_scanning_fileid = value; NotifyPropertyChanged("PLAN_CONTENT_SCANNING_FILEID"); }
        }
    }
}