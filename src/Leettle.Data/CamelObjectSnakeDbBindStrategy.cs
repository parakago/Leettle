using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leettle.Data
{
    public class CamelObjectSnakeDbBindStrategy : BindStrategy
    {
        public CamelObjectSnakeDbBindStrategy(char parameterMarker) : base(parameterMarker)
        {

        }

        public override string ToPropertyName(string dbColName)
        {
            return LeettleDbUtil.SnakeToCamel(dbColName);
        }
    }
}
