using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 解释处理_数据发布 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_DATA_PUBLISH : ModelBase
    {
        private decimal _data_publish_id;
        /// <summary>
        /// 数据发布ID
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal DATA_PUBLISH_ID
        {
            get { return _data_publish_id; }
            set { _data_publish_id = value; NotifyPropertyChanged("DATA_PUBLISH_ID"); }
        }
        private string _process_id;
        /// <summary>
        /// 解释处理ID
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string PROCESS_ID
        {
            get { return _process_id; }
            set { _process_id = value; NotifyPropertyChanged("PROCESS_ID"); }
        }
        private string _interpret_center;
        /// <summary>
        /// 解释中心
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string INTERPRET_CENTER
        {
            get { return _interpret_center.TrimCharEnd(); }
            set { _interpret_center = value; NotifyPropertyChanged("INTERPRET_CENTER"); }
        }
        private DateTime? _log_start_time;
        /// <summary>
        /// 测井开始时间
        /// </summary>
        public DateTime? LOG_START_TIME
        {
            get { return _log_start_time; }
            set { _log_start_time = value; NotifyPropertyChanged("LOG_START_TIME"); }
        }
        private decimal? _log_total_time;
        /// <summary>
        /// 测井总时间
        /// </summary>
        [OracleNumberLength(MaximumLength = 3)]
        public decimal? LOG_TOTAL_TIME
        {
            get { return _log_total_time; }
            set { _log_total_time = value; NotifyPropertyChanged("LOG_TOTAL_TIME"); }
        }
        private decimal? _lost_time;
        /// <summary>
        /// 损失时间
        /// </summary>
        [OracleNumberLength(MaximumLength = 3)]
        public decimal? LOST_TIME
        {
            get { return _lost_time; }
            set { _lost_time = value; NotifyPropertyChanged("LOST_TIME"); }
        }
        private string _team_org_id;
        /// <summary>
        /// 测井队号
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string TEAM_ORG_ID
        {
            get { return _team_org_id; }
            set { _team_org_id = value; NotifyPropertyChanged("TEAM_ORG_ID"); }
        }
        private string _log_series_id;
        /// <summary>
        /// 测井系列ID
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_SERIES_ID
        {
            get { return _log_series_id; }
            set { _log_series_id = value; NotifyPropertyChanged("LOG_SERIES_ID"); }
        }
        private DateTime? _pro_start_time;
        /// <summary>
        /// 测井开始时间
        /// </summary>
        public DateTime? PRO_START_TIME
        {
            get { return _pro_start_time; }
            set { _pro_start_time = value; NotifyPropertyChanged("PRO_START_TIME"); }
        }
        private decimal? _p_total_time;
        /// <summary>
        /// 处理总时间
        /// </summary>
        [OracleNumberLength(MaximumLength = 3)]
        public decimal? P_TOTAL_TIME
        {
            get { return _p_total_time; }
            set { _p_total_time = value; NotifyPropertyChanged("P_TOTAL_TIME"); }
        }
        private string _p_process_software;
        /// <summary>
        /// 处理软件
        /// </summary>
        [OracleStringLength(MaximumLength = 255, CharUsed = CharUsedType.Byte)]
        public string P_PROCESS_SOFTWARE
        {
            get { return _p_process_software; }
            set { _p_process_software = value; NotifyPropertyChanged("P_PROCESS_SOFTWARE"); }
        }   
        private string _processor;
        /// <summary>
        /// 处理人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string PROCESSOR
        {
            get { return _processor; }
            set { _processor = value; NotifyPropertyChanged("PROCESSOR"); }
        }
        private string _interpreter;
        /// <summary>
        /// 解释员
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string INTERPRETER
        {
            get { return _interpreter; }
            set { _interpreter = value; NotifyPropertyChanged("INTERPRETER"); }
        }
        private string _p_supervisor;
        /// <summary>
        /// 成果审核人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string P_SUPERVISOR
        {
            get { return _p_supervisor; }
            set { _p_supervisor = value; NotifyPropertyChanged("P_SUPERVISOR"); }
        }
        private decimal? _result_map_type;
        /// <summary>
        /// 成果图件类型
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? RESULT_MAP_TYPE
        {
            get { return _result_map_type; }
            set { _result_map_type = value; NotifyPropertyChanged("RESULT_MAP_TYPE"); }
        }
        private decimal? _log_originality_data;
        /// <summary>
        /// 原始数据文件大小
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? LOG_ORIGINALITY_DATA
        {
            get { return _log_originality_data; }
            set { _log_originality_data = value; NotifyPropertyChanged("LOG_ORIGINALITY_DATA"); }
        }
        private decimal? _log_interpret_report;
        /// <summary>
        /// 测井解释分析报告大小
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? LOG_INTERPRET_REPORT
        {
            get { return _log_interpret_report; }
            set { _log_interpret_report = value; NotifyPropertyChanged("LOG_INTERPRET_REPORT"); }
        }
        private decimal? _log_interpret_result;
        /// <summary>
        /// 解释成果数据文件大小
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? LOG_INTERPRET_RESULT
        {
            get { return _log_interpret_result; }
            set { _log_interpret_result = value; NotifyPropertyChanged("LOG_INTERPRET_RESULT"); }
        }
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }

        private decimal? _file_number;
        /// <summary>
        /// 文件个数
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? FILE_NUMBER
        {
            get { return _file_number; }
            set { _file_number = value; NotifyPropertyChanged("FILE_NUMBER"); }
        }
        private string _p_scene_rating;
        /// <summary>
        /// 现场评级人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string P_SCENE_RATING
        {
            get { return _p_scene_rating; }
            set { _p_scene_rating = value; NotifyPropertyChanged("P_SCENE_RATING"); }
        }
        private string _p_indoor_rating;
        /// <summary>
        /// 室内评级人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string P_INDOOR_RATING
        {
            get { return _p_indoor_rating; }
            set { _p_indoor_rating = value; NotifyPropertyChanged("P_INDOOR_RATING"); }
        }
        private string _acceptance_way_name;
        /// <summary>
        /// 资料验收方式
        /// </summary>
        [OracleStringLength(MaximumLength=32,CharUsed=CharUsedType.Byte)]
        public string ACCEPTANCE_WAY_NAME
        {
            get { return _acceptance_way_name; }
            set { _acceptance_way_name = value; NotifyPropertyChanged("ACCEPTANCE_WAY_NAME"); }
        }
    }
}