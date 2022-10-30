using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    public static class ConvertString
    {
        public static string TrimChar(this String value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Trim();
        }

        public static string TrimCharEnd(this String value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.TrimEnd();
        }
    }
}
