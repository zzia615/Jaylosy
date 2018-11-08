using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Jaylosy.Kernel.Configurations
{
    public class ConnectionStrings
    {
        protected string this[string name]
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
        }
    }
}
