using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using customerSdmodule.Redis;
using Serilog;
using System.Text.Json;
using TokenManager;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.CheckPassword
{
    public class CheckPasswordAPI:IGetAPI
    {

        Security hash = new Security();
        private checkPasswordData _registration;
        private string _jwtToken;
        public checkPasswordData Registration { get => _registration; set => _registration = value; }
        public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public IResult Get(ModelContext db)
        {
            return checkpassword(db);
        }
        private IResult checkpassword(ModelContext db)
        {
            try
            {
                var existItem = db.RegistrationMaster1s.FirstOrDefault(x => x.Phone == _registration.Phone);
                if (existItem != null)
                {
                    if (existItem.Password == hash.create_hashs(_registration.Password, existItem.RegistartionDate.ToString("yyyyMMdd")))
                    {
                        Log.Information("Success");
                        return Results.Ok(new { status = "Success" });
                    }
                    else
                    {
                        Log.Error("Password is wrong");
                        return Results.NotFound(new { status = "Password is wrong" });
                    }
                }
                else
                {
                    Log.Error("There is no employee");
                    return Results.NotFound(new { status = "There is no employee" });
                }
            }
            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                return Results.NotFound(message);
            }

        }

        public List<Exception> Validate(ModelContext db)
        {
            string uniqueKey = TokenManagement.Extract(JwtToken);
            var cache = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
            List<Exception> FailedValidations = new List<Exception>();


            var existItem = db.RegistrationMaster1s.FirstOrDefault(x => x.Phone == _registration.Phone);

            var password = db.RegistrationMaster1s.Where(x => x.Password == hash.create_hashs(_registration.Password, existItem.RegistartionDate.ToString("yyyyMMdd"))).SingleOrDefault();



            if (existItem == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Mobile Number and password"));
            }
            if (password == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Mobile Number and password"));
            }
            return FailedValidations;
        }

    }
}
