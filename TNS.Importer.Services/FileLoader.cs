using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Importer.Services
{
    public class FileLoader
    {
        public FileInfo checkForFileErrorsFile(string physicalPath)
        {
            if (string.IsNullOrWhiteSpace(physicalPath))
            {
                throw new ArgumentNullException("Load file requires a filename");
            }

            FileInfo fi = new FileInfo(physicalPath);
            if (!fi.Extension.Equals(".xlsx", StringComparison.InvariantCultureIgnoreCase) && !fi.Extension.Equals(".xls", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("this requires a xlsx file. This is excel file ");
            }

            if (!File.Exists(physicalPath))
            {
                throw new FileNotFoundException("This file doesn't exist");
            }

            return fi;
        }

        public FileStream LoadFile(string physicalPath)
        {
            FileInfo fi = checkForFileErrorsFile(physicalPath);
            FileStream fs = new FileStream(physicalPath, FileMode.Open);
            return fs;
        }

    }
}
