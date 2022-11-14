using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.DeleteBH
{
    public class DeleteBHAPI:BaseApi
    {

        private DeleteBHData _data;
       // private string jwtToken;

        public DeleteBHData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => jwtToken; set => jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return DeleteBH(db);
        }
        public ResponseData DeleteBH(ModelContext db)
        {
            try
            {
                if (Data.Flag == 0)
                {
                    var schedule = db.SdScheduleTrans.Where(x => x.RtId == Data.RtId && x.TraDt ==DateTime.Parse(Data.TransactionDate) && x.StatusId == 2).SingleOrDefault();
                    if (schedule == null)
                    {
                        Log.Error("There is No Data Found");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new {status="There is no data found"});
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);

                        Log.Information("not found");
                        return _Response;
                        // return Results.NotFound(new { status = "failed" });
                    }
                    else
                    {
                        schedule.BhId = Data.BHId;
                        schedule.StatusId = 0;
                        db.SaveChanges();
                        Log.Information("Success");
                        //  return Results.Ok(new { status = "success" });

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new {status="success"});
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(new {status="success"});

                        Log.Information("Success");
                        return _Response;
                    }
                }
                else if (Data.Flag == 1)
                {
                    var schedule = db.SdScheduleTrans.Where(x => x.RtId == Data.RtId && x.StatusId == 2).ToList();
                    var scheduleMaster = db.SdScheduleMasters.FirstOrDefault(x => x.RtId == Data.RtId);
                    if (schedule.Count() == 0)
                    {
                        Log.Error("No Entry");
                        // return Results.NotFound(new { status = "No entry" });
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new {status="no entry"});
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data= JsonSerializer.Serialize(new { status = "No entry" });


                        Log.Information("not found");
                        return _Response;
                    }

                    else
                    {
                        foreach (var data in schedule)
                        {
                            data.StatusId = 0;
                            data.BhId = Data.BHId;
                            db.SaveChanges();
                        }
                        scheduleMaster.StatusId = 0;
                        db.SaveChanges();
                        Log.Information("Success");
                        // return Results.Ok(new { status = "success" });
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new {status="success"});
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "success" });                      
                        return _Response;
                    }

                }
                else
                {
                    Log.Error("There Is No Other Flag");
                    //return Results.NotFound(new { status = "No other flag" });
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "No other flag" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "No other flag" });
                    return _Response;

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                var message = new { Status = "something went wrong" };
                //return Results.NotFound(new { status = "server error", Message = ex.Message });
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //_Response.data = JsonSerializer.Serialize(new { status = ex.Message });
                return _Response;
            }

        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<DeleteBHData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            //  var verify = db.UserLoginMst1s.Where(x => x.UserId == Data.Userid && x.Phone == Data.Phone).ToList();
            //if (verify == null)
            //{
            //    FailedValidation.Add(new ApplicationException("Invalid Input"));
            //}
            return FailedValidation;
        }


    }
}
