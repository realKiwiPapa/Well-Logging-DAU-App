using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 多方联席会 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_CONTACT_WILL : ModelBase
    {
        private string _contactid;
        /// <summary>
        /// 联系会ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Byte)]
        public string CONTACTID
        {
            get { return _contactid.TrimCharEnd(); }
            set { _contactid = value; }
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
        private string _a_punish;
        /// <summary>
        /// 地质监督
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string A_PUNISH
        {
            get { return _a_punish.TrimCharEnd(); }
            set { _a_punish = value; }
        }
        private string _rrside_site_punish;
        /// <summary>
        /// 工程监督
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string RRSIDE_SITE_PUNISH
        {
            get { return _rrside_site_punish.TrimCharEnd(); }
            set { _rrside_site_punish = value; }
        }
        private string _drill_crew_chief;
        /// <summary>
        /// 钻井队负责人
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string DRILL_CREW_CHIEF
        {
            get { return _drill_crew_chief.TrimCharEnd(); }
            set { _drill_crew_chief = value; }
        }
        private string _log_team_leader;
        /// <summary>
        /// 测井小队队长
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOG_TEAM_LEADER
        {
            get { return _log_team_leader.TrimCharEnd(); }
            set { _log_team_leader = value; }
        }
        private string _log_operator__name;
        /// <summary>
        /// 测井小队操作员
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_OPERATOR__NAME
        {
            get { return _log_operator__name; }
            set { _log_operator__name = value; }
        }
        private string _log_safe_name;
        /// <summary>
        /// 测井小队安全员
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOG_SAFE_NAME
        {
            get { return _log_safe_name.TrimCharEnd(); }
            set { _log_safe_name = value; }
        }
        private string _log_land_leader;
        /// <summary>
        /// 测井小队地面组长
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOG_LAND_LEADER
        {
            get { return _log_land_leader.TrimCharEnd(); }
            set { _log_land_leader = value; }
        }
        private string _log_company_punish;
        /// <summary>
        /// 测井公司安全监督
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOG_COMPANY_PUNISH
        {
            get { return _log_company_punish.TrimCharEnd(); }
            set { _log_company_punish = value; }
        }
        private string _log_company_leader;
        /// <summary>
        /// 测井公司领导
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOG_COMPANY_LEADER
        {
            get { return _log_company_leader.TrimCharEnd(); }
            set { _log_company_leader = value; }
        }
        private string _meeting_sum;
        /// <summary>
        /// 会议纪要
        /// </summary>
        [OracleStringLength(MaximumLength = 500, CharUsed = CharUsedType.Byte)]
        public string MEETING_SUM
        {
            get { return _meeting_sum.TrimCharEnd(); }
            set { _meeting_sum = value; }
        }
        private string _writer;
        /// <summary>
        /// 记录人
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string WRITER
        {
            get { return _writer.TrimCharEnd(); }
            set { _writer = value; }
        }
        private DateTime? _meeting_date;
        /// <summary>
        /// 会议日期
        /// </summary>
        public DateTime? MEETING_DATE
        {
            get { return _meeting_date; }
            set { _meeting_date = value; }
        }
        private decimal? _meeting_time;
        /// <summary>
        /// 会议时间
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? MEETING_TIME
        {
            get { return _meeting_time; }
            set { _meeting_time = value; }
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
        private string _b_punish;
        /// <summary>
        /// 川庆安全监督
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string B_PUNISH
        {
            get { return _b_punish.TrimCharEnd(); }
            set { _b_punish = value; }
        }
        private string _mud_crew_chief;
        /// <summary>
        /// 录井负责人
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string MUD_CREW_CHIEF
        {
            get { return _mud_crew_chief.TrimCharEnd(); }
            set { _mud_crew_chief = value; }
        }
        private string _solp_crew_chief;
        /// <summary>
        /// 泥浆负责人
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string SOLP_CREW_CHIEF
        {
            get { return _solp_crew_chief.TrimCharEnd(); }
            set { _solp_crew_chief = value; }
        }
        private string _log_interpretation;
        /// <summary>
        /// 测井解释负责人
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOG_INTERPRETATION
        {
            get { return _log_interpretation.TrimCharEnd(); }
            set { _log_interpretation = value; }
        }

    }
}