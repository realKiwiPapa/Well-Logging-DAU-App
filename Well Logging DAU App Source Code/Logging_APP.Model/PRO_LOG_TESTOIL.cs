using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	/// <summary>
	/// 井试油参数
	/// </summary>
	[Serializable]
    public class PRO_LOG_TESTOIL : ModelBase
	{
		#region Model
		private string _testoilid;
		private string _job_plan_cd;
		private string _requisition_cd;
		private DateTime? _test_date;
		private string _layer_name;
		private decimal? _start_depth;
		private decimal? _end_depth;
		private decimal? _output_oil_every_day;
		private decimal? _output_water_every_day;
		private decimal? _output_gas_every_day;
		private decimal? _hydrosulfide;
		private decimal? _formation_pressure;
		private decimal? _choke;
		private decimal? _oil_pressure;
		private decimal? _casing_pressure;
		private string _test_conclusion;
		private string _detailed_description_test;
		private string _note;
		/// <summary>
		/// 试油ID
		/// </summary>
        [Required]
		public string TESTOILID
		{
			set{ _testoilid=value;}
			get{return _testoilid;}
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
		/// 试油日期
		/// </summary>
		public DateTime? TEST_DATE
		{
			set{ _test_date=value;}
			get{return _test_date;}
		}
		/// <summary>
		/// 层位名
		/// </summary>
		public string LAYER_NAME
		{
			set{ _layer_name=value;}
			get{return _layer_name;}
		}
		/// <summary>
		/// 始深度
		/// </summary>
		public decimal? START_DEPTH
		{
			set{ _start_depth=value;}
			get{return _start_depth;}
		}
		/// <summary>
		/// 结束深度
		/// </summary>
		public decimal? END_DEPTH
		{
			set{ _end_depth=value;}
			get{return _end_depth;}
		}
		/// <summary>
		/// 日产油
		/// </summary>
		public decimal? OUTPUT_OIL_EVERY_DAY
		{
			set{ _output_oil_every_day=value;}
			get{return _output_oil_every_day;}
		}
		/// <summary>
		/// 日产水
		/// </summary>
		public decimal? OUTPUT_WATER_EVERY_DAY
		{
			set{ _output_water_every_day=value;}
			get{return _output_water_every_day;}
		}
		/// <summary>
		/// 日产气
		/// </summary>
		public decimal? OUTPUT_GAS_EVERY_DAY
		{
			set{ _output_gas_every_day=value;}
			get{return _output_gas_every_day;}
		}
		/// <summary>
		/// H2S
		/// </summary>
		public decimal? HYDROSULFIDE
		{
			set{ _hydrosulfide=value;}
			get{return _hydrosulfide;}
		}
		/// <summary>
		/// 地层压力
		/// </summary>
		public decimal? FORMATION_PRESSURE
		{
			set{ _formation_pressure=value;}
			get{return _formation_pressure;}
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
		/// 油压
		/// </summary>
		public decimal? OIL_PRESSURE
		{
			set{ _oil_pressure=value;}
			get{return _oil_pressure;}
		}
		/// <summary>
		/// 套压
		/// </summary>
		public decimal? CASING_PRESSURE
		{
			set{ _casing_pressure=value;}
			get{return _casing_pressure;}
		}
		/// <summary>
		/// 试油结论
		/// </summary>
		public string TEST_CONCLUSION
		{
			set{ _test_conclusion=value;}
			get{return _test_conclusion;}
		}
		/// <summary>
		/// 试油详细文字描述
		/// </summary>
		public string DETAILED_DESCRIPTION_TEST
		{
			set{ _detailed_description_test=value;}
			get{return _detailed_description_test;}
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

