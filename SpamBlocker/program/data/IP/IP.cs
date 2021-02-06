using System.Text;

namespace SpamBlocker.program.data.IP
{
    abstract class IP
    {
        public string Ip { get; protected set; }
        public string sourceFile { get; set; }
        public string ruleName { get; set; }
        public int Count { get; set; } = 1;
        public int DangerCount { get; protected set; }

        public override string ToString() => Ip;

        public abstract void Registrer(string ip, int num = 1);
        public virtual bool Matches(IP ip) => this.Ip.Equals(ip.Ip);

        public static long ToBits(string ip)
        {
            long bits = 0;

            string[] bytes = ip.Split('.');
            bits += byte.Parse(bytes[0]) << 24;
            bits += byte.Parse(bytes[1]) << 16;
            bits += byte.Parse(bytes[2]) << 8;
            bits += byte.Parse(bytes[3]);

            return bits;
        }
        public static string Masked(string ip, int mask)
        {
            long bitMask = 0;
            for (int i = 0; i < mask; i++)
            {
                bitMask <<= 1;
                bitMask++;
            }
            bitMask <<= 32 - mask;

            long bits = ToBits(ip) & bitMask;

            return BitsToString(bits) + "/" + mask;
        }
        private static string BitsToString(long bits)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bits >> 24 & 0xFF).Append(".")
                .Append(bits >> 16 & 0xFF).Append(".")
                .Append(bits >> 8 & 0xFF).Append(".")
                .Append(bits & 0xFF);
            return sb.ToString();
        }
    }
}
