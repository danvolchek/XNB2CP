using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.DirectoryMapping.Exceptions;
using XNB2CP.DiffGenerators.Differences;
using XNB2CP.DiffGenerators.Exceptions;
using XNB2CP.DiffGenerators.Generators;
using static XNB2CP.DiffGenerators.Differences.Difference;

namespace XNB2CP.DiffGenerators
{
    internal class DifferenceGenerator
    {
        public static Difference GetDifferences(string absoluteFileA, string absoluteFileB)
        {
            JObject aInfo = JObject.Parse(File.ReadAllText(absoluteFileA));
            JObject bInfo = JObject.Parse(File.ReadAllText(absoluteFileB));

            DifferenceType aType = GetDifferenceType(aInfo);
            DifferenceType bType = GetDifferenceType(bInfo);

            if (aType != bType)
                throw new InvalidDifferenceException($"{absoluteFileA} and {absoluteFileB} have different underlying types: {aType} and {bType}");

            Console.WriteLine(aType);
            switch (aType)
            {
                case DifferenceType.Dictionary:
                    return DictionaryDifferenceGenerator.GetDifference(aInfo, bInfo);
                case DifferenceType.Image:
                    return ImageDifferenceGenerator.GetDifference(absoluteFileA, absoluteFileB);
                default:
                    return null;
            }
        }

        //Ultra fragileTM
        private static DifferenceType GetDifferenceType(JObject info)
        {
            JToken readers = info["readers"];
            if (readers == null)
                throw new InvalidXNBException("Unable to determine Difference Type");
            if (!(readers is JArray))
                throw new InvalidXNBException("Unable to determine Difference Type");

            JArray readerArray = readers as JArray;

            foreach (JToken item in readers)
            {
                JObject reader = item as JObject;

                string readerType = reader["type"].ToString();
                if (readerType.Contains("DictionaryReader"))
                {
                    return DifferenceType.Dictionary;
                }
                else if (readerType.Contains("Texture2DReader") && info["content"]["export"].ToString().EndsWith(".png"))   
                    return DifferenceType.Image;


            }

            throw new InvalidXNBException("Unsupported Type.");
        }
    }
}
