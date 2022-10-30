using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Logging_App.Utility
{
    public static class ExtensionMethods
    {
        public static T FieldEx<T>(this DataRow row, string columnName)
        {
            Type type = typeof(T);
            if (type.IsGenericType)
                type = type.GetGenericArguments()[0];
            if (type != row.Table.Columns[columnName].DataType)
            {
                object v = row[columnName];
                if (v == DBNull.Value)
                {
                    v = null;
                    return (T)v;
                }
                else
                    return (T)Convert.ChangeType(v, type);
            }
            else
                return row.Field<T>(columnName);
        }

        public static T FieldEx<T>(this DataRow row, int columnIndex)
        {
            Type type = typeof(T);
            if (type.IsGenericType)
                type = type.GetGenericArguments()[0];
            if (type != row.Table.Columns[columnIndex].DataType)
            {
                object v = row[columnIndex];
                if (v == DBNull.Value)
                {
                    v = null;
                    return (T)v;
                }
                else
                    return (T)Convert.ChangeType(v, type);
            }
            else
                return row.Field<T>(columnIndex);
        }
    }
}
