using customerSdmodule.Model1;
using Serilog;

namespace customerSdmodule.ModelClass.Getbalancesample
{
    public class GetbalanceApi : IGetAPI
    {
        private GetbalanceData _data;

        public GetbalanceData Data { get => _data; set => _data = value; }

        public IResult Get(ModelContext db)
        {
            return getbalance(db);
        }

        public IResult getbalance(ModelContext db)
        {

            try
            {
                var Getbalance = (from sdmaster in db.SdMasters


                            where sdmaster.DepositId == Data.DepositNumber && sdmaster.StatusId==1
                            select new
                            {
                               accountBalance = sdmaster.DepositAmt,
                               accountHolder = sdmaster.CustName                               

                            }).SingleOrDefault();
                if (Getbalance == null)
                {
                    Log.Error("Incorrect Deposit Number");
                    var results = new
                    {
                        Status = "Incorrect Deposit Number",
                    };
                    return Results.NotFound(results);
                }
                else
                {
                    Log.Information("Successfully");
                    return Results.Ok(Getbalance);
                }
            }
            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                return Results.NotFound(message);
            }

        }



        public List<Exception> Validate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();

            var verify = db.SdMasters.Where(x => x.DepositId == Data.DepositNumber).ToList();
            if (verify == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Input"));
            }
            return FailedValidations;

        }
    }
}
