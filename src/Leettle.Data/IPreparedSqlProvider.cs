namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPreparedSqlProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlId"></param>
        /// <returns></returns>
        string GetSql(string sqlId);
    }
}
