using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.BHGroup
{
    public class BHGroupAPI:BaseApi
    {
       // private string _jwtToken;

       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return BhGroup(db);
        }
        public ResponseData BhGroup(ModelContext db)
        {
            try
            {
                var schedule = (from customer in db.SdMasters
                                join scheduleDetails in db.SdScheduleTrans on customer.DepositId equals scheduleDetails.DepositId
                                where customer.StatusId == 1 &&
                                scheduleDetails.StatusId == 2 && scheduleDetails.TraDt.Date > DateFunctions.sysdate(db).Date
                                group scheduleDetails by scheduleDetails.RtId into sg
                                // orderby scheduleDetails.TraDt descending
                                select new
                                {
                                    RtId = sg.Key,
                                    detail = (from scheduleDetail in db.SdScheduleTrans
                                              join master in db.SdMasters on scheduleDetail.DepositId equals master.DepositId
                                              join customer in db.Customers on master.CustId equals customer.CustId
                                              where scheduleDetail.RtId == sg.Key && scheduleDetail.StatusId == 2 &&
                                              scheduleDetail.TraDt.Date > DateFunctions.sysdate(db).Date
                                              select new
                                              {
                                                  customerName = customer.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                                  depositNumber = scheduleDetail.DepositId,
                                                  date = scheduleDetail.TraDt,
                                                  amount = scheduleDetail.Amount,
                                                  rtid = scheduleDetail.RtId,
                                                  statusid = scheduleDetail.StatusId,
                                              }).ToList(),


                                }).ToList();

                if (schedule != null)
                {
                    // return Results.Ok(schedule);
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(schedule);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                  //  _Response.data = JsonSerializer.Serialize(schedule);
                    return _Response;
                }
                else

                {
                    Log.Error("There is no sheduled Transactions");
                    //return Results.NotFound(new { status = "error" });
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "error" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "error" });                  
                    return _Response;
                }
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

            // Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize(new { DeviceID = base._cache.DeviceId });
            // Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
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
