using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using customerSdmodule.Redis;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.LoginMpin
{
    public class LoginMpinApi : BaseApi
    {
        Random rnd = new Random();
        Security hash = new Security();


        private LoginMpinData _data;
       // private string _jwtToken;

        public LoginMpinData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
           return mpinlogin(db);
        }
        private ResponseData mpinlogin(ModelContext db)
        {
            try
            {


                var RegData = db.UserLoginMst1s.FirstOrDefault(x => x.Devicetoken == Data.deviceToken && x.Appwebstatus == "2");
                if (RegData == null)
                {
                    // return Results.NotFound(new { status = "user is not registered" });
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "user is not registered" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new { status = "user is not registered" });
                    return _Response;
                }
                else
                {
                    if (RegData.Mpin == hash.create_hashs(Data.mpin, RegData.MpinDt.ToString("yyyyMMdd")) && Data.deviceToken == RegData.Devicetoken)
                    {
                        var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                        var result = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                        var customer = db.Customers.Where(x => x.CustId == RegData.Id).
                    Select(x => new
                    {
                        customerId = x.CustId,
                        customerName = x.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                        firmId = x.FirmId,
                        branchId = x.BranchId,
                        maritalStatus = x.MaritalStatus,
                        fatherName = x.FatherName,
                        phoneNumber = x.Phone1,
                        pinNo = x.PinNo,
                        houseName = x.HouseName,
                        locality = x.Locality,
                        postcode = x.Locality,
                        userType = "Customer",
                        token = rnd.Next().ToString(),

                    }).SingleOrDefault();
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(customer);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(customer);
                        return _Response;
                        // return Results.Ok(customer);

                    }
                    else
                    {
                        Log.Error("mpin and Device token is not match");
                        // return Results.NotFound(new { status = "mpin and Device token is not valid" });
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "mpin and Device Token is not valid" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "mpin and Device Token is not valid" });
                        return _Response;
                    }
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<LoginMpinData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
            var verify = db.UserLoginMst1s.Where(x => x.Mpin == Data.mpin && x.Devicetoken == Data.deviceToken).ToList();
            if(verify == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Input"));
            }
            return FailedValidations;
        }
    }
}
