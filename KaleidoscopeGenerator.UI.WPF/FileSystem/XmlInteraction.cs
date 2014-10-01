using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KaleidoscopeGenerator.UI.WPF.FileSystem
{
    public class XmlInteraction : IInteraction
    {

        public void save(object obj, string path)
        {
            var serializer = new XmlSerializer(obj.GetType());
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public T load<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(path))
            {
                return (T)serializer.Deserialize(reader);
            }
            // this should never happe as Deserialize throws if something goes wrong
            throw new InvalidOperationException();
        }
    }
}
