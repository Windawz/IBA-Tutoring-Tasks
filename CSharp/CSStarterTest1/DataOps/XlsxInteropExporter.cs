using System;
using System.Collections.Generic;
using System.Reflection;

using Excel = Microsoft.Office.Interop.Excel;

namespace CSStarterTest1.DataOps
{
    internal sealed class XlsxInteropExporter
    {
        public XlsxInteropExporter(string fileName)
        {
            _app = InitApp(Title);

            FileName = fileName;
        }

        private Excel.Application _app;

        public string Title { get; set; } = "Objects";
        public string FileName { get; set; }

        public void ExportRange(IEnumerable<object> datas)
        {
            int rowIndex = 1;
            foreach (var data in datas)
            {
                Excel.Worksheet sheet = GetMainWorksheet();
                AddRecord(data, sheet, rowIndex);
                rowIndex++;
            }

            Save();
        }
        public void Export(object data)
        {
            Excel.Worksheet sheet = GetMainWorksheet();

            AddRecord(data, sheet, 1);

            Save();
        }
        public void Quit()
        {
            GetMainWorkbook().Close();
            _app.Quit();
            _app = null!;
        }

        private void Save()
        {
            GetMainWorkbook().SaveAs(Filename: $"{FileName}.xlsx");
        }
        private Excel.Worksheet GetMainWorksheet()
        {
            Excel.Workbook book = GetMainWorkbook();
            Excel.Sheets sheets = book.Worksheets;
            Excel.Worksheet sheet = sheets[1];
            return sheet;
        }

        private Excel.Workbook GetMainWorkbook() =>
            _app.Workbooks[1];
        private static void AddRecord(object data, Excel.Worksheet sheet, int rowIndex)
        {
            PropertyInfo[] props = data.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo prop = props[i];
                string formatted = DataFieldFormatter.Format(prop.GetValue(data));

                Excel.Range cells = sheet.Cells;
                cells[rowIndex, i + 1] = formatted;
            }
        }
        private static Excel.Application InitApp(string title)
        {
            Excel.Application app = new();

            app.SheetsInNewWorkbook = 1;
            app.DisplayAlerts = false;

            Excel.Workbooks books = app.Workbooks;
            Excel.Workbook book = books.Add();

            Excel.Sheets sheets = book.Worksheets;
            Excel.Worksheet sheet = sheets[1];
            sheet.Name = title;

            return app;
        }
    }
}
