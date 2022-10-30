using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_时效 modle
    /// </summary>
    [Serializable]
    public class DM_LOG_WORK_ANING : ModelBase
    {
        private decimal? _aningid;
        /// <summary>
        /// 时效ID
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? ANINGID
        {
            get { return _aningid; }
            set { _aningid = value; }
        }
        private string _requisition_cd;
        /// <summary>
        /// 通知单编码
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string REQUISITION_CD
        {
            get { return _requisition_cd; }
            set { _requisition_cd = value; }
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
        private DateTime? _received_inform_time;
        /// <summary>
        /// 接通知时间
        /// </summary>
        public DateTime? RECEIVED_INFORM_TIME
        {
            get { return _received_inform_time; }
            set { _received_inform_time = value; }
        }
        private DateTime? _requirements_time;
        /// <summary>
        /// 要求到井时间
        /// </summary>
        public DateTime? REQUIREMENTS_TIME
        {
            get { return _requirements_time; }
            set { _requirements_time = value; }
        }
        private DateTime? _arrive_time;
        /// <summary>
        /// 实际到井时间
        /// </summary>
        public DateTime? ARRIVE_TIME
        {
            get { return _arrive_time; }
            set { _arrive_time = value; }
        }
        private DateTime? _receiving_time;
        /// <summary>
        /// 实际接井时间
        /// </summary>
        public DateTime? RECEIVING_TIME
        {
            get { return _receiving_time; }
            set { _receiving_time = value; }
        }
        private DateTime? _hand_time;
        /// <summary>
        /// 交井时间
        /// </summary>
        public DateTime? HAND_TIME
        {
            get { return _hand_time; }
            set { _hand_time = value; }
        }
        private DateTime? _leave_time;
        /// <summary>
        /// 离开井场时间
        /// </summary>
        public DateTime? LEAVE_TIME
        {
            get { return _leave_time; }
            set { _leave_time = value; }
        }
        private decimal? _wait_time;
        /// <summary>
        /// 等待工具时间
        /// </summary>
        public decimal? WAIT_TIME
        {
            get { return _wait_time; }
            set { _wait_time = value; }
        }
        private DateTime? _log_start_time;
        /// <summary>
        /// 测井开始时间
        /// </summary>
        public DateTime? LOG_START_TIME
        {
            get { return _log_start_time; }
            set { _log_start_time = value; }
        }
        private DateTime? _log_end_time;
        /// <summary>
        /// 测井结束时间
        /// </summary>
        public DateTime? LOG_END_TIME
        {
            get { return _log_end_time; }
            set { _log_end_time = value; }
        }
        private decimal? _log_total_time;
        /// <summary>
        /// 测井总时间
        /// </summary>
        [OracleNumberLength(MaximumLength = 3)]
        public decimal? LOG_TOTAL_TIME
        {
            get { return _log_total_time; }
            set { _log_total_time = value; }
        }
        private decimal? _lost_time;
        /// <summary>
        /// 损失时间
        /// </summary>
        [OracleNumberLength(MaximumLength = 3)]
        public decimal? LOST_TIME
        {
            get { return _lost_time; }
            set { _lost_time = value; }
        }
        private decimal? _winch_running_time;
        /// <summary>
        /// 绞车运行时间
        /// </summary>
        public decimal? WINCH_RUNNING_TIME
        {
            get { return _winch_running_time; }
            set { _winch_running_time = value; }
        }
        private string _departure_point_;
        /// <summary>
        /// 出发地
        /// </summary>
        [OracleStringLength(MaximumLength = 256, CharUsed = CharUsedType.Byte)]
        public string DEPARTURE_POINT_
        {
            get { return _departure_point_; }
            set { _departure_point_ = value; }
        }
        private decimal? _unilateral_distance;
        /// <summary>
        /// 单程距离
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? UNILATERAL_DISTANCE
        {
            get { return _unilateral_distance; }
            set { _unilateral_distance = value; }
        }
        private DateTime? _departure_point_time;
        /// <summary>
        /// 出发地时间
        /// </summary>
        public DateTime? DEPARTURE_POINT_TIME
        {
            get { return _departure_point_time; }
            set { _departure_point_time = value; }
        }

        private string _return_point;
        /// <summary>
        /// 返回地
        /// </summary>
        [OracleStringLength(MaximumLength = 256, CharUsed = CharUsedType.Byte)]
        public string RETURN_POINT
        {
            get { return _return_point; }
            set { _return_point = value; }
        }

        private DateTime? _return_point_time;
        /// <summary>
        /// 返回地时间
        /// </summary>
        public DateTime? RETURN_POINT_TIME
        {
            get { return _return_point_time; }
            set { _return_point_time = value; }
        }
    }
}