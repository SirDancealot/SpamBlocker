using SpamBlocker.program.data;
using SpamBlocker.program.logic;
using SpamBlocker.program.ui;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlocker.program
{
    class Program
    {
        private static string[] yes = { "yes", "on", "true" };
        public static bool debug() => yes.Contains(ConfigurationManager.AppSettings.Get("debugInfo").ToLower());
        public static bool isAdmin() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        static void Main(string[] args)
        {
            FileSettingSection section = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).Sections["fileSettingsSection"] as FileSettingSection;
            FileSettingElementCollection coll = section.FileSettings;
            foreach (FileSettingElement element in coll)
            {
                Console.WriteLine(element.ReadPath);
            }

            Logger l = Logger.getINSTANCE();
            if (debug())
            {
                l.logRun();
            }
            if (!isAdmin())
            {
                l.logMissingAdmin();
                l.close();
                Environment.Exit(0);
            }
            /*
            var testSection = ConfigurationManager.GetSection("testSection") as NameValueCollection;

            Console.WriteLine(testSection["testKey"]);
            */
            Dictionary<string, IPaddr> dict = FileReader.ReadFolder(ConfigurationManager.AppSettings.Get("readPath"));


#if !DEBUG
            FirewallManager.BlockIPs(dict);
#else
            Console.ReadKey();
#endif
            l.close();
        }
    }
}
