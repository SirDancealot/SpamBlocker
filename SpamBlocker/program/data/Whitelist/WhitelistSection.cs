using System.Configuration;

namespace SpamBlocker.program.data.Whitelist
{
    class WhitelistSection : ConfigurationSection
    {
        [ConfigurationProperty("whitelisted", IsDefaultCollection = true)]
        public WhitelistElementCollection Whitelisted
        {
            get { return (WhitelistElementCollection)this["whitelisted"]; }
            set { this["whitelisted"] = value; }
        }
    }
}
