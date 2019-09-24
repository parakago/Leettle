using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    class Dataset : IDataset
    {
        private DbCommandWrapper dbCommand;
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
            using (DbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                consumer.Invoke(new DatasetDataReader(dbDataReader, dbCommand.BindStrategy));
            }
        }

        public List<T> OpenAndFetchList<T>() where T : class, new()
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
    }
}
