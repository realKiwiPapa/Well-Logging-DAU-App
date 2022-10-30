using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    [Serializable]
    public class LogPlanSearch:DM_LOG_OPS_PLAN
    {
        private DateTime? _requirements_time_start;
        /// <summary>
        /// 要求到井开始时间
        /// </summary>	
        public DateTime? REQUIREMENTS_TIME_START
        {
            get { return _requirements_time_start; }
            set { _requirements_time_start = value; }
        }
        private DateTime? _requirements_time_end;
        /// <summary>
        /// 要求到井结束时间
        /// </summary>	
        public DateTime? REQUIREMENTS_TIME_END
        {
            get { return _requirements_time_end; }
            set { _requirements_time_end = value; }
        }
    }
}
