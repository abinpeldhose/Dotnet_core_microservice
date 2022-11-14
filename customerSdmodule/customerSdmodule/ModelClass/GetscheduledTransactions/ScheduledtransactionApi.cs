using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.GetscheduledTransactions
{
    public class ScheduledtransactionApi : BaseApi
    {

        private ScheduledTransactionData _data;
       // private string _jwtToken;

        public ScheduledTransactionData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
           return scheduled(db);
        }


        private ResponseData scheduled(ModelContext db)
        {
            try
            {
                var schedule = (from customer in db.SdMasters
                                join scheduleDetails in db.SdScheduleTrans on customer.DepositId equals scheduleDetails.DepositId
                                where customer.CustId == Data.CustomerID && customer.StatusId == 1 &&
                                (scheduleDetails.StatusId == 1 || scheduleDetails.StatusId == 2) && scheduleDetails.TraDt.Date > DateFunctions.sysdate(db).Date

                                orderby scheduleDetails.TraDt descending
                                select new
                                {
                                    transactionType = scheduleDetails.Type,
                                    fromAccount = scheduleDetails.DepositId,
                                    toAccount = scheduleDetails.AccountNumber,
                                    date = scheduleDetails.TraDt,
                                    amount = scheduleDetails.Amount,
                                    status = scheduleDetails.StatusId,
                                    rtId = scheduleDetails.RtId,
                                }).ToList();

                if (schedule.Count() > 0)
                {
                    //return Results.Ok(schedule);

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(schedule);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(schedule);
                    return _Response;
                }
                else

                {
                    Log.Error("There is no sheduled Transactions");
                    // return Results.NotFound(new { status = "error" });
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "There is no sheduled Transactions" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new { status = "There is no sheduled Transactions" });
                    return _Response;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "Somthings went wrong" });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<ScheduledTransactionData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }



        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> errors = new List<Exception>();
            var verify = db.SdMasters.Where(x => x.CustId == Data.CustomerID).ToList();
            if(verify == null)
            {
                errors.Add(new Exception("Invalid input"));
            }
            return errors;
        }
    }
}
