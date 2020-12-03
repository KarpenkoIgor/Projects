using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    interface IConfigurationParser
    {
        T Parse<T>() where T : class;
    }
}
