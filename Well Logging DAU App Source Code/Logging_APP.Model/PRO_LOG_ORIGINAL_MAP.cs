using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    [Serializable]
    public class PRO_LOG_ORIGINAL_MAP:ModelBase
    {
        private string _mapid;
        /// <summary>
        /// 图件ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength=32,CharUsed=CharUsedType.Byte)]
        public string MAPID
        {
            get { return _mapid; }
            set { _mapid = value; NotifyPropertyChanged("MAPID"); }
        }
        private string _process_id;
        /// <summary>
        /// 解释处理ID
        /// </summary>
        [OracleStringLength(MaximumLength=100,CharUsed=CharUsedType.Byte)]
        public string PROCESS_ID
        {
            get { return _process_id; }
            set { _process_id = value;NotifyPropertyChanged("PROCESS_ID"); }
        }
        private string _maps_name;
        /// <summary>
        /// 图件名称
        /// </summary>
        [OracleStringLength(MaximumLength=32,CharUsed=CharUsedType.Byte)]
        public string MAPS_NAME
        {
            get { return _maps_name; }
            set { _maps_name = value; NotifyPropertyChanged("MAPS_NAME"); }
        }
        private decimal? _map_start_dep;
        /// <summary>
        /// 起始深度
        /// </summary>
        [OracleNumberLength(MaximumLength=12)]
        public decimal? MAP_START_DEP
        {
            get { return _map_start_dep; }
            set { _map_start_dep = value; NotifyPropertyChanged("MAP_START_DEP"); }
        }
        private decimal? _map_end_dep;
        /// <summary>
        /// 结束深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 12)] 
        public decimal? MAP_END_DEP
        {
            get { return _map_end_dep; }
            set { _map_end_dep = value; NotifyPropertyChanged("MAP_END_DEP"); }
        }
        private string _map_scale;
        /// <summary>
        /// 绘图比例
        /// </summary>
        [OracleStringLength(MaximumLength=32,CharUsed=CharUsedType.Byte)]
        public string MAP_SCALE
        {
            get { return _map_scale; }
            set { _map_scale = value; NotifyPropertyChanged("MAP_SCALE"); }
        }
        private string _map_data_name;
        /// <summary>
        /// 图件数据名
        /// </summary>
        [OracleStringLength(MaximumLength =255, CharUsed = CharUsedType.Char)]
        public string MAP_DATA_NAME
        {
            get { return _map_data_name; }
            set { _map_data_name = value; NotifyPropertyChanged("MAP_DATA_NAME"); }
        }
        private decimal? _rework_num;
        /// <summary>
        /// 返工次数
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)] 
        public decimal? REWORK_NUM
        {
            get { return _rework_num; }
            set { _rework_num = value; NotifyPropertyChanged("REWORK_NUM"); }
        }
        private string _rework_why;
        /// <summary>
        /// 返工原因
        /// </summary>
        [OracleStringLength(MaximumLength = 1024, CharUsed = CharUsedType.Byte)]
        public string REWORK_WHY
        {
            get { return _rework_why; }
            set { _rework_why = value; NotifyPropertyChanged("REWORK_WHY"); }
        }
        private DateTime? _map_date;
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime? MAP_DATE
        {
            get { return _map_date; }
            set { _map_date = value; NotifyPropertyChanged("MAP_DATE"); }
        }
        private string _reviewer;
        /// <summary>
        /// 审核人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string REVIEWER
        {
            get { return _reviewer; }
            set { _reviewer = value; NotifyPropertyChanged("REVIEWER"); }
        }
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 1024, CharUsed = CharUsedType.Byte)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }
    }
}
