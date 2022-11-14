using customerSdmodule.Model1;
using System.Text.Json;
using NS=Newtonsoft.Json;
using static RedisCacheDemo.RedisCacheStore;
using System.Text;

namespace customerSdmodule.ModelClass
{
    public class BaseApi
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
           //_cache=Json
            return Token;
        }

        protected virtual bool ValidToken()
        {
            var id= TokenManager.TokenManagement.ValidateToken(_jwtToken);
            bool TokenValid = _cache.UserId == TokenManager.TokenManagement.ValidateToken(_jwtToken);
            return TokenValid;
        }
        public IResult Get(ModelContext db)
        {
            return CustomisedGet(db);
        }

        protected bool ValidateHash()
        {
            string ReceviedHash = _Hash;
            bool Result = false;
            string _serialisedDataBlock = GetSerialisedDataBlockWithDeviceToken().Replace(@"\u002B", "+").Replace(@"\u0026", "&");//String.Concat(GetSerialisedDataBlockWithDeviceToken().Where(c => !Char.IsWhiteSpace(c)));//GetSerialisedDataBlockWithDeviceToken();
            
            string _generatedHash = CreateMD5(_serialisedDataBlock);
            Console.WriteLine(GetSerialisedDataBlockWithDeviceToken().Replace(@"\u002B", "+").Replace(@"\u0026", "&"));
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

        protected virtual IResult CustomisedGet(ModelContext db)
        {
            throw new NotImplementedException("MustImplment this function");
        }

        public IResult Validate(ModelContext db)
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
                    _cache = JsonSerializer.Deserialize<Redis.CacheData>(RedisRun.Get(UniqueKey, null));
                    string DeviceToken = _cache.DeviceId;
                    int _ResponseCode = _Response.responseCode;
                    _Response.responseCode = 0;
                    _Response.deviceToken = DeviceToken;                    

                    if(_cache.UserId==null)
                    {
                        _Response.jwtToken = TokenManager.TokenManagement.GenerateToken(_cache.DeviceId, UniqueKey);
                    }
                    else
                    {
                        _Response.jwtToken = TokenManager.TokenManagement.GenerateToken(_cache.UserId, UniqueKey);
                    }                 
                                                                                                                              //_serialisedDataBlock = Encoding.UTF8.GetString(bytes);


                    try
                    {
                        //Newtonsoft.Json.Linq.JObject jsonObject;
                      
                        
                        string responseData = JsonSerializer.Serialize(_Response.data);
                        string _Element = responseData;

                        Newtonsoft.Json.Linq.JObject jsonObject;

                        int _ElementCount = responseData.Count();
                        if (_ElementCount > 0)
                        {
                            _Element = Newtonsoft.Json.JsonConvert.SerializeObject(new { Data = _Element });
                        }

                        jsonObject = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(_Element);


                        // jsonObject = JsonConvert.DeserializeObject<dynamic>(_Response.data);   //JsonSerializer.Deserialize<dynamic>(_Response.data);
                        jsonObject.Add("JWTToken", _Response.jwtToken);
                        jsonObject.Add("DeviceToken", _Response.deviceToken);


                        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);//Replace("u0022","\"");
                        
                        jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonString);
                        // jsonString= Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);
                        Console.WriteLine(jsonString.Replace(@"\", "").Replace("\"{","{").Replace("}\"","}").Replace("\"[{", "[{").Replace("}]\"", "}]").Replace("\"[","[").Replace("]\"","]").Replace(@"u0026", "&"));
                        _Response.hash = CreateMD5(jsonString.Replace(@"\", "").Replace("\"{", "{").Replace("}\"", "}").Replace("\"[{", "[{").Replace("}]\"", "}]").Replace("\"[", "[").Replace("]\"", "]").Replace(@"u0026", "&"));
                        //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject));
                        //_Response.hash = CreateMD5(Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject));
                    }
                    catch(Exception Ex)
                    {
                        throw Ex;
                    }


                    //var serilizedblock = JsonSerializer.Serialize(_Response);
                    //Console.WriteLine(serilizedblock);
                    //Console.WriteLine(JsonSerializer.Serialize(_Response).Replace("u0022", "\"").Replace(@"\", ""));
                    //_Response.hash = CreateMD5(JsonSerializer.Serialize(_Response).Replace("u0022","\"").Replace(@"\",""));

                    _Response.deviceToken = String.Empty;

                    RedisRun.Set(UniqueKey, this._cache.ToString());

                    switch (_ResponseCode)
                    {
                        case 404:
                            _APIResult= Results.NotFound(_Response);
                            break;
                        case 400:
                            _APIResult= Results.BadRequest(_Response);
                            break;
                        case 200: _APIResult= Results.Ok(_Response);
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
      
           
       
        protected virtual IResult OnValidationFailed(ModelContext db, List<Exception> Exceptions)
        {
            StringBuilder ErrBuilder = new StringBuilder();
            ErrBuilder.AppendLine("This Api is failed because of following reasons");
            foreach (Exception Validation in Exceptions)
            {
                ErrBuilder.AppendLine(Validation.Message);
            }
            string ErrorMsg = ErrBuilder.ToString();

            Console.WriteLine(ErrorMsg);

            return Results.BadRequest(ErrorMsg);
        }
        protected virtual List<Exception> CustomisedValidate(ModelContext db)
        {
            throw new NotImplementedException("MustImplment this function");
        }

        protected virtual ResponseData OnValidationSuccess(ModelContext db)
        {
            throw new NotImplementedException("MustImplment this function");
        }
    }
}
