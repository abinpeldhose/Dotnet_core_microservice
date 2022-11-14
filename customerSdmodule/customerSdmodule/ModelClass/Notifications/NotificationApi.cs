using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.Notifications
{
    public class NotificationApi : BaseApi
    {

        private NotificationData _data;
        
     
        string _clientID = Guid.NewGuid().ToString();

        public NotificationData Data { get => _data; set => _data = value; }
        

        public ResponseData Get(ModelContext db)
        {
            return Notification(db);
        }
        public ResponseData Notification(ModelContext db)
        {
            try
            {
                var user = (from alert in db.Alerts
                            join details in db.AlertDetails on alert.Type equals details.Alerttype



                            where alert.Id == Data.Userid && alert.ReadDate == null
                            select new
                            {
                                userId = alert.Id,
                                alertId = alert.AlertId,
                                type = alert.Type,
                                image = details.Image,
                                subject = alert.Subject,
                                date = alert.AlertDate,
                                description = alert.AlertDescription,

                            }).ToList();
                if (user.Count() == 0)
                {
                    Log.Error("There is No Alerts");

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "There is no Alerts" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new {status="There is no Alerts"});
                    return _Response;


                }
                else
                {
                    Log.Information("/Sucesss");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(user);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(user);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<NotificationData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
            var userid = db.AlertDetails.Where(x => x.Alerttype == Data.Userid).ToList();
            if(userid == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid UserId is entered"));
            }
            return FailedValidations;
        }
    }
}
