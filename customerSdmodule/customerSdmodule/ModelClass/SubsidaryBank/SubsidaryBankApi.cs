using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.SubsidaryBank
{
    public class SubsidaryBankApi : BaseApi
    {
   
        private SubsidaryBankData _data;
       // private string _jwtToken;
       
        string _clientID = Guid.NewGuid().ToString();

        public SubsidaryBankData Data { get => _data; set => _data = value; }
      //  public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return SubsidaryBank(db);
        }
        public ResponseData SubsidaryBank(ModelContext db)
        {
            try
            {
                if (Data.Modeoftransaction.ToUpper() == "payment".ToUpper())
                {
                    var user = (from banksubsidary in db.SubsidaryMasters


                                where banksubsidary.FirmId == Data.Firmid && banksubsidary.BranchId == Data.Branchid /*&& (banksubsidary.BranchId == Data.Branchid || banksubsidary.BranchId == 0)*/
                                select new
                                {
                                    accountName = banksubsidary.AccountName,
                                    accountNo = banksubsidary.AccountNo,
                                    bankBranchId = banksubsidary.BranchId,

                                }).ToList();
                    if (user.Count() == 0)
                    {
                        Log.Error("There is no subsidary banks found");
                        var results = new
                        {
                            Status = "There is no subsidary banks found",
                        };
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(results);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                        // return Results.NotFound(results);


                    }
                    else
                    {
                        Log.Information("/Sucesss");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(user);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                        // return Results.Ok(user);
                    }
                }

                else if (Data.Modeoftransaction.ToUpper() == "receipt".ToUpper())
                {
                    var user = (from banksubsidary in db.SubsidaryMasters


                                where banksubsidary.FirmId == Data.Firmid && banksubsidary.ParentAcc == 32100 && (banksubsidary.BranchId == Data.Branchid || banksubsidary.BranchId == 0)
                                orderby banksubsidary.BranchId descending
                                select new
                                {
                                    accountName = banksubsidary.AccountName,
                                    accountNo = banksubsidary.AccountNo,
                                    bankBranchId = banksubsidary.BranchId,

                                }).ToList();
                    if (user.Count() == 0)
                    {
                        Log.Error("There is no subsidary banks found");
                        var results = new
                        {
                            status = "There is no subsidary banks found",
                        };
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(results);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                        //  return Results.NotFound(results);


                    }
                    else
                    {
                        Log.Information("/Sucesss");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(user);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                        //  return Results.Ok(user);
                    }
                }
                else
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Invalid Selection" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;

                    // return Results.NotFound(new { status = "Invalid Selection" });
                }



            }

            catch (Exception ex)
            {
                var message = new { status = "something went wrong" };
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;

                // return Results.NotFound(message);
            }

        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
           List<Exception> FailedValidations = new List<Exception>();
            var value = db.SubsidaryMasters.Where(x => x.FirmId == Data.Firmid && x.BranchId == Data.Branchid).ToList();
            if(value == null)
            {
                FailedValidations.Add(new ApplicationException("Please enter valid data"));
            }
            return FailedValidations;
        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<SubsidaryBankData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

    }
}
