using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //解释处理_分层表
    [Serializable]
    public class COM_LOG_LAYER : ModelBase
    {
        /// <summary>
        /// 层位ID
        /// </summary>		
        private decimal _layerid;
        [Required]
        public decimal LAYERID
        {
            get { return _layerid; }
            set { _layerid = value; }
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
        /// 层位名
        /// </summary>		
        private string _formation_name;
        public string FORMATION_NAME
        {
            get { return _formation_name; }
            set { _formation_name = value; }
        }
        /// <summary>
        /// 电测起始深度（斜深）
        /// </summary>		
        private decimal _st_deviated_dep;
        public decimal ST_DEVIATED_DEP
        {
            get { return _st_deviated_dep; }
            set { _st_deviated_dep = value; }
        }
        /// <summary>
        /// 电测终止深度（斜深）
        /// </summary>		
        private decimal _en_deviated_dep;
        public decimal EN_DEVIATED_DEP
        {
            get { return _en_deviated_dep; }
            set { _en_deviated_dep = value; }
        }
        /// <summary>
        /// 电测起始深度（垂深）
        /// </summary>		
        private decimal _st_vertical_dep;
        public decimal ST_VERTICAL_DEP
        {
            get { return _st_vertical_dep; }
            set { _st_vertical_dep = value; }
        }
        /// <summary>
        /// 电测终止深度（垂深）
        /// </summary>		
        private decimal _en_vertical_dep;
        public decimal EN_VERTICAL_DEP
        {
            get { return _en_vertical_dep; }
            set { _en_vertical_dep = value; }
        }
    }
}

