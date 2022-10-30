using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //解释处理形成的表格化大块数据体
    [Serializable]
    public class PRO_LOG_PROCESSING_CURVE_DATA : ModelBase
    {
        /// <summary>
        /// 曲线ID
        /// </summary>		
        private string _curveid;
        [Required]
        public string CURVEID
        {
            get { return _curveid; }
            set { _curveid = value; }
        }
        /// <summary>
        /// 曲线块序号
        /// </summary>		
        private decimal _block_nunber;
        public decimal BLOCK_NUNBER
        {
            get { return _block_nunber; }
            set { _block_nunber = value; }
        }
        /// <summary>
        /// 曲线块大小
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

