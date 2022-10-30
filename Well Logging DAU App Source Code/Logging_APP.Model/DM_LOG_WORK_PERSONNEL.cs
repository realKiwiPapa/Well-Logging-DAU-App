using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_人员 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_WORK_PERSONNEL : ModelBase
    {
        private string _personnel_id;
        /// <summary>
        /// 人员ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string PERSONNEL_ID
        {
            get { return _personnel_id; }
            set { _personnel_id = value; }
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
        private string _log_team_leader;
        /// <summary>
        /// 队长姓名
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_TEAM_LEADER
        {
            get { return _log_team_leader; }
            set { _log_team_leader = value; }
        }
        private string _contact_telephone;
        /// <summary>
        /// 联系电话
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string CONTACT_TELEPHONE
        {
            get { return _contact_telephone; }
            set { _contact_telephone = value; }
        }
        private string _log_operator__name;
        /// <summary>
        /// 测井操作员姓名
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_OPERATOR__NAME
        {
            get { return _log_operator__name; }
            set { _log_operator__name = value; }
        }
        private string _source_operator_name;
        /// <summary>
        /// 源操作员姓名
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string SOURCE_OPERATOR_NAME
        {
            get { return _source_operator_name; }
            set { _source_operator_name = value; }
        }
        private string _log_supervision_name;
        /// <summary>
        /// 测井监督姓名
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_SUPERVISION_NAME
        {
            get { return _log_supervision_name; }
            set { _log_supervision_name = value; }
        }
        private string _other_construction_persons;
        /// <summary>
        /// 其他施工人员姓名
        /// </summary>
        [OracleStringLength(MaximumLength = 1024, CharUsed = CharUsedType.Byte)]
        public string OTHER_CONSTRUCTION_PERSONS
        {
            get { return _other_construction_persons; }
            set { _other_construction_persons = value; }
        }
        private string _field_inspector;
        /// <summary>
        /// 现场验收员
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string FIELD_INSPECTOR
        {
            get { return _field_inspector; }
            set { _field_inspector = value; }
        }
        private string _indoor_inspector;
        /// <summary>
        /// 室内验收员
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string INDOOR_INSPECTOR
        {
            get { return _indoor_inspector; }
            set { _indoor_inspector = value; }
        }
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; }
        }

    }
}