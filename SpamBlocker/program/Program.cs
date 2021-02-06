using SpamBlocker.program.data.IP;
using SpamBlocker.program.data.FileSetting;
using SpamBlocker.program.logic;
using SpamBlocker.program.ui;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;

namespace SpamBlocker.program
{
    class Program
    {
        private static readonly string[] yes = { "yes", "on", "true" };
        public static bool Debug() => yes.Contains(ConfigurationManager.AppSettings.Get("debugInfo").ToLower()) || DebugFull();
        public static bool DebugFull() => "full".Equals(ConfigurationManager.AppSettings.Get("debugInfo").ToLower());
        public static bool IsAdmin() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        static void Main(string[] args)
        {
#if DEBUG
            DebugPrint();
#endif
            Logger l = Logger.GetINSTANCE();
            if (Debug())
            {
                l.LogRun();
            }
            if (!IsAdmin())
            {
                l.LogMissingAdmin();
                l.Close();
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
            DebugPrint();
            Console.ReadKey();
#else
            FirewallManager.BlockIPs(IPManager.getInstance().Values);
#endif
            l.Close();
        }

        static void DebugPrint()
        {
            IP i1 = new IPaddr("192.168.1.237", 100);
            IP i2 = new IPrange("192.168.1.1", 25, 100, true);
            Console.WriteLine(i1);
            Console.WriteLine(i2);

            Console.WriteLine(IP.Masked("192.168.1.1", 9));

            foreach (IP iP in IPManager.GetInstance().Values)
            {
                Console.WriteLine("Ip: " + iP + " count: " + iP.Count);
            }
        }
    }
}
