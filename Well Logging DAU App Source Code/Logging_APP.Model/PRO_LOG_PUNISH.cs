using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_测井监督 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_PUNISH : ModelBase
    {
        private decimal? _punishid;
        /// <summary>
        /// 监督ID
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? PUNISHID
        {
            get { return _punishid; }
            set { _punishid = value; NotifyPropertyChanged("PUNISHID"); }
        }
        private string _job_plan_cd;
        /// <summary>
        /// 作业计划书编号
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string JOB_PLAN_CD
        {
            get { return _job_plan_cd; }
            set { _job_plan_cd = value; NotifyPropertyChanged("JOB_PLAN_CD"); }
        }
        private string _reason;
        /// <summary>
        /// 主要原因
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Char)]
        public string REASON
        {
            get { return _reason; }
            set { _reason = value; NotifyPropertyChanged("REASON"); }
        }
        private DateTime? _punish_date;
        /// <summary>
        /// 处罚日期
        /// </summary>
        public DateTime? PUNISH_DATE
        {
            get { return _punish_date; }
            set { _punish_date = value; NotifyPropertyChanged("PUNISH_DATE"); }
        }
        private string _punish_suggestion;
        /// <summary>
        /// 处罚意见
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Char)]
        public string PUNISH_SUGGESTION
        {
            get { return _punish_suggestion; }
            set { _punish_suggestion = value; NotifyPropertyChanged("PUNISH_SUGGESTION"); }
        }
        private string _is_rectify;
        /// <summary>
        /// 问题是否整改
        /// </summary>
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Char)]
        public string IS_RECTIFY
        {
            get { return _is_rectify; }
            set { _is_rectify = value; NotifyPropertyChanged("IS_RECTIFY"); }
        }
        private DateTime? _revise_date;
        /// <summary>
        /// 整改日期
        /// </summary>
        public DateTime? REVISE_DATE
        {
            get { return _revise_date; }
            set { _revise_date = value; NotifyPropertyChanged("REVISE_DATE"); }
        }
        private string _log_supervision_id;
        /// <summary>
        /// 测井监督处罚人
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOG_SUPERVISION_ID
        {
            get { return _log_supervision_id.TrimCharEnd(); }
            set { _log_supervision_id = value; NotifyPropertyChanged("LOG_SUPERVISION_ID"); }
        }
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Char)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }
        private decimal? _down_well_sequence;
        /// <summary>
        /// 下井趟次号
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? DOWN_WELL_SEQUENCE
        {
            get { return _down_well_sequence; }
            set { _down_well_sequence = value; NotifyPropertyChanged("DOWN_WELL_SEQUENCE"); }
        }

    }
}