using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.WithdrawalTo
{
    public class WithdrawalToApi : BaseApi
    {
        private WithdrawalToData _data;

       // private string _jwtToken;

        public WithdrawalToData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return WithdrawalTo(db);
        }
        public ResponseData WithdrawalTo(ModelContext db)
        {
            try
            {
                if (Data.Usertype.ToLower() == "employee")
                {
                    var customer = (from cust in db.Customers
                                    join master in db.SdMasters on cust.CustId equals master.CustId
                                    where master.DepositId == Data.Depositid
                                    select new
                                    {
                                        customerName = cust.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                        mobileNumber = cust.Phone1,
                                        status = master.StatusId,
                                    }).SingleOrDefault();
                    if (customer == null)
                    {
                        Log.Error("NotFound");
                        // return Results.NotFound(new { status = "SD number not found" });
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "SD number not found" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                       // _Response.data = JsonSerializer.Serialize(new { status = "SD number not found" });
                        return _Response;
                    }


                    else if (customer.status == 0)
                    {
                        Log.Error("settled account");
                        // return Results.NotFound(new { status = "Settled Account" });
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "settled account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //  _Response.data = JsonSerializer.Serialize(new { status = "settled account" });
                        return _Response;
                    }
                    else
                    {
                        Log.Information("Success");
                        // return Results.Ok(customer);
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(customer);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(customer);
                        return _Response;
                    }
                }
                else if (Data.Usertype.ToLower() == "customer")
                {
                    var customer = (from cust in db.Customers
                                    join master in db.SdMasters on cust.CustId equals master.CustId
                                    where master.DepositId == Data.Depositid
                                    select new
                                    {
                                        customerName = cust.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                        mobileNumber = cust.Phone1,
                                        status = master.StatusId,
                                    }).SingleOrDefault();
                    if (customer == null)
                    {
                        Log.Error("NotFound");
                        // return Results.NotFound(new { status = "SD number not found" });
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "SD number not found" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "SD number not found" });
                        return _Response;
                    }

                    else if (customer.status == 0)
                    {
                        Log.Error("Settled Account");
                        // return Results.NotFound(new { status = "Settled Account" });
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Settled Account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //  _Response.data = JsonSerializer.Serialize(new { status = "Settled Account" });
                        return _Response;
                    }
                    else
                    {
                        Log.Information("Success");
                        // return Results.Ok(customer);
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(customer);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //  _Response.data = JsonSerializer.Serialize(customer);
                        return _Response;
                    }


                }
                else
                {
                    Log.Error("There is no other method");
                    //return Results.NotFound(new { status = "There is no other method" });
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "There is no other method" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "There is no other method" });
                    return _Response;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                //return Results.BadRequest(new { status = "server error", message = ex.Message });
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "server error", message = ex.Message });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<WithdrawalToData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            var verify = db.SdMasters.Where(x => x.DepositId == Data.Depositid).ToList();
            if (verify == null)
            {
                FailedValidation.Add(new ApplicationException("Invalid Input"));
            }
            return FailedValidation;
        }
    }
}
