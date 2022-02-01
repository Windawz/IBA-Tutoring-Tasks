using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using E = Microsoft.Office.Interop.Excel;

namespace CSStarterTest1.DataOps
{
    public class XlsxExporter : IExporter
    {
        public XlsxExporter()
        {
            // excel installed?
            if (Type.GetTypeFromProgID("Excel.Application") is null)
            {
                throw new NotSupportedException("Excel installation not found");
            }
        }

        public void Export(object? data, string path)
        {
            using (var excel = Excel.OpenInstance("TestProgram"))
            {
                if (data is IEnumerable elems)
                {
                    foreach (var elem in elems)
                    {
                        excel.AddRecord(elem);
                    }
                }
                else if (data is not null)
                {
                    excel.AddRecord(data);
                }

                excel.Save(path);
            }

            // Better safe than sorry
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // Layered the Excel stuff in a separate class just to be extra safe.
        private class Excel : IDisposable
        {
            private Excel(string title)
            {
                _app = new E.Application();
                _app.SheetsInNewWorkbook = 1;
                _app.DisplayAlerts = false;

                _books = _app.Workbooks;
                _book = _books.Add();

                _sheets = _book.Worksheets;
                _sheet = _sheets[1];
                _sheet.Name = title;

                _sheetCells = _sheet.Cells;

                Title = title;
            }
            
            private E.Application _app;
            private E.Workbooks _books;
            private E.Workbook _book;
            private E.Sheets _sheets;
            private E.Worksheet _sheet;
            private E.Range _sheetCells;
            private bool _disposed;

            public static Excel? Instance { get; private set; }
            public string Title { get; }

            public static Excel OpenInstance(string title)
            {
                if (Instance is null)
                {
                    Instance = new Excel(title);
                }

                return Instance;
            }
            public void AddRecord(object data)
            {
                int row = _sheet.UsedRange.Rows.Count;
                PropertyInfo[] props = data.GetType().GetProperties();

                for (int i = 0; i < props.Length; i++)
                {
                    object value = props[i].GetValue(data)!;
                    string formatted = new DataFieldFormatter().Format(value);

                    _sheetCells[row, i + 1] = formatted;
                }
            }
            public void Save(string path)
            {
                _book.SaveAs(Filename: path);
            }
            public void Dispose()
            {
                if (!_disposed)
                {
                    // KILL IT WITH FIRE
                    _sheetCells = null!;
                    _sheet = null!;
                    _sheets = null!;
                    _book.Close();
                    _book = null!;
                    _books.Close();
                    _books = null!;
                    _app.Quit();
                    _app = null!;

                    Instance = null;
                    GC.SuppressFinalize(this);
                    _disposed = true;
                }
            }

            ~Excel()
            {
                Dispose();
            }
        }
    }
}
