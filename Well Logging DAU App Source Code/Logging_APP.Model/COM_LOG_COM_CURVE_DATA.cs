using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    [Serializable]
    public class COM_LOG_COM_CURVE_DATA:ModelBase
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
        /// 曲线块序号
        /// </summary>
        private decimal? _block_number;

        public decimal? BLOCK_NUMBER
        {
            get { return _block_number; }
            set { _block_number = value; }
        }
        /// <summary>
        /// 曲线块大小
        /// </summary>
        private decimal? _block_size;

        public decimal? BLOCK_SIZE
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
