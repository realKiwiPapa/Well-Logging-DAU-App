using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 解释处理曲线评级 modle
    /// </summary>
    [Serializable]
    public class PRO_LOG_PROCESSING_CURVERATING : ModelBase
    {
        private string _process_id;
        /// <summary>
        /// 解释处理ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 100, CharUsed = CharUsedType.Byte)]
        public string PROCESS_ID
        {
            get { return _process_id; }
            set { _process_id = value; NotifyPropertyChanged("PROCESS_ID"); }
        }
        private decimal? _processing_item_id;
        /// <summary>
        /// 解释处理项目编码ID
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? PROCESSING_ITEM_ID
        {
            get { return _processing_item_id; }
            set { _processing_item_id = value; NotifyPropertyChanged("PROCESSING_ITEM_ID"); }
        }
        private string _scale;
        /// <summary>
        /// 比例尺
        /// </summary>
        [OracleStringLength(MaximumLength = 20, CharUsed = CharUsedType.Byte)]
        public string SCALE
        {
            get { return _scale; }
            set { _scale = value; NotifyPropertyChanged("SCALE"); }
        }
        private decimal? _rlev;
        /// <summary>
        /// 采用间距
        /// </summary>
        [OracleNumberLength(MaximumLength = 7)]
        public decimal? RLEV
        {
            get { return _rlev; }
            set { _rlev = value; NotifyPropertyChanged("RLEV"); }
        }
        private string _scene_rating;
        /// <summary>
        /// 现场评级
        /// </summary>
        [OracleStringLength(MaximumLength = 20, CharUsed = CharUsedType.Byte)]
        public string SCENE_RATING
        {
            get { return _scene_rating; }
            set { _scene_rating = value; NotifyPropertyChanged("SCENE_RATING"); }
        }
        private string _indoor_rating;
        /// <summary>
        /// 室内评级
        /// </summary>
        [OracleStringLength(MaximumLength = 20, CharUsed = CharUsedType.Byte)]
        public string INDOOR_RATING
        {
            get { return _indoor_rating; }
            set { _indoor_rating = value; NotifyPropertyChanged("INDOOR_RATING"); }
        }
        private decimal _curve_id;
        /// <summary>
        /// 测井曲线ID
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal CURVE_ID
        {
            get { return _curve_id; }
            set { _curve_id = value; NotifyPropertyChanged("CURVE_ID"); }
        }
        private decimal? _start_dep;
        /// <summary>
        /// 起始深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? START_DEP
        {
            get { return _start_dep; }
            set { _start_dep = value; NotifyPropertyChanged("START_DEP"); }
        }
        private decimal? _end_dep;
        /// <summary>
        /// 结束深度
        /// </summary>
        [OracleNumberLength(MaximumLength = 10)]
        public decimal? END_DEP
        {
            get { return _end_dep; }
            set { _end_dep = value; NotifyPropertyChanged("END_DEP"); }
        }
        private string _why;
        /// <summary>
        /// 不合格原因
        /// </summary>
        [OracleStringLength(MaximumLength=1024,CharUsed=CharUsedType.Byte)]
        public string WHY
        {
            get { return _why; }
            set { _why = value; NotifyPropertyChanged("WHY"); }
        }
    }
}