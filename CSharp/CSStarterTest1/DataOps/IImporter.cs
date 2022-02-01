using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSStarterTest1.DataOps
{
    public interface IImporter
    {
        Data[] Import(string path);
    }
}
