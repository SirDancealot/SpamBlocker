using System.Collections.Generic;

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
            ips = new List<string>
            {
                ip
            };
            DangerCount = dangerCount;
            UniqueIps = uniqe;
        }

        public override bool Matches(IP ip)
        {
            if (!(ip is IPrange))
                return false;
            return base.Matches(ip) && (ip as IPrange).Mask.Equals(this.Mask);
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
