using System.Collections.Generic;

namespace SpamBlocker.program.data.IP
{
    class IPrange : IP
    {
        public int Mask { get; }
        private readonly bool UniqueIps;
        private readonly List<string> ips;

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
            if (!Masked(ip.Ip, Mask).Equals(Ip))
                return false;
            if (!(ip is IPrange))
                return true;
            return (Mask <= (ip as IPrange).Mask);
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
