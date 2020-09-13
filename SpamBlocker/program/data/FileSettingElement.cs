using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlocker.program.data
{
    public class FileSettingElement : ConfigurationElement
    {
        [ConfigurationProperty("readPath", IsKey = true, IsRequired = true)]
        public string ReadPath
        {
            get { return (string)this["readPath"]; }
            set { this["readPath"] = value; }
        }

        [ConfigurationProperty("count", DefaultValue = 3, IsRequired = false)]
        public int Count
        {
            get { return (int)this["count"]; }
            set { this["count"] = value; }
        }

        [ConfigurationProperty("rangeCheck", DefaultValue = false, IsRequired = false)]
        public bool rangeCheck
        {
            get { return (bool)this["rangeCheck"]; }
            set { this["rangeCheck"] = value; }
        }

        [ConfigurationProperty("maskSize", DefaultValue = 24, IsRequired = false)]
        public int maskSize
        {
            get { return (int)this["maskSize"]; }
            set { this["maskSize"] = value; }
        }
    }
}
