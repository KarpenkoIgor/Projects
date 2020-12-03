using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsService1
{
    class ConfigurationProvider
    {
        public readonly IConfigurationParser confParser;
        public ConfigurationProvider(string confPath) 
        {
            switch (Path.GetExtension(confPath)) 
            {
                case ".json":
                    confParser = new JSONParser(confPath);
                    break;
                case ".xml":
                    confParser = new XMLParser(confPath);
                    break;
            }
        }
    }
}
