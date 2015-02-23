using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using TNS.Importer.Models;
using TNS.Importer.Services;



namespace TNS.Importer.Tests.Failure
{

    [Trait("File loader and other stuff..>", "...including errors")]
    public class FileParsingTests
    {

        [Fact(DisplayName = "Excel row, has only one span")]
        public void XlsxFileParsedViaDomReturnsProduct()
        {
            //Moq<Row> getSpans = Moq.Match<Row>()
        
        }

    }
}
