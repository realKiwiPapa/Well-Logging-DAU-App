using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_地面设备 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_UP_EQUIP : ModelBase
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
        private string _team_org_id;
        /// <summary>
        /// 测井队号
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string TEAM_ORG_ID
        {
            get { return _team_org_id; }
            set { _team_org_id = value; }
        }
        private string _log_series_id;
        /// <summary>
        /// 测井系列ID
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_SERIES_ID
        {
            get { return _log_series_id; }
            set { _log_series_id = value; }
        }
        private string _winch_licenceplate;
        /// <summary>
        /// 绞车牌照
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WINCH_LICENCEPLATE
        {
            get { return _winch_licenceplate; }
            set { _winch_licenceplate = value; }
        }
        private decimal? _winchc_able_length;
        /// <summary>
        /// 绞车电缆长度
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? WINCHC_ABLE_LENGTH
        {
            get { return _winchc_able_length; }
            set { _winchc_able_length = value; }
        }
        private string _loggingtruck_licenseplate;
        /// <summary>
        /// 工作车牌照
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOGGINGTRUCK_LICENSEPLATE
        {
            get { return _loggingtruck_licenseplate; }
            set { _loggingtruck_licenseplate = value; }
        }

    }
}