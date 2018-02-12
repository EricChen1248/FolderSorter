using System;

namespace FolderSorter.Classes.DataClasses
{
    public class MoveFileData
    {
        public int _id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set;  }
        public string Destination { get; set;  }
        public string Time { get; }

        public MoveFileData()
        {
            
        }
        public MoveFileData(string fileName, string fileType, string destination)
        {
            FileName = fileName;
            FileType = fileType;
            Destination = destination;
            Time = DateTime.UtcNow.ToLongTimeString();
        }

        public bool Equals(MoveFileData other)
        {
            return other.FileName == FileName && other.Destination == Destination && other.Time == Time && other.FileType == FileType;
        }
    }
}
