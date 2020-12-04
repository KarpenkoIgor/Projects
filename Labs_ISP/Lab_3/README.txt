ConfigurationProvider работает с конфигурационными файлами xml и json,
используя соответствующие парсеры, которые реализуют интерфейс IConfigurationParser.
Сам Logger работает с ConfigurationSettings, который, в зависимости от поступившего
ему конфигурационного файла, с помощью ConfigurationProvider использует соответствующий парсер и
метод Parse.