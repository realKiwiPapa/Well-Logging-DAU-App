using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Utility
{
    [Serializable]
    public class AttachInfo
    {
        public string F_NAME { get; set; }
        public int F_START { get; set; }
        public int F_SIZE { get; set; }
    }
}
