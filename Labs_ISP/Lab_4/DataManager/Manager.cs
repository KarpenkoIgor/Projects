using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ConfigurationManager;
using ServiceLayer;

namespace DataManager
{    class Manager
    {
        private readonly string targetDirectory;
        private readonly ServiceLayerClass serviceLayer;
        private readonly FileMoving fileMoving;

        public Manager(DataManagerSettings settings)
        {
            serviceLayer = new ServiceLayerClass(settings.ConnectionString);
            this.targetDirectory = settings.TargetPath;
            fileMoving = new FileMoving();
        }

        public void Start() 
        {
            try
            {
                Process();
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), true, Encoding.Default))
                {
                    sw.WriteLine("Ошибка обработки данных:", ex.Message);
                }
            }
        }
        public void Process()
        {
            string xmlFileName;
            xmlFileName = serviceLayer.CreateXmlFromDataBase();
            fileMoving.Move(xmlFileName, targetDirectory);
            FileInfo xmlPathFile = new FileInfo(xmlFileName);
            string xsdFileName = Path.ChangeExtension(Path.Combine(xmlPathFile.DirectoryName, "XSD_") + xmlPathFile.Name, "xsd");
            fileMoving.Move(xsdFileName, targetDirectory);
        }
    }
}
