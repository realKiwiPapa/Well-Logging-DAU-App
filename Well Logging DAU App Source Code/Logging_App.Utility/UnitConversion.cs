using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Utility
{
    public static class UnitConversion
    {
        public static string HumanReadableByteCount(long bytes)
        {
            int unit = 1024;
            if (bytes < unit) return bytes + " B";
            int exp = (int)(Math.Log(bytes) / Math.Log(unit));
            return String.Format("{0:F2} {1}B", bytes / Math.Pow(unit, exp), "KMGTPE"[exp - 1]);
        }
    }
}
