using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_井下设备 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_DOWNHOLE_EQUIP : ModelBase
    {
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
        private string _equipid;
        /// <summary>
        /// 设备ID
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string EQUIPID
        {
            get { return _equipid; }
            set { _equipid = value; NotifyPropertyChanged("EQUIPID"); }
        }
        private string _deviceaccount_name;
        /// <summary>
        /// 设备名称
        /// </summary>
        [OracleStringLength(MaximumLength = 64, CharUsed = CharUsedType.Byte)]
        public string DEVICEACCOUNT_NAME
        {
            get { return _deviceaccount_name; }
            set { _deviceaccount_name = value; NotifyPropertyChanged("DEVICEACCOUNT_NAME"); }
        }
        private string _deviceaccount_id;
        /// <summary>
        /// 设备编号
        /// </summary>
        [OracleStringLength(MaximumLength = 64, CharUsed = CharUsedType.Byte)]
        public string DEVICEACCOUNT_ID
        {
            get { return _deviceaccount_id; }
            set { _deviceaccount_id = value; NotifyPropertyChanged("DEVICEACCOUNT_ID"); }
        }
        private string _team;
        /// <summary>
        /// 所在班组
        /// </summary>
        [OracleStringLength(MaximumLength = 64, CharUsed = CharUsedType.Byte)]
        public string TEAM
        {
            get { return _team; }
            set { _team = value; NotifyPropertyChanged("TEAM"); }
        }
        private string _working_state;
        /// <summary>
        /// 工作状态
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WORKING_STATE
        {
            get { return _working_state; }
            set { _working_state = value; NotifyPropertyChanged("WORKING_STATE"); }
        }
        private string _fault_description;
        /// <summary>
        /// 故障描述
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string FAULT_DESCRIPTION
        {
            get { return _fault_description; }
            set { _fault_description = value; NotifyPropertyChanged("FAULT_DESCRIPTION"); }
        }
        private decimal? _down_well_sequence;
        /// <summary>
        /// 下井趟次号
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? DOWN_WELL_SEQUENCE
        {
            get { return _down_well_sequence; }
            set { _down_well_sequence = value;}
        }

    }
}