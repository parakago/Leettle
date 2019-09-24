namespace Leettle.Data
{
    public abstract class BindStrategy
    {
        public char ParameterMarker { get; }
        public BindStrategy(char parameterMarker)
        {
            ParameterMarker = parameterMarker;
        }

        public abstract string ToPropertyName(string dbColName);
    }
}
