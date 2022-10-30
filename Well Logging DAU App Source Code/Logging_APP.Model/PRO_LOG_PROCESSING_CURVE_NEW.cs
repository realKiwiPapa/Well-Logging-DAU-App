using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 解释处理_处理项目曲线 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_PROCESSING_CURVE_NEW : ModelBase
    {
        private decimal _curveid;
        ///// <summary>
        ///// 曲线ID
        ///// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal CURVEID
        {
            get { return _curveid; }
            set { _curveid = value; NotifyPropertyChanged("CURVEID"); }
        }
        //private string _process_id;
        ///// <summary>
        ///// 解释处理ID
        ///// </summary>
        //[OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        //public string PROCESS_ID
        //{
        //    get { return _process_id; }
        //    set { _process_id = value; NotifyPropertyChanged("PROCESS_ID"); }
        //}
        //private string _itemid;
        ///// <summary>
        ///// 项目ID
        ///// </summary>
        //[OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        //public string ITEMID
        //{
        //    get { return _itemid; }
        //    set { _itemid = value; NotifyPropertyChanged("ITEMID"); }
        //}

        //private decimal? _processing_item_id;
        ///// <summary>
        ///// 解释处理项目编码
        ///// </summary>
        //[OracleNumberLength(MaximumLength = 38)]
        //public decimal? PROCESSING_ITEM_ID
        //{
        //    get { return _processing_item_id; }
        //    set { _processing_item_id = value; NotifyPropertyChanged("PROCESSING_ITEM_ID"); }
        //}
        private string _curve_name;
        /// <summary>
        /// 曲线名
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string CURVE_NAME
        {
            get { return _curve_name; }
            set { _curve_name = value; NotifyPropertyChanged("CURVE_NAME"); }
        }
        private decimal? _curve_start_dep;
        /// <summary>
        /// 曲线始深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? CURVE_START_DEP
        {
            get { return _curve_start_dep; }
            set { _curve_start_dep = value; NotifyPropertyChanged("CURVE_START_DEP"); }
        }
        private decimal? _curve_end_dep;
        /// <summary>
        /// 曲线止深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? CURVE_END_DEP
        {
            get { return _curve_end_dep; }
            set { _curve_end_dep = value; NotifyPropertyChanged("CURVE_END_DEP"); }
        }
        private decimal? _curve_rlev;
        /// <summary>
        /// 曲线深度采样
        /// </summary>
        [OracleNumberLength(MaximumLength = 9)]
        public decimal? CURVE_RLEV
        {
            get { return _curve_rlev; }
            set { _curve_rlev = value; NotifyPropertyChanged("CURVE_RLEV"); }
        }
        private string _curve_unit;
        /// <summary>
        /// 曲线单位
        /// </summary>
        [OracleStringLength(MaximumLength = 8, CharUsed = CharUsedType.Byte)]
        public string CURVE_UNIT
        {
            get { return _curve_unit; }
            set { _curve_unit = value; NotifyPropertyChanged("CURVE_UNIT"); }
        }
        private decimal? _curve_t_sample;
        /// <summary>
        /// T轴采样点数
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? CURVE_T_SAMPLE
        {
            get { return _curve_t_sample; }
            set { _curve_t_sample = value; NotifyPropertyChanged("CURVE_T_SAMPLE"); }
        }
        private string _curve_t_unit;
        /// <summary>
        /// T轴单位
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string CURVE_T_UNIT
        {
            get { return _curve_t_unit; }
            set { _curve_t_unit = value; NotifyPropertyChanged("CURVE_T_UNIT"); }
        }
        private decimal? _curve_t_max_value;
        /// <summary>
        /// T轴最大值
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? CURVE_T_MAX_VALUE
        {
            get { return _curve_t_max_value; }
            set { _curve_t_max_value = value; NotifyPropertyChanged("CURVE_T_MAX_VALUE"); }
        }
        private decimal? _curve_t_min_value;
        /// <summary>
        /// T轴最小值
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? CURVE_T_MIN_VALUE
        {
            get { return _curve_t_min_value; }
            set { _curve_t_min_value = value; NotifyPropertyChanged("CURVE_T_MIN_VALUE"); }
        }
        private decimal? _curve_t_relv;
        /// <summary>
        /// T轴采样间距
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? CURVE_T_RELV
        {
            get { return _curve_t_relv; }
            set { _curve_t_relv = value; NotifyPropertyChanged("CURVE_T_RELV"); }
        }
        private string _curve_data_type;
        /// <summary>
        /// 数据类型
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string CURVE_DATA_TYPE
        {
            get { return _curve_data_type; }
            set { _curve_data_type = value; NotifyPropertyChanged("CURVE_DATA_TYPE"); }
        }
        private decimal? _curve_data_length;
        /// <summary>
        /// 数据长度
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? CURVE_DATA_LENGTH
        {
            get { return _curve_data_length; }
            set { _curve_data_length = value; NotifyPropertyChanged("CURVE_DATA_LENGTH"); }
        }
        //private string _data_property;
        ///// <summary>
        ///// 曲线属性：原始,处理生成
        ///// </summary>
        //[OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        //public string DATA_PROPERTY
        //{
        //    get { return _data_property; }
        //    set { _data_property = value; NotifyPropertyChanged("DATA_PROPERTY"); }
        //}
        private decimal? _data_info;
        /// <summary>
        /// 维数信息
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? DATA_INFO
        {
            get { return _data_info; }
            set { _data_info = value; NotifyPropertyChanged("DATA_INFO"); }
        }
        //private string _p_curvesoftware_name;
        ///// <summary>
        ///// 曲线处理软件名
        ///// </summary>
        //[OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        //public string P_CURVESOFTWARE_NAME
        //{
        //    get { return _p_curvesoftware_name; }
        //    set { _p_curvesoftware_name = value; NotifyPropertyChanged("P_CURVESOFTWARE_NAME"); }
        //}
        //private decimal? _data_storage_way;
        ///// <summary>
        ///// 数据存储方式
        ///// </summary>
        //[OracleNumberLength(MaximumLength = 38)]
        //public decimal? DATA_STORAGE_WAY
        //{
        //    get { return _data_storage_way; }
        //    set { _data_storage_way = value; NotifyPropertyChanged("DATA_STORAGE_WAY"); }
        //}
        private decimal? _data_size;
        /// <summary>
        /// 数据总字节数
        /// </summary>
        [OracleNumberLength(MaximumLength = 12)]
        public decimal? DATA_SIZE
        {
            get { return _data_size; }
            set { _data_size = value; NotifyPropertyChanged("DATA_SIZE"); }
        }
        //private string _file_storage_path;
        ///// <summary>
        ///// 文件存储路径
        ///// </summary>
        //[OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        //public string FILE_STORAGE_PATH
        //{
        //    get { return _file_storage_path; }
        //    set { _file_storage_path = value; NotifyPropertyChanged("FILE_STORAGE_PATH"); }
        //}
        private byte[] _file_data;
        ///// <summary>
        ///// 数据体
        ///// </summary>
        public byte[] FILE_DATA
        {
            get { return _file_data; }
            set { _file_data = value; NotifyPropertyChanged("FILE_DATA"); }
        }
        //private string _curve_note;
        ///// <summary>
        ///// 曲线其他描述
        ///// </summary>
        //[OracleStringLength(MaximumLength = 256, CharUsed = CharUsedType.Byte)]
        //public string CURVE_NOTE
        //{
        //    get { return _curve_note; }
        //    set { _curve_note = value; NotifyPropertyChanged("CURVE_NOTE"); }
        //}
        //private string _note;
        ///// <summary>
        ///// 备注
        ///// </summary>
        //[OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        //public string NOTE
        //{
        //    get { return _note; }
        //    set { _note = value; NotifyPropertyChanged("NOTE"); }
        //}
        private decimal? _fileid;
        /// <summary>
        /// FILEID
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? FILEID
        {
            get { return _fileid; }
            set { _fileid = value; NotifyPropertyChanged("FILEID"); }
        }
        private string _curve_cd;
        /// <summary>
        /// CURVE_CD
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string CURVE_CD
        {
            get { return _curve_cd; }
            set { _curve_cd = value; NotifyPropertyChanged("CURVE_CD"); }
        }
        private string _data_path;
        /// <summary>
        /// 
        /// </summary>
        public string DATA_PATH
        {
            get { return _data_path; }
            set { _data_path = value; NotifyPropertyChanged("DATA_PATH"); }
        }
    }
}