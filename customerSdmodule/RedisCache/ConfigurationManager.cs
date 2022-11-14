using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    static class ConfigurationManager
    {
        public static IConfiguration AppSetting
        {
            get;
        }
        
    }
}
