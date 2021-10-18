using PharmacyApp.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmacyApp.Services.ApiServices
{
    public class RestService
    {
        public IRestResponse Authorization(string login, string password)
        {
            RestRequest request = new RestRequest($"{Constants.apiAddress}/token", Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("Login", login);
            request.AddParameter("Password", password);
            var response = UserService.Instance.RestClient.Execute(request);         
            return response;
        }

        public IRestResponse SendRequest(RestRequest request)
        {
            var response = UserService.Instance.RestClient.Execute(request);
            return response;
        }
    }
}
