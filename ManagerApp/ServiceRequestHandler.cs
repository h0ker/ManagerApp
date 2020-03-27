using ManagerApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ManagerApp
{
    class ServiceRequestHandler
    {
        public static async Task<T> MakeServiceCall<T>(ServiceRequest req, object body = null, Dictionary<string, string> urlParams = null) where T : new()
        {
            HttpClient client = new HttpClient();
            var timeStart = DateTime.Now;

            using (var request = CreateHttpRequest(req, body, urlParams))
            {
                T obj = new T();
                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        try //to deserialize the object
                        {
                            obj = JsonConvert.DeserializeObject<T>(content);
                             
                            //todo -- why is this here
                            if(obj == null)
                            {
                                obj = new T();
                            }
                            return obj;
                        }                        
                        catch (Exception ex) //error deserializing object
                        {
                            Debug.WriteLine("Malformed response. Message: " + ex.Message);
                            return obj;
                        }
                    }
                    else //response has an error status
                    {
                        return obj;                        
                    }
                }
                catch (Exception ex)//any other error
                {
                    Debug.WriteLine("An error occured. Message: " + ex.Message);
                    return obj;
                }
            }
        }

        private static HttpRequestMessage CreateHttpRequest(ServiceRequest serviceRequest, object body = null, Dictionary<string, string> urlParams = null)
        {
            var url = (urlParams == null) ? serviceRequest.Url : AddURLParams(serviceRequest.Url, urlParams); //if there are URL params >> add them to url
            var httpRequestMessage = new HttpRequestMessage(serviceRequest.Method, url);

            var customHeaders = serviceRequest.Headers;
            if (customHeaders != null)  //add custom headers to the request message
            {
                foreach (KeyValuePair<string, string> entry in customHeaders)
                {
                    httpRequestMessage.Headers.Add(entry.Key, entry.Value);
                }
            }
            
            if (body != null)
            {
                try //serialize body object and add it to the request
                {
                    var bodyJson = JsonConvert.SerializeObject(body);

                    var requestBody = new StringContent(bodyJson, Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = requestBody;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return httpRequestMessage;
        }

        /// <summary>
        /// Builds the URL paramaters into the URL
        /// </summary>
        /// <returns>New URL with the paramaters added</returns>
        private static string AddURLParams(string url, Dictionary<string, string> urlParams)
        {            
            url += "?";
            foreach (KeyValuePair<string, string> entry in urlParams)
            {
                url += $"{entry.Key}={HttpUtility.UrlEncode(entry.Value)}&";
            }

            return url.TrimEnd('&');                       
        }
    }
}
