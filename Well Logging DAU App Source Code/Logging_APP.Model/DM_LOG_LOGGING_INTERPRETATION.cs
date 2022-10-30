using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 来自录井解释成果表
    /// </summary>
    [Serializable]
    public class DM_LOG_LOGGING_INTERPRETATION : ModelBase
    {
        #region Model
        private string interpretation_cd;
        private string _job_plan_cd;
        private string _requisition_cd;
        private DateTime? _update_date;
        private string _horizon;
        private decimal? _well_top_depth;
        private decimal? _well_bottom_depth;
        private decimal? _thickness;
        private string _ysmc;
        private string _ysms;
        private string _ygys;
        private decimal? _ygjb;
        private decimal? _dlygfxz;
        private string _dlygjb;
        private string _display_type;
        private string _display_generay;
        private string _drilling_change;
        private decimal? _hybrocabon_value;
        private decimal? _ignition;
        private decimal? _cl_ion_content;
        /// <summary>
        /// 录井解释成果ID
        /// </summary>
        [Required]
        public string INTERPRETATION_CD
        {
            set { interpretation_cd = value; }
            get { return interpretation_cd.TrimCharEnd(); }
        }
        /// <summary>
        /// 作业计划书编号
        /// </summary>
        public string JOB_PLAN_CD
        {
            set { _job_plan_cd = value; }
            get { return _job_plan_cd; }
        }
        /// <summary>
        /// 通知单编码
        /// </summary>
        public string REQUISITION_CD
        {
            set { _requisition_cd = value; }
            get { return _requisition_cd; }
        }
        /// <summary>
        /// 同步更新日期
        /// </summary>
        public DateTime? UPDATE_DATE
        {
            set { _update_date = value; }
            get { return _update_date; }
        }
        /// <summary>
        /// 层位
        /// </summary>
        public string HORIZON
        {
            set { _horizon = value; }
            get { return _horizon; }
        }
        /// <summary>
        /// 顶界井深(m)
        /// </summary>
        public decimal? WELL_TOP_DEPTH
        {
            set { _well_top_depth = value; }
            get { return _well_top_depth; }
        }
        /// <summary>
        /// 底界井深(m)
        /// </summary>
        public decimal? WELL_BOTTOM_DEPTH
        {
            set { _well_bottom_depth = value; }
            get { return _well_bottom_depth; }
        }
        /// <summary>
        /// 厚度(m)
        /// </summary>
        public decimal? THICKNESS
        {
            set { _thickness = value; }
            get { return _thickness; }
        }
        /// <summary>
        /// 岩石名称
        /// </summary>
        public string YSMC
        {
            set { _ysmc = value; }
            get { return _ysmc; }
        }
        /// <summary>
        /// 岩石描述
        /// </summary>
        public string YSMS
        {
            set { _ysms = value; }
            get { return _ysms; }
        }
        /// <summary>
        /// 荧光颜色
        /// </summary>
        public string YGYS
        {
            set { _ygys = value; }
            get { return _ygys; }
        }
        /// <summary>
        /// 荧光级别(级)
        /// </summary>
        public decimal? YGJB
        {
            set { _ygjb = value; }
            get { return _ygjb; }
        }
        /// <summary>
        /// 定量荧光分析值
        /// </summary>
        public decimal? DLYGFXZ
        {
            set { _dlygfxz = value; }
            get { return _dlygfxz; }
        }
        /// <summary>
        /// 定量荧光级别
        /// </summary>
        public string DLYGJB
        {
            set { _dlygjb = value; }
            get { return _dlygjb; }
        }
        #endregion Model
        /// <summary>
        /// 显示类型
        /// </summary>
        public string DISPLAY_TYPE
        {
            set { _display_type = value; }
            get { return _display_type; }
        }
        /// <summary>
        /// 显示情况
        /// </summary>
        public string DISPLAY_GENERAY
        {
            set
            { 
                _display_generay = value; 
            }
            get
            { 
                return _display_generay; 
            }
        }

        /// <summary>
        /// 钻时变化
        /// </summary>
        public string DRILLING_CHANGE
        {
            get { return _drilling_change; }
            set { _drilling_change = value; }
        }
        /// <summary>
        /// 全烃值
        /// </summary>
        public decimal? HYBROCABON_VALUE
        {
            get { return _hybrocabon_value; }
            set { _hybrocabon_value = value; }
        }
        /// <summary>
        /// 点火
        /// </summary>
        public decimal? IGNITION
        {
            get { return _ignition; }
            set { _ignition = value; }
        }
        /// <summary>
        /// 氯离子含量
        /// </summary>
        public decimal? CL_ION_CONTENT
        {
            get { return _cl_ion_content; }
            set { _cl_ion_content = value; }
        }
    }
}

