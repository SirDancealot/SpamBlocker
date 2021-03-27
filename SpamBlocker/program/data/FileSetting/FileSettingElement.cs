using System.Configuration;
using System;

namespace SpamBlocker.program.data.FileSetting
{
    public class FileSettingElement : ConfigurationElement
    {
        public FileSettingElement()
        {
            guid = Guid.NewGuid();
        }

        public Guid guid { get; }

        [ConfigurationProperty("ruleName", DefaultValue = "", IsRequired = false)]
        public string RuleName
        {
            get { return (string)this["ruleName"]; }
            set { this["ruleName"] = value; }
        }

        [ConfigurationProperty("readPath", IsRequired = true)]
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

        [ConfigurationProperty("doSearch", DefaultValue = "off", IsRequired = false)]
        public string DoSearch
        {
            get { return ((string)this["doSearch"]).ToLower(); }
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
        
        [ConfigurationProperty("fileCount", DefaultValue = 1, IsRequired = false)]
        public int FileCount
        {
            get { return (int)this["fileCount"]; }
            set { this["fileCount"] = value; }
        }

        [ConfigurationProperty("active", DefaultValue = true, IsRequired = false)]
        public bool Active
        {
            get { return (bool)this["active"]; }
            set { this["active"] = value; }
        }

        public string SourceFile { get; set; }
    }
}
