using customerSdmodule.Model1;
using customerSdmodule.Redis;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.LogOut
{
    public class LogOutAPI:BaseApi
    {
      //  private string jwtToken;

     //   public string JwtToken { get => jwtToken; set => jwtToken = value; }

        public ResponseData Get ()
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cache = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var deviceId=cache.DeviceId;
                JwtToken = TokenManager.TokenManagement.GenerateToken(deviceId, uniqueKey);
                cache.UserId = null;
                cache.BranchId = -1;
                cache.UserType = null;
                cache.JwtToken=JwtToken;
                RedisRun.Set(uniqueKey, JsonSerializer.Serialize<CacheData>(cache));
                // return Results.Ok(new {status="success"});
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(new { status = "success" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //_Response.data = JsonSerializer.Serialize(new { status = "success" });
                return _Response;
            }
            catch(Exception ex)
            {               
                Log.Error(ex.Message);
                // return Results.BadRequest(new { Status = "something went wrong" });
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { Status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { Status = "something went wrong" });
                return _Response;
            }
           
        }
        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get();
            return _Response;

        }
       

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {

            // Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize(new { DeviceID = base._cache.DeviceId });
            // Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {

            List<Exception> FailedValidations = new List<Exception>();

            return FailedValidations;
        }

    }


  
}
