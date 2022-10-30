using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 井身结构数据 modle
    /// </summary>
    [Serializable]
    public class COM_WELLSTRUCTURE_DATA : ModelBase
    {
        private string _wellstructure_id;
        /// <summary>
        /// 井身结构数据ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string WELLSTRUCTURE_ID
        {
            get { return _wellstructure_id; }
            set { _wellstructure_id = value; NotifyPropertyChanged("WELLSTRUCTURE_ID"); }
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
        private string _wellbore_id;
        /// <summary>
        /// 井筒ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Byte)]
        public string WELLBORE_ID
        {
            get { return _wellbore_id; }
            set { _wellbore_id = value; NotifyPropertyChanged("WELLBORE_ID"); }
        }
        private string _no;
        /// <summary>
        /// 序号
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string NO
        {
            get { return _no; }
            set { _no = value; NotifyPropertyChanged("NO"); }
        }
        private string _casing_name;
        /// <summary>
        /// 套管名称
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string CASING_NAME
        {
            get { return _casing_name; }
            set { _casing_name = value; NotifyPropertyChanged("CASING_NAME"); }
        }
        private decimal? _md;
        /// <summary>
        /// 井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? MD
        {
            get { return _md; }
            set { _md = value; NotifyPropertyChanged("MD"); }
        }
        private string _horizon;
        /// <summary>
        /// 层位
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string HORIZON
        {
            get { return _horizon; }
            set { _horizon = value; NotifyPropertyChanged("HORIZON"); }
        }
        private decimal? _bore_size;
        /// <summary>
        /// 井眼尺寸
        /// </summary>
        [OracleNumberLength(MaximumLength = 4)]
        public decimal? BORE_SIZE
        {
            get { return _bore_size; }
            set { _bore_size = value; NotifyPropertyChanged("BORE_SIZE"); }
        }
        private decimal? _casing_od;
        /// <summary>
        /// 套管外径
        /// </summary>
        [OracleNumberLength(MaximumLength = 4)]
        public decimal? CASING_OD
        {
            get { return _casing_od; }
            set { _casing_od = value; NotifyPropertyChanged("CASING_OD"); }
        }
        private decimal? _boingtool_bottom_depth;
        /// <summary>
        /// 套管顶度
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? BOINGTOOL_BOTTOM_DEPTH
        {
            get { return _boingtool_bottom_depth; }
            set { _boingtool_bottom_depth = value; NotifyPropertyChanged("BOINGTOOL_BOTTOM_DEPTH"); }
        }
        private decimal? _setting_depth;
        /// <summary>
        /// 套管下深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? SETTING_DEPTH
        {
            get { return _setting_depth; }
            set { _setting_depth = value; NotifyPropertyChanged("SETTING_DEPTH"); }
        }
        private decimal? _choke_coil_depth;
        /// <summary>
        /// 阻流环深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? CHOKE_COIL_DEPTH
        {
            get { return _choke_coil_depth; }
            set { _choke_coil_depth = value; NotifyPropertyChanged("CHOKE_COIL_DEPTH"); }
        }
        private decimal? _stage_collar1_depth;
        /// <summary>
        /// 分级箍1下深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? STAGE_COLLAR1_DEPTH
        {
            get { return _stage_collar1_depth; }
            set { _stage_collar1_depth = value; NotifyPropertyChanged("STAGE_COLLAR1_DEPTH"); }
        }
        private decimal? _stage_collar2_depth;
        /// <summary>
        /// 分级箍2下深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? STAGE_COLLAR2_DEPTH
        {
            get { return _stage_collar2_depth; }
            set { _stage_collar2_depth = value; NotifyPropertyChanged("STAGE_COLLAR2_DEPTH"); }
        }
        private decimal? _first_cement_top;
        /// <summary>
        /// 一级水泥返深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? FIRST_CEMENT_TOP
        {
            get { return _first_cement_top; }
            set { _first_cement_top = value; NotifyPropertyChanged("FIRST_CEMENT_TOP"); }
        }
        private decimal? _second_cement_top;
        /// <summary>
        /// 二级水泥返深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? SECOND_CEMENT_TOP
        {
            get { return _second_cement_top; }
            set { _second_cement_top = value; NotifyPropertyChanged("SECOND_CEMENT_TOP"); }
        }
        private decimal? _third_cement_top;
        /// <summary>
        /// 三级水泥返深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? THIRD_CEMENT_TOP
        {
            get { return _third_cement_top; }
            set { _third_cement_top = value; NotifyPropertyChanged("THIRD_CEMENT_TOP"); }
        }
        private string _cement_method;
        /// <summary>
        /// 固井方法
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string CEMENT_METHOD
        {
            get { return _cement_method; }
            set { _cement_method = value; NotifyPropertyChanged("CEMENT_METHOD"); }
        }
        private decimal? _artificial_well_bottom_depth;
        /// <summary>
        /// 人工井底
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? ARTIFICIAL_WELL_BOTTOM_DEPTH
        {
            get { return _artificial_well_bottom_depth; }
            set { _artificial_well_bottom_depth = value; NotifyPropertyChanged("ARTIFICIAL_WELL_BOTTOM_DEPTH"); }
        }

    }
}