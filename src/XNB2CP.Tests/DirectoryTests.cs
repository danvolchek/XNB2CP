using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNB2CP.Tests
{
    [TestFixture]
    public class DirectoryTests
    {
        protected string currentDirectory;

        [SetUp]
        public virtual void Init()
        {
            currentDirectory = TestContext.CurrentContext.TestDirectory;
            EnterSubDirectory("TestDirectories");

        }

        protected void EnterSubDirectory(string dir)
        {
            currentDirectory = Path.Combine(currentDirectory, dir);
            Directory.SetCurrentDirectory(currentDirectory);
        }
    }
}
