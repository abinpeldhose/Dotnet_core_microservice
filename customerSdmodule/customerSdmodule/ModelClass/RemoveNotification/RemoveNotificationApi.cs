using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.RemoveNotification
{
    public class RemoveNotificationApi :BaseApi
    {

        private RemoveNotificationData _data;
       

        public RemoveNotificationData Data { get => _data; set => _data = value; }
        

        public ResponseData Get(ModelContext db)
        {
           return clearnotification(db);
        }
        private ResponseData clearnotification(ModelContext db)
        {
            try
            {

                var existitem = db.Alerts.FirstOrDefault(x => x.Id == Data.userId && x.AlertId == Data.alertId);
                if (existitem == null)
                {
                    Results.NotFound();
                }
                existitem.ReadDate = DateFunctions.sysdate(db);

                db.SaveChangesAsync();
                Log.Information("Notification Readed");
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Success" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "success" });
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
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<RemoveNotificationData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();

            if (Data.alertId == null)
            {
                FailedValidations.Add(new ApplicationException("Id is not null here"));
            }
            return FailedValidations;
        }

       
    }
}
