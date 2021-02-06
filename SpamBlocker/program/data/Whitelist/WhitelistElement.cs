using System.Configuration;

namespace SpamBlocker.program.data.Whitelist
{
    class WhitelistElement : ConfigurationElement
    {
        [ConfigurationProperty("ip", IsKey = true, IsRequired = true)]
        public string IP
        {
            get { return (string)this["ip"]; }
            set { this["ip"] = value; }
        }

        [ConfigurationProperty("range", IsRequired = false, DefaultValue = 32)]
        public int Range
        {
            get { return (int)this["range"]; }
            set { this["range"] = value; }
        }
    }
}
