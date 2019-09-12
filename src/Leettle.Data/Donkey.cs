using Leettle.Data.Impl;
using System;
using System.Data.Common;

namespace Leettle.Data
{
    public class Donkey : IDisposable
    {
        private string connectionString;
        private Type dbConnectionType;

        public Donkey(string connectionString, Type dbConnectionType)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.dbConnectionType = dbConnectionType ?? throw new ArgumentNullException(nameof(dbConnectionType));
            if (!dbConnectionType.IsSubclassOf(typeof(DbConnection)))
            {
                throw new ArgumentException("hello world");
            }
        }

        public IConnection OpenConnection()
        {
            DbConnection instance = (DbConnection)Activator.CreateInstance(dbConnectionType);
            instance.ConnectionString = connectionString;
            return new Connection(instance);
        }

        public void Dispose()
        {
            
        }
    }
}
