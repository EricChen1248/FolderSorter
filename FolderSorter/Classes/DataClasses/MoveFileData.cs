using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSorter.Classes.DataClasses
{
    public class MoveFileData
    {
        public string FileName { get; }
        public string FileType { get; }
        public string Destination { get; }
        public DateTime Time { get; }

        public MoveFileData(string fileName, string fileType, string destination, DateTime time)
        {
            FileName = fileName;
            FileType = fileType;
            Destination = destination;
            Time = time;
        }
    }
}
