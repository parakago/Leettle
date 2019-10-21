namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LeettleDb
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IConnection OpenConnection();
    }
}
