using System;
using System.IO;

namespace Leettle.Data
{
    public interface IRawDataset : IDisposable
    {
        void Open();
        bool Next();
        IRawDataset SetParam(String paramName, object paramValue);
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
    }
}
