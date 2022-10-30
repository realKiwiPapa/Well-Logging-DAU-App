using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //解释处理_井眼轨迹数据
    [Serializable]
    public class COM_LOG_WELL_TRAJECTORY : ModelBase
    {
        /// <summary>
        /// 轨迹ID
        /// </summary>		
        private decimal _trajectoryid;
        [Required]
        public decimal TRAJECTORYID
        {
            get { return _trajectoryid; }
            set { _trajectoryid = value; }
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
        private decimal _md;
        public decimal MD
        {
            get { return _md; }
            set { _md = value; }
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
        /// 位移（东）
        /// </summary>		
        private decimal _e_element;
        public decimal E_ELEMENT
        {
            get { return _e_element; }
            set { _e_element = value; }
        }
        /// <summary>
        /// 位移（北）
        /// </summary>		
        private decimal _n_element;
        public decimal N_ELEMENT
        {
            get { return _n_element; }
            set { _n_element = value; }
        }
        /// <summary>
        /// 垂深
        /// </summary>		
        private decimal _tvd;
        public decimal TVD
        {
            get { return _tvd; }
            set { _tvd = value; }
        }
        /// <summary>
        /// 水平位移
        /// </summary>		
        private decimal _all_mov;
        public decimal ALL_MOV
        {
            get { return _all_mov; }
            set { _all_mov = value; }
        }
        /// <summary>
        /// 闭合方位
        /// </summary>		
        private decimal _p_xn_closure_azimuth;
        public decimal P_XN_CLOSURE_AZIMUTH
        {
            get { return _p_xn_closure_azimuth; }
            set { _p_xn_closure_azimuth = value; }
        }
        /// <summary>
        /// 闭合距
        /// </summary>		
        private decimal _p_xn_closed_distance;
        public decimal P_XN_CLOSED_DISTANCE
        {
            get { return _p_xn_closed_distance; }
            set { _p_xn_closed_distance = value; }
        }
        /// <summary>
        /// 狗腿度
        /// </summary>		
        private decimal _dog_leg;
        public decimal DOG_LEG
        {
            get { return _dog_leg; }
            set { _dog_leg = value; }
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
        /// 数据类型
        /// </summary>		
        private string _type;
        public string TYPE
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}

