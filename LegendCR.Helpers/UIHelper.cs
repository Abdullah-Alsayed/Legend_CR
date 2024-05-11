using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace LegendCR.Helpers
{
    public class UIHelper
    {
        public static HttpResponseMessage CreateRequest(string baseurl, HttpMethod method, string relativeUrl,
            string jsonObj = null, string lang = "", string basicAuthUser = "", string basicAuthPassword = "")
        {
            using (var client = new HttpClient())
            {
                //client.Timeout.Add(new TimeSpan(0, 3, 0));
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrWhiteSpace(basicAuthUser) && !string.IsNullOrWhiteSpace(basicAuthPassword))
                {
                    var byteArray = new UTF8Encoding().GetBytes(basicAuthUser + ":" + basicAuthPassword);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


                }
                //client.DefaultRequestHeaders.Add("Accept-Language", lang);

                HttpRequestMessage request = new HttpRequestMessage(method, relativeUrl);

                if (jsonObj != null)
                    request.Content = new StringContent(jsonObj, Encoding.UTF8, "application/json");

                var Res = new HttpResponseMessage();
                try
                {
                    Res = client.SendAsync(request).Result;

                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }

                return Res;
            }
        }

        public static string HtmlToPlainText(string htmlString)
        {
            var text = Regex.Replace(htmlString, @"<(.|\n)*?>", "");
            return text;
        }

        //public static HttpResponseMessage AddRequestToServiceApi(string url, JObject json)
        //{
        //    var Res = new HttpResponseMessage();
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        //            Res = client.PostAsync(url, content).Result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogException(ex.Message, ex.StackTrace);
        //    }

        //    return Res;

        //}

        

    }
}