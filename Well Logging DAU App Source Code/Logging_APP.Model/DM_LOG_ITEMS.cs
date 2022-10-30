using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 测井项目 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_ITEMS : ModelBase
    {
        private string _job_plan_cd;
        /// <summary>
        /// 计划任务书编号
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string JOB_PLAN_CD
        {
            get { return _job_plan_cd; }
            set { _job_plan_cd = value;}
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
        private string _logging_name;
        /// <summary>
        /// 项目名称
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string LOGGING_NAME
        {
            get { return _logging_name; }
            set { _logging_name = value;}
        }
        private decimal? _item_flag;
        /// <summary>
        /// 项目标识
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? ITEM_FLAG
        {
            get { return _item_flag; }
            set { _item_flag = value;}
        }
        private decimal? _st_dep;
        /// <summary>
        /// 起始深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? ST_DEP
        {
            get { return _st_dep; }
            set { _st_dep = value; NotifyPropertyChanged("ST_DEP"); }
        }
        private decimal? _en_dep;
        /// <summary>
        /// 结束深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? EN_DEP
        {
            get { return _en_dep; }
            set { _en_dep = value; NotifyPropertyChanged("EN_DEP"); }
        }
        private string _scale;
        /// <summary>
        /// 比例
        /// </summary>
        [OracleStringLength(MaximumLength = 20, CharUsed = CharUsedType.Byte)]
        public string SCALE
        {
            get { return _scale; }
            set { _scale = value; NotifyPropertyChanged("SCALE"); }
        }
        private decimal? _rlev;
        /// <summary>
        /// 采用间距
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? RLEV
        {
            get { return _rlev; }
            set { _rlev = value; NotifyPropertyChanged("RLEV"); }
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
        private decimal? _log_item_id;
        /// <summary>
        /// 测井项目ID
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? LOG_ITEM_ID
        {
            get { return _log_item_id; }
            set { _log_item_id = value; NotifyPropertyChanged("LOG_ITEM_ID"); }
        }

    }
}