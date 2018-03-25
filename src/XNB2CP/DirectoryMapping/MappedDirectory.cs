using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.DirectoryMapping
{
    internal class MappedDirectory
    {
        public String RootFromDirectory { get; set; } = null;
        public String RootToDirectory { get; set; } = null;
        public IList<MappedFile> MappedFiles { get; set; } = new List<MappedFile>();
        
        public MappedDirectory(string from, string to)
        {
            RootFromDirectory = from;
            RootToDirectory = to;
        }
    }
}
