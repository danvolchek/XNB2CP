using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.ArgumentParsing;
using XNB2CP.DirectoryMapping;

namespace XNB2CP.ContentGeneration
{
    internal class ContentPack
    {
        public string RootDirectory { get; set; }

        public ContentPack(string rootDirectory)
        {
            this.RootDirectory = rootDirectory;
        }

        public void GenerateManifest(ParsedArguments args)
        {
            JObject o = JObject.FromObject(new
            {
                Name = args.ModName,
                Author = args.AuthorName,
                Version = args.ModVersion,
                Description = args.ModDescription,
                UniqueID = $"{args.AuthorName}.{args.ModName}",
                MinimumAPIVersion = "2.0",
                UpdateKeys =
                    from item in args.ModUpdateKeys
                    select item,
                ContentPackFor = new
                {
                    UniqueID = "Pathoschild.ContentPatcher",
                    MinimumVersion = "1.2"
                }
            });

            File.WriteAllText(Path.Combine(RootDirectory, "manifest.json"), JsonConvert.SerializeObject(o, Formatting.Indented));
        }
    }
}
