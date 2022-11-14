using customerSdmodule.Model1;

using static RedisCacheDemo.RedisCacheStore;
using RedisCacheDemo;
using customerSdmodule.ModelClass.AccountSummary;
using customerSdmodule.ModelClass.CustomerAcccounts;
using customerSdmodule;
using customerSdmodule.ModelClass.ReportCompany;
using customerSdmodule.ModelClass.ReportBranch;
using customerSdmodule.ModelClass.CustomerPhoneNumberForget;
using System.Text.Json;
using customerSdmodule.ModelClass.ForgetPassword;
using customerSdmodule.ModelClass.AccountDetails;
using customerSdmodule.ModelClass.AgentEmployee;
using customerSdmodule.ModelClass.Notifications;
using customerSdmodule.ModelClass.WithdrawalTo;
using customerSdmodule.ModelClass.SetMpin;
using customerSdmodule.ModelClass.VerifyOtp;
using customerSdmodule.ModelClass.SearchCustomer;
using customerSdmodule.ModelClass.Registration;
using customerSdmodule.ModelClass.IFSC;
using customerSdmodule.ModelClass.CustomerToAccounts;
using customerSdmodule.ModelClass.CustomerCustomerId;
using customerSdmodule.ModelClass.SubsidaryBank;
using customerSdmodule.ModelClass.Splash;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using customerSdmodule.ModelClass.PaymentGatewayMaster;
using customerSdmodule.ModelClass.forgetpassword;
using customerSdmodule.ModelClass.DateFormat;
using customerSdmodule.ModelClass.Login;
using Serilog;
using customerSdmodule.ModelClass.BHApprove;
using customerSdmodule.ModelClass.CheckMobileNumber;
using customerSdmodule.ModelClass.CheckPassword;
using customerSdmodule.ModelClass.Deposit;
using customerSdmodule.ModelClass;
using customerSdmodule.ModelClass.Settlement;
using customerSdmodule.ModelClass.NewSD;
using customerSdmodule.ModelClass.StatementTransactionDetails;
using customerSdmodule.ModelClass.StatementDetails;
using customerSdmodule.ModelClass.RemoveNotification;
using customerSdmodule.ModelClass.GetCustomerDetails;
using customerSdmodule.ModelClass.withdrawal;
using customerSdmodule.ModelClass.Login1;
using customerSdmodule.ModelClass.Validateuserid;
using customerSdmodule.ModelClass.EligibleSchemes;
using customerSdmodule.ModelClass.DeleteNTransaction;
using customerSdmodule.ModelClass.GetscheduledTransactions;
using customerSdmodule.ModelClass.GetCustomerPhone;
using customerSdmodule.ModelClass.LoginMpin;
using customerSdmodule.ModelClass.Getbalancesample;
using customerSdmodule.ModelClass.GetNotes;
using customerSdmodule.ModelClass.DeleteNotes;
using customerSdmodule.ModelClass.AddNotes;
using customerSdmodule.ModelClass.DeleteBH;
using customerSdmodule.ModelClass.BHGroup;
using customerSdmodule.ModelClass.ChequeEmployeeVerify;
using customerSdmodule.ModelClass.PutEmployeeBounceCheque;
using customerSdmodule.ModelClass.CoapplicantRights;
using customerSdmodule.ModelClass.ChequeReconsilation;
using customerSdmodule.ModelClass.SortedBHAprrove;
using customerSdmodule.ModelClass.SortedBHCheque;
using customerSdmodule.ModelClass.GetBhVerification;
using customerSdmodule.ModelClass.Relationships;
using customerSdmodule.ModelClass.PutBHBounceCheque;
using customerSdmodule.ModelClass.ReturnCheque;
using customerSdmodule.ModelClass.SortedBounceCheques;
using customerSdmodule.sample;
using customerSdmodule.ModelClass.RD;
using customerSdmodule.ModelClass.WithdrawaltoGl;
using customerSdmodule.ModelClass.LogOut;
using customerSdmodule.ModelClass.employeelogin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Syslog("127.0.0.1", 514, AppDomain.CurrentDomain.FriendlyName)
    .CreateLogger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ModelContext>();
builder.Services.AddDbContext<ModelContext_Account>();
var app = builder.Build();
Security hash = new Security();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// CacheService _cacheservice = CacheService();
app.MapPost("Sample", async (ModelContext db,string uni) =>
{
    //RedisRun.Set(", "hello");
    return Results.Ok(RedisRun.Get(uni, null));
});

#region adminApi's

app.MapPost("newregistration", async (ModelContext db,byte FirmId,int BranchId,string Id,string CustomerId,string UserId,string Password,string Phone) =>
{
    try
    {
        var data = new UserLoginMst1
        {
            FirmId = FirmId,
            BranchId = BranchId,
            Id = Id,
            Custid = CustomerId,
            UserId = UserId,
            Password = hash.create_hashs(Password, DateFunctions.sysdate(db).ToString("yyyyMMdd")),
            Phone = Phone,
            RegistartionDate = DateFunctions.sysdate(db),
            PasswordUpdateDate = DateFunctions.sysdate(db),
            MaxDay = 30,
            PasswordRules = 1,
            Status = 1,
            Appwebstatus = "1",
        };
        db.UserLoginMst1s.Add(data);
        db.SaveChanges();
        Log.Information("Success");
        return Results.Ok(new { status = "Success" });
    }
    catch(Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
   

}).WithTags("Admin");
#endregion



app.MapPut("/Changeonly_password", async (ModelContext db, string userId,string password) =>
{
    var loginDetails = db.UserLoginMst1s.FirstOrDefault(x => x.UserId==userId);
    loginDetails.Password = hash.create_hashs(password, loginDetails.RegistartionDate.ToString("yyyyMMdd"));
    loginDetails.PasswordUpdateDate = DateFunctions.sysdate(db);
    db.SaveChanges();
    if(loginDetails!=null)
    {
        loginDetails.Password = hash.create_hashs(password, loginDetails.RegistartionDate.ToString("yyyyMMdd"));
        loginDetails.PasswordUpdateDate = DateFunctions.sysdate(db);
        db.SaveChanges();
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();
    }
   // hash.create_hashs(_log.Password, Register.RegistartionDate.ToString("yyyyMMdd")
}).WithTags("Admin").WithName("Change Passwword Directly");


#region Login
app.MapPost("splash/ApplicationDetails", async (ModelContext db,string RequestJson) =>
{

    try
    {

        var request = JsonSerializer.Deserialize<ApplicationRequest>(RequestJson);
        ApplicationRequest request2 = new ApplicationRequest();

        if (request.Type == ApplicationRequest.Requesttype && request.Ver == (decimal)1.0)
        {
            request2 = JsonSerializer.Deserialize<ApplicationRequest>(RequestJson);
            AppicationAPI appdetail = request2.ApplicationData;
            List<Exception> FailedValidations = appdetail.Validate(db);
            if (FailedValidations.Count > 0)
            {
                StringBuilder ErrBuilder = new StringBuilder();
                ErrBuilder.AppendLine("Get Employee Details is  failed because of following reasons");
                foreach (Exception Validation in FailedValidations)
                {
                    ErrBuilder.AppendLine(Validation.Message);
                }
                string ErrorMsg = ErrBuilder.ToString();

                Console.WriteLine(ErrorMsg);
                // Log.Error(ErrorMsg);
                return Results.BadRequest(ErrorMsg);
            }
            else
            {
                //var details = appdetail.Get(db);

                Log.Information("success");
                return appdetail.Get(db);
            }
        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        
            return Results.BadRequest(new { status = "something went wrong" });
        

    }


}).WithTags("Completed");//Jwt token is included
//app.MapPost("splash/application", async (ModelContext db, string RequestJson) =>
//{
//try
//{

//    Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

//    ApplicationRequest request2 = new ApplicationRequest();

//    if (_request.Type == ApplicationRequest.Requesttype && _request.Ver == (decimal)1.0)
//    {
//        request2 = JsonSerializer.Deserialize<ApplicationRequest>(RequestJson);

//        AppicationAPI _approval = new AppicationAPI();
//        //_approval.JwtToken = _bhapproverequest.JwtToken;// _AccountDetailrequest.JwtToken;
//        _approval.Hash = request2.Hash;
//        //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
//        _approval.Application = request2.ApplicationData.Application;

//        return _approval.Validate(db);

//    }
//    else
//    {
//        //  Log.Warning("Something Went Wrong");
//        return Results.BadRequest("Some Issues Found");
//    }

//}
//catch (Exception ex)
//{
//    var message = new { Status = "something went wrong" };
//    // Log.Error(ex.Message);
//    return Results.BadRequest(ex.Message);
//}


//});

app.MapPost("/sendOtp/forgetPassword", async (ModelContext db, string RequestJson) =>
{
   
    
    var request = JsonSerializer.Deserialize<ForgetOTPRequest>(RequestJson);
    var uniqueKey = TokenManager.TokenManagement.Extract(request.JwtToken);
    var result = JsonSerializer.Deserialize<customerSdmodule.Redis.CacheData>(RedisRun.Get(uniqueKey, null));
    if (result.DeviceId == TokenManager.TokenManagement.ValidateToken(request.JwtToken))
    {
        ForgetOTPRequest request2 = new ForgetOTPRequest();

        if (request.Type == ForgetOTPRequest.Requesttype && request.Ver == (decimal)1.0)
        {
            request2 = JsonSerializer.Deserialize<ForgetOTPRequest>(RequestJson);
            ForgetOTPAPI appdetail = request2.passwordData;

            List<Exception> FailedValidations = appdetail.Validate(db);
            if (FailedValidations.Count > 0)
            {
                StringBuilder ErrBuilder = new StringBuilder();
                ErrBuilder.AppendLine("Get Employee Details is  failed because of following reasons");
                foreach (Exception Validation in FailedValidations)
                {
                    ErrBuilder.AppendLine(Validation.Message);
                }
                string ErrorMsg = ErrBuilder.ToString();

                Console.WriteLine(ErrorMsg);
                // Log.Error(ErrorMsg);
                return Results.BadRequest(ErrorMsg);

            }
            else
            {              


                return appdetail.Get(db);
            }
        }
        else
        {

            return Results.NotFound(new { status = "request is not valid" });
        }
    }
    else
    {
        return Results.NotFound(new {status="Token is not valid"});
    }
       
}).WithTags("Before Login"); //Completed token


app.MapGet("/Employeelogin", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        EmployeeLoginRequest _loginrequest = new EmployeeLoginRequest();

        if (_request.Type == EmployeeLoginRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _loginrequest = JsonSerializer.Deserialize<EmployeeLoginRequest>(RequestJson);

            EmployeeLoginApi _login = new EmployeeLoginApi();
            _login.JwtToken = _loginrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _login.Hash = _loginrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _login.Data = _loginrequest.Data.Data;
            return _login.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("Completed");

app.MapGet("/Authendicate", async (ModelContext db, string RequestJson) =>
{
   
    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        LoginRequest1 _loginrequest = new LoginRequest1();

        if (_request.Type == LoginRequest1.Requesttype && _request.Ver == (decimal)1.0)
        {
            _loginrequest = JsonSerializer.Deserialize<LoginRequest1>(RequestJson);

            Login1 _login = new Login1();
            _login.JwtToken = _loginrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _login.Hash = _loginrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _login.LoginDetails = _loginrequest.Data.LoginDetails;

            return _login.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }
}).WithName("Login API'S").WithTags("Completed"); //Jwt token is included //baseapi


app.MapGet("/Login", async (ModelContext db, string RequestJson) =>
{

    try
    {
       
        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);
       

            LoginRequest _loginrequest = new LoginRequest();

        if (_request.Type == LoginRequest.Requesttype & _request.Ver == (decimal)1.0)
        {
            _loginrequest = JsonSerializer.Deserialize<LoginRequest>(RequestJson);
            if ("12345" == TokenManager.TokenManagement.ValidateToken(_loginrequest.JwtToken))
            {
                LoginApI _login = _loginrequest.Data;
                // LoginApI _tocken = new LoginApI(_loginrequest.JwtToken);
                _login.JwtToken = _loginrequest.JwtToken;


                List<Exception> FailedValidations = _login.Validate(db);
                if (FailedValidations.Count > 0)
                {
                    StringBuilder ErrBuilder = new StringBuilder();
                    ErrBuilder.AppendLine("Login is  failed because of following reasons");
                    foreach (Exception Validation in FailedValidations)
                    {
                        ErrBuilder.AppendLine(Validation.Message);
                    }
                    string ErrorMsg = ErrBuilder.ToString();

                    Console.WriteLine(ErrorMsg);
                    //  Log.Error(ErrorMsg);
                    return Results.BadRequest(ErrorMsg);
                }
                else
                {

                    Log.Information("Login Completed Successfully");
                    return _login.Get(db);//();
                }
            }
            else
            {
               return Results.BadRequest(new { status = "invalid token" });
            }          


        }
        else
        {
          //  Log.Warning("Something Went Wrong");
            return Results.BadRequest("Some Issues Found");
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        return Results.BadRequest(new { status = "something is went wrong" });
    }

}).WithTags(" sample Login");  //not introduce token yet.

app.MapGet("/LogOut", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        LogOutRequest logOutRequest = new LogOutRequest();

        if (_request.Type == LogOutRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            logOutRequest = JsonSerializer.Deserialize<LogOutRequest>(RequestJson);
            LogOutAPI _logout = new LogOutAPI();
            _logout.JwtToken = logOutRequest.JwtToken;
            _logout.Hash = logOutRequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            //    _getcustomerDetails.sortedbouncecheque = _bounce.Data;

            return _logout.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("not check");//Jwt Token included

#endregion


#region BranchHead


app.MapGet("/sortedBouncecheque", async (ModelContext db, string RequestJson) =>
{
    

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        SortedbounceChequeRequest _bounce = new SortedbounceChequeRequest();

        if (_request.Type == SortedbounceChequeRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _bounce = JsonSerializer.Deserialize<SortedbounceChequeRequest>(RequestJson);
            SortedbouceChequeApi _getcustomerDetails = new SortedbouceChequeApi();
            _getcustomerDetails.JwtToken = _bounce.JwtToken;
            _getcustomerDetails.Hash = _bounce.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            //    _getcustomerDetails.sortedbouncecheque = _bounce.Data;

            return _getcustomerDetails.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("completed");//Jwt Token included  ///must be discuss



app.MapGet("/sortedBHcheque", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        SortedBHChequeRequest _bounce = new SortedBHChequeRequest();

        if (_request.Type == SortedBHChequeRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _bounce = JsonSerializer.Deserialize<SortedBHChequeRequest>(RequestJson);
            SortedBHChequeAPI _getcustomerDetails = new SortedBHChequeAPI();
            _getcustomerDetails.JwtToken = _bounce.JwtToken;
            _getcustomerDetails.Hash = _bounce.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            //    _getcustomerDetails.sortedbouncecheque = _bounce.Data;

            return _getcustomerDetails.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("not check");//Jwt Token included





app.MapGet("/sortedBhApprove", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        SortedBHApproveRequest _sorted = new SortedBHApproveRequest();

        if (_request.Type == SortedBHChequeRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _sorted = JsonSerializer.Deserialize<SortedBHApproveRequest>(RequestJson);
            SortedBHApproveAPI sortedBH = new SortedBHApproveAPI();
            sortedBH.JwtToken = _sorted.JwtToken;
            sortedBH.Hash = _sorted.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            //    _getcustomerDetails.sortedbouncecheque = _bounce.Data;

            return sortedBH.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }
    //try
    //{
    //    Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

    //    SortedBHApproveRequest _sorted = new SortedBHApproveRequest();

    //    if (_request.Type == SortedBHApproveRequest.Requesttype & _request.Ver == (decimal)1.0)
    //    {
    //        _sorted = JsonSerializer.Deserialize<SortedBHApproveRequest>(RequestJson);
    //        var uniqueKey = TokenManager.TokenManagement.Extract(_sorted.JwtToken);
    //        var cache = JsonSerializer.Deserialize<customerSdmodule.Redis.CacheData>(RedisRun.Get(uniqueKey, null));

    //        if (cache.UserId == TokenManager.TokenManagement.ValidateToken(_sorted.JwtToken))
    //        {

    //            SortedBHApproveAPI sortedBH = new SortedBHApproveAPI();
    //            _request.JwtToken = _sorted.JwtToken;

    //            List<Exception> FailedValidations = sortedBH.Validate(db);
    //            if (FailedValidations.Count > 0)
    //            {
    //                StringBuilder ErrBuilder = new StringBuilder();
    //                ErrBuilder.AppendLine("Get BhApprove is  failed because of following reasons");
    //                foreach (Exception Validation in FailedValidations)
    //                {
    //                    ErrBuilder.AppendLine(Validation.Message);
    //                }
    //                string ErrorMsg = ErrBuilder.ToString();

    //                Console.WriteLine(ErrorMsg);
    //                Log.Error(ErrorMsg);
    //                return Results.BadRequest(ErrorMsg);
    //            }
    //            else
    //            {

    //                //  Log.Information("Get Customer Details Successfully");
    //                // return Results.Ok("kjcs");
    //                return sortedBH.Get(db);
    //            }
    //        }
    //        else
    //        {
    //            Log.Warning("invalid request");
    //            return Results.NotFound(new { status = "invalid request" });
    //        }



    //    }
    //    else
    //    {
    //        Log.Warning("Something Went Wrong");
    //        return Results.BadRequest("Some Issues Found");
    //    }

    //}
    //catch (Exception ex)
    //{
    //    Log.Error(ex.Message);
    //    return Results.BadRequest(new { status = "something is went wrong" });
    //}

}).WithTags("no data");//Jwt Token included






app.MapPut("PutBhApprove", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        BHApproveRequest _bhapproverequest = new BHApproveRequest();

        if (_request.Type == BHApproveRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _bhapproverequest = JsonSerializer.Deserialize<BHApproveRequest>(RequestJson);

            BHApproveAPI _approval = new BHApproveAPI();
            _approval.JwtToken = _bhapproverequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _approval.Hash = _bhapproverequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _approval.Data = _bhapproverequest.Data.Data;

            return _approval.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }
}).WithName("update Bh approve").WithTags("Completed");//Jwt Token included  //baseapi


app.MapPut("/DeleteBHScheduledTransactions/Ntransactions", async (ModelContext db, String RequestJson) =>
{

    try
    {
        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        DeleteBHRequest _deletebhrequest = new DeleteBHRequest();

        if (_request.Type == DeleteBHRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _deletebhrequest = JsonSerializer.Deserialize<DeleteBHRequest>(RequestJson);

            DeleteBHAPI _cancel = new DeleteBHAPI();
            _cancel.JwtToken = _deletebhrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _cancel.Hash = _deletebhrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _cancel.Data = _deletebhrequest.Data.Data;

            return _cancel.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("Completed");//Jwt Token included  //baseapi


app.MapGet("/GetBHScheduledTransactions/groups", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        BHGroupRequest _groupbhrequest = new BHGroupRequest();

        if (_request.Type == SortedBHChequeRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _groupbhrequest = JsonSerializer.Deserialize<BHGroupRequest>(RequestJson);
            BHGroupAPI _cancel = new BHGroupAPI();
            _cancel.JwtToken = _groupbhrequest.JwtToken;
            _cancel.Hash = _groupbhrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            //    _getcustomerDetails.sortedbouncecheque = _bounce.Data;

            return _cancel.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }
}).WithTags("not check");//Jwt Token included




#endregion

app.MapGet("/CheckMobileNumber", async (ModelContext db, string RequestJson) =>
{
    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        CheckMobileNumberRequest _mobilenumberRequest = new CheckMobileNumberRequest();

        if (_request.Type == CheckMobileNumberRequest.Requesttype & _request.Ver == (decimal)1.0)
        {
            _mobilenumberRequest = JsonSerializer.Deserialize<CheckMobileNumberRequest>(RequestJson);
            if ("12345" == TokenManager.TokenManagement.ValidateToken(_mobilenumberRequest.JwtToken))
            {
                CheckMobileNumberAPI _checkmobilenumber = _mobilenumberRequest.Data;

                List<Exception> FailedValidations = _checkmobilenumber.Validate(db);
                if (FailedValidations.Count > 0)
                {
                    StringBuilder ErrBuilder = new StringBuilder();
                    ErrBuilder.AppendLine("Check Mobile Number is  failed because of following reasons");
                    foreach (Exception Validation in FailedValidations)
                    {
                        ErrBuilder.AppendLine(Validation.Message);
                    }
                    string ErrorMsg = ErrBuilder.ToString();

                    Console.WriteLine(ErrorMsg);
                    Log.Error(ErrorMsg);
                    return Results.BadRequest(ErrorMsg);
                }
                else
                {
                   // Log.Information("Mobile Number Checked Successfully");
                    return _checkmobilenumber.Get(db);
                }
            }
            else
            {
                return Results.BadRequest(new { status = "Token is not valid" });
            }           


        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("exclude");



app.MapGet("/checkPassword", async (ModelContext db, string RequestJson) =>
{
    try
    {
        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        CheckPasswordRequest _checkrequest = new CheckPasswordRequest();

        if (_request.Type == CheckPasswordRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            if ("12345" == TokenManager.TokenManagement.ValidateToken(_checkrequest.JwtToken))
            {


                CheckPasswordAPI _check = _checkrequest.Data;
                _checkrequest = JsonSerializer.Deserialize<CheckPasswordRequest>(RequestJson);
                List<Exception> FailedValidations = _check.Validate(db);
                if (FailedValidations.Count > 0)
                {
                    StringBuilder ErrBuilder = new StringBuilder();
                    ErrBuilder.AppendLine("Check Password is  failed because of following reasons");
                    foreach (Exception Validation in FailedValidations)
                    {
                        ErrBuilder.AppendLine(Validation.Message);
                    }
                    string ErrorMsg = ErrBuilder.ToString();

                    Log.Error(ErrorMsg);
                    return Results.BadRequest(ErrorMsg);
                }
                else
                {
                    Log.Information("Check Password Successfully");
                    return _check.Get(db);
                }
            }
            else
            {
                return Results.BadRequest(new {status="Token is not valid"});
            }
                



        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        return Results.BadRequest(new { status = "something is went wrong" });
    }

}).WithTags("exclude");



app.MapPost("/PostDeposit", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        DepositRequest _depositRequest = new DepositRequest();

        if (_request.Type == DepositRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _depositRequest = JsonSerializer.Deserialize<DepositRequest>(RequestJson);

            DepositApi _deposit = new DepositApi();
            _deposit.JwtToken = _depositRequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _deposit.Hash = _depositRequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _deposit.Data = _depositRequest.Data.Data;

            return _deposit.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("Completed");  //Jwt Token included



app.MapPut("Settlement", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        SettlementRequest _settlementrequest = new SettlementRequest();

        if (_request.Type == DepositRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _settlementrequest = JsonSerializer.Deserialize<SettlementRequest>(RequestJson);

            SettlementAPI _settle = new SettlementAPI();
            _settle.JwtToken = _settlementrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _settle.Hash = _settlementrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _settle.Data = _settlementrequest.Data.Data;

            return _settle.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("Completed");//Jwt Token included

app.MapPost("/PostAccountOpeningFinal", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        NewSDRequest _settlementrequest = new NewSDRequest();

        if (_request.Type == DepositRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _settlementrequest = JsonSerializer.Deserialize<NewSDRequest>(RequestJson);

            NewSDApi _settle = new NewSDApi();
            _settle.JwtToken = _settlementrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _settle.Hash = _settlementrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _settle.Data = _settlementrequest.Data.Data;

            return _settle.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("Completed");//JwtToken Included


app.MapGet("/SubsidaryBanks", async (ModelContext db, string RequestJson) =>
{


    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        SubsidaryBankRequest _subsidaryrequest = new SubsidaryBankRequest();

        if (_request.Type == SubsidaryBankRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _subsidaryrequest = JsonSerializer.Deserialize<SubsidaryBankRequest>(RequestJson);

            SubsidaryBankApi _subsidary = new SubsidaryBankApi();
            _subsidary.JwtToken = _subsidaryrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _subsidary.Hash = _subsidaryrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _subsidary.Data = _subsidaryrequest.Data.Data;

            return _subsidary.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("Completed");//Jwt is included

app.MapGet("/GetStatementTransatctionDetails1", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        StatementTransactionDetailsRequest _transactionrequest = new StatementTransactionDetailsRequest();

        if (_request.Type == StatementTransactionDetailsRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _transactionrequest = JsonSerializer.Deserialize<StatementTransactionDetailsRequest>(RequestJson);

            StatementTransatctionDetailsApi _statements = new StatementTransatctionDetailsApi();
            _statements.JwtToken = _transactionrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _statements.Hash = _transactionrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _statements.Data = _transactionrequest.Data.Data;

            return _statements.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }




}).WithTags("not include, completed"); //Jwt is included  


app.MapGet("/GetCustomerAccounts", async (ModelContext db, string RequestJson) =>
{
    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        CustomerAccountsRequest _customerAccountrequest = new CustomerAccountsRequest();

        if (_request.Type == CustomerAccountsRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _customerAccountrequest = JsonSerializer.Deserialize<CustomerAccountsRequest>(RequestJson);

            CustomerAccountsAPi _login = new CustomerAccountsAPi();
            _login.JwtToken = _customerAccountrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _login.Hash = _customerAccountrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _login.Data = _customerAccountrequest.Data.Data;
            
            return _login.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("Completed");  //jwt is included


app.MapGet("/Accountsummary", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        AccountSummaryRequest _accountsummaryRequest = new AccountSummaryRequest();

        if (_request.Type == AccountSummaryRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _accountsummaryRequest = JsonSerializer.Deserialize<AccountSummaryRequest>(RequestJson);

            AccountSummaryApi _accountsummary = new AccountSummaryApi();
            _accountsummary.JwtToken = _accountsummaryRequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _accountsummary.Hash = _accountsummaryRequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _accountsummary.Data = _accountsummaryRequest.Data.Data;

            return _accountsummary.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("Completed");  //Jwt is included

app.MapGet("/IFSCserach", async (ModelContext db, string RequestJson) =>
    {


        try
        {

            Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

            IfscRequest _ifscRequest = new IfscRequest();

            if (_request.Type == IfscRequest.Requesttype && _request.Ver == (decimal)1.0)
            {
                _ifscRequest = JsonSerializer.Deserialize<IfscRequest>(RequestJson);

                IfscApi _ifsc = new IfscApi();
                _ifsc.JwtToken = _ifscRequest.JwtToken;// _AccountDetailrequest.JwtToken;
                _ifsc.Hash = _ifscRequest.Hash;
                //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
                _ifsc.Data = _ifscRequest.Data.Data;

                return _ifsc.Validate(db);

            }
            else
            {
                Log.Warning("invalid request");
                return Results.NotFound(new { status = "invalid request" });
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            if (ex.Message == "A null key is not valid in this context")
            {
                return Results.UnprocessableEntity(new { status = "session timeout" });
            }
            else
            {
                return Results.BadRequest(new { status = "something went wrong" });
            }

        }



    }).WithTags("Completed"); //Jwt Token is included



app.MapGet("/Notifications", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        NotificationRequest _notificationrequest = new NotificationRequest();

        if (_request.Type == NotificationRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _notificationrequest = JsonSerializer.Deserialize<NotificationRequest>(RequestJson);

            NotificationApi _accountsummary = new NotificationApi();
            _accountsummary.JwtToken = _notificationrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _accountsummary.Hash = _notificationrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _accountsummary.Data = _notificationrequest.Data.Data;

            return _accountsummary.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }
}).WithTags("not check"); //jwt Token is included

/////////////////////////////////////////////////////

app.MapGet("/Customer/CustomerId", async (ModelContext db, string RequestJson) =>
{


    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        CustomerRequest _customerrequest = new CustomerRequest();

        if (_request.Type == CustomerRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _customerrequest = JsonSerializer.Deserialize<CustomerRequest>(RequestJson);

            CustomerApi _customer = new CustomerApi();
            _customer.JwtToken = _customerrequest.JwtToken;
            _customer.Hash = _customerrequest.Hash;

            _customer.Data = _customerrequest.Data.Data;

            return _customer.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("not check");//Jwt Token Included



app.MapGet("/CoApplicantRights", async (ModelContext db,string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        CoapplicantRequest _customerrequest = new CoapplicantRequest();

        if (_request.Type == CoapplicantRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _customerrequest = JsonSerializer.Deserialize<CoapplicantRequest>(RequestJson);

            CoapplicantAPI _getcustomerDetails = new CoapplicantAPI();
            _getcustomerDetails.JwtToken = _customerrequest.JwtToken;
            _getcustomerDetails.Hash = _customerrequest.Hash;         

            return _getcustomerDetails.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }




}).WithTags("not check");//Jwt token is included

app.MapGet("/GetStatementTransatctionDetails", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        StatementTransactionDetailsRequest _transactionrequest = new StatementTransactionDetailsRequest();

        if (_request.Type == StatementTransactionDetailsRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _transactionrequest = JsonSerializer.Deserialize<StatementTransactionDetailsRequest>(RequestJson);

            StatementTransatctionDetailsApi _statements = new StatementTransatctionDetailsApi();
            _statements.JwtToken = _transactionrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _statements.Hash = _transactionrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _statements.Data = _transactionrequest.Data.Data;

            return _statements.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("Completed"); //Jwt token is included


app.MapGet("/GetcustomerDetails", async (ModelContext db, string RequestJson) =>
{


    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        GetCustomerDetailsRequest _customer = new GetCustomerDetailsRequest();

        if (_request.Type == GetCustomerDetailsRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _customer = JsonSerializer.Deserialize<GetCustomerDetailsRequest>(RequestJson);

            GetCustomerDetailsApi _getcustomerDetails = new GetCustomerDetailsApi();
            _getcustomerDetails.JwtToken = _customer.JwtToken;// _AccountDetailrequest.JwtToken;
            _getcustomerDetails.Hash = _customer.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _getcustomerDetails.Data = _customer.Data.Data;

            return _getcustomerDetails.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check");//Jwt Token is included


app.MapPut("clearNotification", async (ModelContext db, String RequestJson) =>
{


    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        RemoveNotificationRequest _clearnotificationrequest = new RemoveNotificationRequest();

        if (_request.Type == RemoveNotificationRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _clearnotificationrequest = JsonSerializer.Deserialize<RemoveNotificationRequest>(RequestJson);

            RemoveNotificationApi _clear = new RemoveNotificationApi();
            _clear.JwtToken = _clearnotificationrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _clear.Hash = _clearnotificationrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _clear.Data = _clearnotificationrequest.Data.Data;

            return _clear.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("not check");//Jwt token is included


app.MapGet("/GetStatementDetails", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        StatementDetailsRequest _statementRequest = new StatementDetailsRequest();

        if (_request.Type == StatementDetailsRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _statementRequest = JsonSerializer.Deserialize<StatementDetailsRequest>(RequestJson);

            StatementDetailsApi _statement = new StatementDetailsApi();
            _statement.JwtToken = _statementRequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _statement.Hash = _statementRequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _statement.Data = _statementRequest.Data.Data;

            return _statement.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("not check");//Jwt token is included





#region withdrawal end points

app.MapPost("/Withdrawal", async (ModelContext db, string RequestJson) =>
{


    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        WithdrawalRequest _withdrawalRequest = new WithdrawalRequest();

        if (_request.Type == WithdrawalRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _withdrawalRequest = JsonSerializer.Deserialize<WithdrawalRequest>(RequestJson);

            WithdrawalAPI _withdrawalDetails = new WithdrawalAPI();
            _withdrawalDetails.JwtToken = _withdrawalRequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _withdrawalDetails.Hash = _withdrawalRequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _withdrawalDetails.WithdrawalDetails = _withdrawalRequest.Withdrawal.WithdrawalDetails;


            return _withdrawalDetails.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        return Results.BadRequest(new { status = "something went wrong" });
    }
   

}).WithTags("not check");//Jwt token is included


app.MapGet("WithdrawalToRD", async (ModelContext_Account db, string RequestJson) =>
{


    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        RDRequest _Rd = new RDRequest();

        if (_request.Type == RDRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _Rd = JsonSerializer.Deserialize<RDRequest>(RequestJson);

            RdApi _Recurring = new RdApi();
            _Recurring.JwtToken = _Rd.JwtToken;// _AccountDetailrequest.JwtToken;
            _Recurring.Hash = _Rd.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _Recurring.Data = _Rd.Data.Data;


            return _Recurring.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        return Results.BadRequest(new { status = "something went wrong" });
    }
   

}).WithTags("not check");//Jwt Token is included


app.MapGet("WithdrawaltoGl", async (ModelContext_Account db, string RequestJson) =>
{


    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        WithdrawaltoGlRequest _Rd = new WithdrawaltoGlRequest();

        if (_request.Type == WithdrawaltoGlRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _Rd = JsonSerializer.Deserialize<WithdrawaltoGlRequest>(RequestJson);

            WithdrwaltoGlApi _Recurring = new WithdrwaltoGlApi();
            _Recurring.JwtToken = _Rd.JwtToken;// _AccountDetailrequest.JwtToken;
            _Recurring.Hash = _Rd.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _Recurring.Data = _Rd.Data.Data;


            return _Recurring.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        return Results.BadRequest(new { status = "something went wrong" });
    }


   

}).WithTags("not check");//Jwt token is included

#endregion



app.MapGet("/SearchAgent/Employee", async (ModelContext db, string RequestJson) =>
{


    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        AgentEmployeeRequest _agentrequest = new AgentEmployeeRequest();

        if (_request.Type == AgentEmployeeRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _agentrequest = JsonSerializer.Deserialize<AgentEmployeeRequest>(RequestJson);

            AgentEmployeeApi _agent = new AgentEmployeeApi();
            _agent.JwtToken = _agentrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _agent.Hash = _agentrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _agent.Data = _agentrequest.Data.Data;


            return _agent.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("not check");//Jwt token is included


app.MapGet("/GetAccountMoreInfo", async (ModelContext db, string RequestJson) =>
{
    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        AccountdetailsRequest _AccountDetailrequest = new AccountdetailsRequest();
       
        if (_request.Type == AccountdetailsRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _AccountDetailrequest = JsonSerializer.Deserialize<AccountdetailsRequest>(RequestJson);
           
                AccountdetailsApi _account = new AccountdetailsApi();
                _account.JwtToken = _AccountDetailrequest.JwtToken;// _AccountDetailrequest.JwtToken;
                _account.Hash = _AccountDetailrequest.Hash;
                //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
                _account.Data = _AccountDetailrequest.Data.Data;

                return _account.Validate(db);         

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check");//Jwt token is included


app.MapGet("/GetPaymentGetwayMaster", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        PaymentGatewayRequest _paymentrequest = new PaymentGatewayRequest();

        if (_request.Type == PaymentGatewayRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _paymentrequest = JsonSerializer.Deserialize<PaymentGatewayRequest>(RequestJson);

            PaymentGatewayApi _payment = new PaymentGatewayApi();
            _payment.JwtToken = _paymentrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _payment.Hash = _paymentrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _payment.Data = _paymentrequest.Data.Data;

            return _payment.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check");//Jwt token is included

app.MapGet("/GetEligibleSchemes", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        GetEligibleRequest _schemes = new GetEligibleRequest();

        if (_request.Type == GetEligibleRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _schemes = JsonSerializer.Deserialize<GetEligibleRequest>(RequestJson);

            GetEligibleApi _geteligibleschemes = new GetEligibleApi();
            _geteligibleschemes.JwtToken = _schemes.JwtToken;// _AccountDetailrequest.JwtToken;
            _geteligibleschemes.Hash = _schemes.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _geteligibleschemes.Data = _schemes.Data.Data;

            return _geteligibleschemes.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check");//Jwt token is included

app.MapGet("/Validateuserid", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        ValidateUserIdRequest _userrequest = new ValidateUserIdRequest();

        if (_request.Type == ValidateUserIdRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _userrequest = JsonSerializer.Deserialize<ValidateUserIdRequest>(RequestJson);

            ValidateUserIdApi _user = new ValidateUserIdApi();
            _user.JwtToken = _userrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _user.Hash = _userrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _user.Data = _userrequest.Data.Data;

            return _user.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check");//Jwt token is included


app.MapGet("/CustomerTOaccounts", async (ModelContext db, string RequestJson) =>
{


    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        CustomertoAccountsRequest _customerequest = new CustomertoAccountsRequest();

        if (_request.Type == CustomertoAccountsRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _customerequest = JsonSerializer.Deserialize<CustomertoAccountsRequest>(RequestJson);

            CustomerToAccountsApi _customer = new CustomerToAccountsApi();
            _customer.JwtToken = _customerequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _customer.Hash = _customerequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _customer.Data = _customerequest.Data.Data;

            return _customer.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check"); //Jwt token is included

app.MapPut("/DeleteScheduledTransactions/Ntransactions", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        DeleteNtransactionRequest _userrequest = new DeleteNtransactionRequest();

        if (_request.Type == DeleteNtransactionRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _userrequest = JsonSerializer.Deserialize<DeleteNtransactionRequest>(RequestJson);

            DeleteNTransactionApi _user = new DeleteNTransactionApi();
            _user.JwtToken = _userrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _user.Hash = _userrequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _user.Data = _userrequest.Data.Data;

            return _user.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check");//Jwt token is included


app.MapPut("/VerifyOTP", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        VerifyOtpRequest _verifyverequest = new VerifyOtpRequest();

        if (_request.Type == VerifyOtpRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _verifyverequest = JsonSerializer.Deserialize<VerifyOtpRequest>(RequestJson);

            VerifyOtpApi _verify = new VerifyOtpApi();
            _verify.JwtToken = _verifyverequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _verify.Hash = _verifyverequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _verify.Data = _verifyverequest.Data.Data;

            return _verify.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check");//Jwt token is included


app.MapPost("/PostRegistration1", async (ModelContext db, String RequestJson) =>
{
    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        RegistrationRequest _registrationRequest = new RegistrationRequest();

        if (_request.Type == RegistrationRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _registrationRequest = JsonSerializer.Deserialize<RegistrationRequest>(RequestJson);

            RegistrationApi _registration = new RegistrationApi();
            _registration.JwtToken = _registrationRequest.JwtToken;
            _registration.Hash = _registrationRequest.Hash;
            
            _registration.Data = _registrationRequest.Data.Data;

            return _registration.Validate(db);
        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("not check");//Jwt token is included

app.MapGet("/Customer/phonenumberforget", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        CustomerPhoneNumberRequest _customerRequest = new CustomerPhoneNumberRequest();

        if (_request.Type == CustomerPhoneNumberRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _customerRequest = JsonSerializer.Deserialize<CustomerPhoneNumberRequest>(RequestJson);

            CustomerPhoneNumberForgetApi _customer = new CustomerPhoneNumberForgetApi();
            _customer.JwtToken = _customerRequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _customer.Hash = _customerRequest.Hash;

            _customer.Data = _customerRequest.Data.Data;

            return _customer.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithName("send otp for otp").WithTags("not check");//Jwt token is included


app.MapGet("/WithdrawalTo", async (ModelContext db, string RequestJson) =>
{
    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        WithdrawalToRequest _withdrawRequest = new WithdrawalToRequest();

        if (_request.Type == WithdrawalToRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _withdrawRequest = JsonSerializer.Deserialize<WithdrawalToRequest>(RequestJson);

            WithdrawalToApi _withdrawto = new WithdrawalToApi();
            _withdrawto.JwtToken = _withdrawRequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _withdrawto.Hash = _withdrawRequest.Hash;

            _withdrawto.Data = _withdrawRequest.Data.Data;

            return _withdrawto.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check");//Jwt token is included


app.MapPut("/SetMpin", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        SetMpinRequest _setmpinrequest = new SetMpinRequest();

        if (_request.Type == SetMpinRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _setmpinrequest = JsonSerializer.Deserialize<SetMpinRequest>(RequestJson);

            SetMpinApi _setmpin = new SetMpinApi();
            _setmpin.JwtToken = _setmpinrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _setmpin.Hash = _setmpinrequest.Hash;

            _setmpin.Data = _setmpinrequest.Data.Data;

            return _setmpin.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("not check");//Jwt token is included

app.MapGet("/SearchCustomer", async (ModelContext db, string RequestJson) =>
{
    
    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        SearchCustomerRequest _searchRequest = new SearchCustomerRequest();

        if (_request.Type == SearchCustomerRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _searchRequest = JsonSerializer.Deserialize<SearchCustomerRequest>(RequestJson);

            SearchCustomerApi _searchCustomer = new SearchCustomerApi();
            _searchCustomer.JwtToken = _searchRequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _searchCustomer.Hash = _searchRequest.Hash;

            _searchCustomer.Data = _searchRequest.Data.Data;

            return _searchCustomer.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("not check");//Jwt token is included



app.MapPut("/forgetpassword", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        ForgetPasswordRequest _forgetpasswordrequest = new ForgetPasswordRequest();

        if (_request.Type == ForgetPasswordRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _forgetpasswordrequest = JsonSerializer.Deserialize<ForgetPasswordRequest>(RequestJson);

            ForgetPasswordAPI _forgetpassword = new ForgetPasswordAPI();
            _forgetpassword.JwtToken = _forgetpasswordrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _forgetpassword.Hash = _forgetpasswordrequest.Hash;

            _forgetpassword.Data = _forgetpasswordrequest.Data.Data;

            return _forgetpassword.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("not check");//Jwt token is included

app.MapGet("/GetScheduledTransactions", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        ScheduledTransactionRequest _schedulerequest = new ScheduledTransactionRequest();

        if (_request.Type == ScheduledTransactionRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _schedulerequest = JsonSerializer.Deserialize<ScheduledTransactionRequest>(RequestJson);

            ScheduledtransactionApi _schedule = new ScheduledtransactionApi();
            _schedule.JwtToken = _schedulerequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _schedule.Hash = _schedulerequest.Hash;

            _schedule.Data = _schedulerequest.Data.Data;

            return _schedule.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        return Results.BadRequest(new { status = "something went wrong" });
    }
   

}).WithTags("not check");//Jwt token is included


app.MapGet("/Customer/phonenumber", async (ModelContext db, string RequestJson) =>
{
    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        GetCustomerPhoneRequest _customerphonerequest = new GetCustomerPhoneRequest();

        if (_request.Type == GetCustomerPhoneRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _customerphonerequest = JsonSerializer.Deserialize<GetCustomerPhoneRequest>(RequestJson);

            GetCustomerPhoneApi _phone = new GetCustomerPhoneApi();
            _phone.JwtToken = _customerphonerequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _phone.Hash = _customerphonerequest.Hash;
            //string DataBlockSerialised = JsonSerializer.Serialize<AccountdetailsApi>(_AccountDetailrequest.Data.Data);
            _phone.Data = _customerphonerequest.Data.Data;

            return _phone.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }

    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }




}).WithTags("not check").WithName("send otp for registration");//Jwt Token is included

app.MapPost("/Login/mpin", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        LoginMpinRequest _mpinRequest = new LoginMpinRequest();

        if (_request.Type == LoginMpinRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _mpinRequest = JsonSerializer.Deserialize<LoginMpinRequest>(RequestJson);

            LoginMpinApi _mpinlogin = new LoginMpinApi();
            _mpinlogin.JwtToken = _mpinRequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _mpinlogin.Hash = _mpinRequest.Hash;

            _mpinlogin.Data = _mpinRequest.Data.Data;

            return _mpinlogin.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

});//Jwt token is included


app.MapGet("/Reports/Company", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        ReportCompanyRequest _reportcompanyRequest = new ReportCompanyRequest();

        if (_request.Type == ReportCompanyRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _reportcompanyRequest = JsonSerializer.Deserialize<ReportCompanyRequest>(RequestJson);

            ReportCompanyApi _report = new ReportCompanyApi();
            _report.JwtToken = _reportcompanyRequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _report.Hash = _reportcompanyRequest.Hash;

            _report.Data = _reportcompanyRequest.Data.Data;

            return _report.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }



}).WithTags("not check");//Jwt token is included


app.MapGet("/Reports/Branch", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        ReportBranchRequest _branchrequest = new ReportBranchRequest();

        if (_request.Type == ReportBranchRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _branchrequest = JsonSerializer.Deserialize<ReportBranchRequest>(RequestJson);

            ReportBranchApi _reportbranch = new ReportBranchApi();
            _reportbranch.JwtToken = _branchrequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _reportbranch.Hash = _branchrequest.Hash;

            _reportbranch.Data = _branchrequest.Data.Data;

            return _reportbranch.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }




}).WithTags("not check");//Jwt token is included



#region Notes


app.MapGet("/GetNotes", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        GetNotesRequest GetNotes = new GetNotesRequest();

        if (_request.Type == GetNotesRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            GetNotes = JsonSerializer.Deserialize<GetNotesRequest>(RequestJson);

            GetNotesAPI notes = new GetNotesAPI();
            notes.JwtToken = GetNotes.JwtToken;// _AccountDetailrequest.JwtToken;
            notes.Hash = GetNotes.Hash;

            notes.Data = GetNotes.Data.Data;

            return notes.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithName("GetNotes")//Jwt token is included
.WithTags("not check");


app.MapPut("/DeleteNotes", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        DeleteNoteRequest DeleteNotes = new DeleteNoteRequest();

        if (_request.Type == DeleteNoteRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            DeleteNotes = JsonSerializer.Deserialize<DeleteNoteRequest>(RequestJson);

            DeleteNoteAPI notes = new DeleteNoteAPI();
            notes.JwtToken = DeleteNotes.JwtToken;// _AccountDetailrequest.JwtToken;
            notes.Hash = DeleteNotes.Hash;

            notes.Data = DeleteNotes.Data.Data;

            return notes.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithName("DeleteNotes")//Jwt token is included
.WithTags("not check");



app.MapPost("/AddNotes",async(ModelContext db,string RequestJson)=>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        AddNoteRequest AddNotes = new AddNoteRequest();

        if (_request.Type == AddNoteRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            AddNotes = JsonSerializer.Deserialize<AddNoteRequest>(RequestJson);

            AddNoteAPI notes = new AddNoteAPI();
            notes.JwtToken = AddNotes.JwtToken;// _AccountDetailrequest.JwtToken;
            notes.Hash = AddNotes.Hash;

            notes.Data = AddNotes.Data.Data;

            return notes.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithName("createNotes")//Jwt token is included
.WithTags("not check");



//app.MapPost("")

#endregion

#region cheque


app.MapPut("Cheque_EmployeeVerificaton", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        ChequeEmployeeVerifyRequest _checkemployeerequest = new ChequeEmployeeVerifyRequest();

        if (_request.Type == ChequeEmployeeVerifyRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _checkemployeerequest = JsonSerializer.Deserialize<ChequeEmployeeVerifyRequest>(RequestJson);

            ChequeEmployeeVerifyApi _employee = new ChequeEmployeeVerifyApi();
            _employee.JwtToken = _checkemployeerequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _employee.Hash = _checkemployeerequest.Hash;

            _employee.Data = _checkemployeerequest.Data.Data;

            return _employee.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("not check");//Jwt token is included


app.MapPut("PutemployeeBounceCheques", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        PutEmployeeBounceRequest _putemployeebouncerequest = new PutEmployeeBounceRequest();

        if (_request.Type == PutEmployeeBounceRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _putemployeebouncerequest = JsonSerializer.Deserialize<PutEmployeeBounceRequest>(RequestJson);

            PutEmployeeBounceApi _employee = new PutEmployeeBounceApi();
            _employee.JwtToken = _putemployeebouncerequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _employee.Hash = _putemployeebouncerequest.Hash;

            _employee.Data = _putemployeebouncerequest.Data.Data;

            return _employee.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("not check");//Jwt token is included


app.MapGet("/ChequeReconsiledList", async (ModelContext db,string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        ChequeReconsilationRequest _putemployeebouncerequest = new ChequeReconsilationRequest();

        if (_request.Type == ChequeReconsilationRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _putemployeebouncerequest = JsonSerializer.Deserialize<ChequeReconsilationRequest>(RequestJson);

            ChequeReconsilationAPI _employee = new ChequeReconsilationAPI();
            _employee.JwtToken = _putemployeebouncerequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _employee.Hash = _putemployeebouncerequest.Hash;

            //_employee.Data = _putemployeebouncerequest.Data.Data;

            return _employee.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }




}).WithTags("not check");//Jwt token is included

app.MapPut("return_cheque", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        ReturnChequeRequest _returnchequerequest = new ReturnChequeRequest();

        if (_request.Type == ReturnChequeRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _returnchequerequest = JsonSerializer.Deserialize<ReturnChequeRequest>(RequestJson);

            ReturnChequeApi _employee = new ReturnChequeApi();
            _employee.JwtToken = _returnchequerequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _employee.Hash = _returnchequerequest.Hash;

            _employee.Data = _returnchequerequest.Data.Data;

            return _employee.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("not check");//Jwt token is included






app.MapPut("PutBHBounceCheques", async (ModelContext db, String RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        PutBHBounceRequest _putbhbouncerequest = new PutBHBounceRequest();

        if (_request.Type == PutBHBounceRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _putbhbouncerequest = JsonSerializer.Deserialize<PutBHBounceRequest>(RequestJson);

            PutBHBounceApi _employee = new PutBHBounceApi();
            _employee.JwtToken = _putbhbouncerequest.JwtToken;// _AccountDetailrequest.JwtToken;
            _employee.Hash = _putbhbouncerequest.Hash;

            _employee.Data = _putbhbouncerequest.Data.Data;

            return _employee.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }


}).WithTags("not check");//Jwt token is included




app.MapGet("/Relationships", async (ModelContext db, string RequestJson) =>
{

    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        RelationshipRequest _customer = new RelationshipRequest();

        if (_request.Type == RelationshipRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _customer = JsonSerializer.Deserialize<RelationshipRequest>(RequestJson);

            RelationshipApi _getcustomerDetails = new RelationshipApi();
            _getcustomerDetails.JwtToken = _customer.JwtToken;// _AccountDetailrequest.JwtToken;
            _getcustomerDetails.Hash = _customer.Hash;

           // _employee.Data = _putbhbouncerequest.Data.Data;

            return _getcustomerDetails.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }

}).WithTags("not check");//Jwt token is included




app.MapGet("/GetBhverification", async (ModelContext db, string RequestJson) =>
{
    try
    {

        Request _request = JsonSerializer.Deserialize<Request>(RequestJson);

        GetBhVerificationRequest _customer = new GetBhVerificationRequest();

        if (_request.Type == GetBhVerificationRequest.Requesttype && _request.Ver == (decimal)1.0)
        {
            _customer = JsonSerializer.Deserialize<GetBhVerificationRequest>(RequestJson);

            GetBhVerificationApi _bhverification = new GetBhVerificationApi();
            _bhverification.JwtToken = _customer.JwtToken;// _AccountDetailrequest.JwtToken;
            _bhverification.Hash = _customer.Hash;

            // _employee.Data = _putbhbouncerequest.Data.Data;

            return _bhverification.Validate(db);

        }
        else
        {
            Log.Warning("invalid request");
            return Results.NotFound(new { status = "invalid request" });
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
        if (ex.Message == "A null key is not valid in this context")
        {
            return Results.UnprocessableEntity(new { status = "session timeout" });
        }
        else
        {
            return Results.BadRequest(new { status = "something went wrong" });
        }

    }
}).WithTags("not check");//Jwt token is included



#endregion






app.Run();
