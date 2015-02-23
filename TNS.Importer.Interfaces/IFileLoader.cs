using System;
namespace TNS.Importer.Interfaces
{
    public interface IFileLoader
    {
        System.IO.FileInfo checkForFileErrorsFile(string physicalPath);
        System.IO.FileStream LoadFile(string physicalPath);
    }
}
