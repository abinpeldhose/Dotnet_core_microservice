using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using Serilog;

namespace customerSdmodule.ModelClass.forgetpassword
{
    public class ForgetOTPAPI:IGetAPI
    {
        Security hash = new Security();
        private ForgetOTPData _forgetPassword;
       // private string status;
        public ForgetOTPData ForgetPassword { get => _forgetPassword; set => _forgetPassword = value; }

        public IResult Get(ModelContext db)
        {
            return forgetPasswordDetails(db);
        }
        public IResult forgetPasswordDetails(ModelContext db)
        {
            try
            {
                //var otp = Convert.ToInt32(DateFunctions.sysdate(db).ToString("yyyydd"));
                int _OTPNo = new Random().Next(100000, 999999);
                var txnId = Convert.ToInt32(DateFunctions.sysdate(db).ToString("HHmmss"));
                var Regdata = db.RegistrationMaster1s.Where(x => x.Phone == ForgetPassword.Mobilenumber).ToList();


                if (Regdata.Count() > 0)
                {
                    foreach (var data in Regdata)
                    {
                        var OTPdata = new Otp
                        {
                            Mobilenumber = ForgetPassword.Mobilenumber,
                            Otp1 = hash.create_hashs(_OTPNo.ToString() + "+5", DateFunctions.sysdate(db).ToString("yyyyMMdd")),//otp,
                            TimeStamp = DateFunctions.sysdate(db),
                            Status = 0,
                            TransactionId = txnId,
                            UserId = data.Id,
                            MaxTime = 1,
                        };

                        db.Otps.Add(OTPdata);
                        db.SaveChangesAsync();
                    }
                    var showdata = new
                    {
                        TransactionId = txnId,
                    };
                    //  Log.Information("Success");
                    return Results.Ok(showdata);
                    //return Results.Created($"/Postotp{ data.Mobilenumber}", new Otp());
                }
                else
                {


                    var Status = new
                    {
                        status = "This user is not registered",
                    };
                    // Log.Error("This User Is Not Registered");
                    return Results.NotFound(Status);
                }

            }
            catch(Exception ex)
            {

                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                return Results.NotFound(message);
            }
           

        }
        public List<Exception> Validate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
            if (ForgetPassword.Mobilenumber == null)
            {
                FailedValidations.Add(new ApplicationException("mobile number is wrong"));
            }
           
            return FailedValidations;
        }
    }
}
