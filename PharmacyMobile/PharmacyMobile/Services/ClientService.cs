using PharmacyMobile.Models.POCO_Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyMobile.Services
{
    public class ClientService
    {
        private static ClientService instance;
        private const string apiIp = "172.20.1.144";
        private const string reserveApiIp = "172.20.1.144";
        public const string apiUrl = "http://172.20.1.144:50765/";
        public const string reserveUrl = "http://172.20.1.144:5000/";
        public Patient CurrentPatient { get; set; }
        public HttpClient HttpClient { get; set; }

        private ClientService() { }

        public static ClientService Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClientService();

                return instance;
            }
        }

        private bool IsConnected()
        {
            try
            {
                // ClientService.Instance.HttpClient.PostAsync().
                Ping p = new Ping();
                PingReply pr = p.Send(IPAddress.Parse(apiIp));
                if (pr.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    PingReply pr2 = p.Send(IPAddress.Parse(reserveApiIp));
                    return pr2.Status == IPStatus.Success;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<HttpResponseMessage> GetRequest(string url)
        {
            try
            {
                var response = await HttpClient.GetAsync(apiUrl + url);
                return response;
            }
            catch 
            {
                try
                {
                    var response = await HttpClient.GetAsync(reserveUrl + url);
                    return response;
                }
                catch
                {
                    return null;
                }
            };
        }

        public async Task<HttpResponseMessage> PostRequest(HttpRequestMessage httpRequestMessage)
        {
            try
            {
                var response = await HttpClient.PostAsync(httpRequestMessage.RequestUri, httpRequestMessage.Content);
                return response;
            }
            catch (Exception)
            {
                try
                {
                    var response = await HttpClient.PostAsync(new Uri
                        (httpRequestMessage.RequestUri.OriginalString.Replace(apiUrl, reserveUrl)), httpRequestMessage.Content);
                    return response;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public async Task<HttpResponseMessage> PutRequest(HttpRequestMessage httpRequestMessage)
        {
            try
            {
                var response = await HttpClient.PutAsync(httpRequestMessage.RequestUri, httpRequestMessage.Content);
                return response;
            }
            catch (Exception)
            {
                try
                {
                    var response = await HttpClient.PutAsync(new Uri
                        (httpRequestMessage.RequestUri.OriginalString.Replace(apiUrl, reserveUrl)), httpRequestMessage.Content);
                    return response;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
