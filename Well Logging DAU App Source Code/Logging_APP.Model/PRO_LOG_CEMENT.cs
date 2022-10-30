using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	/// <summary>
	/// 小队施工-固井参数
	/// </summary>
	[Serializable]
    public class PRO_LOG_CEMENT : ModelBase
	{
		#region Model
		private string _cementid;
		private string _job_plan_cd;
		private string _requisition_cd;
		private DateTime? _update_date;
        private string _cement_properties;
		private decimal? _cement_density_max_value;
        private decimal? _cement_density_min_value;
        private string _cement_type;
        private string _cemented_quantity;
		private decimal? _casing_shoe_depth;
		private decimal? _cement_pre_top;
		private decimal? _cement_height;
		private DateTime? _cement_well_date;
		private DateTime? _open_well_date;
		private decimal? _distance_tubing_and_bushing;
		private decimal? _casing_top_spacing;
		/// <summary>
		/// 固井ID
		/// </summary>
        [Required]
		public string CEMENTID
		{
			set{ _cementid=value;}
			get{return _cementid;}
		}
		/// <summary>
		/// 作业计划书编号
		/// </summary>
		public string JOB_PLAN_CD
		{
			set{ _job_plan_cd=value;}
			get{return _job_plan_cd;}
		}
		/// <summary>
		/// 通知单编码
		/// </summary>
		public string REQUISITION_CD
		{
			set{ _requisition_cd=value;}
			get{return _requisition_cd;}
		}
		/// <summary>
		/// 同步更新日期
		/// </summary>
		public DateTime? UPDATE_DATE
		{
			set{ _update_date=value;}
			get{return _update_date;}
		}
		/// <summary>
		/// 水泥性质
		/// </summary>
		public string CEMENT_PROPERTIES
		{
			set{ _cement_properties=value;}
			get{return _cement_properties;}
		}
		/// <summary>
		/// 水泥浆密度最大值
		/// </summary>
        public decimal? CEMENT_DENSITY_MAX_VALUE
		{
			set{ _cement_density_max_value=value;}
            get { return _cement_density_max_value; }
		}
        /// <summary>
        /// 水泥浆密度最小值
        /// </summary>
        public decimal? CEMENT_DENSITY_MIN_VALUE
        {
            set { _cement_density_min_value = value; }
            get { return _cement_density_min_value; }
        }
        /// <summary>
        /// 水泥浆类型
        /// </summary>
        public string CEMENT_TYPE
        {
            set { _cement_type = value; }
            get { return _cement_type; }
        }
		/// <summary>
		/// 已注水泥量
		/// </summary>
        public string CEMENTED_QUANTITY
		{
			set{ _cemented_quantity=value;}
			get{return _cemented_quantity;}
		}
		/// <summary>
		/// 套管鞋深度
		/// </summary>
		public decimal? CASING_SHOE_DEPTH
		{
			set{ _casing_shoe_depth=value;}
			get{return _casing_shoe_depth;}
		}
		/// <summary>
		/// 水泥设计返高
		/// </summary>
		public decimal? CEMENT_PRE_TOP
		{
			set{ _cement_pre_top=value;}
			get{return _cement_pre_top;}
		}
		/// <summary>
		/// 水泥实际返高
		/// </summary>
		public decimal? CEMENT_HEIGHT
		{
			set{ _cement_height=value;}
			get{return _cement_height;}
		}
		/// <summary>
		/// 固井日期
		/// </summary>
		public DateTime? CEMENT_WELL_DATE
		{
			set{ _cement_well_date=value;}
			get{return _cement_well_date;}
		}
		/// <summary>
		/// 开井日期
		/// </summary>
		public DateTime? OPEN_WELL_DATE
		{
			set{ _open_well_date=value;}
			get{return _open_well_date;}
		}
		/// <summary>
		/// 油补距
		/// </summary>
		public decimal? DISTANCE_TUBING_AND_BUSHING
		{
			set{ _distance_tubing_and_bushing=value;}
			get{return _distance_tubing_and_bushing;}
		}
		/// <summary>
		/// 套补距
		/// </summary>
		public decimal? CASING_TOP_SPACING
		{
			set{ _casing_top_spacing=value;}
			get{return _casing_top_spacing;}
		}
		#endregion Model

	}
}

