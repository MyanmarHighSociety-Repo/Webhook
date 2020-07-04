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

        public static void ResponseMessage(object msg, string token, string postURL = FacebookApiURL.Message_V70URL)
        {
            try
            {
                var responseAwait = HttpHelper.HttpPostRequest(postURL, JsonConvert.SerializeObject(msg), token).GetAwaiter();
                responseAwait.OnCompleted(() =>
                {
                    try
                    {
                        var resMsg = responseAwait.GetResult();
                        var content = resMsg.Content.ReadAsStringAsync().GetAwaiter();

                        content.OnCompleted(() =>
                        {
                            var resultStr = content.GetResult(); Log.Information(String.Format(@"Response content {0}", resultStr));
                        });

                        Log.Information(String.Format(@" StatusCode : {0}, Reason {1}", resMsg.StatusCode, resMsg.ReasonPhrase));
                    }
                    catch (Exception ex)
                    {
                        Log.Error("ResponseMessage Inner Block => " + ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error("ResponseMessage => " + ex.Message);
            }
        }
         
        public static async Task<PersonProfileModel> GetProfileInfo(string PSID, PageModel page)
        {
            Task<HttpResponseMessage> task =  HttpHelper.HttpGetRequest(FacebookApiURL.GraphURL + "/" + PSID, "fields=first_name,last_name,profile_pic", page.Token);
            var responseAwait = await task;
            var content = await responseAwait.Content.ReadAsStringAsync();

            Log.Information(String.Format(@"StatusCode : {0}, Reason {1}", responseAwait.StatusCode, responseAwait.ReasonPhrase));
            Log.Information(String.Format(@"Response content {0}", content));

            return JsonConvert.DeserializeObject<PersonProfileModel>(content); 
        }
         

    }
}
