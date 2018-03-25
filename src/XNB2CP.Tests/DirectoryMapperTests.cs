using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNB2CP.DirectoryMapping;

namespace XNB2CP.Tests
{
    [TestFixture]
    class DirectoryMapperTests : DirectoryTests
    {
        private string SDVContentPath;

        [SetUp]
        public override void Init()
        {
            base.Init();
            EnterSubDirectory("DirectoryMapperTests");
            SDVContentPath = Path.Combine(currentDirectory, "Stardew Valley", "Content");
        }

        [Test]
        public void TestDirectContent()
        {
            MappedDirectory dir = DirectoryMapper.TryMapDirectories(SDVContentPath, Path.Combine(currentDirectory, "TestDirectContent"), out IDictionary<string,string> errors);
            Assert.NotNull(dir);

            Assert.AreEqual(dir.MappedFiles.Count, 1);
            MappedFile file = dir.MappedFiles[0];
          
           
            Assert.AreEqual(Path.Combine(currentDirectory, "TestDirectContent", "Content", "test.xnb"), file.RelativeFilePath);
            Assert.AreEqual(Path.Combine(currentDirectory, "Stardew Valley", "Content", "test.xnb"), file.RelativeMappedPath);
        }

        [Test]
        public void TestIndirectContent()
        {
            MappedDirectory dir = DirectoryMapper.TryMapDirectories(SDVContentPath, Path.Combine(currentDirectory, "TestIndirectContent"), out IDictionary<string, string> errors);
            Assert.Null(dir);
        }

        [Test]
        public void TestDirectContentMissingFile()
        {
            MappedDirectory dir = DirectoryMapper.TryMapDirectories(SDVContentPath, Path.Combine(currentDirectory, "TestDirectContentMissingFile"), out IDictionary<string, string> errors);
            Assert.Null(dir);
        }

        [Test]
        public void TestDirectContentExtraFile()
        {
            MappedDirectory dir = DirectoryMapper.TryMapDirectories(SDVContentPath, Path.Combine(currentDirectory, "TestDirectContentExtraFile"), out IDictionary<string, string> errors);
            Assert.NotNull(dir);

            Assert.AreEqual(dir.MappedFiles.Count, 0);
        }

        [Test]
        public void TestSingleFileUniqueDirect()
        {
            string testFolderPath = Path.Combine(currentDirectory, "TestSingleFileUniqueDirect");
            MappedDirectory dir = DirectoryMapper.TryMapDirectories(SDVContentPath, testFolderPath, out IDictionary<string, string> errors);
            Assert.NotNull(dir);

            Assert.AreEqual(dir.MappedFiles.Count, 1);
            Assert.AreEqual(dir.MappedFiles[0].RelativeFilePath, Path.Combine(testFolderPath, "test.xnb"));
            Assert.AreEqual(dir.MappedFiles[0].RelativeMappedPath, Path.Combine(SDVContentPath, "test.xnb"));
        }

        [Test]
        public void TestSingleFileUniqueIndirect()
        {
            string testFolderPath = Path.Combine(currentDirectory, "TestSingleFileUniqueIndirect");
            MappedDirectory dir = DirectoryMapper.TryMapDirectories(SDVContentPath, testFolderPath, out IDictionary<string, string> errors);
            Assert.NotNull(dir);

            Assert.AreEqual(dir.MappedFiles.Count, 1);
            Assert.AreEqual(dir.MappedFiles[0].RelativeFilePath, Path.Combine(testFolderPath, "test3.xnb"));
            Assert.AreEqual(dir.MappedFiles[0].RelativeMappedPath, Path.Combine(SDVContentPath, "Portraits", "test3.xnb"));
        }

        [Test]
        public void TestSingleFileNotUnique()
        {
            string testFolderPath = Path.Combine(currentDirectory, "TestSingleFileNotUnique");
            MappedDirectory dir = DirectoryMapper.TryMapDirectories(SDVContentPath, testFolderPath, out IDictionary<string, string> errors);
            Assert.Null(dir);

        }
    }
}
