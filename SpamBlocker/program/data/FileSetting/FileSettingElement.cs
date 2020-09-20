using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlocker.program.data.FileSetting
{
    public class FileSettingElement : ConfigurationElement
    {
        [ConfigurationProperty("readPath", IsKey = true, IsRequired = true)]
        public string ReadPath
        {
            get { return (string)this["readPath"]; }
            set { this["readPath"] = value; }
        }

        [ConfigurationProperty("count", DefaultValue = 10, IsRequired = false)]
        public int Count
        {
            get { return (int)this["count"]; }
            set { this["count"] = value; }
        }

        [ConfigurationProperty("rangeCheck", DefaultValue = false, IsRequired = false)]
        public bool RangeCheck
        {
            get { return (bool)this["rangeCheck"]; }
            set { this["rangeCheck"] = value; }
        }

        [ConfigurationProperty("maskSize", DefaultValue = 24, IsRequired = false)]
        public int MaskSize
        {
            get { return (int)this["maskSize"]; }
            set { this["maskSize"] = value; }
        }

        [ConfigurationProperty("delimiter", DefaultValue = ",", IsRequired = false)]
        public char Delim
        {
            get { return (char)this["delimiter"]; }
            set { this["delimiter"] = value; }
        }

        [ConfigurationProperty("ipIndex", DefaultValue = 0, IsRequired = false)]
        public int IpIndex
        {
            get { return (int)this["ipIndex"]; }
            set { this["ipIndex"] = value; }
        }

        [ConfigurationProperty("doSearch", DefaultValue = false, IsRequired = false)]
        public bool DoSearch
        {
            get { return (bool)this["doSearch"]; }
            set { this["doSearch"] = value; }
        }

        [ConfigurationProperty("searchPattern", DefaultValue = "", IsRequired = false)]
        public string SearchPattern
        {
            get { return (string)this["searchPattern"]; }
            set { this["searchPattern"] = value; }
        }

        [ConfigurationProperty("countUniqeIps", DefaultValue = true, IsRequired = false)]
        public bool UniqeIps
        {
            get { return (bool)this["countUniqeIps"]; }
            set { this["countUniqeIps"] = value; }
        }

        [ConfigurationProperty("commentPattern", DefaultValue = "", IsRequired = false)]
        public string CommentStart
        {
            get { return (string)this["commentPattern"]; }
            set { this["commentPattern"] = value; }
        }
        
        [ConfigurationProperty("ruleName", DefaultValue = "", IsRequired = false)]
        public string RuleName
        {
            get { return (string)this["ruleName"]; }
            set { this["ruleName"] = value; }
        }

        public string SourceFile { get; set; }
    }
}
