using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    [Serializable]
    public class HS_USER : ModelBase
    {
        private decimal _col_id;
        /// <summary>
        /// COL_ID
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal COL_ID
        {
            get { return _col_id; }
            set { _col_id = value; NotifyPropertyChanged("COL_ID"); }
        }
        private string _col_loginname;
        /// <summary>
        /// COL_LOGINNAME
        /// </summary>
        [Required]
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string COL_LOGINNAME
        {
            get { return _col_loginname; }
            set { _col_loginname = value; NotifyPropertyChanged("COL_LOGINNAME"); }
        }
        private string _col_name;
        /// <summary>
        /// COL_NAME
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string COL_NAME
        {
            get { return _col_name; }
            set { _col_name = value; NotifyPropertyChanged("COL_NAME"); }
        }
        private string _col_pword;
        /// <summary>
        /// COL_PWORD
        /// </summary>
        [OracleStringLength(MaximumLength = 50, CharUsed = CharUsedType.Byte)]
        public string COL_PWORD
        {
            get { return _col_pword; }
            set { _col_pword = value; NotifyPropertyChanged("COL_PWORD"); }
        }
        private decimal? _col_disabled;
        /// <summary>
        /// COL_DISABLED
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 1)]
        public decimal? COL_DISABLED
        {
            get { return _col_disabled; }
            set { _col_disabled = value; NotifyPropertyChanged("COL_DISABLED"); }
        }
        private decimal? _col_itemindex;
        /// <summary>
        /// COL_ITEMINDEX
        /// </summary>
        [OracleNumberLength(MaximumLength = 38)]
        public decimal? COL_ITEMINDEX
        {
            get { return _col_itemindex; }
            set { _col_itemindex = value; NotifyPropertyChanged("COL_ITEMINDEX"); }
        }

    }
}