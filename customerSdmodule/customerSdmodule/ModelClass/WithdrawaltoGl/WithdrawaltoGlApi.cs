using customerSdmodule.sample;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.WithdrawaltoGl
{ 
        public class WithdrwaltoGlApi : BaseAPIforAccounts
        {
            private WithdrawaltoGldata _data;
            
            public WithdrawaltoGldata Data { get => _data; set => _data = value; }
            

            public ResponseData Get(ModelContext_Account db)
            {
                return WithdrawalToGl(db);
            }
            public ResponseData WithdrawalToGl(ModelContext_Account db)
            {
            try
            {
                if (Data.Usertype.ToLower() == "employee")
                {
                    var data = (from pledge in db.PledgeMasters
                                join branch in db.BranchMasters on pledge.BranchId equals branch.BranchId
                                join status in db.PledgeStatuses on pledge.PledgeNo equals status.PledgeNo
                                where pledge.PledgeNo == Data.Pledgeno && status.StatusId == 1
                                select new
                                {
                                    Customername = pledge.CustName,
                                    BranchId = pledge.BranchId,
                                    BranchName = branch.BranchName,

                                }).FirstOrDefault();
                    if (data == null)
                    {
                        Log.Error("Withdrawal to Gl is failed");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Unable to withdraw to Gold Loan" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "Unable to withdraw to Gold Loan" });
                        return _Response;

                        //return Results.NotFound(new {Status="Unable to withdraw to Gold Loan"});
                    }
                    else
                    {
                        Log.Information("/Sucesss");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(data);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(data);
                        return _Response;
                        // return Results.Ok(data);
                    }
                }
                else if (Data.Usertype.ToLower() == "customer")
                {
                    var data = (from pledge in db.PledgeMasters
                                join status in db.PledgeStatuses on pledge.PledgeNo equals status.PledgeNo
                                join branch in db.BranchMasters on pledge.BranchId equals branch.BranchId
                                where pledge.PledgeNo == Data.Pledgeno && status.StatusId == 1
                                select new
                                {
                                    Customername = pledge.CustName,
                                    BranchId = pledge.BranchId,
                                    BranchName = branch.BranchName,

                                }).FirstOrDefault();
                    if (data == null)
                    {

                        Log.Error("Withdrawal to Gl is failed");
                        // return Results.NotFound(new {Status= "Unable to withdraw to Gold loan" });


                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Unable to withdraw to Gold loan" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(new { status = "Unable to withdraw to Gold loan" });
                        return _Response;

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
                    }
                }
                else
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Some issue found" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "Some issue found" });
                    return _Response;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "Somethings went wrong" });
                return _Response;
            }
            }

            protected override List<Exception> CustomisedValidate(ModelContext_Account db)
            {
                List<Exception> Failedvalidation = new List<Exception>();
                if (Data.Pledgeno == null)
                {
                    Failedvalidation.Add(new Exception("Inavlid Input"));
                }
                return Failedvalidation;
            }
        }
    }

