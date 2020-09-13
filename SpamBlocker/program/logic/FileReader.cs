using SpamBlocker.program.data;
using SpamBlocker.program.ui;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SpamBlocker.program.logic
{
    class FileReader
    {
        private static Dictionary<string, IPaddr> addrs = new Dictionary<string, IPaddr>();
        public static Dictionary<string, IPaddr> ReadFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                throw new ArgumentException("Folder Path illegal", nameof(folder));
            }

            var directory = new DirectoryInfo(folder);
            FileInfo f = directory.GetFiles().OrderBy(sf => sf.Name).Last();

            if (Program.debug())
            {
                if (f.Length == 0)
                    Logger.getINSTANCE().logZero();
                Logger.getINSTANCE().logFName(f.Name);
            }


            string sourceFile = f.Name;
            f = f.CopyTo(ConfigurationManager.AppSettings.Get("runLocation") + "tmp.LOG");
            ReadFile(f, addrs, sourceFile);
            f.Delete();

            return addrs;
        }

        private static void ReadFile(FileInfo f, Dictionary<string, IPaddr> addrs, string sourceFile)
        {
            string fPath = f.FullName;
            foreach (string line in File.ReadLines(fPath))
            {
                if (line.Contains("due to '504 5.7.4 Unrecognized authentication type'"))
                {
                    string ip = line.Split(',')[5].Split(':')[0];
                    if (addrs.ContainsKey(ip))
                    {
                        addrs[ip].Registrer();
                    }
                    else
                    {
                        IPaddr oIp = new IPaddr(ip);
                        oIp.sourceFile = sourceFile;
                        addrs.Add(ip, oIp);
                    }
                }
            }
        }
    }
}
