using FolderSorter.Classes.DataClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FolderSorter.Classes
{
    internal static class FileSorter
    {
        internal static List<Rule> rules = new List<Rule>();
        internal static Rule FallBackRule;
        
        
        public static void Init()
        {
            var rulesList = Server.GetRules();
            foreach (var rule in rulesList)
            {
                rules.Add(rule);
            }

            FallBackRule = Server.GetFallBack() ?? Rule.GenerateFallBack(Path.Combine(UserFolderAPI.GetPath(KnownFolder.Downloads), @"Other"));
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

        public static void AddRule(Rule rule)
        {
            rules.Add(rule);
            UpdateServer();
        }

        public static void DeleteRule(Rule rule)
        {
            rules.Remove(rule);
            UpdateServer();
        }
        
        public static void SortFile(FileInfo file)
        {
            Rule rule = null;
            try
            {
                if (file.Exists == false)
                {
                    return;
                }

                rule = FindRule(file.Name);
                var fileName = GenerateFileName(Path.GetFileName(file.Name));
                File.Move(file.FullName,
                    Path.Combine(rule.DestinationFolder, fileName ?? throw new InvalidOperationException()));

                var data = new MoveFileData(file.Name, rule.Name, rule.DestinationFolder);
                MainWindow.Instance.AddLog(data);
                Server.AddLog(data);
                MoveNotifier.Add(data);

                // TODO: Add notification
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(rule?.DestinationFolder ?? throw new InvalidOperationException());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                // TODO: Add error handling for file moving
                
            }
        }

        public static void UpdateRules()
        {
            Server.UpdateRule(rules, FallBackRule);
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

        private static void UpdateServer()
        {
            rules = rules.OrderBy(r => r.Order).ToList();
            Server.UpdateRule(rules, FallBackRule);
        }
    }
}
