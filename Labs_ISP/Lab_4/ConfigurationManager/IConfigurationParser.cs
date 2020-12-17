using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager
{
    public interface IConfigurationParser
    {
        T Parse<T>() where T : class;
    }
}
