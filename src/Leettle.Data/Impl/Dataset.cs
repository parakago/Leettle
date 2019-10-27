using System;
using System.Collections.Generic;

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
            dbCommand.ExecuteReader(dbDataReaderWrapper =>
            {
                consumer.Invoke(new DatasetDataReader(dbDataReaderWrapper));
            });
        }

        public T OpenAndFetch<T>()
        {
            object result = null;
            Open(datasetReader =>
            {
                if (datasetReader.Next())
                {
                    result = datasetReader.Fetch<T>();
                }
            });

            return (T)result;
        }

        public List<T> OpenAndFetchList<T>()
        {
            List<T> list = new List<T>();
            Open(datasetReader =>
            {
                while (datasetReader.Next())
                {
                    list.Add(datasetReader.Fetch<T>());
                }
            });
            return list;
        }

        public T OpenAndFetchScalar<T>()
        {
            object result = null;
            Open(datasetReader =>
            {
                if (datasetReader.Next())
                {
                    result = datasetReader.DataReader[0];
                }
            });

            return (T)Convert.ChangeType(result, typeof(T));
        }

        public List<T> OpenAndFetchScalarList<T>()
        {
            List<T> list = new List<T>();
            Open(datasetReader =>
            {
                while (datasetReader.Next())
                {
                    list.Add((T)Convert.ChangeType(datasetReader.DataReader[0], typeof(T)));
                }
            });
            return list;
        }
    }
}
