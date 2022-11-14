using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.DeleteNTransaction
{
    public class DeleteNTransactionApi : BaseApi
    {
        private DeleteNTransactionData _data;
        

        public DeleteNTransactionData Data { get => _data; set => _data = value; }
        

        public ResponseData Get(ModelContext db)
        {
            return deleteScheduletransaction(db);
        }

        public ResponseData deleteScheduletransaction(ModelContext db)
        {
            try
            {

                if (Data.flag == 0)
                {
                    var schedule = db.SdScheduleTrans.Where(x => x.RtId == Data.rtId && x.TraDt == Data.transactionDate).SingleOrDefault();

                    if (schedule == null)
                    {
                        Log.Error("There is No Data Found");

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "failed" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "failed" });
                        return _Response;

                    }
                    else if (schedule.UserType == 0 && Data.userType.ToLower() == "employee")
                    {
                        schedule.StatusId = 2;
                         db.SaveChanges();
                        Log.Information("Success");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "success" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(new { status = "success" });
                        return _Response;

                    }
                    else
                    {

                        schedule.StatusId = 0;
                         db.SaveChanges();
                        Log.Information("Success");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "success" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(new { status = "success" });
                        return _Response;
                    }
                }
                else if (Data.flag == 1)
                {
                    var schedule = db.SdScheduleTrans.Where(x => x.RtId == Data.rtId).ToList();
                    var scheduleMaster = db.SdScheduleMasters.FirstOrDefault(x => x.RtId == Data.rtId);
                    if (schedule.Count() == 0)
                    {
                        Log.Error("No Entry");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "no entry" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "no entry" });
                        return _Response; ;
                    }
                    else if (scheduleMaster.UserType == 0 && Data.userType.ToLower() == "employee")
                    {
                        foreach (var data in schedule)
                        {
                            data.StatusId = 2;
                            db.SaveChanges();
                        }
                        scheduleMaster.StatusId = 2;
                        db.SaveChanges();
                        Log.Information("Success");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "success" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "success" });
                        return _Response;
                    }
                    else
                    {
                        foreach (var data in schedule)
                        {
                            data.StatusId = 0;
                            db.SaveChanges();
                        }
                        scheduleMaster.StatusId = 0;
                        db.SaveChanges();
                        Log.Information("Success");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "success" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(new { status = "success" });
                        return _Response;
                    }

                }
                else
                {
                    Log.Error("There Is No Other Flag");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "no other flag" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new { status = "no other flag" });
                    return _Response;
                }

            }
            catch (Exception ex)
            {                
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<DeleteNTransactionData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> Failedvalidation = new List<Exception>();
            var value = db.SdScheduleTrans.Where(x => x.RtId == Data.rtId).ToList();
            if(value == null)
            {
                Failedvalidation.Add(new ApplicationException("Inavlid input"));
            }
            return Failedvalidation;
        }

    }
}
