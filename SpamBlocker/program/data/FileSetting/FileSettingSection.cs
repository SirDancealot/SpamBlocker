using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlocker.program.data.FileSetting
{
    class FileSettingSection : ConfigurationSection
    {
        [ConfigurationProperty("fileSettings", IsDefaultCollection = true)]
        public FileSettingElementCollection FileSettings
        {
            get { return (FileSettingElementCollection)this["fileSettings"]; }
            set { this["fileSettings"] = value;  }
        }
    }
}
