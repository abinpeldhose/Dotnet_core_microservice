using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.GetNotes
{
    public class GetNotesAPI:BaseApi
    {
        private GetNoteData _data;
        
        //private string _jwtToken;
       

        public GetNoteData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return GetNotes(db);
        }
        public ResponseData GetNotes(ModelContext db)
        {
            try
            {
                var user = (from sdnote in db.SdNotes

                            where sdnote.NoteDate == Data.NoteDate
                           && sdnote.EmployeeId == Data.EmployeeId
                            select new
                            {
                                notedescription = sdnote.NoteDescription,
                                notedate = sdnote.NoteDate,
                                noteid = sdnote.NoteId,
                                employeeId = Data.EmployeeId,
                            }).ToList();

                if (user.Count() == 0)
                {
                    Log.Error("There is No Notes");
                    var results = new
                    {
                        Status = "There is No Notes",
                    };
                    // return Results.NotFound(results);

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
                    //return Results.Ok(user);
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(user);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(user);
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
                //_Response.data = JsonSerializer.Serialize(message);
                return _Response;
                // return Results.NotFound(message);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<GetNoteData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
          //  var verify = db.UserLoginMst1s.Where(x => x.UserId == Data.Userid && x.Phone == Data.Phone).ToList();
            //if (verify == null)
            //{
            //    FailedValidation.Add(new ApplicationException("Invalid Input"));
            //}
            return FailedValidation;
        }
    }
    public class GetNoteData:BaseData
    {
        public DateTime NoteDate { get; set; }
        public int EmployeeId { get; set; }
    }
}
