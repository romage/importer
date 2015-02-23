using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Importer.Services
{
    public class ExcelParserException : Exception
    {
        public string ParseError { get; set; }
        public ExcelParserException(string ParseError)
        {
            this.ParseError = ParseError;
        }

    }

}
