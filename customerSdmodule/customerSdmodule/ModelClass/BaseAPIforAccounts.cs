
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;
using System.Text;
using customerSdmodule.sample;

namespace customerSdmodule.ModelClass
{
    public class BaseAPIforAccounts
    {

        private string _jwtToken;
        protected Redis.CacheData _cache;
        private string _Hash = string.Empty;

        public string JwtToken { get => _jwtToken; set => _jwtToken = GetAPIState(value); }
        public string Hash { get => _Hash; set => _Hash = value; }

        private string GetAPIState(string Token)
        {
            string uniqueKey = TokenManager.TokenManagement.Extract(Token);
            _cache = JsonSerializer.Deserialize<Redis.CacheData>(RedisRun.Get(uniqueKey, null));
            return Token;
        }

        protected virtual bool ValidToken()
        {
            bool TokenValid = _cache.UserId == TokenManager.TokenManagement.ValidateToken(_jwtToken);
            return TokenValid;
        }
        public IResult Get(ModelContext_Account db)
        {
            return CustomisedGet(db);
        }

        protected bool ValidateHash()
        {
            string ReceviedHash = _Hash;
            bool Result = false;
            string _serialisedDataBlock = String.Concat(GetSerialisedDataBlockWithDeviceToken().Where(c => !Char.IsWhiteSpace(c)));//GetSerialisedDataBlockWithDeviceToken();
            string _generatedHash = CreateMD5(_serialisedDataBlock);
            Console.WriteLine(String.Concat(GetSerialisedDataBlockWithDeviceToken().Where(c => !Char.IsWhiteSpace(c))));
            Result = ReceviedHash == _generatedHash;
            return Result;
        }

        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }
        protected virtual string GetSerialisedDataBlockWithDeviceToken()
        {
            throw new NotImplementedException();
        }

        protected virtual IResult CustomisedGet(ModelContext_Account db)
        {
            throw new NotImplementedException("MustImplment this function");
        }

        public IResult Validate(ModelContext_Account db)
        {
            List<Exception> Exceptions = new List<Exception>();
            IResult _APIResult = Results.BadRequest("API Failed");

            if (ValidToken() && ValidateHash())
            {

                Exceptions = CustomisedValidate(db);

                if (Exceptions.Count > 0)
                {
                    _APIResult = OnValidationFailed(db, Exceptions);
                }
                else
                {
                    ResponseData _Response = OnValidationSuccess(db);

                    string UniqueKey = TokenManager.TokenManagement.Extract(_jwtToken);
                    string DeviceToken = _cache.DeviceId;
                    int _ResponseCode = _Response.responseCode;
                    _Response.responseCode = 0;
                    _Response.deviceToken = DeviceToken;



                    _Response.jwtToken = TokenManager.TokenManagement.GenerateToken(_cache.UserId, UniqueKey);

                    string _serialisedDataBlock = String.Concat(JsonSerializer.Serialize(_Response).Where(c => !Char.IsWhiteSpace(c)));//GetSerialisedDataBlockWithDeviceToken();
                                                                                                                                       //byte[] bytes = Encoding.Default.GetBytes(_serialisedDataBlock);
                                                                                                                                       //_serialisedDataBlock = Encoding.UTF8.GetString(bytes);



                    Console.WriteLine(String.Concat(JsonSerializer.Serialize(_Response).Where(c => !Char.IsWhiteSpace(c))));


                    _Response.hash = CreateMD5(_serialisedDataBlock);


                    _Response.deviceToken = String.Empty;

                    RedisRun.Set(UniqueKey, this._cache.ToString());

                    switch (_ResponseCode)
                    {
                        case 404:
                            _APIResult = Results.NotFound(_Response);
                            break;
                        case 400:
                            _APIResult = Results.BadRequest(_Response);
                            break;
                        case 200:
                            _APIResult = Results.Ok(_Response);
                            break;

                    }

                }
            }
            else
            {
                _APIResult = Results.Unauthorized();
            }
            return _APIResult;
        }



        protected virtual IResult OnValidationFailed(ModelContext_Account db, List<Exception> Exceptions)
        {
            StringBuilder ErrBuilder = new StringBuilder();
            ErrBuilder.AppendLine("Fetching Customer Account Details  is  failed because of following reasons");
            foreach (Exception Validation in Exceptions)
            {
                ErrBuilder.AppendLine(Validation.Message);
            }
            string ErrorMsg = ErrBuilder.ToString();

            Console.WriteLine(ErrorMsg);

            return Results.BadRequest(ErrorMsg);
        }
        protected virtual List<Exception> CustomisedValidate(ModelContext_Account db)
        {
            throw new NotImplementedException("MustImplment this function");
        }

        protected virtual ResponseData OnValidationSuccess(ModelContext_Account db)
        {
            throw new NotImplementedException("MustImplment this function");
        }
    }
}
