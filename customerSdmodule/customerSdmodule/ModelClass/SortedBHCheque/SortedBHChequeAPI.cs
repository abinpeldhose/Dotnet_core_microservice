﻿using customerSdmodule.Model1;
using customerSdmodule.Redis;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.SortedBHCheque
{
    public class SortedBHChequeAPI:BaseApi
    {
       // private string jwtToken;


       // public string JwtToken { get => jwtToken; set => jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return SortedBhCheque(db);
        }


        public ResponseData SortedBhCheque(ModelContext db)
        {
            try
            {
                var uniqueId = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueId, null));

                var data = (from so in db.SdChequereconcilations
                            where (so.StatusId == 1 || so.StatusId == 2 )&& cacheDetails.BranchId==so.BranchbankId
                            orderby so.BhVerifyDate
                            select new
                            {
                                customerName = so.CustomerName,
                                chequeNumber = so.Chequeno,
                                chequeSubmitDate = so.ChqSubmiteDate,
                                firmId = so.FirmId,
                                branchId = so.BranchId,
                                amount = so.Amount,
                                statusId = so.StatusId,
                                //   BhStatus = so.BhStatus,
                                depositid = so.DepositId,
                                employeeCode = so.EmployeeCode,
                                customerBank = so.CustomerBank,
                                depositBank = so.SubsidiarybankName,

                                BHId = so.BhId,

                            }).ToList();
                if (data.Count() == 0)
                {
                    Log.Error("There is No sorted BH cheque");
                    var results = new
                    {
                        Status = "There is No sorted BH cheque",
                    };
                    // return Results.NotFound(results);
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(results);
                    return _Response;


                }
                else
                {
                    Log.Information("/Sucesss");
                    //return Results.Ok(data);
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(data);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(data);
                    return _Response;
                }
            }
            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.InnerException.Message);
                //return Results.NotFound(message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(message);
                return _Response;
            }
        }
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            //  var verify = db.UserLoginMst1s.Where(x => x.UserId == Data.Userid && x.Phone == Data.Phone).ToList();
            //if (verify == null)
            //{
            //    FailedValidation.Add(new ApplicationException("Invalid Input"));
            //}
            return FailedValidation;
        }

        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
           // Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize(new {DeviceID=base._cache.DeviceId});
           // Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
    }
}
