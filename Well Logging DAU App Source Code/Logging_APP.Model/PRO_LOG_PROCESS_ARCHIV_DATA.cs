using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //解释处理_成果大块数据归档
    [Serializable]
    public class PRO_LOG_PROCESS_ARCHIV_DATA : ModelBase
    {
        /// <summary>
        /// 归档ID
        /// </summary>		
        private string _archivid;
        [Required]
        public string ARCHIVID
        {
            get { return _archivid; }
            set { _archivid = value; }
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
        /// 解释处理项目编码
        /// </summary>		
        private string _processing_item_id;
        public string PROCESSING_ITEM_ID
        {
            get { return _processing_item_id; }
            set { _processing_item_id = value; }
        }
        /// <summary>
        /// 归档数据文件名
        /// </summary>		
        private string _archiv_data_name;
        public string ARCHIV_DATA_NAME
        {
            get { return _archiv_data_name; }
            set { _archiv_data_name = value; }
        }
        /// <summary>
        /// 归档数据文件大小
        /// </summary>		
        private decimal _archiv_data_size;
        public decimal ARCHIV_DATA_SIZE
        {
            get { return _archiv_data_size; }
            set { _archiv_data_size = value; }
        }
        /// <summary>
        /// 归档数据格式
        /// </summary>		
        private string _archiv_data_format;
        public string ARCHIV_DATA_FORMAT
        {
            get { return _archiv_data_format; }
            set { _archiv_data_format = value; }
        }
        /// <summary>
        /// 归档数据存放?
        /// </summary>		
        private string _archiv_data_storage;
        public string ARCHIV_DATA_STORAGE
        {
            get { return _archiv_data_storage; }
            set { _archiv_data_storage = value; }
        }
        /// <summary>
        /// 归档日期
        /// </summary>		
        private DateTime? _archiv_date;
        public DateTime? ARCHIV_DATE
        {
            get { return _archiv_date; }
            set { _archiv_date = value; }
        }
        /// <summary>
        /// 归档人
        /// </summary>		
        private string _archiv_person;
        public string ARCHIV_PERSON
        {
            get { return _archiv_person; }
            set { _archiv_person = value; }
        }
        /// <summary>
        /// 归档审核人
        /// </summary>		
        private string _archiv_verifier;
        public string ARCHIV_VERIFIER
        {
            get { return _archiv_verifier; }
            set { _archiv_verifier = value; }
        }
    }
}
