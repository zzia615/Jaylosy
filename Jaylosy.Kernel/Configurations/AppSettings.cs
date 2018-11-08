using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace Jaylosy.Kernel.Configurations
{
    public class AppSettings
    {
        protected string this[string key]
        {
            get
            {
                return ConfigurationManager.AppSettings[key];
            }
        }

        /// <summary>
        /// 日志路径
        /// </summary>
        public string LogDir
        {
            get
            {
                return this["logDir"];
            }
        }
    }
}
