using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_现场快速解释基本信息 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_RAPID_INFO : ModelBase
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
        private string _drill_job_id;
        /// <summary>
        /// 钻井作业ID
        /// </summary>
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string DRILL_JOB_ID
        {
            get { return _drill_job_id; }
            set { _drill_job_id = value; NotifyPropertyChanged("DRILL_JOB_ID"); }
        }
        private string _job_plan_cd;
        /// <summary>
        /// 作业计划书编号
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string JOB_PLAN_CD
        {
            get { return _job_plan_cd; }
            set { _job_plan_cd = value; NotifyPropertyChanged("JOB_PLAN_CD"); }
        }
        private string _log_team;
        /// <summary>
        /// 作业小队
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_TEAM
        {
            get { return _log_team; }
            set { _log_team = value; NotifyPropertyChanged("LOG_TEAM"); }
        }
        private string _log_mode;
        /// <summary>
        /// 测井方式
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_MODE
        {
            get { return _log_mode; }
            set { _log_mode = value; NotifyPropertyChanged("LOG_MODE"); }
        }
        private string _log_server_id;
        /// <summary>
        /// 测井系列
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_SERVER_ID
        {
            get { return _log_server_id; }
            set { _log_server_id = value; NotifyPropertyChanged("LOG_SERVER_ID"); }
        }
        private decimal? _bottom_dep;
        /// <summary>
        /// 井底深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? BOTTOM_DEP
        {
            get { return _bottom_dep; }
            set { _bottom_dep = value; NotifyPropertyChanged("BOTTOM_DEP"); }
        }
        private decimal? _bottom_temperature;
        /// <summary>
        /// 井底温度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? BOTTOM_TEMPERATURE
        {
            get { return _bottom_temperature; }
            set { _bottom_temperature = value; NotifyPropertyChanged("BOTTOM_TEMPERATURE"); }
        }
        private decimal? _mud_resitivity;
        /// <summary>
        /// 钻井液电阻率
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? MUD_RESITIVITY
        {
            get { return _mud_resitivity; }
            set { _mud_resitivity = value; NotifyPropertyChanged("MUD_RESITIVITY"); }
        }
        private decimal? _mud_temerature;
        /// <summary>
        /// 钻井液温度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? MUD_TEMERATURE
        {
            get { return _mud_temerature; }
            set { _mud_temerature = value; NotifyPropertyChanged("MUD_TEMERATURE"); }
        }
        private string _submit_man;
        /// <summary>
        /// 成果提交人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string SUBMIT_MAN
        {
            get { return _submit_man; }
            set { _submit_man = value; NotifyPropertyChanged("SUBMIT_MAN"); }
        }
        private DateTime? _submit_date;
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime? SUBMIT_DATE
        {
            get { return _submit_date; }
            set { _submit_date = value; NotifyPropertyChanged("SUBMIT_DATE"); }
        }
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }
        private decimal? _bottom_pressure;
        /// <summary>
        /// 井底压力
        /// </summary>
        [OracleNumberLength(MaximumLength=8)]
        public decimal? BOTTOM_PRESSURE
        {
            get { return _bottom_pressure; }
            set { _bottom_pressure = value; NotifyPropertyChanged("BOTTOM_PRESSURE"); }
        }
        private decimal? _ground_temperature;
        /// <summary>
        /// 地面温度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? GROUND_TEMPERATURE
        {
            get { return _ground_temperature; }
            set { _ground_temperature = value; NotifyPropertyChanged("GROUND_TEMPERATURE"); }
        }
    }
}