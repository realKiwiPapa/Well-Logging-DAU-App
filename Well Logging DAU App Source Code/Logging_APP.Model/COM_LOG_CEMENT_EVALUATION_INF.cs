using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	/// <summary>
	/// 解释处理_估计质量分段评价数据,A1中有此表
	/// </summary>
	[Serializable]
	public class COM_LOG_CEMENT_EVALUATION_INF: ModelBase
	{
		#region Model
		private decimal _cementid;
		private string _process_id;
		private decimal? _st_dep;
		private decimal? _en_dep;
		private decimal? _max_cbl;
		private decimal? _min_cbl;
		private decimal? _mea_cbl;
		private string _result;
		private string _note;
		/// <summary>
		/// 固井质量ID
		/// </summary>
        [Required]
		public decimal CEMENTID
		{
			set{ _cementid=value;}
			get{return _cementid;}
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
		/// 起始深度
		/// </summary>
		public decimal? ST_DEP
		{
			set{ _st_dep=value;}
			get{return _st_dep;}
		}
		/// <summary>
		/// 结束深度
		/// </summary>
		public decimal? EN_DEP
		{
			set{ _en_dep=value;}
			get{return _en_dep;}
		}
		/// <summary>
		/// 最大声幅
		/// </summary>
		public decimal? MAX_CBL
		{
			set{ _max_cbl=value;}
			get{return _max_cbl;}
		}
		/// <summary>
		/// 最小声幅
		/// </summary>
		public decimal? MIN_CBL
		{
			set{ _min_cbl=value;}
			get{return _min_cbl;}
		}
		/// <summary>
		/// 平均声幅
		/// </summary>
		public decimal? MEA_CBL
		{
			set{ _mea_cbl=value;}
			get{return _mea_cbl;}
		}
		/// <summary>
		/// 结论
		/// </summary>
		public string RESULT
		{
			set{ _result=value;}
			get{return _result;}
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

