using FolderSorter.Classes.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;

namespace FolderSorter.Classes
{
    internal static class FileSorter
    {
        internal static List<Rule> rules = new List<Rule>();
        private static Rule FallBackRule;

         static FileSorter()
        {
            // TODO:Load in rules from database
            var data = new List<string> {".ai"};
            rules.Add(new Rule("Images", @"D:\Users\Eric\Downloads\Images", data, RuleType.ExtensionType));
            data = new List<string> {".zip", ".rar", ".7z", ".tar.gz"};
            rules.Add(new Rule("Compressed", @"D:\Users\Eric\Downloads\Compressed", data, RuleType.ExtensionType));
            FallBackRule = Rule.GenerateFallBack(@"D:\Users\Eric\Downloads\Other");
        }
        
        public static void RegenRules()
        {
            throw new NotImplementedException();
        }

        public static Rule FindRule(string file)
        {
            foreach (var rule in rules)
            {
                if (rule.FitsRule(file))
                {
                    return rule;
                }
            }
            return FallBackRule;
        }

        public static void SortFile(FileInfo file)
        {
            try
            {
                if (file.Exists == false)
                {
                    return;
                }
                var rule = FindRule(file.Name);
                var fileName = GenerateFileName(Path.GetFileName(file.Name));
                File.Move(file.FullName, Path.Combine(rule.DestinationFolder, fileName ?? throw new InvalidOperationException()));

                var data = new MoveFileData(file.Name, rule.Name, rule.DestinationFolder, DateTime.UtcNow);
                MainWindow.Instance.AddLog(data);
                // TODO: Add notification and log
            }
            catch (Exception)
            {
                // TODO: Add error handling for file moving
            }
        }

        private static string GenerateFileName(string fullFilePath)
        {
            var count = 0;
            var fileName = Path.GetFileNameWithoutExtension(fullFilePath);
            var extension = Path.GetExtension(fullFilePath);
            var returnFile = fullFilePath;
            while (File.Exists(fullFilePath))
            {
                returnFile = $"{fileName}({count++})+{extension}";
            }

            return returnFile;
        }
    }
}
