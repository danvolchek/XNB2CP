using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.DiffGenerators.Differences;
using XNB2CP.DirectoryMapping;

namespace XNB2CP.ContentGeneration.DiffParsers
{
    internal class ImageDifferenceParser
    {
        public static JArray Parse(MappedFile file, ImageDifference imageDiff)
        {
            if (imageDiff.IsLoad)
            {
                return new JArray(new JObject
                {
                    ["Action"] = "Load",
                    ["Target"] = file.RelativeMappedPath,
                    ["FromFile"] = Path.Combine("assets", file.RelativeFilePath)
                });
            }
            JArray changes = new JArray();

            foreach (Area change in imageDiff.Differences)
            {
                JObject newToken = new JObject
                {
                    ["Action"] = "EditImage",
                    ["Target"] = file.RelativeMappedPath,
                    ["FromFile"] = Path.Combine("assets", file.RelativeFilePath)
                };

                JObject area = new JObject
                {
                    ["X"] = change.Left,
                    ["Y"] = change.Top,
                    ["Width"] = change.Width,
                    ["Height"] = change.Height
                };
                newToken["FromArea"] = area;
                newToken["ToArea"] = area;

                changes.Add(newToken);
            }

            return changes;
        }
    }
}
