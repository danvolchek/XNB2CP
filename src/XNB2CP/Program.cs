using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.ArgumentParsing;
using XNB2CP.ContentGeneration;
using XNB2CP.DirectoryMapping;

namespace XNB2CP
{
    class Program
    {
        static int Main(string[] args)
        {
            ParsedArguments parsedArgs = null;
            try
            {
                parsedArgs = ArgumentParser.Parse(args);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("An error occured when trying to parse command line arguments: ");
                Console.Error.WriteLine(e.ToString());
                return -1;
            }

            if (Path.GetFileName(parsedArgs.SDVContentRootDirectory) != "Content")
            {
                Console.Error.WriteLine($"\"{parsedArgs.SDVContentRootDirectory}\" doesn't look like a valid SDV Content Directory: ");
                Console.Error.WriteLine($"It doesn't point to a Content folder.");
                return -1;
            }

            MappedDirectory mapping = DirectoryMapper.TryMapDirectories(parsedArgs.SDVContentRootDirectory, parsedArgs.ModRootDirectory, out IDictionary<string, string> errors);

            if (mapping == null)
            {
                Console.Error.WriteLine($"An error occured when trying to map the mod to the game's Content folder: ");
                foreach (KeyValuePair<string, string> errorinfo in errors)
                {
                    Console.Error.WriteLine($"{errorinfo.Key} is invalid because: ${errorinfo.Value}");
                }
                return -1;
            }

            ContentPack p = new ContentPack(Path.Combine(Directory.GetCurrentDirectory(), "CP"));
            if (Directory.Exists(p.RootDirectory))
                Directory.Delete(p.RootDirectory, true);
            Directory.CreateDirectory(p.RootDirectory);
            Console.WriteLine(p.RootDirectory);

            p.GenerateManifest(parsedArgs);

            ContentGenerator.GenerateContent(p.RootDirectory, mapping);

            return 0;
        }
    }
}
