using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace Leettle.Data.Impl
{
    class Dataset : IDataset
    {
        private DbCommandWrapper dbCommand;
        public Dataset(DbCommandWrapper dbCommand)
        {
            this.dbCommand = dbCommand;
        }

        public IDataset SetParam(string paramName, object paramValue)
        {
            dbCommand.AddParam(paramName, paramValue);
            return this;
        }

        public IDataset BindParam(object paramObject)
        {
            dbCommand.BindParam(paramObject);
            return this;
        }

        private Dictionary<int, PropertyInfo> ExtractFieldMappingInfo<T>(DbDataReader dbDataReader)
        {
            var fieldMappingInfo = new Dictionary<int, PropertyInfo>(dbDataReader.FieldCount);

            for (int i = 0; i < dbDataReader.FieldCount; ++i)
            {
                string columnName = dbDataReader.GetName(i);
                string propertyName = dbCommand.BindStrategy.ToPropertyName(columnName);
                var propInfo = LeettleDbUtil.FindProperty(typeof(T), propertyName);
                if (propInfo != null)
                {
                    fieldMappingInfo.Add(i, propInfo);
                }
            }

            return fieldMappingInfo;
        }

        private T Fetch<T>(DbDataReader dbDataReader, Dictionary<int, PropertyInfo> fieldMappingInfo) where T : class, new()
        {
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

        public T OpenAndFetch<T>() where T : class, new()
        {
            using (DbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                if (dbDataReader.Read())
                {
                    var fieldMappingInfo = ExtractFieldMappingInfo<T>(dbDataReader);
                    return Fetch<T>(dbDataReader, fieldMappingInfo);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<T> OpenAndFetchList<T>() where T : class, new()
        {
            using (DbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                Dictionary<int, PropertyInfo> fieldMappingInfo = null;

                List<T> list = new List<T>();
                while (dbDataReader.Read())
                {
                    if (fieldMappingInfo == null)
                    {
                        fieldMappingInfo = ExtractFieldMappingInfo<T>(dbDataReader);
                    }
                    list.Add(Fetch<T>(dbDataReader, fieldMappingInfo));
                }
                return list;
            }    
        }
    }
}
