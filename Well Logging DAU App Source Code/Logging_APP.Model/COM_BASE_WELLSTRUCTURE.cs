using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	 	//井身结构数据
    [Serializable]
    public class COM_BASE_WELLSTRUCTURE : ModelBase
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
		private string _wellbore_id;
        public string WELLBORE_ID
        {
            get{ return _wellbore_id; }
            set{ _wellbore_id = value; }
        }        
		/// <summary>
		/// 井身结构ID
        /// </summary>		
        [Required]
		private string _well_structure_id;
        public string WELL_STRUCTURE_ID
        {
            get{ return _well_structure_id; }
            set{ _well_structure_id = value; }
        }        
		/// <summary>
		/// 序号
        /// </summary>		
		private decimal _no;
        public decimal NO
        {
            get{ return _no; }
            set{ _no = value; }
        }        
		/// <summary>
		/// 开钻次序
        /// </summary>		
		private string _spudin_no;
        public string SPUDIN_NO
        {
            get{ return _spudin_no; }
            set{ _spudin_no = value; }
        }        
		/// <summary>
		/// 套管名称
        /// </summary>		
		private string _casing_name;
        public string CASING_NAME
        {
            get{ return _casing_name; }
            set{ _casing_name = value; }
        }        
		/// <summary>
		/// 井深
        /// </summary>		
		private decimal _well_depth;
        public decimal WELL_DEPTH
        {
            get{ return _well_depth; }
            set{ _well_depth = value; }
        }        
		/// <summary>
		/// 套管下入层位
        /// </summary>		
		private string _layer;
        public string LAYER
        {
            get{ return _layer; }
            set{ _layer = value; }
        }        
		/// <summary>
		/// 井眼尺寸
        /// </summary>		
		private decimal _wellbore_size;
        public decimal WELLBORE_SIZE
        {
            get{ return _wellbore_size; }
            set{ _wellbore_size = value; }
        }        
		/// <summary>
		/// 阻流环1深度
        /// </summary>		
		private decimal _shut_off_baffle1_depth;
        public decimal SHUT_OFF_BAFFLE1_DEPTH
        {
            get{ return _shut_off_baffle1_depth; }
            set{ _shut_off_baffle1_depth = value; }
        }        
		/// <summary>
		/// 阻流环2深度
        /// </summary>		
		private decimal _shut_off_baffle2_depth;
        public decimal SHUT_OFF_BAFFLE2_DEPTH
        {
            get{ return _shut_off_baffle2_depth; }
            set{ _shut_off_baffle2_depth = value; }
        }        
		/// <summary>
		/// 水泥外返深(一级)
        /// </summary>		
		private decimal _cement_top1;
        public decimal CEMENT_TOP1
        {
            get{ return _cement_top1; }
            set{ _cement_top1 = value; }
        }        
		/// <summary>
		/// 水泥外返深(二级)
        /// </summary>		
		private decimal _cement_top2;
        public decimal CEMENT_TOP2
        {
            get{ return _cement_top2; }
            set{ _cement_top2 = value; }
        }        
		/// <summary>
		/// 水泥外返深(三级)
        /// </summary>		
		private decimal _cement_top3;
        public decimal CEMENT_TOP3
        {
            get{ return _cement_top3; }
            set{ _cement_top3 = value; }
        }        
		/// <summary>
		/// 分级注水泥接箍1深度
        /// </summary>		
		private decimal _stage_collar1_depth;
        public decimal STAGE_COLLAR1_DEPTH
        {
            get{ return _stage_collar1_depth; }
            set{ _stage_collar1_depth = value; }
        }        
		/// <summary>
		/// 分级注水泥接箍2深度
        /// </summary>		
		private decimal _stage_collar2_depth;
        public decimal STAGE_COLLAR2_DEPTH
        {
            get{ return _stage_collar2_depth; }
            set{ _stage_collar2_depth = value; }
        }        
		/// <summary>
		/// 不是悬挂，套管顶深为0
        /// </summary>		
		private string _cement_method;
        public string CEMENT_METHOD
        {
            get{ return _cement_method; }
            set{ _cement_method = value; }
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

