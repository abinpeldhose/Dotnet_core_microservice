using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.CustomerToAccounts
{
    public class CustomerToAccountsApi :BaseApi
    {
        private CustomerToAccountsData _data;       
      

        public CustomerToAccountsData Data { get => _data; set => _data = value; }        

        public ResponseData Get(ModelContext db)
        {
            return GetCustomerToAccounts(db);
        }
        public ResponseData GetCustomerToAccounts(ModelContext db)
        {
            try
            {
                if (Data.Usertype.ToLower() == "employee")
                {
                    var employee = (from em in db.PaymentgatewayMasters
                                    where em.UserType.ToLower() == Data.Usertype.ToLower() && em.ProviderId == "105"
                                    select new
                                    {
                                        type = em.PaymentgatewayName,
                                        customerBankName = " ",
                                        customerName = " ",
                                        ifscCode = " ",
                                        accountNumber = " ",
                                        branchName = " ",
                                        status = " ",
                                    }).ToList();
                    var bank = (from bankDetails in db.NeftCustomers
                                join ifsc in db.IfscMasters on bankDetails.IfscCode equals ifsc.IfscCode into ifscDetails
                                from ifscDetail in ifscDetails.DefaultIfEmpty()
                                where bankDetails.CustId == Data.Customerid
                                select new
                                {
                                    type = "bank",
                                    customerBankName = ifscDetail.Bankname,
                                    customerName = bankDetails.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                    ifscCode = bankDetails.IfscCode,
                                    accountNumber = bankDetails.BeneficiaryAccount,
                                    branchName = bankDetails.BeneficiaryBranch,
                                    status = bankDetails.VerifyStatus,

                                }).ToList();


                    var merge = employee.Union(bank.ToList());
                    //  return employee.Count() == 0 ? Results.NotFound() : Results.Ok(new {employee=employee,bank=bank});
                    Log.Information("Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(merge);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(merge);
                    return _Response;
                }
                else if (Data.Usertype.ToLower() == "customer")
                {
                    var customer = (from em in db.PaymentgatewayMasters
                                    where em.UserType.ToLower() == Data.Usertype.ToLower() && em.ProviderId == "106"
                                    select new
                                    {
                                        type = em.PaymentgatewayName,
                                        customerBankName = " ",
                                        customerName = " ",
                                        ifscCode = " ",
                                        accountNumber = " ",
                                        branchName = " ",
                                        status = " ",
                                    }).ToList();

                    var bank = (from bankDetails in db.NeftCustomers
                                join ifsc in db.IfscMasters on bankDetails.IfscCode equals ifsc.IfscCode
                                where bankDetails.CustId == Data.Customerid
                                select new
                                {
                                    type = "bank",
                                    customerBankName = ifsc.Bankname,
                                    customerName = bankDetails.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                    ifscCode = bankDetails.IfscCode,
                                    accountNumber = bankDetails.BeneficiaryAccount,
                                    branchName = bankDetails.BeneficiaryBranch,
                                    status = bankDetails.VerifyStatus,

                                }).ToList();
                    var merge = customer.Union(bank.ToList());
                    Log.Information("Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(merge);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(merge);
                    return _Response;
                }
                else
                {
                    Log.Error("no other type");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "no other type" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "no other type" });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<CustomerToAccountsData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> Failedvalidation = new List<Exception>();
            return Failedvalidation;
        }
    }
}
