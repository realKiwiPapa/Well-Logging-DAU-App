using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	/// <summary>
	/// 311.2mm×3162.68m(直径为311.2毫米的钻头钻到3162.68米)
	/// </summary>
	[Serializable]
    public class PRO_LOG_BIT_PROGRAM : ModelBase
	{
		#region Model
		private string _bitid;
		private string _job_plan_cd;
		private string _requisition_cd;
		private DateTime? _update_date;
		private decimal? _bit_size;
		private decimal? _bit_dep;
		private string _note;
		/// <summary>
		/// 钻头ID
		/// </summary>
        [Required]
		public string BITID
		{
			set{ _bitid=value;}
			get{return _bitid;}
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
		/// 钻头直径
		/// </summary>
		public decimal? BIT_SIZE
		{
			set{ _bit_size=value;}
			get{return _bit_size;}
		}
		/// <summary>
		/// 钻头钻达井深
		/// </summary>
		public decimal? BIT_DEP
		{
			set{ _bit_dep=value;}
			get{return _bit_dep;}
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

