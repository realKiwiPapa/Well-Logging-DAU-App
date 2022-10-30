using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 测井原始数据 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_RAPID_ORIGINAL_DATA : ModelBase
    {
        private string _data_id;
        /// <summary>
        /// 数据体编码
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 45, CharUsed = CharUsedType.Byte)]
        public string DATA_ID
        {
            get { return _data_id; }
            set { _data_id = value; NotifyPropertyChanged("DATA_ID"); }
        }
        private string _job_plan_cd;
        /// <summary>
        /// 作业计划书编号
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string JOB_PLAN_CD
        {
            get { return _job_plan_cd; }
            set { _job_plan_cd = value; NotifyPropertyChanged("JOB_PLAN_CD"); }
        }
        private string _data_name;
        /// <summary>
        /// 为原始数据文件名
        /// </summary>
        [OracleStringLength(MaximumLength = 300, CharUsed = CharUsedType.Char)]
        public string DATA_NAME
        {
            get { return _data_name; }
            set { _data_name = value; NotifyPropertyChanged("DATA_NAME"); }
        }
        private string _original_type;
        /// <summary>
        /// 快速解释成果数据类型
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string ORIGINAL_TYPE
        {
            get { return _original_type; }
            set { _original_type = value; NotifyPropertyChanged("ORIGINAL_TYPE"); }
        }
        private decimal? _start_dep;
        /// <summary>
        /// 该数据体起始深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? START_DEP
        {
            get { return _start_dep; }
            set { _start_dep = value; NotifyPropertyChanged("START_DEP"); }
        }
        private decimal? _end_dep;
        /// <summary>
        /// 该数据体结束深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? END_DEP
        {
            get { return _end_dep; }
            set { _end_dep = value; NotifyPropertyChanged("END_DEP"); }
        }
        private string _log_item;
        /// <summary>
        /// 测井项目
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string LOG_ITEM
        {
            get { return _log_item; }
            set { _log_item = value; NotifyPropertyChanged("LOG_ITEM"); }
        }
        private decimal? _data_size;
        /// <summary>
        /// 数据体大小
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? DATA_SIZE
        {
            get { return _data_size; }
            set { _data_size = value; NotifyPropertyChanged("DATA_SIZE"); }
        }
        private DateTime? _submit_date;
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime? SUBMIT_DATE
        {
            get { return _submit_date; }
            set { _submit_date = value; NotifyPropertyChanged("SUBMIT_DATE"); }
        }
        private string _submit_man;
        /// <summary>
        /// 成果提交人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string SUBMIT_MAN
        {
            get { return _submit_man; }
            set { _submit_man = value; NotifyPropertyChanged("SUBMIT_MAN"); }
        }
        private string _data_recipient;
        /// <summary>
        /// 资料接收人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string DATA_RECIPIENT
        {
            get { return _data_recipient; }
            set { _data_recipient = value; NotifyPropertyChanged("DATA_RECIPIENT"); }
        }
        private byte[] _data;
        /// <summary>
        /// 数据体
        /// </summary>
        public byte[] DATA
        {
            get { return _data; }
            set { _data = value; NotifyPropertyChanged("DATA"); }
        }
        private string _note;
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }
        private decimal? _fileid;
        /// <summary>
        /// 文件ID
        /// </summary>
        public decimal? FILEID
        {
            get { return _fileid; }
            set { _fileid = value; NotifyPropertyChanged("FILEID"); }
        }

        [NonSerialized]
        public string FullName;
    }
}