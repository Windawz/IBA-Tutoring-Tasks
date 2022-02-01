namespace CSStarterTest1.DataOps
{
    public interface IExporter
    {
        void Export(object? data, string path);
    }
}
