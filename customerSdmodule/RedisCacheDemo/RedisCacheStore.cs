using StackExchange.Redis;
using System.Text.Json;

namespace RedisCacheDemo
{
    public class RedisCacheStore
    {
        private string _Hostname = "localhost";
        private int _Port = 6379;
        public RedisCacheStore(string HostName, int Port)
        {
            _Hostname = HostName;
            _Port = Port;

        }
        public string Get(string KeyName,string value)
        {
           // string value;

            ConfigurationOptions options = new ConfigurationOptions()
            {
                EndPoints = { { _Hostname, _Port } },
                AllowAdmin = true,
                ConnectTimeout = 60 * 1000,
                //AbortOnConnectFail = false,
                //ResponseTimeout = 60000,

            };

            using (var redis = ConnectionMultiplexer.Connect(options))
            {
                IDatabase db = redis.GetDatabase();
                if (db.KeyExists(KeyName))
                {
                    value = db.StringGet(KeyName);
                }
                else
                {
                    value = null;
                }
            }
            return value;//JsonSerializer.Deserialize<CacheData>(value);
        }

        public bool Set(string KeyName, string Value)
        {
            bool result;

            RedisValue KeyValue = new RedisValue(Value);//JsonSerializer.Serialize<CacheData>(Value));

            ConfigurationOptions options = new ConfigurationOptions()
            {
                EndPoints = { { _Hostname, _Port } },
                AllowAdmin = true,
                ConnectTimeout = 60 * 1000,
                //AbortOnConnectFail = false,
                //ResponseTimeout = 60000,

            };

            using (var redis = ConnectionMultiplexer.Connect(options))
            {
                IDatabase db = redis.GetDatabase();

                result = db.StringSet(KeyName, KeyValue);
            }

            return result;
        }
        public class RedisRun
        {
            public static void Set(string key, string value)
            {
                RedisCacheStore session = new RedisCacheStore("10.192.5.42", 6379);
                //51.38.74.188
                session.Set(key, value);
            }
            public static string Get(String key,string value)
            {
                RedisCacheStore session = new RedisCacheStore("10.192.5.42", 6379);
                return session.Get(key,value);
            }
        }
    }
}