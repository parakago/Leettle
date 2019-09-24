using System;
using System.IO;

namespace Leettle.Data
{
    public interface IDatasetDataReader
    {
        bool Next();
        object GetObject(string colName);
        string GetString(string colName);
        short GetShort(string colName);
        int GetInt(string colName);
        long GetLong(string colName);
        decimal GetDecimal(string colName);
        double GetDouble(string colName);
        DateTime GetDateTime(string colName);
        byte[] GetBytes(string colName);
        void GetStream(String fieldName, Stream stream);
        T Fetch<T>() where T : class, new();
    }
}
