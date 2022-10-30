using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_作业明细 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_WORK_DETAILS : ModelBase
    {
        private string _detailsid;
        /// <summary>
        /// 明细ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string DETAILSID
        {
            get { return _detailsid; }
            set { _detailsid = value; }
        }
        private string _job_plan_cd;
        /// <summary>
        /// 作业计划书编号
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string JOB_PLAN_CD
        {
            get { return _job_plan_cd; }
            set { _job_plan_cd = value; }
        }
        private decimal? _block_nunber;
        /// <summary>
        /// 原始数据块序号
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? BLOCK_NUNBER
        {
            get { return _block_nunber; }
            set { _block_nunber = value; }
        }
        private string _log_origianl_data_id;
        /// <summary>
        /// 测井原始数据文件ID
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string LOG_ORIGIANL_DATA_ID
        {
            get { return _log_origianl_data_id; }
            set { _log_origianl_data_id = value; }
        }
        private decimal? _down_well_sequence;
        /// <summary>
        /// 下井趟次号
        /// </summary>
        [Required]
        [Range(1,999)]
        //[OracleNumberLength(MaximumLength = 38)]
        public decimal? DOWN_WELL_SEQUENCE
        {
            get { return _down_well_sequence; }
            set { _down_well_sequence = value; NotifyPropertyChanged("DOWN_WELL_SEQUENCE"); }
        }
        private string _is_add;
        /// <summary>
        /// 是否加测
        /// </summary>
        [OracleStringLength(MaximumLength = 1, CharUsed = CharUsedType.Byte)]
        public string IS_ADD
        {
            get { return _is_add; }
            set { _is_add = value; }
        }
        private string _combination_name;
        /// <summary>
        /// 组合名称
        /// </summary>
        [OracleStringLength(MaximumLength = 256, CharUsed = CharUsedType.Byte)]
        public string COMBINATION_NAME
        {
            get { return _combination_name; }
            set { _combination_name = value; }
        }
        private DateTime? _start_time;
        /// <summary>
        /// 仪器入井时间
        /// </summary>
        public DateTime? START_TIME
        {
            get { return _start_time; }
            set { _start_time = value; }
        }
        private DateTime? _end_time;
        /// <summary>
        /// 仪器出井时间
        /// </summary>
        public DateTime? END_TIME
        {
            get { return _end_time; }
            set { _end_time = value; }
        }
        private decimal? _work_hours;
        /// <summary>
        /// 仪器运行时间
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? WORK_HOURS
        {
            get { return _work_hours; }
            set { _work_hours = value; }
        }
        private string _if_success;
        /// <summary>
        /// 是否成功
        /// </summary>
        [OracleStringLength(MaximumLength = 1, CharUsed = CharUsedType.Byte)]
        public string IF_SUCCESS
        {
            get { return _if_success; }
            set { _if_success = value; }
        }
        private decimal? _measure_well_to;
        /// <summary>
        /// 测量顶深
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? MEASURE_WELL_TO
        {
            get { return _measure_well_to; }
            set { _measure_well_to = value; }
        }
        private decimal? _measure_well_from;
        /// <summary>
        /// 测量底深
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? MEASURE_WELL_FROM
        {
            get { return _measure_well_from; }
            set { _measure_well_from = value; }
        }
        private string _well_section;
        /// <summary>
        /// 重复测量井段
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WELL_SECTION
        {
            get { return _well_section; }
            set { _well_section = value; }
        }
        private decimal? _powersupply_voltage;
        /// <summary>
        /// 供电电压
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? POWERSUPPLY_VOLTAGE
        {
            get { return _powersupply_voltage; }
            set { _powersupply_voltage = value; }
        }
        private decimal? _cablehead_voltage;
        /// <summary>
        /// 缆头电压
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? CABLEHEAD_VOLTAGE
        {
            get { return _cablehead_voltage; }
            set { _cablehead_voltage = value; }
        }
        private decimal? _current;
        /// <summary>
        /// 电流
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? CURRENT
        {
            get { return _current; }
            set { _current = value; }
        }
        private decimal? _largest_cable_tension;
        /// <summary>
        /// 最大电缆拉力
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? LARGEST_CABLE_TENSION
        {
            get { return _largest_cable_tension; }
            set { _largest_cable_tension = value; }
        }
        private decimal? _largest_cablehead_tension;
        /// <summary>
        /// 最大缆头拉力
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? LARGEST_CABLEHEAD_TENSION
        {
            get { return _largest_cablehead_tension; }
            set { _largest_cablehead_tension = value; }
        }
        private string _data_name;
        /// <summary>
        /// 数据文件名
        /// </summary>
        [OracleStringLength(MaximumLength = 128, CharUsed = CharUsedType.Byte)]
        public string DATA_NAME
        {
            get { return _data_name; }
            set { _data_name = value; }
        }
        private decimal? _data_size;
        /// <summary>
        /// 数据体大小
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? DATA_SIZE
        {
            get { return _data_size; }
            set { _data_size = value; }
        }
        private decimal? _data_block_num;
        /// <summary>
        /// 数据体块数
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? DATA_BLOCK_NUM
        {
            get { return _data_block_num; }
            set { _data_block_num = value; }
        }
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 256, CharUsed = CharUsedType.Byte)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; }
        }
        private string _filename;
        /// <summary>
        /// 文件名
        /// </summary>
        //[OracleStringLength(MaximumLength = 2000, CharUsed = CharUsedType.Char)]
        public string FILENAME
        {
            get { return _filename; }
            set { _filename = value; }
        }

        private string _log_series_name;
        /// <summary>
        /// 测井系列
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_SERIES_NAME
        {
            get { return _log_series_name; }
            set { _log_series_name = value; }
        }
    }
}