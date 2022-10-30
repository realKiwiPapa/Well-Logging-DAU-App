using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    [Serializable]
    public class SYS_DATA_RIGHTS : ModelBase
    {
        private string _data_id;
        /// <summary>
        /// DATA_ID
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 500, CharUsed = CharUsedType.Byte)]
        public string DATA_ID
        {
            get { return _data_id; }
            set { _data_id = value; NotifyPropertyChanged("DATA_ID"); }
        }
        private decimal? _rights_id;
        /// <summary>
        /// RIGHTS_ID
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? RIGHTS_TYPE_ID
        {
            get { return _rights_id; }
            set { _rights_id = value; NotifyPropertyChanged("RIGHTS_TYPE_ID"); }
        }
        private string _wirteuser;
        /// <summary>
        /// WIRTEUSER
        /// </summary>
        [OracleStringLength(MaximumLength = 20, CharUsed = CharUsedType.Byte)]
        public string WIRTEUSER
        {
            get { return _wirteuser; }
            set { _wirteuser = value; NotifyPropertyChanged("WIRTEUSER"); }
        }
        private string _checkuser;
        /// <summary>
        /// CHECKUSER
        /// </summary>
        [OracleStringLength(MaximumLength = 20, CharUsed = CharUsedType.Byte)]
        public string CHECKUSER
        {
            get { return _checkuser; }
            set { _checkuser = value; NotifyPropertyChanged("CHECKUSER"); }
        }
        private decimal? _state;
        /// <summary>
        /// STATE
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? STATE
        {
            get { return _state; }
            set { _state = value; NotifyPropertyChanged("STATE"); }
        }

        public string NAME { get; set; }
    }
}