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

        public Product Parse(Product product)
        {
            _fileLoader.checkForFileErrorsFile(product.SystemFileName);
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(product.SystemFileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                foreach (WorksheetPart wsp in workbookPart.WorksheetParts)
                {
                    SheetData sheetData = wsp.Worksheet.Elements<SheetData>().First();

                    foreach (Row row in sheetData.Elements<Row>())
                    {
                        checkInitialSpans(row);
                        Score s = new Score();
                        // workbook needs to be passed through as spreadsheet strings are not stored in the cell, but a separate lookup table.
                        s.ScoreName = row.getScoreName(workbookPart);
                        s.ScoreValue = row.getScoreValue();
                        product.Scores.Add(s);
                    }
                }
            }
            return product;
        }


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
