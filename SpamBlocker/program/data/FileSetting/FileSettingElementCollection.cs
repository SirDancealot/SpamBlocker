using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
