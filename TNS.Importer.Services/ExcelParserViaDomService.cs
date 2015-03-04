using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Interfaces;
using TNS.Importer.Models;

namespace TNS.Importer.Services
{
    public class ExcelParserViaDomService : IScoreParser
    {

        public ExcelParserViaDomService(IFileLoader fileLoader)
        {
            this._fileLoader = fileLoader;
        }
        IFileLoader _fileLoader;

        public Product Parse(Product product, string uploadRootPhysicalPath)
        {
            string physicalPath = FileHelper.GetPhysicalFilePath(uploadRootPhysicalPath, product);
            _fileLoader.checkForFileErrorsFile(physicalPath);
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(physicalPath, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                foreach (WorksheetPart wsp in workbookPart.WorksheetParts)
                {
                    SheetData sheetData = wsp.Worksheet.Elements<SheetData>().First();

                    int spans = checkInitialSpans(sheetData.Elements<Row>().First());

                    foreach (Row row in sheetData.Elements<Row>())
                    {
                        //checkInitialSpans(row);
                        // workbook needs to be passed through as spreadsheet strings are not stored in the cell, but a separate lookup table.
                        var fred = row.getScoreId();
                        for (var met = 2; met < spans; met++)
                        {
                            Score s = new Score();
                            s.ScoreName = row.getScoreName(workbookPart);
                            s.ScoreValue = row.getScoreValue(met);
                            product.Scores.Add(s);
                        }
                    }
                }
            }
            return product;
        }

        // spans should return something like 1:3 or 1:2 where 1 is the first columns with data (I think!)
        public int checkInitialSpans(Row r)
        {
            string[] spans = r.Spans.InnerText.Split(':');

            if (spans[0] != "1")
                throw new ExcelParserException("The first column should not be blank");

            int endSpan = 1;
            int.TryParse(spans[1], out endSpan);
            if (endSpan < 2)
            {
                throw new ExcelParserException("There should be at least two columns, with the second column holding the score");
            }
            return endSpan;
        }

    }
}
