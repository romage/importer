using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Interfaces;
using TNS.Importer.Models;

namespace TNS.Importer.Data
{
    public class HomeRepository: IHomeRepository
    {
        IRepository<Product> _productRepo;
        UnitOfWork _uow;

        public HomeRepository()
        {
            ImporterDataContext _ctx = new ImporterDataContext();
            this._productRepo = new Repository<Product>(_ctx);
            this._uow = new UnitOfWork(_ctx);
        }

        public Product SaveProduct(Product product)
        {
            _productRepo.Add(product);
            _uow.SaveChanges();
            return product;
        }
    }
}
