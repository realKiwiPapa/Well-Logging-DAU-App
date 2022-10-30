using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    [Serializable]
    public class JobInfoSearch : ModelBase
    {
        private string _well_job_name;
        /// <summary>
        /// 测井作业井名
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string WELL_JOB_NAME
        {
            get { return _well_job_name; }
            set { _well_job_name = value.TrimChar(); NotifyPropertyChanged("WELL_JOB_NAME"); }
        }
        private string _activity_name;
        /// <summary>
        /// 项目名称
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string ACTIVITY_NAME
        {
            get { return _activity_name; }
            set { _activity_name = value; NotifyPropertyChanged("ACTIVITY_NAME"); }
        }
        private string _well_type;
        /// <summary>
        /// 井型
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string WELL_TYPE
        {
            get { return _well_type; }
            set { _well_type = value; NotifyPropertyChanged("WELL_TYPE"); }
        }
        private string _part_units;
        /// <summary>
        /// 甲方单位（业主单位）
        /// </summary>
        [OracleStringLength(MaximumLength = 33, CharUsed = CharUsedType.Byte)]
        public string PART_UNITS
        {
            get { return _part_units; }
            set { _part_units = value; NotifyPropertyChanged("PART_UNITS"); }
        }
        private int _drill_state=-1;
        /// <summary>
        /// 钻井动态
        /// </summary>
        public int DRILL_STATE
        {
            get { return _drill_state; }
            set { _drill_state = value; NotifyPropertyChanged("DRILL_STATE"); }
        }
    }
}
