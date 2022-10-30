﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.ComponentModel;

using Logging_App.Model;

namespace Logging_App.Utility
{
    /// <summary>
    /// DataTable与实体类互相转换
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    public class ModelHandler<T> where T : ModelBase, INotifyPropertyChanged, new()
    {
        #region DataTable转换成实体类

        /// <summary>
        /// 填充对象列表：用DataSet的第一个表填充实体类
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        public static DataCollection<T> FillModel(DataSet ds)
        {
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[0]);
            }
        }

        /// <summary>  
        /// 填充对象列表：用DataSet的第index个表填充实体类
        /// </summary>  
        public static DataCollection<T> FillModel(DataSet ds, int index)
        {
            if (ds == null || ds.Tables.Count <= index || ds.Tables[index].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[index]);
            }
        }

        ///
        /// <summary>  
        /// 填充对象列表：用DataTable填充实体类
        /// </summary>  
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataCollection<T> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            DataCollection<T> modelList = new DataCollection<T>();
            foreach (DataRow dr in dt.Rows)
            {
                //T model = (T)Activator.CreateInstance(typeof(T));  
                T model = new T();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                    if (propertyInfo != null && dr[i] != DBNull.Value)
                        if (propertyInfo.PropertyType.IsEnum)
                            propertyInfo.SetValue(model, Enum.ToObject(propertyInfo.PropertyType, Enum.Parse(propertyInfo.PropertyType, dr[i].ToString())), null);
                        else
                        {
                            Type type = propertyInfo.PropertyType;
                            object v = dr[i];
                            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {
                                v = Convert.ChangeType(dr[i], Nullable.GetUnderlyingType(type));
                            }
                            else if (type != dr.Table.Columns[i].DataType)
                            {
                                v = Convert.ChangeType(dr[i], type);
                            }
                            propertyInfo.SetValue(model, v, null);
                        }
                }

                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>  
        /// 填充对象：用DataRow填充实体类
        /// </summary>  
        public static T FillModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                {
                    Type type = propertyInfo.PropertyType;
                    object v = dr[i];
                    if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        v = Convert.ChangeType(dr[i], Nullable.GetUnderlyingType(type));
                    }
                    else if (type != dr.Table.Columns[i].DataType)
                    {
                        v = Convert.ChangeType(dr[i], type);
                    }
                    propertyInfo.SetValue(model, v, null);
                }
            }
            return model;
        }

        #endregion

        #region 实体类转换成DataTable

        /// <summary>
        /// 实体类转换成DataSet
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataSet FillDataSet(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(FillDataTable(modelList));
                return ds;
            }
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataTable FillDataTable(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    if (!propertyInfo.CanWrite || !propertyInfo.CanRead) continue;
                    var v = propertyInfo.GetValue(model, null);
                    if (v == null) v = DBNull.Value;
                    dataRow[propertyInfo.Name] = v;
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private static DataTable CreateData(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (!propertyInfo.CanWrite) continue;
                var colType = propertyInfo.PropertyType;
                if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    colType = colType.GetGenericArguments()[0];
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, colType));
            }
            return dataTable;
        }

        #endregion
    }
}
