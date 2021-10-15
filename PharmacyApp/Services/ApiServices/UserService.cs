using Microsoft.AspNetCore.SignalR.Client;
using PharmacyApp.Helpers;
using PharmacyApp.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Services.ApiServices
{
    public class UserService
    {
        private static UserService instance;
        public string UserName { get; private set; }
        public Roles Role { get; private set; }
        public HubConnection HubConnection { get; set; }
        public RestClient RestClient { get; set; }
        private static object syncRoot = new Object();

        private UserService()
        {
            
        }

        public void InirializeService(HubConnection hubConnection, RestClient restClient)
        {
            HubConnection = hubConnection;
            RestClient = restClient;
        }

        public void SetUser(string username, Roles role)
        {
            UserName = username;
            Role = role;
        }

        public static UserService Instance
        {
            get 
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new UserService();
                    }
                }
                return instance;
            }
        }

       
    }
}
