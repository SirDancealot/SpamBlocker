using SpamBlocker.program.data.IP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SpamBlocker.program.ui
{

    class Logger
    {
        private static Logger INSTANCE;
        private static readonly string logPath = ConfigurationManager.AppSettings.Get("runLocation") + "Log.txt";
        private string logContent;

        internal void logRun()
        {
            fOut.WriteLine("SpamBlocker was run the " + DateTime.Now);
        }

        internal void logZero()
        {
            fOut.WriteLine("The newest accessed logfile had a size of 0");
        }

        internal void logCustom(string msg)
        {
            fOut.WriteLine(msg);
        }

        //private FileStream fOut;
        private readonly StreamWriter fOut;


        private Logger()
        {
            INSTANCE = this;
            if (!File.Exists(logPath))
                File.Create(logPath).Close();
            logContent = File.ReadAllText(logPath);
            //fOut = File.Open(logPath, FileMode.Append);
            try
            {
                fOut = File.AppendText(logPath);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot open file");
                //Console.ReadKey();
            }
        }

        internal void logFName(string name)
        {
            fOut.WriteLine("Name of the newest accessed file is: " + name);
        }

        public void logIP(IP ip)
        {
            DateTime now = DateTime.Now;
            if (logContent.Contains(ip.ToString()))
                return;
            logContent += ip;
            StringBuilder sb = new StringBuilder();
            bool rule = false;
            if (ip.ruleName != "")
            {
                rule = true;
                sb.Append("Rule ")
                    .Append(ip.ruleName);

            }
            sb.Append(rule ? " blocked " : "Blocked ")
                .Append(ip)
                .Append(" with ")
                .Append(ip.Count)
                .Append(" attempts on the ")
                .Append(now)
                .Append(" from file ")
                .Append(ip.sourceFile);
            //.Append('\n');
            //fOut.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            fOut.WriteLine(sb.ToString());
        }

        public void logMissingAdmin()
        {
            string errorMsg = "SpamBlocker was run with missing privileges the " + DateTime.Now;
            //fOut.Write(Encoding.ASCII.GetBytes(errorMsg), 0, errorMsg.Length);
            fOut.WriteLine(errorMsg);
        }

        public static Logger getINSTANCE()
        {
            if (INSTANCE == null)
                new Logger();
            return INSTANCE;
        }

        public void close()
        {
            if (fOut != null)
                fOut.Close();
        }
    }
}
