using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.StatementDetails
{
    public class StatementDetailsApi:BaseApi
    {
        private StatementdetailsData _data;
      

     
        
        public StatementdetailsData Data { get => _data; set => _data = value; }

        public ResponseData Get(ModelContext db)
        {
            return StatementDetails(db);
        }
        public ResponseData StatementDetails(ModelContext db)
        {

            try
            {
                decimal openingBalance = db.SdTrans.Where(x => x.DepositId == Data.AccountNumber && x.TraDt < DateTime.Parse(Data.fromDate)).Select(x => (x.Type == "C" ? x.Amount : x.Amount * -1)).Sum();

                var user = (from sdmaster in db.SdMasters
                            join Branchdetails in db.BranchMasters on sdmaster.BranchId equals Branchdetails.BranchId


                            where sdmaster.CustId == Data.CustomerID && sdmaster.DepositId == Data.AccountNumber

                            select new
                            {
                                branchaddress1 = Branchdetails.BranchAdd1!,
                                branchaddress2 = Branchdetails.BranchAdd2!,
                                branchaddress3 = Branchdetails.BranchAdd3!,
                                branchaddress4 = Branchdetails.BranchAdd4!,
                                branchaddress5 = Branchdetails.BranchAdd5!,
                                customerid = sdmaster.CustId,
                                branchName = Branchdetails.BranchName,
                                currentbalance = sdmaster.DepositAmt,
                                accountholderName = sdmaster.CustName.TrimStart(new char[] { '0', '1', '2' }),
                                balance = openingBalance,
                                accountType = sdmaster.DepositType,
                                accountNumber = Data.AccountNumber,

                            }).FirstOrDefault();
                if (user == null)
                {
                    Log.Error("Invalid CustomerId or AccountNumber");
                    var results = new
                    {
                        status = "Invalid CustomerId or AccountNumber",
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
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(user);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(user);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<StatementdetailsData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {

            List<Exception> FailedValidations = new List<Exception>();
            if (Data.CustomerID == null)
            {
                FailedValidations.Add(new ApplicationException("Customer Id invalid"));
            }

            return FailedValidations;
        }

       
    }
}
