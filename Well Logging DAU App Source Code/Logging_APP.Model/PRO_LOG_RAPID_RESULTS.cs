using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队施工_现场快速解释成果表 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_RAPID_RESULTS : ModelBase
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
        private string _requisition_cd;
        /// <summary>
        /// 通知单编码
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string REQUISITION_CD
        {
            get { return _requisition_cd; }
            set { _requisition_cd = value; NotifyPropertyChanged("REQUISITION_CD"); }
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
        private string _rapid_results_type;
        /// <summary>
        /// 快速解释成果数据类型
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string RAPID_RESULTS_TYPE
        {
            get { return _rapid_results_type; }
            set { _rapid_results_type = value; NotifyPropertyChanged("RAPID_RESULTS_TYPE"); }
        }
        private decimal? _start_dep;
        /// <summary>
        /// 起始深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? START_DEP
        {
            get { return _start_dep; }
            set { _start_dep = value; NotifyPropertyChanged("START_DEP"); }
        }
        private decimal? _end_dep;
        /// <summary>
        /// 结束深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? END_DEP
        {
            get { return _end_dep; }
            set { _end_dep = value; NotifyPropertyChanged("END_DEP"); }
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
        private string _filename;
        /// <summary>
        /// 数据文件名
        /// </summary>
        [OracleStringLength(MaximumLength = 300, CharUsed = CharUsedType.Char)]
        public string FILENAME
        {
            get { return _filename; }
            set { _filename = value; NotifyPropertyChanged("FILENAME"); }
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