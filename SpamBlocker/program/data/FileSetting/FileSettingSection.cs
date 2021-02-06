using System.Configuration;

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
