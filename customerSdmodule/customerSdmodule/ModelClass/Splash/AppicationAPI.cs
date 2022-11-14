using customerSdmodule.Model1;
using static RedisCacheDemo.RedisCacheStore;
using System.Text.Json;
using customerSdmodule.Redis;
using RedisCacheDemo;
using TokenManager;
using Serilog;

namespace customerSdmodule.ModelClass.Splash
{
    public class AppicationAPI : IGetAPI
    {
        public Redis.CacheData _cache;
        private ApplicationData _application;
        private string status = "t";

        public ApplicationData Application { get => _application; set => _application = value; }

        public IResult Get(ModelContext db)
        {
            return ApplicationDetails(db);
        }
        public IResult ApplicationDetails(ModelContext db)
        {
            try
            {
                var uniqueKey = Guid.NewGuid().ToString();
                var Token = TokenManagement.GenerateToken(Application.DeviceToken, uniqueKey);
                var applicationDetails = (from application in db.Applications
                                         where application.AppNo == Application.ApplicationNumber
                                         select new
                                         {
                                             appNo = application.AppNo,
                                             versionNo = application.VersionNo,
                                             firmId = application.FirmId,
                                             moduleId = application.ModuleId,
                                             created = application.Builder,
                                             buildDate = application.BuildDate,
                                             splashToken = Token,
                                         }).SingleOrDefault();
                if (applicationDetails == null)
                {

                    //ResponseData _Response = new ResponseData();
                    //_Response.ResponseCode = 404;
                    //_Response.Data = JsonSerializer.Serialize(new { status = "not found" });
                    //return _Response;
                    return Results.NotFound(new { status = "not found" });
                }
                else
                {

                    var storeData = new CacheData
                    {
                        FirmId = (int)applicationDetails.firmId,
                        ModuleId = (int)applicationDetails.moduleId,
                        ApplicationNo = (int)applicationDetails.appNo,
                        Version = applicationDetails.versionNo,
                        DeviceId = Application.DeviceToken,
                    };
                    RedisRun.Set(uniqueKey, JsonSerializer.Serialize<CacheData>(storeData));
                    // RedisCacheSet(uniqueKey, storeData);


                    // return Results.Ok(new { appicationDetails = appicationDetails });
                    //ResponseData _Response = new ResponseData();
                    //_Response.ResponseCode = 200;
                    //_Response.Data = JsonSerializer.Serialize(new { status = "success",applicationDetails=applicationDetails });
                    //return _Response;
                    return Results.Ok(new { applicationDetails = applicationDetails });
                }
            }
            catch (Exception ex)
            {
                //ResponseData _Response = new ResponseData();
                //_Response.ResponseCode = 400;
                //_Response.Data = JsonSerializer.Serialize(new { status = "Somethings went wrong" });
                //return _Response;
                return Results.BadRequest(new { status = "Server Error", Messege = ex.Message });
            }

        }







        public List<Exception> Validate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
            if (Application.ApplicationNumber == 0)
            {
                FailedValidations.Add(new ApplicationException("wrong details"));
            }
            if (status == null)
            {
                FailedValidations.Add(new ApplicationException("Inavalid Data"));
            }
            return FailedValidations;
        }
    }


}
