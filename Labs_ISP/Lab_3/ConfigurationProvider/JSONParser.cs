using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace WindowsService1
{

    class JSONParser: IConfigurationParser
    {   
        private readonly string jsonPath;
        public JSONParser(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }
        public T Parse<T>() where T : class
        {
            try
            {
                using (var fileStream = new FileStream(jsonPath, FileMode.OpenOrCreate))
                {
                    using (var document = JsonDocument.Parse(fileStream))
                    {
                        var jsonElement = document.RootElement;

                        if (typeof(T).GetProperties().First().Name != jsonElement.EnumerateObject().First().Name)
                        {
                            jsonElement = jsonElement.GetProperty(typeof(T).Name);
                        }

                        jsonElement = jsonElement.EnumerateObject().Single(pr => pr.Name == typeof(T).Name).Value;

                        return JsonSerializer.Deserialize<T>(jsonElement.GetRawText());
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), true, Encoding.Default))
                {
                    sw.WriteLine("Ошибка десериализации json-файла:{0}", ex.Message);
                }
                return null;
            }         
        }
    }
}
