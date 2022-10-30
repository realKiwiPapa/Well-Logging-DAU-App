using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //深度	方位	井斜
    [Serializable]
    public class COM_LOG_DEV_AZI : ModelBase
    {
        /// <summary>
        /// 井斜ID
        /// </summary>		
        private decimal _dev_id;
        [Required]
        public decimal DEV_ID
        {
            get { return _dev_id; }
            set { _dev_id = value; }
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
        /// 井深度
        /// </summary>		
        private decimal _dep;
        public decimal DEP
        {
            get { return _dep; }
            set { _dep = value; }
        }
        /// <summary>
        /// 井斜
        /// </summary>		
        private decimal _inclnation;
        public decimal INCLNATION
        {
            get { return _inclnation; }
            set { _inclnation = value; }
        }
        /// <summary>
        /// 方位
        /// </summary>		
        private decimal _azimuth;
        public decimal AZIMUTH
        {
            get { return _azimuth; }
            set { _azimuth = value; }
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
    }
}

