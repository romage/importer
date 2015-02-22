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



namespace TNS.Importer.Tests.Acceptance
{

    [Trait("File loader and other stuff..>", "...including errors")]
    public class Class1
    {
        [Fact(DisplayName="Check the test is working")]
        public void checkTestIsWorking()
        {
            Assert.True(true);
        }

        //[Fact(DisplayName = "FileLoader loads an excel file and returns a data table")]
        //public void FileLoaderLoadsAnExcelFileAndReturnsADataTable()
        //{
        //    string physicalPath = "Book1.xlsx";
        //    var ret = FileLoader.LoadFile(physicalPath);

        //    Assert.IsType<DataTable>(ret);
        //}

        [Fact(DisplayName = "FileLoader thows not found exception if the file doesn't exist")]
        public void FileLoaderThrowsANotFoundExceptionIfFilesDoesNotExist()
        {
            string physicalPath = "blaDiBlaFileDoesntexist.xlsx";
            Assert.Throws<FileNotFoundException>(() => FileLoader.LoadFile(physicalPath));
        }

        [Fact(DisplayName = "FileLoader throws an ArgumentNullException if no file name passed in")]
        public void FileLoaderThrowsArgumentNullExceptionIfNoFileNamePassedThrough()
        {

            string virtualPath = "";
            Assert.Throws<ArgumentNullException>(()=>FileLoader.LoadFile(virtualPath));
        }

        [Fact(DisplayName = "FileLoader throws an Argument Exception if wrong file extension passed in")]
        public void FileLoaderThrowsArgumentExceptionIfWrongFileTypePassedThrough()
        {

            string virtualPath = "fred.doc";
            Assert.Throws<ArgumentException>(() => FileLoader.LoadFile(virtualPath));
        }


        [Fact]
        public void TryingToOpenAModernExcelFile()
        {
            string virtualPath = "Book1.xlsx";
            FileLoader.LoadFile(virtualPath);
            Assert.True(true);
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
        // Confirmation email sent confirming that this has taken place. 
        

        // How does this work in server farm environment? does it need to?
        // Browser version support ?
        // Excel version support ? 



        class UploadController
        { 
                //public ActionResult Upload(UploadedData)
                //{
                    
                //}
        }

        public interface IUploadService
        {
            bool UploadData(UploadedData uploaded);
        }

        public interface IProcessFileService
        {
            bool ProcessFile(UploadedData uploaded);
        }

        public class UploadedData
        {
            public string SpreadsheetName { get; set; }
            public DateTime Uploaded { get; set; }
            public Product Product{ get; set; }
        }



        public class FileLoader
        {
            private static FileInfo checkForFileErrorsFile(string physicalPath)
            {
                if (string.IsNullOrWhiteSpace(physicalPath))
                {
                    throw new ArgumentNullException("Load file requires a filename");
                }

                FileInfo fi = new FileInfo(physicalPath);
                if (!fi.Extension.Equals(".xlsx", StringComparison.InvariantCultureIgnoreCase) && !fi.Extension.Equals(".xls", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ArgumentException("this requires a xlsx file. This is excel file ");
                }

                if (!File.Exists(physicalPath))
                {
                    throw new FileNotFoundException("This file doesn't exist");
                }

                return fi;
            }
            
            
            public static void LoadFile(string physicalPath)
            {

                FileInfo fi = checkForFileErrorsFile(physicalPath);
             
                if (fi.Extension.Equals(".xlsx", StringComparison.InvariantCultureIgnoreCase))
                { 
                   //ParseViaXML(physicalPath);
                    ParseViaSAX(physicalPath);
                    ParseViaDOM(physicalPath);
                }else
                {
                    DataTable dt = ParseOldDb(physicalPath);
                }
           
            }

            private static DataTable ParseOldDb(string physicalPath)
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
                        return dt;
                    }
                }
            }

            private static void ParseViaXML(string physicalPath)
            {
                XmlDocument Workbook;
                ZipArchive za = ZipFile.OpenRead(physicalPath);
                ZipArchiveEntry entry = za.GetEntry("xl/workbook.xml");
                Stream workbookstream = entry.Open();
                XDocument xDoc = XDocument.Load(workbookstream);

                //var worksheets = xDoc.Elements("sheets");


                //el.ElementAt(5).Dump();

                //XElement el = XElement.Load(@"c:\projects\wb1.xml");
                //XNode node = XNode.ReadFrom(el.CreateReader());
                //node.Dump();


                //el.XPathSelectElements("./").Dump();

                //el.Dump();
                //var sheets = el.Elements("sheets");
                //sheets.Dump();

                ////USE LINQPAD
                //XElement.Load(@"c:\projects\wb1.xml").Descendants().ElementAt(4).Dump();
                //// get xpath working. 
            }

            private static void ParseViaDOM(string physicalPath)
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(physicalPath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    
                    string text="";
                    
                    foreach (WorksheetPart wsp in workbookPart.WorksheetParts)
                    {
                        SheetData sheetData = wsp.Worksheet.Elements<SheetData>().First();
                        foreach (Row r in sheetData.Elements<Row>())
                        {
                            foreach (Cell c in r.Elements<Cell>())
                            {
                                if (c!=null && c.DataType!=null && c.DataType == CellValues.SharedString)
                                {
                                    int id = -1;
                                    Int32.TryParse(c.InnerText, out id);
                                    var item = GetSharedStringItemById(workbookPart, id);
                                    if (item.Text != null)
                                    {
                                        text = item.Text.Text;
                                    }
                                    else if (item.InnerText != null)
                                    {
                                        text = item.InnerText;
                                    }
                                    else if (item.InnerXml != null)
                                    {
                                        text = item.InnerXml;
                                    }
                                    System.Diagnostics.Debug.WriteLine(text + "");
                                }
                                else
                                {
                                    text = c.CellValue.Text;
                                    Console.Write(text + " ");
                                    System.Diagnostics.Debug.WriteLine(text + "");
                                }
                            }
                        }
                    
                    }
                    
                }
            }


            public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
            {
                return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
            }


            private static void ParseViaSAX(string physicalPath)
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
                            //if (reader.ElementType == typeof(CellValue))
                            //{
                            //    text = reader.GetText();
                            //    Console.Write(text + " ");

                            //    System.Diagnostics.Debug.WriteLine(text + "");
                            //}
                        }
                    }


                }
            }
        }
        
        public class FileParser
        {
            public FileParser (DataTable data)
	        {

	        }
        }

        public class User
        {
        }

        public class Product
        { 
        }

    }
}
