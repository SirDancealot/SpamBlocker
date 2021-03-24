using SpamBlocker.program.data.IP;
using SpamBlocker.program.data.FileSetting;
using SpamBlocker.program.logic;
using SpamBlocker.program.ui;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using SpamBlocker.program.data.Whitelist;
using System.Collections.Generic;

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

            WhitelistSection wlSection = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).Sections["whitelistSection"] as WhitelistSection;
            WhitelistElementCollection wlColl = wlSection.Whitelisted;
            List<IPrange> whitelist = new List<IPrange>();
            foreach (WhitelistElement element in wlColl)
            {
                whitelist.Add(new IPrange(element.IP, element.Range, 0, false));
            }

            FileSettingSection fsSection = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).Sections["fileSettingsSection"] as FileSettingSection;
            FileSettingElementCollection fsColl = fsSection.FileSettings;
            foreach (FileSettingElement element in fsColl)
            {
                FileReader.ReadFolder(element);
                Console.WriteLine(element.ReadPath);
            }



#if DEBUG
            DebugPrint();
            //FirewallManager.BlockIPs(IPManager.GetInstance().Values, whitelist);
            Console.ReadKey();
#else
            if (Debug())
                DebugPrint();
            FirewallManager.BlockIPs(IPManager.GetInstance().Values, whitelist);
#endif
            l.Close();
        }

        static void DebugPrint()
        {
            foreach (IP iP in IPManager.GetInstance().Values)
            {
                Console.WriteLine("Ip: " + iP + " count: " + iP.Count);
            }
        }
    }
}
