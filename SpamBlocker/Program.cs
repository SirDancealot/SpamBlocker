using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlocker
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, IPaddr> dict = FileReader.ReadFolder(ConfigurationManager.AppSettings.Get("ReadPath"));
            /*foreach (IPaddr ip in dict.Values)
            {
                Console.WriteLine("IP: " + ip.Ip + " registrered " + ip.Count);
            }*/
            string[] arr = { "yes", "true", "on" };
            if (arr.Contains(ConfigurationManager.AppSettings.Get("rangeCheck").ToLower()))
                dict = ExtraCheck.AddRanges(dict);
#if !DEBUG
            FirewallManager.BlockIPs(dict);
#else
            Console.ReadKey();
#endif
        }
    }
}
