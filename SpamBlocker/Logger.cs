using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SpamBlocker
{

    class Logger
    {
        private static Logger INSTANCE;
        private static readonly string logPath = ConfigurationManager.AppSettings.Get("LogPath");
        private string logContent;
        private FileStream fOut;


        private Logger()
        {
            INSTANCE = this;
            if (!File.Exists(logPath))
                File.Create(logPath).Close();
            logContent = File.ReadAllText(logPath);
            fOut = File.Open(logPath, FileMode.Append);
        }

        public void logIP(IPaddr ip)
        {
            DateTime now = DateTime.Now;
            if (logContent.Contains(ip.ToString()))
                return;
            logContent += ip;
            StringBuilder sb = new StringBuilder();
            sb.Append(ip)
                .Append(" was blocked with ")
                .Append(ip.Count)
                .Append(" attempts on the ")
                .Append(now)
                .Append('\n');
            fOut.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            
        }

        public static Logger getINSTANCE()
        {
            if (INSTANCE == null)
                new Logger();
            return INSTANCE;
        }

        public void close()
        {
            fOut.Close();
        }
    }
}
