using System;

namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IDataset NewDataset(string sql);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlId"></param>
        /// <returns></returns>
        IDataset PreparedDataset(string sqlId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        ICommand NewCommand(string sql);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlId"></param>
        /// <returns></returns>
        ICommand PreparedCommand(string sqlId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        void RunInTransaction(Action<IConnection> job);
    }
}
