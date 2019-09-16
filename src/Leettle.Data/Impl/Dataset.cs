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

        private Dictionary<int, PropertyInfo> ExtractFieldMappingInfo<T>(DbDataReader dbDataReader)
        {
            var fieldMappingInfo = new Dictionary<int, PropertyInfo>(dbDataReader.FieldCount);

            for (int i = 0; i < dbDataReader.FieldCount; ++i)
            {
                string fieldName = dbDataReader.GetName(i);
                PropertyInfo propInfo = typeof(T).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propInfo == null)
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
