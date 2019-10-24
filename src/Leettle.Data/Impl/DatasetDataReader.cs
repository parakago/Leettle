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
        private Dictionary<string, int> ordinalMappings;

        public DatasetDataReader(DbDataReader dbDataReader, BindStrategy bindStrategy)
        {
            this.DbDataReader = dbDataReader;
            this.bindStrategy = bindStrategy;
        }

        public bool Next()
        {
            return DbDataReader.Read();
        }

        private int GetOrdinal(string colName)
        {
            if (ordinalMappings == null) ordinalMappings = new Dictionary<string, int>();
            if (!ordinalMappings.TryGetValue(colName, out int ordinal))
            {
                ordinal = DbDataReader.GetOrdinal(colName);
                ordinalMappings.Add(colName, ordinal);
            }
            return ordinal;
        }

        public object GetObject(string colName)
        {
            return GetObject(GetOrdinal(colName));
        }

        public string GetString(string colName)
        {
            return GetString(GetOrdinal(colName));
        }

        public short GetShort(string colName)
        {
            return GetShort(GetOrdinal(colName));
        }

        public int GetInt(string colName)
        {
            return GetInt(GetOrdinal(colName));
        }

        public long GetLong(string colName)
        {
            return GetLong(GetOrdinal(colName));
        }

        public decimal GetDecimal(string colName)
        {
            return GetDecimal(GetOrdinal(colName));
        }

        public double GetDouble(string colName)
        {
            return GetDouble(GetOrdinal(colName));
        }

        public DateTime GetDateTime(string colName)
        {
            return GetDateTime(GetOrdinal(colName));
        }

        public byte[] GetBytes(string colName)
        {
            return GetBytes(GetOrdinal(colName));
        }

        public void GetStream(string colName, Stream stream)
        {
            GetStream(GetOrdinal(colName), stream);
        }

        private static List<FieldPropMapping<T>> ExtractFieldMappingInfo<T>(DbDataReader reader, BindStrategy bindStrategy)
        {
            var fieldPropMappings = new List<FieldPropMapping<T>>(reader.FieldCount);

            for (int i = 0; i < reader.FieldCount; ++i)
            {
                string columnName = reader.GetName(i);
                string propertyName = bindStrategy.ToPropertyName(columnName);
                var propertyInfo = LeettleDbUtil.FindProperty(typeof(T), propertyName);
                if (propertyInfo != null)
                {
                    fieldPropMappings.Add(new FieldPropMapping<T>(i, propertyInfo));
                }
            }

            return fieldPropMappings;
        }

        public T Fetch<T>()
        {
            if (fieldPropMappings == null)
            {
                fieldPropMappings = ExtractFieldMappingInfo<T>(DbDataReader, bindStrategy);
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

        public object GetObject(int ordinal)
        {
            return DbDataReader.GetValue(ordinal);
        }

        public string GetString(int ordinal)
        {
            return DbDataReader.GetString(ordinal);
        }

        public short GetShort(int ordinal)
        {
            return DbDataReader.GetInt16(ordinal);
        }

        public int GetInt(int ordinal)
        {
            return DbDataReader.GetInt32(ordinal);
        }

        public long GetLong(int ordinal)
        {
            return DbDataReader.GetInt64(ordinal);
        }

        public decimal GetDecimal(int ordinal)
        {
            return DbDataReader.GetDecimal(ordinal);
        }

        public double GetDouble(int ordinal)
        {
            return DbDataReader.GetDouble(ordinal);
        }

        public DateTime GetDateTime(int ordinal)
        {
            return DbDataReader.GetDateTime(ordinal);
        }

        public byte[] GetBytes(int ordinal)
        {
            return (byte[])GetObject(ordinal);
        }

        public void GetStream(int ordinal, Stream stream)
        {
            Assert.NotNull(stream, "stream must not be null");

            byte[] bytes = GetBytes(ordinal);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
