using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.PutEmployeeBounceCheque
{
    public class PutEmployeeBounceApi : BaseApi
    {


        private PutEmployeeBounceData _data;

       // private string _jwtToken;

        public PutEmployeeBounceData Data { get => _data; set => _data = value; }
        //public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
           return employeebounce(db);
        }


        private ResponseData employeebounce(ModelContext db)
        {
            try
            {
                ResponseData _Response = new ResponseData();
                var existitem =  db.SdChequereconcilations.FirstOrDefault(x => x.Chequeno == Data.Cheque_no && x.DepositId == Data.DepositId);
                if (existitem == null)
                {
                    // return Results.NotFound();
                    
                    _Response.responseCode = 404;
                    var JsonString = JsonSerializer.Serialize(new { status = "not found" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                    
                    // _Response.data = JsonSerializer.Serialize(new { status = "not found" });
                    return _Response;
                }
                existitem.EmployeeCode = Data.EmpId;
                existitem.EmployeeVerifyDate = DateFunctions.sysdate(db);//DateTime.Now;
                existitem.BhVerifyDate = DateFunctions.sysdate(db);//DateTime.Now;
                existitem.ChequeCleardt =DateTime.Parse( Data.Cleardt);
                existitem.StatusId = 2;
                // existitem.RejectReason = RejectReason;
                db.SaveChangesAsync();
                Log.Information("/Success");
                var message = new { status = "Success" };
                //return Results.Ok(message);
                
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(message);
                return _Response;
            }
            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.InnerException.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Somethings went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //  _Response.data = JsonSerializer.Serialize(new { status = "Somethings went wrong" });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<PutEmployeeBounceData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {

            List<Exception> FailedValidations = new List<Exception>();

            if (Data.DepositId == null)
            {
                FailedValidations.Add(new ApplicationException("Depositid is invalid"));
            }
            return FailedValidations;
        }
    }
}
