using FacebookMessenger.Helper;
using FacebookMessenger.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FacebookMessenger
{
    public class MessageHandler
    {

        public static async Task<RequestModel> GetMessage(Stream body)
        {
            string json;
            using (StreamReader sr = new StreamReader(body, Encoding.UTF8, false))
            {
                json = await sr.ReadToEndAsync();
                Log.Information("Request => " +  json);
            }
            return JsonConvert.DeserializeObject<RequestModel>(json);
        }


        public static async Task<PersonProfileModel> GetProfileInfo(string PSID, PageModel page)
        {
            Task<HttpResponseMessage> task =  HttpHelper.HttpGetRequest(FacebookURL.GraphURL + "/" + PSID, "fields=first_name,last_name,profile_pic", page.Token);
            var responseAwait = await task;
            var content = await responseAwait.Content.ReadAsStringAsync();

            Log.Information(String.Format(@"StatusCode : {0}, Reason {1}", responseAwait.StatusCode, responseAwait.ReasonPhrase));
            Log.Information(String.Format(@"Response content {0}", content));

            return JsonConvert.DeserializeObject<PersonProfileModel>(content); 
        }


        public static Task<HttpResponseMessage> ResponseToFB(ResponseModel msg, PageModel page, string postURL = FacebookURL.Message_V70URL)
        {      
            return HttpHelper.HttpPostRequest(postURL, JsonConvert.SerializeObject(msg), page.Token);
        }

       
    }
}
