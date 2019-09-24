using System;
using System.Collections.Generic;

namespace Leettle.Data
{
    public interface IDataset
    {
        void Open(Action<IDatasetDataReader> consumer);
        List<T> OpenAndFetchList<T>() where T : class, new();
        IDataset SetParam(String paramName, object paramValue);
        IDataset BindParam(object paramObject);
    }
}
