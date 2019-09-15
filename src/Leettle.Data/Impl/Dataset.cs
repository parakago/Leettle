using System;
using System.Data.Common;
using System.IO;

namespace Leettle.Data.Impl
{
    class Dataset : AbstractQuery, IDataset, IDisposable
    {
        private DbDataReader dbDataReader;
        public Dataset(DbCommand dbCommand) : base(dbCommand)
        {
            
        }

        public bool Next()
        {
            return CheckDataReader().Read();
        }

        public void Open()
        {
            dbDataReader = ExecuteReader();
        }

        public IDataset SetParam(string paramName, object paramValue)
        {
            return (IDataset)AddParam(paramName, paramValue);
        }

        public override void Dispose()
        {
            Util.DisposeSilently(dbDataReader);
            base.Dispose();
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
            return Convert.ToString(GetObject(colName));
        }
    }
}
