using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //解释处理_文档
    [Serializable]
    public class PRO_LOG_PROCESS_DOCUMENT : ModelBase
    {
        /// <summary>
        /// 文档ID
        /// </summary>		
        private string _documentid;
        [Required]
        public string DOCUMENTID
        {
            get { return _documentid; }
            set { _documentid = value; }
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
        /// 文档名
        /// </summary>		
        private string _document_name;
        public string DOCUMENT_NAME
        {
            get { return _document_name; }
            set { _document_name = value; }
        }
        /// <summary>
        /// 文档类型
        /// </summary>		
        private string _document_type;
        public string DOCUMENT_TYPE
        {
            get { return _document_type; }
            set { _document_type = value; }
        }
        /// <summary>
        /// 文档格式
        /// </summary>		
        private string _document_format;
        public string DOCUMENT_FORMAT
        {
            get { return _document_format; }
            set { _document_format = value; }
        }
        /// <summary>
        /// 文件标题
        /// </summary>		
        private string _documen_title;
        public string DOCUMEN_TITLE
        {
            get { return _documen_title; }
            set { _documen_title = value; }
        }
        /// <summary>
        /// 关键字
        /// </summary>		
        private string _key_word;
        public string KEY_WORD
        {
            get { return _key_word; }
            set { _key_word = value; }
        }
        /// <summary>
        /// 摘要
        /// </summary>		
        private string _abstract;
        public string ABSTRACT
        {
            get { return _abstract; }
            set { _abstract = value; }
        }
        /// <summary>
        /// 文档编写人
        /// </summary>		
        private string _document_writer;
        public string DOCUMENT_WRITER
        {
            get { return _document_writer; }
            set { _document_writer = value; }
        }
        /// <summary>
        /// 文档审核人
        /// </summary>		
        private string _document_verifier;
        public string DOCUMENT_VERIFIER
        {
            get { return _document_verifier; }
            set { _document_verifier = value; }
        }
        /// <summary>
        /// 文档完成日期
        /// </summary>		
        private DateTime? _document_completion_date;
        public DateTime? DOCUMENT_COMPLETION_DATE
        {
            get { return _document_completion_date; }
            set { _document_completion_date = value; }
        }
        /// <summary>
        /// 文档数据大小
        /// </summary>		
        private decimal _document_data_size;
        public decimal DOCUMENT_DATA_SIZE
        {
            get { return _document_data_size; }
            set { _document_data_size = value; }
        }
        /// <summary>
        /// 文档数据
        /// </summary>		
        private byte[] _document_data;
        public byte[] DOCUMENT_DATA
        {
            get { return _document_data; }
            set { _document_data = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>		
        private string _remark;
        public string REMARK
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}

