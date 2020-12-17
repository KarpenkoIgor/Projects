using System;
using DataAccessLayer;
using XmlGeneratorService;

namespace ServiceLayer
{
    public class ServiceLayerClass
    {
        private DataAccess serviceLayer;
        private XmlGenerator xmlGenerator;

        public ServiceLayerClass(string connectionString)
        {
            serviceLayer = new DataAccess(connectionString);
        }

        public string CreateXmlFromDataBase()
        {
            var list = serviceLayer.GetPersons();
            string xmlFileName;
            xmlGenerator = new XmlGenerator(list);
            xmlFileName = xmlGenerator.Generate();
            xmlGenerator.GenerateXsd();
            return xmlFileName;
        }
    }
}
