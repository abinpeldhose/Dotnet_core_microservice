using customerSdmodule.Model1;
using Serilog;
using customerSdmodule.ModelClass.DateFormat;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.VerifyOtp
{
    public class VerifyOtpApi :BaseApi
    {
        private VerifyOtpData _data;
       // private string _jwtToken;
      

       
        public VerifyOtpData Data { get => _data; set => _data = value; }
      //  public string JwtToken { get => _jwtToken; set => _jwtToken = value; }
        Security hash = new Security();

        public ResponseData Get(ModelContext db)
        {
           return VerifyOTP(db);
        }
        public ResponseData VerifyOTP(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cache = JsonSerializer.Deserialize<customerSdmodule.Redis.CacheData>(RedisRun.Get(uniqueKey, null));

                JwtToken=TokenManager.TokenManagement.GenerateToken(cache.DeviceId,uniqueKey);

                var existitem = db.Otps.FirstOrDefault(x => x.TransactionId == Data.Transactionid && x.Otp1 == hash.create_hashs(Data.Otp.ToString() + "+5", DateFunctions.sysdate(db).ToString("yyyyMMdd")));
                if (existitem == null)
                {
                   
                    Log.Warning("Failed");

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Failed" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new { status = "Failed" });
                    return _Response;
                    // return Results.BadRequest(new { status = "Failed", });
                    //  Log.Error("");//.Text("OTP is not valid");
                }
                else if (existitem.Status == 1)
                {                 

                    Log.Warning("this otp is already used");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "This otp is already used" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "this otp is already used" });
                    return _Response;
                    //return Results.NotFound(new { status = "this otp is already used"});
                }
                else if(existitem.TimeStamp.AddMinutes((double)existitem.MaxTime)<DateFunctions.sysdate(db))
                {
                    Log.Warning("This otp is expired");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "This otp is expired" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new { status = "This otp is expired" });
                    return _Response;
                  //  return Results.NotFound(new {status="This otp is expired"});
                }
                else
                {
                    cache.OtpStatus = true;
                    RedisRun.Set(uniqueKey, JsonSerializer.Serialize<Redis.CacheData>(cache));
                    existitem.Status = 1;
                    db.SaveChanges();
                    Log.Information("Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Success" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new { status = "Success"});
                    return _Response;
                    // return Results.Ok(new { status = "Success",token=JwtToken });
                }

            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "Somethings went wrong" });
                return _Response;
                // return Results.BadRequest(message);
            }
        }
        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<VerifyOtpData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            var verify = db.Otps.Where(x => x.TransactionId == Data.Transactionid && x.Otp1.ToString() == Data.Otp.ToString()).ToList();
            if(verify == null)
            {
                FailedValidation.Add(new ApplicationException("Invalid Inputs"));
            }
            return FailedValidation;
        }
    }
}
