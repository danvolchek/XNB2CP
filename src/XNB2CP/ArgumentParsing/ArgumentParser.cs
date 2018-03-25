using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using XNB2CP.ArgumentParsing;
using System.Collections.ObjectModel;
using XNB2CP.ArgumentParsing.Exceptions;

namespace XNB2CP.ArgumentParsing
{
    internal class ArgumentParser
    {
        //"SDV Dir" "Mod Dir" --author(a), --name(n), --version(v), --description(d), --link(l)
        private static readonly IList<string> allowedOptions = new ReadOnlyCollection<string>(new List<string>() { "--author", "-a", "--name", "-n", "--version", "-v", "--description", "-d", "--update", "-u" });

        public static ParsedArguments Parse(string[] args)
        {
            if (args.Length < 2)
            {
                throw new MissingArgumentException("Both the SDV Dir and Mod Dir must be provided.");
            }

            args[0] = ValidatePath(args[0]);
            args[1] = ValidatePath(args[1]);

            if (args[0] == args[1])
            {
                throw new InvalidPathException($"The SDV Dir can't be the same as the mod path.\nThe path for both was: \"{args[0]}\".");
            }

            ParsedArguments parsed = new ParsedArguments(args[0], args[1]);

            for (int i = 2; i < args.Length; i++)
            {
                if (!allowedOptions.Contains(args[i]))
                    throw new InvalidOptionException($"The option \"{args[i]}\" is not allowed.");
                else if (i == args.Length - 1 || allowedOptions.Contains(args[i + 1]))
                    throw new MissingOptionException($"The option \"{args[i]}\" was specified, but no value was provided for it.");
                
                switch (args[i])
                {
                    case "--author":
                    case "-a":
                        parsed.AuthorName = args[++i];
                        break;
                    case "--name":
                    case "-n":
                        parsed.ModName = args[++i];
                        break;
                    case "--version":
                    case "-v":
                        parsed.ModVersion = args[++i];
                        break;
                    case "--description":
                    case "-d":
                        parsed.ModDescription = args[++i];
                        break;
                    case "--update":
                    case "-u":
                        parsed.ModUpdateKeys = args[++i].Split(',');
                        break;
                }
            }

            return parsed;
        }

        private static string ValidatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new InvalidPathException($"The directory \"{path}\" does not exist.");
            }

            try
            {
                path = Path.GetFullPath(path);
            }
            catch (Exception e)
            {
                throw new InvalidPathException($"The path \"{path}\" is invalid.", e);
            }

          
            return path;
        }
    }
}
