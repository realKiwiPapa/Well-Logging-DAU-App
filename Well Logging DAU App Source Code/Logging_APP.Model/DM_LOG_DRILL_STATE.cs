using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 测井每个星期跟踪的钻井动态，从钻井的班报表取钻井深度 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_DRILL_STATE : ModelBase
    {
        private string _drill_well_id;
        /// <summary>
        /// 钻井动态ID
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
        private decimal? _last_week_md;
        /// <summary>
        /// 上周井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? LAST_WEEK_MD
        {
            get { return _last_week_md; }
            set { _last_week_md = value; NotifyPropertyChanged("LAST_WEEK_MD"); }
        }
        private decimal? _current_week_md;
        /// <summary>
        /// 接通知时间
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? CURRENT_WEEK_MD
        {
            get { return _current_week_md; }
            set { _current_week_md = value; NotifyPropertyChanged("CURRENT_WEEK_MD"); }
        }
        private string _current_layer;
        /// <summary>
        /// 目前层位
        /// </summary>
        [OracleStringLength(MaximumLength = 64, CharUsed = CharUsedType.Byte)]
        public string CURRENT_LAYER
        {
            get { return _current_layer.TrimChar(); }
            set { _current_layer = value; NotifyPropertyChanged("CURRENT_LAYER"); }
        }
        private DateTime? _create_date;
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? CREATE_DATE
        {
            get { return _create_date; }
            set { _create_date = value; NotifyPropertyChanged("CREATE_DATE"); }
        }
        private string _create_man;
        /// <summary>
        /// 更新人
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