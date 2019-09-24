namespace Leettle.Data
{
    public class CleanBindStrategy : BindStrategy
    {
        public CleanBindStrategy(char parameterMarker) : base(parameterMarker)
        {

        }

        public override string ToPropertyName(string dbColName)
        {
            return dbColName;
        }
    }
}
