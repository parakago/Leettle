using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace Leettle.Data.Impl
{
    class DatasetDataReader : IDatasetDataReader
    {
        private DbDataReader dbDataReader;
        private BindStrategy bindStrategy;
        private Dictionary<int, PropertyInfo> fieldMappingInfo;

        public DatasetDataReader(DbDataReader dbDataReader, BindStrategy bindStrategy)
        {
            this.dbDataReader = dbDataReader;
            this.bindStrategy = bindStrategy;
        }

        public bool Next()
        {
            return dbDataReader.Read();
        }

        public object GetObject(string colName)
        {
            return dbDataReader[colName];
        }

        public DateTime GetDateTime(string colName)
        {
            return Convert.ToDateTime(GetObject(colName));
        }

        public decimal GetDecimal(string colName)
        {
            return Convert.ToDecimal(GetObject(colName));
        }

        public double GetDouble(string colName)
        {
            return Convert.ToDouble(GetObject(colName));
        }

        public int GetInt(string colName)
        {
            return Convert.ToInt32(GetObject(colName));
        }

        public long GetLong(string colName)
        {
            return Convert.ToInt64(GetObject(colName));
        }

        public short GetShort(string colName)
        {
            return Convert.ToInt16(GetObject(colName));
        }

        public void GetStream(string colName, Stream stream)
        {
            byte[] bytes = (byte[])GetObject(colName);
            stream.Write(bytes, 0, bytes.Length);
        }

        public byte[] GetBytes(string colName)
        {
            return (byte[])GetObject(colName);
        }

        public string GetString(string colName)
        {
            return Convert.ToString(GetObject(colName));
        }

        public T Fetch<T>() where T : class, new()
        {
            if (fieldMappingInfo == null)
            {
                fieldMappingInfo = ExtractFieldMappingInfo<T>();
            }

            T target = (T)Activator.CreateInstance(typeof(T));
            foreach (var pair in fieldMappingInfo)
            {
                int columnIndex = pair.Key;
                object columnValue = dbDataReader.IsDBNull(columnIndex) ? null : dbDataReader.GetValue(columnIndex);
                if (columnValue != null)
                {
                    PropertyInfo propertyInfo = pair.Value;

                    if (propertyInfo.PropertyType == typeof(double))
                        columnValue = Convert.ToDouble(columnValue);
                    else if (propertyInfo.PropertyType == typeof(short))
                        columnValue = Convert.ToInt16(columnValue);
                    else if (propertyInfo.PropertyType == typeof(int))
                        columnValue = Convert.ToInt32(columnValue);
                    else if (propertyInfo.PropertyType == typeof(long))
                        columnValue = Convert.ToInt64(columnValue);

                    propertyInfo.SetValue(target, columnValue);
                }
            }
            return target;
        }

        public Dictionary<int, PropertyInfo> ExtractFieldMappingInfo<T>()
        {
            var fieldMappingInfo = new Dictionary<int, PropertyInfo>(dbDataReader.FieldCount);

            for (int i = 0; i < dbDataReader.FieldCount; ++i)
            {
                string columnName = dbDataReader.GetName(i);
                string propertyName = bindStrategy.ToPropertyName(columnName);
                var propInfo = LeettleDbUtil.FindProperty(typeof(T), propertyName);
                if (propInfo != null)
                {
                    fieldMappingInfo.Add(i, propInfo);
                }
            }

            return fieldMappingInfo;
        }
    }
}
