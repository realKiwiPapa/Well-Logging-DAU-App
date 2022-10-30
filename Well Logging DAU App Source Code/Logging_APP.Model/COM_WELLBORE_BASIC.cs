using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 井筒基本数据 modle
    /// </summary>
    [Serializable]
    public class COM_WELLBORE_BASIC : ModelBase
    {
        private string _well_id;
        /// <summary>
        /// 井ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Byte)]
        public string WELL_ID
        {
            get { return _well_id; }
            set { _well_id = value; NotifyPropertyChanged("WELL_ID"); }
        }
        private string _wellbore_id;
        /// <summary>
        /// 井筒ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Byte)]
        public string WELLBORE_ID
        {
            get { return _wellbore_id; }
            set { _wellbore_id = value; NotifyPropertyChanged("WELLBORE_ID"); }
        }
        private string _wellbore_wui;
        /// <summary>
        /// 井筒唯一标识
        /// </summary>
        [OracleStringLength(MaximumLength = 60, CharUsed = CharUsedType.Byte)]
        public string WELLBORE_WUI
        {
            get { return _wellbore_wui; }
            set { _wellbore_wui = value; NotifyPropertyChanged("WELLBORE_WUI"); }
        }
        private string _p_wellbore_id;
        /// <summary>
        /// 父井筒代号
        /// </summary>
        [OracleStringLength(MaximumLength = 10, CharUsed = CharUsedType.Byte)]
        public string P_WELLBORE_ID
        {
            get { return _p_wellbore_id.TrimCharEnd(); }
            set { _p_wellbore_id = value; NotifyPropertyChanged("P_WELLBORE_ID"); }
        }
        private string _wellbore_name;
        /// <summary>
        /// 井筒名
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 20, CharUsed = CharUsedType.Byte)]
        public string WELLBORE_NAME
        {
            get { return _wellbore_name; }
            set { _wellbore_name = value.TrimChar(); NotifyPropertyChanged("WELLBORE_NAME"); }
        }
        private string _purpose;
        /// <summary>
        /// 井筒目的
        /// </summary>
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Byte)]
        public string PURPOSE
        {
            get { return _purpose; }
            set { _purpose = value; NotifyPropertyChanged("PURPOSE"); }
        }
        private decimal? _max_well_deviation;
        /// <summary>
        /// 最大井斜
        /// </summary>
        [OracleNumberLength(MaximumLength = 4)]
        public decimal? MAX_WELL_DEVIATION
        {
            get { return _max_well_deviation; }
            set { _max_well_deviation = value; NotifyPropertyChanged("MAX_WELL_DEVIATION"); }
        }
        private decimal? _max_well_deviation_md;
        /// <summary>
        /// 最大井斜对应井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? MAX_WELL_DEVIATION_MD
        {
            get { return _max_well_deviation_md; }
            set { _max_well_deviation_md = value; NotifyPropertyChanged("MAX_WELL_DEVIATION_MD"); }
        }
        private decimal? _design_vertical_tvd;
        /// <summary>
        /// 设计垂深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? DESIGN_VERTICAL_TVD
        {
            get { return _design_vertical_tvd; }
            set { _design_vertical_tvd = value; NotifyPropertyChanged("DESIGN_VERTICAL_TVD"); }
        }
        private DateTime? _wellbore_production_date;
        /// <summary>
        /// 井筒产生日期
        /// </summary>
        public DateTime? WELLBORE_PRODUCTION_DATE
        {
            get { return _wellbore_production_date; }
            set { _wellbore_production_date = value; NotifyPropertyChanged("WELLBORE_PRODUCTION_DATE"); }
        }
        private decimal? _deflection_point_md;
        /// <summary>
        /// 造斜（侧钻）点井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 4)]
        public decimal? DEFLECTION_POINT_MD
        {
            get { return _deflection_point_md; }
            set { _deflection_point_md = value; NotifyPropertyChanged("DEFLECTION_POINT_MD"); }
        }
        private decimal? _md;
        /// <summary>
        /// 实际井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? MD
        {
            get { return _md; }
            set { _md = value; NotifyPropertyChanged("MD"); }
        }
        private decimal? _vertical_well_tvd;
        /// <summary>
        /// 实际垂深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? VERTICAL_WELL_TVD
        {
            get { return _vertical_well_tvd; }
            set { _vertical_well_tvd = value; NotifyPropertyChanged("VERTICAL_WELL_TVD"); }
        }
        private decimal? _plugback_md;
        /// <summary>
        /// 回填深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 4)]
        public decimal? PLUGBACK_MD
        {
            get { return _plugback_md; }
            set { _plugback_md = value; NotifyPropertyChanged("PLUGBACK_MD"); }
        }
        private decimal? _plugback_tvd;
        /// <summary>
        /// 回填垂深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? PLUGBACK_TVD
        {
            get { return _plugback_tvd; }
            set { _plugback_tvd = value; NotifyPropertyChanged("PLUGBACK_TVD"); }
        }
        private decimal? _true_plugbacktotal_depth;
        /// <summary>
        /// 人工井底
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? TRUE_PLUGBACKTOTAL_DEPTH
        {
            get { return _true_plugbacktotal_depth; }
            set { _true_plugbacktotal_depth = value; NotifyPropertyChanged("TRUE_PLUGBACKTOTAL_DEPTH"); }
        }
        private string _design_horizon;
        /// <summary>
        /// 设计层位
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string DESIGN_HORIZON
        {
            get { return _design_horizon; }
            set { _design_horizon = value; NotifyPropertyChanged("DESIGN_HORIZON"); }
        }
        private decimal? _design_md;
        /// <summary>
        /// 设计井深
        /// </summary>
        [OracleNumberLength(MaximumLength = 5)]
        public decimal? DESIGN_MD
        {
            get { return _design_md; }
            set { _design_md = value; NotifyPropertyChanged("DESIGN_MD"); }
        }
        private string _actual_horizon;
        /// <summary>
        /// 实际层位
        /// </summary>
        [OracleStringLength(MaximumLength = 32, CharUsed = CharUsedType.Byte)]
        public string ACTUAL_HORIZON
        {
            get { return _actual_horizon; }
            set { _actual_horizon = value; NotifyPropertyChanged("ACTUAL_HORIZON"); }
        }
        private decimal? _btm_x_coordinate;
        /// <summary>
        /// 井底横坐标
        /// </summary>
        [OracleNumberLength(MaximumLength = 9)]
        public decimal? BTM_X_COORDINATE
        {
            get { return _btm_x_coordinate; }
            set { _btm_x_coordinate = value; NotifyPropertyChanged("BTM_X_COORDINATE"); }
        }
        private decimal? _btm_y_coordinate;
        /// <summary>
        /// 井底纵坐标
        /// </summary>
        [OracleNumberLength(MaximumLength = 9)]
        public decimal? BTM_Y_COORDINATE
        {
            get { return _btm_y_coordinate; }
            set { _btm_y_coordinate = value; NotifyPropertyChanged("BTM_Y_COORDINATE"); }
        }
        private string _wellbore_id_a1;
        /// <summary>
        /// A1井筒ID
        /// </summary>
        [OracleStringLength(MaximumLength = 30, CharUsed = CharUsedType.Byte)]
        public string WELLBORE_ID_A1
        {
            get { return _wellbore_id_a1; }
            set { _wellbore_id_a1 = value; NotifyPropertyChanged("WELLBORE_ID_A1"); }
        }
        private string _wellbore_id_a7;
        /// <summary>
        /// A7井筒ID
        /// </summary>
        [OracleStringLength(MaximumLength = 30, CharUsed = CharUsedType.Byte)]
        public string WELLBORE_ID_A7
        {
            get { return _wellbore_id_a7; }
            set { _wellbore_id_a7 = value; NotifyPropertyChanged("WELLBORE_ID_A7"); }
        }

    }
}