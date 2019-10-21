using System;
using System.Collections.Generic;

namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataset
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumer"></param>
        void Open(Action<IDatasetDataReader> consumer);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T OpenAndFetch<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> OpenAndFetchList<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T OpenAndFetchScalar<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> OpenAndFetchScalarList<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        IDataset SetParam(String paramName, object paramValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        IDataset BindParam(object paramObject);
    }
}
