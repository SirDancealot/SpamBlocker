﻿using SpamBlocker.program.data.FileSetting;
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
                    IPManager.GetInstance().Registrer(ip, settings);
                }
            }
        }
    }
}
