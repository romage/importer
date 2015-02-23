using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Interfaces;
using TNS.Importer.Models;
using TNS.Importer.Data;

namespace TNS.Importer.Services
{
    public class HomeService: IHomeService
    {
        IHomeRepository _repo;
       
        //TODO: add ioc container, and add the interface to the constructor
        public HomeService()
        {
            _repo = new HomeRepository();
        }

        public Product ProcessUploadedFile(Product product)
        {
            return product;
        }


        public Product SaveProduct(Product product)
        {
            return _repo.SaveProduct(product);
        }
    }
}
