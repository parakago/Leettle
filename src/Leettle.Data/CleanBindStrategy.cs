using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
