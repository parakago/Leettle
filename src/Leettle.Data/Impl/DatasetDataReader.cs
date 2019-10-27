using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace Leettle.Data.Impl
{
    class DatasetDataReader : IDatasetDataReader
    {
        public DbDataReader DataReader { get { return dataReaderWrapper.DataReader; } }
        public DbDataReaderWrapper dataReaderWrapper;
        private object objectBinder;

        private Dictionary<string, int> ordinalMappings;

        public DatasetDataReader(DbDataReaderWrapper dataReaderWrapper)
        {
            this.dataReaderWrapper = dataReaderWrapper;
        }

        public bool Next()
        {
            return DataReader.Read();
        }

        private int FindOrdinal(string colName)
        {
            if (ordinalMappings == null) ordinalMappings = new Dictionary<string, int>();
            if (!ordinalMappings.TryGetValue(colName, out int ordinal))
            {
                ordinal = DataReader.GetOrdinal(colName);
                ordinalMappings.Add(colName, ordinal);
            }
            return ordinal;
        }

        public object GetObject(string colName)
        {
            return GetObject(FindOrdinal(colName));
        }

        public string GetString(string colName)
        {
            return GetString(FindOrdinal(colName));
        }

        public short GetShort(string colName)
        {
            return GetShort(FindOrdinal(colName));
        }

        public int GetInt(string colName)
        {
            return GetInt(FindOrdinal(colName));
        }

        public long GetLong(string colName)
        {
            return GetLong(FindOrdinal(colName));
        }

        public decimal GetDecimal(string colName)
        {
            return GetDecimal(FindOrdinal(colName));
        }

        public double GetDouble(string colName)
        {
            return GetDouble(FindOrdinal(colName));
        }

        public DateTime GetDateTime(string colName)
        {
            return GetDateTime(FindOrdinal(colName));
        }

        public byte[] GetBytes(string colName)
        {
            return GetBytes(FindOrdinal(colName));
        }

        public void GetStream(string colName, Stream stream)
        {
            GetStream(FindOrdinal(colName), stream);
        }

        public T Fetch<T>()
        {
            if (objectBinder == null)
            {
                objectBinder = dataReaderWrapper.CreateObjectBinder<T>();
            }

            return ((ObjectBinder<T>)objectBinder).BindNewObject(DataReader);
        }

        public object GetObject(int ordinal)
        {
            return DataReader.GetValue(ordinal);
        }

        public string GetString(int ordinal)
        {
            return DataReader.GetString(ordinal);
        }

        public short GetShort(int ordinal)
        {
            return DataReader.GetInt16(ordinal);
        }

        public int GetInt(int ordinal)
        {
            return DataReader.GetInt32(ordinal);
        }

        public long GetLong(int ordinal)
        {
            return DataReader.GetInt64(ordinal);
        }

        public decimal GetDecimal(int ordinal)
        {
            return DataReader.GetDecimal(ordinal);
        }

        public double GetDouble(int ordinal)
        {
            return Convert.ToDouble(DataReader.GetValue(ordinal));
        }

        public DateTime GetDateTime(int ordinal)
        {
            return DataReader.IsDBNull(ordinal) ? DateTime.MinValue : DataReader.GetDateTime(ordinal);
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
