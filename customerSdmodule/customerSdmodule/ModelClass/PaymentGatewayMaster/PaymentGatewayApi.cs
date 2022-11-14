using customerSdmodule.Model1;
using Serilog;
using System.Text.Json;

namespace customerSdmodule.ModelClass.PaymentGatewayMaster
{
    public class PaymentGatewayApi : BaseApi
    {


        private PaymentGatewayData _data;      



        public PaymentGatewayData Data { get => _data; set => _data = value; }
       

        public ResponseData Get(ModelContext db)
        {
            return GetPaymentGateways(db);
        }
        public ResponseData GetPaymentGateways(ModelContext db)
        {
            try
            {
                if(Data.Usertype.ToLower()=="employee")
                {
                    var data = (from m in db.PaymentgatewayMasters
                                where Data.Usertype.ToUpper() == m.UserType.ToUpper() && Data.Paymenttype.ToUpper() == m.PaymentType.ToUpper() && m.ProviderId == "102"
                                select new
                                {

                                    paymentgatewayname = m.PaymentgatewayName,
                                    providerid = m.ProviderId,
                                    paymentgatewaytype = m.PaymentgatewayType,
                                    commissionflatdescription = m.ComissionflatDescription,

                                }).ToList();

                    if (data.Count() == 0)
                    {
                        Log.Error("No Payment Gateways");
                        var results = new
                        {
                            Status = "No Payment Gateways",
                        };

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(results);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        //  _Response.data = JsonSerializer.Serialize(results);
                        return _Response;
                    }
                    else
                    {
                        Log.Information("/Sucesss");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(data);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(data);
                        return _Response;
                    }
                }
                else
                {
                    var data = (from m in db.PaymentgatewayMasters
                                where Data.Usertype.ToUpper() == m.UserType.ToUpper() && Data.Paymenttype.ToUpper() == m.PaymentType.ToUpper()
                                select new
                                {
                                    paymentgatewayname = m.PaymentgatewayName,
                                    providerid = m.ProviderId,
                                    paymentgatewaytype = m.PaymentgatewayType,
                                    commissionflatdescription = m.ComissionflatDescription,

                                }).ToList();

                    if (data.Count() == 0)
                    {
                        Log.Error("No Payment Gateways");
                        var results = new
                        {
                            Status = "No Payment Gateways",
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
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(data);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        // _Response.data = JsonSerializer.Serialize(data);
                        return _Response;
                    }
                }              

              
            }
            catch (Exception ex)
            {
                
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "Somethings went wrong" });
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<PaymentGatewayData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> Failedvalidation = new List<Exception>();
            if (Data.Usertype is null)
            {
                Failedvalidation.Add(new Exception("Inavlid Input"));
            }
            else
            {
                var paymenttype = db.PaymentgatewayMasters.Where(x => x.UserType == Data.Usertype && x.PaymentgatewayType == Data.Paymenttype).ToList();
                if (paymenttype == null)
                {
                    Failedvalidation.Add(new Exception("Inavlid Input"));
                }
            }
            return Failedvalidation;
        }
    }
}
