using customerSdmodule.Model1;
//using customerSdmodule.sample;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.GetCustomerDetails
{
    public class GetCustomerDetailsApi:BaseApi
    {
        private GetCustomerDetailsData _data;
       

        public GetCustomerDetailsData Data { get => _data; set => _data = value; }
        

        public ResponseData Get(ModelContext db)
        {
            return customerdetails(db);
        }

        private ResponseData customerdetails(ModelContext db)
        {
            try
            {
               // var images=db.CustomerPhotos.FirstorDefault()

                String imageURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ4wLEoiR0baQCYjpHMu_DEsv6qmGkXs99lvRRxAnhZj3_pM_qsIRdYFnjZ5Lozl4q2KNg&usqp=CAU";
                var data = (from customer in db.Customers
                            join country in db.CountryMasters on customer.CountryId equals country.CountryId
                            join photo in db.Photos on customer.CustId equals photo.Custid into photos
                            from image in photos.DefaultIfEmpty()
                            join pin in db.PostMasters on customer.PinNo equals pin.SrNumber into pins
                            from pincode in pins.DefaultIfEmpty()
                            from district in db.DistrictMasters
                            where pincode.DistrictId == district.DistrictId
                            from state in db.StateMasters
                            where district.StateId == state.StateId
                            join custdetails in db.CustomerDetails on customer.CustId equals custdetails.CustId
                            where customer.CustId == Data.customerId

                            select new
                            {
                                customerName = customer.CustName.TrimStart(new char[] { '0', '1', '2' }),
                                mobileNumber1 = customer.Phone1,
                                mobileNumber2 = customer.Phone2,
                                emailId = custdetails.EmailId,//"customer@gmail.com",
                                houseName = customer.HouseName,
                                shareCount = customer.Sharecount,
                                landMark = customer.LandMark,
                                locality = customer.Locality,
                                district = district.DistrictName ?? "  ",
                                state = district.StateId == state.StateId ? state.StateName : " ",
                                // pincode = customer.PinNo,
                                postcode = pincode.PinCode == null ? 0 : pincode.PinCode,
                                countryName = country.CountryName,
                                image = image.Image == null ? imageURL : image.Image,


                            }).SingleOrDefault();
                if (data == null)
                {
                    Log.Error("No Data Found");
                    var results = new
                    {
                        Status = "CustomerId Not Found",
                    };
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(results);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(results);
                    return _Response;

                }
                else
                {
                    Log.Information("/Sucesss");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(data);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    //_Response.data = JsonSerializer.Serialize(data);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<GetCustomerDetailsData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();

            var customerid = db.Customers.Where(x => x.CustId == Data.customerId).SingleOrDefault();


            if (customerid == null)
            {
                FailedValidations.Add(new ApplicationException("Invalid Data"));
            }



            return FailedValidations;
        }

    }
}