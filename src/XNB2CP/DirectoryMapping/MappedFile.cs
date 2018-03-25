using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.DirectoryMapping
{
    internal class MappedFile
    {
        public string RelativeFilePath { get; set; }
        public string RelativeMappedPath { get; set; }

        public MappedFile(string filePath, string mappedPath)
        {
            this.RelativeFilePath = filePath;
            this.RelativeMappedPath = mappedPath;
        }
    }
}
