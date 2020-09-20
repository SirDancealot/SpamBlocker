using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlocker.program.data.IP
{
    class IPrange : IP
    {
        private int Mask;
        private bool UniqueIps;
        List<string> ips;

        public IPrange(string ip, int mask, int dangerCount, bool uniqe)
        {
            Ip = Masked(ip, mask);
            Mask = mask;
            ips = new List<string>();
            ips.Add(ip);
            DangerCount = dangerCount;
            UniqueIps = uniqe;
        }

        public override void Registrer(string ip, int num = 1)
        {
            if (UniqueIps)
            {
                if (ips.Contains(ip))
                    return;
                ips.Add(ip);
            }
            Count += num;
        }
    }
}
