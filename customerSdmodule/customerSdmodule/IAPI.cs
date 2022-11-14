using customerSdmodule.Model1;
using customerSdmodule.sample;

namespace customerSdmodule
{
    public interface IAPI
    {
        public void Save(ModelContext db);
        public List<Exception> Validate( ModelContext db);
    }
    public interface IGetAPI
    {
        public IResult Get(ModelContext db);
        public List<Exception> Validate(ModelContext db);

        
    }
    public interface IGetAPI_Accounts
    {
        public IResult Get(ModelContext_Account db);
        public List<Exception> Validate(ModelContext_Account db);


    }
}
