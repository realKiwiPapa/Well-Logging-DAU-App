using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	/// <summary>
	/// 地层分层数据2
	/// </summary>
	[Serializable]
    public class COM_BASE_STRATA_LAYER2 : ModelBase
	{
		#region Model
		private decimal _seq_no;
		private string _job_plan_cd;
		private string _requisition_cd;
		private string _strat_unit_name;
		private string _strat_unit_s_name;
		private string _age_code;
		private string _stratum_code;
		private string _strat_rank;
		private string _parent_strat_rank;
		private string _rock_describe;
		private decimal? _bottom_depth;
		private decimal? _vertical_depth;
		private decimal? _bottom_height;
		private decimal? _vertical_thickness;
		private decimal? _slant_thicness;
		private string _relations;
		private string _date_type;
		private string _p_scheme_desc;
		private string _row_state;
		private string _create_org;
		private string _create_user;
		private string _update_org;
		private string _update_user;
		private DateTime? _create_date;
		private DateTime? _update_date;
		private string _remark;
		/// <summary>
		/// 序号
		/// </summary>
        [Required]
		public decimal SEQ_NO
		{
			set{ _seq_no=value;}
			get{return _seq_no;}
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
		/// 地层单位名称
		/// </summary>
		public string STRAT_UNIT_NAME
		{
			set{ _strat_unit_name=value;}
			get{return _strat_unit_name;}
		}
		/// <summary>
		/// 地层单位简称
		/// </summary>
		public string STRAT_UNIT_S_NAME
		{
			set{ _strat_unit_s_name=value;}
			get{return _strat_unit_s_name;}
		}
		/// <summary>
		/// 地质单位编码
		/// </summary>
		public string AGE_CODE
		{
			set{ _age_code=value;}
			get{return _age_code;}
		}
		/// <summary>
		/// 地层代码，包含系、组等，加特殊层提示
		/// </summary>
		public string STRATUM_CODE
		{
			set{ _stratum_code=value;}
			get{return _stratum_code;}
		}
		/// <summary>
		/// 地层单位级别
		/// </summary>
		public string STRAT_RANK
		{
			set{ _strat_rank=value;}
			get{return _strat_rank;}
		}
		/// <summary>
		/// 上级地层单位级别
		/// </summary>
		public string PARENT_STRAT_RANK
		{
			set{ _parent_strat_rank=value;}
			get{return _parent_strat_rank;}
		}
		/// <summary>
		/// 地层岩性综述
		/// </summary>
		public string ROCK_DESCRIBE
		{
			set{ _rock_describe=value;}
			get{return _rock_describe;}
		}
		/// <summary>
		/// 底部井深
		/// </summary>
		public decimal? BOTTOM_DEPTH
		{
			set{ _bottom_depth=value;}
			get{return _bottom_depth;}
		}
		/// <summary>
		/// 底部垂深
		/// </summary>
		public decimal? VERTICAL_DEPTH
		{
			set{ _vertical_depth=value;}
			get{return _vertical_depth;}
		}
		/// <summary>
		/// 底部海拔
		/// </summary>
		public decimal? BOTTOM_HEIGHT
		{
			set{ _bottom_height=value;}
			get{return _bottom_height;}
		}
		/// <summary>
		/// 垂厚
		/// </summary>
		public decimal? VERTICAL_THICKNESS
		{
			set{ _vertical_thickness=value;}
			get{return _vertical_thickness;}
		}
		/// <summary>
		/// 斜厚
		/// </summary>
		public decimal? SLANT_THICNESS
		{
			set{ _slant_thicness=value;}
			get{return _slant_thicness;}
		}
		/// <summary>
		/// 接触关系
		/// </summary>
		public string RELATIONS
		{
			set{ _relations=value;}
			get{return _relations;}
		}
		/// <summary>
		/// 数据分类
		/// </summary>
		public string DATE_TYPE
		{
			set{ _date_type=value;}
			get{return _date_type;}
		}
		/// <summary>
		/// 方案描述
		/// </summary>
		public string P_SCHEME_DESC
		{
			set{ _p_scheme_desc=value;}
			get{return _p_scheme_desc;}
		}
		/// <summary>
		/// 行状态:行状态:行状态:标识说明: 0-Deleted 1-Added 2-Updated 3-Locked 4-Invalid 5-Undefined 6~64-保留标识 65~128-用户扩展(65-Transfered)
		/// </summary>
		public string ROW_STATE
		{
			set{ _row_state=value;}
			get{return _row_state;}
		}
		/// <summary>
		/// 创建者所在单位
		/// </summary>
		public string CREATE_ORG
		{
			set{ _create_org=value;}
			get{return _create_org;}
		}
		/// <summary>
		/// 创建者
		/// </summary>
		public string CREATE_USER
		{
			set{ _create_user=value;}
			get{return _create_user;}
		}
		/// <summary>
		/// 修改者所在单位
		/// </summary>
		public string UPDATE_ORG
		{
			set{ _update_org=value;}
			get{return _update_org;}
		}
		/// <summary>
		/// 修改者
		/// </summary>
		public string UPDATE_USER
		{
			set{ _update_user=value;}
			get{return _update_user;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CREATE_DATE
		{
			set{ _create_date=value;}
			get{return _create_date;}
		}
		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime? UPDATE_DATE
		{
			set{ _update_date=value;}
			get{return _update_date;}
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string REMARK
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

