using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_放射源使用情况 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_RADIATION_STATUS : ModelBase
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
            set { _job_plan_cd = value; }
        }
        private string _radiation_no;
        /// <summary>
        /// 源号
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string RADIATION_NO
        {
            get { return _radiation_no; }
            set { _radiation_no = value; NotifyPropertyChanged("RADIATION_NO"); }
        }
        private string _radiation_cd;
        /// <summary>
        /// 源自编号
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string RADIATION_CD
        {
            get { return _radiation_cd; }
            set { _radiation_cd = value; NotifyPropertyChanged("RADIATION_CD"); }
        }
        private string _element;
        /// <summary>
        /// 源核素
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string ELEMENT
        {
            get { return _element; }
            set { _element = value; NotifyPropertyChanged("ELEMENT"); }
        }
        private string _active;
        /// <summary>
        /// 购进活度
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string ACTIVE
        {
            get { return _active; }
            set { _active = value; NotifyPropertyChanged("ACTIVE"); }
        }
        private string _load_person;
        /// <summary>
        /// 装源人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOAD_PERSON
        {
            get { return _load_person; }
            set { _load_person = value; }
        }
        private string _unload_person;
        /// <summary>
        /// 卸源人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string UNLOAD_PERSON
        {
            get { return _unload_person; }
            set { _unload_person = value; }
        }
        private decimal? _under_well_time;
        /// <summary>
        /// 井下工作时间
        /// </summary>
        [OracleNumberLength(MaximumLength = 6)]
        public decimal? UNDER_WELL_TIME
        {
            get { return _under_well_time; }
            set { _under_well_time = value; }
        }
        private decimal? _max_work_pressure;
        /// <summary>
        /// 最高工作压力
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? MAX_WORK_PRESSURE
        {
            get { return _max_work_pressure; }
            set { _max_work_pressure = value; }
        }
        private decimal? _max_work_temperature;
        /// <summary>
        /// 最高工作温度
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? MAX_WORK_TEMPERATURE
        {
            get { return _max_work_temperature; }
            set { _max_work_temperature = value; }
        }
        private string _replace_sourcepacking;
        /// <summary>
        /// 是否更换源盘根
        /// </summary>
        [OracleStringLength(MaximumLength = 6, CharUsed = CharUsedType.Byte)]
        public string REPLACE_SOURCEPACKING
        {
            get { return _replace_sourcepacking; }
            set { _replace_sourcepacking = value; }
        }
        private decimal? _replace_sourcepacking_num;
        /// <summary>
        /// 更换源盘根数量
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? REPLACE_SOURCEPACKING_NUM
        {
            get { return _replace_sourcepacking_num; }
            set { _replace_sourcepacking_num = value; }
        }
        private DateTime? _replace_date;
        /// <summary>
        /// 更换日期
        /// </summary>
        public DateTime? REPLACE_DATE
        {
            get { return _replace_date; }
            set { _replace_date = value; }
        }
        private string _replace_place;
        /// <summary>
        /// 更换地点
        /// </summary>
        [OracleStringLength(MaximumLength = 64, CharUsed = CharUsedType.Byte)]
        public string REPLACE_PLACE
        {
            get { return _replace_place; }
            set { _replace_place = value; }
        }
        private string _replace_person;
        /// <summary>
        /// 更换人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string REPLACE_PERSON
        {
            get { return _replace_person; }
            set { _replace_person = value; }
        }
        private string _radiationid;
        /// <summary>
        /// 放射ID
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string RADIATIONID
        {
            get { return _radiationid; }
            set { _radiationid = value; }
        }
        private decimal? _down_well_sequence;
        /// <summary>
        /// DOWN_WELL_SEQUENCE
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? DOWN_WELL_SEQUENCE
        {
            get { return _down_well_sequence; }
            set { _down_well_sequence = value; }
        }

    }
}