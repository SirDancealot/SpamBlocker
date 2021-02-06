using SpamBlocker.program.data.IP;
using SpamBlocker.program.data.FileSetting;
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
#if DEBUG
            Debug();
#endif
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


            FileSettingSection section = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).Sections["fileSettingsSection"] as FileSettingSection;
            FileSettingElementCollection coll = section.FileSettings;
            foreach (FileSettingElement element in coll)
            {
                FileReader.ReadFolder(element);
                Console.WriteLine(element.ReadPath);
            }



#if DEBUG
            Debug();
            Console.ReadKey();
#else
            FirewallManager.BlockIPs(IPManager.getInstance().Values);
#endif
            l.close();
        }

        static void Debug()
        {
            IP i1 = new IPaddr("192.168.1.237", 100);
            IP i2 = new IPrange("192.168.1.1", 25, 100, true);
            Console.WriteLine(i1);
            Console.WriteLine(i2);

            Console.WriteLine(IP.Masked("192.168.1.1", 9));

            foreach (IP iP in IPManager.getInstance().Values)
            {
                Console.WriteLine("Ip: " + iP + " count: " + iP.Count);
            }
        }
    }
}
