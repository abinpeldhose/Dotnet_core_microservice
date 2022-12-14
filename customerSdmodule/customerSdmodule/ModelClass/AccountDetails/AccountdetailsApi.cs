using customerSdmodule.Model1;
using System.Text.Json;
using Serilog;

namespace customerSdmodule.ModelClass.AccountDetails
{
    public class AccountdetailsApi : BaseApi
    {

      
        private AccountDetailsData _data;
        private string _jwtToken;
        

        public AccountDetailsData Data { get => _data; set => _data = value; }
       

        public ResponseData Get(ModelContext db)
        {
            return ReturnAccountDetails(db);//AccountDetails(db);
        }
        public ResponseData ReturnAccountDetails(ModelContext db)
        {
            try
            {
                var subapplicant = db.SdSubApplicants.Where(t => t.DocumentId == Data.Depositid).
             Select(t => new
             {
                 DepositId = t.DocumentId,
                 category = t.Category,

             }).ToList();

                var coapplicant = db.SdSubApplicants.Where(t => t.DocumentId == Data.Depositid && t.Category == 1)
                .Select(t => new
                {
                    coapplicantRight = t.SubType,
                    coapplicantName = t.Name,
                }).SingleOrDefault();


                var user = (from sd in db.SdMasters
                                // join status in db.StatusMasters  on (int)sub.SubType equals status.StatusId
                                // join sub in db.SdSubApplicants on sd.DepositId equals sub.DocumentId
                            where sd.DepositId == Data.Depositid /*&& sub.Category == 2*/

                            select new
                            {
                                firmid = sd.FirmId,
                                branchid = sd.BranchId,
                                schemeName = sd.DepositType,
                                schemeId = sd.SchemeId,
                                interest = sd.IntRt,
                                depositDate = sd.DepositDate,
                                balance = sd.DepositAmt,
                                accountNumber = sd.DepositId,
                                customerName = sd.CustName,
                                accountType = sd.DepositType,
                                nominee = sd.Nominee,
                                status = sd.StatusId,
                                customerId = sd.CustId,

                                coApplicantRight = subapplicant.Count() > 1 ? coapplicant.coapplicantRight : null,
                                coApplicantName = subapplicant.Count() > 1 ? coapplicant.coapplicantName : null,


                            }).SingleOrDefault();

                if (user == null)
                {
                    Log.Information("no data found");

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(user);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                    // return Results.Ok(user);
                }
                else
                {
                    Log.Information("success");

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(user);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                    // return Results.Ok(user);
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
                //return Results.NotFound(message);
            }

        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<AccountDetailsData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            var depositid = db.SdSubApplicants.Where(x => x.DocumentId == Data.Depositid).ToList();
            var value = db.SdMasters.Where(x => x.DepositId == Data.Depositid).ToList();
            if (depositid == null)
            {
                FailedValidation.Add(new ApplicationException("Inavlid Input"));
            }
            if (value == null)
            {
                FailedValidation.Add(new ApplicationException("Invalid Input"));
            }
            return FailedValidation;
        }

        protected override ResponseData OnValidationSuccess(ModelContext db)
        {
            return Get(db); // ReturnAccountDetails(db);
        }

        //public List<Exception> Validate(ModelContext db)
        //{
        //    List<Exception> FailedValidation = new List<Exception>();
        //    var depositid = db.SdSubApplicants.Where(x => x.DocumentId == Data.Depositid).ToList();
        //    var value = db.SdMasters.Where(x => x.DepositId == Data.Depositid).ToList();
        //    if (depositid == null)
        //    {
        //        FailedValidation.Add(new ApplicationException("Inavlid Input"));
        //    }
        //    if (value == null)
        //    {
        //        FailedValidation.Add(new ApplicationException("Invalid Input"));
        //    }
        //    return FailedValidation;
        //}
    }
}
