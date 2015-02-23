using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Interfaces;
using TNS.Importer.Models;
using TNS.Importer.Data;
using System.IO;

namespace TNS.Importer.Services
{
    public class HomeService: IHomeService
    {
        IHomeRepository _repo;
        IScoreParser _parser;
       
        //TODO: add ioc container, and add the interface to the constructor
        public HomeService(IHomeRepository repo, IScoreParser parser)
        {
           this._repo = repo;
           this._parser = parser;
        }

        public Product ProcessUploadedFile(Product product, string uploadRootPhysicalPath)
        {
            product.DateOfScoreInput = DateTime.Today;
            product.ProcessState = ProcessStateEnum.Processing;

            //TODO: Move to ioc container, and constructor

            try
            {
                //ExcelParserViaDomService parser = new ExcelParserViaDomService(fl);
                Product alt = _parser.Parse(product, uploadRootPhysicalPath);
                product.ProcessState = ProcessStateEnum.FinishedProcessing;
                product.CurrentProcessingFolder = ConfigHelper.ProcessedPath;
                _repo.SaveProduct(product);
            }
            catch (Exception ex)
            {
                moveFileToError(product, uploadRootPhysicalPath);
                //logError();
                throw ex;
            }
            return product;
        }

        private void moveFileToError(Product product, string uploadRootPhysicalPath)
        {
            string existingPhysicalPath = FileHelper.GetPhysicalFilePath(uploadRootPhysicalPath, product);
            string newLocation = existingPhysicalPath.Replace(ConfigHelper.ToBeProcessedPath, ConfigHelper.UnableToProcessPath);
            // should the product be updated... it may have failed at the save which resulted in this code path. 
            FileInfo fi = new FileInfo(existingPhysicalPath);
            fi.MoveTo(newLocation);
        }


        public Product SaveProduct(Product product)
        {
            return _repo.SaveProduct(product);
        }
    }
}
