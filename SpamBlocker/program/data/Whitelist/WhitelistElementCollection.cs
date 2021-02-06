using System.Configuration;

namespace SpamBlocker.program.data.Whitelist
{
    [ConfigurationCollection(typeof(WhitelistElement), AddItemName = "whitelist")]
    class WhitelistElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WhitelistElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WhitelistElement)element).IP;
        }
    }
}
