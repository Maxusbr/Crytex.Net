using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crytex.Service.IService;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Test.FakeImplementations;
using Crytex.Service.Service;
using System.IO;
using Crytex.Model.Models;
using System.Linq;

namespace Crytex.Test
{
    [TestClass]
    public class FileServiceTests
    {
        private IFileService _fileService;
        private string _testDirPath;
        private FakeFileDescriptorRepo _descriptorRepo;

        [TestInitialize]
        public void Init()
        {
            var unitOfWork = new FakeUnitOfWork();
            var fileDescRepo = new FakeFileDescriptorRepo();
            this._descriptorRepo = fileDescRepo;
            this._fileService = new FileService(unitOfWork, fileDescRepo,null,null);

            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "fileServiceTestTmp");
            Directory.CreateDirectory(dirPath);
            this._testDirPath = dirPath;
        }

        [TestMethod]
        public void Test()
        {
            byte[] fileContents = new byte[]{1, 2,3,4,5,6};
            string fileName = "TestFileName.jpg";
            FileType fileType = FileType.Image;
            FileDescriptor resultDescriptor = null;
            using (var stream = new MemoryStream())
            {
                stream.Write(fileContents, 0, fileContents.Length);

                resultDescriptor = this._fileService.SaveFile(stream, fileName, this._testDirPath, fileType);
            }

            var storedDescriptor = this._descriptorRepo._descriptorsStorage.Single();
            Assert.AreEqual(storedDescriptor, resultDescriptor);
            Assert.IsTrue(Path.GetFileNameWithoutExtension(fileName) == resultDescriptor.Name);
            Assert.IsTrue(resultDescriptor.Type == fileType);
            var fileInfo = new FileInfo(Path.Combine(this._testDirPath, resultDescriptor.Path));
            Assert.IsTrue(fileInfo.Exists);
            Assert.IsTrue(fileInfo.Length == fileContents.Length);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Directory.Delete(this._testDirPath, true);
        }
    }
}
