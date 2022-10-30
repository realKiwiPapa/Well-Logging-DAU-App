using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    [Serializable]
    public class COM_LOG_COM_CURVE_INDEX:ModelBase
    {
        /// <summary>
        /// 曲线ID
        /// </summary>
        private string _curveid;

        public string CURVEID
        {
            get { return _curveid; }
            set { _curveid = value; }
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
        /// 解释处理项目ID
        /// </summary>
        private decimal? _processing_item_id;

        public decimal? PROCESSING_ITEM_ID
        {
            get { return _processing_item_id; }
            set { _processing_item_id = value; }
        }
        /// <summary>
        /// 曲线名
        /// </summary>
        private string _curve_name;

        public string CURVE_NAME
        {
            get { return _curve_name; }
            set { _curve_name = value; }
        }
        /// <summary>
        /// 曲线编码
        /// </summary>
        private string _curve_cd;

        public string CURVE_CD
        {
            get { return _curve_cd; }
            set { _curve_cd = value; }
        }
        /// <summary>
        /// 曲线始深度
        /// </summary>
        private decimal? _curve_start_dep;

        public decimal? CURVE_START_DEP
        {
            get { return _curve_start_dep; }
            set { _curve_start_dep = value; }
        }
        /// <summary>
        /// 曲线止深度
        /// </summary>
        private decimal? _curve_end_dep;

        public decimal? CURVE_END_DEP
        {
            get { return _curve_end_dep; }
            set { _curve_end_dep = value; }
        }
        /// <summary>
        /// 曲线深度采样
        /// </summary>
        private decimal? _curve_rlev;

        public decimal? CURVE_RLEV
        {
            get { return _curve_rlev; }
            set { _curve_rlev = value; }
        }
        /// <summary>
        /// 曲线单位
        /// </summary>
        private string _curve_unit;

        public string CURVE_UNIT
        {
            get { return _curve_unit; }
            set { _curve_unit = value; }
        }
        /// <summary>
        /// 曲线点数
        /// </summary>
        private decimal? _curve_sample;

        public decimal? CURVE_SAMPLE
        {
            get { return _curve_sample; }
            set { _curve_sample = value; }
        }
        /// <summary>
        /// 曲线最大值
        /// </summary>
        private decimal? _curve_max_value;

        public decimal? CURVE_MAX_VALUE
        {
            get { return _curve_max_value; }
            set { _curve_max_value = value; }
        }
        /// <summary>
        /// 曲线最小值
        /// </summary>
        private decimal? _curve_min_value;

        public decimal? CURVE_MIN_VALUE
        {
            get { return _curve_min_value; }
            set { _curve_min_value = value; }
        }
        /// <summary>
        /// 曲线数据类型，有符号字节数=1 有符号短整形数=2 有符号长整形数=3
        /// </summary>
        private string _curve_data_type;

        public string CURVE_DATA_TYPE
        {
            get { return _curve_data_type; }
            set { _curve_data_type = value; }
        }
        /// <summary>
        /// 曲线数据类型长度
        /// </summary>
        private decimal? _curve_data_length;

        public decimal? CURVE_DATA_LENGTH
        {
            get { return _curve_data_length; }
            set { _curve_data_length = value; }
        }
        /// <summary>
        /// 曲线处理软件名
        /// </summary>
        private string _p_curvesoftware_name;

        public string P_CURVESOFTWARE_NAME
        {
            get { return _p_curvesoftware_name; }
            set { _p_curvesoftware_name = value; }
        }
        /// <summary>
        /// 数据总字节数
        /// </summary>
        private decimal? _data_size;

        public decimal? DATA_SIZE
        {
            get { return _data_size; }
            set { _data_size = value; }
        }
        /// <summary>
        /// 曲线其他描述
        /// </summary>
        private string _curve_note;

        public string CURVE_NOTE
        {
            get { return _curve_note; }
            set { _curve_note = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        private string _note;

        public string NOTE
        {
            get { return _note; }
            set { _note = value; }
        }
        /// <summary>
        /// 数据块序号
        /// </summary>
        private decimal _block_number;

        public decimal BLOCK_NUMBER
        {
            get { return _block_number; }
            set { _block_number = value; }
        }
        /// <summary>
        /// 数据长度
        /// </summary>
        private decimal _block_size;

        public decimal BLOCK_SIZE
        {
            get { return _block_size; }
            set { _block_size = value; }
        }
        /// <summary>
        /// 曲线数据
        /// </summary>
        private byte[] _curve_data;

        public byte[] CURVE_DATA
        {
            get { return _curve_data; }
            set { _curve_data = value; }
        }
    }
}
