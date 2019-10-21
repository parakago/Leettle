using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace Leettle.Data.Impl
{
    class DatasetDataReader : IDatasetDataReader
    {
        public DbDataReader DbDataReader { get; }
        private readonly BindStrategy bindStrategy;
        private object fieldPropMappings;

        public DatasetDataReader(DbDataReader dbDataReader, BindStrategy bindStrategy)
        {
            this.DbDataReader = dbDataReader;
            this.bindStrategy = bindStrategy;
        }

        public bool Next()
        {
            return DbDataReader.Read();
        }

        public object GetObject(string colName)
        {
            return DbDataReader[colName];
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
            Assert.NotNull(stream, "stream must not be null");

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

        public T Fetch<T>()
        {
            if (fieldPropMappings == null)
            {
                fieldPropMappings = ExtractFieldMappingInfo<T>();
            }

            T target = (T)Activator.CreateInstance(typeof(T));

            foreach (var fieldPropMapping in (List<FieldPropMapping<T>>)fieldPropMappings)
            {
                object columnValue = DbDataReader.IsDBNull(fieldPropMapping.FieldIndex) ? null : DbDataReader.GetValue(fieldPropMapping.FieldIndex);
                if (columnValue != null)
                {
                    fieldPropMapping.SetValue(target, columnValue);
                }
            }

            return target;
        }

        public List<FieldPropMapping<T>> ExtractFieldMappingInfo<T>()
        {
            var fieldPropMappings = new List<FieldPropMapping<T>>(DbDataReader.FieldCount);

            for (int i = 0; i < DbDataReader.FieldCount; ++i)
            {
                string columnName = DbDataReader.GetName(i);
                string propertyName = bindStrategy.ToPropertyName(columnName);
                var propertyInfo = LeettleDbUtil.FindProperty(typeof(T), propertyName);
                if (propertyInfo != null)
                {
                    fieldPropMappings.Add(new FieldPropMapping<T>(i, propertyInfo));
                }
            }

            return fieldPropMappings;
        }
    }
}
