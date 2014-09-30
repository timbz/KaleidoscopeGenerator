using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaleidoscopeGenerator.UI.WPF.FileSystem
{
    interface IInteraction
    {
        void save(object obj, string path);
        T load<T>(string path);
    }
}
