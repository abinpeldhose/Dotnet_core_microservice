using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.GetCustomerPhone
{
    public class GetCustomerPhoneApi : BaseApi
    {

        Security hash = new Security();


        private GetCustomerPhoneData _data;
     //   private string _jwtToken;

        public GetCustomerPhoneData Data { get => _data; set => _data = value; }
      //  public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return getcustomerphone(db);
        }
        private ResponseData getcustomerphone(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cache = JsonSerializer.Deserialize<customerSdmodule.Redis.CacheData>(RedisRun.Get(uniqueKey, null));
                cache.PreAuthendicateToken = Guid.NewGuid().ToString();
                cache.OtpStatus = false;
                // var cusdata = db.Customers.Where(x => x.CustId == customerId).Select(x => x.Phone1.ToString() && x.Phone2.ToString()).SingleOrDefault();
                int _OTPNo = new Random().Next(100000, 999999);
                var cusdata = (from customer in db.Customers
                               where customer.CustId == Data.customerId
                               select new
                               {
                                   phone1 = customer.Phone1 == null ? "0" : customer.Phone1.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                   phone2 = customer.Phone2 == null?"0":customer.Phone2.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                   //phone1 = customer.Phone1.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                   //phone2 = customer.Phone2.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                   firm = customer.FirmId,
                                   branch = customer.BranchId,

                               }).SingleOrDefault();
              // 
                if (cusdata != null)
                {
                    ResponseData _Response = new ResponseData();
                    if (cusdata.phone1.StartsWith("6") || cusdata.phone1.StartsWith("7") || cusdata.phone1.StartsWith("8") || cusdata.phone1.StartsWith("9") || cusdata.phone2.StartsWith("6") || cusdata.phone2.StartsWith("7") || cusdata.phone2.StartsWith("8") || cusdata.phone2.StartsWith("9"))
                    {

                        if (cusdata.phone1.Length == 10 || cusdata.phone2.Length == 10)
                        {
                             var Regdata = db.UserLoginMst1s.Where(x => x.Phone == cusdata.phone1 || x.Phone == cusdata.phone2).SingleOrDefault();
                            if (Regdata == null)
                            {
                                var OTPdata = new Otp
                                {
                                    Mobilenumber = cusdata.phone1.Length == 10 ? cusdata.phone1 : cusdata.phone2,
                                    Otp1 = hash.create_hashs(_OTPNo.ToString() + "+5", DateFunctions.sysdate(db).ToString("yyyyMMdd")),
                                    TimeStamp = DateFunctions.sysdate(db),
                                    Status = 0,
                                    TransactionId = Convert.ToInt32(DateFunctions.sysdate(db).ToString("HHmmss")),
                                    UserId = Data.customerId,
                                    MaxTime=1,
                                };
                                var showdata = new
                                {
                                    transactionId = OTPdata.TransactionId,
                                    phone1 = cusdata.phone1.Length == 10 ? cusdata.phone1 : cusdata.phone2,
                                    token = JwtToken,
                                };
                                db.Otps.Add(OTPdata);
                                db.SaveChanges();
                                if (cusdata == null)
                                {
                                    Log.Error("Customer Not Found");
                                    var results = new
                                    {
                                        Status = "Customer Not Found",
                                    };
                                    // return Results.NotFound("Customer Not Found");
                                   
                                    _Response.responseCode = 404;
                                    var JsonString = JsonSerializer.Serialize(new { status = "Customer Not Found" });
                                    _Response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                                    // _Response.data = JsonSerializer.Serialize(new {status="Customer Not Found"});
                                    return _Response;
                                }
                                Message.Message sms = new Message.Message();
                                sms.SendSms("Otp Send To Employee", (int)cusdata.firm, (int)cusdata.branch, "", 4,Data.customerId, "MABDEP", "9746934470", String.Format("Your MABEN OTP for User ID generation is -  {0}  .Do not share this OTP with anyone- MABEN NIDHI LIMITED", _OTPNo),db);
                                Log.Information(_OTPNo.ToString());
                                Log.Information("Success");
                                // return Results.Ok(showdata);
                               // ResponseData _Response = new ResponseData();
                                _Response.responseCode = 200;
                                var Jsonstring = JsonSerializer.Serialize(showdata);
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                                // _Response.data = JsonSerializer.Serialize(showdata);
                                return _Response;



                                //return Results.Created($"/Postotp{ data.Mobilenumber}", new Otp());
                            }
                            else
                            {

                                var Status = new
                                {
                                    status = "This MobileNumber is Already Registred",
                                };
                                Log.Warning("This MobileNumber is Already Registred");
                                // return Results.BadRequest(Status);
                               // ResponseData _Response = new ResponseData();
                                _Response.responseCode = 404;
                                var Jsonstring = JsonSerializer.Serialize(Status);
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                                // _Response.data = JsonSerializer.Serialize(Status);
                                return _Response;
                            }

                        }
                        else
                        {
                            var Status = new
                            {
                                status = "This MobileNumber is not Valid",
                            };
                            Log.Warning("This MobileNumber is not Valid");
                            // return Results.BadRequest(Status);
                          //  ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(Status);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            // _Response.data = JsonSerializer.Serialize(Status);
                            return _Response;
                        }
                    }
                    else
                    {
                        // return Results.NotFound("Your customer id is not linked with your phone number");
                       // ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Your customer id is not linked with your phone number" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(new {status= "Your customer id is not linked with your phone number" });
                        return _Response;
                    }


                }
                else
                {
                    Log.Error("Customer Not Found");
                    var results = new
                    {
                        Status = "Customer Not Found",
                    };
                    // return Results.NotFound("Customer Not Found");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(results);
                    return _Response;


                }
               

            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                //return Results.NotFound(message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(message);
                return _Response;
            }

        }       

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<GetCustomerPhoneData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();

            return FailedValidations;
        }
    }
}
