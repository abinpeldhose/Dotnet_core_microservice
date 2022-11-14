//using customerSdmodule.Accounts;
using customerSdmodule.sample;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.RD
{
    public class RdApi : BaseAPIforAccounts
    {

        private RdData _data;
       

        public RdData Data { get => _data; set => _data = value; }
       

        public ResponseData Get(ModelContext_Account db)
        {
            return Recurring(db);
        }
        public ResponseData Recurring(ModelContext_Account db)
        {
            try
            {
                if(Data.Usertype.ToLower() == "employee")
                {
                    var data = (from deposit in db.DepositMsts
                                join branch in db.BranchMasters on deposit.BranchId equals branch.BranchId
                                where deposit.DocId == Data.Depositid && deposit.StatusId.ToString() == "1"
                                select new
                                {
                                    customername = deposit.CustName,
                                    branchId=deposit.BranchId,                               
                                    branchName=branch.BranchName,
                                }).FirstOrDefault();
                    if (data==null)
                    {
                        Log.Error("No Rd");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "No Rd" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "No Rd" });
                        return _Response;
                        // return Results.NotFound(new {Status= "Inavlid RD" });
                    }
                    else
                    {
                        Log.Information("/Sucesss");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(data);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(data);
                        return _Response;
                        //return Results.Ok(data);
                    }
                }
                else if(Data.Usertype.ToLower() == "customer")
                {
                    var data = (from deposit in db.DepositMsts
                                join branch in db.BranchMasters on deposit.BranchId equals branch.BranchId
                                where deposit.DocId == Data.Depositid && deposit.StatusId == 1
                                select new
                                {
                                    customername = deposit.CustName,
                                    branchId = deposit.BranchId,
                                    branchName = branch.BranchName,

                                }).FirstOrDefault();
                    if (data ==null)
                    {
                        Log.Error("No Payment Gateways");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "No Payment Gateways" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "No Payment Gateways" });
                        return _Response;
                        // return Results.NotFound(new {Status="No Payment Gateways"});
                    }
                    else
                    {
                        Log.Information("/Sucesss");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(data);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(data);
                        return _Response;
                        // return Results.Ok(data);
                    }
                }
                else
                {

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Some Issue Found" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "Some Issue Found" });
                    return _Response;
                    // return Results.NotFound("Some Issue Found");
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

                // return Results.BadRequest(message);
            }
        }


        protected override ResponseData OnValidationSuccess(ModelContext_Account db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<RdData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext_Account db)
        {
            List<Exception> Failedvalidation = new List<Exception>();
            if (Data.Depositid == null)
            {
                Failedvalidation.Add(new Exception("Inavlid Input"));
            }           
            return Failedvalidation;
        }
    }
}
