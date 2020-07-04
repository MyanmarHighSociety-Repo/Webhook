using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FacebookMessenger.Helper
{
    public static class HttpHelper
    {


        public static Task<HttpResponseMessage> HttpPostRequest(string url, string parameters, string accesstoken)
        {
            var client = new HttpClient();
            Log.Information("Post URL   => " + url);
            Log.Information("Parameters       => " + parameters);

            return client.PostAsync(url + string.Format("?access_token={0}", accesstoken), new StringContent(parameters, Encoding.UTF8, ContentType.Json));

        }

        public static Task<HttpResponseMessage> HttpGetRequest(string url, string accesstoken)
        {
            return HttpGetRequest(url, "", accesstoken);
        }

        public static Task<HttpResponseMessage> HttpGetRequest(string url, string parameters, string accesstoken)
        {
            var client = new HttpClient();
            Log.Information("Get URL   => " + url);
            Log.Information("Parameters       => " + parameters);

            if (String.IsNullOrEmpty(parameters))
                return client.GetAsync(string.Format("{0}?access_token={1}", url, accesstoken));
            else
                return client.GetAsync(string.Format("{0}?{1}&access_token={2}", url, parameters, accesstoken));
        }
    }
}
