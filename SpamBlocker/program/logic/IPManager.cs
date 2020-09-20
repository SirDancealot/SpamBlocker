using SpamBlocker.program.data.FileSetting;
using SpamBlocker.program.data.IP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlocker.program.logic
{
    class IPManager
    {
        Dictionary<string, IP> ips;

        private IPManager() 
        {
            ips = new Dictionary<string, IP>();
        }

        private static IPManager INSTANCE;

        public static IPManager getInstance()
        {
            if (INSTANCE == null)
                INSTANCE = new IPManager();
            return INSTANCE;
        }

        public void Registrer(string ip, FileSettingElement settings)
        {
            if (settings.RangeCheck)
            {
                string ipr = IP.Masked(ip, settings.MaskSize);
                if (ips.ContainsKey(ipr))
                    ips[ipr].Registrer(ip);
                else
                {
                    IP _ip = new IPrange(ip, settings.MaskSize, settings.Count, settings.UniqeIps);
                    _ip.ruleName = settings.RuleName;
                    _ip.sourceFile = settings.SourceFile;
                    ips.Add(ipr, _ip);
                }
            } else
            {
                if (ips.ContainsKey(ip))
                    ips[ip].Registrer(ip);
                else
                {
                    IP _ip = new IPaddr(ip, settings.Count);
                    _ip.ruleName = settings.RuleName;
                    _ip.sourceFile = settings.SourceFile;
                    ips.Add(ip, _ip);
                }
            }
        }

        public Dictionary<string, IP>.ValueCollection Values => ips.Values;
    }
}
