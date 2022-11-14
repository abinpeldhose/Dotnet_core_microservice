using customerSdmodule.Model1;
using Serilog;
using customerSdmodule.ModelClass.DateFormat;
using customerSdmodule.Message;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.CustomerPhoneNumberForget
{
    public class CustomerPhoneNumberForgetApi : BaseApi
    {
       
        private CustomerPhoneNumberData _data;
       // private string _jwtToken;
     
        public CustomerPhoneNumberData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }
        Security hash = new Security();
        public ResponseData Get(ModelContext db)
        {
            return CustomerPhoneNumberForget(db);
        }
        public ResponseData CustomerPhoneNumberForget(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cache = JsonSerializer.Deserialize<customerSdmodule.Redis.CacheData>(RedisRun.Get(uniqueKey, null));
                cache.PreAuthendicateToken = Guid.NewGuid().ToString();
                cache.OtpStatus = false;
                JwtToken=TokenManager.TokenManagement.GenerateToken(cache.PreAuthendicateToken,uniqueKey);
                // var cusdata = db.Customers.Where(x => x.CustId == customerId).Select(x => x.Phone1.ToString() && x.Phone2.ToString()).SingleOrDefault();
                int _OTPNo = new Random().Next(100000, 999999);
                var cusdata = (from customer in db.Customers
                               where customer.CustId == Data.Customerid
                               select new
                               {
                                   phone1 = customer.Phone1.TrimStart(new char[] { '0', '1', '2', '3', '4' })??"0",
                                   phone2 = customer.Phone2.TrimStart(new char[] { '0', '1', '2', '3', '4' })??"0",
                                   firm = customer.FirmId,
                                   branch = customer.BranchId,
                               }).SingleOrDefault();

                if (cusdata != null)
                {
                    var Regdata = db.UserLoginMst1s.Where(x => x.Phone == cusdata.phone1 || x.Phone == cusdata.phone2).SingleOrDefault();
                    if (Regdata != null)
                    {
                        if (Regdata.Phone.StartsWith("6") || Regdata.Phone.StartsWith("7") || Regdata.Phone.StartsWith("8") || Regdata.Phone.StartsWith("9") )
                        {
                            if (Regdata.Phone.Length == 10 )
                            {
                                var OTPdata = new Otp
                                {
                                    Mobilenumber = Regdata.Phone, //cusdata.phone1.Length == 10 ? cusdata.phone1 : cusdata.phone2,
                                    Otp1 = hash.create_hashs(_OTPNo.ToString() + "+5", DateFunctions.sysdate(db).ToString("yyyyMMdd")),
                                    TimeStamp = DateFunctions.sysdate(db),
                                    Status = 0,
                                    TransactionId = Convert.ToInt32(DateFunctions.sysdate(db).ToString("HHmmss")),
                                    UserId = Data.Customerid,
                                    MaxTime=1,
                                };
                                var showdata = new
                                {
                                    TransactionId = OTPdata.TransactionId,
                                    Phone1 = Regdata.Phone,
                                    token=JwtToken,
                                };
                                db.Otps.Add(OTPdata);
                                db.SaveChanges();                             

                                Message.Message sms = new Message.Message();
                                sms.SendSms("Otp Send To Employee", (int)cusdata.firm, (int)cusdata.branch, "", 4, Data.Customerid, "MABDEP", "9746934470", String.Format("Your MABEN OTP for User ID generation is -  {0}  .Do not share this OTP with anyone- MABEN NIDHI LIMITED", _OTPNo),db);
                                Log.Information(_OTPNo.ToString());
                                Log.Information("Success");
                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 200;
                                var Jsonstring = JsonSerializer.Serialize(showdata);
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                                //_Response.data = JsonSerializer.Serialize(showdata);
                                return _Response;

                                // return Results.Ok(showdata);
                            }
                            else
                            {
                                var Status = new
                                {
                                    status = "This MobileNumber is not Valid",
                                };
                                Log.Warning("This MobileNumber is not Valid");

                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 400;
                                var Jsonstring = JsonSerializer.Serialize(Status);
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                                //_Response.data = JsonSerializer.Serialize(Status);
                                return _Response;
                                // return Results.BadRequest(Status);
                            }
                        }
                        else
                        {

                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(new { status = "notfound" });
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);

                            // _Response.data = JsonSerializer.Serialize(new { status = "notfound" });
                            return _Response;

                            // return Results.NotFound("not found");
                        }
                    }
                    else
                    {
                        Log.Information("NotRegistered");

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Not Registered Customer" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(new { status = "Not Registered Customer" });
                        return _Response;


                        //return Results.NotFound(new { status = "Not Registered Customer" });
                    }
                }
                else
                {

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Customer Not Found" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "Customer Not Found" });
                    return _Response;

                    //return Results.NotFound(new { status = "Customer Not Found" });
                }

            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                // return Results.NotFound(message);

                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(message);
                return _Response;
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<CustomerPhoneNumberData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
            var verify = db.Customers.Where(x => x.CustId == Data.Customerid).ToList();
            var user = db.UserLoginMst1s.Where(x => x.Custid == Data.Customerid).ToList();
            if(verify == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Input"));
            }
            if (user== null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Input"));
            }
            return FailedValidations;
        }
    }
}
