using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Models;

namespace TNS.Importer.Interfaces
{
    public interface IHomeRepository
    {
        Product SaveProduct(Product product);
    }
}
