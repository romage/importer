using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Models;

namespace TNS.Importer.Interfaces
{
    public interface IScoreParser
    {
        Product Parse(Product product);
    }
}
