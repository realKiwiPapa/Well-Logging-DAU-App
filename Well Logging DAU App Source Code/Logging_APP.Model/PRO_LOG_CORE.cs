using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
	/// <summary>
	/// С��ʩ��_ȡ�Ĳ���
	/// </summary>
	[Serializable]
    public class PRO_LOG_CORE : ModelBase
	{
		#region Model
		private string _coreid;
		private string _job_plan_cd;
        private string _requisition_cd;
        private DateTime? _update_date;
		private string _formation_name;
		private decimal? _start_depth;
		private decimal? _end_depth;
		private string _core_word_description;
		private byte[] _core_gr_data;
		private string _core_picture_description;
		/// <summary>
		/// ȡ��ID
		/// </summary>
        [Required]
		public string COREID
		{
			set{ _coreid=value;}
			get{return _coreid.TrimCharEnd();}
		}
		/// <summary>
		/// ��ҵ�ƻ�����
		/// </summary>
		public string JOB_PLAN_CD
		{
			set{ _job_plan_cd=value;}
			get{return _job_plan_cd;}
		}
        /// <summary>
        /// ֪ͨ������
        /// </summary>
        public string REQUISITION_CD
        {
            set { _requisition_cd = value; }
            get { return _requisition_cd; }
        }
		/// <summary>
		/// ͬ����������
		/// </summary>
        public DateTime? UPDATE_DATE
		{
            set { _update_date = value; NotifyPropertyChanged("UPDATE_DATE"); }
			get{return _update_date;}
		}
		/// <summary>
		/// ��λ��
		/// </summary>
		public string FORMATION_NAME
		{
            set { _formation_name = value; NotifyPropertyChanged("FORMATION_NAME"); }
            get { return _formation_name.TrimCharEnd(); }
		}
		/// <summary>
		/// ʼ���
		/// </summary>
		public decimal? START_DEPTH
		{
            set { _start_depth = value; NotifyPropertyChanged("START_DEPTH"); }
			get{return _start_depth;}
		}
		/// <summary>
		/// ֹ���
		/// </summary>
		public decimal? END_DEPTH
		{
            set { _end_depth = value; NotifyPropertyChanged("END_DEPTH"); }
			get{return _end_depth;}
		}
		/// <summary>
		/// ȡо��������
		/// </summary>
		public string CORE_WORD_DESCRIPTION
		{
            set { _core_word_description = value; NotifyPropertyChanged("CORE_WORD_DESCRIPTION"); }
            get { return _core_word_description.TrimCharEnd(); }
		}
		/// <summary>
		/// ��оGR����
		/// </summary>
		public byte[] CORE_GR_DATA
		{
			set{ _core_gr_data=value;}
			get{return _core_gr_data;}
		}
		/// <summary>
		/// ȡоͼƬ����
		/// </summary>
		public string CORE_PICTURE_DESCRIPTION
		{
            set { _core_picture_description = value; NotifyPropertyChanged("CORE_PICTURE_DESCRIPTION"); }
            get { return _core_picture_description.TrimCharEnd(); }
		}
		#endregion Model

	}
}

