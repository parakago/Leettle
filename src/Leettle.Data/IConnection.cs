using System;

namespace Leettle.Data
{
    public delegate void TransactionConsumer(IConnection con);

    public interface IConnection : IDisposable
    {
        IDataset NewDataset(string sql);
        IDataset PreparedDataset(string sqlId);
        ICommand NewCommand(string sql);
        ICommand PreparedCommand(string sqlId);
        void RunInTransaction(Action<IConnection> job);
    }
}
