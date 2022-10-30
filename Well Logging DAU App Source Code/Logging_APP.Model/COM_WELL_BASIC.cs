using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 井基本数据 modle
    /// </summary>
    [Serializable]
    public class COM_WELL_BASIC : ModelBase
    {
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
        private string _site_id;
        /// <summary>
        /// 工区唯一标识符
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string SITE_ID
        {
            get { return _site_id; }
            set { _site_id = value; NotifyPropertyChanged("SITE_ID"); }
        }
        private string _well_name;
        /// <summary>
        /// 通用井名
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WELL_NAME
        {
            get { return _well_name; }
            set { _well_name = value.TrimChar(); NotifyPropertyChanged("WELL_NAME"); }
        }
        private string _well_legal_name;
        /// <summary>
        /// 标准井名
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WELL_LEGAL_NAME
        {
            get { return _well_legal_name; }
            set { _well_legal_name = value.TrimChar(); NotifyPropertyChanged("WELL_LEGAL_NAME"); }
        }
        private string _well_uwi;
        /// <summary>
        /// 井唯一标识
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WELL_UWI
        {
            get { return _well_uwi; }
            set { _well_uwi = value; NotifyPropertyChanged("WELL_UWI"); }
        }
        private string _well_struct_unit_description;
        /// <summary>
        /// 井构造单元详细描述
        /// </summary>
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Byte)]
        public string WELL_STRUCT_UNIT_DESCRIPTION
        {
            get { return _well_struct_unit_description; }
            set { _well_struct_unit_description = value; NotifyPropertyChanged("WELL_STRUCT_UNIT_DESCRIPTION"); }
        }
        private string _well_field_unit_name;
        /// <summary>
        /// 井油气田单元名称
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WELL_FIELD_UNIT_NAME
        {
            get { return _well_field_unit_name; }
            set { _well_field_unit_name = value; NotifyPropertyChanged("WELL_FIELD_UNIT_NAME"); }
        }
        private string _well_struct_unit_name;
        /// <summary>
        /// 井构造单元名称
        /// </summary>
        [OracleStringLength(MaximumLength = 40, CharUsed = CharUsedType.Byte)]
        public string WELL_STRUCT_UNIT_NAME
        {
            get { return _well_struct_unit_name; }
            set { _well_struct_unit_name = value; NotifyPropertyChanged("WELL_STRUCT_UNIT_NAME"); }
        }
        private string _beog_location;
        /// <summary>
        /// 地理位置
        /// </summary>
        [OracleStringLength(MaximumLength = 64, CharUsed = CharUsedType.Byte)]
        public string BEOG_LOCATION
        {
            get { return _beog_location; }
            set { _beog_location = value; NotifyPropertyChanged("BEOG_LOCATION"); }
        }
        private string _structural_location;
        /// <summary>
        /// 构造位置
        /// </summary>
        [OracleStringLength(MaximumLength = 128, CharUsed = CharUsedType.Byte)]
        public string STRUCTURAL_LOCATION
        {
            get { return _structural_location; }
            set { _structural_location = value; NotifyPropertyChanged("STRUCTURAL_LOCATION"); }
        }
        private string _traverse_line_location;
        /// <summary>
        /// 测线位置
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string TRAVERSE_LINE_LOCATION
        {
            get { return _traverse_line_location; }
            set { _traverse_line_location = value; NotifyPropertyChanged("TRAVERSE_LINE_LOCATION"); }
        }
        private decimal? _survey_x_axis;
        /// <summary>
        /// 设计井口横坐标
        /// </summary>
        [OracleNumberLength(MaximumLength = 9)]
        public decimal? SURVEY_X_AXIS
        {
            get { return _survey_x_axis; }
            set { _survey_x_axis = value; NotifyPropertyChanged("SURVEY_X_AXIS"); }
        }
        private decimal? _survey_y_axis;
        /// <summary>
        /// 设计井口纵坐标
        /// </summary>
        [OracleNumberLength(MaximumLength = 9)]
        public decimal? SURVEY_Y_AXIS
        {
            get { return _survey_y_axis; }
            set { _survey_y_axis = value; NotifyPropertyChanged("SURVEY_Y_AXIS"); }
        }
        private decimal? _ground_elevation;
        /// <summary>
        /// 地面海拔
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? GROUND_ELEVATION
        {
            get { return _ground_elevation; }
            set { _ground_elevation = value; NotifyPropertyChanged("GROUND_ELEVATION"); }
        }
        private decimal? _system_datum_offset;
        /// <summary>
        /// 基准面海拔
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? SYSTEM_DATUM_OFFSET
        {
            get { return _system_datum_offset; }
            set { _system_datum_offset = value; NotifyPropertyChanged("SYSTEM_DATUM_OFFSET"); }
        }
        private decimal? _ranger_x_axis;
        /// <summary>
        /// 实际井口横坐标
        /// </summary>
        [OracleNumberLength(MaximumLength = 9)]
        public decimal? RANGER_X_AXIS
        {
            get { return _ranger_x_axis; }
            set { _ranger_x_axis = value; NotifyPropertyChanged("RANGER_X_AXIS"); }
        }
        private decimal? _ranger_y_axis;
        /// <summary>
        /// 实际井口纵坐标
        /// </summary>
        [OracleNumberLength(MaximumLength = 9)]
        public decimal? RANGER_Y_AXIS
        {
            get { return _ranger_y_axis; }
            set { _ranger_y_axis = value; NotifyPropertyChanged("RANGER_Y_AXIS"); }
        }
        private decimal? _magnetic_declination;
        /// <summary>
        /// 磁偏角
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? MAGNETIC_DECLINATION
        {
            get { return _magnetic_declination; }
            set { _magnetic_declination = value; NotifyPropertyChanged("MAGNETIC_DECLINATION"); }
        }
        private decimal? _casing_top_depth;
        /// <summary>
        /// 联入深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? CASING_TOP_DEPTH
        {
            get { return _casing_top_depth; }
            set { _casing_top_depth = value; NotifyPropertyChanged("CASING_TOP_DEPTH"); }
        }
        private string _well_head_longitude;
        /// <summary>
        /// 实际井口经度
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WELL_HEAD_LONGITUDE
        {
            get { return _well_head_longitude; }
            set { _well_head_longitude = value; NotifyPropertyChanged("WELL_HEAD_LONGITUDE"); }
        }
        private string _well_head_latitude;
        /// <summary>
        /// 实际井口纬度
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string WELL_HEAD_LATITUDE
        {
            get { return _well_head_latitude; }
            set { _well_head_latitude = value; NotifyPropertyChanged("WELL_HEAD_LATITUDE"); }
        }
        private string _loc_country;
        /// <summary>
        /// 国家(A12)
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOC_COUNTRY
        {
            get { return _loc_country; }
            set { _loc_country = value; NotifyPropertyChanged("LOC_COUNTRY"); }
        }
        private string _loc_state;
        /// <summary>
        /// 省市位置(A12)
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOC_STATE
        {
            get { return _loc_state; }
            set { _loc_state = value; NotifyPropertyChanged("LOC_STATE"); }
        }
        private string _p_loc_city;
        /// <summary>
        /// 地级市(A12)
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string P_LOC_CITY
        {
            get { return _p_loc_city; }
            set { _p_loc_city = value; NotifyPropertyChanged("P_LOC_CITY"); }
        }
        private string _loc_county;
        /// <summary>
        /// 县(A12)
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string LOC_COUNTY
        {
            get { return _loc_county; }
            set { _loc_county = value; NotifyPropertyChanged("LOC_COUNTY"); }
        }
        ////        private byte[] _drill_geo_des;
        /////// <summary>
        /////// 钻井地质设计(A12)
        /////// </summary>
        ////                public byte[] DRILL_GEO_DES
        ////{
        ////    get{ return _drill_geo_des; }
        ////    set{ _drill_geo_des = value; NotifyPropertyChanged("DRILL_GEO_DES"); }
        ////}        
        ////        private byte[] _drill_eng_des;
        /////// <summary>
        /////// 钻井工程设计(A12)
        /////// </summary>
        ////                public byte[] DRILL_ENG_DES
        ////{
        ////    get{ return _drill_eng_des; }
        ////    set{ _drill_eng_des = value; NotifyPropertyChanged("DRILL_ENG_DES"); }
        ////}        
        private string _part_units;
        /// <summary>
        /// 甲方单位（业主单位）
        /// </summary>
        [OracleStringLength(MaximumLength = 33, CharUsed = CharUsedType.Byte)]
        public string PART_UNITS
        {
            get { return _part_units; }
            set { _part_units = value; NotifyPropertyChanged("PART_UNITS"); }
        }
        private string _well_head_flange;
        /// <summary>
        /// 井口法兰
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string WELL_HEAD_FLANGE
        {
            get { return _well_head_flange.TrimCharEnd(); }
            set { _well_head_flange = value; NotifyPropertyChanged("WELL_HEAD_FLANGE"); }
        }
        private decimal? _drill_geo_des_fileid;
        /// <summary>
        /// 钻井地质设计(A12)FILEID
        /// </summary>
        public decimal? DRILL_GEO_DES_FILEID
        {
            get { return _drill_geo_des_fileid; }
            set { _drill_geo_des_fileid = value; NotifyPropertyChanged("DRILL_GEO_DES_FILEID"); }
        }
        private decimal? _drill_eng_des_fileid;
        /// <summary>
        /// 钻井工程设计(A12)FILEID
        /// </summary>
        public decimal? DRILL_ENG_DES_FILEID
        {
            get { return _drill_eng_des_fileid; }
            set { _drill_eng_des_fileid = value; NotifyPropertyChanged("DRILL_ENG_DES_FILEID"); }
        }
        /// <summary>
        /// 临井钻头情况
        /// </summary>
        private string _well_drill_condition;

        public string WELL_DRILL_CONDITION
        {
            get { return _well_drill_condition; }
            set { _well_drill_condition = value; NotifyPropertyChanged("WELL_DRILL_CONDITION"); }
        }

        /// <summary>
        /// 补心高
        /// </summary>
        private decimal? _bx_heihtg;

        public decimal? BX_HEIGHT
        {
            get { return _bx_heihtg; }
            set { _bx_heihtg = value; NotifyPropertyChanged("BX_HEIGHT"); }
        } 
    }
}