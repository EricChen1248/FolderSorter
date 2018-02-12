using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace FolderSorter.Classes
{
    public class Rule
    {
        public int _id { get; set; }
        public string Name { get; set; }
        public string DestinationFolder { get; set; }
        public List<string> Data { get; set; }
        public int Order { get; set; }
        public RuleType Type;

        public int type
        {
            get => (int) Type;
            set => Type = (RuleType) Enum.ToObject(typeof(RuleType), value);
        }

        public bool FallBack { get; set; }

        private HashSet<string> _extensionList;

        public Rule()
        {
            
        }
        
        public Rule(string name, string destinationFolder, List<string> data, RuleType type)
        {
            Name = name;
            DestinationFolder = destinationFolder;
            Type = type;
            UpdateData(data);
        }

        internal void UpdateData(List<string> data)
        {
            Data = data;
            switch (Type)
            {
                case RuleType.FileNameContains:
                    break;
                case RuleType.ExtensionType:
                    _extensionList = new HashSet<string>();
                    foreach (var extension in data)
                    {
                        _extensionList.Add(extension);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static Rule GenerateFallBack(string destinationFolder)
        {
            return new Rule("Other", destinationFolder, new List<string>(), RuleType.ExtensionType) {FallBack = true};
        }
        

        internal bool FitsRule(string file)
        {
            if (FallBack)
            {
                return true;
            }
            switch (Type)
            {
                case RuleType.FileNameContains:
                    var fileName = Path.GetFileNameWithoutExtension(file)?.ToLower();
                    if (fileName == null) return false;
                    
                    foreach (var data in Data)
                    {
                        if (fileName.Contains(data))
                            return true;
                    }
                    return false;
                case RuleType.ExtensionType:
                    var extension = Path.GetExtension(file)?.ToLower();
                    
                    if (_extensionList == null)
                    {
                        _extensionList = new HashSet<string>();
                        foreach (var ext in Data)
                        {
                            _extensionList.Add(ext);
                        }
                    }
                    if (extension != null && _extensionList.Contains(extension))
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        internal void ChangeName(string newName)
        {
            Name = newName;
        }

        internal void MoveLocation(string location)
        {
            DestinationFolder = location;
        }

    }

    public enum RuleType
    {
        [Description("File Name Contains:")]
        FileNameContains,
        [Description("Extension Is:")]
        ExtensionType,
    }
}
