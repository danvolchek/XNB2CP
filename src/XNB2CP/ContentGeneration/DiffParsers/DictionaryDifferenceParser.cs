using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.DiffGenerators.Differences;
using XNB2CP.DirectoryMapping;

namespace XNB2CP.ContentGeneration.DiffParsers
{
    internal class DictionaryDifferenceParser
    {
        public static JArray Parse(MappedFile file, DictionaryDifference dictDiff)
        {
            JObject newToken = new JObject
            {
                ["Action"] = "EditData",
                ["Target"] = file.RelativeMappedPath
            };

            JObject entries = new JObject();
            foreach (KeyValuePair<string, string> changes in dictDiff.Differences)
            {
                entries[changes.Key] = changes.Value;
            }
            newToken["Entries"] = entries;

            return new JArray(newToken);
        }
    }
}
