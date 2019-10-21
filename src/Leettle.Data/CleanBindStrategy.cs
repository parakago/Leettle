namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class CleanBindStrategy : BindStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterMarker"></param>
        public CleanBindStrategy(char parameterMarker) : base(parameterMarker)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbColName"></param>
        /// <returns></returns>
        public override string ToPropertyName(string dbColName)
        {
            return dbColName;
        }
    }
}
