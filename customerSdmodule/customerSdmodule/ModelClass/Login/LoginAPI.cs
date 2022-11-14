using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using static RedisCacheDemo.RedisCacheStore;
using System.Text.Json;
using customerSdmodule.Redis;
using Serilog;
using customerSdmodule.Roles;

namespace customerSdmodule.ModelClass.Login
{
    public class LoginApI : IGetAPI
    {
        Random rnd = new Random();
        Security hash = new Security();

        private LoginData _log;
        private string _jwtToken;

        public LoginData LoginDetails { get => _log; set => _log = value; }
        public string JwtToken { get => _jwtToken; set => _jwtToken = value; }
       
      
        public LoginApI(string JwtToken)
        {
            JwtToken = this.JwtToken;
        }
        public IResult Get(ModelContext db)
        {
            return GetLoginDetails(db);
        }

        
        private IResult GetLoginDetails(ModelContext db)
        {
            try
            {
                var data = db.ModuleTables.Where(x => x.ModuleId == _log.ModuleId).SingleOrDefault();

                
                if (data == null)
                {
                    var results = new
                    {
                        Status = "This module is not available",
                    };
                   // Log.Warning("This Module Is Not Available");
                    return Results.BadRequest(results);       //404
                }
                else
                {
                    //var data1 = db.EmployeeMasters.Where(x => x.FirmId == firmId && x.EmpCode ==
                    //Convert.ToInt32(userId)).SingleOrDefault();
                    var Register = db.RegistrationMaster1s.Where(x => x.UserId == _log.UserId).SingleOrDefault();
                    //  Console.WriteLine(hash.create_hashs(password, Register.RegistartionDate.ToString("yyyyMMdd")));
                    if (Register != null && Register.Password == hash.create_hashs(_log.Password, Register.RegistartionDate.ToString("yyyyMMdd")))
                    {
                        Role role = new Role();
                        var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                        var result = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));

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
                                                userType = "Employee",
                                                branchname = branchmaster.BranchName,
                                                role=role.checkRoles("employee"),
                                            }).SingleOrDefault();
                            result.UserType = Employee.userType;
                            
                            RedisRun.Set(uniqueKey, (JsonSerializer.Serialize<CacheData>(result)));

                            db.SaveChanges();
                          //  Log.Information("Success");
                            return Results.Ok(new {loginDetails=Employee,token=JwtToken});
                        }
                        else
                        {
                            var customer = db.Customers.Where(x => x.CustId == Register.Id.ToString()).
                           Select(x => new
                           {
                               customerId = x.CustId,
                               customerName = x.CustName,
                               firmId = x.FirmId,
                               branchId = x.BranchId,
                               maritalStatus = x.MaritalStatus,
                               fatherName = x.FatherName,
                               phoneNumber = x.Phone1,
                               pinNo = x.PinNo,
                               houseName = x.HouseName,
                               locality = x.Locality,
                               postcode = x.Locality,
                               userType = "Customer",
                               role = role.checkRoles("customer"),
                               sessionId = rnd.Next().ToString(),

                           }).SingleOrDefault();

                            result.UserType = customer.userType;
                            result.BranchId = (int)customer.branchId;
                            
                            RedisRun.Set(uniqueKey, (JsonSerializer.Serialize<CacheData>(result)));

                            db.SaveChanges();
                          //  Log.Information("success");
                            return Results.Ok(new { LoginDetails = customer,token=JwtToken });
                        }
                        
                    }
                    else
                    {
                        var results = new
                        {
                            status = "userid and password are incorrect",
                        };
                       // Log.Error("userid and password are incorrect");
                        return Results.NotFound(results);
                    }
                }

            }

            catch (Exception ex)
            {

                var message = new { status = "something went wrong" };
                Log.Error(ex.Message);
                return Results.NotFound(message);
            }




        }



        public List<Exception> Validate(ModelContext db)
        {
            //Redis.RedisJsonconverter.RedisCacheGet(uniqueKey, null);
            var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
            var result = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));

            var Moduleid = db.ModuleTables.Where(x => x.ModuleId == result.ModuleId).SingleOrDefault();
            var userid = db.RegistrationMaster1s.Where(x => x.UserId == _log.UserId).SingleOrDefault();
            var firmid = db.RegistrationMaster1s.Where(x => x.FirmId == result.FirmId && x.UserId == _log.UserId).SingleOrDefault();

            var password = db.RegistrationMaster1s.Where(x => x.Password == hash.create_hashs(_log.Password, userid.RegistartionDate.ToString("yyyyMMdd"))).SingleOrDefault();
            //  var moduleid = db.RegistrationMaster1s.Where(x => x. == _login.ModuleId.SingleOrDefault();

            List<Exception> FailedValidations = new List<Exception>();
            if (userid == null)
            {
                FailedValidations.Add(new ApplicationException(" Invalid User id and password "));
            }
            if (password == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid User id and password "));
            }
            if (Moduleid == null)
            {
                FailedValidations.Add(new ApplicationException(" Invalid User id and password"));
            }

            if (firmid == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid User id and password  "));
            }




            return FailedValidations;
        }

     

    }

}
