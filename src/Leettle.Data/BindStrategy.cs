using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leettle.Data
{
    public abstract class BindStrategy
    {
        public char ParameterMarker { get; internal set; }
        public BindStrategy(char parameterMarker)
        {
            ParameterMarker = parameterMarker;
        }

        public abstract string ToPropertyName(string dbColName);
    }
}
