using StackExchange.Redis;
using System.Text.Json;

namespace RedisCache
{
    public class CacheService
    {
        private IDatabase _db;
        private static object _lock = new object();


        public CacheService()
        {
            ConfigureRedis();
        }
        private void ConfigureRedis() 
        {
            ConfigurationOptions options = new ConfigurationOptions()
            {
               
                EndPoints = { { "10.192.5.42", 6379 } },
                AllowAdmin = true,
                ConnectTimeout = 60 * 1000,
                //AbortOnConnectFail = false,
                //ResponseTimeout = 60000,

            };
            _db = ConnectionMultiplexer.Connect(options).GetDatabase(); 
        }

       
        public CacheData GetData(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<CacheData>(value);
            }
            return default;
        }
        public bool SetData<CacheData>(string key, CacheData value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }
}