using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PISclient.Helper
{
    public class HttpRequest
    {
        public static string MakeRequest(string url, string username, string password, string method, string body)
        {
            try
            {
                var uri = new Uri(url);

                // Create the request
                HttpWebRequest request = (HttpWebRequest)WebRequest.CreateDefault(uri);
                request.Method = method;
                request.KeepAlive = false;
                request.ContentType = "application/json";
                //string encodedUsernamePass = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                //request.Headers.Add(HttpRequestHeader.Authorization, $"Basic {encodedUsernamePass}");

                if (!method.Equals("GET"))
                {
                    // Send the data
                    StreamWriter reqStream = new StreamWriter(request.GetRequestStream());
                    reqStream.Write(body);
                    reqStream.Close();
                }


                // Get the response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader responseStream = new StreamReader(response.GetResponseStream());

                string line = string.Empty;
                string responseString = string.Empty;

                while ((line = responseStream.ReadLine()) != null)
                {
                    responseString += line;
                }

                responseStream.Close();

                return responseString;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
    }
}
