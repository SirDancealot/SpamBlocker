using System.ComponentModel;
using System.Configuration;
using System.Text;

namespace SpamBlocker.program.data.IP
{
    class IPaddr : IP
    {
        public IPaddr(string ip, int dangerCount)
        {
            Ip = ip;
            DangerCount = dangerCount;
        }

        public override void Registrer(string ip, int num = 1) => Count += num;



    }
}