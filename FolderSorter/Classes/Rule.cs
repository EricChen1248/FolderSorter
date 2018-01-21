using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace FolderSorter.Classes
{
    internal class Rule
    {
        internal string Name;
        internal string DestinationFolder;
        internal List<string> Data;
        internal RuleType Type;
        internal bool FallBack;

        private HashSet<string> _extensionList;

        internal Rule(string name, string destinationFolder, List<string> data, RuleType type)
        {
            Name = name;
            DestinationFolder = destinationFolder;
            Type = type;
            switch (type)
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
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        internal static Rule GenerateFallBack(string destinationFolder)
        {
            return new Rule("Fall Back", destinationFolder, null, RuleType.ExtensionType) {FallBack = true};
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
                    if (extension != null && _extensionList.Contains(extension))
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

    }

    internal enum RuleType
    {
        [Description("File Name Contains:")]
        FileNameContains,
        [Description("Extension Is:")]
        ExtensionType,
    }
}
