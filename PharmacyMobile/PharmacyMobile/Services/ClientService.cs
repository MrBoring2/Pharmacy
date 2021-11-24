using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyMobile.Services
{
    public class ClientService
    {
        private static ClientService instance;
        public const string apiUrl = "http://172.20.1.144:50765/";
        public HttpClient HttpClient { get; set; }
       
        public static ClientService Instance 
        {
            get
            {
                if (instance == null)
                    instance = new ClientService();
                
                return instance;
            }
        }

        public async Task<HttpResponseMessage> GetRequest(string url)
        {
            var response = await HttpClient.GetAsync(apiUrl + url);
            return response;
        }

        public async Task<HttpResponseMessage> PostRequest(string url, StringContent data)
        {
            var response = await HttpClient.PostAsync(apiUrl + url, data);
            return response;
        }
        public async Task<HttpResponseMessage> PostRequest(HttpRequestMessage httpRequestMessage)
        {
            var response = await HttpClient.PostAsync(httpRequestMessage.RequestUri, httpRequestMessage.Content);
            return response;
        }
    }
}
