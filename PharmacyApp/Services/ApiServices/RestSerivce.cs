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
            request.AddCookie("connectionId", UserService.Instance.HubConnection.ConnectionId);
            request.AddParameter("Login", login);
            request.AddParameter("Password", password);
            return UserService.Instance.RestClient.Execute(request);
        }

        public void Exit()
        {
            UserService.Instance.HubConnection.StopAsync();
            UserService.Instance.HubConnection = null;
            UserService.Instance.RestClient = null;
        }

        public IRestResponse SendRequest(RestRequest request)
        {
            request.AddHeader("Authorization", "Bearer " + UserService.Instance.Token);
            var response = UserService.Instance.RestClient.Execute(request);
            return response;
        }
    }
}
