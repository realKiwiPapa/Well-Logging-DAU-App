using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 测井任务通知单 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_TASK : ModelBase
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
            set { _requisition_cd = value; NotifyPropertyChanged("REQUISITION_CD"); }
        }
        private decimal? _requisition_type;
        /// <summary>
        /// 通知单类型
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? REQUISITION_TYPE
        {
            get { return _requisition_type; }
            set { _requisition_type = value; NotifyPropertyChanged("REQUISITION_TYPE"); }
        }
        private DateTime? _predicted_logging_date;
        /// <summary>
        /// 要求到井时间
        /// </summary>
        [Required]
        public DateTime? PREDICTED_LOGGING_DATE
        {
            get { return _predicted_logging_date; }
            set { _predicted_logging_date = value; NotifyPropertyChanged("PREDICTED_LOGGING_DATE"); }
        }
        private string _predicted_logging_interval;
        /// <summary>
        /// 预测井段
        /// </summary>
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Char)]
        public string PREDICTED_LOGGING_INTERVAL
        {
            get { return _predicted_logging_interval; }
            set { _predicted_logging_interval = value; NotifyPropertyChanged("PREDICTED_LOGGING_INTERVAL"); }
        }
        private decimal? _predicted_logging_items_id;
        /// <summary>
        /// 预测项目ID
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? PREDICTED_LOGGING_ITEMS_ID
        {
            get { return _predicted_logging_items_id; }
            set { _predicted_logging_items_id = value; NotifyPropertyChanged("PREDICTED_LOGGING_ITEMS_ID"); }
        }
        private decimal? _department_requisition;
        /// <summary>
        /// 通知单来电单位
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? DEPARTMENT_REQUISITION
        {
            get { return _department_requisition; }
            set { _department_requisition = value; NotifyPropertyChanged("DEPARTMENT_REQUISITION"); }
        }
        private string _person_equisition;
        /// <summary>
        /// 通知单来电人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string PERSON_EQUISITION
        {
            get { return _person_equisition; }
            set { _person_equisition = value; NotifyPropertyChanged("PERSON_EQUISITION"); }
        }
        private DateTime? _rec_notice_time;
        /// <summary>
        /// 通知单来电时间
        /// </summary>
        public DateTime? REC_NOTICE_TIME
        {
            get { return _rec_notice_time; }
            set { _rec_notice_time = value; NotifyPropertyChanged("REC_NOTICE_TIME"); }
        }
        private string _recipient_requisition;
        /// <summary>
        /// 通知单来电接收人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string RECIPIENT_REQUISITION
        {
            get { return _recipient_requisition; }
            set { _recipient_requisition = value; NotifyPropertyChanged("RECIPIENT_REQUISITION"); }
        }
        private string _complete_man;
        /// <summary>
        /// 任务完成单位或小队，一般情况是科级单位
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string COMPLETE_MAN
        {
            get { return _complete_man; }
            set { _complete_man = value; NotifyPropertyChanged("COMPLETE_MAN"); }
        }
        private DateTime? _treatment_date_requisition;
        /// <summary>
        /// 通知单来电处理日期
        /// </summary>
        [Required]
        public DateTime? TREATMENT_DATE_REQUISITION
        {
            get {
                if (_treatment_date_requisition!=null)
                {
                   var in_dt= DateTime.Parse(_treatment_date_requisition.Value.ToShortDateString());
                   var now_dt = DateTime.Parse(DateTime.Now.ToShortDateString());
                   if (in_dt > now_dt)
                       return DateTime.Now;
                }
                return _treatment_date_requisition; 
            }
            set { _treatment_date_requisition = value; NotifyPropertyChanged("TREATMENT_DATE_REQUISITION"); }
        }
        private string _market_identification;
        /// <summary>
        /// 市场标识
        /// </summary>
        [OracleStringLength(MaximumLength = 16, CharUsed = CharUsedType.Byte)]
        public string MARKET_IDENTIFICATION
        {
            get {
                return _market_identification; 
            }
            set { _market_identification = value; NotifyPropertyChanged("MARKET_IDENTIFICATION"); }
        }
        private decimal? _market_classify;
        /// <summary>
        /// 市场类型
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? MARKET_CLASSIFY
        {
            get { return _market_classify; }
            set { _market_classify = value; NotifyPropertyChanged("MARKET_CLASSIFY"); }
        }
        private string _verifier;
        /// <summary>
        /// 审核人
        /// </summary>
        [OracleStringLength(MaximumLength = 33, CharUsed = CharUsedType.Byte)]
        public string VERIFIER
        {
            get { return _verifier; }
            set { _verifier = value; NotifyPropertyChanged("VERIFIER"); }
        }
        private string _brief_description_content;
        /// <summary>
        /// 来电内容简要描述
        /// </summary>
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Byte)]
        public string BRIEF_DESCRIPTION_CONTENT
        {
            get { return _brief_description_content; }
            set { _brief_description_content = value; NotifyPropertyChanged("BRIEF_DESCRIPTION_CONTENT"); }
        }
        //        private byte[] _requisition_scanning;
        ///// <summary>
        ///// 通知单扫描件
        ///// </summary>
        //                public byte[] REQUISITION_SCANNING
        //{
        //    get{ return _requisition_scanning; }
        //    set{ _requisition_scanning = value; NotifyPropertyChanged("REQUISITION_SCANNING"); }
        //}        
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 800, CharUsed = CharUsedType.Char)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }
        private string _drill_statuse;
        /// <summary>
        /// 钻井状态
        /// </summary>
        [OracleStringLength(MaximumLength = 16, CharUsed = CharUsedType.Byte)]
        public string DRILL_STATUSE
        {
            get { return _drill_statuse; }
            set { _drill_statuse = value; NotifyPropertyChanged("DRILL_STATUSE"); }
        }
        private string _well_id;
        /// <summary>
        /// 井ID
        /// </summary>
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Byte)]
        public string WELL_ID
        {
            get { return _well_id.TrimCharEnd(); }
            set { _well_id = value; NotifyPropertyChanged("WELL_ID"); }
        }
        private string _drill_job_id;
        /// <summary>
        /// 作业项目ID
        /// </summary>
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string DRILL_JOB_ID
        {
            get { return _drill_job_id; }
            set { _drill_job_id = value; NotifyPropertyChanged("DRILL_JOB_ID"); }
        }

        private string _well_job_name;
        /// <summary>
        /// 作业井名
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string WELL_JOB_NAME
        {
            get { return _well_job_name; }
            set { _well_job_name = value; NotifyPropertyChanged("WELL_JOB_NAME"); }
        }

        private string _job_purpose;
        /// <summary>
        /// 作业目的
        /// </summary>
        public string JOB_PURPOSE
        {
            get { return _job_purpose; }
            set { _job_purpose = value; NotifyPropertyChanged("JOB_PURPOSE"); }
        }
        private decimal? _requisition_scanning_fileid;
        /// <summary>
        /// 通知单扫描件
        /// </summary>
        public decimal? REQUISITION_SCANNING_FILEID
        {
            get { return _requisition_scanning_fileid; }
            set { _requisition_scanning_fileid = value; NotifyPropertyChanged("REQUISITION_SCANNING_FILEID"); }
        }   
    }
}