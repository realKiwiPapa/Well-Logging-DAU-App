using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	 	//井
    [Serializable]
    public class COM_BASE_WELL : ModelBase
	{
   		     
      	/// <summary>
		/// 井ID
        /// </summary>
        [Required]
		private string _well_id;
        public string WELL_ID
        {
            get{ return _well_id; }
            set{ _well_id = value; }
        }        
		/// <summary>
		/// 工区ID
        /// </summary>		
		private string _site_id;
        public string SITE_ID
        {
            get{ return _site_id; }
            set{ _site_id = value; }
        }        
		/// <summary>
		/// 井名
        /// </summary>		
		private string _well_name;
        public string WELL_NAME
        {
            get{ return _well_name; }
            set{ _well_name = value; }
        }        
		/// <summary>
		/// 标准井名
        /// </summary>		
		private string _well_legalname;
        public string WELL_LEGALNAME
        {
            get{ return _well_legalname; }
            set{ _well_legalname = value; }
        }        
		/// <summary>
		/// 井型
        /// </summary>		
		private string _well_type;
        public string WELL_TYPE
        {
            get{ return _well_type; }
            set{ _well_type = value; }
        }        
		/// <summary>
		/// 井别
        /// </summary>		
		private string _well_sort;
        public string WELL_SORT
        {
            get{ return _well_sort; }
            set{ _well_sort = value; }
        }        
		/// <summary>
		/// 国家
        /// </summary>		
		private string _country;
        public string COUNTRY
        {
            get{ return _country; }
            set{ _country = value; }
        }        
		/// <summary>
		/// 省
        /// </summary>		
		private string _loc_state;
        public string LOC_STATE
        {
            get{ return _loc_state; }
            set{ _loc_state = value; }
        }        
		/// <summary>
		/// 市
        /// </summary>		
		private string _city;
        public string CITY
        {
            get{ return _city; }
            set{ _city = value; }
        }        
		/// <summary>
		/// 县
        /// </summary>		
		private string _loc_county;
        public string LOC_COUNTY
        {
            get{ return _loc_county; }
            set{ _loc_county = value; }
        }        
		/// <summary>
		/// 行政区域
        /// </summary>		
		private string _district;
        public string DISTRICT
        {
            get{ return _district; }
            set{ _district = value; }
        }        
		/// <summary>
		/// 一级构造单元
        /// </summary>		
		private string _struct_code;
        public string STRUCT_CODE
        {
            get{ return _struct_code; }
            set{ _struct_code = value; }
        }        
		/// <summary>
		/// 构造位置
        /// </summary>		
		private string _structure_location;
        public string STRUCTURE_LOCATION
        {
            get{ return _structure_location; }
            set{ _structure_location = value; }
        }        
		/// <summary>
		/// 构造位置
        /// </summary>		
		private string _structure_description;
        public string STRUCTURE_DESCRIPTION
        {
            get{ return _structure_description; }
            set{ _structure_description = value; }
        }        
		/// <summary>
		/// 二维测线位置
        /// </summary>		
		private string _two_dim_position;
        public string TWO_DIM_POSITION
        {
            get{ return _two_dim_position; }
            set{ _two_dim_position = value; }
        }        
		/// <summary>
		/// 三维测线位置
        /// </summary>		
		private string _three_dim_position;
        public string THREE_DIM_POSITION
        {
            get{ return _three_dim_position; }
            set{ _three_dim_position = value; }
        }        
		/// <summary>
		/// 相对位置井名
        /// </summary>		
		private string _relative_well_name;
        public string RELATIVE_WELL_NAME
        {
            get{ return _relative_well_name; }
            set{ _relative_well_name = value; }
        }        
		/// <summary>
		/// 相对位置的距离
        /// </summary>		
		private decimal _relative_space;
        public decimal RELATIVE_SPACE
        {
            get{ return _relative_space; }
            set{ _relative_space = value; }
        }        
		/// <summary>
		/// 相对位置方位
        /// </summary>		
		private string _relative_azimuth;
        public string RELATIVE_AZIMUTH
        {
            get{ return _relative_azimuth; }
            set{ _relative_azimuth = value; }
        }        
		/// <summary>
		/// 地理位置
        /// </summary>		
		private string _geographic_location;
        public string GEOGRAPHIC_LOCATION
        {
            get{ return _geographic_location; }
            set{ _geographic_location = value; }
        }        
		/// <summary>
		/// 设计经度
        /// </summary>		
        private string _design_longitude;
        public string DESIGN_LONGITUDE
        {
            get{ return _design_longitude; }
            set{ _design_longitude = value; }
        }        
		/// <summary>
		/// 设计纬度
        /// </summary>		
        private string _design_latitude;
        public string DESIGN_LATITUDE
        {
            get{ return _design_latitude; }
            set{ _design_latitude = value; }
        }        
		/// <summary>
		/// 实际经度
        /// </summary>		
        private string _actual_longitude;
        public string ACTUAL_LONGITUDE
        {
            get{ return _actual_longitude; }
            set{ _actual_longitude = value; }
        }        
		/// <summary>
		/// 实际纬度
        /// </summary>		
        private string _actual_latitude;
        public string ACTUAL_LATITUDE
        {
            get{ return _actual_latitude; }
            set{ _actual_latitude = value; }
        }        
		/// <summary>
		/// 设计X坐标
        /// </summary>		
		private decimal _design_x_axis;
        public decimal DESIGN_X_AXIS
        {
            get{ return _design_x_axis; }
            set{ _design_x_axis = value; }
        }        
		/// <summary>
		/// 设计Y坐标
        /// </summary>		
		private decimal _design_y_axis;
        public decimal DESIGN_Y_AXIS
        {
            get{ return _design_y_axis; }
            set{ _design_y_axis = value; }
        }        
		/// <summary>
		/// 实际X坐标
        /// </summary>		
		private decimal _actual_x_axis;
        public decimal ACTUAL_X_AXIS
        {
            get{ return _actual_x_axis; }
            set{ _actual_x_axis = value; }
        }        
		/// <summary>
		/// 实际Y坐标
        /// </summary>		
		private decimal _actual_y_axis;
        public decimal ACTUAL_Y_AXIS
        {
            get{ return _actual_y_axis; }
            set{ _actual_y_axis = value; }
        }        
		/// <summary>
		/// 设计地面海拔
        /// </summary>		
		private decimal _design_ground_elevation;
        public decimal DESIGN_GROUND_ELEVATION
        {
            get{ return _design_ground_elevation; }
            set{ _design_ground_elevation = value; }
        }        
		/// <summary>
		/// 实际地面海拔
        /// </summary>		
		private decimal _actual_ground_elevation;
        public decimal ACTUAL_GROUND_ELEVATION
        {
            get{ return _actual_ground_elevation; }
            set{ _actual_ground_elevation = value; }
        }        
		/// <summary>
		/// 测线横坐标
        /// </summary>		
		private decimal _traverse_line_x_axis;
        public decimal TRAVERSE_LINE_X_AXIS
        {
            get{ return _traverse_line_x_axis; }
            set{ _traverse_line_x_axis = value; }
        }        
		/// <summary>
		/// 测线纵坐标
        /// </summary>		
		private decimal _traverse_line_y_axis;
        public decimal TRAVERSE_LINE_Y_AXIS
        {
            get{ return _traverse_line_y_axis; }
            set{ _traverse_line_y_axis = value; }
        }        
		/// <summary>
		/// 海水深度
        /// </summary>		
		private decimal _sea_water_depth;
        public decimal SEA_WATER_DEPTH
        {
            get{ return _sea_water_depth; }
            set{ _sea_water_depth = value; }
        }        
		/// <summary>
		/// 平台号
        /// </summary>		
		private string _terrace_code;
        public string TERRACE_CODE
        {
            get{ return _terrace_code; }
            set{ _terrace_code = value; }
        }        
		/// <summary>
		/// 磁倾角
        /// </summary>		
		private decimal _magnetic_dip;
        public decimal MAGNETIC_DIP
        {
            get{ return _magnetic_dip; }
            set{ _magnetic_dip = value; }
        }        
		/// <summary>
		/// 磁偏角
        /// </summary>		
		private decimal _angle_dip;
        public decimal ANGLE_DIP
        {
            get{ return _angle_dip; }
            set{ _angle_dip = value; }
        }        
		/// <summary>
		/// 磁场强度
        /// </summary>		
		private decimal _magnetic_fieldintensity;
        public decimal MAGNETIC_FIELDINTENSITY
        {
            get{ return _magnetic_fieldintensity; }
            set{ _magnetic_fieldintensity = value; }
        }        
		/// <summary>
		/// 井唯一标识
        /// </summary>		
		private string _well_uwi;
        public string WELL_UWI
        {
            get{ return _well_uwi; }
            set{ _well_uwi = value; }
        }        
		/// <summary>
		/// 工程区块
        /// </summary>		
		private string _well_block;
        public string WELL_BLOCK
        {
            get{ return _well_block; }
            set{ _well_block = value; }
        }        
		/// <summary>
		/// 井油气田单元名称
        /// </summary>		
		private string _well_field_unit_name;
        public string WELL_FIELD_UNIT_NAME
        {
            get{ return _well_field_unit_name; }
            set{ _well_field_unit_name = value; }
        }        
		/// <summary>
		/// 行状态
        /// </summary>		
		private string _row_state;
        public string ROW_STATE
        {
            get{ return _row_state; }
            set{ _row_state = value; }
        }        
		/// <summary>
		/// 创建者所在单位
        /// </summary>		
		private string _create_org;
        public string CREATE_ORG
        {
            get{ return _create_org; }
            set{ _create_org = value; }
        }        
		/// <summary>
		/// 创建者
        /// </summary>		
		private string _create_user;
        public string CREATE_USER
        {
            get{ return _create_user; }
            set{ _create_user = value; }
        }        
		/// <summary>
		/// 修改者所在单位
        /// </summary>		
		private string _update_org;
        public string UPDATE_ORG
        {
            get{ return _update_org; }
            set{ _update_org = value; }
        }        
		/// <summary>
		/// 修改者
        /// </summary>		
		private string _update_user;
        public string UPDATE_USER
        {
            get{ return _update_user; }
            set{ _update_user = value; }
        }        
		/// <summary>
		/// 创建时间
        /// </summary>		
		private DateTime _create_date;
        public DateTime CREATE_DATE
        {
            get{ return _create_date; }
            set{ _create_date = value; }
        }        
		/// <summary>
		/// 修改时间
        /// </summary>		
		private DateTime _update_date;
        public DateTime UPDATE_DATE
        {
            get{ return _update_date; }
            set{ _update_date = value; }
        }        
		/// <summary>
		/// 备注
        /// </summary>		
		private string _remark;
        public string REMARK
        {
            get{ return _remark; }
            set{ _remark = value; }
        }        
		   
	}
}

