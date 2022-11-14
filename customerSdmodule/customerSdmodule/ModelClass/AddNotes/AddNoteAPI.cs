using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.AddNotes
{
    public class AddNoteAPI : BaseApi
    {
        private AddNotesData _data;
      //  private string _jwtToken;

        public AddNotesData Data { get => _data; set => _data = value; }
      //  public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return AddNotes(db);
        }
        public ResponseData AddNotes(ModelContext db)
        {
            try
            {
                var noteid = db.SdNotes.Select(x => (x.NoteId)).Max() + 1;
                var data = new SdNote
                {
                    FirmId = Data.FirmId,
                    BrachId = Data.BranchId,
                    EmployeeId = Data.EmployeeId,
                    NoteDate = Data.NoteDate,
                    NoteDescription = Data.Description,
                    NoteId = noteid,
                };
                db.SdNotes.Add(data);


                db.SaveChanges();
                var message = new { Status = "Success" };
                Log.Information("/Success");
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(message);
                return _Response;
                //return Results.Ok(message);


            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                // return Results.NotFound(message);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<AddNotesData>(Data);
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
}
