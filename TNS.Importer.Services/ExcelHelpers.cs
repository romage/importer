using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Importer.Services
{
    public static class ExcelHelpers
    {

        public static double getScoreValue(this Row r)
        {
            double cellValue = -1;

            Cell c = (Cell)r.ElementAt(1);
            cellValue = double.Parse(c.CellValue.Text);
            return cellValue;
        }

        public static string getScoreName(this Row r, WorkbookPart workbookPart)
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

        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }


    }
}
