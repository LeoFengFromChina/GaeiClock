using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SmokeNote.Logic.Helpers
{
    public sealed class XmlHelper
    {
        public static void Serialize(string path, object data)
        {
            string directoryName = Path.GetDirectoryName(path);
            DirectoryInfo di = new DirectoryInfo(directoryName);
            if (!di.Exists)
            {
                di.Create();
            }

            var file = new FileInfo(path);
            Stream stream;
            if (!file.Exists)
            {
                stream = file.Create();
            }
            else
            {
                stream = file.Open(FileMode.Truncate, FileAccess.Write);
            }
            using (stream)
            {
                XmlSerializer ser = new XmlSerializer(data.GetType());
                ser.Serialize(stream, data);
            }
        }

        public static T Deserialize<T>(string path)
        {
            var file = new FileInfo(path);
            Stream stream = file.Open(FileMode.Open);
            using (stream)
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                var result = (T)ser.Deserialize(stream);
                return result;
            }
        }
    }
}
