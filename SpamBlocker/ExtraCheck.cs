using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlocker
{
    class ExtraCheck
    {
        public static Dictionary<string, IPaddr> AddRanges(Dictionary<string, IPaddr> ips)
        {
            int mask = int.Parse(ConfigurationManager.AppSettings.Get("maskSize"));
            Dictionary<string, IPaddr> ranges = new Dictionary<string, IPaddr>();
            foreach (IPaddr ip in ips.Values)
            {
                if (ranges.ContainsKey(ip.AndMask(mask)))
                    ranges[ip.AndMask(mask)+"/"+mask].Registrer(ip.Count);
                else
                {
                    IPaddr newRange = IPaddr.fromBitMask(ip.AndMask(mask), mask);
                    newRange.Count = ip.Count;
                    ranges.Add(newRange.ToString(), newRange);
                }
                    
            }

            foreach (var newIP in ranges)
            {
                ips.Add(newIP.Key, newIP.Value);
            }

            return ips;
        }
    }
}
