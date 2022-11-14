using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.ReportBranch
{
    public class ReportBranchApi : BaseApi
    {

        private ReportBranchData _data;

       // private string _jwtToken;
    
        public ReportBranchData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return ReportBranch(db);
        }
        public ResponseData ReportBranch(ModelContext db)
        {
            //  var pagesize = 10;
            var Status = new
            {
                status = "failed"
            };
            try
            {

                var reports = (//from report in db.SdTrans
                               from customer in db.SdMasters// on report.DepositId equals customer.DepositId
                               where customer.FirmId == Data.Firmid && customer.BranchId == Data.Branchid
                               orderby customer.DepositDate descending
                               select new
                               {
                                   type = "SAVINGS",
                                   docId = customer.DepositId,
                                   customerName = customer.CustName.TrimStart(new char[] { '0', '1', '2', '3', }),
                                   amount = customer.DepositAmt,
                                   intDate = customer.CloseDate,
                                   intRate = customer.IntRt,
                                   intAcured = 0,
                                   intPayable = 0,
                               }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();
                Log.Information("Success");
                if(reports == null)
                {
                    // return Results.NotFound(Status)
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(Status);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(Status);
                    return _Response;
                }
                else
                {
                    //return Results.Ok(reports);
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(reports);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //  _Response.data = JsonSerializer.Serialize(reports);
                    return _Response;
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<ReportBranchData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            return FailedValidation;
        }
    }
}
