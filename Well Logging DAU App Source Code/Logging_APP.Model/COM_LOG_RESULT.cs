using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //解释处理_解释成果表
    [Serializable]
    public class COM_LOG_RESULT : ModelBase
    {
        /// <summary>
        /// 成果ID
        /// </summary>		
        private decimal _resultid;
        [Required]
        public decimal RESULTID
        {
            get { return _resultid; }
            set { _resultid = value; }
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
        /// 层号
        /// </summary>		
        private string _lay_id;
        public string LAY_ID
        {
            get { return _lay_id; }
            set { _lay_id = value; }
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
        /// 起始深度
        /// </summary>		
        private decimal _start_depth;
        public decimal START_DEPTH
        {
            get { return _start_depth; }
            set { _start_depth = value; }
        }
        /// <summary>
        /// 结束深度
        /// </summary>		
        private decimal _end_depth;
        public decimal END_DEPTH
        {
            get { return _end_depth; }
            set { _end_depth = value; }
        }
        /// <summary>
        /// 有效厚度
        /// </summary>		
        private decimal _valid_thickness;
        public decimal VALID_THICKNESS
        {
            get { return _valid_thickness; }
            set { _valid_thickness = value; }
        }
        /// <summary>
        /// 解释结论
        /// </summary>		
        private string _explain_conclusion;
        public string EXPLAIN_CONCLUSION
        {
            get { return _explain_conclusion; }
            set { _explain_conclusion = value; }
        }
        /// <summary>
        /// 孔隙度最小值
        /// </summary>		
        private decimal _porosity_min_value;
        public decimal POROSITY_MIN_VALUE
        {
            get { return _porosity_min_value; }
            set { _porosity_min_value = value; }
        }
        /// <summary>
        /// 孔隙度最大值
        /// </summary>		
        private decimal _porosity_max_value;
        public decimal POROSITY_MAX_VALUE
        {
            get { return _porosity_max_value; }
            set { _porosity_max_value = value; }
        }
        /// <summary>
        /// 含水饱和度最小值
        /// </summary>		
        private decimal _water_saturation_min_value;
        public decimal WATER_SATURATION_MIN_VALUE
        {
            get { return _water_saturation_min_value; }
            set { _water_saturation_min_value = value; }
        }
        /// <summary>
        /// 含水饱和度最大值
        /// </summary>		
        private decimal _water_saturation_max_value;
        public decimal WATER_SATURATION_MAX_VALUE
        {
            get { return _water_saturation_max_value; }
            set { _water_saturation_max_value = value; }
        }
        /// <summary>
        /// 渗透率
        /// </summary>		
        private decimal _penetrate_rate;
        public decimal PENETRATE_RATE
        {
            get { return _penetrate_rate; }
            set { _penetrate_rate = value; }
        }
        /// <summary>
        /// 自然伽马最小值
        /// </summary>		
        private decimal _gr_min_value;
        public decimal GR_MIN_VALUE
        {
            get { return _gr_min_value; }
            set { _gr_min_value = value; }
        }
        /// <summary>
        /// 自然伽马最大值
        /// </summary>		
        private decimal _gr_max_value;
        public decimal GR_MAX_VALUE
        {
            get { return _gr_max_value; }
            set { _gr_max_value = value; }
        }
        /// <summary>
        /// 补偿声波最小值
        /// </summary>		
        private decimal _soundwave_min_value;
        public decimal SOUNDWAVE_MIN_VALUE
        {
            get { return _soundwave_min_value; }
            set { _soundwave_min_value = value; }
        }
        /// <summary>
        /// 补偿声波最大值
        /// </summary>		
        private decimal _soundwave_max_value;
        public decimal SOUNDWAVE_MAX_VALUE
        {
            get { return _soundwave_max_value; }
            set { _soundwave_max_value = value; }
        }
        /// <summary>
        /// 补偿密度最小值
        /// </summary>		
        private decimal _density_min_value;
        public decimal DENSITY_MIN_VALUE
        {
            get { return _density_min_value; }
            set { _density_min_value = value; }
        }
        /// <summary>
        /// 补偿密度最大值
        /// </summary>		
        private decimal _density_max_value;
        public decimal DENSITY_MAX_VALUE
        {
            get { return _density_max_value; }
            set { _density_max_value = value; }
        }
        /// <summary>
        /// 补偿中子最小值
        /// </summary>		
        private decimal _neutron_min_value;
        public decimal NEUTRON_MIN_VALUE
        {
            get { return _neutron_min_value; }
            set { _neutron_min_value = value; }
        }
        /// <summary>
        /// 补偿中子最小值
        /// </summary>		
        private decimal _neutron_max_value;
        public decimal NEUTRON_MAX_VALUE
        {
            get { return _neutron_max_value; }
            set { _neutron_max_value = value; }
        }
        /// <summary>
        /// 深侧向最小值
        /// </summary>		
        private decimal _resisitance_min_value;
        public decimal RESISITANCE_MIN_VALUE
        {
            get { return _resisitance_min_value; }
            set { _resisitance_min_value = value; }
        }
        /// <summary>
        /// 深侧向最大值
        /// </summary>		
        private decimal _resisitance_max_value;
        public decimal RESISITANCE_MAX_VALUE
        {
            get { return _resisitance_max_value; }
            set { _resisitance_max_value = value; }
        }
        /// <summary>
        /// 浅侧向最小值
        /// </summary>		
        private decimal _qresisitance_min_value;
        public decimal QRESISITANCE_MIN_VALUE
        {
            get { return _qresisitance_min_value; }
            set { _qresisitance_min_value = value; }
        }
        /// <summary>
        /// 浅侧向最大值
        /// </summary>		
        private decimal _qresisitance_max_value;
        public decimal QRESISITANCE_MAX_VALUE
        {
            get { return _qresisitance_max_value; }
            set { _qresisitance_max_value = value; }
        }
        /// <summary>
        /// 120in感应最小值
        /// </summary>		
        private decimal _induction120_min_value;
        public decimal INDUCTION120_MIN_VALUE
        {
            get { return _induction120_min_value; }
            set { _induction120_min_value = value; }
        }
        /// <summary>
        /// 120in感应最大值
        /// </summary>		
        private decimal _induction120_max_value;
        public decimal INDUCTION120_MAX_VALUE
        {
            get { return _induction120_max_value; }
            set { _induction120_max_value = value; }
        }
        /// <summary>
        /// 30in感应最小值
        /// </summary>		
        private decimal _induction30_min_value;
        public decimal INDUCTION30_MIN_VALUE
        {
            get { return _induction30_min_value; }
            set { _induction30_min_value = value; }
        }
        /// <summary>
        /// 30in感应最大值
        /// </summary>		
        private decimal _induction30_max_value;
        public decimal INDUCTION30_MAX_VALUE
        {
            get { return _induction30_max_value; }
            set { _induction30_max_value = value; }
        }
        /// <summary>
        /// 备用字段1
        /// </summary>		
        private decimal _standby_param1;
        public decimal STANDBY_PARAM1
        {
            get { return _standby_param1; }
            set { _standby_param1 = value; }
        }
        /// <summary>
        /// 备用字段2
        /// </summary>		
        private decimal _standby_param2;
        public decimal STANDBY_PARAM2
        {
            get { return _standby_param2; }
            set { _standby_param2 = value; }
        }
        /// <summary>
        /// 备用字段3
        /// </summary>		
        private decimal _standby_param3;
        public decimal STANDBY_PARAM3
        {
            get { return _standby_param3; }
            set { _standby_param3 = value; }
        }
        /// <summary>
        /// 备用字段4
        /// </summary>		
        private decimal _standby_param4;
        public decimal STANDBY_PARAM4
        {
            get { return _standby_param4; }
            set { _standby_param4 = value; }
        }
        /// <summary>
        /// 备用字段5
        /// </summary>		
        private decimal _standby_param5;
        public decimal STANDBY_PARAM5
        {
            get { return _standby_param5; }
            set { _standby_param5 = value; }
        }
        /// <summary>
        /// 备用字段6
        /// </summary>		
        private decimal _standby_param6;
        public decimal STANDBY_PARAM6
        {
            get { return _standby_param6; }
            set { _standby_param6 = value; }
        }
        /// <summary>
        /// 备用字段7
        /// </summary>		
        private decimal _standby_param7;
        public decimal STANDBY_PARAM7
        {
            get { return _standby_param7; }
            set { _standby_param7 = value; }
        }
        /// <summary>
        /// 备用字段8
        /// </summary>		
        private decimal _standby_param8;
        public decimal STANDBY_PARAM8
        {
            get { return _standby_param8; }
            set { _standby_param8 = value; }
        }
    }
}

