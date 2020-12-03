using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    class ConfigurationSettings
    {
        private readonly string configPath;

        //path - Путь файла config
        public ConfigurationSettings(string path) 
        {
            if (File.Exists(path)&&(Path.GetExtension(path) == ".xml" || Path.GetExtension(path) == ".json"))
            {
                this.configPath = path;
            }
            else this.configPath = null;
        }
        public T GetSetting<T>() where T : class
        {
            ConfigurationProvider confProvider = new ConfigurationProvider(configPath);
            return confProvider.confParser.Parse<T>();
        }
    }
}
