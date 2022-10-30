using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	/// <summary>
	/// 解释处理_综合成果数据，从极睿解释平台解释处理成果数据中导入，采样间距为0.125，但有的井次曲线有多，有少
	/// </summary>
	[Serializable]
    public class COM_LOG_COM_CURVEDATA : ModelBase
	{
		#region Model
		private decimal _curvedataid;
		private string _process_id;
		private decimal? _dep;
		private decimal? _sp;
		private decimal? _gr;
		private decimal? _cal;
		private decimal? _dev;
		private decimal? _daz;
		private decimal? _ac;
		private decimal? _cnl;
		private decimal? _den;
		private decimal? _pe;
		private decimal? _rt;
		private decimal? _rxo;
		private decimal? _sgr;
		private decimal? _cgr;
		private decimal? _u;
		private decimal? _th;
		private decimal? _k;
		/// <summary>
		/// 综合ID
		/// </summary>
        [Required]
		public decimal CURVEDATAID
		{
			set{ _curvedataid=value;}
			get{return _curvedataid;}
		}
		/// <summary>
		/// 解释处理ID
		/// </summary>
		public string PROCESS_ID
		{
			set{ _process_id=value;}
			get{return _process_id;}
		}
		/// <summary>
		/// 深度
		/// </summary>
		public decimal? DEP
		{
			set{ _dep=value;}
			get{return _dep;}
		}
		/// <summary>
		/// 自然电位
		/// </summary>
		public decimal? SP
		{
			set{ _sp=value;}
			get{return _sp;}
		}
		/// <summary>
		/// 自然伽玛
		/// </summary>
		public decimal? GR
		{
			set{ _gr=value;}
			get{return _gr;}
		}
		/// <summary>
		/// 井径
		/// </summary>
		public decimal? CAL
		{
			set{ _cal=value;}
			get{return _cal;}
		}
		/// <summary>
		/// 井斜
		/// </summary>
		public decimal? DEV
		{
			set{ _dev=value;}
			get{return _dev;}
		}
		/// <summary>
		/// 方位
		/// </summary>
		public decimal? DAZ
		{
			set{ _daz=value;}
			get{return _daz;}
		}
		/// <summary>
		/// 补偿声波
		/// </summary>
		public decimal? AC
		{
			set{ _ac=value;}
			get{return _ac;}
		}
		/// <summary>
		/// 补偿中子
		/// </summary>
		public decimal? CNL
		{
			set{ _cnl=value;}
			get{return _cnl;}
		}
		/// <summary>
		/// 补偿密度
		/// </summary>
		public decimal? DEN
		{
			set{ _den=value;}
			get{return _den;}
		}
		/// <summary>
		/// 光电指数
		/// </summary>
		public decimal? PE
		{
			set{ _pe=value;}
			get{return _pe;}
		}
		/// <summary>
		/// 深侧向
		/// </summary>
		public decimal? RT
		{
			set{ _rt=value;}
			get{return _rt;}
		}
		/// <summary>
		/// 浅侧向
		/// </summary>
		public decimal? RXO
		{
			set{ _rxo=value;}
			get{return _rxo;}
		}
		/// <summary>
		/// 自然伽玛总量
		/// </summary>
		public decimal? SGR
		{
			set{ _sgr=value;}
			get{return _sgr;}
		}
		/// <summary>
		/// 无铀伽玛
		/// </summary>
		public decimal? CGR
		{
			set{ _cgr=value;}
			get{return _cgr;}
		}
		/// <summary>
		/// 铀
		/// </summary>
		public decimal? U
		{
			set{ _u=value;}
			get{return _u;}
		}
		/// <summary>
		/// 钍
		/// </summary>
		public decimal? TH
		{
			set{ _th=value;}
			get{return _th;}
		}
		/// <summary>
		/// 钾
		/// </summary>
		public decimal? K
		{
			set{ _k=value;}
			get{return _k;}
		}
		#endregion Model
	}
}

