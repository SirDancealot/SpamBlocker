using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SpamBlocker
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
            FileInfo f = directory.GetFiles()[0];
            foreach (var fInfo in directory.GetFiles())
            {
                if (fInfo.LastWriteTime.CompareTo(f.LastWriteTime) > 0)
                    f = fInfo;
            }

            if (f.Length == 0 && Program.debug())
                Logger.getINSTANCE().logZero();

            f = f.CopyTo(ConfigurationManager.AppSettings.Get("RunLocation") + "tmp.LOG");

            ReadFile(f.FullName, addrs);

            f.Delete();

            return addrs;
        }

        private static void ReadFile(string fpath, Dictionary<string, IPaddr> addrs)
        {
            foreach (string line in File.ReadLines(fpath))
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
                        addrs.Add(ip, new IPaddr(ip));
                    }
                }
            }
        }
    }
}
