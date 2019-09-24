using System;
using System.Collections.Generic;
using System.Text;

namespace Leettle.Data
{
    public delegate void TransactionConsumer(IConnection con);

    public interface IConnection : IDisposable
    {
        IDataset NewDataset(string sql);
        ICommand NewCommand(string sql);
        void RunInTransaction(Action<IConnection> job);
    }
}
