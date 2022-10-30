using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 钻井地质设计预测项目 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_GEO_DES_ITEM : ModelBase
    {
        private string _drill_well_id;
        /// <summary>
        /// 设计预项目ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string DRILL_WELL_ID
        {
            get { return _drill_well_id; }
            set { _drill_well_id = value; NotifyPropertyChanged("DRILL_WELL_ID"); }
        }
        private string _drill_job_id;
        /// <summary>
        /// 作业项目ID
        /// </summary>
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string DRILL_JOB_ID
        {
            get { return _drill_job_id; }
            set { _drill_job_id = value; NotifyPropertyChanged("DRILL_JOB_ID"); }
        }
        private decimal? _log_md;
        /// <summary>
        /// 测时井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? LOG_MD
        {
            get { return _log_md; }
            set { _log_md = value; NotifyPropertyChanged("LOG_MD"); }
        }
        private string _log_interval;
        /// <summary>
        /// 测量井段
        /// </summary>
        [OracleStringLength(MaximumLength = 64, CharUsed = CharUsedType.Byte)]
        public string LOG_INTERVAL
        {
            get { return _log_interval.TrimChar(); }
            set { _log_interval = value; NotifyPropertyChanged("LOG_INTERVAL"); }
        }
        private string _log_layer;
        /// <summary>
        /// 测量层位
        /// </summary>
        [OracleStringLength(MaximumLength = 64, CharUsed = CharUsedType.Byte)]
        public string LOG_LAYER
        {
            get { return _log_layer.TrimChar(); }
            set { _log_layer = value; NotifyPropertyChanged("LOG_LAYER"); }
        }
        private string _log_item;
        /// <summary>
        /// 测井项目
        /// </summary>
        [OracleStringLength(MaximumLength = 1024, CharUsed = CharUsedType.Byte)]
        public string LOG_ITEM
        {
            get { return _log_item; }
            set { _log_item = value; NotifyPropertyChanged("LOG_ITEM"); }
        }
        private string _log_scale;
        /// <summary>
        /// 比例
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_SCALE
        {
            get { return _log_scale.TrimChar(); }
            set { _log_scale = value; NotifyPropertyChanged("LOG_SCALE"); }
        }
        private DateTime? _create_date;
        /// <summary>
        /// 录入日期
        /// </summary>
        public DateTime? CREATE_DATE
        {
            get { return _create_date; }
            set { _create_date = value; NotifyPropertyChanged("CREATE_DATE"); }
        }
        private string _create_man;
        /// <summary>
        /// 录入人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string CREATE_MAN
        {
            get { return _create_man.TrimChar(); }
            set { _create_man = value; NotifyPropertyChanged("CREATE_MAN"); }
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