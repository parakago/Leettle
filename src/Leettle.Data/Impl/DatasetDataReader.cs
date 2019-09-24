using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leettle.Data.Impl
{
    class DatasetDataReader : IDatasetDataReader
    {
        private DbDataReader dbDataReader;
        public DatasetDataReader(DbDataReader dbDataReader)
        {
            this.dbDataReader = dbDataReader;
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
    }
}
