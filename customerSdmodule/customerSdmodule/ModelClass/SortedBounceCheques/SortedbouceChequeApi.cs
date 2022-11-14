using customerSdmodule.Model1;
using customerSdmodule.Redis;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.SortedBounceCheques
{
    public class SortedbouceChequeApi :BaseApi
    {
        public ResponseData Get(ModelContext db)
        {
            return sortedbouncecheque(db);
        }


        public ResponseData sortedbouncecheque(ModelContext db)
        {
            try
            {
                var uniqueId = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueId, null));

                var data = (from so in db.SdChequereconcilations
                            where so.StatusId == 4 && so.BranchbankId==cacheDetails.BranchId
                            orderby so.BhVerifyDate
                            select new
                            {
                                customerName = so.CustomerName,
                                chequeNumber = so.Chequeno,
                                employeecode = so.EmployeeCode,
                                chequeSubmitDate = so.ChqSubmiteDate,
                                firmId = so.FirmId,
                                branchId = so.BranchId,
                                amount = so.Amount,
                                depositid = so.DepositId,
                                BounceedDate = so.EmployeeVerifyDate,
                                customerBank = so.CustomerBank,
                                depositBank = so.SubsidiarybankName,
                                bhId = so.BhId,
                            }).ToList();
                ResponseData _Response = new ResponseData();
                
                if (data.Count() == 0)
                {
                    Log.Error("There is No sorted Bounce cheque");
                    var results = new
                    {
                        Status = "There is No sorted Bounce cheque",
                    };
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(results);

                    Log.Information("error");
                    return _Response;


                }
                else
                {

                    
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(data);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(data);

                    Log.Information("Success");
                    return _Response;
                }
            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };

                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = message });

                Log.Information(ex.Message);
                return _Response;
            }


        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
           
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
            ///Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

    }
}
