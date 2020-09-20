using SpamBlocker.program.data.FileSetting;
using SpamBlocker.program.data.IP;
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
        //private static Dictionary<string, IPaddr> addrs = new Dictionary<string, IPaddr>();
        public static void ReadFolder(FileSettingElement settings)
        {
            if (string.IsNullOrWhiteSpace(settings.ReadPath))
            {
                throw new ArgumentException("Folder Path illegal", nameof(settings.ReadPath));
            }

            var directory = new DirectoryInfo(settings.ReadPath);
            FileInfo f = directory.GetFiles().OrderBy(sf => sf.Name).Last();

            if (Program.debug())
            {
                if (f.Length == 0)
                    Logger.getINSTANCE().logZero();
                Logger.getINSTANCE().logFName(f.Name);
            }


            string sourceFile = f.Name;
            string logLoc = ConfigurationManager.AppSettings.Get("runLocation") + "tmp.LOG";
            if (File.Exists(logLoc))
                File.Delete(logLoc);
            f = f.CopyTo(logLoc);
            ReadFile(f, sourceFile, settings);
            f.Delete();
        }

        private static void ReadFile(FileInfo f, string sourceFile, FileSettingElement settings)
        {
            string fPath = f.FullName;
            settings.SourceFile = sourceFile;
            foreach (string line in File.ReadLines(fPath))
            {
                if (line.StartsWith(settings.CommentStart) && settings.CommentStart != "")
                    continue;
                if (settings.DoSearch)
                    if (!line.Contains(settings.SearchPattern))
                        continue;

                string ip = line.Split(settings.Delim)[settings.IpIndex];
                if (ip.Contains(':'))
                    ip = ip.Split(':')[0];
                IPManager.getInstance().Registrer(ip, settings);
            }
        }
    }
}
