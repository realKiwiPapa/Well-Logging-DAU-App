using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    [Serializable]
    public class DM_LOG_DVD_INFO:ModelBase
    {
        private string _process_id;
        [Required]
        public string PROCESS_ID
        {
            get { return _process_id; }
            set { _process_id = value; NotifyPropertyChanged("PROCESS_ID"); }
        }

        private string _process_name;
        public string PROCESS_NAME
        {
            get { return _process_name; }
            set { _process_name = value; NotifyPropertyChanged("PROCESS_NAME"); }
        }

        private string _dvd_dir_name;
        public string DVD_DIR_NAME
        {
            get { return _dvd_dir_name; }
            set { _dvd_dir_name = value;NotifyPropertyChanged("DVD_DIR_NAME"); }
        }

        private string _dvd_number;
        public string DVD_NUMBER
        {
            get { return _dvd_number; }
            set { _dvd_number = value; NotifyPropertyChanged("DVD_NUMBER"); }
        }

        private string _storage_tank_no;
        public string STORAGE_TANK_NO
        {
            get { return _storage_tank_no; }
            set { _storage_tank_no = value; NotifyPropertyChanged("STORAGE_TANK_NO"); }
        }

        private string _copy_dvd_man;
        public string COPY_DVD_MAN
        {
            get { return _copy_dvd_man; }
            set { _copy_dvd_man = value; NotifyPropertyChanged("COPY_DVD_MAN"); }
        }

        private DateTime? _copy_dvd_date;
        public DateTime? COPY_DVD_DATE
        {
            get { return _copy_dvd_date; }
            set { _copy_dvd_date = value; NotifyPropertyChanged("COPY_DVD_DATE"); }
        }

        private string _note;
        public string NOTE
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged("NOTE"); }
        }
    }
}
