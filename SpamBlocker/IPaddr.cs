using System.ComponentModel;
using System.Configuration;
using System.Text;

namespace SpamBlocker
{
    class IPaddr
    {
        public readonly string Ip;
        public readonly int Mask;

        public IPaddr(string ip, int mask = 32)
        {
            Ip = ip;
            Mask = mask;
        }

        public int Count { get; set; } = 1;

        public void Registrer(int num = 1) => Count += num;

        public override string ToString()
        {
            if (Mask != 0 && Mask != 32)
                return Ip;
            return Ip + "/" + Mask;
        }

        public int ToBits()
        {
            int bits = 0;

            string[] bytes = Ip.Split('.');
            bits += (byte.Parse(bytes[0]) << 24);
            bits += (byte.Parse(bytes[1]) << 16);
            bits += (byte.Parse(bytes[2]) << 8);
            bits += byte.Parse(bytes[3]);

            return bits;
        }

        public string AndMask(int mask)
        {
            int bitMask = 0;
            for (int i = 0; i < mask; i++)
            {
                bitMask <<= 1;
                bitMask++;
            }
            bitMask <<= 32 - mask;

            int bits = ToBits() & bitMask;

            return BitsToString(bits);
        }

        public static IPaddr fromBitMask(int bits, int mask)
        {
            return new IPaddr(BitsToString(bits), mask);
        }

        public static IPaddr fromBitMask(string bits, int mask)
        {
            return new IPaddr(bits, mask);
        }

        private static string BitsToString(int bits)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bits & (0xFF << 24)).Append(".")
                .Append(bits & (0xFF << 16)).Append(".")
                .Append(bits & (0xFF <<  8)).Append(".")
                .Append(bits & 0xFF);
            return sb.ToString();
        }
    }
}