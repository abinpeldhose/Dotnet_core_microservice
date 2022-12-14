using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.SearchCustomer
{
    public class SearchCustomerApi : BaseApi
    {

        private SearchCustomerData _data;

       // private string _jwtToken;
       
      
        public SearchCustomerData Data { get => _data; set => _data = value; }
      //  public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
           return SearchCustomer(db);
        }
        public ResponseData SearchCustomer(ModelContext db)
        {
            try
            {
                if (Data.Type.ToUpper() == "CUSTOMERID")

                {

                    var user = (from u in db.Customers
                                join Branch in db.BranchMasters on u.BranchId equals Branch.BranchId
                                join customerdetails in db.CustomerDetails on u.CustId equals customerdetails.CustId

                                where u.CustId == Data.Search

                                select new
                                {

                                    customerName = u.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                    customerId = u.CustId,
                                    customerAddress = u.HouseName,
                                    firmID = u.FirmId,
                                    branchID = u.BranchId,
                                    cardNumber = u.CardNo,
                                    customerPhone1 = u.Phone1,
                                    customerPhone2 = u.Phone2,
                                    branchName = Branch.BranchName,
                                    dob = customerdetails.DateOfBirth,


                                }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();//ToList();
                    if (user.Count() == 0)
                    {
                        Log.Error("CustomerId Not Found");
                        var results = new
                        {
                            Status = "CustomerId Not Found",
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
                        // _Response.data = JsonSerializer.Serialize(user);
                        return _Response;
                    }


                }

                else if (Data.Type.ToUpper() == "CUSTOMERNAME")
                {
                    // var data = db.Customers.Select(u=> u.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }));
                    var user = (from u in db.Customers
                                where u.CustName.Substring(1).StartsWith(Data.Search.ToUpper())
                                join Branch in db.BranchMasters on u.BranchId equals Branch.BranchId
                                join customerdetails in db.CustomerDetails on u.CustId equals customerdetails.CustId
                                orderby u.CustName

                                select new
                                {

                                    customerName = u.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                    customerId = u.CustId,
                                    customerAddress = u.HouseName,
                                    firmID = u.FirmId,
                                    branchID = u.BranchId,
                                    cardNumber = u.CardNo,
                                    customerPhone1 = u.Phone1,
                                    customerPhone2 = u.Phone2,
                                    branchName = Branch.BranchName,
                                    dob = customerdetails.DateOfBirth,



                                }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();//ToList();

                    if (user.Count() == 0)
                    {
                        Log.Error("CustomerName Not Found");
                        var results = new
                        {
                            Status = "CustomerName Not Found",
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




                else if (Data.Type.ToUpper() == "PHONE")
                {
                    var user = (from u in db.Customers
                                where u.Phone1 == Data.Search || u.Phone2 == Data.Search || u.Phone1 == "0" + Data.Search || u.Phone2 == "0" + Data.Search
                                join Branch in db.BranchMasters on u.BranchId equals Branch.BranchId
                                join customerdetails in db.CustomerDetails on u.CustId equals customerdetails.CustId

                                select new
                                {
                                    customerName = u.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                    customerId = u.CustId,
                                    customerAddress = u.HouseName,
                                    firmID = u.FirmId,
                                    branchID = u.BranchId,
                                    cardNumber = u.CardNo,
                                    customerPhone1 = u.Phone1,
                                    customerPhone2 = u.Phone2,
                                    branchName = Branch.BranchName,
                                    dob = customerdetails.DateOfBirth,
                                }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();//ToList();
                    if (user.Count() == 0)
                    {
                        Log.Error("Phone Not Found");
                        var results = new
                        {
                            Status = "Phone Not Found",
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

                else if (Data.Type.ToUpper() == "PAN")
                {
                    var user = (from customers in db.Customers
                                join Branch in db.BranchMasters on customers.BranchId equals Branch.BranchId
                                join details in db.CustomerDetails on customers.CustId equals details.CustId
                                where details.Pan.EndsWith(Data.Search.ToUpper())

                                select new
                                {
                                    customerName = customers.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                    customerId = customers.CustId,
                                    customerAddress = customers.HouseName,
                                    firmID = customers.FirmId,
                                    branchID = customers.BranchId,
                                    cardNumber = customers.CardNo,
                                    customerPhone1 = customers.Phone1,
                                    customerPhone2 = customers.Phone2,
                                    branchName = Branch.BranchName,
                                    dob = details.DateOfBirth,

                                }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();//ToList();
                    if (user.Count() == 0)
                    {
                        Log.Error("Pan Not Found");
                        var results = new
                        {
                            Status = "Pan Not Found",
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


                else if (Data.Type.ToUpper() == "DOCUMENTNO")
                {
                    var user = (from customers in db.Customers
                                join Branch in db.BranchMasters on customers.BranchId equals Branch.BranchId
                                join master in db.SdMasters on customers.CustId equals master.CustId
                                join details in db.CustomerDetails on customers.CustId equals details.CustId
                                where master.DepositId.EndsWith(Data.Search.ToUpper())

                                select new
                                {
                                    customerName = customers.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                    customerId = customers.CustId,
                                    customerAddress = customers.HouseName,
                                    firmID = master.FirmId,
                                    branchID = master.BranchId,
                                    cardNumber = customers.CardNo,
                                    customerPhone1 = customers.Phone1,
                                    customerPhone2 = customers.Phone2,
                                    branchName = Branch.BranchName,
                                    dob = details.DateOfBirth,
                                }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();//ToList();
                    if (user.Count() == 0)
                    {
                        Log.Error("Document Number not Found");
                        var results = new
                        {
                            Status = "Document Number not Found",
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


                else
                {
                    Log.Warning("BadRequest");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Bad request" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(new {status="bad request"});
                    return _Response;
                }
            }
            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.InnerException.Message);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<SearchCustomerData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
            var cust_id = db.Customers.Where(x => x.CustId == Data.Search).ToList();
            var phone = db.Customers.Where(x => x.Phone1 == Data.Search).ToList();
            var name = db.Customers.Where(x => x.CustName == Data.Search).ToList();
            var pan = db.CustomerDetails.Where(x => x.Pan == Data.Search).ToList();
            var depositid = db.SdMasters.Where(x => x.DepositId == Data.Search).ToList();
            if(cust_id == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Customerid"));
            }
            if (phone == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Phone Number"));
            }
            if (name == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Name"));
            }
            if (pan == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Pan Number"));
            }
            if (depositid == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid DepositId"));
            }
            return FailedValidations;




        }
    }
}
