using customerSdmodule.Model1;
using customerSdmodule.Redis;
using Serilog;
using System.Text.Json;
using TokenManager;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.CheckMobileNumber
{
    public class CheckMobileNumberAPI:IGetAPI
    {
        private ChequeMobileNumberData _employeemaster;

        public ChequeMobileNumberData Employeemaster { get => _employeemaster; set => _employeemaster = value; }
        private string _jwtToken;
        public string JwtToken { get => _jwtToken; set => _jwtToken = value; }
        public IResult Get(ModelContext db)
        {
            return checkmobilenumber(db);

        }
        private IResult checkmobilenumber(ModelContext db)
        {

            try
            {
                var Data = (from em in db.EmployeeMasters
                            join ep in db.EmployeePostMasters on em.PostId equals ep.PostId
                            join ed in db.EmployeeDepartmentMasters on em.DepartmentId equals ed.DepId
                            where em.Phone == _employeemaster.Phone
                            select new
                            {
                                employeeCode = em.EmpCode,
                                designation = ep.PostName,
                                userName = em.EmpName,
                                firmId = em.FirmId,
                                branchId = em.BranchId,
                                department = ed.DepName,

                            }).SingleOrDefault();

                if (Data == null)
                {
                    Log.Error("No Data Found");
                  //  throw new FileNotFoundException(message:"");
                    throw new Exception(message: "NoDataFound");
                    return Results.NotFound(new {status="no data found"});

                }
                else
                {
                    Log.Information("Successfully Checked Mobile Number");
                    return Results.Ok(new { Customer = Data, token = JwtToken });
                }
            }
            catch (FileNotFoundException fx)
            {
                throw new FileNotFoundException();
                return Results.BadRequest("NotFound");
            }

            catch (Exception ex)
            {
                var message = new { Status = "Something went wrong" };
                Log.Error(ex.Message);
                return Results.NotFound(message);
            }

        }


        public List<Exception> Validate(ModelContext db)
        {
            string uniqueKey = TokenManagement.Extract(JwtToken);
            var cache = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
            List<Exception> FailedValidations = new List<Exception>();
            var phone = db.EmployeeMasters.Where(x => x.Phone == _employeemaster.Phone).SingleOrDefault();
            if (phone == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Data"));
            }



            return FailedValidations;
        }
    }
}
