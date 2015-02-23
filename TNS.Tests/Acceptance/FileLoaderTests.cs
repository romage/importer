using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Services;
using Xunit;

namespace TNS.Importer.Tests.Acceptance
{
    [Trait("FileLoader", "File exists, and is correctly formatted")]
    public class FileLoaderTests
    {
        FileLoader _fileLoader;
        public FileLoaderTests()
        {
            this._fileLoader = new FileLoader();
        }

        [Fact(DisplayName = "Check the test is working")]
        public void checkTestIsWorking()
        {
            Assert.True(true);
        }


        [Fact(DisplayName = "FileLoader thows not found exception if the file doesn't exist")]
        public void FileLoaderThrowsANotFoundExceptionIfFilesDoesNotExist()
        {

            string physicalPath = "blaDiBlaFileDoesntexist.xlsx";
            Assert.Throws<FileNotFoundException>(() => _fileLoader.LoadFile(physicalPath));
        }

        [Fact(DisplayName = "FileLoader throws an ArgumentNullException if no file name passed in")]
        public void FileLoaderThrowsArgumentNullExceptionIfNoFileNamePassedThrough()
        {
            string virtualPath = "";
            Assert.Throws<ArgumentNullException>(() => _fileLoader.LoadFile(virtualPath));
        }

        [Fact(DisplayName = "FileLoader throws an Argument Exception if wrong file extension passed in")]
        public void FileLoaderThrowsArgumentExceptionIfWrongFileTypePassedThrough()
        {

            string virtualPath = "fred.doc";
            Assert.Throws<ArgumentException>(() => _fileLoader.LoadFile(virtualPath));
        }

    }
}
