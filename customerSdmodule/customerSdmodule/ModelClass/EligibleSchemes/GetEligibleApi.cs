using customerSdmodule.Model1;
using customerSdmodule.Redis;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.EligibleSchemes
{
    public class GetEligibleApi:BaseApi
    {
        private GetEligibleSchemesData _data;

       

        public GetEligibleSchemesData Data { get => _data; set => _data = value; }
       

        public ResponseData Get(ModelContext db)
        {
            return eligibleschemes(db);
        }

        private ResponseData eligibleschemes(ModelContext db)
        {
            try
            {

                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var today = DateTime.Now;

                ///var date = db.SdSchemes.Where(x => x.BranchId == Data.branchid && x.FromDate < today && today< x.ToDate).Select(x => x.SchemeId);
                var maxAmount = db.GeneralParameters.Where(x => x.ParmtrId == 10 && x.ModuleId == 4).Select(x => x.ParmtrValue).SingleOrDefault();
                var MinAmount = db.GeneralParameters.Where(x => x.ParmtrId == 7 && x.ModuleId == 4).Select(x => x.ParmtrValue).SingleOrDefault();

              
                    var data = (from sdscheme in db.SdSchemes
                                join sdinterest in db.SdInterests on sdscheme.SchemeId equals sdinterest.SchemeId

                                where sdscheme.BranchId == cacheDetails.BranchId && sdinterest.BranchId== cacheDetails.BranchId && sdscheme.FromDate < today && today < sdscheme.ToDate
                                select new
                                {
                                    schemeId = sdscheme.SchemeId,
                                    SchmeName = sdscheme.Scheme,
                                    interestRate = sdinterest.IntRate,
                                    MinimumAmount = Convert.ToInt32(MinAmount),
                                    MaxAmount = Convert.ToInt64(maxAmount),

                                }).ToList();
                    if (data.Count() == 0)
                    {
                      Log.Error("There is No Eligible Schemes");
                    var results = new
                    {
                        Status = "There is No Eligible Schemes",
                    };
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
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //_Response.data = JsonSerializer.Serialize(message);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<GetEligibleSchemesData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
           // var branchid = db.SdSchemes.Where(x => x.BranchId == Data.branchid).ToList();


            //if (branchid.Count() == 0)
            //{
            //    FailedValidations.Add(new ApplicationException("Inavalid Data"));
            //}

            return FailedValidations;
        }
    }
}