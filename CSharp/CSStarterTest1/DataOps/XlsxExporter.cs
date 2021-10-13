using System;
using System.Collections.Generic;

namespace CSStarterTest1.DataOps
{
    public sealed class XlsxExporter : IDisposable
    {
        public XlsxExporter(string fileName)
        {
            _exporter = new(fileName);
        }

        private XlsxInteropExporter _exporter;
        private bool _disposed = false;

        public void ExportRange(IEnumerable<object> datas) =>
            _exporter.ExportRange(datas);
        public void Export(object data) =>
            _exporter.Export(data);
        public void Dispose()
        {
            DisposeImpl();
            GC.SuppressFinalize(this);
        }

        private void DisposeImpl()
        {
            if (!_disposed)
            {
                ExporterCleanup();
                GCCleanup();

                _disposed = true;
            }
        }
        private void ExporterCleanup()
        {
            _exporter.Quit();
            _exporter = null!;
        }
        private static void GCCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        ~XlsxExporter()
        {
            DisposeImpl();
        }
    }
}
