using static RedisCacheDemo.RedisCacheStore;
using System.Text.Json;

namespace customerSdmodule.Redis
{
    public class RedisJsonconverter
    {
        public static void RedisCacheSet(string key,Redis.CacheData value)
        {
            RedisRun.Set(key, JsonSerializer.Serialize<CacheData>(value));
        }
        public static CacheData RedisCacheGet(string key,string value)
        {
            return JsonSerializer.Deserialize<CacheData>(RedisRun.Get(key, value));
        }
    }
}
