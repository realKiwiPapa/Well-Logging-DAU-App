using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_远程指导 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_REMOTE_DIRECT : ModelBase
    {
        private decimal? _directid;
        /// <summary>
        /// 指导ID
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? DIRECTID
        {
            get { return _directid; }
            set { _directid = value; NotifyPropertyChanged("DIRECTID"); }
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
        private string _expertise;
        /// <summary>
        /// 专家意见
        /// </summary>
        [OracleStringLength(MaximumLength = 2000, CharUsed = CharUsedType.Char)]
        public string EXPERTISE
        {
            get { return _expertise; }
            set { _expertise = value; NotifyPropertyChanged("EXPERTISE"); }
        }
        private string _expert_leader;
        /// <summary>
        /// 专家组长
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Char)]
        public string EXPERT_LEADER
        {
            get { return _expert_leader; }
            set { _expert_leader = value; NotifyPropertyChanged("EXPERT_LEADER"); }
        }
        private DateTime? _direct_date;
        /// <summary>
        /// 指导日期
        /// </summary>
        public DateTime? DIRECT_DATE
        {
            get { return _direct_date; }
            set { _direct_date = value; NotifyPropertyChanged("DIRECT_DATE"); }
        }
        private string _direct_means;
        /// <summary>
        /// 指导方式
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string DIRECT_MEANS
        {
            get { return _direct_means; }
            set { _direct_means = value; NotifyPropertyChanged("DIRECT_MEANS"); }
        }
        private string _expertise_receive_man;
        /// <summary>
        /// 专家意见接收人
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Char)]
        public string EXPERTISE_RECEIVE_MAN
        {
            get { return _expertise_receive_man; }
            set { _expertise_receive_man = value; NotifyPropertyChanged("EXPERTISE_RECEIVE_MAN"); }
        }
        private string _expertise_effect;
        /// <summary>
        /// 专家意见执行效果
        /// </summary>
        [OracleStringLength(MaximumLength = 1000, CharUsed = CharUsedType.Char)]
        public string EXPERTISE_EFFECT
        {
            get { return _expertise_effect; }
            set { _expertise_effect = value; NotifyPropertyChanged("EXPERTISE_EFFECT"); }
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

    }
}