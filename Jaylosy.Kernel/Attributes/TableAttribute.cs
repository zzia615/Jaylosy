using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jaylosy.Kernel.Attributes
{
    public class TableAttribute:Attribute
    {
        public string Name { get; set; }
        public string PrimaryKey { get; set; }
    }
}
