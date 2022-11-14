using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.StatementTransactionDetails
{
    public class StatementTransatctionDetailsApi : BaseApi
    {

        private StatementTransactionDetailsData _data;
       
        public StatementTransactionDetailsData Data { get => _data; set => _data = value; }
       

        public ResponseData Get(ModelContext db)
        {
            return statementtransaction(db);
        }

        private ResponseData statementtransaction(ModelContext db)
        {
            try
            {

                var user = (from sdmaster in db.SdMasters
                                // join Branchdetails in db.BranchMasters on sdmaster.BranchId equals Branchdetails.BranchId
                            join sdtr in db.SdTrans on sdmaster.DepositId equals sdtr.DepositId
                            where sdmaster.CustId == Data.CustomerID && sdmaster.DepositId == Data.AccountNumber &&
                           // Convert.ToDateTime(sdtr.TraDt).Date>= fromDate && Convert.ToDateTime(sdtr.TraDt).Date <= toDate
                           sdtr.TraDt >= DateTime.Parse(Data.fromDate)&& sdtr.TraDt <= DateTime.Parse(Data.toDate)

                            orderby sdtr.TraDt, sdtr.TransId
                            select new
                            {
                                transactionDate = sdtr.TraDt,
                                description = sdtr.Descr,
                                amount = sdtr.Amount,
                                transactionType = sdtr.Type,
                                transactionId = sdtr.TransId,

                            }).ToList();
                if (user.Count() == 0)
                {
                    Log.Error("There is No Statement TransactionDetails");
                    var results = new
                    {
                        status = "There is No Statement TransactionDetails",
                    };
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(results);
                    return _Response;


                }
                else
                {
                    Log.Information("/Sucesss");
                    // return Results.Ok(user);
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(user);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "success",data=user });
                    return _Response;
                }
            }

            catch (Exception ex)
            {
                var message = new { status = "something went wrong" };
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(message);
                return _Response;
            }


        }



        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();

            var details = db.SdMasters.Where(x => x.CustId == Data.CustomerID && x.DepositId == Data.AccountNumber).ToList();

            if (details.Count() == 0)
            {
                FailedValidations.Add(new ApplicationException("invalid Data"));
            }





            return FailedValidations;
        }

        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<StatementTransactionDetailsData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }




       

    }
}
