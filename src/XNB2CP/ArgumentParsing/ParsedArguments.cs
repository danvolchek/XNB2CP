using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.ArgumentParsing
{
    internal class ParsedArguments
    {
        public string SDVContentRootDirectory { get; set; }
        public string ModRootDirectory { get; set; }
        public string OutputDirectory { get; set; } = Directory.GetCurrentDirectory();

        public string AuthorName { get; set; } = "";
        public string ModName { get; set; }  = "";
        public string ModVersion { get; set; } = "1.0.0";
        public string ModDescription { get; set; } = "";
        public IEnumerable<string> ModUpdateKeys { get; set; } = new List<string>();

        public ParsedArguments(string sDVContentRootDirectory, string modRootDirectory)
        {
            SDVContentRootDirectory = sDVContentRootDirectory;
            ModRootDirectory = modRootDirectory;
        }
    }
}
