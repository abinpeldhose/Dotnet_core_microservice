using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.AgentEmployee
{
    public class AgentEmployeeApi  : BaseApi
    {

        private AgentEmployeeData _data;       
       
      

        public AgentEmployeeData Data { get => _data; set => _data = value; }
        

        public ResponseData Get(ModelContext db)
        {
            return AgnetEmployee(db);
        }
        public ResponseData AgnetEmployee(ModelContext db)
        {
            try
            {

                var data1 = db.SdAgentMasters.Where(x => x.AgentId.ToString() == Data.Search).Select(x => x.AgentName).SingleOrDefault();
                var data2 = db.EmployeeMasters.Where(x => x.EmpCode.ToString() == Data.Search).Select(x => x.EmpName).SingleOrDefault();


                if (Data.Type.ToUpper() == "MOBILENUMBER")
                {                  


                    var user = (from u in db.Customers
                                where u.Phone1 == Data.Search || u.Phone2 == Data.Search || u.Phone1 == "0" + Data.Search || u.Phone2 == "0" + Data.Search
                                //where u.Phone1.EndsWith(search.ToUpper()) || u.Phone2.EndsWith(search.ToUpper())

                                select new
                                {                                   
                                    name = u.CustName.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                                }).FirstOrDefault();//ToList();

                    if (user == null)
                    {
                        Log.Error("Phone Not Found");


                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Phone not found" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(new { status = "Phone not found" });
                        return _Response;

                    }
                    else
                    {
                        Log.Information("/Sucesss");
                        var message = new { status = "customer found", name = user.name };
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(user);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //  _Response.data = JsonSerializer.Serialize(user);
                        return _Response;
                    }
                }
                else if (Data.Type.ToUpper() == "EMPCODE")
                {

                    var data = (from u in db.EmployeeMasters


                                where u.EmpCode == Convert.ToInt32(Data.Search)

                                select new
                                {

                                    //status = u.EmpCode,
                                    name = u.EmpName,


                                }).FirstOrDefault();
                    if (data == null)
                    {
                        Log.Error("Empid Not Found");
                        var results = new
                        {
                            status = "Empid Not Found",
                        };

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(results);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(results);
                        return _Response;

                    }
                    else
                    {
                        Log.Information("/Sucesss");
                        var message = new { status = "Employee Id Found", name =data.name };
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(message);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //_Response.data = JsonSerializer.Serialize(message);
                        return _Response; ;
                    }



                }

                else
                {
                    Log.Error("Not Found");

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Not Found" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status ="Not Found"});
                    return _Response; 
                }

            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);

                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(message);
                return _Response;
            }

        }

        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<AgentEmployeeData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
            var search = db.Customers.Where(x => x.Phone1 == Data.Search && x.Phone2 == Data.Search).ToList();
            var value = db.EmployeeMasters.Where(x => x.EmpCode.ToString() == Data.Search && x.EmpCode.ToString() == Data.Search).ToList();
            if (search == null)
            {
                FailedValidation.Add(new ApplicationException("Inavlid Input"));
            }
            if (search == null)
            {
                FailedValidation.Add(new ApplicationException("Invalid Input"));
            }
            return FailedValidation;
        }
    }
}
