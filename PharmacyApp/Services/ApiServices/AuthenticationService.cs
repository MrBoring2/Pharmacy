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
    public class AuthenticationService
    {

        public IRestResponse Authorization(string login, string password)
        {
            RestRequest request = new RestRequest($"{Constants.apiAddress}/token", Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("grant_type", "password");
            request.AddHeader("username", login);
            request.AddHeader("login", password);
            request.AddParameter("Login", login);
            request.AddParameter("Password", password);
            //HttpClient client = new HttpClient();
          
            //request.AddParameter
            var response = UserService.Instance.RestClient.Execute(request);
            MessageBox.Show(response.StatusCode + " | " + response.Content);
            return response;
        }
    }
}
