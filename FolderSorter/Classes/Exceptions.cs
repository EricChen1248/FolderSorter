using System;

namespace FolderSorter.Classes
{
    internal class InvalidLocationException : Exception
    {
        internal InvalidLocationException(string location) : base(location)
        {
            
        }
    }
}
