using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using XNB2CP.ArgumentParsing.Exceptions;
using System.IO;
using XNB2CP.ArgumentParsing;

namespace XNB2CP.Tests
{
    [TestFixture]
    public class ArgumentParserTests : DirectoryTests
    {
        [SetUp]
        public override void Init()
        {
            base.Init();
            EnterSubDirectory("ArgumentParserTests");
        }

        [Test]
        public void TestNotEnoughArguments()
        {
            Assert.Throws<MissingArgumentException>(() => ArgumentParser.Parse(new string[] { }));
            Assert.Throws<MissingArgumentException>(() => ArgumentParser.Parse(new string[] { "test" }));
        }

        [Test]
        public void TestInvalidPathEmpty()
        {
            Assert.Throws<InvalidPathException>(() => ArgumentParser.Parse(new string[] { "", "TestMod" }));
            Assert.Throws<InvalidPathException>(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "" }));
        }

        [Test]
        public void TestInvalidPathNonExistant()
        {
            Assert.Throws<InvalidPathException>(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "bla" }));
            Assert.Throws<InvalidPathException>(() => ArgumentParser.Parse(new string[] { "bla", "TestMod" }));
        }

        [Test]
        public void TestSamePaths()
        {
            Assert.Throws<InvalidPathException>(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "Stardew Valley" }));
        }

        [Test]
        public void TestValidPaths()
        {
            Assert.DoesNotThrow(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod" }));
        }


        [Test]
        public void TestMissingOptionLength()
        {
            Assert.Throws<MissingOptionException>(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "--author" }));
            Assert.Throws<MissingOptionException>(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-a" }));
        }


        [Test]
        public void TestMissingOptionValue()
        {
            Assert.Throws<MissingOptionException>(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-a", "--name" }));
            Assert.Throws<MissingOptionException>(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "--name", "--name" }));
        }

        [Test]
        public void TestInvalidOption()
        {
            Assert.Throws<InvalidOptionException>(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-b" }));
            Assert.Throws<InvalidOptionException>(() => ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "--bob" }));
        }

        [Test]
        public void TestSetOneOptionLong()
        {

            ParsedArguments args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "--author", "Author Name" });
            Assert.AreEqual("Author Name", args.AuthorName);

            args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "--name", "Mod Name" });
            Assert.AreEqual("Mod Name", args.ModName);

            args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "--version", "1.0.0" });
            Assert.AreEqual("1.0.0", args.ModVersion);

            args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "--description", "Mod Description" });
            Assert.AreEqual("Mod Description", args.ModDescription);

            args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "--update", "Nexus:1234" });
            Assert.AreEqual(1, args.ModUpdateKeys.Count());
            Assert.AreEqual("Nexus:1234", args.ModUpdateKeys.ElementAt(0));
        }

        [Test]
        public void TestSetOneOptionShort()
        {

            ParsedArguments args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-a", "Author Name" });
            Assert.AreEqual("Author Name", args.AuthorName);

            args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-n", "Mod Name" });
            Assert.AreEqual("Mod Name", args.ModName);

            args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-v", "1.0.0" });
            Assert.AreEqual("1.0.0", args.ModVersion);

            args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-d", "Mod Description" });
            Assert.AreEqual("Mod Description", args.ModDescription);

            args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-u", "Nexus:1234" });
            Assert.AreEqual(1, args.ModUpdateKeys.Count());
            Assert.AreEqual("Nexus:1234", args.ModUpdateKeys.ElementAt(0));
        }

        [Test]
        public void TestOptionsDontAffectEachOther()
        {
            ParsedArguments args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-a", "John Doe" });
            Assert.AreEqual("John Doe", args.AuthorName);
            Assert.AreEqual("", args.ModName);
            Assert.AreEqual("1.0.0", args.ModVersion);
            Assert.AreEqual("", args.ModDescription);
            Assert.AreEqual("", args.ModUpdateKeys);
        }

        [Test]
        public void TestMultipleUpdateKeys()
        {
            ParsedArguments args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-u", "Nexus:1234,Nexus:4321" });
            Assert.AreEqual(2, args.ModUpdateKeys.Count());
            Assert.AreEqual("Nexus:1234", args.ModUpdateKeys.ElementAt(0));
            Assert.AreEqual("Nexus:4321", args.ModUpdateKeys.ElementAt(1));
        }

        [Test]
        public void TestNoUpdateKeys()
        {
            ParsedArguments args = ArgumentParser.Parse(new string[] { "Stardew Valley", "TestMod", "-u", "" });
            Assert.AreEqual(1, args.ModUpdateKeys.Count());
            Assert.AreEqual("", args.ModUpdateKeys.ElementAt(0));
        }

        [Test]
        public void TestSetMultipleOptions()
        {
            ParsedArguments args = ArgumentParser.Parse(new string[] {
                "Stardew Valley", "TestMod",
                "-a", "John Doe",
                "-n", "Test Mod"});
            Assert.AreEqual("John Doe", args.AuthorName);
            Assert.AreEqual("Test Mod", args.ModName);

            args = ArgumentParser.Parse(new string[] {
                "Stardew Valley", "TestMod",
                "-v", "-10",
                "--description", "This mod is super cool."});
            Assert.AreEqual("-10", args.ModVersion);
            Assert.AreEqual("This mod is super cool.", args.ModDescription);
        }

    }
}
