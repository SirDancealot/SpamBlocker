using SpamBlocker.program.data.FileSetting;
using SpamBlocker.program.ui;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

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

            FileInfo[] files = directory.GetFiles().OrderByDescending(sf => sf.Name).ToArray();

            if (Program.Debug())
            {
                if (files[0].Length == 0)
                    Logger.GetINSTANCE().LogZero();
                Logger.GetINSTANCE().LogFName(files[0].Name);
            }

            string sourceFile = files[0].Name;

            //Make sure a temporary location for the log-files to be copied to exists and that it is empty
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

            //Coppy log-files to temporary location
            List<FileInfo> fileInfos = new List<FileInfo>();
            for (int i = 0; i < Math.Min(files.Length, settings.FileCount); i++)
            {
                fileInfos.Add(files[i].CopyTo(logLoc + "\\" + files[i].Name));
                if (Program.DebugFull())
                    Logger.GetINSTANCE().LogCustom("Read file " + files[i].Name);
            }

            ReadFile(fileInfos, sourceFile, settings);

            //Cleanup temporary files
            foreach (var fName in Directory.GetFiles(logLoc))
            {
                File.Delete(fName);
            }
            Directory.Delete(logLoc);
        }

        private static void ReadFile(List<FileInfo> files, string sourceFile, FileSettingElement settings)
        {
            settings.SourceFile = sourceFile;
            string searchType = settings.DoSearch;
            bool search = !searchType.Equals("off");
            string[] searchTerms = (!search ? null : settings.SearchPattern.Split(new string[] { "\\n" }, StringSplitOptions.None));
            foreach (FileInfo file in files)
            {
                foreach (string line in File.ReadLines(file.FullName))
                {
                    bool matches = true;
                    if (line.StartsWith(settings.CommentStart) && settings.CommentStart != "")
                        continue;

                    if (search)
                    {
                        switch (searchType)
                        {
                            case "none":
                                foreach (var term in searchTerms)
                                {
                                    if (line.Contains(term))
                                    {
                                        matches = false;
                                        break;
                                    }
                                }
                                break;
                            case "any":
                                matches = false;
                                foreach (var term in searchTerms)
                                {
                                    if (line.Contains(term))
                                    {
                                        matches = true;
                                        break;
                                    }
                                }
                                break;
                            case "all":
                                foreach (var term in searchTerms)
                                {
                                    if (!line.Contains(term))
                                    {
                                        matches = false;
                                        break;
                                    }
                                }
                                break;
                            default:
                                Logger.GetINSTANCE().LogError("invalid value of doSearch parameter in config, use 'off'/'none'/'any'/'all'");
                                break;
                        }
                    }
                    if (!matches)
                        continue;

                    string ip = line.Split(settings.Delim)[settings.IpIndex];
                    if (ip.Contains(':'))
                        ip = ip.Split(':')[0];
                    IPManager.GetInstance().Registrer(ip, settings);
                }
            }
        }
    }
}
