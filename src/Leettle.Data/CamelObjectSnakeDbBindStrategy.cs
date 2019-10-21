namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class CamelObjectSnakeDbBindStrategy : BindStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterMarker"></param>
        public CamelObjectSnakeDbBindStrategy(char parameterMarker) : base(parameterMarker)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbColName"></param>
        /// <returns></returns>
        public override string ToPropertyName(string dbColName)
        {
            return LeettleDbUtil.SnakeToCamel(dbColName);
        }
    }
}
