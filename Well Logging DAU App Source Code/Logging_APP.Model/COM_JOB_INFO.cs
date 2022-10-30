using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 为测井专业的测井作业井名基本信息表，也就是测井作业的井名 modle
    /// </summary>
    [Serializable]
    public class COM_JOB_INFO : ModelBase
    {
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
        private string _well_id;
        /// <summary>
        /// 井ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Byte)]
        public string WELL_ID
        {
            get { return _well_id; }
            set { _well_id = value; NotifyPropertyChanged("WELL_ID"); }
        }
        private string _well_id_a7;
        /// <summary>
        /// A7井ID
        /// </summary>
        [OracleStringLength(MaximumLength = 30, CharUsed = CharUsedType.Byte)]
        public string WELL_ID_A7
        {
            get { return _well_id_a7; }
            set { _well_id_a7 = value; NotifyPropertyChanged("WELL_ID_A7"); }
        }
        private string _well_id_a1;
        /// <summary>
        /// A1井ID
        /// </summary>
        [OracleStringLength(MaximumLength = 30, CharUsed = CharUsedType.Byte)]
        public string WELL_ID_A1
        {
            get { return _well_id_a1; }
            set { _well_id_a1 = value; NotifyPropertyChanged("WELL_ID_A1"); }
        }
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
        private string _field;
        /// <summary>
        /// 井所属区域
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string FIELD
        {
            get { return _field; }
            set { _field = value; NotifyPropertyChanged("FIELD"); }
        }
        private string _well_the_market;
        /// <summary>
        /// 井的投资单位，如勘探事业部、开发事业部、西气东输项目部等
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string WELL_THE_MARKET
        {
            get { return _well_the_market; }
            set { _well_the_market = value; NotifyPropertyChanged("WELL_THE_MARKET"); }
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
        private string _well_sort;
        /// <summary>
        /// 井别
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string WELL_SORT
        {
            get { return _well_sort; }
            set { _well_sort = value; NotifyPropertyChanged("WELL_SORT"); }
        }
        private string _true_completion_formation;
        /// <summary>
        /// 设计完钻层位
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string TRUE_COMPLETION_FORMATION
        {
            get { return _true_completion_formation; }
            set { _true_completion_formation = value; NotifyPropertyChanged("TRUE_COMPLETION_FORMATION"); }
        }
        private string _complete_method;
        /// <summary>
        /// 设计完井方法
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string COMPLETE_METHOD
        {
            get { return _complete_method; }
            set { _complete_method = value; NotifyPropertyChanged("COMPLETE_METHOD"); }
        }
        private string _job_purpose;
        /// <summary>
        /// 作业目的
        /// </summary>
        [OracleStringLength(MaximumLength = 2000, CharUsed = CharUsedType.Byte)]
        public string JOB_PURPOSE
        {
            get { return _job_purpose; }
            set { _job_purpose = value; NotifyPropertyChanged("JOB_PURPOSE"); }
        }
        private string _drill_unit;
        /// <summary>
        /// 钻井单位
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string DRILL_UNIT
        {
            get { return _drill_unit; }
            set { _drill_unit = value; NotifyPropertyChanged("DRILL_UNIT"); }
        }
        private string _design_horizon;
        /// <summary>
        /// 设计目的层
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string DESIGN_HORIZON
        {
            get { return _design_horizon; }
            set { _design_horizon = value; NotifyPropertyChanged("DESIGN_HORIZON"); }
        }
        private decimal? _design_md;
        /// <summary>
        /// 设计钻达井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? DESIGN_MD
        {
            get { return _design_md; }
            set { _design_md = value; NotifyPropertyChanged("DESIGN_MD"); }
        }
        /// <summary>
        /// 完钻井深
        /// </summary>
        private decimal? _welldone_dep;

        public decimal? WELLDONE_DEP
        {
            get { return _welldone_dep; }
            set { _welldone_dep = value; }
        }
        /// <summary>
        /// 开钻日期
        /// </summary>
        private DateTime? _start_well_date;

        public DateTime? START_WELL_DATE
        {
            get { return _start_well_date; }
            set { _start_well_date = value; }
        }
        /// <summary>
        /// 完钻日期
        /// </summary>
        private DateTime? _end_well_date;

        public DateTime? END_WELL_DATE
        {
            get { return _end_well_date; }
            set { _end_well_date = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        private string _note;

        public string NOTE
        {
            get { return _note; }
            set { _note = value; }
        }
    }
}