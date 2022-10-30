using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 解释处理_图件 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_PROCESS_MAP : ModelBase
    {
        private string _mapid;
        /// <summary>
        /// 图件ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string MAPID
        {
            get { return _mapid; }
            set { _mapid = value; NotifyPropertyChanged("MAPID"); }
        }
        private string _process_id;
        /// <summary>
        /// 解释处理ID
        /// </summary>
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string PROCESS_ID
        {
            get { return _process_id; }
            set { _process_id = value; NotifyPropertyChanged("PROCESS_ID"); }
        }
        private decimal? _processing_item_id;
        /// <summary>
        /// 解释处理项目编码
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? PROCESSING_ITEM_ID
        {
            get { return _processing_item_id; }
            set { _processing_item_id = value; NotifyPropertyChanged("PROCESSING_ITEM_ID"); }
        }
        private string _maps_coding;
        /// <summary>
        /// 图件编码
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string MAPS_CODING
        {
            get { return _maps_coding; }
            set { _maps_coding = value; NotifyPropertyChanged("MAPS_CODING"); }
        }
        private decimal? _map_start_dep;
        /// <summary>
        /// 起始深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? MAP_START_DEP
        {
            get { return _map_start_dep; }
            set { _map_start_dep = value; NotifyPropertyChanged("MAP_START_DEP"); }
        }
        private decimal? _map_end_dep;
        /// <summary>
        /// 结束深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? MAP_END_DEP
        {
            get { return _map_end_dep; }
            set { _map_end_dep = value; NotifyPropertyChanged("MAP_END_DEP"); }
        }
        private string _map_scale;
        /// <summary>
        /// 出图深度比例
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string MAP_SCALE
        {
            get { return _map_scale; }
            set { _map_scale = value; NotifyPropertyChanged("MAP_SCALE"); }
        }
        private string _map_data_name;
        /// <summary>
        /// 出图数据文件名
        /// </summary>
        [OracleStringLength(MaximumLength = 255, CharUsed = CharUsedType.Char)]
        public string MAP_DATA_NAME
        {
            get { return _map_data_name; }
            set { _map_data_name = value; NotifyPropertyChanged("MAP_DATA_NAME"); }
        }
        private decimal? _map_pdf_size;
        /// <summary>
        /// PDF文件大小
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? MAP_PDF_SIZE
        {
            get { return _map_pdf_size; }
            set { _map_pdf_size = value; NotifyPropertyChanged("MAP_PDF_SIZE"); }
        }
        private byte[] _map_pdf_data;
        /// <summary>
        /// PDF文件数据
        /// </summary>
        public byte[] MAP_PDF_DATA
        {
            get { return _map_pdf_data; }
            set { _map_pdf_data = value; NotifyPropertyChanged("MAP_PDF_DATA"); }
        }
        private string _p_process_software;
        /// <summary>
        /// 解释处理软件
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string P_PROCESS_SOFTWARE
        {
            get { return _p_process_software; }
            set { _p_process_software = value; NotifyPropertyChanged("P_PROCESS_SOFTWARE"); }
        }
        private DateTime _map_out_date;
        /// <summary>
        /// 出图日期
        /// </summary>
        public DateTime MAP_OUT_DATE
        {
            get { return _map_out_date; }
            set { _map_out_date = value; NotifyPropertyChanged("MAP_OUT_DATE"); }
        }
        private string _map_person;
        /// <summary>
        /// 出图人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string MAP_PERSON
        {
            get { return _map_person; }
            set { _map_person = value; NotifyPropertyChanged("MAP_PERSON"); }
        }
        private decimal? _map_number;
        /// <summary>
        /// 出图份数
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? MAP_NUMBER
        {
            get { return _map_number; }
            set { _map_number = value; NotifyPropertyChanged("MAP_NUMBER"); }
        }
        private decimal? _map_template_size;
        /// <summary>
        /// 绘图模版数据大小
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? MAP_TEMPLATE_SIZE
        {
            get { return _map_template_size; }
            set { _map_template_size = value; NotifyPropertyChanged("MAP_TEMPLATE_SIZE"); }
        }
        private byte[] _map_template_data;
        /// <summary>
        /// 绘图模版数据
        /// </summary>
        public byte[] MAP_TEMPLATE_DATA
        {
            get { return _map_template_data; }
            set { _map_template_data = value; NotifyPropertyChanged("MAP_TEMPLATE_DATA"); }
        }
        private string _map_verifier;
        /// <summary>
        /// 审核人
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string MAP_VERIFIER
        {
            get { return _map_verifier; }
            set { _map_verifier = value; NotifyPropertyChanged("MAP_VERIFIER"); }
        }

    }
}