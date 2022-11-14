using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.DeleteNotes
{
    public class DeleteNoteAPI:BaseApi
    {
        private DeleteNoteData _data;
      //  private string _jwtToken;

        public DeleteNoteData Data { get => _data; set => _data = value; }
      //  public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return DeleteNote(db);
        }
        public ResponseData DeleteNote(ModelContext db)
        {
            try
            {
                var existItem = db.SdNotes.FirstOrDefault(x => x.EmployeeId == Data.EmployeeId && x.NoteDescription == Data.NoteDescription && x.NoteDate == Data.NoteDate && x.NoteId == Data.NoteId);
                if (existItem == null)
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "not available" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "not available" });
                    return _Response;
                }
                else
                {

                    db.SdNotes.Remove(existItem);
                    db.SaveChanges();
                    var message = new { Status = "Deleted Successfully" };
                    Log.Information("/Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(message);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(message);
                    return _Response;
                    // return Results.Ok(message);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<DeleteNoteData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidation = new List<Exception>();
           
            return FailedValidation;
        }
    }
}
