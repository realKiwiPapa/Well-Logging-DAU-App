using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	 	//小队施工_套管参数
    [Serializable]
    public class PRO_LOG_CASIN : ModelBase
	{
   		     
      	/// <summary>
		/// 套管ID
        /// </summary>	
        [Required]
		private string _casinid;
        public string CASINID
        {
            get{ return _casinid; }
            set{ _casinid = value; }
        }        
		/// <summary>
		/// 作业计划书编号
        /// </summary>		
		private string _job_plan_cd;
        public string JOB_PLAN_CD
        {
            get{ return _job_plan_cd; }
            set{ _job_plan_cd = value; }
        }        
		/// <summary>
		/// 通知单编码
        /// </summary>		
		private string _requisition_cd;
        public string REQUISITION_CD
        {
            get{ return _requisition_cd; }
            set{ _requisition_cd = value; }
        }        
		/// <summary>
		/// 同步更新日期
        /// </summary>		
		private DateTime _update_date;
        public DateTime UPDATE_DATE
        {
            get{ return _update_date; }
            set{ _update_date = value; }
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
		/// 套管类型
        /// </summary>		
		private string _casing_type;
        public string CASING_TYPE
        {
            get{ return _casing_type; }
            set{ _casing_type = value; }
        }        
		/// <summary>
		/// 套管外径
        /// </summary>		
		private decimal _casing_outsize;
        public decimal CASING_OUTSIZE
        {
            get{ return _casing_outsize; }
            set{ _casing_outsize = value; }
        }        
		/// <summary>
		/// 套管数量
        /// </summary>		
		private decimal _casing_quantity;
        public decimal CASING_QUANTITY
        {
            get{ return _casing_quantity; }
            set{ _casing_quantity = value; }
        }        
		/// <summary>
		/// 套管扣型
        /// </summary>		
		private string _casing_thread_type;
        public string CASING_THREAD_TYPE
        {
            get{ return _casing_thread_type; }
            set{ _casing_thread_type = value; }
        }        
		/// <summary>
		/// 套管钢级
        /// </summary>		
		private string _casing_grade;
        public string CASING_GRADE
        {
            get{ return _casing_grade; }
            set{ _casing_grade = value; }
        }        
		/// <summary>
		/// 套管壁厚
        /// </summary>		
		private decimal _pipe_thickness;
        public decimal PIPE_THICKNESS
        {
            get{ return _pipe_thickness; }
            set{ _pipe_thickness = value; }
        }        
		/// <summary>
		/// 套管长度
        /// </summary>		
		private decimal _p_xn_casing_length;
        public decimal P_XN_CASING_LENGTH
        {
            get{ return _p_xn_casing_length; }
            set{ _p_xn_casing_length = value; }
        }        
		/// <summary>
		/// 累计长度
        /// </summary>		
		private decimal _accumulative_length;
        public decimal ACCUMULATIVE_LENGTH
        {
            get{ return _accumulative_length; }
            set{ _accumulative_length = value; }
        }        
		/// <summary>
		/// 下入底深
        /// </summary>		
		private decimal _running_bottom_depth;
        public decimal RUNNING_BOTTOM_DEPTH
        {
            get{ return _running_bottom_depth; }
            set{ _running_bottom_depth = value; }
        }        
		/// <summary>
		/// 上扣扭矩
        /// </summary>		
		private decimal _screw_on_torque;
        public decimal SCREW_ON_TORQUE
        {
            get{ return _screw_on_torque; }
            set{ _screw_on_torque = value; }
        }        
		/// <summary>
		/// 扶正器位置
        /// </summary>		
		private decimal _centering_device_position;
        public decimal CENTERING_DEVICE_POSITION
        {
            get{ return _centering_device_position; }
            set{ _centering_device_position = value; }
        }        
		/// <summary>
		/// 扶正器数量
        /// </summary>		
		private decimal _centering_device_no;
        public decimal CENTERING_DEVICE_NO
        {
            get{ return _centering_device_no; }
            set{ _centering_device_no = value; }
        }        
		/// <summary>
		/// 扶正器类型
        /// </summary>		
		private string _centering_device_type;
        public string CENTERING_DEVICE_TYPE
        {
            get{ return _centering_device_type; }
            set{ _centering_device_type = value; }
        }        
		/// <summary>
		/// 扶正器尺寸
        /// </summary>		
		private decimal _centering_device_size;
        public decimal CENTERING_DEVICE_SIZE
        {
            get{ return _centering_device_size; }
            set{ _centering_device_size = value; }
        }        
		/// <summary>
		/// 基管内径
        /// </summary>		
		private decimal _basetube_boresize;
        public decimal BASETUBE_BORESIZE
        {
            get{ return _basetube_boresize; }
            set{ _basetube_boresize = value; }
        }        
		/// <summary>
		/// 基管外径
        /// </summary>		
		private decimal _basetube_outsize;
        public decimal BASETUBE_OUTSIZE
        {
            get{ return _basetube_outsize; }
            set{ _basetube_outsize = value; }
        }        
		/// <summary>
		/// 基管每米孔数
        /// </summary>		
		private decimal _basetube_unitnumber;
        public decimal BASETUBE_UNITNUMBER
        {
            get{ return _basetube_unitnumber; }
            set{ _basetube_unitnumber = value; }
        }        
		/// <summary>
		/// 基管类型
        /// </summary>		
		private string _basetube_type;
        public string BASETUBE_TYPE
        {
            get{ return _basetube_type; }
            set{ _basetube_type = value; }
        }        
		/// <summary>
		/// 基管缝宽
        /// </summary>		
		private decimal _basetube_slotwidth;
        public decimal BASETUBE_SLOTWIDTH
        {
            get{ return _basetube_slotwidth; }
            set{ _basetube_slotwidth = value; }
        }        
		/// <summary>
		/// 基管孔径
        /// </summary>		
		private decimal _basetube_porediameter;
        public decimal BASETUBE_POREDIAMETER
        {
            get{ return _basetube_porediameter; }
            set{ _basetube_porediameter = value; }
        }        
		/// <summary>
		/// 筛管缝宽
        /// </summary>		
		private decimal _sievetube_slotwidth;
        public decimal SIEVETUBE_SLOTWIDTH
        {
            get{ return _sievetube_slotwidth; }
            set{ _sievetube_slotwidth = value; }
        }        
		/// <summary>
		/// 筛管缝长
        /// </summary>		
		private decimal _sievetube_slotlength;
        public decimal SIEVETUBE_SLOTLENGTH
        {
            get{ return _sievetube_slotlength; }
            set{ _sievetube_slotlength = value; }
        }        
		/// <summary>
		/// 套管下深
        /// </summary>		
		private decimal _pipe_height;
        public decimal PIPE_HEIGHT
        {
            get{ return _pipe_height; }
            set{ _pipe_height = value; }
        }        
		/// <summary>
		/// 填充深度 
        /// </summary>		
		private decimal _current_log_fill;
        public decimal CURRENT_LOG_FILL
        {
            get{ return _current_log_fill; }
            set{ _current_log_fill = value; }
        }        
		/// <summary>
		/// 回填深度 
        /// </summary>		
		private decimal _current_log_plugback;
        public decimal CURRENT_LOG_PLUGBACK
        {
            get{ return _current_log_plugback; }
            set{ _current_log_plugback = value; }
        }        
		/// <summary>
		///  备注
        /// </summary>		
		private string _note;
        public string NOTE
        {
            get{ return _note; }
            set{ _note = value; }
        }        
		   
	}
}

