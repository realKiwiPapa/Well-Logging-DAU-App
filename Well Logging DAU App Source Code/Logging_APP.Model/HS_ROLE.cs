using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    [Serializable]
    public class HS_ROLE : ModelBase
    {
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
        private decimal _col_roletype;
        /// <summary>
        /// COL_ROLETYPE
        /// </summary>
        [Required]
        [OracleNumberLength(MaximumLength = 38)]
        public decimal COL_ROLETYPE
        {
            get { return _col_roletype; }
            set { _col_roletype = value; NotifyPropertyChanged("COL_ROLETYPE"); }
        }

        private string _col_rolename;
        public string COL_ROLENAME
        {
            get { return _col_rolename; }
            set { _col_rolename = value; NotifyPropertyChanged("COL_ROLENAME"); }
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; NotifyPropertyChanged("Selected"); }
        }
    }
}