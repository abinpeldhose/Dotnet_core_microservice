using System.Text.Json;

namespace customerSdmodule.Redis
{
    public class CacheData
    {
        private int _firmId;
        private int branchId;
        private int _moduleId;
        private int _applicationNo;
        private string _userName;
        private string _customerId;
        private string _version;
        private string _userType;
        private string _chequeNo;
        private string _userId;
        private string deviceId;
        private string jwtToken;
        private bool otpStatus;
        private string preAuthendicateToken;

        public int FirmId { get => _firmId; set => _firmId = value; }
        public int ModuleId { get => _moduleId; set => _moduleId = value; }
        public int ApplicationNo { get => _applicationNo; set => _applicationNo = value; }
        public string Version { get => _version; set => _version = value; }
        public string UserType { get => _userType; set => _userType = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string ChequeNo { get => _chequeNo; set => _chequeNo = value; }
        public string CustomerId { get => _customerId; set => _customerId = value; }
        public int BranchId { get => branchId; set => branchId = value; }
        public string UserId { get => _userId; set => _userId = value; }
        public string DeviceId { get => deviceId; set => deviceId = value; }
        public string JwtToken { get => jwtToken; set => jwtToken = value; }
        public bool OtpStatus { get => otpStatus; set => otpStatus = value; }
        public string PreAuthendicateToken { get => preAuthendicateToken; set => preAuthendicateToken = value; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<CacheData>(this);
        }

        public static CacheData Create(string JsonState)
        {
            return JsonSerializer.Deserialize<CacheData>(JsonState);
        }
    }
}
