using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.DirectoryMapping;
using XNB2CP.DiffGenerators;
using XNB2CP.DiffGenerators.Differences;
using Newtonsoft.Json.Linq;
using XNB2CP.ContentGeneration.DiffParsers;

namespace XNB2CP.ContentGeneration
{
    internal class ContentGenerator
    {
        private static string xnbcliDir;
        private static string packedDir;
        private static string unpackedDir;
        private static string executableExtension = "";

        private static void ResetIntermediateFolders()
        {
            if (Directory.Exists(packedDir))
                Directory.Delete(packedDir, true);
            if (Directory.Exists(unpackedDir))
                Directory.Delete(unpackedDir, true);

            Directory.CreateDirectory(packedDir);
            Directory.CreateDirectory(unpackedDir);
        }

        public static void GenerateContent(string outputDirectory, MappedDirectory mapping)
        {
            GetTempDirs();
            ResetIntermediateFolders();

            UnpackModFiles(mapping);

            JObject contentFile = ParseUnpackedFiles(mapping);
            Directory.CreateDirectory(outputDirectory);
            File.WriteAllText(Path.Combine(outputDirectory, "content.json"), contentFile.ToString());
        }

        private static void GetTempDirs()
        {
            xnbcliDir = Path.Combine(Directory.GetCurrentDirectory(), "xnbcli");

            //https://github.com/Pathoschild/SMAPI/blob/develop/src/SMAPI.Installer/InteractiveInstaller.cs#L384
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                    xnbcliDir = Path.Combine(xnbcliDir, "mac");
                    break;
                case PlatformID.Unix:
                    xnbcliDir = Path.Combine(xnbcliDir, "linux");
                    break;
                default:
                    xnbcliDir = Path.Combine(xnbcliDir, "windows");
                    executableExtension = ".exe";
                    break;
            }
            packedDir = Path.Combine(xnbcliDir, "packed");
            unpackedDir = Path.Combine(xnbcliDir, "unpacked");
        }

        private static void UnpackModFiles(MappedDirectory mapping)
        {
            foreach (MappedFile file in mapping.MappedFiles)
            {
                string newModPath = RewriteFileName(Path.Combine(packedDir, file.RelativeFilePath), "mod_");
                string newGamePath = RewriteFileName(Path.Combine(packedDir, file.RelativeMappedPath), "game_");

                Directory.CreateDirectory(Path.GetDirectoryName(newModPath));
                Directory.CreateDirectory(Path.GetDirectoryName(newGamePath));
                File.Copy(Path.Combine(mapping.RootFromDirectory, file.RelativeFilePath), newModPath);
                File.Copy(Path.Combine(mapping.RootToDirectory, file.RelativeMappedPath), newGamePath);
            }

            string executablePath = Path.Combine(xnbcliDir, $"xnbcli{executableExtension}");
            string args = $"unpack \"{packedDir}\" \"{unpackedDir}\"";
            //Console.WriteLine($"\"{executablePath}\" {args}");

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Arguments = args
                }
            };
            process.Start();
            process.WaitForExit();
        }

        private static JObject ParseUnpackedFiles(MappedDirectory mapping)
        {
            JArray changes = new JArray();
            JObject contentFile = new JObject
            {
                ["Format"] = "1.0",
                ["Changes"] = changes
            };
            
            foreach (MappedFile file in mapping.MappedFiles)
            {
                string newModPath = RewriteFileName(Path.Combine(unpackedDir, file.RelativeFilePath), "mod_");
                string newGamePath = RewriteFileName(Path.Combine(unpackedDir, file.RelativeMappedPath), "game_");

                newModPath = Path.ChangeExtension(newModPath, "json");
                newGamePath = Path.ChangeExtension(newGamePath, "json");

                try
                {
                    Difference d = DifferenceGenerator.GetDifferences(newModPath, newGamePath);
                    if (d is DictionaryDifference dictDiff)
                    {
                        changes.Merge(DictionaryDifferenceParser.Parse(file, dictDiff));
                    } else if (d is ImageDifference imageDiff)
                    {
                        changes.Merge(ImageDifferenceParser.Parse(file, imageDiff));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    //TODO: REMOVE THIS
                }
            }

            return contentFile;

        }

        private static string RewriteFileName(string fullPath, string newPrefix)
        {
            DirectoryInfo parent = Directory.GetParent(fullPath);
            return Path.Combine(parent.FullName, newPrefix + Path.GetFileName(fullPath));
        }
    }
}
