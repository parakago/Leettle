using System;
using System.Collections.Generic;

namespace Leettle.Data
{
    public interface IDataset
    {
        void Open(Action<IDatasetDataReader> consumer);
        T OpenAndFetch<T>();
        List<T> OpenAndFetchList<T>();
        T OpenAndFetchScalar<T>();
        List<T> OpenAndFetchScalarList<T>();
        IDataset SetParam(String paramName, object paramValue);
        IDataset BindParam(object paramObject);
    }
}
