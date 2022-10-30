using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //解释处理_大块原始数据归档
    [Serializable]
    public class PRO_LOG_BIG_ORIGINAL_DATA : ModelBase
    {
        /// <summary>
        /// 归档ID
        /// </summary>		
        private string _originalid;
        [Required]
        public string ORIGINALID
        {
            get { return _originalid; }
            set { _originalid = value; }
        }
        /// <summary>
        /// 解释处理ID
        /// </summary>		
        private string _process_id;
        public string PROCESS_ID
        {
            get { return _process_id; }
            set { _process_id = value; }
        }
        /// <summary>
        /// 下井趟次号
        /// </summary>		
        private decimal _down_well_sequence;
        public decimal DOWN_WELL_SEQUENCE
        {
            get { return _down_well_sequence; }
            set { _down_well_sequence = value; }
        }
        /// <summary>
        /// 测量方向
        /// </summary>		
        private string _measurement_direction;
        public string MEASUREMENT_DIRECTION
        {
            get { return _measurement_direction; }
            set { _measurement_direction = value; }
        }
        /// <summary>
        /// 原始数据文件名
        /// </summary>		
        private string _original_data_name;
        public string ORIGINAL_DATA_NAME
        {
            get { return _original_data_name; }
            set { _original_data_name = value; }
        }
        /// <summary>
        /// 原始数据文件大小
        /// </summary>		
        private decimal _original_data_size;
        public decimal ORIGINAL_DATA_SIZE
        {
            get { return _original_data_size; }
            set { _original_data_size = value; }
        }
        /// <summary>
        /// 原始数据类型
        /// </summary>		
        private string _original_data_type_;
        public string ORIGINAL_DATA_TYPE_
        {
            get { return _original_data_type_; }
            set { _original_data_type_ = value; }
        }
        /// <summary>
        /// 原始数据格式
        /// </summary>		
        private string _original_data_format_;
        public string ORIGINAL_DATA_FORMAT_
        {
            get { return _original_data_format_; }
            set { _original_data_format_ = value; }
        }
        /// <summary>
        /// 原始数据存放?
        /// </summary>		
        private string _original_data_storage;
        public string ORIGINAL_DATA_STORAGE
        {
            get { return _original_data_storage; }
            set { _original_data_storage = value; }
        }
        /// <summary>
        /// 归档日期
        /// </summary>		
        private DateTime? _archived_date;
        public DateTime? ARCHIVED_DATE
        {
            get { return _archived_date; }
            set { _archived_date = value; }
        }
        /// <summary>
        /// 归档人
        /// </summary>		
        private string _archived_person;
        public string ARCHIVED_PERSON
        {
            get { return _archived_person; }
            set { _archived_person = value; }
        }
        /// <summary>
        /// 归档审核人
        /// </summary>		
        private string _archived_verifier;
        public string ARCHIVED_VERIFIER
        {
            get { return _archived_verifier; }
            set { _archived_verifier = value; }
        }
    }
}

