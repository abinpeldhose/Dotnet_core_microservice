using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    public interface ICacheService
    {
        public CacheData GetData(string key);
       public  bool SetData(string key, CacheData value, DateTimeOffset expirationTime);
        public object RemoveData(string key);
    }
}
