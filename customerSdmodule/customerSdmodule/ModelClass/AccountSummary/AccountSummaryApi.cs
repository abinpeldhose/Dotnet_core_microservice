using customerSdmodule.Model1;
using Serilog;
using customerSdmodule.ModelClass.DateFormat;
using System.Text.Json;

namespace customerSdmodule.ModelClass.AccountSummary
{
    public class AccountSummaryApi :BaseApi
    {

        private AccountSummaryData _data;
        
        
        string _clientID = Guid.NewGuid().ToString();

        public AccountSummaryData Data { get => _data; set => _data = value; }
        

        public ResponseData Get(ModelContext db)
        {
            return Accountsummary(db);
        }
        public ResponseData Accountsummary(ModelContext db)
        {
            try
            {

                var closedate = db.SdMasters.Where(x => x.DepositId == Data.Depositid && x.CustId == Data.Customerid).Select(x => x.CloseDate).SingleOrDefault();


                String closedatee = closedate.ToString("yyyy-MM-dd");

                DateTime Todate = DateFunctions.sysdate(db);

                // Todate =Convert.ToDateTime("2022-SEP-10") ;


                InterestCalculator intr = new InterestCalculator();
                string todate = Todate.ToString("yyyy-MM-dd");

                Decimal amount = intr.getInterest(db, Data.Depositid, closedatee, todate);
                var SettledAmount = db.SdMasters.Where(x => x.DepositId == Data.Depositid && x.CustId == Data.Customerid).Select(x => x.DepositAmt).SingleOrDefault();
                var settle = Math.Round(SettledAmount + amount);
                var Notround = SettledAmount + amount;
                var round = settle - Notround;
                decimal int_charge = db.GeneralParameters.Where(x => x.ParmtrId == 15 && x.ModuleId == 4).Select(x => Convert.ToDecimal(x.ParmtrValue)).SingleOrDefault();
                var frm_date = db.SdMasters.Where(x => x.DepositId == Data.Depositid).Select(x => x.DepositDate).SingleOrDefault();
                var interval = frm_date.AddMonths(6);
                if (Todate < interval)
                {
                    var data = (from summary in db.SdMasters
                                where summary.CustId == Data.Customerid && summary.DepositId == Data.Depositid
                                select new
                                {
                                    accountType = summary.DepositType,
                                    accountNumber = summary.DepositId,
                                    balance = summary.DepositAmt,
                                    Interest = amount,
                                    roundindDifference = round,
                                    intrestRate = summary.IntRt,
                                    Status = summary.StatusId,
                                    settleAmount = Math.Round(summary.DepositAmt + amount) - int_charge,
                                }).SingleOrDefault();

                    if (data == null)
                    {
                        Log.Error("Invalid Account Number Or DepositId");

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Invalid Account Number Or DepositId" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                        // return Results.NotFound(new { status = "Invalid Account Number Or DepositId" });

                    }
                    else
                    {
                        Log.Information("Account Summary details Fetched Successfully");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(data);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                        // return Results.Ok(data);
                    }
                }
                else
                {
                    var data = (from summary in db.SdMasters
                                where summary.CustId == Data.Customerid && summary.DepositId == Data.Depositid
                                select new
                                {

                                    accountType = summary.DepositType,
                                    accountNumber = summary.DepositId,
                                    balance = summary.DepositAmt,
                                    interest = amount,
                                    roundindDifference = round,
                                    intrestRate = summary.IntRt,
                                    status = summary.StatusId,

                                    settleAmount = Math.Round(summary.DepositAmt + amount),


                                }).SingleOrDefault();



                    if (data == null)
                    {
                        Log.Error("Invalid Account Number Or DepositId");

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Invalid Account Number Or DepositId" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                        // return Results.NotFound(new { status = "Invalid Account Number Or DepositId" });

                    }
                    else
                    {
                        Log.Information("Account Summary details Fetched Successfully");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(data);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                        //return Results.Ok(data);
                    }
                }


                


            }
            catch (Exception ex)
            {


                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { Status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
                //return Results.NotFound(new { Status = "something went wrong" });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<AccountSummaryData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            var depositid = db.SdMasters.Where(x => x.DepositId == Data.Depositid && x.CustId == Data.Customerid).SingleOrDefault();

            List<Exception> FailedValidations = new List<Exception>();

            if (depositid == null)
            {
                FailedValidations.Add(new ApplicationException("Deposit Id and  Customer Id are  Invalid"));
            }



            return FailedValidations;
        }
    }
}
