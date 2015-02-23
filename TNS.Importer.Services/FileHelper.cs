using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Models;
using System.Web;


namespace TNS.Importer.Services
{
    public class FileHelper
    {
        
        public  static string GetPhysicalFilePath(Product product)
        {
            var root = ConfigHelper.UploadFileRoot;
            var folder = product.CurrentProcessingFolder;
            var fileName = product.SystemFileNameWithExtension;
            string combined = System.IO.Path.Combine(root, folder, fileName);

            // how to access the mappath function across the various layers. 
            // don't really want to store the full physical path - the virtual one makes more sense. 
            return "";
        }
    }
}
