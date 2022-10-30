using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	/// <summary>
	/// 小队施工_生产参数
	/// </summary>
	[Serializable]
    public class PRO_LOG_PRODUCE : ModelBase
	{
		#region Model
		private string _workid;
		private string _job_plan_cd;
		private string _requisition_cd;
		private DateTime? _update_date;
		private decimal? _oil_shoe_depth;
		private string _well_fluid_type;
		private decimal? _oil_line_inner_diameter;
		private decimal? _bellmouth_depth;
		private decimal? _choke;
		private decimal? _sul_hyd;
		private decimal? _wellhead_press;
		private decimal? _wellbottom_press;
		private string _wellhead_flange_type;
		private decimal? _water_pro_per_day;
		private decimal? _air_day_production;
		private decimal? _oil_pro_per_day;
		private decimal? _casing_pressure;
		private decimal? _oil_pressure;
		private string _note;
		/// <summary>
		/// 工作ID
		/// </summary>
        [Required]
		public string WORKID
		{
			set{ _workid=value;}
			get{return _workid;}
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
		/// 油管鞋深度
		/// </summary>
		public decimal? OIL_SHOE_DEPTH
		{
			set{ _oil_shoe_depth=value;}
			get{return _oil_shoe_depth;}
		}
		/// <summary>
		/// 井液类型
		/// </summary>
		public string WELL_FLUID_TYPE
		{
			set{ _well_fluid_type=value;}
			get{return _well_fluid_type;}
		}
		/// <summary>
		/// 油管内径
		/// </summary>
		public decimal? OIL_LINE_INNER_DIAMETER
		{
			set{ _oil_line_inner_diameter=value;}
			get{return _oil_line_inner_diameter;}
		}
		/// <summary>
		/// 喇叭口深度
		/// </summary>
		public decimal? BELLMOUTH_DEPTH
		{
			set{ _bellmouth_depth=value;}
			get{return _bellmouth_depth;}
		}
		/// <summary>
		/// 油嘴
		/// </summary>
		public decimal? CHOKE
		{
			set{ _choke=value;}
			get{return _choke;}
		}
		/// <summary>
		/// 硫化氢含量
		/// </summary>
		public decimal? SUL_HYD
		{
			set{ _sul_hyd=value;}
			get{return _sul_hyd;}
		}
		/// <summary>
		/// 井口压力
		/// </summary>
		public decimal? WELLHEAD_PRESS
		{
			set{ _wellhead_press=value;}
			get{return _wellhead_press;}
		}
		/// <summary>
		/// 井底压力
		/// </summary>
		public decimal? WELLBOTTOM_PRESS
		{
			set{ _wellbottom_press=value;}
			get{return _wellbottom_press;}
		}
		/// <summary>
		/// 井口法兰型号
		/// </summary>
		public string WELLHEAD_FLANGE_TYPE
		{
			set{ _wellhead_flange_type=value;}
			get{return _wellhead_flange_type;}
		}
		/// <summary>
		/// 日产水
		/// </summary>
		public decimal? WATER_PRO_PER_DAY
		{
			set{ _water_pro_per_day=value;}
			get{return _water_pro_per_day;}
		}
		/// <summary>
		/// 日产气
		/// </summary>
		public decimal? AIR_DAY_PRODUCTION
		{
			set{ _air_day_production=value;}
			get{return _air_day_production;}
		}
		/// <summary>
		/// 日产油
		/// </summary>
		public decimal? OIL_PRO_PER_DAY
		{
			set{ _oil_pro_per_day=value;}
			get{return _oil_pro_per_day;}
		}
		/// <summary>
		/// 套管压力
		/// </summary>
		public decimal? CASING_PRESSURE
		{
			set{ _casing_pressure=value;}
			get{return _casing_pressure;}
		}
		/// <summary>
		/// 油管压力
		/// </summary>
		public decimal? OIL_PRESSURE
		{
			set{ _oil_pressure=value;}
			get{return _oil_pressure;}
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string NOTE
		{
			set{ _note=value;}
			get{return _note;}
		}
		#endregion Model

	}
}

