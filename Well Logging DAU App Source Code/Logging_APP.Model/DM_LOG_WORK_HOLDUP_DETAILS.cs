using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_遇阻情况 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_WORK_HOLDUP_DETAILS : ModelBase
    {
        private string _holdupid;
        /// <summary>
        /// 遇阻ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string HOLDUPID
        {
            get { return _holdupid; }
            set { _holdupid = value; }
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
            set { _job_plan_cd = value; }
        }
        private decimal? _down_well_sequence;
        /// <summary>
        /// 下井趟次号
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? DOWN_WELL_SEQUENCE
        {
            get { return _down_well_sequence; }
            set { _down_well_sequence = value; }
        }
        private DateTime? _holdup_date;
        /// <summary>
        /// 遇阻日期
        /// </summary>
        public DateTime? HOLDUP_DATE
        {
            get { return _holdup_date; }
            set { _holdup_date = value; }
        }
        private string _holdup_md;
        /// <summary>
        /// 遇阻深度
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string HOLDUP_MD
        {
            get { return _holdup_md; }
            set { _holdup_md = value; }
        }
        private string _holdup_type;
        /// <summary>
        /// 遇阻类型
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string HOLDUP_TYPE
        {
            get { return _holdup_type; }
            set { _holdup_type = value; }
        }
        private string _wb_obstruction_id;
        /// <summary>
        /// 井筒障碍物号
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WB_OBSTRUCTION_ID
        {
            get { return _wb_obstruction_id; }
            set { _wb_obstruction_id = value; }
        }
        private string _obstruction_desc;
        /// <summary>
        /// 障碍物描述
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string OBSTRUCTION_DESC
        {
            get { return _obstruction_desc; }
            set { _obstruction_desc = value; }
        }
        private decimal? _tool_diameter;
        /// <summary>
        /// 工具直径
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? TOOL_DIAMETER
        {
            get { return _tool_diameter; }
            set { _tool_diameter = value; }
        }

    }
}