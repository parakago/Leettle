using System;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    public class DonkeyImpl : Donkey
    {
        private string connectionString;
        private Type dbConnectionType;

        public DonkeyImpl(string connectionString, Type dbConnectionType)
        {
            this.connectionString = connectionString;
            this.dbConnectionType = dbConnectionType;
        }

        public IConnection OpenConnection()
        {
            DbConnection instance = (DbConnection)Activator.CreateInstance(dbConnectionType);
            instance.ConnectionString = connectionString;
            return new Connection(instance);
        }
    }
}
