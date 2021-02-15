using SpamBlocker.program.data.IP;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace SpamBlocker.program.ui
{

    class Logger
    {
        private static Logger INSTANCE;
        private static  string logPath;
        private string logContent;

        internal void LogRun()
        {
            fOut.WriteLine("SpamBlocker was run the " + DateTime.Now);
        }

        internal void LogZero()
        {
            fOut.WriteLine("The newest accessed logfile had a size of 0");
        }

        internal void LogCustom(string msg)
        {
            fOut.WriteLine(msg);
        }

        internal void LogWhitelisted(string ip)
        {
            fOut.WriteLine("IP: " + ip + " was marked to be blocked, but was found in the whitelist");
        }

        //private FileStream fOut;
        private readonly StreamWriter fOut;


        private Logger()
        {
            INSTANCE = this;
            logPath = ConfigurationManager.AppSettings.Get("runLocation");
            if (!logPath.EndsWith("/") || !logPath.EndsWith("\\"))
                logPath += "/";
            logPath += "Log.txt";
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

        internal void LogFName(string name)
        {
            fOut.WriteLine("Name of the newest accessed file is: " + name);
        }

        public void LogIP(IP ip)
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

        public void LogMissingAdmin()
        {
            string errorMsg = "SpamBlocker was run with missing privileges the " + DateTime.Now;
            LogError(errorMsg);
        }

        public void LogError(string errorMsg)
        {
            fOut.WriteLine("Error: " + errorMsg);
        }

        public static Logger GetINSTANCE()
        {
            if (INSTANCE == null)
                new Logger();
            return INSTANCE;
        }

        public void Close()
        {
            if (fOut != null)
                fOut.Close();
        }
    }
}
