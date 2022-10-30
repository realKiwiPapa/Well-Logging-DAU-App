using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 解释处理_处理项目，为该次施工解释处理项目统计表 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_PROCESSING_ITEM : ModelBase
    {
        private string _itemid;
        /// <summary>
        /// 项目ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string ITEMID
        {
            get { return _itemid; }
            set { _itemid = value; NotifyPropertyChanged("ITEMID"); }
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
        private decimal _processing_item_id;
        /// <summary>
        /// 解释处理项目编码
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal PROCESSING_ITEM_ID
        {
            get { return _processing_item_id; }
            set { _processing_item_id = value; NotifyPropertyChanged("PROCESSING_ITEM_ID"); }
        }
        private decimal? _p_curve_number;
        /// <summary>
        /// 对应曲线条数
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? P_CURVE_NUMBER
        {
            get { return _p_curve_number; }
            set { _p_curve_number = value; NotifyPropertyChanged("P_CURVE_NUMBER"); }
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
        private string _p_well_interval;
        /// <summary>
        /// 处理井段
        /// </summary>
        [OracleStringLength(MaximumLength = 128, CharUsed = CharUsedType.Byte)]
        public string P_WELL_INTERVAL
        {
            get { return _p_well_interval; }
            set { _p_well_interval = value; NotifyPropertyChanged("P_WELL_INTERVAL"); }
        }
        private string _data_name;
        /// <summary>
        /// 数据文件名 该项目数据存放对应的文件名称
        /// </summary>
        [OracleStringLength(MaximumLength = 64, CharUsed = CharUsedType.Byte)]
        public string DATA_NAME
        {
            get { return _data_name; }
            set { _data_name = value; NotifyPropertyChanged("DATA_NAME"); }
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
        private DateTime? _p_start_date;
        /// <summary>
        /// 处理开始日期
        /// </summary>
        public DateTime? P_START_DATE
        {
            get { return _p_start_date; }
            set { _p_start_date = value; NotifyPropertyChanged("P_START_DATE"); }
        }
        private string _scale;
        /// <summary>
        /// 比例
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string SCALE
        {
            get { return _scale; }
            set { _scale = value; NotifyPropertyChanged("SCALE"); }
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
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 256, CharUsed = CharUsedType.Char)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }

        private string _log_series_name;
        /// <summary>
        /// 测井系列
        /// </summary>
        [OracleStringLength(MaximumLength=32,CharUsed=CharUsedType.Byte)]
        public string LOG_SERIES_NAME
        {
            get { return _log_series_name; }
            set { _log_series_name = value; NotifyPropertyChanged("LOG_SERIES_NAME"); }
        }

        private string _log_data_foramt;
        /// <summary>
        /// 原始数据文件格式
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string LOG_DATA_FORMAT
        {
            get { return _log_data_foramt; }
            set { _log_data_foramt = value; NotifyPropertyChanged("LOG_DATA_FORMAT"); }
        }
    }
}