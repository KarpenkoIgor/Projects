using System;
using System.Collections.Generic;
using System.IO;
using Models;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace XmlGeneratorService
{
    public class XmlGenerator: IXmlGenerator
    {
        private readonly List<Person> persons;
        private string xmlPath;

        public XmlGenerator(List<Person> persons)
        {
            this.persons = persons;
            xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Persons.xml");
        }

        public string Generate()
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Person>));

                using (FileStream fs = new FileStream(xmlPath, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, persons);
                }
            }
            catch (Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }
            return xmlPath;
        }

        public void GenerateXsd()
        {
            try
            {
                XmlReader reader = XmlReader.Create(xmlPath);
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                XmlSchemaInference schema = new XmlSchemaInference();
                schemaSet = schema.InferSchema(reader);
                FileInfo xmlPathFile = new FileInfo(xmlPath);
                xmlPath = Path.Combine(xmlPathFile.DirectoryName, "XSD_") + xmlPathFile.Name;

                using (XmlWriter writer = XmlWriter.Create(Path.ChangeExtension(xmlPath, "xsd")))
                {
                    foreach (XmlSchema s in schemaSet.Schemas())
                    {
                        s.Write(writer);
                        writer.Close();
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sw = new StreamWriter(path, true);
                sw.Write(ex.Message);
            }
        }
    }
}
