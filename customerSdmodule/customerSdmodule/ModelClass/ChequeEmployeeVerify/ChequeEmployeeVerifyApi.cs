using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.ChequeEmployeeVerify
{
    public class ChequeEmployeeVerifyApi : BaseApi
    {
       // private string _jwtToken;
        private ChequeEmployeeVerifyData _chequereconcilation;

       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }
        public ChequeEmployeeVerifyData Data { get => _chequereconcilation; set => _chequereconcilation = value; }

        public ResponseData Get(ModelContext db)
        {
            return employeeverify(db);
        }


        private ResponseData employeeverify(ModelContext db)
        {

            try
            {
                var existitem =  db.SdChequereconcilations.FirstOrDefault(x => x.DepositId == Data.Depositid && x.StatusId == 0 && x.Chequeno == Data.chqNo);
                if (existitem == null)
                {
                    // return Results.NotFound();
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new {status="not found"});
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "not found" });
                    return _Response;
                }

                else
                {
                    existitem.ChequeCleardt =DateTime.Parse(Data.ClearDate);
                    // existitem.ChequeClearStatus = 1;
                    existitem.StatusId = 1;
                    existitem.EmployeeVerifyDate = DateFunctions.sysdate(db);
                    db.SaveChanges();
                    Log.Information("Success");
                    var val = new { Status = "Successfully Updated" };
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(val);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(val);
                    return _Response;
                    //return Results.Ok(val);
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
                // _Response.data = JsonSerializer.Serialize(message);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<ChequeEmployeeVerifyData>(Data);
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
