using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 解释处理作业 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_PROCESS : ModelBase
    {
        private string _process_id;
        /// <summary>
        /// 解释处理作业ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string PROCESS_ID
        {
            get { return _process_id; }
            set { _process_id = value; NotifyPropertyChanged("PROCESS_ID"); }
        }
        private string _log_data_id;
        /// <summary>
        /// 测井源数据ID
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string LOG_DATA_ID
        {
            get { return _log_data_id; }
            set { _log_data_id = value; NotifyPropertyChanged("LOG_DATA_ID"); }
        }
        private string _well_id;
        /// <summary>
        /// 井ID
        /// </summary>
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Byte)]
        public string WELL_ID
        {
            get { return _well_id.TrimCharEnd(); }
            set { _well_id = value; NotifyPropertyChanged("WELL_ID"); }
        }
        private string _drill_job_id;
        /// <summary>
        /// 作业项目ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string DRILL_JOB_ID
        {
            get { return _drill_job_id; }
            set { _drill_job_id = value; NotifyPropertyChanged("DRILL_JOB_ID"); }
        }
        private string _process_name;
        /// <summary>
        /// 解释处理作业名
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string PROCESS_NAME
        {
            get { return _process_name; }
            set { _process_name = value; NotifyPropertyChanged("PROCESS_NAME"); }
        }
        private string _process_code;
        /// <summary>
        /// 解释处理作业编码
        /// </summary>
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Byte)]
        public string PROCESS_CODE
        {
            get { return _process_code.TrimCharEnd(); }
            set { _process_code = value; NotifyPropertyChanged("PROCESS_CODE"); }
        }
        private DateTime? _process_start_date;
        /// <summary>
        /// 接通知时间
        /// </summary>
        public DateTime? PROCESS_START_DATE
        {
            get { return _process_start_date; }
            set { _process_start_date = value; NotifyPropertyChanged("PROCESS_START_DATE"); }
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

    }
}