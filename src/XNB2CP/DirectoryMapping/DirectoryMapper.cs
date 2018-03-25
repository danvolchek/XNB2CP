using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.DirectoryMapping.Exceptions;

namespace XNB2CP.DirectoryMapping
{
    internal class DirectoryMapper
    {
        public static MappedDirectory TryMapDirectories(string SDVContentPath, string modPath, out IDictionary<String, String> errors)
        {
            MappedDirectory dir;
            errors = new Dictionary<String, String>();
            dir = TryMapContentDirectories(SDVContentPath, modPath, errors);

            if (dir != null)
            {
                return MakeMappedFilesRelative(dir);
            }

            dir = TryMapIndividualFiles(SDVContentPath, modPath, errors);
            return MakeMappedFilesRelative(dir);
        }

        private static MappedDirectory MakeMappedFilesRelative(MappedDirectory m)
        {
            if (m == null)
                return null;

            if (m.RootFromDirectory.ElementAt(m.RootFromDirectory.Length - 1) != Path.DirectorySeparatorChar)
                m.RootFromDirectory = $"{m.RootFromDirectory}{Path.DirectorySeparatorChar}";
            if (m.RootToDirectory.ElementAt(m.RootToDirectory.Length - 1) != Path.DirectorySeparatorChar)
                m.RootToDirectory = $"{m.RootToDirectory}{Path.DirectorySeparatorChar}";

            foreach (MappedFile file in m.MappedFiles)
            {
                if (!file.RelativeFilePath.Contains(m.RootFromDirectory))
                    throw new InvalidOperationException($"{file.RelativeFilePath} does not contain {m.RootFromDirectory}.");

                file.RelativeFilePath = file.RelativeFilePath.Replace(m.RootFromDirectory, "");

                if (!file.RelativeMappedPath.Contains(m.RootToDirectory))
                    throw new InvalidOperationException($"{file.RelativeMappedPath} does not contain {m.RootToDirectory}.");

                file.RelativeMappedPath = file.RelativeMappedPath.Replace(m.RootToDirectory, "");
            }

            return m;
        }

        private static MappedDirectory TryMapContentDirectories(string SDVContentPath, string modPath, IDictionary<String, String> errors)
        {
            string modContentPath = Path.Combine(modPath, "Content");

            bool hasOnlyContentFolder = Directory.Exists(modContentPath) && Directory.EnumerateDirectories(modPath).Count() == 1
                && Directory.EnumerateFiles(modPath, "*.xnb").Count() == 0;

            if (hasOnlyContentFolder)
            {
                try
                {
                    MappedDirectory dir = new MappedDirectory(modContentPath, SDVContentPath);
                    CheckDirectoryContainsOther(SDVContentPath, modContentPath, dir);
                    return dir;
                }
                catch (InvalidMappingException e)
                {
                    errors[modPath] = e.Message;
                }
            }
            else
            {

                errors[modPath] = $"{modPath} does not contain a Content folder.";

                /*foreach (string needlePath in Directory.EnumerateDirectories(modPath))
                {
                    TryMapContentDirectories(SDVPath, needlePath, mappings, errors);
                }*/
            }

            return null;

        }

        private static MappedDirectory TryMapIndividualFiles(string SDVContentPath, string modPath, IDictionary<String, String> errors)
        {
            MappedDirectory directory = new MappedDirectory(modPath, SDVContentPath);
            bool hasOnlyXNB = Directory.EnumerateDirectories(modPath).Count() == 0
                && Directory.EnumerateFiles(modPath, "*.xnb").Count() == Directory.EnumerateFiles(modPath).Count();

            if (hasOnlyXNB)
            {
                foreach (string needlePath in Directory.EnumerateFiles(modPath))
                {
                    string needleFileName = Path.GetFileName(needlePath);
                    //TOOD: Cache this once
                    IEnumerable<string> hayStackLocs = Directory.EnumerateFiles(SDVContentPath, needleFileName, SearchOption.AllDirectories);

                    if (hayStackLocs.Count() > 1)
                    {
                        errors[modPath] = $"{needleFileName} appears more than once in SDV's content folder.";
                        return null;
                    }
                    else if (hayStackLocs.Count() == 0)
                    {
                        errors[modPath] = $"{needleFileName} wasn't in SDV's content folder.";
                        return null;
                    }
                    else
                    {
                        directory.MappedFiles.Add(new MappedFile(needlePath, hayStackLocs.ElementAt(0)));
                    }
                }

                return directory;
            }
            else
            {
                errors[modPath] = $"{modPath} doesn't only contain XNB files.";
                return null;
            }

        }

        private static void CheckDirectoryContainsOther(string haystack, string needle, MappedDirectory mappedNeedle)
        {
            //Console.WriteLine($"Checking that {haystack} contains {needle} !");

            foreach (string needlePath in Directory.EnumerateFiles(needle))
            {
                //Console.WriteLine($"Checking file {needlePath} !");
                string haystackPath = Path.Combine(haystack, Path.GetFileName(needlePath));

                if (!File.Exists(haystackPath))
                {
                    throw new InvalidMappingException($"{haystack} does not contain {Path.GetFileName(needlePath)}.");
                }

                mappedNeedle.MappedFiles.Add(new MappedFile(needlePath, haystackPath));
            }
            //Console.WriteLine("Files are OK !");

            foreach (string needlePath in Directory.EnumerateDirectories(needle))
            {

                // Console.WriteLine($"Combining {haystack} with {Path.GetFileName(needlePath)} to get:");
                string haystackPath = Path.Combine(haystack, Path.GetFileName(needlePath));
                // Console.WriteLine($"Checking that {haystackPath} exists!");

                if (!Directory.Exists(haystackPath))
                {
                    throw new InvalidMappingException($"{haystack} does not contain {Path.GetFileName(needlePath)}.");
                }

                CheckDirectoryContainsOther(haystackPath, needlePath, mappedNeedle);
            }

        }
    }
}
