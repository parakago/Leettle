using System;
using System.Collections.Generic;
using System.Text;

namespace Leettle.Data.Example
{
    class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDT { get; set; }
        public override string ToString()
        {
            return string.Format("{{id: {0}, name:\"{1}\", CreateDT: \"{2:o}\"}}", Id, Name, CreateDT);
        }
    }
}
