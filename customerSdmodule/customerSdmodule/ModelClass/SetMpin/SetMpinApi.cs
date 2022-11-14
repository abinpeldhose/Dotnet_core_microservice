using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.SetMpin
{
    public class SetMpinApi : BaseApi
    {

        private SetMpinData _data;

       // private string _jwtToken;
        
       
        public SetMpinData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }
        Security hash = new Security();
        public ResponseData Get(ModelContext db)
        {
            return SetMpin(db);
        }
        public ResponseData SetMpin(ModelContext db)
        {
            try
            {
                var setmpin =  db.UserLoginMst1s.FirstOrDefault(x => x.UserId == Data.Userid && x.Phone == Data.Phone);
                var checkmpin = db.UserLoginMst1s.FirstOrDefault(x => x.UserId == Data.Userid && x.Appwebstatus == "1" && x.Phone == Data.Phone);
                if (setmpin == null)
                {
                    // return Results.NotFound(new { status = "not registered" });
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "not registered" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new { status = "not registered" });
                    return _Response;
                }
                else if (checkmpin == null)
                {
                    // return Results.NotFound(new { status = "already mpin created in this number" });
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "already mpin created in this number" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new { status = "already mpin created in this number" });
                    return _Response;
                }
                else
                {

                    setmpin.Mpin = hash.create_hashs(Data.Mpin, DateFunctions.sysdate(db).ToString("yyyyMMdd"));
                    setmpin.MpinDt = DateFunctions.sysdate(db);
                    setmpin.Imeinumber = Data.Imeinumber;
                    setmpin.Devicetoken = Data.Devicetoken;
                    setmpin.Appwebstatus = "2";
                    setmpin.SmsRefId = Data.Smsrefid;
                     db.SaveChanges();
                    Log.Information("Success");
                    //return Results.Ok(new { status = "Success" });
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Success" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new { status = "success" });
                    return _Response;
                }

            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                // return Results.BadRequest(message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "somethings went wrong" });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<SetMpinData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            var verify = db.UserLoginMst1s.Where(x => x.UserId == Data.Userid && x.Phone == Data.Phone).ToList();
            if(verify == null)
            {
                FailedValidation.Add(new ApplicationException("Invalid Input"));
            }
            return FailedValidation;
        }
    }
}
