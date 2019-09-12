using System;
using System.Data.Common;
using System.IO;

namespace Leettle.Data.Impl
{
    class Dataset : IDataset
    {
        private DbCommand dbCmd;
        private DbDataReader dbDataReader;
        public Dataset(DbCommand dbCmd)
        {
            this.dbCmd = dbCmd;
        }

        public bool Next()
        {
            return CheckDataReader().Read();
        }

        public void Open()
        {
            try
            {
                dbDataReader = dbCmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw SqlException.Wrap(e, dbCmd);
            }
        }

        public IDataset SetParam(string paramName, object paramValue)
        {
            var dbParam = dbCmd.CreateParameter();
            dbParam.ParameterName = paramName;
            dbParam.Value = paramValue;
            dbCmd.Parameters.Add(dbParam);
            return this;
        }

        public void Dispose()
        {
            if (dbDataReader != null)
            {
                dbDataReader.Close();
            }

            dbCmd.Dispose();
        }

        private DbDataReader CheckDataReader()
        {
            if (dbDataReader == null)
            {
                throw new Exception("open dataset first");
            }
            return dbDataReader;
        }

        public object GetObject(string colName)
        {
            return CheckDataReader()[colName];
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
            object value = GetObject(colName);
            return value == null ? null : Convert.ToString(value);
        }
    }
}
