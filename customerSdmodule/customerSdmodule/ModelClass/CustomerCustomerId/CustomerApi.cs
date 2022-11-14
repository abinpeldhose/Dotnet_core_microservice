using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.CustomerCustomerId
{
    public class CustomerApi : BaseApi
    {
        
        private CustomerData _data;
       
        string _clientID = Guid.NewGuid().ToString();
         

        public CustomerData Data { get => _data; set => _data = value; }
      

        public ResponseData Get(ModelContext db)
        {
            return Customer(db);
        }
        public ResponseData Customer(ModelContext db)
        {
            try
            {
                var user = (from customer in db.Customers
                            where customer.CustId == Data.CustomerId
                            /*|| banksubsidary.BranchId==0*/
                            select new
                            {
                                firmId = customer.FirmId,
                                branchId = customer.BranchId,
                                customerId = customer.CustId,
                                customerName = customer.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),

                            }).SingleOrDefault();
                if (user == null)
                {
                    Log.Error("Customer Not Found");
                    var results = new
                    {
                        Status = "Customer Not Found",
                    };
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //  _Response.data = JsonSerializer.Serialize(results);
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
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<CustomerData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
            var custid = db.Customers.Where(x => x.CustId == Data.CustomerId).ToList();
            if(custid == null)
            {
                FailedValidations.Add(new ApplicationException("invalid customerid"));
            }
            return FailedValidations;
        }
    }
}
