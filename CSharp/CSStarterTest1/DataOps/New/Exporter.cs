
using System.IO;

namespace CSStarterTest1.DataOps.New
{
    public abstract class Exporter : IExporter
    {
        protected abstract string Extension { get; }

        public void Export(object? data, string path)
        {
            if (!Path.HasExtension(path))
            {
                path = Path.ChangeExtension(path, Extension);
            }
            if (Path.GetFileNameWithoutExtension(path) == "")
            {
                int pos = path.LastIndexOf('.');
                path = path.Insert(pos, "TestProgram");
            }

            ExportImpl(data, path);
        }

        protected abstract void ExportImpl(object? data, string path);
    }
}
