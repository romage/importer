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
            ExcelParserViaDom domParser = new ExcelParserViaDom();
            var ret = domParser.Parse(_physicalFileName);
            Assert.IsType<Product>(ret);
        }


        public interface IScoreParser
        {
            Product Parse(string physicalPath);
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

        public class ExcelParserViaDom : IScoreParser
        {
            public Product Parse(string physicalPath)
            {
                var returnProduct = new Product();

                //_fileLoader.checkForFileErrorsFile(_physicalFileName);
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(physicalPath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();

                    string text = "";

                    foreach (WorksheetPart wsp in workbookPart.WorksheetParts)
                    {
                        SheetData sheetData = wsp.Worksheet.Elements<SheetData>().First();

                        foreach (Row row in sheetData.Elements<Row>())
                        {
                            checkInitialSpans(row);

                            Score s = new Score();
                            s.ScoreName = getScoreName(row, workbookPart);
                            s.ScoreValue = getScoreValue(row);

                            returnProduct.Scores.Add(s);
                        }

                    }

                }
                return returnProduct;
            }

            private double getScoreValue(Row r)
            {
                double cellValue = -1;
                
                Cell c = (Cell)r.ElementAt(1);
                cellValue = double.Parse(c.CellValue.Text);
                return cellValue;
            }

            private string getScoreName(Row r, WorkbookPart workbookPart)
            {
                string cellText = string.Empty;
                Cell c = (Cell)r.ElementAt(0);
                if (c != null && c.DataType != null && c.DataType == CellValues.SharedString)
                {
                    int id = -1;
                    Int32.TryParse(c.InnerText, out id);
                    var item = GetSharedStringItemById(workbookPart, id);
                    if (item.Text != null)
                    {
                        cellText = item.Text.Text;
                    }
                    else if (item.InnerText != null)
                    {
                        cellText = item.InnerText;
                    }
                    else if (item.InnerXml != null)
                    {
                        cellText = item.InnerXml;
                    }
                }
                return cellText;
            }

            private int checkInitialSpans(Row r)
            {
                string[] spans = r.Spans.InnerText.Split(':');

                if(spans[0] != "1")
                    throw new ExcelParserException("The first column should not be blank");

                int endSpan= 1;
                int.TryParse(spans[1], out endSpan);
                if (endSpan < 2)
                {
                    throw new ExcelParserException("There should be at least two columns, with the second column holding the score");
                }
                return endSpan;
            }
        }

        public class ExcelParserException : Exception 
        {
            public string ParseError { get; set; }
            public ExcelParserException(string ParseError)
            {
                this.ParseError = ParseError;
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

        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
            {
                return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
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
