using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.IFSC
{
    public class IfscApi : BaseApi
    {

        private IfscData _data;

        string _clientID = Guid.NewGuid().ToString();
             
        public IfscData Data { get => _data; set => _data = value; }
        

        public ResponseData Get(ModelContext db)
        {
            return GetIfsc(db);
        }
        public ResponseData GetIfsc(ModelContext db)
        {

            try
            {

                var user = (from bankifsc in db.IfscMasters


                            where bankifsc.IfscCode == Data.Ifsccode
                            select new
                            {
                                Bankname = bankifsc.Bankname,
                                Branchname = bankifsc.Branch

                            }).SingleOrDefault();
                if (user == null)
                {
                    Log.Error("There is No Bank Found This IFSC");
                    var results = new
                    {
                        Status = "There is No Bank Found This IFSC",
                    };
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(results);
                    return _Response; ;


                }
                else
                {
                    Log.Information("IFSC Details Fetched Successfully");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(user);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(user);
                    return _Response;
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<IfscData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            var ifsc = db.IfscMasters.Where(x => x.IfscCode == Data.Ifsccode).SingleOrDefault();

            List<Exception> FailedValidations = new List<Exception>();

            if (ifsc == null)
            {
                FailedValidations.Add(new ApplicationException("Please Check Your  Ifsc Code"));
            }

            return FailedValidations;
        }
    }
}
