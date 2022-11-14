using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheDemo
{
    public class CacheData1
    {
        private int _firmId;
        private int _moduleId;
        private string _applicationNo;
        private string _version;

        public int FirmId { get => _firmId; set => _firmId = value; }
        public int ModuleId { get => _moduleId; set => _moduleId = value; }
        public string ApplicationNo { get => _applicationNo; set => _applicationNo = value; }
        public string Version { get => _version; set => _version = value; }
    }
}
