namespace CSStarterTest1.DataOps.New
{
    public interface IExporter
    {
        void Export(object? data, string path);
    }
}
