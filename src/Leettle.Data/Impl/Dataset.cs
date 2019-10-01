using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    class Dataset : IDataset
    {
        private readonly DbCommandWrapper dbCommand;
        public Dataset(DbCommandWrapper dbCommand)
        {
            this.dbCommand = dbCommand;
        }

        public IDataset SetParam(string paramName, object paramValue)
        {
            dbCommand.AddParam(paramName, paramValue);
            return this;
        }

        public IDataset BindParam(object paramObject)
        {
            dbCommand.BindParam(paramObject);
            return this;
        }

        public void Open(Action<IDatasetDataReader> consumer)
        {
            dbCommand.ExecuteReader(dbDataReader =>
            {
                consumer.Invoke(new DatasetDataReader(dbDataReader, dbCommand.BindStrategy));
            });
        }

        public T OpenAndFetch<T>()
        {
            object result = null;
            Open(dr =>
            {
                if (dr.Next())
                {
                    result = dr.Fetch<T>();
                }
            });

            return (T)result;
        }

        public List<T> OpenAndFetchList<T>()
        {
            List<T> list = new List<T>();
            Open(dr =>
            {
                while (dr.Next())
                {
                    list.Add(dr.Fetch<T>());
                }
            });
            return list;
        }

        public T OpenAndFetchScalar<T>()
        {
            object result = null;
            Open(dr =>
            {
                if (dr.Next())
                {
                    result = dr.DbDataReader[0];
                }
            });

            return (T)Convert.ChangeType(result, typeof(T));
        }

        public List<T> OpenAndFetchScalarList<T>()
        {
            List<T> list = new List<T>();
            Open(dr =>
            {
                while (dr.Next())
                {
                    list.Add((T)Convert.ChangeType(dr.DbDataReader[0], typeof(T)));
                }
            });
            return list;
        }
    }
}
