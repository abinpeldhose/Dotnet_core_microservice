using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.ReturnCheque
{
    public class ReturnChequeApi : BaseApi
    {


        private ReturnChequeData _data;
       // private string _jwtToken;

        public ReturnChequeData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return returncheque(db);
        }

        private ResponseData returncheque(ModelContext db)
        {
            try
            {
                ResponseData _Response = new ResponseData();
                var existitem = db.SdChequereconcilations.FirstOrDefault(x => x.DepositId == Data.Depositid && x.StatusId == 1 || x.StatusId == 2 && x.Chequeno == Data.chqNo);
                if (existitem == null)
                {
                    var message = new { Status = "Not found" };
                    //return Results.NotFound(message);
                    
                    _Response.responseCode = 404;
                    var JsonString = JsonSerializer.Serialize(message);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                    //  _Response.data = JsonSerializer.Serialize(message);
                    return _Response;
                }

                existitem.ChequeCleardt = null;
                existitem.StatusId = 0;
                existitem.EmployeeVerifyDate = null;
                db.SaveChangesAsync();
                Log.Information("Success");
                // return Results.Ok("Success");
               
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Success" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //_Response.data = JsonSerializer.Serialize(new {status="Success"});
                return _Response;
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<ReturnChequeData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();

            if (Data.Depositid == null)
            {
                FailedValidations.Add(new ApplicationException("Depositid is invalid"));
            }
            return FailedValidations;
        }
    }
}
