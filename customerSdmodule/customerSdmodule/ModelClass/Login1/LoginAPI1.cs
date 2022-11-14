using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using customerSdmodule.Redis;
using customerSdmodule.Roles;
using Serilog;
using System.Text.Json;
using TokenManager;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.Login1
{
    public class Login1 : BaseApi
    {
        Random rnd = new Random();
        Security hash = new Security();

        private LoginData1 _log;
        //  private string _jwtToken;

        public LoginData1 LoginDetails { get => _log; set => _log = value; }
       


        //private IResult GetLoginDetails(ModelContext db)
        private ResponseData GetLoginDetails(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cache = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));

                //  var result = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));

                var Register = db.UserLoginMst1s.Where(x => x.UserId == _log.UserId).SingleOrDefault();
                   var Token = TokenManagement.GenerateToken("12345", uniqueKey);
                var deviceDetails = db.BlockedDevices.Where(x => x.DeviceId == cache.DeviceId).FirstOrDefault();
                if (deviceDetails == null)
                {
                    deviceDetails = new BlockedDevice
                    {
                        DeviceId = cache.DeviceId,
                        LastAttemptDate = DateFunctions.sysdate(db),
                        ActiveStatus = 0,
                        Attempt = 0
                    };
                    db.BlockedDevices.Add(deviceDetails);
                }
                int attempt = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["attempt"]);
                if (deviceDetails.Attempt <= attempt)
                {
                    if (Register != null && Register.Password == hash.create_hashs(_log.Password, Register.RegistartionDate.ToString("yyyyMMdd")))
                    {
                        Role role = new Role();

                        if (Register.Id.Length <= 6)
                        {
                            var Employee = (from x in db.EmployeeMasters
                                            join branchmaster in db.BranchMasters on x.BranchId equals branchmaster.BranchId
                                            where x.EmpCode == Convert.ToInt32(Register.Id)
                                            select new
                                            {
                                                empCode = x.EmpCode,
                                                empName = x.EmpName,
                                                empType = x.EmpType,
                                                firmId = x.FirmId,
                                                branchId = x.BranchId,
                                                statusId = x.StatusId,
                                                accessId = x.AccessId,
                                                designationId = x.DesignationId,
                                                departmentId = x.DepartmentId,
                                                postId = x.PostId,
                                                mobileNumber = x.Phone,
                                                sessionId = rnd.Next().ToString(),
                                                userType = /*x.PostId == 10 ? "BH" : x.PostId == 1?"ABH":"Employee",//*/ "Employee", //i can't change user type because its depended table
                                                branchname = branchmaster.BranchName,
                                                loginToken = Token,
                                                userAccess = x.PostId == 10 ? role.checkRoles("BH") : x.PostId == 1 ? role.checkRoles("ABH") : role.checkRoles("Employee"),
                                            }).SingleOrDefault();
                            deviceDetails.Attempt = 0;
                            deviceDetails.LastAttemptDate = DateFunctions.sysdate(db);
                            db.SaveChanges();
                            cache.UserType = Employee.userType;
                            cache.UserId = Employee.empCode.ToString();
                            cache.BranchId = Employee.branchId;
                            cache.UserName = Employee.empName;
                            cache.CustomerId = Employee.empCode.ToString();

                            RedisRun.Set(uniqueKey, (JsonSerializer.Serialize<CacheData>(cache)));

                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(Employee);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            // _Response.data = JsonSerializer.Serialize<dynamic>(Employee);
                            Console.WriteLine(_Response.data);

                            Log.Information("Success");
                            return _Response;
                            //return Results.Ok(Employee);
                        }
                        else
                        {
                           var customer = db.Customers.Where(x => x.CustId == Register.Id.ToString()).
                           Select(x => new
                           {
                               customerId = x.CustId,
                               customerName = x.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                               firmId = x.FirmId,
                               branchId = x.BranchId,
                               maritalStatus = x.MaritalStatus,
                               fatherName = x.FatherName,
                               phoneNumber = x.Phone1,
                               pinNo = x.PinNo,
                               houseName = x.HouseName,
                               locality = x.Locality,
                               postcode = x.Locality,
                               userAccess = role.checkRoles("customer"),
                               userType = "Customer",
                               loginToken = Token,

                           }).SingleOrDefault();
                            deviceDetails.Attempt = 0;
                            deviceDetails.LastAttemptDate = DateFunctions.sysdate(db);
                            cache.UserType = customer.userType;
                            cache.UserId = customer.customerId.ToString();
                            cache.BranchId = (int)customer.branchId;
                            cache.CustomerId = customer.customerId;
                            cache.UserName = customer.customerName;

                            db.SaveChanges();
                            RedisRun.Set(uniqueKey, (JsonSerializer.Serialize<CacheData>(cache)));

                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(customer);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            //  _Response.data = JsonSerializer.Serialize(customer);



                            Log.Information("Success");
                            return _Response;
                            //return Results.Ok(Employee);
                        }
                    }
                    else
                    {
                        var results = new
                        {
                            Status = "userid and password are incorrect",
                        };
                        Log.Error("userid and password are incorrect");
                        deviceDetails.Attempt = (byte)(deviceDetails.Attempt + 1);
                        deviceDetails.LastAttemptDate = DateFunctions.sysdate(db);
                        db.SaveChanges();

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(results);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(results);

                        Log.Information("userid and password are incorrect");
                        return _Response;
                    }
                }
                else
                {
                    Log.Error("this user is blocked");

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "this user is blocked" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "this user is blocked" });

                    Log.Information("this user is blocked");
                    return _Response;

                    //return Results.BadRequest(new {status="this user is blocked"});

                }               

            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Information(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = message });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = message });


                return _Response;                
            }
        }


        protected override bool ValidToken()
        {
            var id= TokenManagement.ValidateToken(JwtToken);
            bool TokenValid = _cache.DeviceId == TokenManager.TokenManagement.ValidateToken(JwtToken);
            return TokenValid;
        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            LoginDetails.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<LoginData1>(LoginDetails);
            LoginDetails.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

       
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            //var depositid = db.SdSubApplicants.Where(x => x.DocumentId == Data.Depositid).ToList();
            //var value = db.SdMasters.Where(x => x.DepositId == Data.Depositid).ToList();
            //if (depositid == null)
            //{
            //    FailedValidation.Add(new ApplicationException("Inavlid Input"));
            //}
            //if (value == null)
            //{
            //    FailedValidation.Add(new ApplicationException("Invalid Input"));
            //}
            return FailedValidation;
        }

        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = GetLoginDetails(db);
            return _Response;
 
        }

    //public List<Exception> Validate(ModelContext db)
    //    {
    //        //Redis.RedisJsonconverter.RedisCacheGet(uniqueKey, null);
    //        var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
    //        var result = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));

    //        var Moduleid = db.ModuleTables.Where(x => x.ModuleId == result.ModuleId).SingleOrDefault();
    //      //  var userid = db.UserLoginMst1s.Where(x => x.UserId == _log.UserId).SingleOrDefault();
    //        var firmid = db.UserLoginMst1s.Where(x => x.FirmId == result.FirmId && x.UserId == _log.UserId).SingleOrDefault();

    //      //  var password = db.UserLoginMst1s.Where(x => x.Password == hash.create_hashs(_log.Password, userid.RegistartionDate.ToString("yyyyMMdd"))).SingleOrDefault();
    //        //  var moduleid = db.RegistrationMaster1s.Where(x => x. == _login.ModuleId.SingleOrDefault();

    //        List<Exception> FailedValidations = new List<Exception>();
    //        //if (userid == null)
    //        //{
    //        //    FailedValidations.Add(new ApplicationException(" Invalid User id and password "));
    //        //}
    //        //if (password == null)
    //        //{
    //        //    FailedValidations.Add(new ApplicationException("Invalid User id and password "));
    //        //}
    //        if (Moduleid == null)
    //        {
    //            FailedValidations.Add(new ApplicationException(" Invalid User id and password"));
    //        }

    //        if (firmid == null)
    //        {
    //            FailedValidations.Add(new ApplicationException("Invalid User id and password  "));
    //        }




    //        return FailedValidations;
    //    }



}
    }

