using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 预测项目信息 modle 
    /// </summary>
    [Serializable]
    public class DM_LOG_PREDICTED_ITEM : ModelBase
    {

        private decimal? _predicted_logging_items_id;
        /// <summary>
        /// 预测项目ID
        /// </summary>	
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? PREDICTED_LOGGING_ITEMS_ID
        {
            get { return _predicted_logging_items_id; }
            set { _predicted_logging_items_id = value; }
        }

        private string _predicted_logging_name; 
        /// <summary>
        /// 预测项目名称
        /// </summary>	
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Char)]
        public string PREDICTED_LOGGING_NAME
        {
            get { return _predicted_logging_name; }
            set
            {
                _predicted_logging_name = value;
                //NotifyPropertyChanged("PREDICTED_LOGGING_NAME");
            }
        }

        private decimal? _pre_st_dep;  
        /// <summary>
        /// 预测项目起始深度
        /// </summary>	
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? PRE_ST_DEP
        {
            get { return _pre_st_dep; }
            set
            {
                _pre_st_dep = value;
                NotifyPropertyChanged("PRE_ST_DEP");
            }
        }

        private decimal? _pre_en_dep; 
        /// <summary>
        /// 预测项目结束深度
        /// </summary>	
        [OracleNumberLength(MaximumLength = 8)]
        public decimal? PRE_EN_DEP
        {
            get { return _pre_en_dep; }
            set { _pre_en_dep = value; NotifyPropertyChanged("PRE_EN_DEP"); }
        }

        private string _pre_scale;
        /// <summary>
        /// 预测项目比例（1：200）
        /// </summary>	
        [OracleStringLength(MaximumLength = 20, CharUsed = CharUsedType.Char)]
        public string PRE_SCALE
        {
            get { return _pre_scale; }
            set { _pre_scale = value; NotifyPropertyChanged("PRE_SCALE"); }
        }
	
        private string _note;    
        /// <summary>
        /// 备注
        /// </summary>
        [OracleStringLength(MaximumLength = 200, CharUsed = CharUsedType.Char)]
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }

        private decimal? _log_item_id;    
        /// <summary>
        /// 预测项目ID
        /// </summary>	
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? LOG_ITEM_ID
        {
            get { return _log_item_id; }
            set { _log_item_id = value; }
        }

    }
}