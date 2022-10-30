using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_基本信息 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_BASE : ModelBase
    {
        private string _baseid;
        /// <summary>
        /// 基本ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string BASEID
        {
            get { return _baseid; }
            set { _baseid = value; }
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
        private string _job_purpose;
        /// <summary>
        /// 作业目的
        /// </summary>
        [OracleStringLength(MaximumLength = 80, CharUsed = CharUsedType.Char)]
        public string JOB_PURPOSE
        {
            get { return _job_purpose; }
            set { _job_purpose = value; }
        }
        private string _collaboration_department;
        /// <summary>
        /// 协作单位
        /// </summary>
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Char)]
        public string COLLABORATION_DEPARTMENT
        {
            get { return _collaboration_department; }
            set { _collaboration_department = value; }
        }
        private string _construction_org;
        /// <summary>
        /// 结算单位
        /// </summary>
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Char)]
        public string CONSTRUCTION_ORG
        {
            get { return _construction_org; }
            set { _construction_org = value; }
        }
        private DateTime? _received_inform_time;
        /// <summary>
        /// 接通知时间
        /// </summary>
        public DateTime? RECEIVED_INFORM_TIME
        {
            get { return _received_inform_time; }
            set { _received_inform_time = value; }
        }
        private decimal? _p_xn_drill_depth;
        /// <summary>
        /// 钻达井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? P_XN_DRILL_DEPTH
        {
            get { return _p_xn_drill_depth; }
            set { _p_xn_drill_depth = value; }
        }
        private decimal? _p_xn_log_depth;
        /// <summary>
        /// 测时井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? P_XN_LOG_DEPTH
        {
            get { return _p_xn_log_depth; }
            set { _p_xn_log_depth = value; }
        }
        private string _job_layer;
        /// <summary>
        /// 作业层位
        /// </summary>
        [OracleStringLength(MaximumLength = 120, CharUsed = CharUsedType.Char)]
        public string JOB_LAYER
        {
            get { return _job_layer; }
            set { _job_layer = value; }
        }
        private string _job_profile_series;
        /// <summary>
        /// 作业剖面系列
        /// </summary>
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Char)]
        public string JOB_PROFILE_SERIES
        {
            get { return _job_profile_series; }
            set { _job_profile_series = value; }
        }
        private decimal? _maximum_slope;
        /// <summary>
        /// 最大斜度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? MAXIMUM_SLOPE
        {
            get { return _maximum_slope; }
            set { _maximum_slope = value; }
        }
        private decimal? _maximum_slope_depth;
        /// <summary>
        /// 最大斜度深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? MAXIMUM_SLOPE_DEPTH
        {
            get { return _maximum_slope_depth; }
            set { _maximum_slope_depth = value; }
        }
        private decimal? _mud_resitivity;
        /// <summary>
        /// 钻井液电阻率
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? MUD_RESITIVITY
        {
            get { return _mud_resitivity; }
            set { _mud_resitivity = value; }
        }
        private decimal? _mud_temerature;
        /// <summary>
        /// 钻井液温度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? MUD_TEMERATURE
        {
            get { return _mud_temerature; }
            set { _mud_temerature = value; }
        }
        private decimal? _bottom_temperature;
        /// <summary>
        /// 井底温度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? BOTTOM_TEMPERATURE
        {
            get { return _bottom_temperature; }
            set { _bottom_temperature = value; }
        }
        private decimal? _bottom_dep;
        /// <summary>
        /// 井底深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? BOTTOM_DEP
        {
            get { return _bottom_dep; }
            set { _bottom_dep = value; }
        }
        private string _wellbore_fluid;
        /// <summary>
        /// 井筒流体
        /// </summary>
        [OracleStringLength(MaximumLength = 40, CharUsed = CharUsedType.Byte)]
        public string WELLBORE_FLUID
        {
            get { return _wellbore_fluid; }
            set { _wellbore_fluid = value; }
        }
        private decimal? _start_depth;
        /// <summary>
        /// 测量起始深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? START_DEPTH
        {
            get { return _start_depth; }
            set { _start_depth = value; }
        }
        private decimal? _end_depth;
        /// <summary>
        /// 测量结束深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? END_DEPTH
        {
            get { return _end_depth; }
            set { _end_depth = value; }
        }
        private string _is_complete;
        /// <summary>
        /// 完成情况
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string IS_COMPLETE
        {
            get { return _is_complete; }
            set { _is_complete = value; }
        }
        private string _log_type;
        /// <summary>
        /// 测井类型
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_TYPE
        {
            get { return _log_type; }
            set { _log_type = value; }
        }
        private string _log_mode;
        /// <summary>
        /// 测井方式
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_MODE
        {
            get { return _log_mode; }
            set { _log_mode = value; }
        }
        private decimal? _instrument_count;
        /// <summary>
        /// 仪器总的下井次数
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? INSTRUMENT_COUNT
        {
            get { return _instrument_count; }
            set { _instrument_count = value; }
        }
        private decimal? _instrument_success;
        /// <summary>
        /// 仪器下井一次测井成功的次数
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? INSTRUMENT_SUCCESS
        {
            get { return _instrument_success; }
            set { _instrument_success = value; }
        }
        private string _wave_selection;
        /// <summary>
        /// 固井声幅正负波选择
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WAVE_SELECTION
        {
            get { return _wave_selection; }
            set { _wave_selection = value; }
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
        private string _drill_statuse;
        /// <summary>
        /// 钻井状态
        /// </summary>	
        [OracleStringLength(MaximumLength = 16, CharUsed = CharUsedType.Byte)]
        public string DRILL_STATUSE
        {
            get { return _drill_statuse; }
            set { _drill_statuse = value; }
        }
    }
}