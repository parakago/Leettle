using System;
using System.Collections.Generic;
using System.Text;

namespace Leettle.Data
{
    public delegate void TransactionJob(IConnection con);

    public interface IConnection : IDisposable
    {
        IDataset NewDataset(string sql);
        ICommand NewCommand(string sql);
        void RunInTransaction(TransactionJob job);
    }
}
