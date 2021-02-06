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

            //FileInfo f = directory.GetFiles().OrderBy(sf => sf.Name).Last();

            FileInfo[] files = directory.GetFiles().OrderByDescending(sf => sf.Name).ToArray();

            if (Program.debug())
            {
                if (files[0].Length == 0)
                    Logger.getINSTANCE().logZero();
                Logger.getINSTANCE().logFName(files[0].Name);
            }


            string sourceFile = files[0].Name;
            string logLoc = ConfigurationManager.AppSettings.Get("runLocation") + "tmplogs";
            if (Directory.Exists(logLoc))
            {
                foreach (var fName in Directory.GetFiles(logLoc))
                {
                    File.Delete(fName);
                }
            }
            else
                Directory.CreateDirectory(logLoc);

            List<FileInfo> fileInfos = new List<FileInfo>();
            for (int i = 0; i < Math.Min(files.Length, settings.FileCount); i++)
            {
                fileInfos.Add(files[i].CopyTo(logLoc + "\\" + files[i].Name));
                if (Program.debug())
                    Logger.getINSTANCE().logCustom("Read file " + files[i].Name);
            }

            ReadFile(fileInfos, sourceFile, settings);

            foreach (var fName in Directory.GetFiles(logLoc))
            {
                File.Delete(fName);
            }
            Directory.Delete(logLoc);
        }

        private static void ReadFile(List<FileInfo> files, string sourceFile, FileSettingElement settings)
        {
            settings.SourceFile = sourceFile;
            foreach (FileInfo file in files)
            {
                foreach (string line in File.ReadLines(file.FullName))
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
}
