using customerSdmodule.Properties;
using Newtonsoft.Json;
using System.Text;

namespace customerSdmodule.ModelClass
{
        public class ApiHandler
        {
            public IResult InvokeHttpClient<T>(string ApiName, object request) where T : class
            {

                //string authToken="";
                using (var httpClient = new HttpClient())
                {
                   
                    string apiUrl = Resources.API;
                    string inputJson = System.Text.Json.JsonSerializer.Serialize(request);
                    var response = httpClient.PostAsync(apiUrl + ApiName, new StringContent(inputJson, Encoding.UTF8, "application/json")).Result;
                    if (response.IsSuccessStatusCode)
                    {

                        var jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();

                        var ob = JsonConvert.DeserializeObject<T>(jsonResponse);
                        return Results.Ok(ob);
                    }
                    else
                    {
                        //ResponseBuilder resp = new ResponseBuilder();
                        //resp.status = "Fail";
                        return Results.NotFound(new { status = "Failed" });
                    }


                }
            }
        }
    
}
