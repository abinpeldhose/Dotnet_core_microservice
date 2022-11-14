using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.ForgetPassword
{
    public class ForgetPasswordAPI:BaseApi
    {
        Security hash = new Security();

       
        private ForgetPasswordData _data;

       // private string _jwtToken;
    

        public ForgetPasswordData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }
        public ResponseData Get(ModelContext db)
        {
            return forgetPasswordDetails(db);
        }
        public ResponseData forgetPasswordDetails(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cache = JsonSerializer.Deserialize<customerSdmodule.Redis.CacheData>(RedisRun.Get(uniqueKey, null));
                var existItem =  db.UserLoginMst1s.FirstOrDefault(x => x.Phone == Data.Mobilenumber && x.Custid == Data.Customerid);
                if( cache.OtpStatus == true)
                {
                    if (existItem != null && Data.Newpassword != null)
                    {
                        existItem.Password = hash.create_hashs(Data.Newpassword, existItem.RegistartionDate.ToString("yyyyMMdd"));
                        existItem.PasswordUpdateDate = DateFunctions.sysdate(db);// DateTime.Now;
                        db.SaveChanges();
                        Log.Information("Success");
                        //  return Results.Ok(new { status = "Success" });
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Success" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "Success" });
                        return _Response;

                    }
                    else
                    {
                        Log.Error("Password is not changed");
                        //return Results.NotFound(new { status = "Password is not changed" });
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Password is not changed" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(new { status = "Password is not changed" });
                        return _Response;
                    }
                }
                else
                {
                    Log.Error("unknown user");
                    // return Results.NotFound(new { status = "otp is not verified" });
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "otp is not verified" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "otp is not verified" });
                    return _Response;
                }
               

            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(message);
                return _Response; ;
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<ForgetPasswordData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
            var verify = db.UserLoginMst1s.Where(x => x.Custid == Data.Customerid && x.Phone == Data.Mobilenumber).ToList();
            if(verify == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Input"));
            }
         
            return FailedValidations;
        }
    }
}
