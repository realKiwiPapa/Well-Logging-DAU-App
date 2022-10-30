using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	/// <summary>
	/// 小队施工_钻井液参数
	/// </summary>
	[Serializable]
    public class PRO_LOG_SLOP : ModelBase
	{
		#region Model
		private string _mudid;
		private string _job_plan_cd;
		private string _requisition_cd;
		private DateTime? _update_date;
		private string _jtlt;
		private string _slop_properties;
		private decimal? _p_xn_mud_density;
		private decimal? _slop_persent;
		private decimal? _slop_ph;
		private decimal? _drill_flu_visc;
		private decimal? _slop_temperature;
		private decimal? _slop_resistivity;
		private decimal? _drilling_fluid_salinity;
		private decimal? _mud_filtrate_salinity;
		private decimal? _cake_density;
		/// <summary>
		/// 钻井液ID
		/// </summary>
        [Required]
		public string MUDID
		{
			set{ _mudid=value;}
			get{return _mudid;}
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
		/// 就是钻井液的中文名字，为A1字段
		/// </summary>
		public string JTLT
		{
			set{ _jtlt=value;}
			get{return _jtlt;}
		}
		/// <summary>
		/// 钻井液类型
		/// </summary>
		public string SLOP_PROPERTIES
		{
			set{ _slop_properties=value;}
			get{return _slop_properties;}
		}
		/// <summary>
		/// 钻井液密度
		/// </summary>
		public decimal? P_XN_MUD_DENSITY
		{
			set{ _p_xn_mud_density=value;}
			get{return _p_xn_mud_density;}
		}
		/// <summary>
		/// 钻井液比重(g/cm)
		/// </summary>
		public decimal? SLOP_PERSENT
		{
			set{ _slop_persent=value;}
			get{return _slop_persent;}
		}
		/// <summary>
		/// 钻井液PH值
		/// </summary>
		public decimal? SLOP_PH
		{
			set{ _slop_ph=value;}
			get{return _slop_ph;}
		}
		/// <summary>
		/// 钻井液粘度(S)
		/// </summary>
		public decimal? DRILL_FLU_VISC
		{
			set{ _drill_flu_visc=value;}
			get{return _drill_flu_visc;}
		}
		/// <summary>
		/// 钻井液温度
		/// </summary>
		public decimal? SLOP_TEMPERATURE
		{
			set{ _slop_temperature=value;}
			get{return _slop_temperature;}
		}
		/// <summary>
		/// 钻井液电阻率
		/// </summary>
		public decimal? SLOP_RESISTIVITY
		{
			set{ _slop_resistivity=value;}
			get{return _slop_resistivity;}
		}
		/// <summary>
		/// 钻井液矿化度
		/// </summary>
		public decimal? DRILLING_FLUID_SALINITY
		{
			set{ _drilling_fluid_salinity=value;}
			get{return _drilling_fluid_salinity;}
		}
		/// <summary>
		/// 钻井液滤液矿化度
		/// </summary>
		public decimal? MUD_FILTRATE_SALINITY
		{
			set{ _mud_filtrate_salinity=value;}
			get{return _mud_filtrate_salinity;}
		}
		/// <summary>
		/// 泥饼密度
		/// </summary>
		public decimal? CAKE_DENSITY
		{
			set{ _cake_density=value;}
			get{return _cake_density;}
		}
		#endregion Model

	}
}

