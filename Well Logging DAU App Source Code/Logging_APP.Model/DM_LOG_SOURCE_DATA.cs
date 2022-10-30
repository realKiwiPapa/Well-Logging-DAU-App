using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 解释作业数据源信息表 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_SOURCE_DATA : ModelBase
    {
        private string _log_data_id;
        /// <summary>
        /// 测井源数据ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string LOG_DATA_ID
        {
            get { return _log_data_id; }
            set { _log_data_id = value; NotifyPropertyChanged("LOG_DATA_ID"); }
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
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 400, CharUsed = CharUsedType.Byte)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; NotifyPropertyChanged("Selected"); }
        }
    }
}