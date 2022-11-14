using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.Validateuserid
{
    public class ValidateUserIdApi:BaseApi
    {

        private ValidateUserIdData _data;
        

        public ValidateUserIdData Data { get => _data; set => _data = value; }
        

        public ResponseData Get(ModelContext db)
        {
            return Validateuserid(db);
        }
        public ResponseData Validateuserid(ModelContext db)
        {
            try
            {

                var user = db.UserLoginMst1s.Where(x => x.UserId == Data.UserId).SingleOrDefault();
                if (user == null)
                {
                    Log.Information("Success");
                    var results = new
                    {
                        status = "Success",
                    };
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(results);
                    return _Response;
                }
                else
                {
                    Log.Information("/UserId Already Exist");
                    var results = new
                    {

                        status = Data.UserId + " Already Exist",
                    };
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(results);
                    return _Response;
                }
            }

            catch (Exception ex)
            {
                
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Somethings went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "Somethings went wrong" });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<ValidateUserIdData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
           
            if (Data.UserId == " ")
            { 
                FailedValidations.Add(new ApplicationException("Please enter valid data"));
            }
            return FailedValidations;
        }
    }
}
