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
using TNS.Importer.Interfaces;

namespace TNS.Importer.Tests.Acceptance
{

    [Trait("File loader and other stuff..>", "...including errors")]
    public class FileParsingTests
    {
        string _physicalFileName;
        FileLoader _fileLoader;
        public FileParsingTests()
        {
            this._physicalFileName = @"C:\Projects\test\importer\TNS.Tests\bin\Debug\Book1.xlsx";
            _fileLoader = new FileLoader();
        }

        //[Fact(DisplayName = "FileLoader loads an old excel file via oldDB and returns a data table")]
        //public void FileLoaderLoadsAnOldExcelFileAndReturnsADataTable()
        //{
        //    string physicalPath = "Book1.xlsx";
        //    //var ret = FileLoader.ParseOldDb(physicalPath);

        //    Assert.IsType<DataTable>(ret);
        //}

     
        [Fact(DisplayName="xlsx file parsed via dom retuns product")]
        public void XlsxFileParsedViaDomReturnsProduct()
        {
            ExcelParserViaDomService domParser = new ExcelParserViaDomService(new FileLoader());
            var ret = domParser.Parse(_physicalFileName);
            Assert.IsType<Product>(ret);
        }

       

        public class ExcelParserViaSax : IScoreParser
        {
            public Product Parse(string physicalPath)
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(physicalPath, false))
                {


                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                    var sheetCount = workbookPart.Workbook.Sheets.Count();



                    foreach (WorksheetPart wsp in workbookPart.WorksheetParts)
                    {

                        var columnCount = wsp.Worksheet.Descendants<Column>().Count();
                        var rowCount = wsp.Worksheet.Descendants<Row>().Count();
                        var cellCount = wsp.Worksheet.Descendants<Cell>().Count();

                        OpenXmlReader reader = OpenXmlReader.Create(wsp);
                        string text;
                        while (reader.Read())
                        {
                            text = reader.GetText();
                            System.Diagnostics.Debug.WriteLine(text + "");
                        }
                    }


                }
                throw new NotImplementedException();
                return default(Product);

            }
        }
        public class ExcelParserViaOld : IScoreParser
        {
            public Product Parse(string physicalPath)
            {
                string conStr = "Provider=Microsoft.ACE.OLEDB.12.0;data source='" + physicalPath + "';Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1\" ";
                string cmd = "select * from [sheet1$]";
                var dt = new DataTable();

                using (OleDbConnection cn = new OleDbConnection(conStr))
                {
                    cn.Open();
                    DataTable schema = cn.GetSchema();

                    using (OleDbDataAdapter ad = new OleDbDataAdapter(cmd, cn))
                    {
                        ad.Fill(dt);
                        //return dt;
                    }
                }
                throw new NotImplementedException();
                return default(Product);
            }
        }

       

    }
}



// User can sign in. 
// user is logged in


// An Authenticated user is able to view upload page

// User uploads a spreadsheet
// Meta data is uploaded with the spreadsheet ? what additional data is required ? should there be an association with the a "product"
// spreadsheet is "correctly" named
// Spreadsheet is the correct format
// Uploaded spreadsheet is saved to the ToBeProcessedFolder

// Spreadsheet Parsed and Processed
// Use Xml (unzip the xlsx file)
// Use OldDb 
// Use SSIS
// Use 
// User third party software https://github.com/ExcelDataReader/ExcelDataReader
// Confirmation email sent confirming that this has taken place. 

// How does this work in server farm environment? does it need to?
// Browser version support ?
// Excel version support ? 
