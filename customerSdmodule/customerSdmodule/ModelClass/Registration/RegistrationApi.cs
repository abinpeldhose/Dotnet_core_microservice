using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.Registration
{
    public class RegistrationApi : BaseApi
    {
        private RegistartionData _data;
       // private string _jwtToken;
     
        public RegistartionData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }
        Security hash = new Security();
        public ResponseData Get(ModelContext db)
        {
            return Registration(db);
        }
        public ResponseData Registration(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cache = JsonSerializer.Deserialize<customerSdmodule.Redis.CacheData>(RedisRun.Get(uniqueKey, null));
                if(cache.OtpStatus==true)
                {
                    var data = new UserLoginMst1
                    {
                        FirmId = (byte)Data.Firmid,
                        BranchId = Data.Branchid,
                        Id = Data.Customerid,
                        Custid = Data.Customerid,
                        UserId = Data.Userid,
                        Password = hash.create_hashs(Data.Password, DateFunctions.sysdate(db).ToString("yyyyMMdd")),
                        Phone = Data.Phone,
                        RegistartionDate = DateFunctions.sysdate(db),
                        PasswordUpdateDate = DateFunctions.sysdate(db),
                        MaxDay = 30,
                        PasswordRules = 1,
                        Status = 1,
                        Appwebstatus = "1",
                    };
                    db.UserLoginMst1s.Add(data);
                    db.SaveChanges();
                    Log.Information("Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Success" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "Success" });
                    return _Response;
                    //return Results.Ok(new { status = "Success" });
                }
                else
                {
                    Log.Information("unknown user");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "unknown user" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "unknown user" });
                    return _Response;
                }              

            }
            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //_Response.data = JsonSerializer.Serialize(new { status = "somethings went wrong" });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<RegistartionData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            var verify = db.UserLoginMst1s.Where(x => x.FirmId == Data.Firmid && x.BranchId == Data.Branchid && x.Custid == Data.Customerid && x.Phone == Data.Phone).FirstOrDefault();
            if(verify != null)
            {
                FailedValidation.Add(new ApplicationException("This registration is already used"));
            }
            return FailedValidation;
        }
    }
}
