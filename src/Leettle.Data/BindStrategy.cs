namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BindStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        public char ParameterMarker { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterMarker"></param>
        protected BindStrategy(char parameterMarker)
        {
            ParameterMarker = parameterMarker;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbColName"></param>
        /// <returns></returns>
        public abstract string ToPropertyName(string dbColName);
    }
}
