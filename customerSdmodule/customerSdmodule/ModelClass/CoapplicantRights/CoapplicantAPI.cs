using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.CoapplicantRights
{
    public class CoapplicantAPI:BaseApi
    {

       

        public ResponseData Get(ModelContext db)
        {
            return CoApplicant(db);
        }
        public ResponseData CoApplicant(ModelContext db)
        {
            try
            {
                var data = (from coApplicant in db.SdStatusMasters
                            where coApplicant.ColumnName == "SUB_TYPE"
                            select new
                            {
                                statusId = coApplicant.StatusId,
                                status = coApplicant.Status,
                            }).ToList();
                Log.Information("Success");
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(data);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //_Response.data = JsonSerializer.Serialize(data);
                return _Response;
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
            //Data.DeviceID = base._cache.DeviceId;
            var Data = new { DeviceID = base._cache.DeviceId };
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize(Data);
            // Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();          
            return FailedValidation;
        }

    }
}
