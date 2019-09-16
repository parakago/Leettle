using System;
using System.Collections.Generic;

namespace Leettle.Data
{
    public interface IDataset
    {
        T OpenAndFetch<T>() where T : class, new();
        List<T> OpenAndFetchList<T>() where T : class, new();
        IDataset SetParam(String paramName, object paramValue);
    }
}
