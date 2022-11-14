﻿using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.CustomerAcccounts
{
    public class CustomerAccountsAPi : BaseApi
    {
        private CustomerAccountsData _data;
        
        
        string _clientID = Guid.NewGuid().ToString();

        public CustomerAccountsData Data { get => _data; set => _data = value; }
        

        public ResponseData Get(ModelContext db)
        {
            return CustomerAccounts(db);
        }

        public ResponseData CustomerAccounts(ModelContext db)
        {
            try
            {

                if (Data.Id.Length <= Convert.ToDecimal(14))
                {
                    var user = db.SdMasters.Where(u => u.CustId == Data.Id)
                .Select(u => new
                {
                    accountType = u.DepositType,
                    balance = u.DepositAmt,
                    accountNumber = u.DepositId,
                    accountName = u.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                    intrestRate = u.IntRt,
                    schemeId = u.SchemeId,
                    status = u.StatusId,
                    firmId = u.FirmId,
                    branchID = u.BranchId,

                }).ToList();
                    if (user.Count() == 0)
                    {
                        Log.Error("There is No Customer Accounts");
                        var results = new
                        {
                            Status = "There is No Customer Accounts",
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
                        // return Results.Ok(user);
                    }


                }
                else if (Data.Id.Length >= Convert.ToDecimal(16))
                {
                    var user = db.SdMasters.Where(u => u.DepositId == Data.Id)
               .Select(u => new
               {
                   accountType = u.DepositType,
                   balance = u.DepositAmt,
                   accountNumber = u.DepositId,
                   accountName = u.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                   intrestRate = u.IntRt,
                   schemeId = u.SchemeId,
                   status = u.StatusId,
                   firmId = u.FirmId,
                   branchID = u.BranchId,

               }).ToList();
                    if (user.Count() == 0)
                    {
                        Log.Error("There is No Customer Accounts");
                        var results = new
                        {
                            Status = "There is No Customer Accounts",
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
                        // return Results.Ok(user);
                    }

                }
                else
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new {status="Some issue found"});
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
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
                return _Response;
                // return Results.NotFound(message);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<CustomerAccountsData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {

            List<Exception> FailedValidations = new List<Exception>();

            return FailedValidations;
        }
    }
}
