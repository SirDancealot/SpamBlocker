using System.Configuration;

namespace SpamBlocker.program.data.FileSetting
{
    [ConfigurationCollection(typeof(FileSettingElement), AddItemName = "fileSetting")]
    class FileSettingElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileSettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileSettingElement)element).ReadPath;
        }
    }
}
