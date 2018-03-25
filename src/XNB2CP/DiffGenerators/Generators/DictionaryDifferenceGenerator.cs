using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.DiffGenerators.Differences;

namespace XNB2CP.DiffGenerators.Generators
{
    internal class DictionaryDifferenceGenerator
    {
        public static DictionaryDifference GetDifference(JObject needle, JObject haystack)
        {
            IDictionary<string, string> diffs = new Dictionary<string, string>();

            JObject haystackContent = haystack["content"] as JObject;
            JObject needleContent = needle["content"] as JObject;

            foreach (KeyValuePair<string,JToken> entry in needleContent)
            {
                if(!haystackContent.ContainsKey(entry.Key) || haystackContent[entry.Key].ToString() != entry.Value.ToString())
                {
                    diffs[entry.Key] = entry.Value.ToObject<string>();
                }
            }

            return new DictionaryDifference(diffs);

        }
    }
}
