using System;
using System.Data.Common;
using System.IO;

namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatasetDataReader
    {
        /// <summary>
        /// 
        /// </summary>
        DbDataReader DbDataReader { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Next();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        object GetObject(string colName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        string GetString(string colName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        short GetShort(string colName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        int GetInt(string colName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        long GetLong(string colName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        decimal GetDecimal(string colName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        double GetDouble(string colName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        DateTime GetDateTime(string colName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        byte[] GetBytes(string colName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="stream"></param>
        void GetStream(String fieldName, Stream stream);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Fetch<T>();
    }
}
