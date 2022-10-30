using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    [Serializable]
    public class PRO_LOG_PROCESSING_CURVE_INDEX:ModelBase
    {
        private decimal _curveid;

        public decimal CURVEID
        {
            get { return _curveid; }
            set { _curveid = value; }
        }
        private string _process_id;

        public string PROCESS_ID
        {
            get { return _process_id; }
            set { _process_id = value; }
        }
        private decimal? _processing_item_id;

        public decimal? PROCESSING_ITEM_ID
        {
            get { return _processing_item_id; }
            set { _processing_item_id = value; }
        }
        private string _curve_name;

        public string CURVE_NAME
        {
            get { return _curve_name; }
            set { _curve_name = value; }
        }
        private decimal? _curve_start_dep;

        public decimal? CURVE_START_DEP
        {
            get { return _curve_start_dep; }
            set { _curve_start_dep = value; }
        }
        private decimal? _curve_end_dep;

        public decimal? CURVE_END_DEP
        {
            get { return _curve_end_dep; }
            set { _curve_end_dep = value; }
        }
        private decimal? _curve_rlev;

        public decimal? CURVE_RLEV
        {
            get { return _curve_rlev; }
            set { _curve_rlev = value; }
        }
        private string _curve_unit;

        public string CURVE_UNIT
        {
            get { return _curve_unit; }
            set { _curve_unit = value; }
        }
        private decimal? _curve_t_sample;

        public decimal? CURVE_T_SAMPLE
        {
            get { return _curve_t_sample; }
            set { _curve_t_sample = value; }
        }
        private string _curve_t_unit;

        public string CURVE_T_UNIT
        {
            get { return _curve_t_unit; }
            set { _curve_t_unit = value; }
        }
        private decimal? _curve_t_max_value;

        public decimal? CURVE_T_MAX_VALUE
        {
            get { return _curve_t_max_value; }
            set { _curve_t_max_value = value; }
        }
        private decimal? _curve_t_min_value;

        public decimal? CURVE_T_MIN_VALUE
        {
            get { return _curve_t_min_value; }
            set { _curve_t_min_value = value; }
        }
        private decimal? _curve_t_relv;

        public decimal? CURVE_T_RELV
        {
            get { return _curve_t_relv; }
            set { _curve_t_relv = value; }
        }
        private string _curve_data_type;

        public string CURVE_DATA_TYPE
        {
            get { return _curve_data_type; }
            set { _curve_data_type = value; }
        }
        private decimal? _curve_data_length;

        public decimal? CURVE_DATA_LENGHT
        {
            get { return _curve_data_length; }
            set { _curve_data_length = value; }
        }
        private string _data_property;

        public string DATA_PROPERTY
        {
            get { return _data_property; }
            set { _data_property = value; }
        }
        private decimal? _data_info;

        public decimal? DATA_INFO
        {
            get { return _data_info; }
            set { _data_info = value; }
        }
        private string _p_curvesoftware_name;

        public string P_CURVESOFTWARE_NAME
        {
            get { return _p_curvesoftware_name; }
            set { _p_curvesoftware_name = value; }
        }
        private decimal? _data_storage_way;

        public decimal? DATA_STORAGE_WAY
        {
            get { return _data_storage_way; }
            set { _data_storage_way = value; }
        }
        private decimal? _data_size;

        public decimal? DATA_SIZE
        {
            get { return _data_size; }
            set { _data_size = value; }
        }
        private string _curve_note;

        public string CURVE_NOTE
        {
            get { return _curve_note; }
            set { _curve_note = value; }
        }
        private string _note;

        public string NOTE
        {
            get { return _note; }
            set { _note = value; }
        }
        private string _curve_cd;

        public string CURVE_CD
        {
            get { return _curve_cd; }
            set { _curve_cd = value; }
        }
        private string _data_path;
        /// <summary>
        /// 
        /// </summary>
        public string DATA_PATH
        {
            get { return _data_path; }
            set { _data_path = value; NotifyPropertyChanged("DATA_PATH"); }
        }

        private decimal? _block_number;

        public decimal? BLOCK_NUMBER
        {
            get { return _block_number; }
            set { _block_number = value; }
        }

        private decimal? _block_size;

        public decimal? BLOCK_SIZE
        {
            get { return _block_size; }
            set { _block_size = value; }
        }
        private byte[] _curve_data;

        public byte[] CURVE_DATA
        {
            get { return _curve_data; }
            set { _curve_data = value; }
        }

        private decimal? _fileid;

        public decimal? FILEID
        {
            get { return _fileid; }
            set { _fileid = value;}
        }

        private string _maps_coding;

        public string MAPS_CODING
        {
            get { return _maps_coding; }
            set { _maps_coding = value; }
        }
    }
}
