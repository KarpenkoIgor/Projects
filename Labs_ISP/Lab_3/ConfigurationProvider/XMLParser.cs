using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.IO;

namespace WindowsService1
{
    class XMLParser : IConfigurationParser
    {
        private readonly string xmlPath;
        private readonly string xsdPath;

        public XMLParser(string xmlPath)
        {
            this.xmlPath = xmlPath;

            if (File.Exists(Path.ChangeExtension(xmlPath, "xsd")))
            {
                xsdPath = Path.ChangeExtension(xmlPath, "xsd");
            }
        }

        public T Parse<T>() where T : class
        {
            if (!Validate())
            {
                return null;
            }
            try
            {
                XDocument xdoc = XDocument.Load(xmlPath);
                var xmlElements = xdoc.Elements(typeof(T).Name).DescendantsAndSelf().Select(element=>element);
                string xmlFormat = xmlElements.First().ToString();
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                using (TextReader tr = new StringReader(xmlFormat))
                {
                    return formatter.Deserialize(tr) as T;
                }
            }
            catch (Exception ex) 
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"),true, Encoding.Default))
                {
                    sw.WriteLine("Ошибка десериализации xml-файла:{0}", ex.Message);
                }

                return null;
            }
        }

        private bool Validate()
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, XmlReader.Create(xsdPath));

                var xmlReader = XmlReader.Create(xmlPath, settings);
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlReader);
                return true;
            }
            catch (Exception ex)
            {
                using (var streamWriter = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), true, Encoding.Default))
                {
                    streamWriter.WriteLine("Ошибка валидации: {0}", ex.Message);
                }

                return false;
            }

        }
    }
}
