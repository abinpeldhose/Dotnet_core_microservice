using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using customerSdmodule.Redis;
using customerSdmodule.Roles;
using Serilog;
using System.Text.Json;
using TokenManager;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.employeelogin
{
    public class EmployeeLoginApi:BaseApi
    {
        Random rnd = new Random();
        securitylogin hash=new securitylogin();

        private EmployeeLoginData _log;
        
      //  private string _jwtToken;

        public EmployeeLoginData Data { get => _log; set => _log = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }


       
        public ResponseData Get(ModelContext db)
        {
            return GetLoginDetails(db);
        }

        private ResponseData GetLoginDetails(ModelContext db)
        {
            try
            {
                var BHList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 101, 234, 235, 251, 252, 319, 1064, 478, 1040, 146, 148, 149, 424, 433, 377 };
                var ABHList = new List<int>() { };
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var result = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var Token = TokenManagement.GenerateToken("12345", uniqueKey);
                var cache = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var deviceDetails = db.BlockedDevices.Where(x => x.DeviceId == cache.DeviceId).FirstOrDefault();

                //  var result = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));

                //  var Register1 = db.EmployeeMasters.Where(x => x.EmpCode == Convert.ToInt32(_log.UserId)).Select(x => x.AccessId).FirstOrDefault();
                var Register = db.EmployeeMasters.Where(x => x.EmpCode == Convert.ToInt32(_log.UserId) && x.StatusId == 1).SingleOrDefault();
                //var Token = TokenManagement.GenerateToken("12345", uniqueKey);
                var password = hash.CreateHash(_log.UserId + "raju" + _log.Password);


                var PasswordOracletoDotnet = Register.Password.ToString().Replace("-", string.Empty);


                byte[] bytes = ParseHex(PasswordOracletoDotnet.ToString());
                Guid guid = new Guid(bytes);
                var ConvertedPassword = guid.ToString("N").ToUpperInvariant();
                static byte[] ParseHex(string text)
                {
                    // Not the most efficient code in the world, but
                    // it works...
                    byte[] ret = new byte[text.Length / 2];
                    for (int i = 0; i < ret.Length; i++)
                    {
                        ret[i] = Convert.ToByte(text.Substring(i * 2, 2), 16);
                    }
                    return ret;
                }
                
                    if (Register != null && ConvertedPassword == password)
                    {
                        Role role = new Role();


                        var Employee = (from x in db.EmployeeMasters
                                        join branchmaster in db.BranchMasters on x.BranchId equals branchmaster.BranchId
                                        where x.EmpCode == Convert.ToInt32(Register.EmpCode)
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
                                            userType = /*x.PostId == 10 ? "BH" : x.PostId == 1 ? "ABH" : "Employee",*/ "Employee",
                                            branchname = branchmaster.BranchName,
                                            loginToken = Token,
                                            userAccess = BHList.Contains(x.PostId) ? role.checkRoles("BH") : x.PostId == 1 ? role.checkRoles("ABH") : role.checkRoles("Employee"),

                                        }).SingleOrDefault();

                        result.UserType = Employee.userType;
                        result.UserId = Employee.empCode.ToString();
                        result.BranchId = Employee.branchId;
                        result.UserName = Employee.empName;
                        result.CustomerId = Employee.empCode.ToString();

                        RedisRun.Set(uniqueKey, (JsonSerializer.Serialize<CacheData>(result)));
                        Log.Information("Success");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(Employee);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    



                    
                        return _Response;
                        // return Results.Ok(Employee);

                    }
                    else
                    {
                        var results = new
                        {
                            status = "userid and password are incorrect",
                        };
                        Log.Error("userid and password are incorrect");
                        // return Results.NotFound(results);
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(results);

                    Log.Information("userid and password are incorrect");
                        return _Response;
                    }
                //}
                //else
                //{
                   
                //        Log.Error("this user is blocked");

                //        ResponseData _Response = new ResponseData();
                //        _Response.responseCode = 404;
                //        _Response.data = JsonSerializer.Serialize(new { status = "this user is blocked" });

                //        Log.Information("this user is blocked");
                //        return _Response;

                //        //return Results.BadRequest(new {status="this user is blocked"});

                    
                //}
            }



            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                //  Log.Error(ex.Message);
                // return Results.NotFound(message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new {status=message});
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //_Response.data = JsonSerializer.Serialize(new { status = message });

                Log.Information(ex.Message);
                return _Response;
            }

        }

        protected override bool ValidToken()
        {
            var id = TokenManagement.ValidateToken(JwtToken);
            bool TokenValid = _cache.DeviceId == TokenManager.TokenManagement.ValidateToken(JwtToken);
            return TokenValid;
        }
        /// <summary>
        /// ///////////////////////////real//////////////////////
        /// </summary>
        /// <returns></returns>
        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            
            Data.DeviceID = _cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<EmployeeLoginData>(Data);//.Replace(@"\u002B", "+").Replace(@"\u0026", "&");


            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }




        //    Data.DeviceID = String.Empty;
        //    return _SerialisedDataBlockWithDeviceToken;
        //}
        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = GetLoginDetails(db);
            return _Response;

        }
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            
            List<Exception> FailedValidations = new List<Exception>();
            //if (userid == null)
            //{
            //    FailedValidations.Add(new ApplicationException(" Invalid User id and password "));
           



            return FailedValidations;
        }



    }
}
