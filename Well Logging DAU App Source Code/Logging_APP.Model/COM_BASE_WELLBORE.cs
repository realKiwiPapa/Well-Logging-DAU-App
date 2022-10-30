using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	 	//井筒
    [Serializable]
    public class COM_BASE_WELLBORE : ModelBase
	{
   		     
      	/// <summary>
		/// 井ID
        /// </summary>		
		private string _well_id;
        public string WELL_ID
        {
            get{ return _well_id; }
            set{ _well_id = value; }
        }        
		/// <summary>
		/// 井筒ID
        /// </summary>	
        [Required]
		private string _wellbore_id;
        public string WELLBORE_ID
        {
            get{ return _wellbore_id; }
            set{ _wellbore_id = value; }
        }        
		/// <summary>
		/// 父井筒ID
        /// </summary>		
		private string _parent_wellbore_id;
        public string PARENT_WELLBORE_ID
        {
            get{ return _parent_wellbore_id; }
            set{ _parent_wellbore_id = value; }
        }        
		/// <summary>
		/// 井筒编号
        /// </summary>		
		private string _wellbore_no;
        public string WELLBORE_NO
        {
            get{ return _wellbore_no; }
            set{ _wellbore_no = value; }
        }        
		/// <summary>
		/// 井筒名:当为主井筒时，填“主井筒”，当为侧钻/??斜或者多分支井时，填写侧钻井的通用（汉字）井名或分支井同的汉字名称，当为侧钻井时，通用井筒名的命名规范可参照井的通用井名命名规范
        /// </summary>		
		private string _wellbore_name;
        public string WELLBORE_NAME
        {
            get{ return _wellbore_name; }
            set{ _wellbore_name = value; }
        }        
		/// <summary>
		/// 标准井筒名
        /// </summary>		
		private string _wellbore_legel_name;
        public string WELLBORE_LEGEL_NAME
        {
            get{ return _wellbore_legel_name; }
            set{ _wellbore_legel_name = value; }
        }        
		/// <summary>
		/// 主要目的层
        /// </summary>		
		private string _target_formation;
        public string TARGET_FORMATION
        {
            get{ return _target_formation; }
            set{ _target_formation = value; }
        }        
		/// <summary>
		/// 设计完钻层位
        /// </summary>		
		private string _design_finish_formation;
        public string DESIGN_FINISH_FORMATION
        {
            get{ return _design_finish_formation; }
            set{ _design_finish_formation = value; }
        }        
		/// <summary>
		/// 实际完钻层位
        /// </summary>		
		private string _actual_finsh_formation;
        public string ACTUAL_FINSH_FORMATION
        {
            get{ return _actual_finsh_formation; }
            set{ _actual_finsh_formation = value; }
        }        
		/// <summary>
		/// 设计测深
        /// </summary>		
		private decimal _authorized_md;
        public decimal AUTHORIZED_MD
        {
            get{ return _authorized_md; }
            set{ _authorized_md = value; }
        }        
		/// <summary>
		/// 设计垂深
        /// </summary>		
		private decimal _authorized_tvd;
        public decimal AUTHORIZED_TVD
        {
            get{ return _authorized_tvd; }
            set{ _authorized_tvd = value; }
        }        
		/// <summary>
		/// 井底测深:井底测量深度
        /// </summary>		
		private decimal _bh_md;
        public decimal BH_MD
        {
            get{ return _bh_md; }
            set{ _bh_md = value; }
        }        
		/// <summary>
		/// 井底垂深:井底真垂直深度
        /// </summary>		
		private decimal _bh_tvd;
        public decimal BH_TVD
        {
            get{ return _bh_tvd; }
            set{ _bh_tvd = value; }
        }        
		/// <summary>
		/// 井底横坐标
        /// </summary>		
		private decimal _offset_east_bh;
        public decimal OFFSET_EAST_BH
        {
            get{ return _offset_east_bh; }
            set{ _offset_east_bh = value; }
        }        
		/// <summary>
		/// 井底纵坐标
        /// </summary>		
		private decimal _offset_north_bh;
        public decimal OFFSET_NORTH_BH
        {
            get{ return _offset_north_bh; }
            set{ _offset_north_bh = value; }
        }        
		/// <summary>
		/// 井底纬度
        /// </summary>		
		private decimal _latitude_bh;
        public decimal LATITUDE_BH
        {
            get{ return _latitude_bh; }
            set{ _latitude_bh = value; }
        }        
		/// <summary>
		/// 井底经度
        /// </summary>		
		private decimal _longitude_bh;
        public decimal LONGITUDE_BH
        {
            get{ return _longitude_bh; }
            set{ _longitude_bh = value; }
        }        
		/// <summary>
		/// 井底位置描述
        /// </summary>		
		private string _description_bh;
        public string DESCRIPTION_BH
        {
            get{ return _description_bh; }
            set{ _description_bh = value; }
        }        
		/// <summary>
		/// 回填深度
        /// </summary>		
		private decimal _plugback_md;
        public decimal PLUGBACK_MD
        {
            get{ return _plugback_md; }
            set{ _plugback_md = value; }
        }        
		/// <summary>
		/// 回填垂深
        /// </summary>		
		private decimal _plugback_tvd;
        public decimal PLUGBACK_TVD
        {
            get{ return _plugback_tvd; }
            set{ _plugback_tvd = value; }
        }        
		/// <summary>
		/// 是否斜井:是否是斜井，填写"Y"或"N"
        /// </summary>		
		private string _is_deviated;
        public string IS_DEVIATED
        {
            get{ return _is_deviated; }
            set{ _is_deviated = value; }
        }        
		/// <summary>
		/// 完钻日期
        /// </summary>		
		private DateTime _end_drilling_date;
        public DateTime END_DRILLING_DATE
        {
            get{ return _end_drilling_date; }
            set{ _end_drilling_date = value; }
        }        
		/// <summary>
		/// 完钻依据
        /// </summary>		
		private string _end_drilling_basis;
        public string END_DRILLING_BASIS
        {
            get{ return _end_drilling_basis; }
            set{ _end_drilling_basis = value; }
        }        
		/// <summary>
		/// 井筒类型
        /// </summary>		
		private string _wellbore_type;
        public string WELLBORE_TYPE
        {
            get{ return _wellbore_type; }
            set{ _wellbore_type = value; }
        }        
		/// <summary>
		/// 井筒产生日期
        /// </summary>		
		private DateTime _wellbore_production_date;
        public DateTime WELLBORE_PRODUCTION_DATE
        {
            get{ return _wellbore_production_date; }
            set{ _wellbore_production_date = value; }
        }        
		/// <summary>
		/// 和阻流环不一样
        /// </summary>		
		private decimal _artificial_wellbottom;
        public decimal ARTIFICIAL_WELLBOTTOM
        {
            get{ return _artificial_wellbottom; }
            set{ _artificial_wellbottom = value; }
        }        
		/// <summary>
		/// 造斜（侧钻）点井深
        /// </summary>		
		private decimal _deflection_point_md;
        public decimal DEFLECTION_POINT_MD
        {
            get{ return _deflection_point_md; }
            set{ _deflection_point_md = value; }
        }        
		/// <summary>
		/// 最大井斜
        /// </summary>		
		private decimal _max_well_deviation;
        public decimal MAX_WELL_DEVIATION
        {
            get{ return _max_well_deviation; }
            set{ _max_well_deviation = value; }
        }        
		/// <summary>
		/// 最大井斜对应井深
        /// </summary>		
		private decimal _max_well_deviation_md;
        public decimal MAX_WELL_DEVIATION_MD
        {
            get{ return _max_well_deviation_md; }
            set{ _max_well_deviation_md = value; }
        }        
		/// <summary>
		/// 行状态:行状态:行状态:标识说明: 0-Deleted 1-Added 2-Updated 3-Locked 4-Invalid 5-Undefined 6~64-保留标识 65~128-用户扩展(65-Transfered)
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
		/// 备注:备注:注释
        /// </summary>		
		private string _remark;
        public string REMARK
        {
            get{ return _remark; }
            set{ _remark = value; }
        }        
		   
	}
}

